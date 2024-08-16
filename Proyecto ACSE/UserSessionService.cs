using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_ACSE
{
    public class UserSessionService
    {
        private static UserSessionService _instance;

        public static UserSessionService Instance => _instance ??= new UserSessionService();

        public User CurrentUser { get; set; }

        private UserSessionService() { }

        public void ClearSession()
        {
            CurrentUser = null;
        }
    }
}