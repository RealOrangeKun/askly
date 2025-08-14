# Askly

Askly is a modern, full-stack question-answering platform built with .NET 9, PostgreSQL (with pgvector), Redis, and a modular frontend. It features advanced caching, vector search, and a clean CQRS architecture.

---

## Features

- **.NET 9 API** with Carter and MediatR (CQRS)
- **PostgreSQL** with [pgvector](https://github.com/pgvector/pgvector) for semantic search
- **HybridCache**: L1 (in-memory) + L2 (Redis) caching for blazing-fast responses
- **OpenAPI/Swagger** with API key authentication
- **FluentValidation** for robust input validation
- **Docker Compose** for local development
- **Frontend**: Modular HTML/CSS/JS (with Tailwind)

---

## Project Structure

```
askly/
├── backend/
│   ├── docker-compose.yml
│   ├── Dockerfile
│   ├── src/
│   │   └── API/
│   │       └── Askly.Api/           # .NET 9 Web API
│   └── postgres_data/               # Local Postgres volume
├── frontend/
│   ├── html/                        # Static HTML pages
│   ├── css/                         # Styles (Tailwind)
│   └── js/                          # JS modules
└── README.md
```

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)

### 1. Clone the repository
```sh
git clone https://github.com/RealOrangeKun/askly.git
cd askly
```

### 2. Start the stack
```sh
cd backend
# Build and run all services (API, Postgres, Redis)
docker-compose up --build
```

- API: [http://localhost:5000](http://localhost:5000)
- Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- Postgres: `localhost:5432` (user: postgres, pass: postgres)
- Redis: `localhost:6379`

### 3. Frontend
Open any HTML file in `frontend/html/` directly in your browser.

---

## API Overview

- **/api/questions/ask**: Find similar answered questions (vector search)
- **/api/questions**: Create new questions
- **/api/questions/{id}**: Answer a question

All endpoints require an API key header: `x-api-key: <your-key>` (see `appsettings.Development.json`)

---

## Caching Architecture

- **HybridCache**: Combines in-memory (L1) and Redis (L2) for fast, distributed caching
- **OutputCache**: HTTP response caching for repeated identical requests
- **Cache keys**: Normalized and hashed for semantic deduplication

---

## Development

- Edit backend code in `backend/src/API/Askly.Api/`
- Edit frontend in `frontend/`
- Use `docker-compose down -v` to reset all data

---

## License

MIT

---

## Credits
- [pgvector](https://github.com/pgvector/pgvector)
- [Carter](https://github.com/CarterCommunity/Carter)
- [HybridCache](https://github.com/TurnerSoftware/Microsoft.Extensions.Caching.Hybrid)
- [FluentValidation](https://fluentvalidation.net/)
- [StackExchange.Redis](https://stackexchange.github.io/StackExchange.Redis/)
