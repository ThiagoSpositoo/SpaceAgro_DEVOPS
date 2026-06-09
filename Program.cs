using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceAgro.DotNetApi.Data;
using SpaceAgro.DotNetApi.Models;
using SpaceAgro.DotNetApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// CONFIGURAÇÃO DE SERVIÇOS (DI)
// =========================================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SpaceAgro API - Painel Agroclimatológico",
        Version = "v1",
        Description = "Microsserviço de inteligência geoespacial integrado a dados de satélite da NASA, leituras de sensores e banco de dados em container."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// A API pode funcionar com Oracle no ambiente FIAP ou PostgreSQL no Docker/DevOps.
var dbProvider = Environment.GetEnvironmentVariable("DB_PROVIDER")
                 ?? builder.Configuration["DatabaseProvider"]
                 ?? "Oracle";

if (dbProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) ||
    dbProvider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
{
    var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONN_STRING")
                                   ?? builder.Configuration.GetConnectionString("PostgresConnection")
                                   ?? builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(postgresConnectionString));
}
else
{
    var oracleConnectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING")
                                 ?? builder.Configuration.GetConnectionString("OracleConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(oracleConnectionString));
}

builder.Services.AddHttpClient<NasaSpaceService>();

var app = builder.Build();

// =========================================================================
// PREPARAÇÃO DO BANCO PARA O AMBIENTE DOCKER
// =========================================================================

var autoCreateDatabase = string.Equals(
    Environment.GetEnvironmentVariable("AUTO_CREATE_DATABASE"),
    "true",
    StringComparison.OrdinalIgnoreCase
);

if (autoCreateDatabase)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    for (var tentativa = 1; tentativa <= 10; tentativa++)
    {
        try
        {
            context.Database.EnsureCreated();

            if (!context.Talhoes.Any())
            {
                var talhao = new Talhao
                {
                    Nome = "Talhao Marte 01",
                    Cultura = "Milho",
                    AreaHectares = 18.5,
                    Latitude = -23.5505,
                    Longitude = -46.6333,
                    IdProdutor = 1
                };

                context.Talhoes.Add(talhao);
                context.SaveChanges();

                context.LeiturasSensores.Add(new LeituraSensor
                {
                    Temperatura = 31.8,
                    UmidadeAr = 64.2,
                    UmidadeSolo = 38.5,
                    DataHora = DateTime.UtcNow,
                    IdDispositivo = talhao.Id
                });

                context.SaveChanges();
            }

            Console.WriteLine("Banco de dados preparado com sucesso para o ambiente Docker.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Tentativa {tentativa}/10: banco ainda indisponivel. Detalhe: {ex.Message}");
            Thread.Sleep(5000);
        }
    }
}

// =========================================================================
// PIPELINE DE MIDDLEWARES HTTP
// =========================================================================

app.UseSwagger(options =>
{
    options.RouteTemplate = "openapi/{documentName}.json";
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "SpaceAgro API v1");
    options.RoutePrefix = "swagger";
});

app.MapScalarApiReference(options =>
{
    options.WithTitle("SpaceAgro API - Documentação Executiva");
    options.WithTheme(ScalarTheme.DeepSpace);
    options.WithOpenApiRoutePattern("/openapi/v1.json");
    options.WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch);
});

app.UseCors("AllowAll");

// =========================================================================
// ENDPOINTS DE STATUS
// =========================================================================

app.MapGet("/", () => Results.Ok(new
{
    projeto = "SpaceAgro API",
    status = "online",
    documentacaoSwagger = "/swagger",
    documentacaoScalar = "/scalar/v1",
    endpointsPrincipais = new[]
    {
        "/api/talhoes",
        "/api/leituras",
        "/api/climaespacial/previsao?lat=-23.5505&lon=-46.6333",
        "/api/climaespacial/diagnostico/1"
    }
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow
}));

