﻿using OfficePass.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OfficePass.Areas.Adminka.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Display(Name = "Уровень доступа")]
        public Role Role { get; set; }

        [Display(Name = "Сотрудник")]
        public string UserProfile { get; set; }

    }
}