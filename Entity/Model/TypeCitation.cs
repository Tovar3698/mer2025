using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class TypeCitation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Frequency { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public string Property5 { get; set; }

        // Propiedad de navegación
        public virtual ICollection<Citation> Citations { get; set; }

        public TypeCitation()
        {
            Citations = new List<Citation>();
        }
    }
}