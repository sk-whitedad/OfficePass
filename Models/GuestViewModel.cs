using System.ComponentModel.DataAnnotations;

namespace OfficePass.Models
{
    public class GuestViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string SurName { get; set; }

        [Display(Name = "ФИО")]
        public string? FullName { get; set; }

        [Display(Name = "Серия")]
        public string DocumentSerial { get; set; }

        [Display(Name = "Номер")]
        public string DocumentNumber { get; set; }

        [Display(Name = "Организация")]
        public string? CompanyName { get; set; }

        [Display(Name = "Организация")]
        public int CompanyId { get; set; }

        [Display(Name = "Тип документа")]
        public string? DocumentType { get; set; }

        [Display(Name = "Тип документа")]
        public int DocumentTypeId { get; set; }

    }
}
