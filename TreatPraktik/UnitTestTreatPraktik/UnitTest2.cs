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
        private SpecialFieldsViewModel sfvm;

        [TestInitialize()]
        public void Initialize()
        {
            ifvm = ItemFilterViewModel.Instance;
            glvm = GroupListViewModel.Instance;
            wvm = WorkspaceViewModel.Instance;
            sfvm = new SpecialFieldsViewModel();
            gtvm = new GroupTableViewModel();
            
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

        [TestMethod]
        public void DragAndDropToolboxItemToWorkspace()
        {
            ifvm.PopulateToolbox();
            GroupType gt = wvm.PageList[14].GroupTypeOrders[1].Group;
            List<ToolboxItem> tbiList = ifvm.DesignItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbi = tbiList.Find(x => x.ItemType.DesignID.Equals("38")); //ItemType-Name: Temperatur
            int l = 0;
            while (l < gt.ItemOrder.Count)
            {
                GroupType deepCopyGroupType = DeepCopyGroupType(gt);
                ObservableCollection<ItemTypeOrder> modifiedItemTypeOrderList = deepCopyGroupType.ItemOrder;
                gtvm.Group = deepCopyGroupType;
                gtvm.AdjustItemOrder(gtvm.Group);
                ItemTypeOrder dropTargetItemTypeOrder = deepCopyGroupType.ItemOrder[l];
                List<ItemTypeOrder> unmodifiedItemTypeOrderList = gt.ItemOrder.ToList();
                int i = deepCopyGroupType.ItemOrder.IndexOf(dropTargetItemTypeOrder);
                gtvm.HandleToolboxItemDrop(deepCopyGroupType, tbi, dropTargetItemTypeOrder);
                int skipped = 0;
                while (i < unmodifiedItemTypeOrderList.Count - skipped)
                {
                    Assert.AreEqual(modifiedItemTypeOrderList[i + 1].ItemOrder,
                        unmodifiedItemTypeOrderList[i].ItemOrder + 1); //For each itemtype, check if ItemOrder is incremented by 1
                    if (!unmodifiedItemTypeOrderList[i].DesignID.Equals("198"))
                        Assert.AreEqual(modifiedItemTypeOrderList[i + 1].Item, unmodifiedItemTypeOrderList[i + skipped].Item);
                    else
                    {
                        skipped++;
                        Assert.AreEqual(modifiedItemTypeOrderList[i + 1].Item, unmodifiedItemTypeOrderList[i + skipped].Item);
                    }
                    i++;
                }
                Assert.AreEqual(modifiedItemTypeOrderList[l].Item.DesignID, tbi.ItemType.DesignID); //Check if dropped ItemType is inserted at the right location
                l++;
            }
            //måske tjekke hvad der ikke ændrede state

        }

        [TestMethod]
        public void DragAndDropToolboxSpecialItemEmptyFieldToWorkspace()
        {
            ifvm.PopulateToolbox();
            GroupType gt = wvm.PageList[14].GroupTypeOrders[1].Group;
            List<ToolboxItem> tbsiList = sfvm.SpecialItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbsi = tbsiList.Find(x => x.ItemType.DesignID.Equals("197"));
            int l = 0;
            while (l < gt.ItemOrder.Count)
            {
                GroupType deepCopyGroupType = DeepCopyGroupType(gt);
                ObservableCollection<ItemTypeOrder> modifiedItemTypeOrderList = deepCopyGroupType.ItemOrder;
                gtvm.Group = deepCopyGroupType;
                gtvm.AdjustItemOrder(gtvm.Group);
                ItemTypeOrder dropTargetItemTypeOrder = deepCopyGroupType.ItemOrder[l];
                List<ItemTypeOrder> unmodifiedItemTypeOrderList = gt.ItemOrder.ToList();
                int i = deepCopyGroupType.ItemOrder.IndexOf(dropTargetItemTypeOrder);
                gtvm.HandleToolboxItemDrop(deepCopyGroupType, tbsi, dropTargetItemTypeOrder);
                int skipped = 0;
                while (i < unmodifiedItemTypeOrderList.Count - skipped)
                {
                    Assert.AreEqual(modifiedItemTypeOrderList[i + 1].ItemOrder,
                        unmodifiedItemTypeOrderList[i].ItemOrder + 1); //For each itemtype, check if ItemOrder is incremented by 1
                    if (!unmodifiedItemTypeOrderList[i].DesignID.Equals("198"))
                        Assert.AreEqual(modifiedItemTypeOrderList[i + 1].Item, unmodifiedItemTypeOrderList[i + skipped].Item);
                    else
                    {
                        skipped++;
                        Assert.AreEqual(modifiedItemTypeOrderList[i + 1].Item, unmodifiedItemTypeOrderList[i + skipped].Item);
                    }
                    i++;
                }
                Assert.AreEqual(modifiedItemTypeOrderList[l].Item.DesignID, tbsi.ItemType.DesignID); //Check if dropped ItemType is inserted at the right location
                l++;
            }
            //måske tjekke hvad der ikke ændrede state

        }

        /// <summary>
        /// Creates an exact copy of a grouptype, including its list of ItemTypeOrders
        /// </summary>
        /// <param name="gt"></param>
        /// <returns></returns>
        public GroupType DeepCopyGroupType(GroupType gt)
        {
            GroupType deepCopyGroupType = new GroupType();
            deepCopyGroupType.DanishTranslationText = gt.DanishTranslationText;
            deepCopyGroupType.EnglishTranslationText = gt.EnglishTranslationText;
            deepCopyGroupType.GroupHeader = gt.GroupHeader;
            deepCopyGroupType.GroupTypeID = gt.GroupTypeID;
            deepCopyGroupType.ItemOrder = gt.ItemOrder;
            deepCopyGroupType.LanguageID = gt.LanguageID;
            deepCopyGroupType.ResourceID = gt.ResourceID;
            deepCopyGroupType.ResourceType = gt.ResourceType;
            deepCopyGroupType.ResourceTypeID = gt.ResourceTypeID;
            ObservableCollection<ItemTypeOrder> deepCopyItemTypeOrders = new ObservableCollection<ItemTypeOrder>(); 
            foreach (ItemTypeOrder ito in gt.ItemOrder)
            {
                ItemTypeOrder clonedito = new ItemTypeOrder();
                clonedito.DesignID = ito.DesignID;
                clonedito.GroupTypeID = ito.GroupTypeID;
                clonedito.IncludedTypeID = ito.IncludedTypeID;
                clonedito.Item = ito.Item;
                clonedito.ItemOrder = ito.ItemOrder;
                deepCopyItemTypeOrders.Add(clonedito);
            }

            deepCopyGroupType.ItemOrder = deepCopyItemTypeOrders;
            return deepCopyGroupType;
        }

        /// <summary>
        /// A test, which confirms that the drag and drop of an emptyfield, results in the right changes. 
        /// Creates a deep copy of the original list before simulating a drag and drop with an emptyfield.
        /// The original list is then compared with the modified list regarding itemorder and item reference 
        /// </summary>
        [TestMethod]
        public void DragAndDropToolboxSpecialEmptyFieldItemToWorkspace()
        {
            ifvm.PopulateToolbox();
            GroupType gt = wvm.PageList[14].GroupTypeOrders[1].Group;
            List<ToolboxItem> tbiList = sfvm.SpecialItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbi = tbiList.Find(x => x.ItemType.DesignID.Equals("197")); //ItemType-Name: EmptyField
            int l = 0;
            while (l < gt.ItemOrder.Count)
            {
                GroupType deepCopyGroupType = DeepCopyGroupType(gt);
                ObservableCollection<ItemTypeOrder> modifiedItemTypeOrderList = deepCopyGroupType.ItemOrder;
                gtvm.Group = deepCopyGroupType;
                gtvm.AdjustItemOrder(gtvm.Group);
                ItemTypeOrder dropTargetItemTypeOrder = deepCopyGroupType.ItemOrder[l];
                List<ItemTypeOrder> unmodifiedItemTypeList = gt.ItemOrder.ToList();
                int i = deepCopyGroupType.ItemOrder.IndexOf(dropTargetItemTypeOrder);
                gtvm.HandleToolboxItemDrop(deepCopyGroupType, tbi, dropTargetItemTypeOrder);
                int skipped = 0;
                while (i < unmodifiedItemTypeList.Count - skipped)
                {
                    Assert.AreEqual(modifiedItemTypeOrderList[i + 1].ItemOrder,
                        unmodifiedItemTypeList[i].ItemOrder + 1); //For each itemtype, check if ItemOrder is incremented by 1
                    if (!unmodifiedItemTypeList[i].DesignID.Equals("198"))
                        Assert.AreEqual(modifiedItemTypeOrderList[i + 1].Item, unmodifiedItemTypeList[i + skipped].Item);
                    else
                    {
                        skipped++;
                        Assert.AreEqual(modifiedItemTypeOrderList[i + 1].Item, unmodifiedItemTypeList[i + skipped].Item);
                    }
                    i++;
                }
                Assert.AreEqual(modifiedItemTypeOrderList[l].Item.DesignID, tbi.ItemType.DesignID); //Check if dropped ItemType is inserted at the right location
                l++;
            }
        }

        [TestMethod]
        public void DragAndDropToolboxSpecialNewLineItemToWorkspace()
        {
            GroupType gt = wvm.PageList[14].GroupTypeOrders[1].Group;
            List<ToolboxItem> tbiList = sfvm.SpecialItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbi = tbiList.Find(x => x.ItemType.DesignID.Equals("198")); //ItemType-Name: NewLineItem

            int l = 0;
            while (l < gt.ItemOrder.Count)
            {
                GroupType deepCopyGroupType = DeepCopyGroupType(gt);
                ObservableCollection<ItemTypeOrder> modifiedItemTypeOrderList = deepCopyGroupType.ItemOrder;
                gtvm.Group = deepCopyGroupType;
                gtvm.AdjustItemOrder(gtvm.Group);
                ItemTypeOrder dropTargetItemTypeOrder = deepCopyGroupType.ItemOrder[l];
                List<ItemTypeOrder> unmodifiedItemTypeList = gt.ItemOrder.ToList();
                int i = 0;
                gtvm.HandleToolboxItemDrop(deepCopyGroupType, tbi, dropTargetItemTypeOrder);
                int skipped = 0;
                //gt.ItemOrder[i - 1].ItemOrder + (4 - (gt.ItemOrder[i - 1].ItemOrder % 4));
                //while (i < unmodifiedItemTypeList.Count - skipped)
                while (i < 4)
                {
                    Assert.AreEqual(l + skipped, modifiedItemTypeOrderList[i].ItemOrder);
                    if (modifiedItemTypeOrderList[i].DesignID.Equals("198"))
                        skipped = skipped + (int)(4 - (modifiedItemTypeOrderList[i].ItemOrder % 4));
                    i++;
                }
                //Assert.AreEqual(modifiedItemTypeOrderList[l].Item.DesignID, tbi.ItemType.DesignID); //Check if dropped ItemType is inserted at the right location
                l++;
            }
        }

        [TestMethod]
        public void DragAndDropToolboxToNullField()
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



        /// <summary>
        /// Case 20
        /// </summary>
        [TestMethod]
        public void DragAndDropInsertGroupInWorkSpaceByDropZone()
        {
            glvm.PopulateGTList();
            List<ToolboxGroup> tbgList = glvm.DesignItemsView.Cast<ToolboxGroup>().ToList();
            ToolboxGroup tbg = tbgList.Find(x => x.Group.GroupTypeID.Equals("16"));
            ObservableCollection<GroupTypeOrder> gtoList = wvm.PageList[14].GroupTypeOrders; // page 15
            gtvm.GroupTypeOrderCollection = gtoList;
            gtvm.AdjustGroupOrder();

            GroupType gt = tbg.Group;
            gtvm.InsertGroupLast(gt, "15");
            Assert.AreEqual(gtoList.Last().Group, tbg.Group);
            Assert.AreEqual(gtoList.Last().GroupOrder, gtoList.Count);
        }

        /// <summary>
        /// Case 19 - A.GroupOrder < B.GroupOrder
        /// </summary>
        [TestMethod]
        public void DragAndDropBetweenGroupsInWorkSpaceBA()
        {
            ObservableCollection<GroupTypeOrder> gtoList = wvm.PageList[14].GroupTypeOrders; // page 15
            GroupTypeOrder targetGroupTypeOrder = wvm.PageList[14].GroupTypeOrders[0]; //drop target = first group
            GroupTypeOrder draggedGroupTypeOrder = wvm.PageList[14].GroupTypeOrders[3]; //dragged group = last group
            gtvm.GroupTypeOrderCollection = gtoList;
            gtvm.AdjustGroupOrder();
            ObservableCollection<GroupTypeOrder> unmodifiedgtoList = DeepCopyGroupTypeOrderList(gtoList);
            
            
            gtvm.HandleGroupTableDrop(targetGroupTypeOrder, draggedGroupTypeOrder);
            Assert.AreEqual(gtoList[0], draggedGroupTypeOrder);
            int i = gtoList.IndexOf(targetGroupTypeOrder);
            while (i < unmodifiedgtoList.Count)
            {
                Assert.AreEqual(unmodifiedgtoList[i - 1].Group, gtoList[i].Group);
                Assert.AreEqual(unmodifiedgtoList[i].GroupOrder, i + 1);
                Assert.AreEqual(gtoList[i].GroupOrder, i + 1);
                i++;
            }
        }

        /// <summary>
        /// Case 19 - A.GroupOrder > B.GroupOrder
        /// </summary>
        [TestMethod]
        public void DragAndDropBetweenGroupsInWorkSpaceAB()
        {
            ObservableCollection<GroupTypeOrder> gtoList = wvm.PageList[14].GroupTypeOrders; // page 15
            GroupTypeOrder draggedGroupTypeOrder = wvm.PageList[14].GroupTypeOrders[0]; //drop target = first group
            GroupTypeOrder targetGroupTypeOrder = wvm.PageList[14].GroupTypeOrders[3]; //dragged group = last group
            gtvm.GroupTypeOrderCollection = gtoList;
            gtvm.AdjustGroupOrder();
            ObservableCollection<GroupTypeOrder> unmodifiedgtoList = DeepCopyGroupTypeOrderList(gtoList);


            gtvm.HandleGroupTableDrop(targetGroupTypeOrder, draggedGroupTypeOrder);
            Assert.AreEqual(gtoList.Last(), draggedGroupTypeOrder);
            int i = gtoList.IndexOf(targetGroupTypeOrder);
            int j = gtoList.IndexOf(draggedGroupTypeOrder);
            while (i < j)
            {
                Assert.AreEqual(unmodifiedgtoList[i].Group, gtoList[i - 1].Group);
                Assert.AreEqual(unmodifiedgtoList[i].GroupOrder, i + 1);
                Assert.AreEqual(gtoList[i].GroupOrder, i + 1);
                i++;
            }
        }

        /// <summary>
        /// Creates a copy of GroupTypeOrderList. GroupTypeOrder are deep copied and GroupType are referenced
        /// </summary>
        /// <param name="gtoList"></param>
        /// <returns></returns>
        public ObservableCollection<GroupTypeOrder> DeepCopyGroupTypeOrderList(ObservableCollection<GroupTypeOrder> gtoList)
        {
            ObservableCollection<GroupTypeOrder> deepCopyGroupTypeOrders = new ObservableCollection<GroupTypeOrder>();
            foreach (GroupTypeOrder gto in gtoList)
            {
                GroupTypeOrder clonedgto = new GroupTypeOrder();
                clonedgto.DepartmentID = gto.DepartmentID;
                clonedgto.Group = gto.Group;
                clonedgto.GroupOrder = gto.GroupOrder;
                clonedgto.GroupTypeID = gto.GroupTypeID;
                clonedgto.PageTypeID = gto.PageTypeID;
                deepCopyGroupTypeOrders.Add(clonedgto);
            }
            return deepCopyGroupTypeOrders;
        }

        /// <summary>
        /// Case 4
        /// </summary>
        [TestMethod]
        public void DropOnRowContainingNewLineItem()
        {
            try
            {
                List<ToolboxItem> tbsiList = sfvm.SpecialItemsView.Cast<ToolboxItem>().ToList();
                ToolboxItem tbsi = tbsiList.Find(x => x.ItemType.DesignID.Equals("198"));
                ObservableCollection<GroupTypeOrder> gtoList = wvm.PageList[14].GroupTypeOrders;
                GroupType gt = gtoList[1].Group;
                ItemTypeOrder dropTargetItemTypeOrder = gt.ItemOrder[4];
                gtvm.GroupTypeOrderCollection = gtoList;
                gtvm.Group = gt;
                gtvm.HandleToolboxItemDrop(gt, tbsi, dropTargetItemTypeOrder);
                Assert.Fail("Should throw an exception");
            }
            catch (Exception e)
            {
                Assert.AreEqual("The row already contains a <NewLineItem>", e.Message);
            }
        }

        /// <summary>
        /// Case 19
        /// </summary>
        [TestMethod]
        public void DragAndDropInsertNewExistingGroupInWorkSpace()
        {
            glvm.PopulateGTList();
            List<ToolboxGroup> tbgList = glvm.DesignItemsView.Cast<ToolboxGroup>().ToList();
            ToolboxGroup tbg = tbgList.Find(x => x.Group.GroupTypeID.Equals("16"));
            ObservableCollection<GroupTypeOrder> gtoList = wvm.PageList[14].GroupTypeOrders; // page 15
            ObservableCollection<GroupTypeOrder> unmodifiedgtoList = DeepCopyGroupTypeOrderList(gtoList); 
            GroupTypeOrder targetGroupTypeOrder = gtoList[2];
            gtvm.GroupTypeOrderCollection = gtoList;
            gtvm.AdjustGroupOrder();
            gtvm.InsertGroup(targetGroupTypeOrder, tbg.Group);
            int targetIndex = gtoList.IndexOf(targetGroupTypeOrder);
            Assert.AreEqual(targetIndex, 3);
            int i = targetIndex + 1;
            while (i < unmodifiedgtoList.Count)
            {
                Assert.AreEqual(unmodifiedgtoList[i].Group, gtoList[i - 1].Group);
                Assert.AreEqual(unmodifiedgtoList[i].GroupOrder + 1, gtoList[i - 1].GroupOrder);
                i++;
            }
        }

        /// <summary>
        /// Case 2
        /// </summary>
        [TestMethod]
        public void DropToolboxItemOnEmptyField()
        {
            ifvm.PopulateToolbox();
            GroupType gt = wvm.PageList[14].GroupTypeOrders[0].Group;
            List<ToolboxItem> tbiList = ifvm.DesignItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbi = tbiList.Find(x => x.ItemType.DesignID.Equals("38")); //ItemType-Name: Temperatur
            List<ToolboxItem> tbsiList = sfvm.SpecialItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbsi = tbsiList.Find(x => x.ItemType.DesignID.Equals("197"));
            ItemTypeOrder dropTargetItemTypeOrder = new ItemTypeOrder()
            {
                DesignID = null,
                GroupTypeID = null,
                IncludedTypeID = null,
                Item = null,
                ItemOrder = gt.ItemOrder.Count()
            };
            gtvm.HandleToolboxItemDrop(gt, tbi, dropTargetItemTypeOrder);
            Assert.AreEqual(gt.ItemOrder[gt.ItemOrder.Count() - 1].Item.DesignID, tbi.ItemType.DesignID);
        }

        /// <summary>
        /// Case 17
        /// </summary>
        [TestMethod]
        public void DropToolboxItemOnNullField()
        {
            ifvm.PopulateToolbox();
            GroupType gt = wvm.PageList[14].GroupTypeOrders[0].Group;
            List<ToolboxItem> tbiList = ifvm.DesignItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbi = tbiList.Find(x => x.ItemType.DesignID.Equals("38")); //ItemType-Name: Temperatur
            ItemTypeOrder dropTargetItemTypeOrder = new ItemTypeOrder()
            {
                DesignID = null,
                GroupTypeID = null,
                IncludedTypeID = null,
                Item = null,
                ItemOrder = 7
            };
            int lastIndex = gt.ItemOrder.Count - 1;
            gtvm.HandleToolboxItemDrop(gt, tbi, dropTargetItemTypeOrder);
            Assert.AreEqual(gt.ItemOrder[7].DesignID, tbi.ItemType.DesignID);
            int i = lastIndex + 1;
            while (i < gt.ItemOrder.Count - 1)
            {
                Assert.AreEqual(gt.ItemOrder[i].DesignID, "197");
                i++;
            }
        }

        /// <summary>
        /// Case 17
        /// </summary>
        [TestMethod]
        public void DropToolboxSpecialItemOnNullField()
        {
            ifvm.PopulateToolbox();
            GroupType gt = wvm.PageList[14].GroupTypeOrders[0].Group;
            List<ToolboxItem> tbsiList = sfvm.SpecialItemsView.Cast<ToolboxItem>().ToList();
            ToolboxItem tbsi = tbsiList.Find(x => x.ItemType.DesignID.Equals("197"));
            ItemTypeOrder dropTargetItemTypeOrder = new ItemTypeOrder()
            {
                DesignID = null,
                GroupTypeID = null,
                IncludedTypeID = null,
                Item = null,
                ItemOrder = 7
            };
            int lastIndex = gt.ItemOrder.Count - 1;
            gtvm.HandleToolboxItemDrop(gt, tbsi, dropTargetItemTypeOrder);
            Assert.AreEqual(gt.ItemOrder[7].DesignID, tbsi.ItemType.DesignID);
            int i = lastIndex + 1;
            while (i < gt.ItemOrder.Count - 1)
            {
                Assert.AreEqual(gt.ItemOrder[i].DesignID, "197");
                i++;
            }
        }

        [TestMethod]
        public void DragAndDropBetweenStandardItems()
        {
            ObservableCollection<GroupTypeOrder> gtoList = wvm.PageList[14].GroupTypeOrders;
            GroupType gt = wvm.PageList[14].GroupTypeOrders[0].Group;
            gtvm.GroupTypeOrderCollection = gtoList;
            ItemTypeOrder draggedItemTypeOrder = gt.ItemOrder[1];
            ItemTypeOrder targetItemTypeOrder = gt.ItemOrder[3];
            // Need more code
        }

        [TestMethod]
        public void DragAndDropBetweenGroupsWithMultipleDepartmentsInWorkSpace()
        {

        }
    }
}
