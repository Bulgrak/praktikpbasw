using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Bibliography;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.View;

namespace TreatPraktik.ViewModel
{
    public class GroupTableViewModel
    {
        public GroupType Group { get; set; }
        public ObservableCollection<GroupTypeOrder> GroupTypeOrderCollection { get; set; }
        public WorkspaceViewModel _wvm;

        public GroupTableViewModel()
        {
            _wvm = WorkspaceViewModel.Instance;
        }

        /// <summary>
        /// Edits group in terms of translation text and departments
        /// </summary>
        /// <param name="gto">the group to be edit</param>
        /// <param name="engTransText">english text</param>
        /// <param name="danTransText">danish text</param>
        /// <param name="departmentList">departments to be shown at</param>
        public void EditGroup(GroupTypeOrder gto, string engTransText, string danTransText, List<string> departmentList)
        {
            //PageType page = (from a in PageList where a.PageTypeID.Equals(pageTypeID) select a).FirstOrDefault();
            //GroupTypeOrder group = (from b in page.GroupTypeOrders where b.GroupTypeID.Equals(groupTypeID) select b).FirstOrDefault();

            gto.Group.DanishTranslationText = danTransText;
            gto.Group.EnglishTranslationText = engTransText;
            RefreshLanguage(gto);
            //if(departmentList.Contains("-1") && departmentList.Count == 1)
            //{
            //    GroupTypeOrder gtoRemoved = GroupTypeOrderCollection.First(x => x.DepartmentID.Equals("-1");
            //}
            //if(GroupTypeOrderCollection.Any<

            foreach (string departmentID in departmentList)
            {
                int i = 0;
                bool departmentIDExist = false;

                while (i < GroupTypeOrderCollection.Count && !departmentIDExist)
                {
                    if (departmentID.Equals(GroupTypeOrderCollection[i].DepartmentID) && GroupTypeOrderCollection[i].GroupTypeID.Equals(gto.GroupTypeID))
                    {
                        departmentIDExist = true;
                    }
                    i++;
                }
                if (!departmentIDExist)
                {
                    GroupTypeOrder clonedGto = new GroupTypeOrder();
                    clonedGto.DepartmentID = departmentID;
                    clonedGto.Group = gto.Group;
                    clonedGto.GroupOrder = gto.GroupOrder;
                    clonedGto.GroupTypeID = gto.GroupTypeID;
                    clonedGto.PageTypeID = gto.PageTypeID;
                    GroupTypeOrderCollection.Add(clonedGto);
                }

                //if (!GroupTypeOrderCollection.Any(x => x.DepartmentID.Equals(departmentID)) && GroupTypeOrderCollection.Any(x => x.GroupTypeID.Equals(gto.GroupTypeID)))
                //{
                //    GroupTypeOrder clonedGto = new GroupTypeOrder();
                //    clonedGto.DepartmentID = departmentID;
                //    clonedGto.Group = gto.Group;
                //    clonedGto.GroupOrder = gto.GroupOrder;
                //    clonedGto.GroupTypeID = gto.GroupTypeID;
                //    clonedGto.PageTypeID = gto.PageTypeID;
                //    GroupTypeOrderCollection.Add(clonedGto);
                //}

            }
            CleanUpRemovedDepartments(gto, departmentList);
            GroupTypeOrderCollection.Sort(gtoItem => gtoItem.GroupOrder);
        }

        /// <summary>
        /// Removes an ItemTypeOrder from a group
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="ito">The ItemTypeOrder to be removed</param>
        public void RemoveItemTypeOrder(GroupType gt, ItemTypeOrder ito)
        {
            gt.ItemOrder.Remove(ito);
            AdjustItemOrder(gt);
        }


        public void RefreshLanguage(GroupTypeOrder gto)
        {
            string languageID = gto.Group.LanguageID;
            gto.Group.LanguageID = languageID;
        }

