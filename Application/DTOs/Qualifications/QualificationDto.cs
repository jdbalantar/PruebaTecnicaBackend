using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Qualifications
{
    public class QualificationDto
    {
        public required int Id { get; set; }
        public required string Student{ get; set; }
        public required decimal Score { get; set; }
    }
}
