using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    class WorkspaceViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PageType> PageList { get; set; }
        private RelayCommand<PageType> _pageParameterCommand;
        private ObservableCollection<GroupType> _groupList { get; set; }
        public PageType PageTypeSelected { get; set; }

        public WorkspaceViewModel()
        {
            PageList = GetAllPages();
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

        public ObservableCollection<PageType> GetAllPages()
        {
            ImportExcel excel = ImportExcel.Instance;

            List<PageType> pages = (from a in excel.WorkSheetktUIPageType.ktUIPageTypeList
                                    select new PageType
                                    {
                                        PageTypeID = a.PageTypeID,
                                        PageName = a.PageType,
                                        Groups = new ObservableCollection<GroupType>(from b in excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.
                                                                         Where(b => b.PageTypeID.Equals(a.PageTypeID))
                                                                                     join c in excel.WorkSheetExaminedGroup.ExaminedGroupList on b.GroupTypeID equals c.ID
                                                                                     select new GroupType
                                                                                     {
                                                                                         GroupTypeID = b.GroupTypeID,
                                                                                         GroupName = c.GroupType,
                                                                                         GroupOrder = b.GroupOrder,
                                                                                         Items = new ObservableCollection<ItemType>(from d in excel.WorkSheetktUIOrder.ktUIOrderList.
                                                                                                                        Where(d => d.GroupTypeID.Equals(b.GroupTypeID))
                                                                                                                                    join e in excel.WorkSheetUIDesign.ktUIDesignList on d.DesignID equals e.DesignID
                                                                                                                                    select new ItemType
                                                                                                                                    {
                                                                                                                                        DesignID = d.DesignID,
                                                                                                                                        GroupOrder = d.GroupOrder,
                                                                                                                                        DatabaseFieldName = e.DatabaseFieldName
                                                                                                                                    }),
                                                                                     }),
                                    }).ToList();

            ObservableCollection<PageType> obsCol = new ObservableCollection<PageType>(pages);

            return obsCol;
        }

        

        public ICommand PageParameterCommand
        {
            get
            {
                if (null == _pageParameterCommand)
                {
                    _pageParameterCommand = new RelayCommand<PageType>(ExecutePageParameterCommand);
                }
                return _pageParameterCommand;
            }
        }

        //private void ExecutePageParameterCommand(PageType page)
        //{

        //}

        public void ExecutePageParameterCommand(PageType page)
        {
            bool found = false;
            int i = 0;
            //ObservableCollection<GroupType> temp = null;

            while (!found)
            {
                if (PageList[i].PageTypeID.Equals(page.PageTypeID))
                {
                    found = true;
                    _groupList = PageList[i].Groups;
                    //temp = PageList[i].Groups;
                }
                else
                {
                    i++;
                }

            }
            
        }

        #region GetAllPages method using foreach
        //public List<PageType> GetAllPages2()
        //{
        //    ImportExcel excel = ImportExcel.Instance;

        //    List<PageType> pages = (from a in excel.WorkSheetktUIPageType.ktUIPageTypeList
        //                            select new PageType
        //                            {
        //                                PageTypeID = a.PageTypeID,
        //                                PageName = a.PageType
        //                            }).ToList();


        //    foreach (var item in pages)
        //    {
        //        item.Groups = (from c in excel.WorkSheetExaminedGroup.ExaminedGroupList
        //                       join b in excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.
        //                            Where(b => b.PageTypeID.Equals(item.PageTypeID)) on c.ID equals b.GroupTypeID
        //                       select new GroupType
        //                       {
        //                           GroupTypeID = b.GroupTypeID,
        //                           GroupName = c.GroupType,
        //                           GroupOrder = b.GroupOrder
        //                       }).ToList();

        //    }
        //    return pages;
        //}
        #endregion
    }
}