        /// <summary>
        /// Removes departments which do no exist in the list of departments for a group
        /// </summary>
        /// <param name="gto"></param>
        /// <param name="departmentList"></param>
        public void CleanUpRemovedDepartments(GroupTypeOrder gto, List<string> departmentList)
        {
            int j = 0;
            while (j < GroupTypeOrderCollection.Count)
            {
                GroupTypeOrder gtoItem = GroupTypeOrderCollection[j];
                if (gtoItem.GroupTypeID.Equals(gto.GroupTypeID))
                {
                    //bool remove = true;
                    //int i = 0;
                    //while (i < departmentList.Count && remove)
                    //{
                    //    if (gtoItem.DepartmentID.Equals(departmentList[i]))
                    //    {
                    //        remove = false;
                    //    }
                    //    i++;
                    //}

                    //if (remove)
                    //{
                    //    GroupTypeOrderCollection.Remove(gtoItem);
                    //}
                    if (!departmentList.Any(x => x.Equals(gtoItem.DepartmentID)))
                    {
                        GroupTypeOrderCollection.Remove(gtoItem);
                        j--;
                    }
                }
                j++;
            }
        }

        public void RemoveGroup(GroupTypeOrder gto)
        {
            List<GroupTypeOrder> gtoList = FindDuplicatesOfGroups(gto);
            foreach (GroupTypeOrder groupTypeOrder in gtoList)
            {
                GroupTypeOrderCollection.Remove(groupTypeOrder);
            }
            //RefreshGroupOrder();
            AdjustGroupOrder();
        }

        /// <summary>
        /// Handles drop logic for items from toolbox. Drop logic depends on the type of the dropped item 
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="tbi"></param>
        /// <param name="dropTargetItemTypeOrder"></param>
        public void HandleToolboxItemDrop(GroupType gt, ToolboxItem tbi, ItemTypeOrder dropTargetItemTypeOrder)
        {
            string dropTargetDesignID = dropTargetItemTypeOrder.DesignID;
            if (tbi.ItemType.DesignID.Equals("198") && dropTargetDesignID != null)
            {
                ToolboxSpecialNewLineItemDrop(dropTargetItemTypeOrder, tbi, gt);
            }
            if (dropTargetDesignID != null && !tbi.ItemType.DesignID.Equals("198") && !dropTargetDesignID.Equals("197"))
            {
                ToolboxItemDropOnStandardItem(dropTargetItemTypeOrder, tbi, gt);
            }
            if (dropTargetDesignID == null) //drop on null field
            {
                ToolboxItemDropOnNullField(dropTargetItemTypeOrder, tbi, gt);
            }
            if (dropTargetDesignID != null && dropTargetDesignID.Equals("197") && !tbi.ItemType.DesignID.Equals("198"))
            {
                ToolboxItemDropOnEmptyField(dropTargetItemTypeOrder, tbi, gt);
            }
        }

