using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using TreatPraktik.Model.WorkSheets;


namespace TreatPraktik.ViewModel
{
    public class ImportExcel
    {
        private static ImportExcel _instance;

        //Declare variables to hold refernces to Excel objects.
        Workbook _workBook;
        SharedStringTable _sharedStrings;
        IEnumerable<Sheet> _workSheets;
        WorksheetPart _ktExaminedGroupSheet;
        WorksheetPart _ktUiDesignSheet;
        WorksheetPart _ktUiFieldIncludedTypeSheet;
        WorksheetPart _ktUiGroupOrderSheet;
        WorksheetPart _ktUiOrderSheet;
        WorksheetPart _ktUiPageTypeSheet;
        WorksheetPart _ktResourcesSheet;
        WorksheetPart _ktResourceTranslationSheet;
        WorksheetPart _ktResourceTypeSheet;
        WorksheetPart _qaGroupsSheet;
        WorksheetPart _qAktUiDesignSheet;

        //Declare helper variables.
        string _ktExaminedID;
        string _ktUiDesignID;
        string _ktUiFieldID;
        string _ktUiGroupOrderID;
        string _ktUiOrderID;
        string _ktResourcesID;
        string _ktResourceTranslationID;
        string _ktResourceTypeID;
        string _ktUiPageTypeID;
        string _qaGroupsID;
        string _qAktUiDesignID;

        public WorkSheetktExaminedGroup _workSheetktExaminedGroup { get; set; }
        public WorkSheetktUIDesign _workSheetUIDesign { get; set; }
        public WorkSheetktUIFieldIncludedType _workSheetktUIFieldIncludedType { get; set; }
        public WorkSheetktUIGroupOrder _workSheetktUIGroupOrder { get; set; }
        public WorkSheetktUIOrder _workSheetktUIOrder { get; set; }
        public WorkSheetktResources _workSheetktResources { get; set; }
        public WorkSheetktResourceTranslation _workSheetktResourceTranslation { get; set; }
        public WorkSheetktResourceType _workSheetktResourceType { get; set; }
        public WorkSheetUIPageType _workSheetktUIPageType { get; set; }
        public WorkSheetQAGroup _workSheetQAGroups { get; set; }
        public WorkSheetQAktUIDesign _workSheetQAktUIDesign { get; set; }

        public bool ImportFileOK;

        public bool FileTypeOk;
        public bool SheetNameOk;

        private ImportExcel()
        {
            ImportFileOK = true;
            FileTypeOk = true;
            SheetNameOk = true;
        }

        private void ResetImportConfiguraion()
        {
            WorkSheetktExaminedGroup.Instance = null;
            _ktExaminedGroupSheet = null;
            _ktExaminedID = "";
            _workSheetktExaminedGroup = WorkSheetktExaminedGroup.Instance;

            WorkSheetktUIGroupOrder.Instance = null;
            _ktUiGroupOrderSheet = null;
            _ktUiGroupOrderID = "";
            _workSheetktUIGroupOrder = WorkSheetktUIGroupOrder.Instance;

            WorkSheetktUIOrder.Instance = null;
            _ktUiOrderSheet = null;
            _ktUiOrderID = "";
            _workSheetktUIOrder = WorkSheetktUIOrder.Instance;

            WorkSheetktResources.Instance = null;
            _ktResourcesSheet = null;
            _ktResourcesID = "";
            _workSheetktResources = WorkSheetktResources.Instance;

            WorkSheetktResourceTranslation.Instance = null;
            _ktResourceTranslationSheet = null;
            _ktResourceTranslationID = "";
            _workSheetktResourceTranslation = WorkSheetktResourceTranslation.Instance;

            _workBook = null;
            _workSheets = null;
            _sharedStrings = null;
        }

