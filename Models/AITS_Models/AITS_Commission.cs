using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BEEERP.Models.AITS_Models
{
    public class AITS_Commission
    {
        public int EmployeeID { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }
        [NotMapped]
        public string Code { get; set; }
        [NotMapped]
        public string Designation { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public decimal SalesCommission { get; set; } = 0;
        [NotMapped]
        public decimal OrdinalCommission { get; set; } = 0;
        [NotMapped]
        public decimal InboundCommission { get; set; } = 0;
        [NotMapped]
        public decimal OutboundCommission { get; set; }=0;
        [NotMapped]
        public decimal GBCommission { get; set; } = 0;
    }
}