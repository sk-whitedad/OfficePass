using Microsoft.AspNetCore.Mvc.Rendering;
using OfficePass.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Areas.Adminka.Models
{
	public class EditUserViewModel
    {
		public int Id {  get; set; }

        public int UserProfileId { get; set; }

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

        [Required(ErrorMessage = "Поле Имя не должно быть пустым")]
        [Display(Name = "Имя")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Поле Фамилия не должно быть пустым")]
        [Display(Name = "Фамилия")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Поле Отчество не должно быть пустым")]
        [Display(Name = "Отчество")]
        public string? Surname { get; set; }

        [Display(Name = "Профиль")]
        public int RoleId {  get; set; }

        [Display(Name = "Подразделение")]
		public int GroupId { get; set; }

        [Display(Name = "Должность")]
        public int SpecializationId { get; set; }

        [Display(Name = "Статус руководителя")]
        public bool IsBoss { get; set; }
    }
}
