using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.View;

namespace TreatPraktik.ViewModel
{
    public class GroupTableViewModel
    {
        public GroupType Group { get; set; }
        public ObservableCollection<GroupTypeOrder> GroupTypeOrderCollection { get; set; }

        public GroupTableViewModel()
        {

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
        }

        public void IncrementGroupOrderType(int position, int startPosition)
        {
            int i = position + 1;
            while (i <= startPosition)
            {
                GroupTypeOrderCollection[i].GroupOrder++;
                i++;
            }
        }
    }
}
