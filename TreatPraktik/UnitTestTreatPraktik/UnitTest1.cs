using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace UnitTestTreatPraktik
{
    [TestClass]
    public class ExportExcel
    {
        private WorkspaceViewModel _wvm;
        private ImportExcel _impExcel;
        private TreatPraktik.ViewModel.ExportExcel _exExcel;

        private List<ktExaminedGroup> _oldExGroup;
        private List<ktUIGroupOrder> _oldGroupOrder;
        private List<ktUIOrder> _oldOrder;
        private List<ktResources> _oldResources;
        private List<ktResourceTranslation> _oldResTranslation;

        private string _importPath;
        private string _exportPath;

        [TestInitialize()]
        public void Initialize()
        {
            _importPath = @"C:\UITablesToStud.xlsx";

            _wvm = WorkspaceViewModel.Instance;
            _wvm.LoadWorkspace(_importPath);
            _impExcel = ImportExcel.Instance;
            _exExcel = TreatPraktik.ViewModel.ExportExcel.Instance;

            //Get original lists of worksheets
            _oldExGroup = (from a in _impExcel._workSheetktExaminedGroup.ExaminedGroupList 
                           select new ktExaminedGroup
                           {
                               DataQualityScore = a.DataQualityScore, Expanded = a.Expanded, 
                               GroupExpendable = a.GroupExpendable, GroupIdentifier = a.GroupIdentifier, 
                               GroupType = a.GroupType, ID = a.ID, Name = a.Name, RequiredScore = a.RequiredScore
                           }).ToList();

            _oldGroupOrder = (from a in _impExcel._workSheetktUIGroupOrder.ktUIGroupOrderList
                                  select new ktUIGroupOrder
                                  {
                                      DepartmentID = a.DepartmentID, GroupOrder = a.GroupOrder, 
                                      GroupTypeID = a.GroupTypeID, PageTypeID = a.PageTypeID
                                  }).ToList();

            _oldOrder = (from a in _impExcel._workSheetktUIOrder.ktUIOrderList
                             select new ktUIOrder
                             {
                                 DesignID = a.DesignID, GroupOrder = a.GroupOrder, GroupTypeID = a.GroupTypeID,
                                 IncludedTypeID = a.IncludedTypeID, PageTypeID = a.PageTypeID
                             }).ToList();

            _oldResources = (from a in _impExcel._workSheetktResources.ktResourceList
                                 select new ktResources
                                 {
                                     ResourceID = a.ResourceID, ResourceResxID = a.ResourceResxID,
                                     ResourceTypeID = a.ResourceTypeID
                                 }).ToList();

            _oldResTranslation = (from a in _impExcel._workSheetktResourceTranslation.ktResourceTranslationList
                                      select new ktResourceTranslation
                                      {
                                          LanguageID = a.LanguageID, ResourceID = a.ResourceID, 
                                          TranslationText = a.TranslationText
                                      }).ToList();
        }

        [TestMethod]
        public void IsGroupCreatedInList()
        {
            _wvm.CreateGroup("15", "1", 8.0, "New group", "Ny gruppe");


            _exportPath = @"C:\UnitTest.xlsx";
            _exExcel.CreateNewExcel(_exportPath);

            _impExcel.ImportExcelConfiguration(_exportPath);

            //_workspaceVM.CreateGroup("15", "1", 9.0, "Jesper", "Er sej");

            CollectionAssert.Contains()
        }
    }
}
