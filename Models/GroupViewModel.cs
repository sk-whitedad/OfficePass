using OfficePass.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Models
{
    public class GroupViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название подразделения")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

    }
}
