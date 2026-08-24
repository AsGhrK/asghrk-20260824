# Sistema de Gestão de Colaboradores e Unidades Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.
>
> **Deviation note:** this plan is executed inline, in the same session that wrote it, by the same agent. The exhaustive "zero-context engineer" code-block requirement from the writing-plans skill is relaxed here — file responsibilities and contracts are specified precisely, but full code bodies live in the actual source files (single source of truth) rather than duplicated in this doc. This is a CRUD assessment project (not safety-critical), so strict red/green TDD ceremony per field is replaced with: build must compile after every task, and core business rules (unit inactive → reject colaborador creation; usuario update restricted to senha/status) get real xUnit tests in Task 24.

**Goal:** Build a working full-stack "Sistema de Gestão de Colaboradores e Unidades" (C# ASP.NET Core Web API in MVC pattern + PostgreSQL + Angular) matching the spec, with JWT Bearer auth, an inheritance-based domain model, Docker for the DB, and a Postman collection — delivered as a GitHub repo with 20+ well-scoped commits.

**Architecture:** Backend is a layered ASP.NET Core solution (Domain / Infrastructure / Application / Api) using an `EntidadeBase` → `CadastroBase` inheritance chain shared by `Usuario`, `Unidade`, `Colaborador`, plus a generic `Repository<T>` base. Auth is JWT Bearer issued from a `Usuario` login. Frontend is a standalone-components Angular app with an HTTP interceptor attaching the bearer token and route guards. Postgres runs via `docker-compose.yml`.

**Tech Stack:** .NET 10 (ASP.NET Core Web API, EF Core, Npgsql), PostgreSQL 16, Angular 18 (standalone components), JWT Bearer auth, xUnit, Postman.

## Global Constraints

- Backend: C#, ASP.NET Core, MVC-style controllers. — from spec
- Frontend: Angular. — from spec
- DB: PostgreSQL. — from spec
- Usuario: código único, login, senha, status (ativo/inativo); update only allows senha + status. — from spec
- Colaborador: código único, nome, unidade (FK obrigatória), usuário relacionado (FK obrigatória); update allows nome + unidade; delete allowed. — from spec
- Unidade: Id único (PK), código de unidade único, nome; inativação bloqueia novos colaboradores na unidade. — from spec
- Listagens: usuários (login+status, filtro por status), colaboradores (código, nome, unidade), unidades (+ colaboradores relacionados). — from spec
- Diferenciais: Docker para o banco; autenticação Bearer token. — from spec
- Requisitos: arquitetura MVC, pattern de herança, portal com todas funcionalidades, testável via Postman. — from spec
- Repo name: `<git-username>-<date:YYYYMMDD>` → `asghrk-20260824`. — from spec submission instructions
- Minimum 20 well-made commits, one logical unit of work each. — from user request

---

## File Map

```
gestao-colaboradores/
  backend/
    GestaoColaboradores.sln
    src/
      GestaoColaboradores.Domain/         # EntidadeBase, CadastroBase, Usuario, Unidade, Colaborador
      GestaoColaboradores.Infrastructure/  # AppDbContext, EF configs, migrations, Repository<T>, specific repos
      GestaoColaboradores.Application/     # DTOs, IUsuarioService/UnidadeService/ColaboradorService + impls, JwtTokenService
      GestaoColaboradores.Api/             # Controllers (MVC), Program.cs, appsettings.json
    tests/
      GestaoColaboradores.Tests/           # xUnit: business rule tests
  frontend/
    (Angular workspace: src/app/core, src/app/features/{usuarios,unidades,colaboradores}, src/app/auth)
  docker-compose.yml
  postman/
    GestaoColaboradores.postman_collection.json
    GestaoColaboradores.postman_environment.json
  README.md
  .gitignore
```

## Task List (each task = one commit)

1. **chore: scaffold repo** — `.gitignore` (dotnet+node+ide), empty `README.md` stub.
2. **feat(domain): inheritance base classes** — `EntidadeBase` (Id, CriadoEm, AtualizadoEm), `CadastroBase : EntidadeBase` (Codigo). Both abstract.
3. **feat(domain): entities** — `Usuario : CadastroBase` (Login, SenhaHash, Ativo), `Unidade : CadastroBase` (Nome, Ativo, ICollection<Colaborador>), `Colaborador : CadastroBase` (Nome, UnidadeId, Unidade, UsuarioId, Usuario).
4. **feat(infra): AppDbContext** — Npgsql provider, fluent config: unique indexes on `Codigo` per table, required FKs, `Colaborador.UsuarioId` unique (1 usuário : 1 colaborador).
5. **feat(infra): generic repository pattern** — `IRepository<T>`/`Repository<T>` (Add/Update/Delete/GetById/GetAll/AnyAsync) operating on `EntidadeBase`.
6. **feat(infra): specific repositories** — `IUsuarioRepository`/`UsuarioRepository` (+ByLogin), `IUnidadeRepository`/`UnidadeRepository` (+with colaboradores include), `IColaboradorRepository`/`ColaboradorRepository`, all inheriting `Repository<T>`.
7. **chore(infra): initial EF Core migration** — `dotnet ef migrations add InitialCreate`, verify it generates without a live DB connection using the design-time factory.
8. **feat(application): DTOs** — Create/Update/Response DTOs for the 3 entities + `LoginDto`/`TokenResponseDto`.
9. **feat(application): UsuarioService** — create (hash senha, validate código/login unique), update (senha/status only), list (+status filter), get by id. Uses `PasswordHasher<Usuario>`.
10. **feat(application): UnidadeService** — create (código único), inativar (status only), list with colaboradores, get by id.
11. **feat(application): ColaboradorService** — create (validates unidade ativa + usuário exists and not already linked), update (nome/unidade only), delete, list, get by id.
12. **feat(auth): JwtTokenService** — token generation (sub=login, claims), config-bound `JwtOptions` (key/issuer/audience/expiry).
13. **feat(api): BaseApiController + AuthController** — shared `BaseApiController` (problem-details helpers), `POST /api/auth/login` (validates senha, checks Ativo, issues JWT).
14. **feat(api): UsuariosController** — POST/PUT/GET/GET-by-id, `[Authorize]`.
15. **feat(api): UnidadesController** — POST/PUT(inativar)/GET/GET-by-id, `[Authorize]`.
16. **feat(api): ColaboradoresController** — POST/PUT/DELETE/GET/GET-by-id, `[Authorize]`.
17. **feat(api): Program.cs wiring** — DI registrations, JWT Bearer auth, Swagger with Bearer scheme, CORS for Angular dev origin, global exception → ProblemDetails middleware.
18. **chore(docker): docker-compose.yml** — postgres:16 service, named volume, env vars matching `appsettings.Development.json` connection string.
19. **test(backend): xUnit business-rule tests** — colaborador rejected when unidade inativa; usuario update ignores login/codigo changes; login rejected when usuario inativo. In-memory EF provider.
20. **docs: Postman collection + environment** — all endpoints incl. login, `{{baseUrl}}`/`{{token}}` variables, a pre-request/test script on login that sets `{{token}}` from the response.
21. **feat(frontend): Angular workspace scaffold** — `ng new frontend`, routing, base layout shell, environment files (apiUrl).
22. **feat(frontend): auth** — `AuthService` (login, token storage, logout), `authInterceptor` (attach Bearer), `authGuard` (route protection), login page.
23. **feat(frontend): Usuarios feature** — service + list page (status filter) + create/edit form (edit = senha/status only).
24. **feat(frontend): Unidades feature** — service + list page (shows colaboradores) + create form + inactivate action.
25. **feat(frontend): Colaboradores feature** — service + list page + create/edit form (unidade select) + delete action.
26. **style(frontend): shell polish** — nav bar with links + logout, consistent form/table styling, empty/loading states.
27. **docs: finalize README** — architecture explanation (inheritance pattern), how to run (`docker compose up`, `dotnet ef database update`, `dotnet run`, `ng serve`), how to test via Postman, env vars.

## Verification per task

- Backend tasks (2–19): `dotnet build` must succeed after the task's commit; task 19 additionally runs `dotnet test`.
- Frontend tasks (21–26): `ng build` must succeed after the task's commit.
- Final: full `dotnet build` + `dotnet test` + `ng build` clean, then push to GitHub repo `asghrk-20260824`.

---

## Self-Review

**Spec coverage:** cadastro/update/list usuários → 9,14; cadastro/update/delete/list colaboradores → 11,16; cadastro/update(inativação)/list unidades c/ colaboradores → 10,15; unidade inativa bloqueia novo colaborador → 11,19; Docker DB → 18; Bearer auth → 12,13,22; MVC → 14–16; herança → 2,3,5,6; Postman → 20. No gaps found.

**Placeholder scan:** none — every task names concrete files/classes/endpoints.

**Type consistency:** `EntidadeBase`/`CadastroBase` naming, `IRepository<T>`, `IUsuarioService`/`IUnidadeService`/`IColaboradorService`, DTO names reused consistently task 8→9-11→14-16→20-25.
