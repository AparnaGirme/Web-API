using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class EmployeeSecurity
    {
        public static bool Login(string username, string password)
        {
            using(PracticeEntities pe = new PracticeEntities())
            {
                return pe.Users.Any(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && user.Password.Equals(password));
            }
        }
    }
}