// =========================================================================
// CRUD - TALHÕES
// =========================================================================

app.MapGet("/api/talhoes", async (AppDbContext context) =>
    Results.Ok(await context.Talhoes.AsNoTracking().ToListAsync()))
.WithName("ListarTalhoes")
.WithOpenApi();

app.MapGet("/api/talhoes/{id:int}", async (int id, AppDbContext context) =>
{
    var talhao = await context.Talhoes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    return talhao is null ? Results.NotFound("Talhão não encontrado.") : Results.Ok(talhao);
})
.WithName("BuscarTalhaoPorId")
.WithOpenApi();

app.MapPost("/api/talhoes", async ([FromBody] Talhao talhao, AppDbContext context) =>
{
    context.Talhoes.Add(talhao);
    await context.SaveChangesAsync();
    return Results.Created($"/api/talhoes/{talhao.Id}", talhao);
})
.WithName("CriarTalhao")
.WithOpenApi();

app.MapPut("/api/talhoes/{id:int}", async (int id, [FromBody] Talhao dadosAtualizados, AppDbContext context) =>
{
    var talhao = await context.Talhoes.FindAsync(id);
    if (talhao is null) return Results.NotFound("Talhão não encontrado.");

    talhao.Nome = dadosAtualizados.Nome;
    talhao.Cultura = dadosAtualizados.Cultura;
    talhao.AreaHectares = dadosAtualizados.AreaHectares;
    talhao.Latitude = dadosAtualizados.Latitude;
    talhao.Longitude = dadosAtualizados.Longitude;
    talhao.IdProdutor = dadosAtualizados.IdProdutor;

    await context.SaveChangesAsync();
    return Results.Ok(talhao);
})
.WithName("AtualizarTalhao")
.WithOpenApi();

