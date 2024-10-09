using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    [Index(propertyNames: nameof(Name), IsUnique = true)]
    public class Group
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Цех/Отдел")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Профиль")]
        public List<UserProfile>? UserProfile { get; set; }
    }
}
