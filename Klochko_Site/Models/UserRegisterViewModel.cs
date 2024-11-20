using System.ComponentModel.DataAnnotations;
namespace Klochko_Site.Models;

public class UserRegisterViewModel
{
    [Required]
    [Display(Name = "Имя пользователя")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Required]
    [Display(Name = "ФИО")]
    public string FullName { get; set; }

    [Required]
    [Display(Name = "Телефон")]
    public string Phone { get; set; }

    [EmailAddress]
    [Display(Name = "Электронная почта")]
    public string? Email { get; set; }

    [Display(Name = "Адрес")]
    public string? Address { get; set; }
}
