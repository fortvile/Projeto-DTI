DTI — Leads (Invited / Accepted)

Projeto full-stack simples para gerenciar leads com abas Invited e Accepted.

Backend: .NET 8 (ASP.NET Core Web API) + SQL Server + EF Core

Frontend: React (CRA) + CSS puro

Testes: xUnit (EFCore InMemory)

Regras:

Accept: muda status para Aceito; se price > 500, aplica 10% de desconto; registra “fake e-mail” em arquivo .txt.

Decline: muda status para Recusado.

Arquitetura & Pastas
DTI/
├─ backend/
│  ├─ DTI.Api/            # Web API (controllers, Program.cs, Swagger, Notifications/)
│  ├─ DTI.Domain/         # Entidades (Lead)
│  ├─ DTI.Infrastructure/ # DbContext, EF Core, Migrations
│  └─ DTI.Tests/          # Testes xUnit
└─ frontend/              # React (CRA)

Pré-requisitos

.NET 8 SDK — dotnet --version ≥ 8

Node 18+ e npm — node -v

SQL Server (Express / LocalDB / Developer / Full)

Windows + OneDrive: se notar erros de rede/porta, execute o projeto fora do OneDrive.

Banco de Dados (Connection String)

Arquivo: backend/DTI.Api/appsettings.json

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DTI;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" }
  },
  "AllowedHosts": "*"
}


Ajuste Server= conforme sua instância:

Instância padrão local: Server=localhost

SQL Express: Server=localhost\\SQLEXPRESS

LocalDB: Server=(localdb)\\MSSQLLocalDB

Instância nomeada: Server=SEU-PC\\NOME

Se precisar usuário/senha:
Server=localhost;Database=DTI;User Id=sa;Password=SUASENHA;TrustServerCertificate=True;

Backend — Configurar, Migrar e Rodar

No diretório DTI/backend:

dotnet build


Criar/atualizar banco com EF (se já há migrations):

dotnet ef database update -p DTI.Infrastructure -s DTI.Api


Criar migration do zero (opcional):

dotnet ef migrations add InitialCreate -p DTI.Infrastructure -s DTI.Api
dotnet ef database update -p DTI.Infrastructure -s DTI.Api


Rodar a API:

dotnet run --project DTI.Api


API: http://localhost:5206

Swagger: http://localhost:5206/swagger

Endpoint principal: http://localhost:5206/api/Leads

Os “e-mails” fake de Accept são gravados em arquivos .txt em:
backend/DTI.Api/Notifications/ (criados automaticamente na primeira aceitação).

Testando pelo Swagger

Abra http://localhost:5206/swagger e use este JSON no POST /api/Leads
(não envie id nem dateCreated):

{
  "category": "Dev ;)",
  "contactFirstName": "Rafael",
  "contactLastName": "Vilefort",
  "description": "Candidato à estágio",
  "email": "rafaelvilefort@gmail.com",
  "isContacted": false,
  "phoneNumber": "(31) 99373-8307",
  "price": 0,
  "suburb": "BH",
  "status": "Pendente"
}

Listar Leads

GET /api/Leads — todos

GET /api/Leads?status=Pendente — convidados

GET /api/Leads?status=Aceito — aceitos

GET /api/Leads?status=Recusado — recusados

Accept / Decline

PUT /api/Leads/{id}/accept

Se price > 500 ⇒ aplica 10% de desconto

Registra “e-mail” fake em arquivo .txt

PUT /api/Leads/{id}/decline

Frontend (React)

Configurar base da API:

Arquivo: frontend/src/services/api.js

import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5206/api" // NÃO inclua /leads aqui
});

export default api;


Instalar e rodar:

cd frontend
npm install
npm start


App: http://localhost:3000

Aba Invited lista status = "Pendente" e mostra botões Accept / Decline

Aba Accepted lista aceitos (exibe também nome completo, telefone, email)

Exemplos via cURL
PowerShell (Windows) — use aspas duplas e escape com \"
curl -Method POST `
  -Uri "http://localhost:5206/api/Leads" `
  -Headers @{ "Content-Type" = "application/json" } `
  -Body "{
    \"category\":\"Dev ;)\",
    \"contactFirstName\":\"Rafael\",
    \"contactLastName\":\"Vilefort\",
    \"description\":\"Candidato à estágio\",
    \"email\":\"rafaelvilefort@gmail.com\",
    \"isContacted\":false,
    \"phoneNumber\":\"(31) 99373-8307\",
    \"price\":0,
    \"suburb\":\"BH\",
    \"status\":\"Pendente\"
  }"

Git Bash / Linux / macOS
curl -X POST "http://localhost:5206/api/Leads" \
  -H "Content-Type: application/json" \
  -d '{
    "category":"Dev ;)",
    "contactFirstName":"Rafael",
    "contactLastName":"Vilefort",
    "description":"Candidato à estágio",
    "email":"rafaelvilefort@gmail.com",
    "isContacted":false,
    "phoneNumber":"(31) 99373-8307",
    "price":0,
    "suburb":"BH",
    "status":"Pendente"
  }'


Accept / Decline:

curl -X PUT "http://localhost:5206/api/Leads/1/accept"
curl -X PUT "http://localhost:5206/api/Leads/1/decline"

Testes

No diretório DTI/backend:

dotnet test


Saída esperada: testes passando (criação/listagem de lead em memória).

Problemas comuns

Página React vazia: confirme que a API está em http://localhost:5206 e o frontend/src/services/api.js usa baseURL: "http://localhost:5206/api".

CORS / Network Error: reinicie o backend; confira Program.cs e o Swagger; verifique firewall/antivírus.

404 em / do backend: a API não tem HTML em / — use /swagger ou /api/Leads.