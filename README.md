# SpaceAgro DEVOPS - Global Solution 2026/1

## Descrição da Solução

O **SpaceAgro** é uma solução desenvolvida para a **Global Solution 2026/1**, conectada ao tema da **economia espacial** e ao uso de tecnologias para resolver problemas reais na Terra.

A proposta da aplicação é apoiar o **monitoramento agrícola inteligente**, permitindo o cadastro de talhões e o registro de leituras de sensores ambientais. A solução simula um cenário em que dados locais de sensores podem ser integrados a informações geoespaciais e satelitais para auxiliar na análise climática, prevenção de riscos e tomada de decisão no agronegócio.

Nesta entrega de **DevOps Tools & Cloud Computing**, a API foi conteinerizada com Docker e executada em uma **VM Linux na Azure**, integrada a um banco de dados PostgreSQL também em container.

---

## Links da Entrega

### Repositório GitHub

```txt
https://github.com/ThiagoSpositoo/SpaceAgro_DEVOPS
```

### API em Nuvem

```txt
http://68.154.48.178:8080/swagger
```

### Health Check

```txt
http://68.154.48.178:8080/health
```

### Vídeo no YouTube

```txt
GRAVANDO...
```

---

## Tecnologias Utilizadas

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Docker
* Docker Compose
* Azure Virtual Machine
* Ubuntu Server 24.04 LTS
* Swagger/OpenAPI
* Git e GitHub

---

## Objetivo da Entrega DevOps

O objetivo desta entrega é demonstrar a execução da aplicação em ambiente de nuvem utilizando containers Docker.

A solução possui:

* Um container para a API .NET;
* Um container para o banco de dados PostgreSQL;
* Rede Docker para comunicação entre os containers;
* Volume nomeado para persistência dos dados;
* Variáveis de ambiente para configuração;
* Portas expostas para acesso externo;
* Execução em VM Linux na Azure;
* Evidência de funcionamento por meio de logs, comandos Docker, Swagger e SELECT no banco.

---

## Arquitetura da Solução

A aplicação foi executada em uma VM Linux na Azure com Docker Engine instalado. Dentro da VM, o Docker Compose foi utilizado para subir dois containers integrados: um para a API e outro para o banco de dados.

```txt
Usuário
   |
   v
Internet
   |
   v
VM Azure - Ubuntu Server
IP Público: 68.154.48.178
   |
   v
Docker Engine
   |
   v
Docker Network: spaceagro_devops_spaceagro-network-rm566462
   |
   |-- Container: api-rm566462
   |      |-- API .NET
   |      |-- Porta: 8080
   |      |-- Usuário não-root: appuser
   |
   |-- Container: db-rm566462
          |-- PostgreSQL 16
          |-- Porta: 5432
          |-- Volume: spaceagro_devops_postgres-data-rm566462
```

---

## Informações da VM

| Item                | Informação                     |
| ------------------- | ------------------------------ |
| Provedor            | Microsoft Azure                |
| Nome da VM          | `vm-spaceagro-devops-rm566462` |
| Grupo de Recursos   | `rg-spaceagro-devops-eastus`   |
| Sistema Operacional | Ubuntu Server 24.04 LTS        |
| Região              | East US 2                      |
| IP Público          | `68.154.48.178`                |
| Porta SSH           | `22`                           |
| Porta da API        | `8080`                         |

---

## Containers Docker

### Container da API

| Item                  | Valor                                  |
| --------------------- | -------------------------------------- |
| Nome do container     | `api-rm566462`                         |
| Imagem                | `spaceagro-dotnet-api-rm566462:latest` |
| Tecnologia            | .NET 8                                 |
| Porta interna         | `8080`                                 |
| Porta externa         | `8080`                                 |
| Diretório de trabalho | `/app`                                 |
| Usuário               | `appuser`                              |

### Container do Banco de Dados

| Item              | Valor                                     |
| ----------------- | ----------------------------------------- |
| Nome do container | `db-rm566462`                             |
| Imagem            | `postgres:16`                             |
| Banco             | PostgreSQL                                |
| Porta interna     | `5432`                                    |
| Porta externa     | `5432`                                    |
| Database          | `spaceagrodb`                             |
| Usuário           | `spaceagro`                               |
| Volume            | `spaceagro_devops_postgres-data-rm566462` |

