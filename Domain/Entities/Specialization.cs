using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    [Index(propertyNames: nameof(Name), IsUnique = true)]
    public class Specialization
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Должность")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        public List<UserProfile> UserProfiles { get; set; }
    }
}
