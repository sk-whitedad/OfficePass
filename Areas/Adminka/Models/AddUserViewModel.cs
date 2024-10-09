using Microsoft.AspNetCore.Mvc.Rendering;
using OfficePass.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Areas.Adminka.Models
{
	public class AddUserViewModel
	{
        [Required (ErrorMessage ="Выберите сотрудника")]
        [Display(Name = "Пользователь")]
        public int UserProfileId { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Выберите права доступа")]
        [Display(Name = "Права")]
        public int RoleId { get; set; }

    }
}
