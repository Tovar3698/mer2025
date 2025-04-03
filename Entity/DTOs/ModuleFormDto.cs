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

        public int ModuleFormaId { get; set; }   
        public int ModuleId { get; set; }
        public int FormId {  get; set; }
        public object moduleform { get; set; }
        public string ModuleFormName { get; set; }

        public int Name { get; set; }
    }
}
