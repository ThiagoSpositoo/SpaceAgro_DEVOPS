# Comandos para executar na VM Azure

## Instalar Docker e Git
```bash
sudo apt update
sudo apt install docker.io docker-compose-plugin git -y
sudo systemctl enable docker
sudo systemctl start docker
```

## Verificar instalação
```bash
docker --version
docker compose version
git --version
```

## Clonar o repositório
```bash
git clone LINK_DO_REPOSITORIO
cd SpaceAgro.DotNetApi
```

## Subir a aplicação
```bash
sudo docker compose up -d --build
```

## Verificar containers
```bash
sudo docker ps
```

## Acessar API
```text
http://IP_DA_VM:8080/swagger
http://IP_DA_VM:8080/health
```

## Abrir portas no Azure NSG
Liberar as portas:
- 22 para SSH
- 8080 para API
- 5432 para banco, se o professor solicitar evidência de acesso externo
