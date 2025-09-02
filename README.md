# DTI — Leads (Invited / Accepted)

Projeto fullstack simples para gerenciar **leads** com abas **Invited** e **Accepted**.

- **Backend**: .NET 8 (ASP.NET Core Web API) + **SQL Server** + **EF Core**
- **Frontend**: React (CRA) + CSS puro
- **Testes**: xUnit (+ EFCore InMemory)
- **Regras de negócio**:
  - **Accept**: muda status para **Aceito**; se `price > 500`, aplica **10% de desconto**; registra notificação "fake email" em arquivo `.txt`.
  - **Decline**: muda status para **Recusado**.


---

## ⚙️ Pré-requisitos

- **.NET 8 SDK** (`dotnet --version` >= 8)
- **Node 18+** (`node -v`) e **npm**
- **SQL Server** (Express, LocalDB, Developer ou Full)

⚠️ **Importante**: se você estiver usando **OneDrive**, evite rodar o projeto dentro dele (pode causar bloqueio de portas). Se der erro de rede, mova a pasta para fora do OneDrive.

---

⚙️ Configuração do Banco de Dados

Arquivo: **backend/DTI.Api/appsettings.json**

Copia o código abaixo e coloca no arquivo json em cima.

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DTI;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" }
  },
  "AllowedHosts": "*"
}


👉 Ajuste o Server= conforme sua instância do SQL Server:

**Server=localhost\\SQLEXPRESS → SQL Server Express**
(No caso, foi usado o SQLEXPRESS nessa aplicação)

Server=localhost → SQL Server Developer / Full

Server=(localdb)\\MSSQLLocalDB → LocalDB

🚀 Rodando o Backend
1) Restaurar e compilar
cd backend
dotnet build

2) Criar/atualizar banco com EF:
**dotnet ef database update -p DTI.Infrastructure -s DTI.Api**

3) Subir a API:
**dotnet run --project DTI.Api**


API: http://localhost:5206/api/Leads
Swagger: http://localhost:5206/swagger
(No caso do meu computador, o Kestrel escolheu a porta 5206 para rodar a aplicação, para forçar que a aplicação suba na porta 5206 use "dotnet run --project DTI.Api --urls "http://localhost:5206")

🧪 Testando no Swagger

Exemplo de POST /api/Leads (⚠️ não inclua id nem dateCreated):

Copia esse json e coloca no try out do Swagger

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

Endpoints principais

GET /api/Leads → todos

GET /api/Leads?status=Pendente → convidados

GET /api/Leads?status=Aceito → aceitos

GET /api/Leads?status=Recusado → recusados

PUT /api/Leads/{id}/accept → aceita (aplica desconto e cria notificação .txt)

PUT /api/Leads/{id}/decline → recusa

📂 Notificações ficam em:
backend/DTI.Api/Notifications/*.txt

🎨 Frontend (React)
1) Configurar API base

Abra outro terminal.

Arquivo: frontend/src/services/api.js

**import axios from "axios";**

const api = axios.create({
  baseURL: "http://localhost:5206/api" // NÃO inclua /leads aqui
});

export default api;

2) Instalar dependências e rodar
**cd frontend
npm install
npm start**


**App disponível em: http://localhost:3000**
(O react usa como porta padrão a 3000)

Aba Invited → lista leads status = "Pendente".

Botões Accept / Decline → atualizam o backend.

Aba Accepted → lista leads aceitos (com extras: nome completo, telefone, email).

🔧 Exemplo via cURL
PowerShell (Windows)
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

Linux/macOS
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

✅ Testes

No diretório backend/:

**dotnet test**


Saída esperada: todos os testes aprovados.

🐛 Problemas comuns

Página React vazia → confirme que a API está rodando em http://localhost:5206 e que frontend/src/services/api.js aponta para baseURL: "http://localhost:5206/api".

CORS / Network Error → reinicie o backend; veja se o Program.cs habilita CORS.

404 em / → a API não tem página HTML em /, use /swagger ou /api/Leads.
