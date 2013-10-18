using System.ComponentModel;

namespace TreatPraktik.Model.WorkspaceObjects
{
    public class ItemTypeOrder : INotifyPropertyChanged
    {
        //Points to the Item
        public ItemType Item { get; set; }

        //Used to link the ItemTypeOrder to the GroupType
        public string GroupTypeID { get; set; }

        //used to order the items
        private double _itemOrder { get; set; }

        //Used to link the ItemTypeOrder to the Item
        public string DesignID { get; set; }

        public ItemTypeOrder()
        {

        }

        public ItemTypeOrder(ItemType item, string groupTypeID, double itemOrder, string designID)
        {
            this.Item = item;
            this.GroupTypeID = groupTypeID;
            ItemOrder = itemOrder;
            this.DesignID = designID;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        public double ItemOrder
        {
            get
            {
                return _itemOrder;
            }
            set
            {
                _itemOrder = value;
                OnPropertyChanged("ItemOrder");
            }
        }
    }
}
