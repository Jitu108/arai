# 🧠 ARAI – Autonomous Risk Adjustment Intelligence Platform

**ARAI** (Autonomous Risk Adjustment Intelligence) is a modular, AI-augmented healthcare analytics platform that automates **risk adjustment**, **HCC mapping**, and **gap detection** using a **microservice architecture**. It brings together **.NET 8**, **SQL Server**, **Python**, and **Docker** to deliver scalable, data-driven insights for value-based care.

---

## ⚙️ Core Highlights

* 🧩 **Domain-Driven Architecture:** Clean separation of Domain, Application, and Infrastructure layers across microservices.
* ⚡ **Microservice Isolation:** Each service (Patient, HCC Mapping, Gap Engine) runs independently and communicates via HTTP APIs.
* 🧠 **AI-Enabled Agents:** Python-based agent orchestrator for ETL, document embedding, and LLM-based recommendations.
* 🐳 **Containerized Deployment:** Fully Dockerized stack with API Gateway and SQL Server.
* 🔍 **Extensible & Scalable:** Modular building blocks make it easy to add new domains and microservices.

---

## 🧩 Folder Structure (Simplified)

```
arai/
├─ .env.example                 # Environment variable template
├─ docker-compose.yml           # Multi-container orchestration
├─ database/
│  └─ seed.sql                  # Seed data (patients, conditions, mappings)
└─ src/
   ├─ BuildingBlocks/           # Reusable shared kernel across services
   │  ├─ Domain/                # Core entities, value objects, events
   │  ├─ Application/           # MediatR behaviors, interfaces
   │  └─ Infrastructure/        # Time & system abstractions
   │
   ├─ Services/
   │  ├─ PatientSvc/            # Patient domain and CRUD operations
   │  ├─ HccMappingSvc/         # ICD → HCC mapping and versioning
   │  └─ GapEngineSvc/          # Clinical gap detection rules and queries
   │
   ├─ Gateways/OcelotGateway/   # API gateway routing layer
   ├─ Agents/AgentOrchestrator/ # Python orchestrator for AI and RAG workflows
   └─ Web/ClientApp/            # Lightweight web interface
```

---

## 🏗️ Architecture Overview
```
graph TD
    UI[Web Client / API Gateway] -->|HTTP| P1[PatientSvc]
    UI -->|HTTP| P2[HccMappingSvc]
    UI -->|HTTP| P3[GapEngineSvc]
    P1 --> DB[(SQL Server)]
    P2 --> DB
    P3 --> DB
    P1 --> AG[Agent Orchestrator (Python)]
    AG --> VDB[(FAISS / Qdrant)]
    AG --> OCR[OCR / Document Parser]
```

---

## 🚀 Quick Start

```bash
# 1️⃣ Clone repo
git clone https://github.com/<your-username>/arai.git && cd arai

# 2️⃣ Launch containers
docker-compose up -d --build

# 3️⃣ Initialize SQL Server
sqlcmd -S localhost -U sa -P Strong@Pass123 -i database/seed.sql

# 4️⃣ Access Services
PatientSvc API → http://localhost:5001/swagger  
HccMappingSvc API → http://localhost:5002/swagger  
GapEngineSvc API → http://localhost:5003/swagger  
Gateway → http://localhost:5000  

# 5️⃣ Stop containers
docker-compose down
```

---

## 🧠 AI & Agent Layer

* **Agent Orchestrator:** Python microservice managing AI-based workflows.
* **Embeddings & RAG:** Text and code embeddings for context retrieval.
* **OCR Support:** Extracts structured data from scanned PDFs.
* **Vector Indexing:** Uses FAISS/Qdrant for efficient document retrieval.
* **Guardrails:** Ensures HIPAA-compliant handling of PHI/PII.

---

## 🧩 Building Blocks

| Layer              | Purpose                                               |
| ------------------ | ----------------------------------------------------- |
| **Domain**         | Entities, Value Objects, and Domain Events            |
| **Application**    | Command and Query Handlers, DTOs, and Behaviors       |
| **Infrastructure** | Persistence, Repositories, and EF Core Configurations |

These foundational components allow each microservice to remain **autonomous**, **consistent**, and **testable**.

---

## 🧪 Example Use Cases

* Create or update a patient profile via `PatientSvc` API.
* Map ICD-10 codes to HCCs using `HccMappingSvc`.
* Detect documentation gaps for RAF optimization using `GapEngineSvc`.
* Orchestrate document analysis and coding suggestions using the Python agent.

---

## 📦 Deployment Notes

* **All services** can run independently or via `docker-compose`.
* **API Gateway (Ocelot)** routes traffic to services.
* **SQL Server container** holds unified data schema.
* **Environment variables** can be customized via `.env` file.

---

## 🔮 Future Enhancements

* Event-driven messaging (RabbitMQ/Kafka)
* Centralized logging and monitoring (Grafana/Prometheus)
* AuthN/AuthZ via OAuth2 and Keycloak
* AI-based risk scoring model integration
* Dashboard analytics for patient cohorts

---

## 👤 Author

**Jitendra Gupta** – Principal Architect & Developer
Connect on [LinkedIn](https://www.linkedin.com/in/jitendra-gupta/) for collaboration or technical discussions.
