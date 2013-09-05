using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using TreatPraktik.Model;


namespace TreatPraktik.ViewModel
{
    class ImportExcel
    {
        //Declare variables to hold refernces to Excel objects.
        Workbook workBook;
        SharedStringTable sharedStrings;
        IEnumerable<Sheet> workSheets;
        WorksheetPart ktExaminedGroupSheet;
        WorksheetPart ktUIDesignSteet;
        WorksheetPart ktUIFieldIncludedTypeSheet;
        WorksheetPart ktUIGroupOrderSheet;
        WorksheetPart ktUIOrderSheet;
        WorksheetPart ktUIPageTypeSheet;

        //Declare helper variables.
        string ktExaminedID;
        string ktUIDesignID;
        string ktUIFieldID;
        string ktUIGroupID;
        string ktUIOrderID;
        string ktUIPageID;
        List<ktExaminedGroup> examinedGroupList;
        List<ktUIDesign> uiDesignList;
        List<ktUIFieldIncludedType> uiFieldIncludedList;
        List<ktUiGroupOrder> uiGroupOrderList;
        List<ktUIOrder> uiOrderList;
        List<ktUIPageType> uiPageTypeList;

        public ImportExcel()
        {
            //Open the Excel workbook.
            using (SpreadsheetDocument document =
              SpreadsheetDocument.Open(@"D:\MyFiles\Dropbox\Treat praktik\UITablesToStud.xlsx", true))
            {
                //References to the workbook and Shared String Table.
                workBook = document.WorkbookPart.Workbook;
                workSheets = workBook.Descendants<Sheet>();
                sharedStrings =
                  document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                //Reference to Excel Worksheet with Customer data.
                ktExaminedID =
                  workSheets.First(s => s.Name == @"Customer").Id;
                custSheet =
                  (WorksheetPart)document.WorkbookPart.GetPartById(custID);

            }
        }
        
    }
}
