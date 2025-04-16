# FinanceTracker. Backend

Backend repo of the FinanceTracker project.

## Application Settings

The following configuration values have to be set prior to running the app:

```json
{
  "AppSettings": {
    "Token": "", // 64 characters or longer
    "Issuer": "",
    "Audience": ""
  },
  "ConnectionStrings": {
    "DefaultConnection": ""
  }
}
```

## Ports

The component port numbers are as follows:

| Component | Local Env         | Docker Env        | Docker Inside   |
| --------- | ----------------- | ----------------- | --------------- |
| core.api  | `15000` / `15050` | `16000` / `16060` | `8080` / `8081` |
| core.db   |                   | `25432`           | `5432`          |

ASP.NET Core ports are listed as HTTP / HTTPS for running application.

## Architecture Notes

The basic flow is as follows:

- Instead of specific Request/Response classes, the (more) general Model (or DTO) data classes are used. Reuse of existing Models is encouraged.
- The request is sent and received by the controller where it is deserialized as an instance of a Model class.
- The endpoint method is only allowed to perform _transformations necessary for the data to be fed into the Command/Query instance_ (see `Api.Controllers.UpdateUser` for reference).
- Most of the operations are performed in the appropriate Handlers. Commands/Queries and their corresponding Results are defined next to their Handlers.
- Specific services and repositories required by a Handler are accessed via corresponding factories and saved as fields in the Handler class (see `Application.Users.Commands.LoginUser` for reference).
- Services only contain methods used by more than one Handler.
- Database contexts can only be accessed by Repositories.
- On returning the value in the controller method the Result class is "unpacked".

## Migrations

Migrations are to be added using the dotnet ef migrations tool while positioned in the Infrastructure layer of the project:

```shell
    dotnet ef migrations add Init -o ./Migrations --startup-project ../Api/Api.csproj
```
