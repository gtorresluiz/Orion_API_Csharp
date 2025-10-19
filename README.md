# 💸 Orión API — CRUD (C# + Oracle)

A **Orión API** é uma aplicação desenvolvida em **C# (.NET 8)** com **ASP.NET Core Web API**, conectada a um banco de dados **Oracle**.  
O sistema simula uma **plataforma digital de investimentos** com um visual jovem e gamificado, permitindo **gerenciamento completo de usuários** com campos como **saldo, nível de profissão** e **tipo de investimento**.

---

## 👥 Integrantes

- **Eduardo Fedeli Souza** — RM550132  
- **Gabriel Torres Luiz** — RM98600  
- **Otávio Vitoriano Da Silva** — RM552012  

---

## 🚀 Tecnologias Utilizadas

- **C# 12**  
- **.NET 8 (ASP.NET Core Web API)**  
- **Entity Framework Core 8**  
- **Oracle Database** (via `Oracle.EntityFrameworkCore`)  
- **Swagger (Swashbuckle)** — com **tema personalizado** em tons de roxo:  
  `#F6EEFD`, `#E4C9F9`, `#CE95F3`, `#BB59EC`, `#952FC1`, `#641D83`, `#370C4A`

---

## ⚙️ Estrutura do Projeto

O projeto **mantém uma arquitetura em camadas**, organizada da seguinte forma:

📦 SprintCsharp
┣ 📂 Application → Serviços e lógica de negócio (UserService)
┣ 📂 Domain → Modelos e entidades (User)
┣ 📂 Infra → Repositórios e contexto de banco (AppDbContext)
┣ 📂 Controllers → Endpoints da API (UserController)
┣ 📂 wwwroot/swagger-ui → Arquivos estáticos e CSS personalizado do Swagger
┣ 📜 Program.cs → Configuração principal da aplicação e injeções
┗ 📜 README.md → Documentação do projeto


---

## 🔌 Conexão com o Banco de Dados Oracle

A conexão é feita diretamente no **Program.cs**, sem `appsettings.json`, conforme o exemplo abaixo:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle("User Id=RMXXXXX;Password=XXXXXX;Data Source=oracle.fiap.com.br:1521/ORCL;")
);

```

## 🧱 Funcionalidades da API

A API possui endpoints REST que permitem manipular usuários no banco Oracle.

Método	  Endpoint	          Descrição
GET	      /api/users	        Retorna todos os usuários cadastrados
GET	      /api/users/{id}	    Retorna um usuário específico
POST	    /api/users	        Cadastra um novo usuário
PUT	      /api/users/{id}	    Atualiza os dados de um usuário existente
DELETE	  /api/users/{id}	    Exclui um usuário do banco de dados

## 🧩 Exemplo de Objeto JSON

POST /api/users
{
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "password": "123456",
  "balance": 1500.00,
  "preferredInvestment": "Renda Fixa",
  "level": "Pleno"
}

PUT /api/users/1
{
  "name": "João Silva",
  "email": "joao.silva@empresa.com",
  "password": "123456",
  "balance": 2000.00,
  "preferredInvestment": "Ações",
  "level": "Sênior"
}

## 🎨 Swagger Customizado

O Swagger UI foi personalizado para refletir a identidade visual da marca Orión, com tons de roxo e elementos modernos.

📁 O arquivo de estilo se encontra em:
wwwroot/swagger-ui/custom.css

🎯 Acesse o Swagger pelo navegador:
http://localhost:5000

## ⚡ Como Executar o Projeto

Clonar o repositório
git clone https://github.com/seuusuario/SprintCsharp.git

Abrir no Visual Studio / VS Code

Restaurar os pacotes NuGet
dotnet restore

Executar a aplicação
dotnet run

Acessar o Swagger
http://localhost:5000

## 🏫 Projeto Acadêmico

Desenvolvido como parte da graduação em Engenharia de Software na FIAP, integrando os conceitos de:

Desenvolvimento com .NET 8 e C#
Boas práticas de arquitetura (Domain, Application, Infra)
Entity Framework Core com Oracle
Criação e consumo de APIs REST
Documentação via Swagger e personalização visual
