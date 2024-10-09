using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Areas.Adminka.Models
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "Логин")]
        public string? Login { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }
 
        [Display(Name = "Подразделение")]
        public string Group { get; set; }

        [Display(Name = "Должность")]
        public string Specialization { get; set; }

        [Display(Name = "Руководитель")]
        public bool IsBoss { get; set; }
    }
}