        public void ImportExcelConfiguration(string path)
        {
            ResetImportConfiguraion();

            //Check that the file is an excel file
            if (CheckFileType(path))
            {
                if (CheckSheetNames(path))
                {
                    //Open the Excel workbook.
                    using (SpreadsheetDocument document =
                        SpreadsheetDocument.Open(path, true))
                    {
                        //References to the workbook and Shared String Table.
                        _workBook = document.WorkbookPart.Workbook;
                        _workSheets = _workBook.Descendants<Sheet>();
                        _sharedStrings = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                        //Reference to Excel Worksheet with ktExaminedGroup data.
                        _ktExaminedID = _workSheets.First(s => s.Name == this._workSheetktExaminedGroup.SheetName).Id;
                        _ktExaminedGroupSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktExaminedID);

                        ////Load ktExaminedGroup data to business object.
                        //this._workSheetktExaminedGroup.LoadExaminedGroup(_ktExaminedGroupSheet.Worksheet, _sharedStrings);

                        //Reference to Excel Worksheet with ktUIFieldIncludedTypeSheet data.
                        _ktUiGroupOrderID = _workSheets.First(s => s.Name == this._workSheetktUIGroupOrder.SheetName).Id;
                        _ktUiGroupOrderSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktUiGroupOrderID);

                        ////Load ktUIFieldIncludedType data to business object.
                        //this._workSheetktUIGroupOrder.LoadUIGroupOrder(_ktUiGroupOrderSheet.Worksheet, _sharedStrings);

                        //Reference to Excel Worksheet with ktUIFieldIncludedTypeSheet data.
                        _ktUiOrderID = _workSheets.First(s => s.Name == this._workSheetktUIOrder.SheetName).Id;
                        _ktUiOrderSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktUiOrderID);

                        //Load ktUIFieldIncludedType data to business object.
                        this._workSheetktUIOrder.LoadUIOrder(_ktUiOrderSheet.Worksheet, _sharedStrings);

                        //Reference to Excel Worksheet with ktResource data.
                        _ktResourcesID = _workSheets.First(s => s.Name == this._workSheetktResources.SheetName).Id;
                        _ktResourcesSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktResourcesID);

                        ////Load ktResource data to business object.
                        this._workSheetktResources.LoadktResources(_ktResourcesSheet.Worksheet, _sharedStrings);

                        ////Reference to Excel Worksheet with ktResourceTranslation data.
                        _ktResourceTranslationID =
                            _workSheets.First(s => s.Name == this._workSheetktResourceTranslation.SheetName).Id;
                        _ktResourceTranslationSheet =
                            (WorksheetPart)document.WorkbookPart.GetPartById(_ktResourceTranslationID);

                        ////Load ktResouceTranslation data to business object.
                        this._workSheetktResourceTranslation.LoadktResourceTranslation(_ktResourceTranslationSheet.Worksheet,
                            _sharedStrings);

