using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    public class Guest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string SurName { get; set; }

        [Display(Name = "Документ")]
        public string Document { get; set; }

        public int Company_Id {  get; set; }

        [Display(Name = "Название")]
        public Company? Company { get; set; }
    }
}
