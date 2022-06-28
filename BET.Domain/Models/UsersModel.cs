using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BET.Domain.Models
{

    public class UsersModel
    {
        [Key]
        public int UserId { get; set; }
  
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "UserSalt")]
        public string UserSalt { get; set; }

      
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    
    }

    [DataContract]
    public class LoginPurchaseModel
    {
      [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int UserId { get; set; }
    }


}
