using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Teachers
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Identification { get; set; }
    }
}
