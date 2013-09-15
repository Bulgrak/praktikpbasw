using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkSheets;


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
        WorksheetPart QAGroupsSheet;
        WorksheetPart QAktUIDesignSheet;

        //Declare helper variables.
        string ktExaminedID;
        string ktUIDesignID;
        string ktUIFieldID;
        string ktUIGroupOrderID;
        string ktUIOrderID;
        string ktResourcesID;
        string ktResourceTranslationID;
        string ktResourceTypeID;
        string ktUIPageTypeID;
        string QAGroupsID;
        string QAktUIDesignID;

        public WorkSheetktExaminedGroup WorkSheetExaminedGroup { get; set; }
        public WorkSheetktUIDesign WorkSheetUIDesign { get; set; }
        public WorkSheetktUIFieldIncludedType WorkSheetktUIFieldIncludedType { get; set; }
        public WorkSheetktUIGroupOrder WorkSheetktUIGroupOrder { get; set; }
        public WorkSheetktUIOrder WorkSheetktUIOrder { get; set; }
        public WorkSheetktResources WorkSheetktResources { get; set; }
        public WorkSheetktResourceTranslation WorkSheetktResourceTranslation { get; set; }
        public WorkSheetktResourceType WorkSheetktResourceType { get; set; }
        public WorkSheetUIPageType WorkSheetktUIPageType { get; set; }
        public WorkSheetQAGroup WorkSheetQAGroups { get; set; }
        public WorkSheetQAktUIDesign WorkSheetQAktUIDesign { get; set; }

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
            this.WorkSheetktUIPageType = new WorkSheetUIPageType();
            this.WorkSheetQAGroups = new WorkSheetQAGroup();
            this.WorkSheetQAktUIDesign = new WorkSheetQAktUIDesign();

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
                ktUIFieldIncludedTypeSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktUIFieldID);

                //Load ktUIFieldIncludedType data to business object.
                this.WorkSheetktUIFieldIncludedType.LoadUIFieldIncludedType(ktUIFieldIncludedTypeSheet.Worksheet, sharedStrings);

                //Reference to Excel Worksheet with ktUIFieldIncludedTypeSheet data.
                ktUIGroupOrderID = workSheets.First(s => s.Name == this.WorkSheetktUIGroupOrder.SheetName).Id;
                ktUIGroupOrderSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktUIGroupOrderID);
                
                //Load ktUIFieldIncludedType data to business object.
                this.WorkSheetktUIGroupOrder.LoadUIGroupOrder(ktUIGroupOrderSheet.Worksheet, sharedStrings);

                //Reference to Excel Worksheet with ktUIFieldIncludedTypeSheet data.
                ktUIOrderID = workSheets.First(s => s.Name == this.WorkSheetktUIOrder.SheetName).Id;
                ktUIOrderSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktUIOrderID);

                //Load ktUIFieldIncludedType data to business object.
                this.WorkSheetktUIOrder.LoadUIOrder(ktUIOrderSheet.Worksheet, sharedStrings);

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

                ////Reference to Excel Worksheet with ktUIPageType data.
                ktUIPageTypeID = workSheets.First(s => s.Name == this.WorkSheetktUIPageType.SheetName).Id;
                ktUIPageTypeSheet = (WorksheetPart)document.WorkbookPart.GetPartById(ktUIPageTypeID);

                ////Load ktResource data to business object.
                this.WorkSheetktUIPageType.LoadUIPageType(ktUIPageTypeSheet.Worksheet, sharedStrings);

                ////Reference to Excel Worksheet with QAGroups data.
                QAGroupsID = workSheets.First(s => s.Name == this.WorkSheetQAGroups.SheetName).Id;
                QAGroupsSheet = (WorksheetPart)document.WorkbookPart.GetPartById(QAGroupsID);

                ////Load QAGroups data to business object.
                this.WorkSheetQAGroups.LoadQAGroups(QAGroupsSheet.Worksheet, sharedStrings);

                ////Reference to Excel Worksheet with QAGroups data.
                QAktUIDesignID = workSheets.First(s => s.Name == this.WorkSheetQAktUIDesign.SheetName).Id;
                QAktUIDesignSheet = (WorksheetPart)document.WorkbookPart.GetPartById(QAktUIDesignID);

                ////Load QAGroups data to business object.
                this.WorkSheetQAktUIDesign.LoadQAktUIDesign(QAktUIDesignSheet.Worksheet, sharedStrings);
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
