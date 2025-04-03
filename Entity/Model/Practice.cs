using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entity.Model
{
    public class Practice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDate { get; set; }

        // Propiedad de navegación
        public virtual ICollection<Schedule> Schedule { get; set; }

        public Practice()
        {
            Schedule = new List<Schedule>();
        }
    }
}
