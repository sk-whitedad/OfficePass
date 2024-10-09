using System.ComponentModel.DataAnnotations;

namespace OfficePass.Models
{
    public class SpecializationViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Должность")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
