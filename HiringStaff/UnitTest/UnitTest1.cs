using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiringStaff;
using System.IO;


namespace UnitTest
{
    [TestClass]
    public class ExportFile
    {
        [TestMethod]
        public void ExportPDF()
        {
            MultiReport multiReport = new MultiReport();
            multiReport.ExportingSalaryReportPDF("testPDF.pdf");
            Assert.IsTrue(File.Exists("testPDF.pdf"));
            File.Delete("testPDF.pdf");
        }

        [TestMethod]
        public void ExportCSV() 
        {
            MultiReport multiReport = new MultiReport();
            multiReport.ExportingSalaryReportCSV("testCSV.csv");
            Assert.IsTrue(File.Exists("testCSV.csv"));
            File.Delete("testCSV.csv");
        }

        [TestMethod]
        public void ExportXLSX()
        {
            MultiReport multiReport = new MultiReport();
            multiReport.ExportingSalaryReportXLSX("testXLSX.xlsx");
            Assert.IsTrue(File.Exists("testXLSX.xlsx"));
            File.Delete("testXLSX.xlsx");
        }
    }
}
