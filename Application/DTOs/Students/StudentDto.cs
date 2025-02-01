using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Students
{
    public class StudentDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Identification { get; set; }
        public required string Course { get; set; }
    }
}
