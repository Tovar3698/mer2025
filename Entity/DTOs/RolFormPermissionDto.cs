using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RolFormPermissionDto
    {
        public int RolFormPermissioId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RolId { get; set; }
        public int FormId {  get; set; }
        public int PermissionId { get; set; }


    }
}
