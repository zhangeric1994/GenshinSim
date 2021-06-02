using GenshinSim;
using Microsoft.Office.Tools.Ribbon;
using STK.DataTable;
using System;
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
                DataTable dataTable = GenshinDataTableReader.Instance.ReadExcelWorksheet(application.ActiveSheet, out Type _);

                string directory = folderBrowserDialog.SelectedPath + "\\generated";
                string file = string.Format("{0}\\{1}.json", directory, dataTable.name);

                DataTableManager.Instance.ImportFromJSON(file);

                dataTable.ExportToJSON(directory);
            }
        }


        private void ExportTextButton_Click(object sender, RibbonControlEventArgs e)
        {
            var application = Globals.ThisAddIn.Application;


            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.SelectedPath = application.ActiveWorkbook.Path;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TextTableExport.ExportExcelWorkbook(Globals.ThisAddIn.Application.ActiveWorkbook, folderBrowserDialog.SelectedPath + "\\generated\\text");
            }
        }
    }
}
