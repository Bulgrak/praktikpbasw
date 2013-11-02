using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
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
        private WorkspaceViewModel wvm;
        private GroupTableViewModel gtvm;

        [TestInitialize()]
        public void Initialize()
        {
            ifvm = ItemFilterViewModel.Instance;
            glvm = GroupListViewModel.Instance;
            wvm = WorkspaceViewModel.Instance;
            gtvm = new GroupTableViewModel();
            

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
        /// Danish LanguageID = 2
        /// English LanguageID = 1
        /// </summary>
        [TestMethod]
        //[UNTP-00x]
        public void ToolboxItemsFilterList()
        {
            ToolboxItem tbi = new ToolboxItem();
            ItemType it = new ItemType();
            it.DesignID = "123";
            it.DanishTranslationToolTip = "ToolTip - Dette er en test";
            it.EnglishTranslationToolTip = "ToolTip - This is a test";
            it.DanishTranslationText = "Dette er en test";
            it.EnglishTranslationText = "This is a test";
            it.LanguageID = "2";
            tbi.ItemType = it;

            ToolboxItem tbi2 = new ToolboxItem();
            ItemType it2 = new ItemType();
            it2.DesignID = "123";
            it2.DanishTranslationToolTip = "ToolTip - Dette er endnu en test";
            it2.EnglishTranslationToolTip = "ToolTip - This is another test";
            it2.DanishTranslationText = "Header - Dette er endnu en test";
            it2.EnglishTranslationText = "Header - This is another test";
            it2.LanguageID = "2";
            tbi2.ItemType = it2;

            ifvm.ToolboxItemList.Add(tbi);
            ifvm.ToolboxItemList.Add(tbi2);
            ifvm.LanguageID = "2";
            ifvm.SetupToolBoxItemCollectionView();
            ifvm.FilterString = "endnu";
            Assert.IsTrue(ifvm.DesignItemsView.Contains(tbi2));
            Assert.IsFalse(ifvm.DesignItemsView.Contains(tbi));
        }

        /// <summary>
        /// Checks if the filter takes the group name into account
        /// Danish LanguageID = 2
        /// English LanguageID = 1
        /// </summary>
        [TestMethod]
        //[SRS-001]
        public void ToolboxGroupsFilterList()
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

        //[TestMethod]
        //public void DragAndDropToolboxItemToWorkspace()
        //{
        //    ifvm.PopulateToolbox();
        //    GroupType gt = wvm.PageList[14].GroupTypeOrders[1].Group;
        //    //gtvm.Group = gt;
        //    List<ToolboxItem> tbiList = ifvm.DesignItemsView.Cast<ToolboxItem>().ToList();
        //    ToolboxItem tbi = tbiList.Find(x => x.ItemType.DesignID.Equals("38")); //ItemType-Name: Temperatur
        //    //List<ItemTypeOrder> deepCopyItemTypeOrderList = new List<ItemTypeOrder>();

        //    //foreach (ItemTypeOrder ito in gt.ItemOrder)
        //    //{
        //    //    ItemTypeOrder clonedito = new ItemTypeOrder();
        //    //    clonedito.DesignID = ito.DesignID;
        //    //    clonedito.GroupTypeID = ito.GroupTypeID;
        //    //    clonedito.IncludedTypeID = ito.IncludedTypeID;
        //    //    clonedito.Item = ito.Item;
        //    //    clonedito.ItemOrder = ito.ItemOrder;
        //    //    deepCopyItemTypeOrderList.Add(clonedito);
        //    //}

            
        //    int l = 0;
        //    while (l < gt.ItemOrder.Count)
        //    {

        //        ObservableCollection<ItemTypeOrder> deepCopyItemTypeOrderList = new ObservableCollection<ItemTypeOrder>();
        //        GroupType deepCopyGroupType = new GroupType();
        //        deepCopyGroupType.DanishTranslationText = gt.DanishTranslationText;
        //        deepCopyGroupType.EnglishTranslationText = gt.EnglishTranslationText;
        //        deepCopyGroupType.GroupHeader = gt.GroupHeader;
        //        deepCopyGroupType.GroupTypeID = gt.GroupTypeID;
        //        deepCopyGroupType.ItemOrder = gt.ItemOrder; 
        //        deepCopyGroupType.LanguageID = gt.LanguageID;
        //        deepCopyGroupType.ResourceID = gt.ResourceID;
        //        deepCopyGroupType.ResourceType = gt.ResourceType;
        //        deepCopyGroupType.ResourceTypeID = gt.ResourceTypeID;

        //        foreach (ItemTypeOrder ito in gt.ItemOrder)
        //        {
        //            ItemTypeOrder clonedito = new ItemTypeOrder();
        //            clonedito.DesignID = ito.DesignID;
        //            clonedito.GroupTypeID = ito.GroupTypeID;
        //            clonedito.IncludedTypeID = ito.IncludedTypeID;
        //            clonedito.Item = ito.Item;
        //            clonedito.ItemOrder = ito.ItemOrder;
        //            deepCopyItemTypeOrderList.Add(clonedito);
        //        }
        //        deepCopyGroupType.ItemOrder = deepCopyItemTypeOrderList;
        //        gtvm.Group = deepCopyGroupType;
        //        gtvm.AdjustItemOrder(gtvm.Group);
        //        ItemTypeOrder dropTargetItemTypeOrder = deepCopyGroupType.ItemOrder[l];

        //        List<ItemTypeOrder> changedItemTypeOrderList = new List<ItemTypeOrder>();
        //        foreach (ItemTypeOrder ito in gt.ItemOrder)
        //        {
        //            ItemTypeOrder clonedito = new ItemTypeOrder();
        //            clonedito.DesignID = ito.DesignID;
        //            clonedito.GroupTypeID = ito.GroupTypeID;
        //            clonedito.IncludedTypeID = ito.IncludedTypeID;
        //            clonedito.Item = ito.Item;
        //            clonedito.ItemOrder = ito.ItemOrder;
        //            changedItemTypeOrderList.Add(clonedito);
        //        }
                
                
        //        gtvm.HandleToolboxItemDrop(deepCopyGroupType, tbi, dropTargetItemTypeOrder);

                

        //        int i = deepCopyGroupType.ItemOrder.IndexOf(dropTargetItemTypeOrder);
        //        while (i < changedItemTypeOrderList.Count)
        //        {
        //            Assert.AreEqual(deepCopyItemTypeOrderList[i+1].ItemOrder,
        //                changedItemTypeOrderList[i].ItemOrder + 1); //Check if ItemOrders are incremented by 1
        //            Assert.AreEqual(deepCopyItemTypeOrderList[i+1].Item, changedItemTypeOrderList[i].Item);
        //            i++;
        //        }
        //        Assert.AreEqual(deepCopyItemTypeOrderList[l].Item.DesignID, tbi.ItemType.DesignID); //Check if dropped ItemType is inserted at the right location
        //        l++;
        //    }
        //    //måske tjekke hvad der ikke ændrede state
            
        //}

        [TestMethod]
        public void DragAndDropToolboxItemToWorkspace()
        {
            ifvm.PopulateToolbox();
            GroupType gt = wvm.PageList[14].GroupTypeOrders[0].Group;
            List<ToolboxItem> tbiList = ifvm.DesignItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbi = tbiList.Find(x => x.ItemType.DesignID.Equals("38")); //ItemType-Name: Temperatur
            ObservableCollection<ItemTypeOrder> deepCopyItemTypeOrderList = new ObservableCollection<ItemTypeOrder>();
            foreach (ItemTypeOrder ito in gt.ItemOrder)
            {
                ItemTypeOrder clonedito = new ItemTypeOrder();
                clonedito.DesignID = ito.DesignID;
                clonedito.GroupTypeID = ito.GroupTypeID;
                clonedito.IncludedTypeID = ito.IncludedTypeID;
                clonedito.Item = ito.Item;
                clonedito.ItemOrder = ito.ItemOrder;
                deepCopyItemTypeOrderList.Add(clonedito);
            }

                deepCopyGroupType.ItemOrder = deepCopyItemTypeOrderList;
                gtvm.Group = deepCopyGroupType;
                gtvm.AdjustItemOrder(gtvm.Group);
                ItemTypeOrder dropTargetItemTypeOrder = deepCopyGroupType.ItemOrder[l];

                List<ItemTypeOrder> changedItemTypeOrderList = new List<ItemTypeOrder>();

                gtvm.HandleToolboxItemDrop(deepCopyGroupType, tbi, dropTargetItemTypeOrder);



                int i = deepCopyGroupType.ItemOrder.IndexOf(dropTargetItemTypeOrder);
                while (i < changedItemTypeOrderList.Count)
                {
                    Assert.AreEqual(deepCopyItemTypeOrderList[i + 1].ItemOrder,
                        changedItemTypeOrderList[i].ItemOrder + 1); //Check if ItemOrders are incremented by 1
                    Assert.AreEqual(deepCopyItemTypeOrderList[i + 1].Item, changedItemTypeOrderList[i].Item);
                    i++;
                }
                Assert.AreEqual(deepCopyItemTypeOrderList[l].Item.DesignID, tbi.ItemType.DesignID); //Check if dropped ItemType is inserted at the right location
        }



        [TestMethod]
        public void DragAndDropToolboxSpecialItemToWorkspace()
        {
            
        }

        [TestMethod]
        public void DragAndDropToolboxGroupToWorkspace()
        {
            
        }

        [TestMethod]
        public void DragAndDropBetweenItemsInAGroupInWorkSpace()
        {
            
        }

        [TestMethod]
        public void DragAndDropBetweenGroupsInWorkSpace()
        {
            
        }

        [TestMethod]
        public void DragAndDropBetweenGroupsWithMultipleDepartmentsInWorkSpace()
        {

        }
    }
}
