using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Module
    {
        public string Active;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }

        //Dependencia para la tabla pivote ModuleForm
        public List<ModuleForm> ModuleForm { get; set; } = new List<ModuleForm>();
    }
}
