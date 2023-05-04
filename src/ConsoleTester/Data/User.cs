using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TextDatabase;

namespace TextDataBaseTester
{
    public class User:IAR
    {
        public string Name { get; set; }
        public string Surname { get; set; }        

        public static List<User> GetDummyUsers()
        {
            List<User> users = new List<User>();
            users.Add(new User() { Id = 1, Name = "Arthur", Surname = "Conan Doyle" });
            users.Add(new User() { Id = 2, Name = "Michael", Surname = "Jackson" });
            users.Add(new User() { Id = 3, Name = "Pepe", Surname = "Botella" });
            users.Add(new User() { Id = 4, Name = "Lola", Surname = "López" });

            return users;
        }
    }
}
