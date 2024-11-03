using System.ComponentModel.DataAnnotations;

namespace OfficePass.Areas.Adminka.Models
{
    public class SpecializationViewModel
    {   
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните поле Должность")]
        [Display(Name = "Должность")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
