
namespace ExcelDataTableExporter
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.DataTableGroup = this.Factory.CreateRibbonGroup();
            this.ExportDataTableButton = this.Factory.CreateRibbonButton();
            this.ExportTextButton = this.Factory.CreateRibbonButton();
            this.TextTableGroup = this.Factory.CreateRibbonGroup();
            this.tab1.SuspendLayout();
            this.DataTableGroup.SuspendLayout();
            this.TextTableGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.DataTableGroup);
            this.tab1.Groups.Add(this.TextTableGroup);
            this.tab1.Label = "GenshinSim";
            this.tab1.Name = "tab1";
            // 
            // DataTableGroup
            // 
            this.DataTableGroup.Items.Add(this.ExportDataTableButton);
            this.DataTableGroup.Label = "Data Table";
            this.DataTableGroup.Name = "DataTableGroup";
            // 
            // ExportDataTableButton
            // 
            this.ExportDataTableButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ExportDataTableButton.Label = "Export";
            this.ExportDataTableButton.Name = "ExportDataTableButton";
            this.ExportDataTableButton.OfficeImageId = "ExportTextFile";
            this.ExportDataTableButton.ShowImage = true;
            this.ExportDataTableButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ExportButton_Click);
            // 
            // ExportTextButton
            // 
            this.ExportTextButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ExportTextButton.Label = "Export";
            this.ExportTextButton.Name = "ExportTextButton";
            this.ExportTextButton.OfficeImageId = "ExportTextFile";
            this.ExportTextButton.ShowImage = true;
            this.ExportTextButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ExportTextButton_Click);
            // 
            // TextTableGroup
            // 
            this.TextTableGroup.Items.Add(this.ExportTextButton);
            this.TextTableGroup.Label = "Text Table";
            this.TextTableGroup.Name = "TextTableGroup";
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.DataTableGroup.ResumeLayout(false);
            this.DataTableGroup.PerformLayout();
            this.TextTableGroup.ResumeLayout(false);
            this.TextTableGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup DataTableGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ExportDataTableButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup TextTableGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ExportTextButton;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
