#pragma warning disable CS8616
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalypsoCasino.Models;

public class Player
{
    [Key]
    public int PlayerId { get; set; }

    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long!")]
    [MaxLength(50, ErrorMessage = "Username cannot surpass 50 characters! ")]
    public string? Username { get; set; }

    public int? Bank { get; set; }
    public int? HighestBank { get; set; }

    [Range(10, int.MaxValue, ErrorMessage = "You have to bet at least $10!")]
    [IncrementsOfTen]
    public int? Bet { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}

public class IncrementsOfTenAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("You have to bet something!");
        }

        if (value is not int betValue)
        {
            return new ValidationResult("Invalid bet type.");
        }

        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));

        // Check if the bet is an increment of 10 (10, 20, 30... etc.)
        if (betValue % 10 != 0)
        {
            return new ValidationResult("Your bet must be an increment of 10!");
        }

        return ValidationResult.Success;
    }
}

