using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Areas.Adminka.Models
{
    public class AddUserProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Firstname { get; set; }

        [Display(Name = "Фамилия")]
        public string? Lastname { get; set; }

        [Display(Name = "Отчество")]
        public string? Surname { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }
 
        [Display(Name = "Подразделение")]
        public int GroupId { get; set; }

        [Display(Name = "Должность")]
        public int SpecializationId { get; set; }

        [Display(Name = "Руководитель")]
        public bool IsBoss { get; set; }
    }
}
