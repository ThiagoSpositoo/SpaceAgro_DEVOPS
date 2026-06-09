# SpaceAgro DotNet API - Global Solution 2026/1

## Descrição da Solução

O **SpaceAgro** é uma API desenvolvida em **.NET** para a Global Solution 2026/1, com foco na temática de **economia espacial** aplicada a problemas reais da Terra.

A proposta da solução é utilizar tecnologia, dados e infraestrutura para apoiar o monitoramento agrícola inteligente, permitindo o cadastro de talhões e o registro de leituras de sensores relacionadas ao ambiente agrícola.

A solução pode ser integrada a dados espaciais, como informações climáticas e satelitais, para auxiliar na tomada de decisão, prevenção de riscos e melhoria da produtividade no agronegócio.

---

## Objetivo da Entrega DevOps

Esta entrega tem como objetivo demonstrar a conteinerização da API .NET e do banco de dados utilizando Docker, com execução em ambiente de nuvem.

A arquitetura da solução utiliza dois containers Docker integrados:

* Um container para a **API .NET**
* Um container para o **banco de dados PostgreSQL**

Os dois containers executam na mesma rede Docker, com persistência de dados através de volume nomeado.

---

## Tecnologias Utilizadas

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Docker
* Docker Compose
* Swagger/OpenAPI
* Azure VM / Ambiente em Nuvem

---

## Estrutura do Projeto

```txt
SpaceAgro.DotNetApi_DEVOPS/
├── Controllers/
├── Data/
├── Models/
├── DTOs/
├── Program.cs
├── appsettings.json
├── appsettings.Docker.json
├── SpaceAgro.DotNetApi.csproj
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── README.md
└── docs/
    ├── arquitetura-devops.svg
    ├── COMANDOS_AZURE_VM.md
    └── ROTEIRO_VIDEO_DEVOPS.md
```

---

## Arquitetura DevOps

A arquitetura da solução foi construída para executar em ambiente de nuvem utilizando containers Docker.

Fluxo da arquitetura:

```txt
Usuário
   ↓
Internet
   ↓
VM em Nuvem
   ↓
Docker Engine
   ↓
Docker Network: gs-network-rm566462
   ├── Container api-rm566462
   │      └── API .NET - Porta 8080
   │
   └── Container db-rm566462
          └── PostgreSQL - Porta 5432
                 ↓
            Volume postgres-data-rm566462
```

### Componentes da Arquitetura

| Componente         | Descrição                                           |
| ------------------ | --------------------------------------------------- |
| Usuário            | Pessoa que acessa a API ou o Swagger pelo navegador |
| VM em Nuvem        | Servidor onde os containers Docker são executados   |
| Docker Engine      | Responsável por executar e gerenciar os containers  |
| Container da API   | Executa a aplicação .NET                            |
| Container do Banco | Executa o banco PostgreSQL                          |
| Docker Network     | Permite a comunicação entre API e banco             |
| Volume Nomeado     | Garante a persistência dos dados do banco           |

---

## Containers da Solução

### Container da API

| Item                  | Valor          |
| --------------------- | -------------- |
| Nome do container     | `api-rm566462` |
| Porta interna         | `8080`         |
| Porta externa         | `8080`         |
| Tecnologia            | .NET 8         |
| Usuário do container  | `appuser`      |
| Diretório de trabalho | `/app`         |

### Container do Banco de Dados

| Item              | Valor                    |
| ----------------- | ------------------------ |
| Nome do container | `db-rm566462`            |
| Banco             | PostgreSQL               |
| Porta interna     | `5432`                   |
| Porta externa     | `5432`                   |
| Database          | `spaceagrodb`            |
| Usuário           | `spaceagro`              |
| Volume            | `postgres-data-rm566462` |

---

## Funcionalidades da API

A API possui CRUD completo para as seguintes entidades:

### Talhão

Representa uma área agrícola monitorada pela solução.

Operações disponíveis:

* Criar talhão
* Listar talhões
* Buscar talhão por ID
* Atualizar talhão
* Excluir talhão

### Leitura de Sensor

Representa os dados coletados de sensores associados a um talhão.

Operações disponíveis:

* Criar leitura de sensor
* Listar leituras
* Buscar leitura por ID
* Atualizar leitura
* Excluir leitura

---

## Relacionamento entre Tabelas

A solução possui relacionamento entre as tabelas:

```txt
TB_TALHAO 1:N TB_LEITURA_SENSOR
```

Ou seja:

* Um talhão pode possuir várias leituras de sensor.
* Cada leitura de sensor pertence a um talhão.

Esse relacionamento permite registrar múltiplas medições ambientais para uma mesma área agrícola.

---

## Pré-requisitos

Para executar o projeto, é necessário ter instalado:

* Docker
* Docker Compose
* Git

No Windows, também é necessário estar com o **Docker Desktop aberto e em execução**.

---

## Como Executar Localmente

### 1. Clonar o Repositório

```bash
git clone COLOQUE_AQUI_O_LINK_DO_REPOSITORIO
cd SpaceAgro.DotNetApi_DEVOPS
```

### 2. Verificar se o Docker Está Funcionando

```bash
docker --version
docker compose version
docker info
```

### 3. Subir os Containers

```bash
docker compose up -d --build
```

Esse comando irá:

* Construir a imagem da API .NET
* Baixar a imagem do PostgreSQL
* Criar a rede Docker
* Criar o volume nomeado do banco
* Subir o container da API
* Subir o container do banco de dados

### 4. Verificar os Containers em Execução

```bash
docker ps
```

Devem aparecer os containers:

```txt
api-rm566462
db-rm566462
```

### 5. Verificar os Logs da API

```bash
docker logs api-rm566462
```

### 6. Verificar os Logs do Banco

```bash
docker logs db-rm566462
```

---

## Acessos da Aplicação

### Swagger

```txt
http://localhost:8080/swagger
```

### Health Check

```txt
http://localhost:8080/health
```

---

## Comandos de Teste da API

### Testar Health Check

```bash
curl http://localhost:8080/health
```

### Acessar Swagger pelo navegador

```txt
http://localhost:8080/swagger
```

---

## Evidências de Execução dos Containers

Durante a demonstração, foram utilizados os comandos abaixo para comprovar o funcionamento da solução.

### Verificar containers ativos

```bash
docker ps
```

### Exibir logs da API

```bash
docker logs api-rm566462
```

### Exibir logs do banco

```bash
docker logs db-rm566462
```

---

## Acessando o Container da API

```bash
docker exec -it api-rm566462 bash
```

Dentro do container:

```bash
pwd
ls -l
whoami
exit
```

O comando `whoami` deve retornar:

```txt
appuser
```

Isso demonstra que a aplicação não está sendo executada com usuário root.

---

## Acessando o Container do Banco

```bash
docker exec -it db-rm566462 bash
```

Dentro do container:

```bash
pwd
ls -l
whoami
exit
```

---

## Evidência de Persistência no Banco de Dados

Para acessar o PostgreSQL dentro do container:

```bash
docker exec -it db-rm566462 psql -U spaceagro -d spaceagrodb
```

Dentro do PostgreSQL, executar:

```sql
\dt
SELECT * FROM "TB_TALHAO";
SELECT * FROM "TB_LEITURA_SENSOR";
```

Para sair do PostgreSQL:

```sql
\q
```

Esses comandos demonstram que os dados criados pela API estão sendo persistidos no banco de dados em container.

---

## Executando em Ambiente de Nuvem

A execução em nuvem pode ser feita em uma VM Linux, como uma máquina virtual na Azure.

### 1. Atualizar pacotes da VM

```bash
sudo apt update
```

### 2. Instalar Docker, Docker Compose e Git

```bash
sudo apt install docker.io docker-compose-plugin git -y
```

### 3. Habilitar e iniciar o Docker

```bash
sudo systemctl enable docker
sudo systemctl start docker
```

### 4. Clonar o repositório

```bash
git clone (https://github.com/ThiagoSpositoo/SpaceAgro_DEVOPS)
cd SpaceAgro.DotNetApi_DEVOPS
```

### 5. Subir os containers

```bash
sudo docker compose up -d --build
```

### 6. Verificar os containers

```bash
sudo docker ps
```

### 7. Acessar a API em nuvem

```txt
http://IP_DA_VM:8080/swagger
```