        /// <summary>
        /// Handles drop logic for items from a group. Drop logic depends on both drop target and dragged item
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="targetItemType"></param>
        /// <param name="draggedItemType"></param>
        public void HandleDropAndDropBetweenItems(GroupType gt, ItemTypeOrder targetItemType, ItemTypeOrder draggedItemType)
        {
            if (!gt.Equals(Group)) //dropping an item from one group to another
            {
                ToolboxItem tbi = new ToolboxItem();
                RemoveItemTypeOrder(gt, draggedItemType);
                tbi.ItemType = draggedItemType.Item;
                HandleToolboxItemDrop(Group, tbi, targetItemType);
                GroupTypeOrderCollection.Sort(x => x.GroupOrder); //refresh all groups to show the changes
            }
            else
            {
                int draggedPosition = gt.ItemOrder.IndexOf(draggedItemType);
                double targetItemTypeNo = targetItemType.ItemOrder; //affected item
                int targetPosition = gt.ItemOrder.IndexOf(targetItemType);

                if (targetItemType != draggedItemType)
                {
                    gt.ItemOrder.Remove(draggedItemType);
                    if (targetItemType.DesignID == null && !draggedItemType.DesignID.Equals("198"))
                    {
                        AdjustItemOrder(gt);
                        draggedItemType.ItemOrder = targetItemType.ItemOrder;
                        gt.ItemOrder.Add(draggedItemType);
                        gt.ItemOrder.Sort(i => i.ItemOrder);
                        GenerateEmptyFields(gt);
                    }
                    else if (targetItemType.DesignID == null && draggedItemType.DesignID.Equals("198"))
                    {

                        draggedItemType.ItemOrder = targetItemType.ItemOrder;
                        gt.ItemOrder.Add(draggedItemType);
                        gt.ItemOrder.Sort(i => i.ItemOrder);
                        GenerateEmptyFields(gt);
                        AdjustItemOrder(gt);


                    }

                    else if (draggedItemType.DesignID.Equals("198"))
                    {
                        if (draggedItemType.ItemOrder > targetItemType.ItemOrder)
                        {
                            gt.ItemOrder.Insert(targetPosition, draggedItemType);
                            draggedItemType.ItemOrder = targetItemTypeNo;
                            gt.ItemOrder.Sort(i => i.ItemOrder);
                            AdjustItemOrder(gt);
                        }
                        else
                        {
                            draggedItemType.ItemOrder = targetItemTypeNo;

                            //gt.ItemOrder.Insert(targetPosition - 1, draggedItemType);
                            gt.ItemOrder.Insert(targetPosition - 1, draggedItemType);
                            gt.ItemOrder.Sort(i => i.ItemOrder);
                            GenerateEmptyFields(gt);
                            AdjustItemOrder(gt);
                        }
                        //GenerateEmptyFields(gt);
                        gt.ItemOrder.Sort(i => i.ItemOrder);
                    }

                    else if (targetItemType.DesignID != null && draggedItemType.DesignID != null /*&& !draggedItemType.DesignID.Equals("198")*/)
                    {
                        if (draggedItemType.ItemOrder > targetItemType.ItemOrder)
                        {
                            gt.ItemOrder.Insert(targetPosition, draggedItemType);
                            draggedItemType.ItemOrder = targetItemTypeNo;
                        }
                        else
                        {
                            if (gt.ItemOrder.Count != targetPosition)
                            {
                                draggedItemType.ItemOrder = targetItemTypeNo;
                                gt.ItemOrder.Insert(targetPosition, draggedItemType);
                            }
                            else
                            {
                                draggedItemType.ItemOrder = targetItemTypeNo;
                                gt.ItemOrder.Add(draggedItemType);
                            }
                        }
                        AdjustItemOrder(gt, targetPosition, draggedPosition);

                        GenerateEmptyFields(gt);
                        gt.ItemOrder.Sort(i => i.ItemOrder);
                    }
                }
            }
        }

        /// <summary>
        /// Handles drop logic for NewLineItem from toolbox
        /// </summary>
        /// <param name="dropTargetItemTypeOrder"></param>
        /// <param name="tbi"></param>
        /// <param name="gt"></param>
        public void ToolboxSpecialNewLineItemDrop(ItemTypeOrder dropTargetItemTypeOrder, ToolboxItem tbi, GroupType gt)
        {
            CheckForNewLineItem(dropTargetItemTypeOrder);
            ItemTypeOrder itemTypeOrder = new ItemTypeOrder();
            itemTypeOrder.DesignID = tbi.ItemType.DesignID;
            ItemType itemType = new ItemType();
            itemType.DesignID = tbi.ItemType.DesignID;
            itemType.Header = tbi.ItemType.Header;
            itemTypeOrder.ItemOrder = dropTargetItemTypeOrder.ItemOrder;
            itemType.DanishTranslationText = tbi.ItemType.DanishTranslationText;
            itemType.EnglishTranslationText = tbi.ItemType.EnglishTranslationText;
            itemType.LanguageID = tbi.ItemType.LanguageID;
            itemTypeOrder.GroupTypeID = gt.GroupTypeID;
            itemTypeOrder.IncludedTypeID = "1";
            itemTypeOrder.Item = itemType;

            int index = gt.ItemOrder.IndexOf(dropTargetItemTypeOrder);

            gt.ItemOrder.Insert(index, itemTypeOrder);
            int draggedIndex = gt.ItemOrder.IndexOf(itemTypeOrder);

            AdjustItemOrderNewLineItemDrop(gt, draggedIndex);
            //AdjustItemOrder(gt);
        }