---

## Estrutura do Projeto

```txt
SpaceAgro_DEVOPS/
├── Controllers/
├── Data/
├── Migrations/
├── Models/
├── Services/
├── Properties/
├── docs/
│   ├── arquitetura-devops.svg
│   ├── COMANDOS_AZURE_VM.md
│   └── ROTEIRO_VIDEO_DEVOPS.md
├── Program.cs
├── SpaceAgro.DotNetApi.csproj
├── SpaceAgro.DotNetApi.http
├── appsettings.json
├── appsettings.Development.json
├── appsettings.Docker.json
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── .env.example
└── README.md
```

---

## Funcionalidades da API

A API possui CRUD completo para as principais entidades da solução.

### Talhões

A entidade **Talhão** representa uma área agrícola monitorada.

Endpoints disponíveis:

| Método | Endpoint            | Descrição               |
| ------ | ------------------- | ----------------------- |
| GET    | `/api/talhoes`      | Lista todos os talhões  |
| POST   | `/api/talhoes`      | Cadastra um novo talhão |
| GET    | `/api/talhoes/{id}` | Busca um talhão por ID  |
| PUT    | `/api/talhoes/{id}` | Atualiza um talhão      |
| DELETE | `/api/talhoes/{id}` | Remove um talhão        |

### Leituras de Sensores

A entidade **LeituraSensor** representa dados coletados por sensores ambientais.

Endpoints disponíveis:

| Método | Endpoint             | Descrição                 |
| ------ | -------------------- | ------------------------- |
| GET    | `/api/leituras`      | Lista todas as leituras   |
| POST   | `/api/leituras`      | Cadastra uma nova leitura |
| GET    | `/api/leituras/{id}` | Busca uma leitura por ID  |
| PUT    | `/api/leituras/{id}` | Atualiza uma leitura      |
| DELETE | `/api/leituras/{id}` | Remove uma leitura        |

### Clima Espacial

A API também possui endpoints voltados à análise climática/geoespacial.

| Método | Endpoint                                    | Descrição                           |
| ------ | ------------------------------------------- | ----------------------------------- |
| GET    | `/api/climaespacial/previsao`               | Retorna previsão climática simulada |
| GET    | `/api/climaespacial/diagnostico/{talhaoId}` | Retorna diagnóstico de um talhão    |

---

## Relacionamento entre Tabelas

A aplicação possui relacionamento entre as tabelas:

```txt
TB_TALHAO 1:N TB_LEITURA_SENSOR
```

Ou seja:

* Um talhão pode possuir várias leituras de sensor;
* Cada leitura de sensor está associada ao contexto de monitoramento agrícola da solução.

Tabelas existentes no banco:

```txt
TB_TALHAO
TB_LEITURA_SENSOR
```

---

## Pré-requisitos para Execução

Para executar o projeto, é necessário ter instalado:

* Git
* Docker
* Docker Compose

Em ambiente Windows, também é necessário estar com o **Docker Desktop** em execução.

---

## Como Executar Localmente

### 1. Clonar o repositório

```bash
git clone https://github.com/ThiagoSpositoo/SpaceAgro_DEVOPS.git
cd SpaceAgro_DEVOPS
```

### 2. Subir os containers

```bash
docker compose up -d --build
```

### 3. Verificar os containers

```bash
docker ps
```

Devem aparecer os containers:

```txt
api-rm566462
db-rm566462
```

### 4. Acessar o Swagger local

```txt
http://localhost:8080/swagger
```

### 5. Testar o Health Check local

```bash
curl http://localhost:8080/health
```

---

## Como Executar na VM Azure

### 1. Acessar a VM por SSH

```bash
ssh -i "CAMINHO_DA_CHAVE.pem" azureuser@68.154.48.178
```

### 2. Instalar Docker e Git na VM

```bash
sudo apt update
sudo apt install ca-certificates curl git -y
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
sudo systemctl enable docker
sudo systemctl start docker
```

### 3. Conferir as versões instaladas

```bash
docker --version
docker compose version
git --version
```

### 4. Clonar o repositório

```bash
git clone https://github.com/ThiagoSpositoo/SpaceAgro_DEVOPS.git
cd SpaceAgro_DEVOPS
```

