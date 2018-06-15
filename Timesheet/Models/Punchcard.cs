using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheet.Models
{
    [Table("Punchcard")]
    public class Punchcard
    {
        [Key]
        public int PunchcardId { get; set; }

        [Display(Name = "Work Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime WorkDate { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime EndTime { get; set; }


        [Display(Name = "Break Time")]
        public Decimal BreakTime { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        [Display(Name = "Worked Time")]
        public TimeSpan WorkedHours
        {
            get
            {
                return this.EndTime - this.StartTime - TimeSpan.FromHours((double)this.BreakTime);
            }
        }

        [NotMapped]
        public string actionString { get; set; }



        [NotMapped]
        [Display(Name = "Duration")]
        public TimeSpan DurationTime
        {
            get
            {
                return this.EndTime - this.StartTime;
            }
        }

    }
}
