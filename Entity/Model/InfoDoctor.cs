using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class InfoDoctor
    {
        public int Id { get; set; }
        public string Specialty { get; set; }
        public DateTime RegistrationData { get; set; }
        public int IdUser { get; set; }

        // Propiedades de navegación
        public virtual User User { get; set; }
    }
}