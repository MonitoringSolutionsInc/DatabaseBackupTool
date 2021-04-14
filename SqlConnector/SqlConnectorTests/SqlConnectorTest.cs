using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlConnector; 

namespace SqlConnectorTests
{
    [TestClass]
    public class SqlConnectorTest
    {
        [TestMethod]
        public void CreateCommand_QueryStringIsNotEmpty_ReturnPassedSQLString()
        {
             // Arrange
            SQLConnector conn = new SQLConnector("");
            conn.InitializeConnection();

            // Act
            string sql = "SELECT * FROM DATABASE";
            var result = conn.CreateCommand(sql);

            // Assert
            Assert.IsTrue(result.CommandText == sql);
        }
        [TestMethod]
        public void CreateCommand_QueryStringEmpty_ReturnPassedSQLString()
        {
            // Arrange
            SQLConnector conn = new SQLConnector("");
            conn.InitializeConnection();

            // Act
            string sql = "";
            var result = conn.CreateCommand(sql);

            // Assert
            Assert.IsTrue(result.CommandText == sql);
        }
        [TestMethod]
        public void CreateCommand_QueryStringNull_ThrowsNullQueryStringException()
        {
            // Arrange
            SQLConnector conn = new SQLConnector("");
            conn.InitializeConnection();

            // Act
            string sql = null;

            // Assert
            var exception = Assert.ThrowsException<NullQueryStringException>(()=> conn.CreateCommand(sql));
            Assert.IsTrue(exception.Message == "The Query String was null.");
        }

    }
}

