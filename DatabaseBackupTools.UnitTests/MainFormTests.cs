using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseBackupTools.UnitTests
{
    [TestClass]
    public class MainFormTests
    {
        [TestMethod]
        public void GetDatabases_ListIsNotEmpty_ReturnsNonNullList()
        {
            // Arrange
            var mainform = new DatabaseBackupTool.MainForm();
            mainform.InitializeConnection();
            // Act
            var result = mainform.GetDatabases();

            // Assert
            Assert.IsTrue(result != null);
        }
        [TestMethod]
        public void GetDatabases_ListIsEmpty_ReturnsNonNullList() /* This tests the method's catch block. 
                                                                    We do not call mainform.InitializeConnection(), 
                                                                    therefore it cannot connect to the database. */
        {
            // Arrange
            var mainform = new DatabaseBackupTool.MainForm();

            // Act
            var result = mainform.GetDatabases();

            // Assert
            Assert.IsTrue(result != null);
        }
    }
}
