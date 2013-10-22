using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkSheets;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    public class WorkspaceViewModel : INotifyPropertyChanged
    {
        public bool _changedFlag; // Changes to true if there are new changes to the PageList

        private static WorkspaceViewModel _instance;

        private ImportExcel _excel;

        public ObservableCollection<PageType> PageList { get; private set; }     // <-- The only list that changes should be made in
        private ObservableCollection<GroupTypeOrder> GroupList { get; set; }
        private ObservableCollection<ItemTypeOrder> _itemList;
        public ICollectionView PageTypeItemsView { get; set; }

        private int _groupCounter;

        public WorkspaceViewModel()
        {
            PageList = new ObservableCollection<PageType>();
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"Ressources\Configuration.xlsx");
            LoadWorkspace(path);

            //PageList = wvm.PageList;
            PageTypeItemsView = CollectionViewSource.GetDefaultView(PageList);
            PageTypeItemsView.Filter = ItemFilter;
            PageTypeItemsView.Refresh();
            
        }

        public ObservableCollection<ItemTypeOrder> ItemList
        {
            get
            {
                return _itemList;
            }
            private set
            {
                _itemList = value;
            }
        }

        public void LoadNewConfigurations(string path)
        {
            _changedFlag = false;

            PageList.Clear();

            _excel = ImportExcel.Instance;

            _excel.ImportExcelConfiguration(path);

            ObservableCollection<PageType> tempList = GetAllPages();
            GroupList = GetAllGroups();
            ItemList = GetAllItems();

            LinkCollections(tempList, GroupList, ItemList);

            foreach (PageType page in tempList)
            {
                PageList.Add(page);
            }

            _groupCounter = 0;
            int index = 0;

            while (index < WorkSheetktResources.Instance.ktResourceList.Count)
            {
                if (Convert.ToInt32(WorkSheetktResources.Instance.ktResourceList[index].ResourceID) > _groupCounter)
                {
                    _groupCounter = Convert.ToInt32(WorkSheetktResources.Instance.ktResourceList[index].ResourceID);
                }

                index++;
            }

        }

        public void LoadWorkspace(string path)
        {
            _changedFlag = false;

            _excel = ImportExcel.Instance;

            _excel.ImportExcelFromFile(path);

            PageList = GetAllPages();
            GroupList = GetAllGroups();
            ItemList = GetAllItems();

            LinkCollections(PageList, GroupList, ItemList);

            _groupCounter = 0;
            int index = 0;

            while (index < WorkSheetktResources.Instance.ktResourceList.Count)
            {
                if (Convert.ToInt32(WorkSheetktResources.Instance.ktResourceList[index].ResourceID) > _groupCounter)
                {
                    _groupCounter = Convert.ToInt32(WorkSheetktResources.Instance.ktResourceList[index].ResourceID);
                }

                index++;
            }
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
        
        /// <summary>
        /// Gets all the PageTypes from Excel
        /// </summary>
        /// <returns>Collection of pages</returns>
        private ObservableCollection<PageType> GetAllPages()
        {
            //Pages
            //Sort on language 1=English and on ResourceType 7=SiteMapResource
            var query = (from aaa in _excel._workSheetktResources.ktResourceList
                         join bbb in _excel._workSheetktResourceTranslation.ktResourceTranslationList on aaa.ResourceID equals bbb.ResourceID
                         join ccc in _excel._workSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("7")) on aaa.ResourceTypeID equals ccc.ResourceTypeID
                         select new
                          {
                              bbb.LanguageID,
                              aaa.ResourceResxID,
                              bbb.TranslationText
                          }).ToList();

            List<PageType> pageList = (from a in _excel._workSheetktUIPageType.ktUIPageTypeList
                                       select new PageType
                                       {
                                           ResourceType = a.PageType + "_Title",
                                           PageTypeID = a.PageTypeID
                                       }).ToList();

            foreach (PageType page in pageList)
            {
                foreach (var item in query)
                {
                    if (item.ResourceResxID == page.ResourceType)
                    {
                        if (item.LanguageID.Equals("2"))
	                    {
		                    page.DanishTranslationText = item.TranslationText;

	                    }
                        else if (item.LanguageID.Equals("1"))
	                    {
                            page.EnglishTranslationText = item.TranslationText;
	                    }
                    }
                }
                page.LanguageID = "2";
            }

            ObservableCollection<PageType> obsCol = new ObservableCollection<PageType>(pageList);

            return obsCol;
        }

        /// <summary>
        /// Gets all the GroupTypes from Excel
        /// </summary>
        /// <returns>Collection of groups</returns>
        private ObservableCollection<GroupTypeOrder> GetAllGroups()
        {
            //Groups
            //Sort on language 1=English and on ResourceType 1=DataGroupHeading
            var query = (from aaa in _excel._workSheetktResources.ktResourceList
                          join bbb in _excel._workSheetktResourceTranslation.ktResourceTranslationList on aaa.ResourceID equals bbb.ResourceID
                          join ccc in _excel._workSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("1")) on aaa.ResourceTypeID equals ccc.ResourceTypeID
                          select new
                          {
                              bbb.LanguageID,
                              aaa.ResourceResxID,
                              bbb.TranslationText,
                              aaa.ResourceTypeID,
                              aaa.ResourceID
                          }).ToList();

            List<GroupTypeOrder> groupOrderList = (from a in _excel._workSheetktUIGroupOrder.ktUIGroupOrderList.OrderBy(m => m.GroupOrder)
                                         select new GroupTypeOrder
                                         {
                                             DepartmentID = a.DepartmentID,
                                             PageTypeID = a.PageTypeID,
                                             GroupTypeID = a.GroupTypeID,
                                             GroupOrder = a.GroupOrder
                                         }).ToList();

            List<GroupType> groupList = (from a in _excel._workSheetktExaminedGroup.ExaminedGroupList
                                         join b in _excel._workSheetktResources.ktResourceList on a.GroupType equals b.ResourceResxID into hej
                                         from c in hej.DefaultIfEmpty()
                                         select new GroupType
                                         {
                                             GroupTypeID = a.ID,
                                             ResourceType = a.GroupType
                                         }).ToList();

            foreach (GroupType group in groupList)
            {
                foreach (var item in query)
                {
                    if (item.ResourceResxID == group.ResourceType)
                    {
                        if (item.LanguageID.Equals("2"))
                        {
                            group.DanishTranslationText = item.TranslationText;

                        }
                        else if (item.LanguageID.Equals("1"))
                        {
                            group.EnglishTranslationText = item.TranslationText;
                        }

                        group.ResourceTypeID = item.ResourceTypeID;
                        group.ResourceID = item.ResourceID;
                    }
                }
                group.LanguageID = "2";
            }
            
            //Link GroupType to GroupOrderType
            for (int i = 0; i < groupOrderList.Count; i++)
            {
                foreach (GroupType group in groupList)
                {
                    if (groupOrderList[i].GroupTypeID.Equals(group.GroupTypeID))
                    {
                        groupOrderList[i].Group = group;
                    }
                }
            }
            
            //Groups = new ObservableCollection<GroupType>(groupList);
            ObservableCollection<GroupTypeOrder> obsCol = new ObservableCollection<GroupTypeOrder>(groupOrderList);

            return obsCol;
        }

        /// <summary>
        /// Gets all the ItemTypes from Excel
        /// </summary>
        /// <returns>Collection of items</returns>
        private ObservableCollection<ItemTypeOrder> GetAllItems()
        {
            //Items
            //Sort on language 1=English and on ResourceType 2=DataItemHeading
            var query = (from aa in _excel._workSheetktResources.ktResourceList
                          join bb in _excel._workSheetktResourceTranslation.ktResourceTranslationList on aa.ResourceID equals bb.ResourceID
                          join cc in _excel._workSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("2")) on aa.ResourceTypeID equals cc.ResourceTypeID
                          select new
                          {
                              bb.LanguageID,
                              aa.ResourceResxID,
                              bb.TranslationText
                          }).ToList();

            List<ItemTypeOrder> itemOrders =
                (from a in _excel._workSheetktUIOrder.ktUIOrderList.OrderBy(m => m.GroupOrder)
                    select new ItemTypeOrder
                    {
                        GroupTypeID = a.GroupTypeID,
                        ItemOrder = a.GroupOrder,
                        DesignID = a.DesignID,
                        IncludedTypeID = a.IncludedTypeID
                    }).ToList();

            List<ItemType> itemList = (from a in _excel._workSheetUIDesign.ktUIDesignList 
                
                select new ItemType
                {
                    ResourceType = a.ResxID,
                    DesignID = a.DesignID,
                    //IncludedTypeID = a.IncludedTypeID
                }).ToList();

            //Set DanishTranslationText & EnglishTranslationText
            //Set LanguageID to 2 <-- Danish
            foreach (ItemType itemType in itemList)
            {
                foreach (var item in query)
                {
                    if (item.ResourceResxID == itemType.ResourceType)
                    {
                        if (item.LanguageID.Equals("2"))
                        {
                            itemType.DanishTranslationText = item.TranslationText;

                        }
                        else if (item.LanguageID.Equals("1"))
                        {
                            itemType.EnglishTranslationText = item.TranslationText;
                        }
                    }
                }
                itemType.LanguageID = "2";
            }

            //Link ItemType to ItemTypeOrder
            for (int i = 0; i < itemOrders.Count; i++)
            {
                foreach (ItemType item in itemList)
                {
                    if (itemOrders[i].DesignID.Equals(item.DesignID))
                    {
                        itemOrders[i].Item = item;
                    }
                }
            }

            ObservableCollection<ItemTypeOrder> obsCol = new ObservableCollection<ItemTypeOrder>(itemOrders);

            return obsCol;
        }

        /// <summary>
        /// Puts ItemTypes into GroupTypes, and GroupTypes into PageTypes
        /// </summary>
        private void LinkCollections(ObservableCollection<PageType> pages, ObservableCollection<GroupTypeOrder> groupOrderTypes, ObservableCollection<ItemTypeOrder> itemTypeOrder)
        {
            //Put items into groups
            for (int i = 0; i < groupOrderTypes.Count; i++)
            {
                for (int k = 0; k < itemTypeOrder.Count; k++)
                {
                    if (groupOrderTypes[i].Group.GroupTypeID.Equals(itemTypeOrder[k].GroupTypeID))
                    {
                        if (!groupOrderTypes[i].Group.ItemOrder.Contains(itemTypeOrder[k]))
                        {
                            groupOrderTypes[i].Group.ItemOrder.Add(itemTypeOrder[k]);
                        }
                    }
                }
            }

            //foreach (GroupTypeOrder typeOrder in groupOrderTypes)
            //{
            //    foreach (ItemTypeOrder it in typeOrder.Group.ItemOrder)
            //    {
            //        if (it.DesignID.Equals("63"))
            //        {
            //            Console.WriteLine(typeOrder.GroupTypeID + " - " + it.DesignID);
            //        }
                    
            //    }
            //}

            //Put groups into pages
            for (int i = 0; i < pages.Count; i++)
            {
                for (int k = 0; k < groupOrderTypes.Count; k++)
                {
                    if (pages[i].PageTypeID.Equals(groupOrderTypes[k].PageTypeID))
                    {
                        pages[i].GroupTypeOrders.Add(groupOrderTypes[k]);
                    }
                }
            }

            //foreach (PageType hej in PageList)
            //{
            //    foreach (GroupTypeOrder gt in hej.Groups)
            //    {
            //        foreach (ItemTypeOrder it in gt.Group.ItemOrder)
            //        {
            //            if (it.DesignID.Equals("63"))
            //            {
            //                Console.WriteLine(hej.PageTypeID + " - " + gt.GroupTypeID + " - " + it.DesignID);
            //            }
            //        }
            //    }
                

            //}

            //int x = 0;
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkspaceViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WorkspaceViewModel();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        /// <summary>
        /// Creates a new group on a the page
        /// </summary>
        /// <param name="pageTypeId">The id for the selected page</param>
        /// <param name="languageId">The selected language of the application</param>
        /// <param name="groupOrder">The group order number on the selected page</param>
        /// <param name="englishTranslationText">The english group name</param>
        /// <param name="danishTranslationText">The danish group name</param>
        public void CreateGroup(string pageTypeId, string languageId, double groupOrder, string englishTranslationText, string danishTranslationText)
        {
            GroupTypeOrder gtOrder = new GroupTypeOrder();
            GroupType groupType = new GroupType();

            //Create new GroupTypeOrder
            gtOrder.DepartmentID = "-1";
            gtOrder.PageTypeID = pageTypeId;

            int highestId = 0;

            foreach (PageType page in PageList)
            {
                int index = 0;

                while (index < page.GroupTypeOrders.Count)
                {
                    if (Convert.ToInt32(page.GroupTypeOrders[index].GroupTypeID) > highestId)
                    {
                        highestId = Convert.ToInt32(page.GroupTypeOrders[index].GroupTypeID);
                    }

                    index++;
                }
            }

            gtOrder.GroupTypeID = (highestId + 1).ToString();
            gtOrder.GroupOrder = groupOrder;

            //Create new GroupType
            groupType.GroupTypeID = (highestId + 1).ToString();
            groupType.DanishTranslationText = danishTranslationText;
            groupType.EnglishTranslationText = englishTranslationText;
            groupType.LanguageID = languageId;

            string hej = englishTranslationText.Replace(" ", string.Empty);
            int i = 1;
            foreach (PageType page in PageList)
            {
                while ((from a in page.GroupTypeOrders where a.Group.ResourceType.Equals(hej + i) select a).Any())
                {
                    i++;
                }
            }
            
            hej = hej + i;
            
            groupType.ResourceType = hej;

            groupType.ResourceID = (_groupCounter + 1).ToString();
            _groupCounter++;
            groupType.ResourceTypeID = "1";

            //Reference GroupTypeOrder with GroupType
            gtOrder.Group = groupType;

            
            int hello = 0;
            while (hello < PageList.Count)
            {
                if (PageList[hello].PageTypeID.Equals(pageTypeId))
                {
                    PageList[hello].GroupTypeOrders.Add(gtOrder);
                }
                hello++;
            }
        }

        /// <summary>
        /// Rename an existing group
        /// </summary>
        /// <param name="pageTypeID">The id for the selected page</param>
        /// <param name="groupTypeID">The id for the selected group</param>
        /// <param name="engTransText">The english group name</param>
        /// <param name="danTransText">The danish group name</param>
        public void RenameGroup(string pageTypeID, string groupTypeID, string engTransText, string danTransText)
        {
            PageType page = (from a in PageList where a.PageTypeID.Equals(pageTypeID) select a).FirstOrDefault();
            GroupTypeOrder group = (from b in page.GroupTypeOrders where b.GroupTypeID.Equals(groupTypeID) select b).FirstOrDefault();

            group.Group.DanishTranslationText = danTransText;
            group.Group.EnglishTranslationText = engTransText;
        }

        private bool ItemFilter(object item)
        {
            PageType pt = (PageType)item;
            if (pt.PageTypeID.Equals("15") || pt.PageTypeID.Equals("16") || pt.PageTypeID.Equals("17"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
