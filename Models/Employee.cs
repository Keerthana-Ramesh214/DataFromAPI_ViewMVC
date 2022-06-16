using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataFromAPI_Mvc.Models
{
    public class Employee
    {
        public int Eid { get; set; }
        public string Empname { get; set; }
        public double? Salary { get; set; }
        public DateTime? Doj { get; set; }
        public string City { get; set; }
        public int? Deptid { get; set; }
    }
}
