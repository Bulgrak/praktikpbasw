using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    public class GroupViewModel
    {
        public GroupType Group { get; set; }

        public GroupViewModel()
        {
            
        }

        void AdjustItemOrderNewLineItem(GroupType gt, int draggedPosition)
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

        void DecrementItemOrder(GroupType gt, int targetPosition, int draggedPosition)
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

        void IncrementItemOrder(GroupType gt, int position, int startPosition)
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
    }
}
