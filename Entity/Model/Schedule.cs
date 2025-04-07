using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public int InfoDoctorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public int PracticeId { get; set; }

        // Propiedades de navegación
        public virtual InfoDoctor InfoDoctor { get; set; }
        public virtual Practice Practice { get; set; }
    }
}