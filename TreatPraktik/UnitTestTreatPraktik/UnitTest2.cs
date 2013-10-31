using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.View;
using TreatPraktik.ViewModel;

namespace UnitTestTreatPraktik
{
    [TestClass]
    public class UnitTest2
    {
        private ItemFilterViewModel ifvm;
        private GroupListViewModel glvm;

        [TestInitialize()]
        public void Initialize()
        {
            ifvm = ItemFilterViewModel.Instance;
            glvm = GroupListViewModel.Instance;
            //_importPath = @"C:\UITablesToStud.xlsx";

            //_wvm = WorkspaceViewModel.Instance;
            //_wvm.LoadWorkspace(_importPath);
            //_impExcel = ImportExcel.Instance;
            //_exExcel = TreatPraktik.ViewModel.ExportExcel.Instance;

            //WorkspaceUserControl wpuc = new WorkspaceUserControl();
            //ObservableCollection<GroupTypeOrder> groups = new ObservableCollection<GroupTypeOrder>();
            //GroupTypeOrder gto = new GroupTypeOrder();
            //gto.DepartmentID = "1";
            //gto.GroupOrder = 1;
            //gto.GroupTypeID = "1";
            //gto.PageTypeID = "15";
            //GroupType gt = new GroupType();
            //gt.GroupHeader = "Gert";
            //gt.GroupTypeID = "1";
            //gt.LanguageID = "1";
            //gt.ResourceID = "1";
            //gt.ResourceType = "1";
            //gt.ResourceTypeID = "1";




            //GroupContainerUserControl groupContainerUserControl = new GroupContainerUserControl();
            //groupContainerUserControl.PopulateGroupTable();
        }


        /// <summary>
        /// Checks if the filter takes header and tooltip into account
        /// </summary>
        [TestMethod]
        //[UNTP-00x]
        public void FilterListToolboxItemsTest()
        {
            ToolboxItem tbi = new ToolboxItem();
            ItemType it = new ItemType();
            it.DanishTranslationToolTip = "ToolTip - Dette er en test";
            it.EnglishTranslationToolTip = "ToolTip - This is a test";
            it.DanishTranslationText = "Dette er en test";
            it.EnglishTranslationText = "This is a test";
            it.LanguageID = "2";
            tbi.ItemType = it;

            ToolboxItem tbi2 = new ToolboxItem();
            ItemType it2 = new ItemType();
            it2.DanishTranslationToolTip = "ToolTip - This is another test";
            it2.EnglishTranslationToolTip = "ToolTip - This is another test";
            it2.DanishTranslationText = "Header - Dette er endnu en test";
            it2.EnglishTranslationText = "Header - This is another test";
            it2.LanguageID = "2";
            tbi2.ItemType = it2;

            ifvm.ToolboxItemList.Add(tbi);
            ifvm.ToolboxItemList.Add(tbi2);
            ifvm.LanguageID = "2";
            ifvm.SetupToolBoxItemCollectionView();
            ifvm.FilterString = "another";
            Assert.IsTrue(ifvm.DesignItemsView.Contains(tbi2));
            Assert.IsFalse(ifvm.DesignItemsView.Contains(tbi));
        }

        /// <summary>
        /// Checks if the filter takes the group name into account
        /// </summary>
        [TestMethod]
        //[SRS-001]
        public void FilterListToolboxGroupsTest()
        {
            GroupType gt = new GroupType();
            gt.DanishTranslationText = "Øre, næse og hals";
            gt.EnglishTranslationText = "Ear, nose and throat";
            gt.GroupTypeID = "10";
            gt.ItemOrder = null;
            gt.LanguageID = "2";
            gt.ResourceID = "";
            gt.ResourceType = null;
            gt.ResourceTypeID = null;
            ToolboxGroup tbGroup = new ToolboxGroup();
            tbGroup.Group = gt;

            
            GroupType gt2 = new GroupType();
            gt2.DanishTranslationText = "Risikofaktorer for infektion";
            gt2.EnglishTranslationText = "Risk Factors for infection";
            gt2.GroupTypeID = "10";
            gt2.ItemOrder = null;
            gt2.LanguageID = "2";
            gt2.ResourceID = "";
            gt2.ResourceType = null;
            gt2.ResourceTypeID = null;
            ToolboxGroup tbGroup2 = new ToolboxGroup();
            tbGroup2.Group = gt2;

            glvm.GTList.Add(tbGroup);
            glvm.GTList.Add(tbGroup2);
            glvm.LanguageID = "2";
            glvm.SetupToolBoxItemCollectionView();
            glvm.FilterString = "Risk Factors";
            //ifvm.DesignItemsView = CollectionViewSource.GetDefaultView(ifvm.ToolboxItemList);
            Assert.IsTrue(glvm.DesignItemsView.Contains(tbGroup2));
            Assert.IsFalse(glvm.DesignItemsView.Contains(tbGroup));
        }
    }
}
