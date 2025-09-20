using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
    public InvestmentType PreferredInvestment { get; set; }
    public ProfessionLevel Level { get; set; }
    public DateTime CreatedAt { get; set; }

    [NotMapped]
    public DateTime? UpdatedAt { get; set; }

    public static bool IsValidEmail(string email);
}