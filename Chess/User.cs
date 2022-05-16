using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class User
    {
        public string login;
        public string password;
        public string userIP;
        public bool loggedIn;
        public string name;
        public int rating;
        public bool findingMatch;

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
