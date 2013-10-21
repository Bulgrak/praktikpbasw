using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;
using TreatPraktik.ViewModel;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for GroupContainerUserControl.xaml
    /// </summary>
    public partial class GroupTableContainerUserControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<GroupTypeOrder> _gtoObsCollection;
        //public ObservableCollection<GroupTypeOrder> GtoObsCollection { get; set; }
        public GroupTableContainerViewModel GTCViewModel { get; set; }

        public GroupTableContainerUserControl()
        {
            InitializeComponent();
            this.DataContext = this;
            //GtoObsCollection = new ObservableCollection<GroupTypeOrder>();
            //GtoObsCollection.CollectionChanged += items_CollectionChanged;
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

        public void AdjustGroupOrder(GroupTypeOrder draggedGroupTypeOrder, GroupTypeOrder targetGroupTypeOrder)
        {
            //GTCViewModel.AdjustGroupOrder(draggedGroupTypeOrder, targetGroupTypeOrder);
        }

        public ObservableCollection<GroupTypeOrder> GtoObsCollection
        {
            get { return _gtoObsCollection; }
            set
            {
                _gtoObsCollection = value;
                OnPropertyChanged("GtoObsCollection");
            }
        }
    }
}
