using System;
using System.ComponentModel.DataAnnotations;

namespace WPH.Models.CustomDataModels.UserManagment
{
    public class LoginViewModel
    {
        public Guid Guid { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Pass3 { get; set; }
        
        public string Code { get; set; }
        
        public int? UserCode { get; set; }

        public string CaptchaCode { get; set; }
    }
}