### 5. Subir os containers

```bash
sudo docker compose up -d --build
```

### 6. Verificar os containers em execução

```bash
sudo docker ps
```

Resultado esperado:

```txt
api-rm566462
db-rm566462
```

### 7. Acessar a API em nuvem

```txt
http://68.154.48.178:8080/swagger
```

### 8. Testar o Health Check em nuvem

```txt
http://68.154.48.178:8080/health
```

---

## Comandos de Evidência Utilizados

### Verificar containers ativos

```bash
sudo docker ps
```

### Ver logs da API

```bash
sudo docker logs api-rm566462
```

### Ver logs do banco

```bash
sudo docker logs db-rm566462
```

### Acessar o container da API

```bash
sudo docker exec -it api-rm566462 bash
```

Dentro do container da API:

```bash
pwd
ls -l
whoami
exit
```

Resultado esperado do `whoami`:

```txt
appuser
```

Isso comprova que a aplicação não está rodando como usuário root.

### Acessar o PostgreSQL no container do banco

```bash
sudo docker exec -it db-rm566462 psql -U spaceagro -d spaceagrodb
```

Dentro do PostgreSQL:

```sql
\dt
SELECT * FROM "TB_TALHAO";
SELECT * FROM "TB_LEITURA_SENSOR";
\q
```

---

## Testes Realizados na API

### Health Check

```bash
curl http://localhost:8080/health
```

Resposta esperada:

```json
{
  "status": "healthy"
}
```

### Criar um Talhão

```bash
curl -X POST http://localhost:8080/api/talhoes \
-H "Content-Type: application/json" \
-d '{"nome":"Talhao Orbital Azure","cultura":"Soja","areaHectares":25.5,"latitude":-23.5505,"longitude":-46.6333,"idProdutor":1}'
```

### Listar Talhões

```bash
curl http://localhost:8080/api/talhoes
```

### Criar uma Leitura de Sensor

```bash
curl -X POST http://localhost:8080/api/leituras \
-H "Content-Type: application/json" \
-d '{"temperatura":32.5,"umidadeAr":61.2,"umidadeSolo":37.8,"idDispositivo":1}'
```

### Listar Leituras

```bash
curl http://localhost:8080/api/leituras
```

---

## Evidência de Persistência

Após criar registros pela API, os dados foram consultados diretamente no PostgreSQL dentro do container do banco.

Comando para acessar o banco:

```bash
sudo docker exec -it db-rm566462 psql -U spaceagro -d spaceagrodb
```

Comandos SQL utilizados:

```sql
\dt
SELECT * FROM "TB_TALHAO";
SELECT * FROM "TB_LEITURA_SENSOR";
```

Resultado validado:

* A tabela `TB_TALHAO` apresentou os talhões cadastrados;
* A tabela `TB_LEITURA_SENSOR` apresentou as leituras cadastradas;
* Isso comprova que os dados criados pela API foram persistidos no banco de dados em container.

---

## Dockerfile

A aplicação possui um `Dockerfile` próprio para gerar a imagem personalizada da API .NET.

Principais pontos atendidos:

* Uso de imagem base oficial do .NET;
* Build da aplicação em etapa separada;
* Publicação da aplicação em modo Release;
* Definição de diretório de trabalho `/app`;
* Criação de usuário não-root `appuser`;
* Exposição da porta `8080`;
* Execução da aplicação dentro do container.

---

## Docker Compose

O arquivo `docker-compose.yml` é responsável por subir os dois containers da solução:

* `api-rm566462`
* `db-rm566462`

Principais recursos utilizados:

* Build da imagem personalizada da API;
* Container PostgreSQL com imagem pública;
* Rede Docker compartilhada;
* Volume nomeado para persistência;
* Variáveis de ambiente;
* Portas expostas;
* Dependência entre API e banco.

---

## Variáveis de Ambiente

### API

| Variável                               | Descrição                                   |
| -------------------------------------- | ------------------------------------------- |
| `ASPNETCORE_ENVIRONMENT`               | Define o ambiente Docker                    |
| `ASPNETCORE_URLS`                      | Define a porta de execução da API           |
| `ConnectionStrings__DefaultConnection` | Define a string de conexão com o PostgreSQL |

### Banco de Dados

