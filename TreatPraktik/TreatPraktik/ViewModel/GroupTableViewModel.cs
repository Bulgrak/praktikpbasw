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

            GroupTypeOrderCollection.Sort(gtoItem => gtoItem.GroupOrder);
        }

        public void CleanUpRemovedDepartments(GroupTypeOrder gto, List<string> departmentList)
        {
            foreach (GroupTypeOrder gtoItem in GroupTypeOrderCollection)
            {
                if (gtoItem.GroupTypeID.Equals(gto.GroupTypeID))
                {
                    bool remove = true;
                    int i = 0;
                    while (i < departmentList.Count && remove)
                    {
                        if (gtoItem.DepartmentID.Equals(gto.DepartmentID))
                        {
                            remove = false;
                        }
                        i++;
                    }

                    if (remove)
                    {
                        GroupTypeOrderCollection.Remove(gto);
                    }
                }
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
                                                    (4 - (gt.ItemOrder[i - 1].ItemOrder%4));
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
            GroupTypeOrderCollection.Remove(draggedGroupTypeOrder);

            
            if (draggedGroupTypeOrder.GroupOrder > targetGroupTypeOrder.GroupOrder)
            {
                GroupTypeOrderCollection.Insert(targetPosition, draggedGroupTypeOrder);
                draggedGroupTypeOrder.GroupOrder = targetGroupTypeOrder.GroupOrder;
            }
            else
            {
                if (GroupTypeOrderCollection.Count != targetPosition)
                {
                    draggedGroupTypeOrder.GroupOrder = targetGroupTypeOrder.GroupOrder;
                    GroupTypeOrderCollection.Insert(targetPosition, draggedGroupTypeOrder);
                }
                else
                {
                    draggedGroupTypeOrder.GroupOrder = targetGroupTypeOrder.GroupOrder;
                    GroupTypeOrderCollection.Add(draggedGroupTypeOrder);
                }
            }
            AdjustGroupOrder(targetPosition, draggedPosition);
            GroupTypeOrderCollection.Sort(gto => gto.GroupOrder);

            _wvm._changedFlag = true;
        }

        public void AdjustGroupOrder(int targetPosition, int draggedPosition)
        {
            if (targetPosition < draggedPosition)
            {
                IncrementGroupOrderType(targetPosition, draggedPosition);
            }
            else
            {
                DecrementGroupOrderType(targetPosition, draggedPosition);
            }
        }

        public void DecrementGroupOrderType(int targetPosition, int draggedPosition)
        {
            int i = targetPosition - 1;
            while (i >= draggedPosition)
            {
                GroupTypeOrderCollection[i].GroupOrder--;
                i--;
            }

            _wvm._changedFlag = true;
        }

        public void IncrementGroupOrderType(int position, int startPosition)
        {
            int i = position + 1;
            while (i <= startPosition)
            {
                GroupTypeOrderCollection[i].GroupOrder++;
                i++;
            }

            _wvm._changedFlag = true;
        }

        public void RefreshGroupOrder()
        {
            int i = 0;
            while(i < GroupTypeOrderCollection.Count)
            {
                GroupTypeOrderCollection[i].GroupOrder = i+1;
                i++;
            }
        }
    }
}
