# Roteiro do vídeo - DevOps Tools & Cloud Computing

## 1. Abertura
Apresentar o projeto SpaceAgro, uma API .NET voltada ao monitoramento agroclimatológico com dados de sensores, talhões e integração com dados espaciais da NASA.

## 2. Mostrar o README
Explicar que o repositório possui Dockerfile, docker-compose.yml, arquitetura macro, tutorial de execução e comandos de teste.

## 3. Clonar o repositório na VM em nuvem
```bash
git clone LINK_DO_REPOSITORIO
cd SpaceAgro.DotNetApi
```

## 4. Subir os containers em background
```bash
sudo docker compose up -d --build
```

## 5. Mostrar containers rodando
```bash
sudo docker ps
```

Mostrar os containers:
- api-rm566462
- db-rm566462

## 6. Mostrar logs dos containers
```bash
sudo docker logs api-rm566462
sudo docker logs db-rm566462
```

## 7. Entrar no container da API
```bash
sudo docker exec -it api-rm566462 sh
pwd
ls -l
whoami
exit
```

O `whoami` deve mostrar `appuser`, comprovando que a API não roda como root.

## 8. Entrar no container do banco
```bash
sudo docker exec -it db-rm566462 sh
pwd
ls -l
whoami
exit
```

## 9. Testar API
No navegador ou terminal:
```bash
curl http://IP_DA_VM:8080/health
curl http://IP_DA_VM:8080/api/talhoes
curl http://IP_DA_VM:8080/api/leituras
```

Swagger:
```text
http://IP_DA_VM:8080/swagger
```

## 10. Demonstrar CRUD
Criar um talhão:
```bash
curl -X POST http://IP_DA_VM:8080/api/talhoes \
  -H "Content-Type: application/json" \
  -d '{"nome":"Talhao Orbital 02","cultura":"Soja","areaHectares":25.4,"latitude":-22.90,"longitude":-47.06,"idProdutor":2}'
```

Criar uma leitura:
```bash
curl -X POST http://IP_DA_VM:8080/api/leituras \
  -H "Content-Type: application/json" \
  -d '{"temperatura":34.5,"umidadeAr":55.2,"umidadeSolo":28.7,"idDispositivo":1}'
```

Listar novamente:
```bash
curl http://IP_DA_VM:8080/api/talhoes
curl http://IP_DA_VM:8080/api/leituras
```

## 11. Provar persistência com SELECT no banco
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

## 12. Encerramento
Explicar que a aplicação está em nuvem, conteinerizada com Docker, com API e banco em containers separados, rede Docker, volume nomeado, variáveis de ambiente, portas expostas e persistência validada por SELECT direto no banco.
