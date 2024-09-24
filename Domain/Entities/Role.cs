using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    public partial class Role
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Уровень")]
        public string Name { get; set; } = null!;

        public List<User> User { get; set; } = new List<User>();
    }
}
