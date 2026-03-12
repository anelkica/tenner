# 🎞️ tenner
![ASP.NET Core 10](https://img.shields.io/badge/ASP.NET%2010-%23512bd4.svg?style=flat-square&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)

Self-hosted GIF browser reviving the discontinued Tenor API, made in ASP.NET Core with Docker and Redis.

Clients are planned to be available for:
- Native Windows and Linux (Avalonia C#)
- Web (React)


## 🔍 Considerations
[Unfortunately, Tenor API has discontinued its service.](https://support.google.com/tenor/answer/10455265#whatll-happen-to-the-tenor-api&zippy=%2Cwhatll-happen-to-the-tenor-api) However, the website may or may not continue working, which means it's possible to continue using the API to a degree based off a public API key given to web clients.

There are no guarantees, as most platforms are moving over to Giphy or alternatives, as such this project serves as a passion project for fun.

## ✨ Features
- GIF searching
- Search by trending

## 🌱 Tech stack
- ASP.NET Core 10
- Docker
- Redis for caching
- Unit tests via xUnit + Moq
- Docs via Scalar (OpenAPI)

## 💻 Downloads
![Under Construction](https://img.shields.io/badge/UNDER%20CONSTRUCTION-FFD700?style=flat-square&logo=construction&logoColor=000&labelColor=000)

Planned: 
- Native Windows and Linux (Avalonia C#)
- Web (React)

## 🗝️ Obtaining an API key
1. Open [tenor.com](https://tenor.com) in the browser
2. Open DevTools → Network tab
3. Search for any GIF
4. Filter requests by `tenor.googleapis.com/v2`
5. Copy the `key=` value from any request URL

## 🐋 Setup via Docker
### Prerequisites
- Docker

### Clone the repository
```bash
git clone https://github.com/anelkica/tenner
cd tenner
```

### Configure environment
- Copy `.env.example` to `.env`
- Add public API key to `TENOR_API_KEY=` in `.env`

### Build and run the container
```bash
docker-compose up --build
```

### Open Scalar API docs
Exposed to `http://localhost:3333/scalar` by default.

## 🛠️ Development
### Prerequisites
- Docker
- .NET 10 SDK

### Clone the repository
```bash
git clone https://github.com/anelkica/tenner
cd tenner
```

### Configure environment
- Copy `.env.example` to `.env`
- Add public API key to `TENOR_API_KEY=` in `.env`

### Start Redis container
```bash
docker run -d -p 6379:6379 redis:alpine
```

### Add API key to secrets
```bash
cd Tenner.API
dotnet user-secrets set "TenorConfig:ApiKey" "YOUR_EXTRACTED_KEY"
```

### Run locally
```bash
cd Tenner.API
dotnet watch run
```

### Unit tests
```bash
dotnet test
```

### Open Scalar API docs
Exposed to `http://localhost:3333/scalar` by default.