        /// <summary>
        /// Makes sure that a NewLineItem cannot be dropped on a row in a group that already contains a NewLineItem
        /// </summary>
        /// <param name="dropTargetItemTypeOrder"></param>
        public void CheckForNewLineItem(ItemTypeOrder dropTargetItemTypeOrder)
        {
            int itemOrder = (int)dropTargetItemTypeOrder.ItemOrder;
            int i = itemOrder - (itemOrder % 4);
            int j = itemOrder + (4 - (itemOrder % 4));
            while (i < j && i < Group.ItemOrder.Count - 1)
            {
                if (Group.ItemOrder[i].DesignID.Equals("198"))
                {
                    throw new Exception("The row already contains a <NewLineItem>");
                }
                i++;
            }
        }

        /// <summary>
        /// Handles drop logic for standard item from toolbox
        /// </summary>
        /// <param name="dropTargetItemTypeOrder"></param>
        /// <param name="tbi"></param>
        /// <param name="gt"></param>
        public void ToolboxItemDropOnStandardItem(ItemTypeOrder dropTargetItemTypeOrder, ToolboxItem tbi, GroupType gt)
        {
            ItemTypeOrder itemTypeOrder = new ItemTypeOrder();
            itemTypeOrder.DesignID = tbi.ItemType.DesignID;
            ItemType itemType = new ItemType { DesignID = tbi.ItemType.DesignID, Header = tbi.ItemType.Header };
            itemTypeOrder.ItemOrder = dropTargetItemTypeOrder.ItemOrder;
            itemType.DanishTranslationText = tbi.ItemType.DanishTranslationText;
            itemType.EnglishTranslationText = tbi.ItemType.EnglishTranslationText;
            itemType.LanguageID = tbi.ItemType.LanguageID;
            itemTypeOrder.GroupTypeID = gt.GroupTypeID;
            itemTypeOrder.IncludedTypeID = "1";
            itemTypeOrder.Item = itemType;
            int startPosition = gt.ItemOrder.IndexOf(dropTargetItemTypeOrder);
            MoveItemsForward(startPosition, itemTypeOrder, gt);
        }

        /// <summary>
        /// Handles drop logic on null field
        /// </summary>
        /// <param name="dropTargetItemTypeOrder"></param>
        /// <param name="tbi"></param>
        /// <param name="gt"></param>
        public void ToolboxItemDropOnNullField(ItemTypeOrder dropTargetItemTypeOrder, ToolboxItem tbi, GroupType gt)
        {
            dropTargetItemTypeOrder.DesignID = tbi.ItemType.DesignID;
            ItemType itemType = new ItemType();
            itemType.DesignID = tbi.ItemType.DesignID;
            itemType.Header = tbi.ItemType.Header;
            itemType.DanishTranslationText = tbi.ItemType.DanishTranslationText;
            itemType.EnglishTranslationText = tbi.ItemType.EnglishTranslationText;
            itemType.LanguageID = tbi.ItemType.LanguageID;
            dropTargetItemTypeOrder.GroupTypeID = gt.GroupTypeID;
            dropTargetItemTypeOrder.IncludedTypeID = "1";
            dropTargetItemTypeOrder.Item = itemType;
            gt.ItemOrder.Add(dropTargetItemTypeOrder);
            GenerateEmptyFields(gt);
        }

        /// <summary>
        /// Handles drop logic on emptyfield
        /// </summary>
        /// <param name="dropTargetItemTypeOrder"></param>
        /// <param name="tbi"></param>
        /// <param name="gt"></param>
        public void ToolboxItemDropOnEmptyField(ItemTypeOrder dropTargetItemTypeOrder, ToolboxItem tbi, GroupType gt)
        {
            dropTargetItemTypeOrder.DesignID = tbi.ItemType.DesignID;
            dropTargetItemTypeOrder.Item.Header = tbi.ItemType.Header;
            dropTargetItemTypeOrder.Item.DanishTranslationText = tbi.ItemType.DanishTranslationText;
            dropTargetItemTypeOrder.Item.EnglishTranslationText = tbi.ItemType.EnglishTranslationText;
            dropTargetItemTypeOrder.Item.LanguageID = tbi.ItemType.LanguageID;
            dropTargetItemTypeOrder.GroupTypeID = gt.GroupTypeID;
            dropTargetItemTypeOrder.IncludedTypeID = "1";
        }

