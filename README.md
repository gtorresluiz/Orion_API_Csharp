# 📈 Plataforma de Investimentos - CRUD Simples (C# + Oracle)

Este projeto é uma aplicação **ASP.NET Core MVC** conectada a um **banco de dados Oracle**.  
O objetivo é simular uma **plataforma digital de investimentos** com um tom jovem e gamificado, permitindo o gerenciamento de usuários com saldo, nível de profissão e tipo de investimento.

---

## Integrantes 

Eduardo Fedeli Souza (RM550132)
Gabriel Torres Luiz (RM98600)
Murilo Henrique Obinata (RM99855)
Otávio Vitoriano Da Silva (RM552012)

---

## 🚀 Tecnologias Utilizadas

- **C# 12**  
- **.NET 8**  
- **ASP.NET Core MVC**  
- **Entity Framework Core**  
- **Oracle Database** (`Oracle.EntityFrameworkCore`)  

---

## 📂 Estrutura do Projeto

A solução foi organizada em **camadas** dentro de um único projeto para manter a arquitetura limpa:

  ### 📂 Application # Serviços e lógica de negócios
  ### 📂 Domain # Modelos e entidades
  ### 📂 Infra # Repositórios e acesso a dados
  ### 📂 Migrations # Migrações do EF Core (se aplicável)
  ### 📂 Views # Páginas MVC (CRUD)
  ### Program.cs # Configuração principal da aplicação
  ### AppDbContext.cs # Configuração do banco de dados

---

## ⚙️ Configuração da Conexão com o Oracle

A conexão com o banco **não usa `appsettings.json`**, sendo configurada diretamente no `Program.cs`:

  csharp
  builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseOracle("User Id=RMXXXXX;Password=XXXXXX;Data Source=oracle.fiap.com.br:1521/ORCL;")
  );

## Funcionalidades Implementadas
  
  ✅ Cadastro de usuários
  ✅ Listagem de usuários
  ✅ Edição de usuários (com opções de Nível de Profissão e Tipo de Investimento)
  ✅ Exclusão de usuários
  ✅ Validações de e-mail e saldo

## Exemplo de Uso

  Cadastrar usuário preenchendo nome, e-mail válido, saldo e opções de profissão/investimento.
  
  Editar usuário para atualizar saldo ou trocar tipo de investimento.
  
  Excluir usuário da plataforma.
  
  Listar todos os usuários cadastrados.

## 👨‍🏫 Projeto Acadêmico

  Este projeto foi desenvolvido como parte da graduação na FIAP, integrando conhecimentos de:
  
  Arquitetura de software (Domain, Application, Infra)
  
  ASP.NET Core MVC
  
  Banco de dados Oracle
  
  Boas práticas de programação

  Desenvolvimento em C#
