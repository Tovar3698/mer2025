using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ModuleFormDto
    {
        public int ModuleFormId;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ModuleFormaId { get; set; }   
        public int ModuleId { get; set; }
        public int FormId {  get; set; }
        public object moduleform { get; set; }
        public string ModuleFormName { get; set; }

    }
}
