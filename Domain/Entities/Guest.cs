﻿using System.ComponentModel.DataAnnotations;

namespace OfficePass.Domain.Entities
{
    public class Guest
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

        [Display(Name = "Серия")]
        public string DocumentSerial { get; set; }

        [Display(Name = "Номер")]
        public string DocumentNumber { get; set; }

        public int CompanyId {  get; set; }

        [Display(Name = "Название")]
        public Company? Company { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Создатель")]
        public User? User { get; set; }

        public int DocumentTypeId { get; set; }

        [Display(Name = "Тип документа")]
        public DocumentType? DocumentType { get; set; }
    }
}