        /// <summary>
        /// Determines how the order of the items in a group should be adjusted
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="targetPosition"></param>
        /// <param name="draggedPosition"></param>
        public void AdjustItemOrder(GroupType gt, int targetPosition, int draggedPosition)
        {
            if (targetPosition < draggedPosition)
            {
                IncrementItemOrder(gt, targetPosition, draggedPosition);
            }
            else
            {
                DecrementItemOrder(gt, targetPosition, draggedPosition);
            }
            _wvm._changedFlag = true;
        }

        /// <summary>
        /// Adjust the order of the items in a group in relation to drop of a NewLineItem
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="draggedPosition"></param>
        public void AdjustItemOrderNewLineItemDrop(GroupType gt, int draggedPosition)
        {
            int j = 0;
            bool stop = false;
            while (j < gt.ItemOrder.Count && !stop)
            {
                if (gt.ItemOrder[j].Item.DesignID.Equals("198"))
                {
                    stop = true;
                }
                gt.ItemOrder[j].ItemOrder = j;
                j++;
            }
            int i = draggedPosition;
            while (i < gt.ItemOrder.Count)
            {
                if (gt.ItemOrder[i].DesignID.Equals("198"))
                {
                    if (i != 0)
                        gt.ItemOrder[i].ItemOrder = gt.ItemOrder[i - 1].ItemOrder + 1;
                    i++;
                    gt.ItemOrder[i].ItemOrder = gt.ItemOrder[i - 1].ItemOrder + (4 - (gt.ItemOrder[i - 1].ItemOrder % 4));
                    i++;
                }
                else
                {
                    gt.ItemOrder[i].ItemOrder = gt.ItemOrder[i - 1].ItemOrder + 1;
                    i++;
                }
            }
            _wvm._changedFlag = true;
        }

        /// <summary>
        /// Adjust the order of the items in a group
        /// </summary>
        /// <param name="gt"></param>
        public void AdjustItemOrder(GroupType gt)
        {
            int j = 0;
            bool stop = false;
            while (j < gt.ItemOrder.Count && !stop)
            {
                if (gt.ItemOrder[j].Item.DesignID.Equals("198"))
                {
                    stop = true;
                }
                gt.ItemOrder[j].ItemOrder = j;
                j++;
            }
            int i = j - 1;
            if (i > 0)
            {
                while (i < gt.ItemOrder.Count)
                {
                    if (gt.ItemOrder[i].DesignID.Equals("198") && i + 1 != gt.ItemOrder.Count)
                    {
                        i++;
                        gt.ItemOrder[i].ItemOrder = gt.ItemOrder[i - 1].ItemOrder +
                                                    (4 - (gt.ItemOrder[i - 1].ItemOrder % 4));
                        i++;
                    }
                    else
                    {
                        gt.ItemOrder[i].ItemOrder = gt.ItemOrder[i - 1].ItemOrder + 1;
                        i++;
                    }
                }
            }
            _wvm._changedFlag = true;
        }

        public void DecrementItemOrder(GroupType gt, int targetPosition, int draggedPosition)
        {
            int i = targetPosition - 1;
            while (i >= draggedPosition)
            {
                if (!gt.ItemOrder[i].DesignID.Equals("198"))
                {
                    gt.ItemOrder[i].ItemOrder--;
                }
                else
                {
                    gt.ItemOrder[i].ItemOrder--;
                    break;
                }
                i--;
            }
            _wvm._changedFlag = true;
        }

        public void IncrementItemOrder(GroupType gt, int position, int startPosition)
        {
            int i = position + 1;
            while (i <= startPosition)
            {
                if (!gt.ItemOrder[i].DesignID.Equals("198"))
                {
                    gt.ItemOrder[i].ItemOrder++;
                }
                else
                {
                    break;
                }
                i++;
            }
            _wvm._changedFlag = true;
        }

