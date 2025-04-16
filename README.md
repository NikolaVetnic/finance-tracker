# FinanceTracker

FinanceTracker is a personal project aimed at creating a personal expense tracking web app.

## Ports

The component port numbers are as follows:

| Component   | Local Env         | Docker Env        | Docker Inside   |
| ----------- | ----------------- | ----------------- | --------------- |
| ft.api      | `15000` / `15050` | `16000` / `16060` | `8080` / `8081` |
| ft.frontend | `13000`           | `14000`           | `13000`         |
| ft.database |                   | `25432`           | `5432`          |

ASP.NET Core ports are listed as HTTP / HTTPS for running application.

## Naming Policy

Icons are used to indicate the commit type:

- _chore_ : 🧹 `:broom`
- _docs_ : 📖 `:open_book`
- _feat_ : ✨ `:sparkles`
- _fix_ : 🛠 `:hammer_and_wrench`
- _PR in Draft status_ : 📝 `:memo`
- _refactor_ : 🔄 `:counterclockwise_arrows_button`
- _various_ : 🧩 `:puzzle_piece`

The usual phrasing with a verb in imperative mode at the beginning is encouraged.
