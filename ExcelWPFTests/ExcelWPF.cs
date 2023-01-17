using System;
using ExcelWPF.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExcelWPFTests
{
    [TestClass]
    public class ExcelWPF
    {
        PasswordCheck passwordCheckobj = new PasswordCheck();
        /// <summary>
        /// проверяет правильность пароля 12345
        /// </summary>
        [TestMethod]
        public void RightPassword_RightPassword_ReturnTrue()
        {
            //arrange
            string entrypassword = "12345";
            // assert 
            Assert.IsTrue(passwordCheckobj.RightPassword(entrypassword));

        }

        /// <summary>
        /// проверяет правильность пароля libero%88f
        /// </summary>
        [TestMethod]
        public void RightPassword_RightPasswordTwo_ReturnTrue()
        {
            //arrange
            string entrypassword = "libero%88f";
            // assert 
            Assert.IsTrue(passwordCheckobj.RightPassword(entrypassword));

        }

        /// <summary>
        /// проверяет неправильность пустого пароля 
        /// </summary>
        [TestMethod]
        public void RightPassword_EmptyPassword_ReturnFalse()
        {
            //arrange
            string entrypassword = "";
            // assert 
            Assert.IsFalse(passwordCheckobj.RightPassword(entrypassword));

        }

        /// <summary>
        /// проверяет неправильность пароля с лишними символами
        /// </summary>
        [TestMethod]
        public void RightPassword_WRongSymbols_ReturnFalse()
        {
            //arrange
            string entrypassword = "!!!!!";
            // assert 
            Assert.IsFalse(passwordCheckobj.RightPassword(entrypassword));

        }

        /// <summary>
        /// проверяет неправильность пароля с русскими буквами
        /// </summary>
        [TestMethod]
        public void RightPassword_Cyrilic_ReturnFalse()
        {
            //arrange
            string entrypassword = "аловапр";
            // assert 
            Assert.IsFalse(passwordCheckobj.RightPassword(entrypassword));

        }
    }
}
