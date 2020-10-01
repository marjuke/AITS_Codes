using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BEEERP.Models.AITS_Models
{
    public class AITS_SaleEntry
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public int CodeID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime SaleDate { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public string CodeName { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public string Designation { get; set; }
    }
}