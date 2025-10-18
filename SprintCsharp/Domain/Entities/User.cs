using System.Text.RegularExpressions;

namespace SprintCsharp.Domain.Entities;

public enum InvestmentType { RendaFixa = 0, RendaVariavel = 1, TesouroDireto = 2, Cripto = 3 }
public enum ProfessionLevel { Estagiario = 0, Junior = 1, Pleno = 2, Senior = 3, Lead = 4 }

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0m;
    public InvestmentType PreferredInvestment { get; set; } = InvestmentType.RendaFixa;
    public ProfessionLevel Level { get; set; } = ProfessionLevel.Estagiario;
    public DateTime? UpdatedAt { get; set; }

    public static bool IsValidEmail(string email)
        => Regex.IsMatch(email ?? string.Empty, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
}