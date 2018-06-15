using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheet.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        //[UserName]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
