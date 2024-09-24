using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    public class UserProfile
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string? Firstname { get; set; }

        [Display(Name = "Фамилия")]
        public string? Lastname { get; set; }

        [Display(Name = "Отчество")]
        public string? Surname { get; set; }

        [Display(Name = "Должность")]
        public string? Specialization { get; set; }

        [Display(Name = "Отдел/Цех")]
        public string? Group {  get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
