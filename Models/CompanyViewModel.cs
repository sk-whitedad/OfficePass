using System.ComponentModel.DataAnnotations;

namespace OfficePass.Models
{
    public class CompanyViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название компании")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите адрес")]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

    }
}
