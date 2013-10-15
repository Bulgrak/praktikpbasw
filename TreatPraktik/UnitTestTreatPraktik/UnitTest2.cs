using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            //_importPath = @"C:\UITablesToStud.xlsx";

            //_wvm = WorkspaceViewModel.Instance;
            //_wvm.LoadWorkspace(_importPath);
            //_impExcel = ImportExcel.Instance;
            //_exExcel = TreatPraktik.ViewModel.ExportExcel.Instance;

            //WorkspaceUserControl wpuc = new WorkspaceUserControl();
            ObservableCollection<GroupTypeOrder> groups = new ObservableCollection<GroupTypeOrder>();
            GroupTypeOrder gto = new GroupTypeOrder();
            gto.DepartmentID = "1";
            gto.GroupOrder = 1;
            gto.GroupTypeID = "1";
            gto.PageTypeID = "15";
            GroupType gt = new GroupType();
            gt.GroupHeader = "Gert";
            gt.GroupTypeID = "1";
            gt.LanguageID = "1";
            gt.ResourceID = "1";
            gt.ResourceType = "1";
            gt.ResourceTypeID = "1";


            

            Group group = new Group();
            group.PopulateGroupTable();
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
