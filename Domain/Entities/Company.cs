using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    [Index(propertyNames: nameof(Name), IsUnique = true)]
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

        public List<Guest> Guests { get; set; }

    }
}
