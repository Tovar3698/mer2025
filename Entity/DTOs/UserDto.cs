using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class UserDto
    {
        

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
     

        public static object User { get; set; }
        public static int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Telephone { get; set; }
        
    }
}
