using SprintCsharp.Application.Services;
using SprintCsharp.Domain.Entities;

namespace SprintCsharp.UI;

public class ConsoleApp
{
    private readonly UserService _svc;

    public ConsoleApp(UserService svc)
    {
        _svc = svc;
    }

    public async Task RunAsync()
    {
        var running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== InvestmentApp (Console) ===");
            Console.WriteLine("1) Criar usuário");
            Console.WriteLine("2) Listar usuários");
            Console.WriteLine("3) Editar usuário");
            Console.WriteLine("4) Deletar usuário");
            Console.WriteLine("5) Depositar");
            Console.WriteLine("6) Sacar");
            Console.WriteLine("7) Sair");
            Console.Write("Opção: ");

            var opt = Console.ReadLine();
            try
            {
                switch (opt)
                {
                    case "1": await Create(); break;
                    case "2": await List(); break;
                    case "3": await Edit(); break;
                    case "4": await Delete(); break;
                    case "5": await Deposit(); break;
                    case "6": await Withdraw(); break;
                    case "7": running = false; break;
                    default: Console.WriteLine("Opção inválida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine();
                Console.WriteLine("Pressione Enter para continuar...");
                Console.ReadLine();
            }
        }
    }

    private async Task Create()
    {
        Console.Write("Nome: ");
        var name = Console.ReadLine() ?? string.Empty;

        Console.Write("Email: ");
        var email = Console.ReadLine() ?? string.Empty;

        Console.Write("Saldo inicial: ");
        var balance = Console.ReadLine() ?? "0";

        Console.WriteLine("Tipo investimento (0=RendaFixa,1=RendaVariavel,2=TesouroDireto,3=Cripto): ");
        var t = Console.ReadLine();
        var inv = ParseEnum<InvestmentType>(t, InvestmentType.RendaFixa);

        Console.WriteLine("Nível (0=Estagiario,1=Junior,2=Pleno,3=Senior,4=Lead): ");
        var l = Console.ReadLine();
        var lvl = ParseEnum<ProfessionLevel>(l, ProfessionLevel.Estagiario);

        var (success, message, created) = await _svc.CreateAsync(name, email, balance, inv, lvl);
        Console.WriteLine(message);
        if (success && created != null) Console.WriteLine($"Id: {created.Id}");
    }

    private async Task List()
    {
        var users = await _svc.ListAsync();
        if (users.Count == 0)
        {
            Console.WriteLine("Nenhum usuário cadastrado.");
            return;
        }

        Console.WriteLine("Id | Nome | Email | Saldo | Investimento | Nível | UpdatedAt");
        foreach (var u in users)
        {
            Console.WriteLine($"{u.Id} | {u.Name} | {u.Email} | {u.Balance} | {u.PreferredInvestment} | {u.Level} | {u.UpdatedAt?.ToString("s")}");
        }
    }

    private async Task Edit()
    {
        Console.Write("Id do usuário a editar: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

        var user = await _svc.GetAsync(id);
        if (user == null) { Console.WriteLine("Usuário não encontrado."); return; }

        Console.Write($"Nome ({user.Name}): ");
        var name = ReadOrDefault(user.Name);

        Console.Write($"Email ({user.Email}): ");
        var email = ReadOrDefault(user.Email);

        Console.Write($"Saldo ({user.Balance}): ");
        var balance = ReadOrDefault(user.Balance.ToString());

        Console.WriteLine($"Tipo investimento ({(int)user.PreferredInvestment}): ");
        var t = Console.ReadLine();
        var inv = ParseEnum<InvestmentType>(t, user.PreferredInvestment);

        Console.WriteLine($"Nível ({(int)user.Level}): ");
        var l = Console.ReadLine();
        var lvl = ParseEnum<ProfessionLevel>(l, user.Level);

        var (success, message) = await _svc.UpdateAsync(id, name, email, balance, inv, lvl);
        Console.WriteLine(message);
    }

    private async Task Delete()
    {
        Console.Write("Id do usuário a deletar: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

        var (success, message) = await _svc.DeleteAsync(id);
        Console.WriteLine(message);
    }

    private async Task Deposit()
    {
        Console.Write("Id do usuário: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

        Console.Write("Valor a depositar: ");
        var amount = Console.ReadLine() ?? string.Empty;

        var (success, message) = await _svc.DepositAsync(id, amount);
        Console.WriteLine(message);
    }

    private async Task Withdraw()
    {
        Console.Write("Id do usuário: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

        Console.Write("Valor a sacar: ");
        var amount = Console.ReadLine() ?? string.Empty;

        var (success, message) = await _svc.WithdrawAsync(id, amount);
        Console.WriteLine(message);
    }

    private static string ReadOrDefault(string defaultValue)
    {
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return defaultValue;
        return input.Trim();
    }

    private static T ParseEnum<T>(string? input, T @default) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(input)) return @default;
        if (int.TryParse(input, out var num) && Enum.IsDefined(typeof(T), num))
        {
            return (T)Enum.ToObject(typeof(T), num);
        }

        // tenta parse por nome (ex: "RendaFixa")
        if (Enum.TryParse<T>(input, true, out var parsed)) return parsed;
        return @default;
    }
}
