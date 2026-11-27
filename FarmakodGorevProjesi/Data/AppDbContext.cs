using System;
using System.Collections.Generic;
using System.Text;

namespace FarmakodGorevProjesi.Data
{
    public class AppDbContext
    {

        private string userNameDt = "mustafa";
        private string passwordDt = "kale";

        public bool Login(string userName,string password)
        {
            return userName == userNameDt && password == passwordDt;
        
        }
    }
}
