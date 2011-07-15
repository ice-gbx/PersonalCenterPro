using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalCenter.Service.User
{
    [Serializable]
    public class UserView
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Uname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Skin { get; set; }
    }
}