| Variável            | Descrição              |
| ------------------- | ---------------------- |
| `POSTGRES_DB`       | Nome do banco de dados |
| `POSTGRES_USER`     | Usuário do banco       |
| `POSTGRES_PASSWORD` | Senha do banco         |

---

## Portas Utilizadas

| Porta | Serviço    | Descrição                   |
| ----- | ---------- | --------------------------- |
| 22    | SSH        | Acesso remoto à VM          |
| 8080  | API .NET   | Acesso externo à aplicação  |
| 5432  | PostgreSQL | Banco de dados em container |

---

## Volume Nomeado

O banco utiliza volume nomeado para garantir persistência dos dados:

```txt
spaceagro_devops_postgres-data-rm566462
```

Esse volume mantém os dados do PostgreSQL mesmo se o container for parado ou recriado.

---

## Rede Docker

Os containers se comunicam por meio da rede Docker criada pelo Compose:

```txt
spaceagro_devops_spaceagro-network-rm566462
```

A API acessa o banco utilizando o nome do serviço/container do banco na rede Docker.

---

## Checklist dos Requisitos DevOps

| Requisito                                      | Status   |
| ---------------------------------------------- | -------- |
| API .NET conteinerizada                        | Atendido |
| Dockerfile da aplicação                        | Atendido |
| Imagem personalizada da aplicação              | Atendido |
| Banco de dados em container separado           | Atendido |
| PostgreSQL utilizado como banco                | Atendido |
| Dois containers integrados                     | Atendido |
| Rede Docker entre API e banco                  | Atendido |
| Volume nomeado para persistência               | Atendido |
| Variáveis de ambiente na API                   | Atendido |
| Variáveis de ambiente no banco                 | Atendido |
| Portas expostas                                | Atendido |
| Container da aplicação com RM no nome          | Atendido |
| Container do banco com RM no nome              | Atendido |
| Aplicação executando com usuário não-root      | Atendido |
| Diretório de trabalho definido no Dockerfile   | Atendido |
| CRUD completo                                  | Atendido |
| Mínimo de duas tabelas relacionadas            | Atendido |
| SELECT no banco para evidência de persistência | Atendido |
| Execução em nuvem na Azure                     | Atendido |
| README com How To                              | Atendido |
| Arquitetura macro documentada                  | Atendido |

---

## Roteiro Resumido da Demonstração

Durante o vídeo, foram demonstrados:

1. Repositório GitHub do projeto;
2. README com instruções de execução;
3. VM Linux criada na Azure;
4. Acesso remoto por SSH;
5. Instalação do Docker e Git;
6. Clone do repositório dentro da VM;
7. Execução do `docker compose up -d --build`;
8. Verificação dos containers com `sudo docker ps`;
9. Acesso ao Swagger pelo IP público;
10. Teste do endpoint `/health`;
11. Criação e listagem de registros pela API;
12. Acesso direto ao banco PostgreSQL dentro do container;
13. Execução de comandos `SELECT`;
14. Verificação do usuário não-root com `whoami`;
15. Exibição dos logs da API e do banco.

---

## Integrantes

| Nome                 | RM | Turma |
| -------------------- | -- | ----- |
| Thiago Sposito | 561694 | 2TDSA |
| Vitor Madrigrano | 564191 | 2TDSR |
| Pedro Henrique Gomes Silva  | 562606 | 2TDSA |
| Murilo Macedo Silva | 566462 | 2TDSA |
| Lucas Lopes Rodrigues | 563544 | 2TDSA |

---

## Considerações Finais

A entrega DevOps do projeto **SpaceAgro** demonstra a execução de uma API .NET integrada a um banco PostgreSQL em containers Docker, utilizando Docker Compose em uma VM Linux na Azure.

A solução atende aos requisitos propostos ao apresentar:

* Aplicação conteinerizada;
* Banco em container separado;
* Persistência de dados com volume nomeado;
* Comunicação entre containers via rede Docker;
* Variáveis de ambiente;
* Portas expostas;
* Execução em nuvem;
* Evidências de funcionamento por logs, Swagger, comandos Docker e consultas SQL.

Com isso, a aplicação deixa de funcionar apenas em ambiente local e passa a ser executada em uma infraestrutura mais próxima de um ambiente real de implantação.