app.MapDelete("/api/talhoes/{id:int}", async (int id, AppDbContext context) =>
{
    var talhao = await context.Talhoes.FindAsync(id);
    if (talhao is null) return Results.NotFound("Talhão não encontrado.");

    context.Talhoes.Remove(talhao);
    await context.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("ExcluirTalhao")
.WithOpenApi();

// =========================================================================
// CRUD - LEITURAS DE SENSOR
// =========================================================================

app.MapGet("/api/leituras", async (AppDbContext context) =>
    Results.Ok(await context.LeiturasSensores.AsNoTracking().ToListAsync()))
.WithName("ListarLeituras")
.WithOpenApi();

app.MapGet("/api/leituras/{id:int}", async (int id, AppDbContext context) =>
{
    var leitura = await context.LeiturasSensores.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
    return leitura is null ? Results.NotFound("Leitura não encontrada.") : Results.Ok(leitura);
})
.WithName("BuscarLeituraPorId")
.WithOpenApi();

app.MapGet("/api/talhoes/{talhaoId:int}/leituras", async (int talhaoId, AppDbContext context) =>
{
    var existeTalhao = await context.Talhoes.AnyAsync(t => t.Id == talhaoId);
    if (!existeTalhao) return Results.NotFound("Talhão não encontrado.");

    var leituras = await context.LeiturasSensores
        .AsNoTracking()
        .Where(l => l.IdDispositivo == talhaoId)
        .OrderByDescending(l => l.DataHora)
        .ToListAsync();

    return Results.Ok(leituras);
})
.WithName("ListarLeiturasPorTalhao")
.WithOpenApi();

app.MapPost("/api/leituras", async ([FromBody] LeituraSensor leitura, AppDbContext context) =>
{
    var existeTalhao = await context.Talhoes.AnyAsync(t => t.Id == leitura.IdDispositivo);
    if (!existeTalhao) return Results.BadRequest("O ID_DISPOSITIVO informado deve corresponder a um talhão existente.");

    if (leitura.DataHora == default)
    {
        leitura.DataHora = DateTime.UtcNow;
    }

    context.LeiturasSensores.Add(leitura);
    await context.SaveChangesAsync();
    return Results.Created($"/api/leituras/{leitura.Id}", leitura);
})
.WithName("CriarLeitura")
.WithOpenApi();

app.MapPut("/api/leituras/{id:int}", async (int id, [FromBody] LeituraSensor dadosAtualizados, AppDbContext context) =>
{
    var leitura = await context.LeiturasSensores.FindAsync(id);
    if (leitura is null) return Results.NotFound("Leitura não encontrada.");

    var existeTalhao = await context.Talhoes.AnyAsync(t => t.Id == dadosAtualizados.IdDispositivo);
    if (!existeTalhao) return Results.BadRequest("O ID_DISPOSITIVO informado deve corresponder a um talhão existente.");

    leitura.Temperatura = dadosAtualizados.Temperatura;
    leitura.UmidadeAr = dadosAtualizados.UmidadeAr;
    leitura.UmidadeSolo = dadosAtualizados.UmidadeSolo;
    leitura.DataHora = dadosAtualizados.DataHora == default ? DateTime.UtcNow : dadosAtualizados.DataHora;
    leitura.IdDispositivo = dadosAtualizados.IdDispositivo;

    await context.SaveChangesAsync();
    return Results.Ok(leitura);
})
.WithName("AtualizarLeitura")
.WithOpenApi();

app.MapDelete("/api/leituras/{id:int}", async (int id, AppDbContext context) =>
{
    var leitura = await context.LeiturasSensores.FindAsync(id);
    if (leitura is null) return Results.NotFound("Leitura não encontrada.");

    context.LeiturasSensores.Remove(leitura);
    await context.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("ExcluirLeitura")
.WithOpenApi();

// =========================================================================
// ENDPOINTS DE INTELIGÊNCIA GEOESPACIAL / NASA
// =========================================================================

app.MapGet("/api/climaespacial/previsao", async (
    [FromQuery] double lat,
    [FromQuery] double lon,
    NasaSpaceService nasaService) =>
{
    if (lat == 0 || lon == 0) return Results.BadRequest("A latitude e longitude são obrigatórias.");

    var dadosNasa = await nasaService.BuscarPrevisaoAgroAsync(lat, lon);
    return Results.Ok(dadosNasa);
})
.WithName("GetPrevisaoSatelite")
.WithOpenApi();

app.MapGet("/api/climaespacial/diagnostico/{talhaoId:int}", async (
    int talhaoId,
    AppDbContext context,
    NasaSpaceService nasaService) =>
{
    var talhao = await context.Talhoes.FindAsync(talhaoId);
    if (talhao == null) return Results.NotFound("Talhão não encontrado.");

    var dadosNasa = await nasaService.BuscarPrevisaoAgroAsync(talhao.Latitude, talhao.Longitude);

    var ultimaLeitura = await context.LeiturasSensores
        .Where(l => l.IdDispositivo == talhaoId)
        .OrderByDescending(l => l.DataHora)
        .FirstOrDefaultAsync();

    var diagnostico = new
    {
        TalhaoNome = talhao.Nome,
        Cultura = talhao.Cultura,
        Coordenadas = new { lat = talhao.Latitude, lon = talhao.Longitude },
        DadosMacro_Nasa = dadosNasa,
        DadosMicro_SoloAtual = ultimaLeitura != null ? new
        {
            temperaturaSolo = ultimaLeitura.Temperatura,
            umidadeAr = ultimaLeitura.UmidadeAr,
            umidadeSolo = ultimaLeitura.UmidadeSolo,
            ultimaAtualizacao = ultimaLeitura.DataHora
        } : null,
        RecomendacaoSistema = "Análise preditiva executada. Verifique o painel mobile para alertas e tomada de decisão."
    };

    return Results.Ok(diagnostico);
})
.WithName("GetDiagnosticoCompleto")
.WithOpenApi();

app.Run();
