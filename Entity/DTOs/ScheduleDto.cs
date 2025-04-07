using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int InfoDoctorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreationDate { get; set; }
        public string IsDeleted { get; set; }
        public int PracticeId { get; set; }

        // Propiedades de navegación
        public virtual InfoDoctor InfoDoctor { get; set; }
        public virtual Practice Practice { get; set; }
    }
}
