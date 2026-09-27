# BenzForum

A discussion forum with real-time comments. ASP.NET Core 8 Web API plus an
Angular 18 client, in one repository.

```
BenzForumBackend/BenzForum/   .NET 8 API
  Controllers/  Services/  Repositories/  Data/  Helpers/  Hubs/  Migrations/
BenzForumFront/               Angular 18 client
```

Backend layering is Controller → Service → Repository. Controllers stay thin:
they validate input, call one service, and map the result. Business rules live
in services. Data access goes through `IRepository<T>`; controllers never touch
`ForumContext` directly.

## Commands

```bash
# backend
cd BenzForumBackend/BenzForum
dotnet restore && dotnet build
dotnet ef database update
dotnet run                       # https://localhost:7xxx, Swagger in Development

# tests
dotnet test

# frontend
cd BenzForumFront
npm ci && npm start              # http://localhost:4200

# everything
docker compose up --build
```

## Configuration

No secrets in `appsettings.json`. The JWT signing key and the connection string
come from user secrets locally (`dotnet user-secrets set "Jwt:Key" "..."`) and
from environment variables in containers. `appsettings.Example.json` is
committed with empty values and documents every key the app reads. If you find
a real-looking value in a committed config file, treat it as a bug and say so.

## Conventions

- **Namespaces**: everything under `BenzForum.*`. The codebase currently mixes
  `BenzForum` and `ForumApp` — when you touch a file, move it to `BenzForum.*`
  and fix its usings. Do not do a repo-wide rename in one commit.
- **Async**: every I/O path is `async`/`await` end to end. No `.Result`, no
  `.Wait()`.
- **Logging**: `ILogger<T>` with named placeholders — `_logger.LogWarning("User
  already exists: {Username}", name)`. Never string interpolation.
- **Errors**: services throw domain exceptions; controllers translate them to
  status codes. Do not return `null` to mean "not found" from a new service.
- **Passwords**: BCrypt only. Nothing else, ever.
- **Tests**: xUnit, one test class per service, `MethodName_Condition_Expected`
  naming. Test behaviour through the service, with a faked repository — do not
  write tests that only assert a mock was called.
- **Decisions**: anything a future reader would ask "why?" about goes in
  `docs/adr/NNNN-short-title.md` (context / decision / consequences). I write
  these; you may review them.

## Current work

Upgrading an existing, working project. Do not rewrite it. The code quality is
already reasonable — what is missing is everything around it.

1. Get backend and frontend running locally again; fix whatever has rotted.
2. Align namespaces as files are touched.
3. Move the JWT key out of `appsettings.json`; add `appsettings.Example.json`.
4. Add a test project. 15–20 real tests across `UserService`, `PostService`,
   `CommentService`. Cover the failure paths: duplicate registration, wrong
   password, comment on a missing post.
5. Multi-stage `Dockerfile` for the API, plus `docker-compose.yml` bringing up
   API, SQL Server and the client together.
6. GitHub Actions: restore → build → test → docker build, badge in the README.
7. Rewrite the README: what it is, a Mermaid architecture diagram, how to run
   it, and the reasoning behind the main choices.
8. Three ADRs — generic repository vs. per-entity repositories, SignalR vs.
   polling for comments, JWT vs. server-side sessions.
9. MIT licence.

## Definition of done

A stranger clones the repository, runs `docker compose up`, and has it working
in under five minutes. The README explains what was built and why. CI is green.
Tests fail if someone breaks authentication.
