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

        [TestInitialize()]
        public void Initialize()
        {
            ifvm = ItemFilterViewModel.Instance;
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

        [TestMethod]
        public void FilterTest()
        {
            ToolboxItem tbi = new ToolboxItem();
            tbi.ToolTip = "This is a test";
            tbi.DanishTranslationText = "Dette er en test";
            tbi.EnglishTranslationText = "This is a test";
            tbi.LanguageID = "2";

            ToolboxItem tbi2 = new ToolboxItem();
            tbi2.ToolTip = "Tooltip - This is another test";
            tbi2.DanishTranslationText = "Header - Dette er endnu en test";
            tbi2.EnglishTranslationText = "Header - This is another test";
            tbi.LanguageID = "2";

            ifvm.ToolboxItemList.Add(tbi);
            ifvm.ToolboxItemList.Add(tbi2);
            ifvm.LanguageID = "2";
            ifvm.SetupToolBoxItemCollectionView();
            ifvm.FilterString = "another";
            //ifvm.DesignItemsView = CollectionViewSource.GetDefaultView(ifvm.ToolboxItemList);
            Assert.IsTrue(ifvm.DesignItemsView.Contains(tbi2));
        }
    }
}