        /// <summary>
        /// Generates emptyfields for gaps between items of any time. Takes NewLineItem into account when generating
        /// </summary>
        /// <param name="gt"></param>
        public void GenerateEmptyFields(GroupType gt)
        {
            int i = 0;
            while (i < gt.ItemOrder.Count)
            {
                int noOfEmptyFieldsCounter = 0;
                if (gt.ItemOrder.Count > 1)
                {
                    if (gt.ItemOrder[i].ItemOrder != i)
                    {
                        int totalNumberOfEmptyFields = 0;
                        if (i != 0 && !gt.ItemOrder[i - 1].DesignID.Equals("198"))
                        {
                            noOfEmptyFieldsCounter = (int)gt.ItemOrder[i].ItemOrder - (int)gt.ItemOrder[i - 1].ItemOrder;
                            totalNumberOfEmptyFields = noOfEmptyFieldsCounter;
                            while (noOfEmptyFieldsCounter > 1) //Insert Empty fields
                            {
                                gt.ItemOrder.Insert(i, CreateEmptyField(gt, i + noOfEmptyFieldsCounter - 2));
                                noOfEmptyFieldsCounter--;
                            }
                            //i = totalNumberOfEmptyFields - 1;
                            if (totalNumberOfEmptyFields > 1)
                            {
                                i = i + totalNumberOfEmptyFields - 1;
                            }
                        }
                        else
                        {
                            if (i != 0)
                            {
                                totalNumberOfEmptyFields = (int)gt.ItemOrder[i].ItemOrder -
                                                           ((int)gt.ItemOrder[i - 1].ItemOrder +
                                                            (4 - (((int)gt.ItemOrder[i - 1].ItemOrder) % 4)));
                            }
                            else
                            {
                                totalNumberOfEmptyFields = (int)gt.ItemOrder[i].ItemOrder;

                            }


                            ItemTypeOrder firstEmptyFieldGenerated = null;
                            noOfEmptyFieldsCounter = totalNumberOfEmptyFields;
                            while (noOfEmptyFieldsCounter > 0)
                            {

                                //gt.ItemOrder.Insert(i, CreateEmptyField(gt, (int)gt.ItemOrder[i].ItemOrder - noOfEmptyFieldsCounter));
                                ItemTypeOrder emptyField = CreateEmptyField(gt,
                                    (int)gt.ItemOrder[i].ItemOrder - noOfEmptyFieldsCounter);
                                if (noOfEmptyFieldsCounter == totalNumberOfEmptyFields)
                                    firstEmptyFieldGenerated = emptyField;
                                gt.ItemOrder.Insert(i, emptyField);
                                noOfEmptyFieldsCounter--;
                            }
                            if (totalNumberOfEmptyFields > 0)
                            {
                                //i = gt.ItemOrder.Count - 1;
                                i = gt.ItemOrder.IndexOf(firstEmptyFieldGenerated) + 1;
                            }
                        }
                    }
                    i++;

                }
                else
                {
                    noOfEmptyFieldsCounter = (int)gt.ItemOrder[i].ItemOrder;
                    //totalNumberOfEmptyFields = noOfEmptyFieldsCounter + i;
                    while (noOfEmptyFieldsCounter > 0)
                    {
                        gt.ItemOrder.Insert(i, CreateEmptyField(gt, i + noOfEmptyFieldsCounter - 2));
                        noOfEmptyFieldsCounter--;
                    }
                    break;
                }
            }
            _wvm._changedFlag = true;
        }

        public ItemTypeOrder CreateEmptyField(GroupType gt, int itemOrder)
        {
            ItemTypeOrder itemTypeOrder = new ItemTypeOrder();
            itemTypeOrder.GroupTypeID = gt.GroupTypeID;
            itemTypeOrder.DesignID = "197";
            ItemType emptyFieldItemType = new ItemType();
            emptyFieldItemType.DesignID = "197";
            emptyFieldItemType.Header = "<EmptyField>";
            itemTypeOrder.IncludedTypeID = "1";
            itemTypeOrder.ItemOrder = itemOrder;
            itemTypeOrder.Item = emptyFieldItemType;

            _wvm._changedFlag = true;

            return itemTypeOrder;
        }

        public void MoveItemsForward(int startPosition, ItemTypeOrder newItemType, GroupType gt)
        {
            ObservableCollection<ItemTypeOrder> itemTypeList = gt.ItemOrder;
            bool stopCounting = false;
            int i = startPosition;
            while (i < itemTypeList.Count && !stopCounting)
            {
                if (itemTypeList[i].DesignID.Equals("198"))
                {
                    if (itemTypeList[i].ItemOrder % 4 == 3)
                    {
                        itemTypeList.RemoveAt(i);
                    }
                    else
                    {
                        itemTypeList[i].ItemOrder++;
                    }
                    stopCounting = true;
                }
                else
                {
                    itemTypeList[i].ItemOrder++;
                    i++;
                }
            }
            itemTypeList.Add(newItemType);
            itemTypeList.Sort(ito => ito.ItemOrder);

            _wvm._changedFlag = true;
        }

