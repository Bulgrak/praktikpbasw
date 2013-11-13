using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Windows;
using TreatPraktik.Ressources.ExcelClasses;

namespace TreatPraktik.ViewModel
{
    public class ExportExcel
    {
        private static ExportExcel _instance;

        private ExaminedGroup _examinedGroup;
        private Order _order;
        private GroupOrder _groupOrder;
        private Resources _resources;
        private ResourceTranslation _rTranslation;
        private ECktUIDesign _uiDesign;
        private ECQAGroups _qaGroups;
        private ECQAktUIDesign _qAktUiDesign;
        private ResourceType _resourceType;
        private ECktUIPageType _pageType;
        private ECktUIFieldIncludedType _fieldIncludedType;

        private WorkspaceViewModel _wvm;

        private ExportExcel()
        {
            _wvm = WorkspaceViewModel.Instance;
        }

        /// <summary>
        /// Creates a new Excel file.
        /// </summary>
        /// <param name="path">Defines the path to where the file should be placed</param>
        public void CreateNewExcel(string path)
        {
            _examinedGroup = new ExaminedGroup();
            _order = new Order();
            _groupOrder = new GroupOrder();
            _resources = new Resources();
            _rTranslation = new ResourceTranslation();
            _uiDesign = new ECktUIDesign();
            _qaGroups = new ECQAGroups();
            _qAktUiDesign = new ECQAktUIDesign();
            _resourceType = new ResourceType();
             _pageType = new ECktUIPageType();
            _fieldIncludedType = new ECktUIFieldIncludedType();

            try
            {
                // Create a spreadsheet document by supplying the filepath.
                // By default, AutoSave = true, Editable = true, and Type = xlsx.
                SpreadsheetDocument spreadsheetDocument =
                    SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild(new Sheets());

                //Add sheets to the Workbook
                _examinedGroup.CreateSheet(sheets, spreadsheetDocument, workbookpart);
                _order.CreateSheet(sheets, spreadsheetDocument, workbookpart);
                
                _groupOrder.CreateSheet(sheets, spreadsheetDocument, workbookpart);
                _resources.CreateSheet(sheets, spreadsheetDocument, workbookpart);
                _rTranslation.CreateSheet(sheets, spreadsheetDocument, workbookpart);
                _uiDesign.CreateSheet(sheets, spreadsheetDocument, workbookpart);
            _qaGroups.CreateSheet(sheets, spreadsheetDocument, workbookpart);
            _qAktUiDesign.CreateSheet(sheets, spreadsheetDocument, workbookpart);
            _resourceType.CreateSheet(sheets, spreadsheetDocument, workbookpart);
            _pageType.CreateSheet(sheets, spreadsheetDocument, workbookpart);
            _fieldIncludedType.CreateSheet(sheets, spreadsheetDocument, workbookpart);
            
                

                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();

                _wvm._changedFlag = false;
            }
            catch (Exception e)
            {
                // Maybe save exception in a log file
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Singleton implementation
        /// </summary>
        public static ExportExcel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExportExcel();
                }
                return _instance;
            }
        }

    }
}
