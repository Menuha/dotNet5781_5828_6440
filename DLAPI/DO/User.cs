using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Bonus. DO entity with user information.
    /// The manager does not have access to passenger information.
    /// </summary>
    public class User
    {
        public string UserName { get; set; } //Key
        public string Password { get; set; }
        public bool Admin { get; set; }
    }
}
