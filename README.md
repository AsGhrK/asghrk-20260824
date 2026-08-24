# Sistema de Gestão de Colaboradores e Unidades

Sistema para cadastro e gestão de **Usuários**, **Colaboradores** e **Unidades**, com autenticação via Bearer token (JWT).

- **Backend**: C# / ASP.NET Core Web API (arquitetura MVC)
- **Frontend**: Angular (standalone components)
- **Banco de dados**: PostgreSQL (via Docker)

## Arquitetura

O backend é dividido em 4 camadas, cada uma em seu próprio projeto .NET:

```
backend/
  src/
    GestaoColaboradores.Domain/         # Entidades e pattern de herança
    GestaoColaboradores.Infrastructure/ # EF Core, DbContext, Repositórios
    GestaoColaboradores.Application/    # DTOs, regras de negócio (Services), JWT
    GestaoColaboradores.Api/            # Controllers (MVC), Program.cs
  tests/
    GestaoColaboradores.Tests/          # xUnit
frontend/                               # Angular
docker-compose.yml                      # PostgreSQL
postman/                                # Coleção + ambiente para testes manuais
```

### Pattern de herança

Usuário, Unidade e Colaborador compartilham a mesma estrutura de cadastro (um código único
+ campos de auditoria). Isso é modelado com uma cadeia de herança no domínio:

```
EntidadeBase (Id, CriadoEm, AtualizadoEm)
  └── CadastroBase (Codigo)
        ├── Usuario (Login, SenhaHash, Ativo)
        ├── Unidade (Nome, Ativo, Colaboradores)
        └── Colaborador (Nome, UnidadeId, UsuarioId)
```

Na camada de infraestrutura, o mesmo princípio se repete com um repositório genérico
`Repository<T> : IRepository<T> where T : EntidadeBase`, especializado por
`UsuarioRepository`, `UnidadeRepository` e `ColaboradorRepository`.

### Regras de negócio principais

- Usuário: cadastro com código, login, senha e status; atualização permite alterar **apenas** senha e status.
- Colaborador: cadastro exige unidade **ativa** e usuário existente e ainda não vinculado a outro colaborador; atualização permite alterar nome e unidade; pode ser removido.
- Unidade: cadastro com código único e nome; pode ser inativada — unidade inativa **não permite novos colaboradores** (validado em `ColaboradorService`).

## Como rodar

### 1. Banco de dados (Docker)

```bash
docker compose up -d
```

Sobe um PostgreSQL 16 em `localhost:5432` (`gestao_colaboradores` / usuário `postgres` / senha `postgres`),
já compatível com a connection string padrão em `backend/src/GestaoColaboradores.Api/appsettings.json`.

### 2. Backend

```bash
cd backend
dotnet restore
dotnet run --project src/GestaoColaboradores.Api
```

As migrations do EF Core são aplicadas automaticamente na inicialização (em ambiente de desenvolvimento).
A API sobe em `https://localhost:7162` (e `http://localhost:5058`), com Swagger em `/swagger`.

Para rodar os testes:

```bash
dotnet test
```

### 3. Frontend

```bash
cd frontend
npm install
npm start
```

Acesse `http://localhost:4200`. O `environment.ts` já aponta para `https://localhost:7162/api`.

### 4. Testando via Postman

Importe `postman/GestaoColaboradores.postman_collection.json` e o ambiente
`postman/GestaoColaboradores.postman_environment.json`. O request **Auth > Login** salva o token
retornado na variável de ambiente `token`, usada automaticamente (Bearer auth) pelos demais requests.

## Autenticação

Todas as rotas (exceto `POST /api/auth/login`) exigem um header `Authorization: Bearer {token}`.
O token é emitido no login e expira conforme `Jwt:ExpiryMinutes` em `appsettings.json` (60 min por padrão).

## Endpoints

| Recurso | Método | Rota | Descrição |
|---|---|---|---|
| Auth | POST | `/api/auth/login` | Autentica e retorna o token |
| Usuários | POST | `/api/usuarios` | Cadastra usuário |
| Usuários | PUT | `/api/usuarios/{id}` | Atualiza senha/status |
| Usuários | GET | `/api/usuarios?ativo=true` | Lista (com filtro de status opcional) |
| Usuários | GET | `/api/usuarios/{id}` | Consulta por id |
| Unidades | POST | `/api/unidades` | Cadastra unidade |
| Unidades | PUT | `/api/unidades/{id}` | Ativa/inativa a unidade |
| Unidades | GET | `/api/unidades` | Lista com colaboradores relacionados |
| Unidades | GET | `/api/unidades/{id}` | Consulta por id |
| Colaboradores | POST | `/api/colaboradores` | Cadastra colaborador |
| Colaboradores | PUT | `/api/colaboradores/{id}` | Atualiza nome/unidade |
| Colaboradores | DELETE | `/api/colaboradores/{id}` | Remove colaborador |
| Colaboradores | GET | `/api/colaboradores` | Lista colaboradores |
| Colaboradores | GET | `/api/colaboradores/{id}` | Consulta por id |

## Notas

- Os testes de frontend (`ng test`) usam Karma + Chrome; se o Chrome não estiver instalado na máquina,
  defina a variável `CHROME_BIN` apontando para um binário Chrome/Chromium.
