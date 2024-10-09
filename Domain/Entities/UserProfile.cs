using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    public class UserProfile
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Firstname { get; set; }

        [Display(Name = "Фамилия")]
        public string? Lastname { get; set; }

        [Display(Name = "Отчество")]
        public string? Surname { get; set; }

        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

        public int GroupId { get; set; }

        [Display(Name = "Подразделение")]
        public Group? Group { get; set; }

        [Required]
        public int SpecializationId { get; set; }

        [Display(Name = "Должность")]
        public Specialization? Specialization { get; set; }

        [Display(Name = "Сотрудник")]
        public User? User { get; set; }

        [Required]
        public bool IsBoss {  get; set; }

        //private string fullname;
        //public string FullName 
        //{
        //    set
        //    {
        //        fullname = $"{Lastname} {Firstname} {Surname}"; 
        //    }
        //    get { return fullname; }
        //}
    }
}
