using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class User
    {
        public string login { get; set; }
        public string password { get; set; }
        public string userIP { get; set; }
        public bool loggedIn { get; set; }
        public string name { get; set; }
        public int rating { get; set; }
        public bool findingMatch { get; set; }

        public User(string login, string password, string userIP)
        {
            this.login = login;
            this.password = password;
            this.userIP = userIP;
            findingMatch = false;
            //Connect to MySQL, if success -> loggedIn = true; also get: name, rating
        }

        public User()
        {

        }
    }
}