                        if (this._workSheetktExaminedGroup.LoadExaminedGroup(_ktExaminedGroupSheet.Worksheet, _sharedStrings) &&
                            this._workSheetktUIGroupOrder.LoadUIGroupOrder(_ktUiGroupOrderSheet.Worksheet, _sharedStrings)
                            )
                        {
                            _workSheetktExaminedGroup.AcceptChanges();
                            _workSheetktUIGroupOrder.AcceptChanges();
                        }
                    }
                }
            }

            if (!FileTypeOk)
            {
                ImportFileOK = false;
                MessageBox.Show("This file is not an excel file with the correct (.xlsx) file extension",
                    "Wrong file type", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!SheetNameOk)
            {
                ImportFileOK = false;
                MessageBox.Show("The configuration excel file does not contain the correct excel sheets",
                    "Wrong excel file", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!_workSheetktExaminedGroup.ColumnHeadersOk &&
                !_workSheetktUIGroupOrder.ColumnHeadersOk
                )
            {
                ImportFileOK = false;
                MessageBox.Show("One or all of the excel sheet does not have the right column headers",
                    "Wrong column names", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!_workSheetktExaminedGroup.DataOnSheetOk &&
                !_workSheetktUIGroupOrder.DataOnSheetOk
                )
            {
                ImportFileOK = false;
                MessageBox.Show("One or all of the excel sheets does not have any data",
                   "No data on sheet", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        /// <summary>
        /// Imports all data from the original Excel file 
        /// </summary>
        public void ImportExcelFromFile(string path)
        {
            _workSheetktExaminedGroup = WorkSheetktExaminedGroup.Instance;
            _workSheetUIDesign = WorkSheetktUIDesign.Instance;
            _workSheetktUIFieldIncludedType = WorkSheetktUIFieldIncludedType.Instance;
            _workSheetktUIGroupOrder = WorkSheetktUIGroupOrder.Instance;
            _workSheetktUIOrder = WorkSheetktUIOrder.Instance;
            _workSheetktResources = WorkSheetktResources.Instance;
            _workSheetktResourceTranslation = WorkSheetktResourceTranslation.Instance;
            _workSheetktResourceType = WorkSheetktResourceType.Instance;
            _workSheetktUIPageType = WorkSheetUIPageType.Instance;
            _workSheetQAGroups = WorkSheetQAGroup.Instance;
            _workSheetQAktUIDesign = WorkSheetQAktUIDesign.Instance;

            //Check that the file is an excel file
            if (CheckFileType(path))
            {
                if (CheckSheetNames(path))
                {
                    //Open the Excel workbook.
                    using (SpreadsheetDocument document =
                      SpreadsheetDocument.Open(path, true))
                    {
                        //References to the workbook and Shared String Table.
                        _workBook = document.WorkbookPart.Workbook;
                        _workSheets = _workBook.Descendants<Sheet>();
                        _sharedStrings = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                        //Reference to Excel Worksheet with ktExaminedGroup data.
                        _ktExaminedID = _workSheets.First(s => s.Name == this._workSheetktExaminedGroup.SheetName).Id;
                        _ktExaminedGroupSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktExaminedID);

                        

                        //Reference to Excel Worksheet with ktUIDesign data.
                        _ktUiDesignID = _workSheets.First(s => s.Name == this._workSheetUIDesign.SheetName).Id;
                        _ktUiDesignSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktUiDesignID);



                        //Reference to Excel Worksheet with ktUIFieldIncludedTypeSheet data.
                        _ktUiFieldID = _workSheets.First(s => s.Name == this._workSheetktUIFieldIncludedType.SheetName).Id;
                        _ktUiFieldIncludedTypeSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktUiFieldID);

                        //Load ktUIFieldIncludedType data to business object.
                        this._workSheetktUIFieldIncludedType.LoadUIFieldIncludedType(_ktUiFieldIncludedTypeSheet.Worksheet, _sharedStrings);

                        //Reference to Excel Worksheet with ktUIFieldIncludedTypeSheet data.
                        _ktUiGroupOrderID = _workSheets.First(s => s.Name == this._workSheetktUIGroupOrder.SheetName).Id;
                        _ktUiGroupOrderSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktUiGroupOrderID);



                        //Reference to Excel Worksheet with ktUIFieldIncludedTypeSheet data.
                        _ktUiOrderID = _workSheets.First(s => s.Name == this._workSheetktUIOrder.SheetName).Id;
                        _ktUiOrderSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktUiOrderID);

                        //Load ktUIFieldIncludedType data to business object.
                        this._workSheetktUIOrder.LoadUIOrder(_ktUiOrderSheet.Worksheet, _sharedStrings);

                        //Reference to Excel Worksheet with ktResource data.
                        _ktResourcesID = _workSheets.First(s => s.Name == this._workSheetktResources.SheetName).Id;
                        _ktResourcesSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktResourcesID);

                        ////Load ktResource data to business object.
                        this._workSheetktResources.LoadktResources(_ktResourcesSheet.Worksheet, _sharedStrings);

                        ////Reference to Excel Worksheet with ktResourceTranslation data.
                        _ktResourceTranslationID = _workSheets.First(s => s.Name == this._workSheetktResourceTranslation.SheetName).Id;
                        _ktResourceTranslationSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktResourceTranslationID);

                        ////Load ktResouceTranslation data to business object.
                        this._workSheetktResourceTranslation.LoadktResourceTranslation(_ktResourceTranslationSheet.Worksheet, _sharedStrings);


                        ////Reference to Excel Worksheet with ktResource data.
                        _ktResourceTypeID = _workSheets.First(s => s.Name == this._workSheetktResourceType.SheetName).Id;
                        _ktResourceTypeSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktResourceTypeID);

                        ////Load ktResource data to business object.
                        this._workSheetktResourceType.LoadktResourceType(_ktResourceTypeSheet.Worksheet, _sharedStrings);

                        ////Reference to Excel Worksheet with ktUIPageType data.
                        _ktUiPageTypeID = _workSheets.First(s => s.Name == this._workSheetktUIPageType.SheetName).Id;
                        _ktUiPageTypeSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_ktUiPageTypeID);

                        ////Load ktResource data to business object.
                        this._workSheetktUIPageType.LoadUIPageType(_ktUiPageTypeSheet.Worksheet, _sharedStrings);

                        ////Reference to Excel Worksheet with QAGroups data.
                        _qaGroupsID = _workSheets.First(s => s.Name == this._workSheetQAGroups.SheetName).Id;
                        _qaGroupsSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_qaGroupsID);

                        ////Load QAGroups data to business object.
                        this._workSheetQAGroups.LoadQAGroups(_qaGroupsSheet.Worksheet, _sharedStrings);

                        ////Reference to Excel Worksheet with QAGroups data.
                        _qAktUiDesignID = _workSheets.First(s => s.Name == this._workSheetQAktUIDesign.SheetName).Id;
                        _qAktUiDesignSheet = (WorksheetPart)document.WorkbookPart.GetPartById(_qAktUiDesignID);

                        ////Load QAGroups data to business object.
                        this._workSheetQAktUIDesign.LoadQAktUIDesign(_qAktUiDesignSheet.Worksheet, _sharedStrings);

                        if (this._workSheetktExaminedGroup.LoadExaminedGroup(_ktExaminedGroupSheet.Worksheet, _sharedStrings) &&
                            this._workSheetktUIGroupOrder.LoadUIGroupOrder(_ktUiGroupOrderSheet.Worksheet, _sharedStrings) &&
                            this._workSheetUIDesign.LoadUIDesign(_ktUiDesignSheet.Worksheet, _sharedStrings)
                            // +++
                            )
                        {
                            _workSheetktExaminedGroup.AcceptChanges();
                            _workSheetUIDesign.AcceptChanges();
                            _workSheetktUIGroupOrder.AcceptChanges();
                        }
                    }
                }
            }

            if (!FileTypeOk)
            {
                ImportFileOK = false;
                MessageBox.Show("This file is not an excel file with the correct (.xlsx) file extension",
                    "Wrong file type", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!SheetNameOk)
            {
                ImportFileOK = false;
                MessageBox.Show("The configuration excel file does not contain the correct excel sheets",
                    "Wrong excel file", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!_workSheetktExaminedGroup.ColumnHeadersOk &&
                !_workSheetktUIGroupOrder.ColumnHeadersOk &&
                !_workSheetUIDesign.ColumnHeadersOk
                // +++
                )
            {
                ImportFileOK = false;
                MessageBox.Show("One or all of the excel sheet does not have the right column headers",
                    "Wrong column names", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!_workSheetktExaminedGroup.DataOnSheetOk &&
                !_workSheetktUIGroupOrder.DataOnSheetOk &&
                !_workSheetUIDesign.DataOnSheetOk
                // +++
                )
            {
                ImportFileOK = false;
                MessageBox.Show("One or all of the excel sheets does not have any data",
                   "No data on sheet", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static ImportExcel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ImportExcel();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Checks if the filetype is in an ".xlsx" excel file format
        /// </summary>
        /// <param name="path">Path to the excel file</param>
        /// <returns></returns>
        public bool CheckFileType(string path)
        {
            if (path != "")
            {
                string fileExt = System.IO.Path.GetExtension(path);

                if (fileExt.Equals(".xlsx"))
                {
                    return true;
                }
            }
            FileTypeOk = false;
            return false;
        }

        /// <summary>
        /// Check if the excel file contains the correct sheetnames
        /// </summary>
        /// <param name="path">Path to the excel file</param>
        /// <returns></returns>
        public bool CheckSheetNames(string path)
        {
            using (SpreadsheetDocument document =
                SpreadsheetDocument.Open(path, true))
            {
                //References to the workbook
                _workBook = document.WorkbookPart.Workbook;
                _workSheets = _workBook.Descendants<Sheet>();

                if (_workSheets.Any(x => x.Name == this._workSheetktExaminedGroup.SheetName ||
                                         x.Name == this._workSheetUIDesign.SheetName ||
                                         x.Name == this._workSheetktUIFieldIncludedType.SheetName ||
                                         x.Name == this._workSheetktUIGroupOrder.SheetName ||
                                         x.Name == this._workSheetktUIOrder.SheetName ||
                                         x.Name == this._workSheetktResources.SheetName ||
                                         x.Name == this._workSheetktResourceTranslation.SheetName ||
                                         x.Name == this._workSheetktResourceType.SheetName ||
                                         x.Name == this._workSheetktUIPageType.SheetName ||
                                         x.Name == this._workSheetQAGroups.SheetName ||
                                         x.Name == this._workSheetQAktUIDesign.SheetName))
                {
                    return true;
                }
                if (_workSheets.Any(x => x.Name == this._workSheetktExaminedGroup.SheetName ||
                                         x.Name == this._workSheetktUIGroupOrder.SheetName ||
                                         x.Name == this._workSheetktUIOrder.SheetName ||
                                         x.Name == this._workSheetktResources.SheetName ||
                                         x.Name == this._workSheetktResourceTranslation.SheetName))
                {
                    return true;
                }
                SheetNameOk = false;
                return false;
            }
        }
    }
}
