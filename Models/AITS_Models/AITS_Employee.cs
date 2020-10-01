using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BEEERP.Models.AITS_Models
{
    public class AITS_Employee
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmpID { get; set; }
        public string EmployeeName { get; set; }
        public int ReferenceEmpID { get; set; }
        public DateTime Date { get; set; }
        public DateTime JoiningDate { get; set; }
        [NotMapped]
        public string RefeneceName { get; set; }
        [NotMapped]
        public int DesignationID { get; set; }
    }
}