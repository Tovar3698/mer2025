using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class CitationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ProgrammedDate { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public int InfoDoctorId { get; set; }
        public bool IsControl { get; set; }
        public int ControlId { get; set; }
        public int UserId1 { get; set; } // Relación secundaria
    }
}
