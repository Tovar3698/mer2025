using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RelatedPerson
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TypeRelation { get; set; }
        public string Description { get; set; }

        // Propiedades de navegación
        public virtual Person Person { get; set; }
        public virtual User User { get; set; }
    }
}