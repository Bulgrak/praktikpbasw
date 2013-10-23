using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Bibliography;
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
        /// Rename an existing group
        /// </summary>
        /// <param name="gto">The group to be renamed</param>
        /// <param name="engTransText">The english group name</param>
        /// <param name="danTransText">The danish group name</param>
        public void EditGroup(GroupTypeOrder gto, string engTransText, string danTransText, List<string> departmentList)
        {
            //PageType page = (from a in PageList where a.PageTypeID.Equals(pageTypeID) select a).FirstOrDefault();
            //GroupTypeOrder group = (from b in page.GroupTypeOrders where b.GroupTypeID.Equals(groupTypeID) select b).FirstOrDefault();

            gto.Group.DanishTranslationText = danTransText;
            gto.Group.EnglishTranslationText = engTransText;
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
            }
            CleanUpRemovedDepartments(gto, departmentList);
            GroupTypeOrderCollection.Sort(gtoItem => gtoItem.GroupOrder);
        }

        public void CleanUpRemovedDepartments(GroupTypeOrder gto, List<string> departmentList)
        {
            int j = 0;
            while (j < GroupTypeOrderCollection.Count)
            {
                GroupTypeOrder gtoItem = GroupTypeOrderCollection[j];
                if (gtoItem.GroupTypeID.Equals(gto.GroupTypeID))
                {
                    bool remove = true;
                    int i = 0;
                    while (i < departmentList.Count && remove)
                    {
                        if (gtoItem.DepartmentID.Equals(departmentList[i]))
                        {
                            remove = false;
                        }
                        i++;
                    }

                    if (remove)
                    {
                        GroupTypeOrderCollection.Remove(gtoItem);
                    }
                }
                j++;
            }
        }

        public void RemoveGroup(GroupTypeOrder gto)
        {
            GroupTypeOrderCollection.Remove(gto);
            RefreshGroupOrder();
        }

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

        public void AdjustItemOrderNewLineItem(GroupType gt, int draggedPosition)
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
                    if (gt.ItemOrder[i].DesignID.Equals("198"))
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
                            totalNumberOfEmptyFields = (int)gt.ItemOrder[i].ItemOrder - ((int)gt.ItemOrder[i - 1].ItemOrder + (4 - (((int)gt.ItemOrder[i - 1].ItemOrder) % 4)));
                            noOfEmptyFieldsCounter = totalNumberOfEmptyFields;
                            while (noOfEmptyFieldsCounter > 0)
                            {
                                gt.ItemOrder.Insert(i, CreateEmptyField(gt, (int)gt.ItemOrder[i].ItemOrder - noOfEmptyFieldsCounter));
                                noOfEmptyFieldsCounter--;
                            }
                            if (totalNumberOfEmptyFields != 0)
                            {
                                i = gt.ItemOrder.Count - 1;
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

        public void HandleGroupTableDrop(GroupTypeOrder targetGroupTypeOrder, GroupTypeOrder draggedGroupTypeOrder)
        {
            int draggedPosition = GroupTypeOrderCollection.IndexOf(draggedGroupTypeOrder);
            int targetPosition = GroupTypeOrderCollection.IndexOf(targetGroupTypeOrder);
            List<GroupTypeOrder> gtoList = FindDuplicatesOfGroups(draggedGroupTypeOrder);
            foreach (GroupTypeOrder gto in gtoList)
            {
                GroupTypeOrderCollection.Remove(gto);
            }

            if (draggedGroupTypeOrder.GroupOrder > targetGroupTypeOrder.GroupOrder)
            {
                foreach (GroupTypeOrder gto in gtoList)
                {
                    GroupTypeOrderCollection.Insert(targetPosition, gto);
                }
                //draggedGroupTypeOrder.GroupOrder = targetGroupTypeOrder.GroupOrder;
            }
            else
            {
                if (GroupTypeOrderCollection.Count != targetPosition)
                {
                    //draggedGroupTypeOrder.GroupOrder = targetGroupTypeOrder.GroupOrder;
                    foreach (GroupTypeOrder gto in gtoList)
                    {
                        if (gtoList.Count > 1)
                        {
                            GroupTypeOrderCollection.Insert(targetPosition - 1, gto);
                        }
                        else
                        {
                            GroupTypeOrderCollection.Insert(targetPosition, gto);
                        }
                    }
                }
                else
                {
                    //draggedGroupTypeOrder.GroupOrder = targetGroupTypeOrder.GroupOrder;
                    foreach (GroupTypeOrder gto in gtoList)
                    {
                        GroupTypeOrderCollection.Add(gto);
                    }
                }
            }
            AdjustGroupOrder();
            GroupTypeOrderCollection.Sort(gto => gto.GroupOrder);

            _wvm._changedFlag = true;
        }

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

        //public void AdjustGroupOrder(int targetPosition, int draggedPosition)
        //{
        //    if (targetPosition < draggedPosition)
        //    {
        //        IncrementGroupOrderType(targetPosition, draggedPosition);
        //    }
        //    else
        //    {
        //        DecrementGroupOrderType(targetPosition, draggedPosition);
        //    }
        //}

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

        //public void DecrementGroupOrderType(int targetPosition, int draggedPosition)
        //{
        //    GroupTypeOrder previousGto = null;
        //    int i = targetPosition - 1;
        //    while (i >= draggedPosition)
        //    {
        //        GroupTypeOrder currentGto = GroupTypeOrderCollection[i];
        //        if (i < GroupTypeOrderCollection.Count - 1)
        //        {
        //            previousGto = GroupTypeOrderCollection[i + 1];
        //            if (currentGto.GroupTypeID.Equals(previousGto.GroupTypeID))
        //            {
        //                currentGto.GroupOrder = previousGto.GroupOrder;
        //            }
        //            else
        //            {
        //                GroupTypeOrderCollection[i].GroupOrder--;
        //            }
        //        }
        //        else
        //        {
        //            GroupTypeOrderCollection[i].GroupOrder--;
        //        }
        //        i--;
        //    }

        //    _wvm._changedFlag = true;
        //}

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
            //foreach (GroupTypeOrder gto in GroupTypeOrderCollection)
            //    foreach (GroupTypeOrder gtoCompare in GroupTypeOrderCollection)
            //        if (gtoCompare.GroupTypeID.Equals(gto.GroupTypeID))
            //            noOfIdenticalGroups++;
            //if (noOfIdenticalGroups != 0)
            //{
            //    noOfIdenticalGroups--;
            //}
            //return noOfIdenticalGroups;


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