        public void InsertGroup(GroupTypeOrder targetGroupTypeOrder, GroupType groupType)
        {
            int targetPosition = GroupTypeOrderCollection.IndexOf(targetGroupTypeOrder);

            GroupTypeOrder gto = new GroupTypeOrder();
            gto.DepartmentID = "-1";
            gto.GroupOrder = targetGroupTypeOrder.GroupOrder - 1;
            gto.PageTypeID = targetGroupTypeOrder.PageTypeID;
            gto.GroupTypeID = groupType.GroupTypeID;
            gto.Group = groupType;


            GroupTypeOrderCollection.Insert(targetPosition, gto);
        }

        /// <summary>
        /// Inserts a group at the end of a list. Used when dropping a new group from toolbox on a page
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="pageTypeID"></param>
        public void InsertGroupLast(GroupType gt, string pageTypeID)
        {
            GroupTypeOrder gtoCompare = GroupTypeOrderCollection.Last();
            GroupTypeOrder gto = new GroupTypeOrder();
            gto.DepartmentID = "-1";
            gto.PageTypeID = pageTypeID;
            gto.Group = gt;
            gto.GroupTypeID = gt.GroupTypeID;
            gto.GroupOrder = gtoCompare.GroupOrder + 1;
            GroupTypeOrderCollection.Add(gto);
        }

        /// <summary>
        /// Finds the index of the last occurrence of a group. Used for group with multiple departments
        /// </summary>
        /// <param name="gto"></param>
        /// <returns></returns>
        public int FindLastOccurrence(GroupTypeOrder gto)
        {
            int i = GroupTypeOrderCollection.IndexOf(GroupTypeOrderCollection.First(x => x.GroupTypeID.Equals(gto.GroupTypeID)));

            while (i < GroupTypeOrderCollection.Count)
            {
                if (!GroupTypeOrderCollection[i].GroupTypeID.Equals(gto.GroupTypeID))
                    break;
                i++;
            }
            return i;
        }

        public void HandleGroupTableDrop(GroupTypeOrder targetGroupTypeOrder, GroupTypeOrder draggedGroupTypeOrder)
        {
            List<GroupTypeOrder> draggedMultipleGTOList = FindDuplicatesOfGroups(draggedGroupTypeOrder); // For a particular group, there is an instance for each department
            //int draggedPosition = FindLastOccurrence(draggedGroupTypeOrder);
            //List<GroupTypeOrder> targetMultipleGTOList = FindDuplicatesOfGroups(targetGroupTypeOrder);
            int targetPosition = GroupTypeOrderCollection.IndexOf(targetGroupTypeOrder);
            double targetGroupOrder = targetGroupTypeOrder.GroupOrder;

            foreach (GroupTypeOrder gto in draggedMultipleGTOList)
            {
                GroupTypeOrderCollection.Remove(gto); // prepare for moving the group(s)
            }

            if (draggedGroupTypeOrder.GroupOrder > targetGroupTypeOrder.GroupOrder)
            {
                foreach (GroupTypeOrder gto in draggedMultipleGTOList)
                {
                    GroupTypeOrderCollection.Insert(targetPosition, gto);
                }
            }
            else
            {
                if (GroupTypeOrderCollection.Max(x => x.GroupOrder) != targetGroupOrder)
                {
                    foreach (GroupTypeOrder gto in draggedMultipleGTOList)
                    {
                        targetPosition = FindLastOccurrence(targetGroupTypeOrder); // make sure that the group(s) are inserted at the end of a sequence of duplicated gropus
                        GroupTypeOrderCollection.Insert(targetPosition, gto);
                    }
                }
                else
                {
                    foreach (GroupTypeOrder gto in draggedMultipleGTOList)
                    {
                        GroupTypeOrderCollection.Add(gto);
                    }
                }
            }
            AdjustGroupOrder();
            GroupTypeOrderCollection.Sort(gto => gto.GroupOrder);

            _wvm._changedFlag = true;
        }
        