---

## Portas Utilizadas

| Porta | Serviço    |
| ----- | ---------- |
| 22    | SSH da VM  |
| 8080  | API .NET   |
| 5432  | PostgreSQL |

---

## Variáveis de Ambiente

### API

| Variável                               | Descrição                                       |
| -------------------------------------- | ----------------------------------------------- |
| `ASPNETCORE_ENVIRONMENT`               | Define o ambiente da aplicação                  |
| `ASPNETCORE_URLS`                      | Define a URL usada pela API dentro do container |
| `ConnectionStrings__DefaultConnection` | String de conexão com o banco PostgreSQL        |

### Banco

| Variável            | Descrição              |
| ------------------- | ---------------------- |
| `POSTGRES_DB`       | Nome do banco de dados |
| `POSTGRES_USER`     | Usuário do banco       |
| `POSTGRES_PASSWORD` | Senha do banco         |

---

## Volume Nomeado

O banco utiliza o volume nomeado:

```txt
postgres-data-rm566462
```

Esse volume garante que os dados do PostgreSQL sejam mantidos mesmo que o container seja parado ou recriado.

---

## Rede Docker

Os containers utilizam a rede:

```txt
gs-network-rm566462
```

Essa rede permite que a API se comunique com o banco usando o nome do container do banco:

```txt
db-rm566462
```

---

## Como Parar os Containers

```bash
docker compose down
```

---

## Como Parar e Remover os Dados do Banco

Atenção: este comando remove também o volume do banco.

```bash
docker compose down -v
```

---

## Checklist de Requisitos DevOps

| Requisito                              | Status   |
| -------------------------------------- | -------- |
| Container da aplicação .NET            | Atendido |
| Container do banco de dados            | Atendido |
| Banco PostgreSQL em container separado | Atendido |
| Dockerfile da aplicação                | Atendido |
| Docker Compose                         | Atendido |
| Rede Docker entre API e banco          | Atendido |
| Volume nomeado no banco                | Atendido |
| Variáveis de ambiente                  | Atendido |
| Portas expostas                        | Atendido |
| Container com RM no nome               | Atendido |
| Aplicação com usuário não-root         | Atendido |
| Diretório de trabalho definido         | Atendido |
| CRUD completo                          | Atendido |
| Duas tabelas relacionadas              | Atendido |
| Evidência com SELECT no banco          | Atendido |
| Execução em nuvem                      | Atendido |
| Logs dos containers                    | Atendido |
| Comandos docker exec                   | Atendido |

---

## Roteiro Resumido da Demonstração

Durante o vídeo de demonstração, devem ser apresentados:

1. Repositório no GitHub.
2. README com o tutorial de execução.
3. Clone do repositório na VM.
4. Execução do comando `docker compose up -d --build`.
5. Verificação dos containers com `docker ps`.
6. Logs da API e do banco.
7. Acesso ao Swagger.
8. Teste do CRUD.
9. Acesso ao container da API com `docker exec`.
10. Execução dos comandos `pwd`, `ls -l` e `whoami`.
11. Acesso ao container do banco.
12. Execução dos comandos `pwd`, `ls -l` e `whoami`.
13. SELECT nas tabelas do banco.
14. Explicação da arquitetura macro da solução.

---

## Links da Entrega

### GitHub

```txt
(https://github.com/ThiagoSpositoo/SpaceAgro_DEVOPS)
```

### Vídeo no YouTube

```txt
COLOQUE_AQUI_O_LINK_DO_VIDEO
```

### API em Nuvem

```txt
http://IP_DA_VM:8080/swagger
```

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

A entrega DevOps do projeto SpaceAgro demonstra a conteinerização de uma API .NET integrada a um banco PostgreSQL, utilizando Docker e Docker Compose.
A solução atende aos requisitos da Global Solution ao apresentar uma arquitetura com dois containers integrados, comunicação por rede Docker, persistência de dados com volume nomeado, variáveis de ambiente, portas expostas, CRUD completo e evidências de funcionamento em ambiente de nuvem.
Com isso, o projeto deixa de funcionar apenas em ambiente local e passa a ter uma estrutura mais próxima de uma aplicação real, executável, documentada e preparada para implantação em cloud.
