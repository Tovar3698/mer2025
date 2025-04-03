using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class RolUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Form { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; }

    }
}
