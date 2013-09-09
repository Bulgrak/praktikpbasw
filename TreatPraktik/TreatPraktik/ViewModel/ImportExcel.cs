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
        private static ImportExcel instance;

        //Declare variables to hold refernces to Excel objects.
        Workbook workBook;
        SharedStringTable sharedStrings;
        IEnumerable<Sheet> workSheets;
        WorksheetPart ktExaminedGroupSheet;
        WorksheetPart ktUIDesignSheet;
        WorksheetPart ktUIFieldIncludedTypeSheet;
        WorksheetPart ktUIGroupOrderSheet;
        WorksheetPart ktUIOrderSheet;
        WorksheetPart ktUIPageTypeSheet;
        WorksheetPart ktResourcesSheet;
        WorksheetPart ktResourceTranslationSheet;
        WorksheetPart ktResourceTypeSheet;

        //Declare helper variables.
        string ktExaminedID;
        string ktUIDesignID;
        string ktUIFieldID;
        string ktUIGroupID;
        string ktUIOrderID;
        string ktUIPageID;
        string ktResourcesID;
        string ktResourceTranslationID;
        string ktResourceTypeID;
        //List<ktExaminedGroup> ExaminedGroupList;
        //public List<ktUIDesign> UIDesignList { get; set;}
        //List<ktUIFieldIncludedType> UIFieldList;
        //List<ktUIGroupOrder> UIGroupList;
        //List<ktUIOrder> UIOrderList;
        //List<ktUIPageType> UIPageList;

        public WorkSheetktExaminedGroup WorkSheetExaminedGroup { get; set; }
        public WorkSheetktUIDesign WorkSheetUIDesign { get; set; }
        public WorkSheetktUIFieldIncludedType WorkSheetktUIFieldIncludedType { get; set; }
        public WorkSheetktUIGroupOrder WorkSheetktUIGroupOrder { get; set; }
        public WorkSheetktUIOrder WorkSheetktUIOrder { get; set; }
        public WorkSheetktResources WorkSheetktResources { get; set; }
        public WorkSheetktResourceTranslation WorkSheetktResourceTranslation { get; set; }
        public WorkSheetktResourceType WorkSheetktResourceType { get; set; }

        //List<ktExaminedGroup> ExaminedGroupList;
        //public List<ktUIDesign> UIDesignList { get; set; }
        //List<ktUIFieldIncludedType> UIFieldList;
        //List<ktUIGroupOrder> UIGroupList;
        //List<ktUIOrder> UIOrderList;
        //List<ktUIPageType> UIPageList;

        public ImportExcel()
        {
            this.WorkSheetExaminedGroup = new WorkSheetktExaminedGroup();
            this.WorkSheetUIDesign = new WorkSheetktUIDesign();
            this.WorkSheetktUIFieldIncludedType = new WorkSheetktUIFieldIncludedType();
            this.WorkSheetktUIGroupOrder = new WorkSheetktUIGroupOrder();
            this.WorkSheetktUIOrder = new WorkSheetktUIOrder();
            this.WorkSheetktResources = new WorkSheetktResources();
            this.WorkSheetktResourceTranslation = new WorkSheetktResourceTranslation();
            this.WorkSheetktResourceType = new WorkSheetktResourceType();

            //Open the Excel workbook.
            using (SpreadsheetDocument document =
              SpreadsheetDocument.Open(@"C:\UITablesToStud.xlsx", true))
            {
                //References to the workbook and Shared String Table.
                workBook = document.WorkbookPart.Workbook;
                workSheets = workBook.Descendants<Sheet>();
                sharedStrings = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                //Reference to Excel Worksheet with ktExaminedGroup data.
                ktExaminedID = workSheets.First(s => s.Name == this.WorkSheetExaminedGroup.SheetName).Id;
                ktExaminedGroupSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktExaminedID);

                //Load ktExaminedGroup data to business object.
                this.WorkSheetExaminedGroup.LoadExaminedGroup(ktExaminedGroupSheet.Worksheet, sharedStrings);

                //Reference to Excel Worksheet with ktUIDesign data.
                ktUIDesignID = workSheets.First(s => s.Name == this.WorkSheetUIDesign.SheetName).Id;
                ktUIDesignSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktUIDesignID);

                //Load ktDesign data to business object.
                this.WorkSheetUIDesign.LoadUIDesign(ktUIDesignSheet.Worksheet, sharedStrings);

                //Reference to Excel Worksheet with ktUIFieldIncludedTypeSheet data.
                ktUIFieldID = workSheets.First(s => s.Name == this.WorkSheetktUIFieldIncludedType.SheetName).Id;
                ktUIFieldIncludedTypeSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktUIDesignID);

                //Load ktUIFieldIncludedType data to business object.
                this.WorkSheetktUIFieldIncludedType.LoadUIFieldIncludedType(ktUIFieldIncludedTypeSheet.Worksheet, sharedStrings);

                //Reference to Excel Worksheet with ktResource data.
                ktResourcesID = workSheets.First(s => s.Name == this.WorkSheetktResources.SheetName).Id;
                ktResourcesSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktResourcesID);

                ////Load ktResource data to business object.
                this.WorkSheetktResources.LoadktResources(ktResourcesSheet.Worksheet, sharedStrings);

                ////Reference to Excel Worksheet with ktResourceTranslation data.
                ktResourceTranslationID = workSheets.First(s => s.Name == this.WorkSheetktResourceTranslation.SheetName).Id;
                ktResourceTranslationSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktResourceTranslationID);

                ////Load ktResouceTranslation data to business object.
                this.WorkSheetktResourceTranslation.LoadktResourceTranslation(ktResourceTranslationSheet.Worksheet, sharedStrings);


                ////Reference to Excel Worksheet with ktResource data.
                ktResourceTypeID = workSheets.First(s => s.Name == this.WorkSheetktResourceType.SheetName).Id;
                ktResourceTypeSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktResourceTypeID);

                ////Load ktResource data to business object.
                this.WorkSheetktResourceType.LoadktResourceType(ktResourceTypeSheet.Worksheet, sharedStrings);
            }
        }

        public static ImportExcel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ImportExcel();
                }
                return instance;
            }
        }
    }
}