        /// <summary>
        /// Finds duplications of a group. Duplications are caused by having multiple departments
        /// </summary>
        /// <param name="draggedGto"></param>
        /// <returns></returns>
        public List<GroupTypeOrder> FindDuplicatesOfGroups(GroupTypeOrder draggedGto)
        {
            List<GroupTypeOrder> gtoList = new List<GroupTypeOrder>();
            foreach (GroupTypeOrder gto in GroupTypeOrderCollection)
            {
                if (gto.GroupTypeID.Equals(draggedGto.GroupTypeID))
                {
                    gtoList.Add(gto);
                }
            }
            return gtoList;
        }

        /// <summary>
        /// Adjust the order of the groups. Takes duplications of a group into account
        /// </summary>
        public void AdjustGroupOrder()
        {
            GroupTypeOrder previousGto = null;
            int i = 0;
            int skipped = 0;
            while (i < GroupTypeOrderCollection.Count)
            {
                GroupTypeOrder currentGto = GroupTypeOrderCollection[i];
                if (i > 0)
                {
                    previousGto = GroupTypeOrderCollection[i - 1];
                    if (currentGto.GroupTypeID.Equals(previousGto.GroupTypeID))
                    {
                        currentGto.GroupOrder = previousGto.GroupOrder;
                        skipped++;
                    }
                    else
                    {
                        GroupTypeOrderCollection[i].GroupOrder = i + 1 - skipped;
                    }
                }
                else
                {
                    GroupTypeOrderCollection[i].GroupOrder = i + 1;
                }
                i++;
            }

            _wvm._changedFlag = true;
        }

        public void DecrementGroupOrderType(int targetPosition, int draggedPosition)
        {
            GroupTypeOrder previousGto = null;
            int i = GroupTypeOrderCollection.Count - 1;
            int counter = GroupTypeOrderCollection.Count - FindNoOfDuplicates() + 1;
            while (i >= 0)
            {
                GroupTypeOrder currentGto = GroupTypeOrderCollection[i];
                if (i < GroupTypeOrderCollection.Count - 1)
                {
                    previousGto = GroupTypeOrderCollection[i + 1];
                    if (currentGto.GroupTypeID.Equals(previousGto.GroupTypeID))
                    {
                        currentGto.GroupOrder = previousGto.GroupOrder;
                    }
                    else
                    {
                        counter--;
                        GroupTypeOrderCollection[i].GroupOrder = counter;
                    }
                }
                else
                {
                    counter--;
                    GroupTypeOrderCollection[i].GroupOrder = counter;
                }
                i--;
            }
            _wvm._changedFlag = true;
        }

        public int FindNoOfDuplicates()
        {
            int noOfDuplicates = 0;

            var duplicateList = from gto in GroupTypeOrderCollection
                                group gto.GroupTypeID by gto.GroupTypeID into g
                                let count = g.Count()
                                orderby count descending
                                select new { Value = g.Key, Count = count };
            foreach (var x in duplicateList)
            {
                if (x.Count > 1)
                {
                    noOfDuplicates += x.Count;
                }
            }
            if (noOfDuplicates > 1)
            {
                noOfDuplicates--;
            }
            return noOfDuplicates;
        }

        public void IncrementGroupOrderType(int position, int startPosition)
        {
            GroupTypeOrder previousGto = null;
            int i = position + 1;
            while (i <= startPosition)
            {
                GroupTypeOrder currentGto = GroupTypeOrderCollection[i];
                if (i > 0)
                {
                    previousGto = GroupTypeOrderCollection[i - 1];
                    if (currentGto.GroupTypeID.Equals(previousGto.GroupTypeID))
                    {
                        currentGto.GroupOrder = previousGto.GroupOrder;
                    }
                    else
                    {
                        GroupTypeOrderCollection[i].GroupOrder++;
                    }
                }
                else
                {
                    GroupTypeOrderCollection[i].GroupOrder++;
                }
                i++;
            }

            _wvm._changedFlag = true;
        }

        /// <summary>
        /// Adjust the order of the group
        /// </summary>
        public void RefreshGroupOrder()
        {
            int i = 0;
            while (i < GroupTypeOrderCollection.Count)
            {
                GroupTypeOrderCollection[i].GroupOrder = i + 1;
                i++;
            }
        }
    }
}
