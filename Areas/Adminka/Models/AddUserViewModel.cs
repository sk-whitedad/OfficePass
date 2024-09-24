using Microsoft.AspNetCore.Mvc.Rendering;
using OfficePass.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Areas.Adminka.Models
{
	public class AddUserViewModel
	{
		public int Id {  get; set; }

		[Required]
		[Display(Name = "Логин")]
		public string Login { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; } = "123";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Повторите пароль")]
        public string ConfirmPassword { get; set; } = "123";


        [Required]
		[Display(Name ="Роль")]
		public string Role { get; set; }

        [Display(Name = "Профиль")]
		public UserProfile UserProfile { get; set; }
    }
}
