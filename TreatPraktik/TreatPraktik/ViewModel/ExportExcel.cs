using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.View;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Windows;
using TreatPraktik.Model.WorkSheets;
using TreatPraktik.Ressources.ExcelClasses;

namespace TreatPraktik.ViewModel
{
    public class ExportExcel
    {
        private static ExportExcel instance;

        ExaminedGroup examinedGroup;
        Resources resources;
        GroupOrder groupOrder;
        Order order;
        

        //WorkSheetktResourceTranslation resourceTranslation;
        //WorkSheetktResourceType resourceType;
        //WorkSheetktUIDesign design;
        //WorkSheetktUIFieldIncludedType fieldIncludeType;
        //WorkSheetktUIOrder order;
        //WorkSheetQAGroup qaGroup;
        //WorkSheetQAktUIDesign qaDesign;
        //WorkSheetUIPageType pageType;

        private ExportExcel()
        {
        }

        /// <summary>
        /// Creates a new Excel file.
        /// </summary>
        /// <param name="path">Defines the path to where the file should be placed</param>
        public void CreateNewExcel(string path)
        {
            examinedGroup = new ExaminedGroup();
            resources = new Resources();
            groupOrder = new GroupOrder();
            order = new Order();
            
            //resourceTranslation = WorkSheetktResourceTranslation.Instance;
            //resourceType = WorkSheetktResourceType.Instance;
            //design = WorkSheetktUIDesign.Instance;
            //fieldIncludeType = WorkSheetktUIFieldIncludedType.Instance;
            //groupOrder = WorkSheetktUIGroupOrder.Instance;
            //order = WorkSheetktUIOrder.Instance;
            //qaGroup = WorkSheetQAGroup.Instance;
            //qaDesign = WorkSheetQAktUIDesign.Instance;
            //pageType = WorkSheetUIPageType.Instance;

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
                    AppendChild<Sheets>(new Sheets());

                //Add sheets to the Workbook
                examinedGroup.CreateSheet(sheets, spreadsheetDocument, workbookpart);
                resources.CreateSheet(sheets, spreadsheetDocument, workbookpart);
                //groupOrder.CreateSheet(sheets, spreadsheetDocument, workbookpart);
                order.CreateSheet(sheets, spreadsheetDocument, workbookpart);

                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();
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
                if (instance == null)
                {
                    instance = new ExportExcel();
                }
                return instance;
            }
        }

    }
}
