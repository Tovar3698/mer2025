using System;
using System.Collections.Generic;

namespace Entity.Model
{
    public class Citation
    {
        public int Id { get; set; }
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
