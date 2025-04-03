using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //Dependencia para la tabla pivote RolFormPermission
        public List<RolFormPermission> RolFormPermission{ get; set; } = new List<RolFormPermission>();

        //Dependencia para la tabla pivote ModuleForm
        public List<ModuleForm> ModuleForm { get; set; } = new List<ModuleForm>();
        public bool IsDeleted { get; set; }
    }
}
