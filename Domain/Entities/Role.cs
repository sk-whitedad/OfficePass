using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    [Index(propertyNames: nameof(Name), IsUnique = true)]
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
