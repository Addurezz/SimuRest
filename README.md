# 🧪 SimuRest – A Lightweight REST Mock Server for Developers

SimuRest is a **lightweight, extensible HTTP mock server** for developers who need to simulate REST APIs.  
It’s designed as a **learning project** and **testing tool**, focusing on a clean architecture and a fluent builder API.

Unlike heavy frameworks, SimuRest doesn’t bring in ASP.NET Core.  
It’s just you, `HttpListener`, and a structured service layer.

---

## ✨ Key Features
- ⚡ **Route configuration with a fluent builder API**
- 📝 **Static and dynamic responses** (via `Func<SimuRequest, SimuResponse>`)
- ⏱️ **Artificial delays** to test client resilience
- 💾 **In-memory persistence** to store/retrieve responses
- 🛠️ **Async request handling** with `HttpListener`
- 🧩 **Separation of concerns**: HTTP, Routing, Persistence, Builder
- 🧪 **Designed for experimentation** (cloud, REST, frontend integration)

---

## 🚀 Quick Start

```csharp
using SimuRest.Core.Services.Builders;
using System.Net.Http;

// Build a mock server
var server = new SimuServerBuilder()
    .Port(5000)

    // GET /hello -> "Hello, World!"
    .Setup(HttpMethod.Get, "/hello")
        .Responds(req => new SimuResponse(200, "Hello, World!"))
        .Apply()

    // POST /echo -> echoes request body
    .Setup(HttpMethod.Post, "/echo")
        .Responds(req => new SimuResponse(200, req.Body))
        .Apply()

    // GET /slow -> delayed response
    .Setup(HttpMethod.Get, "/slow")
        .Delay(3000)
        .Responds(req => new SimuResponse(200, "This was slow..."))
        .Apply()

    .Server;

// Run the server
await server.Start();
