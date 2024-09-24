using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    public partial class Company
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }
    }
}
