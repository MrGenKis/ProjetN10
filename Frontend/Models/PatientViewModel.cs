using System.ComponentModel.DataAnnotations;

namespace Frontend.Models;

public class PatientViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Prénom")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Nom")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Date de naissance")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Display(Name = "Genre")]
    public string Gender { get; set; } = string.Empty;

    [Display(Name = "Adresse")]
    public string? Address { get; set; }

    [Display(Name = "Téléphone")]
    public string? PhoneNumber { get; set; }
}