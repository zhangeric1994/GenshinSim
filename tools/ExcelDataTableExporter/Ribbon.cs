using GenshinSim;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;


namespace ExcelDataTableExporter
{
    public partial class Ribbon
    {
        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();


        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
        }


        private void ExportButton_Click(object sender, RibbonControlEventArgs e)
        {
            var application = Globals.ThisAddIn.Application;


            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.SelectedPath = application.ActiveWorkbook.Path;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Worksheet activeSheet = application.ActiveSheet;

                using (StreamWriter writer = File.CreateText(string.Format("{0}/generated/{1}.json", folderBrowserDialog.SelectedPath, activeSheet.Name)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, GenshinDataTableReader.Instance.ReadExcelWorksheet(activeSheet, out Type dataTableType));
                }
            }
        }
    }
}
