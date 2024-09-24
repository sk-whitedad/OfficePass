using System.ComponentModel.DataAnnotations;

namespace OfficePass.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Укажите имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
