using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BEEERP.Models.AITS_Models
{
    public class AITS_EmployeeDesignation
    {
        [Key, Column("ID", Order = 0)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [Key, Column("EmpID", Order = 1)]
        public int EmpID { get; set; }
        [Key, Column("DesignationID", Order = 2)]
        public int DesignationID { get; set; }
        public int FirstCode { get; set; }
        public int SecondCode { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public string DesignationName { get; set; }
        [NotMapped]
        public string FirstCodeName { get; set; }
        [NotMapped]
        public string SecondCodeName { get; set; }
    }
}