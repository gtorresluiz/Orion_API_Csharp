using SprintCsharp.Application.Interfaces;
using SprintCsharp.Domain.Entities;

namespace SprintCsharp.UI;

public static class Menu
{
    public static async Task RunAsync(IUserService svc)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== InvestmentApp (Console) ===");
            Console.WriteLine("1) Criar usuário");
            Console.WriteLine("2) Listar usuários");
            Console.WriteLine("3) Depositar");
            Console.WriteLine("4) Sacar");
            Console.WriteLine("5) Exportar JSON");
            Console.WriteLine("6) Importar JSON");
            Console.WriteLine("7) Exportar TXT");
            Console.WriteLine("8) Sair");
            Console.Write("Opção: ");
            var opt = Console.ReadLine();
            try
            {
                switch (opt)
                {
                    case "1": await CreateUser(svc); break;
                    case "2": await ListUsers(svc); break;
                    case "3": await Deposit(svc); break;
                    case "4": await Withdraw(svc); break;
                    case "5": await ExportJson(svc); break;
                    case "6": await ImportJson(svc); break;
                    case "7": await ExportTxt(svc); break;
                    case "8": return;
                    default: Console.WriteLine("Opção inválida"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            Console.WriteLine("Pressione Enter para continuar...");
            Console.ReadLine();
        }
    }

    static async Task CreateUser(IUserService svc)
    {
        Console.Write("Nome: "); var name = Console.ReadLine() ?? "";
        Console.Write("Email: "); var email = Console.ReadLine() ?? "";
        Console.Write("Saldo inicial: "); var bal = decimal.TryParse(Console.ReadLine(), out var v) ? v : 0m;
        Console.WriteLine("Tipos: 0=RendaFixa,1=RendaVariavel,2=TesouroDireto,3=Cripto");
        Console.Write("Tipo (numero): "); var t = int.TryParse(Console.ReadLine(), out var ti) ? ti : 0;
        Console.WriteLine("Níveis: 0=Estagiario,1=Junior,2=Pleno,3=Senior,4=Lead");
        Console.Write("Nível (numero): "); var l = int.TryParse(Console.ReadLine(), out var li) ? li : 0;
        var user = await svc.CreateAsync(name, email, bal, (InvestmentType)t, (ProfessionLevel)l);
        Console.WriteLine($"Criado: Id={user.Id} Nome={user.Name}");
    }

    static async Task ListUsers(IUserService svc)
    {
        var users = await svc.GetAllAsync();
        Console.WriteLine("Id | Nome | Email | Saldo | Investimento | Nível");
        foreach (var u in users) Console.WriteLine($"{u.Id} | {u.Name} | {u.Email} | {u.Balance} | {u.PreferredInvestment} | {u.Level}");
    }

    static async Task Deposit(IUserService svc)
    {
        Console.Write("Id do usuário: "); var id = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Valor: "); var v = decimal.Parse(Console.ReadLine() ?? "0");
        await svc.DepositAsync(id, v);
        Console.WriteLine("Depósito efetuado.");
    }

    static async Task Withdraw(IUserService svc)
    {
        Console.Write("Id do usuário: "); var id = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Valor: "); var v = decimal.Parse(Console.ReadLine() ?? "0");
        await svc.WithdrawAsync(id, v);
        Console.WriteLine("Saque efetuado.");
    }

    static async Task ExportJson(IUserService svc)
    {
        Console.Write("Caminho (ex: users.json): "); var p = Console.ReadLine() ?? "users.json";
        await svc.ExportToJsonAsync(p);
        Console.WriteLine("Exportado.");
    }

    static async Task ImportJson(IUserService svc)
    {
        Console.Write("Caminho do arquivo JSON: "); var p = Console.ReadLine() ?? "users.json";
        await svc.ImportFromJsonAsync(p);
        Console.WriteLine("Importado.");
    }

    static async Task ExportTxt(IUserService svc)
    {
        Console.Write("Caminho (ex: users.txt): "); var p = Console.ReadLine() ?? "users.txt";
        await svc.ExportToTxtAsync(p);
        Console.WriteLine("Exportado.");
    }
}
