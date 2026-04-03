# IPB2 Online Book Store System - Clean Feature-Based Setup

This repository now includes the architecture you asked for:

1. **Domain Layer** (`IPB2.OnlineBookStoreSystem.Domain`)
   - Entities
   - Feature Request/Response contracts
   - Interfaces
   - Async domain service (`BookStoreDomainService`)

2. **Minimal API** (`IPB2.OnlineBookStoreSystem.MinimalApi`)
   - Feature-based endpoint mapping (`Features/*`)
   - Async/await data access using `Microsoft.Data.SqlClient`
   - Uses domain service + repository pattern

3. **MVC App** (`IPB2.OnlineBookStoreSystem.Mvc`)
   - Feature-based folders (`Features/*`)
   - Request/Response models per feature
   - Uses `HttpClient` (`BookStoreApiClient`) to call Minimal API

4. Existing **WinForms** project is kept for reference.

## Run Order

- Start Minimal API first.
- Then run MVC app (configure `Api:BaseUrl` in MVC `appsettings.json`).
- Update connection string in Minimal API `appsettings.json`.


## Git Upload (if push fails)

If your project **cannot upload to GitHub**, use this exact flow:

```bash
git status
git add .
git commit -m "online bookstore architecture update"

git branch -M main
git remote add origin https://github.com/<your-username>/<your-repo>.git
# if origin already exists: git remote set-url origin https://github.com/<your-username>/<your-repo>.git

git push -u origin main
```

If GitHub rejects username/password, use a **Personal Access Token (PAT)** as password.

Also make sure `bin/`, `obj/`, `.vs/` are ignored (this repository now includes `.gitignore`).


## PR Conflict Fix (for `IPB2.Consoleapp.sln`)

If GitHub shows a conflict in `IPB2.Consoleapp.sln`, do this in your local branch:

```bash
git fetch origin
git checkout <your-branch>
git merge origin/master
```

Then keep:
- `IPB2.Consoleapp.sln` for Console + WinForms only
- `IPB2.OnlineBookStoreSystem.sln` for Domain + MinimalApi + MVC

After that:

```bash
git add IPB2.Consoleapp.sln IPB2.OnlineBookStoreSystem.sln
git commit -m "resolve sln conflict by splitting solutions"
git push
```


## Merge Conflict Prevention

This repository includes `.gitattributes` rules (`merge=union`) for `*.sln` and `README.md` to reduce repeated local merge conflicts.
