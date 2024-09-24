using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    public partial class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string? Login { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Display(Name = "Уровень")]
        public Role Role { get; set; }

        [Display(Name = "Профиль")]
        public UserProfile? UserProfile { get; set; }
    }
}
