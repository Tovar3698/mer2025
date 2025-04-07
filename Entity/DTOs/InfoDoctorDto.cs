using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class InfoDoctorDto
    {
        public int Id { get; set; }
        public string Specialty { get; set; }
        public string TableName { get; set; }
        public DateTime RegistrationData { get; set; }
        public int IdUser { get; set; }

        // Propiedades de navegación
        public virtual User User { get; set; }
        public object Registration { get; set; }

    }
}
