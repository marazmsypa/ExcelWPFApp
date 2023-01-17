using ExcelWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExcelWPF.Classes
{
    public class PasswordCheck
    {
        /// <summary>
        /// проверяет правильность пароля
        /// </summary>
       
        public bool RightPassword(string password)
        {

            string regPassword = @"^[0-9a-zA-Z/_/%]{5,50}$";
            if (Regex.IsMatch(password, regPassword))
            {
                return true;
            }
            return false;
        }
       
    }
}
