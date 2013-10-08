using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkSheets;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    public class WorkspaceViewModel : INotifyPropertyChanged
    {
        private static WorkspaceViewModel _instance;

        private readonly ImportExcel _excel;

        public ObservableCollection<PageType> PageList { get; private set; }
        private ObservableCollection<GroupTypeOrder> GroupList { get; set; }
        private ObservableCollection<GroupType> Groups { get; set; }
        private ObservableCollection<ItemType> ItemList { get; set; }

        private int GroupCounter;

        public WorkspaceViewModel()
        {
            _excel = ImportExcel.Instance;

            PageList = GetAllPages();
            GroupList = GetAllGroups();
            ItemList = GetAllItems();

            LinkCollections(PageList, Groups, GroupList, ItemList);

            GroupCounter = 0;
            int index = 0;

            while (index < WorkSheetktResources.Instance.ktResourceList.Count)
            {
                if (Convert.ToInt32(WorkSheetktResources.Instance.ktResourceList[index].ResourceID) > GroupCounter)
                {
                    GroupCounter = Convert.ToInt32(WorkSheetktResources.Instance.ktResourceList[index].ResourceID);
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
            var query = (from aaa in _excel.WorkSheetktResources.ktResourceList
                         join bbb in _excel.WorkSheetktResourceTranslation.ktResourceTranslationList on aaa.ResourceID equals bbb.ResourceID
                         join ccc in _excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("7")) on aaa.ResourceTypeID equals ccc.ResourceTypeID
                         select new
                          {
                              bbb.LanguageID,
                              aaa.ResourceResxID,
                              bbb.TranslationText
                          }).ToList();

            List<PageType> pageList = (from a in _excel.WorkSheetktUIPageType.ktUIPageTypeList
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
            var query = (from aaa in _excel.WorkSheetktResources.ktResourceList
                          join bbb in _excel.WorkSheetktResourceTranslation.ktResourceTranslationList on aaa.ResourceID equals bbb.ResourceID
                          join ccc in _excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("1")) on aaa.ResourceTypeID equals ccc.ResourceTypeID
                          select new
                          {
                              bbb.LanguageID,
                              aaa.ResourceResxID,
                              bbb.TranslationText,
                              aaa.ResourceTypeID,
                              aaa.ResourceID
                          }).ToList();

            List<GroupTypeOrder> groupOrderList = (from a in _excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.OrderBy(m => m.GroupOrder)
                                         select new GroupTypeOrder
                                         {
                                             DepartmentID = a.DepartmentID,
                                             PageTypeID = a.PageTypeID,
                                             GroupTypeID = a.GroupTypeID,
                                             GroupOrder = a.GroupOrder
                                         }).ToList();

            List<GroupType> groupList = (from a in _excel.WorkSheetExaminedGroup.ExaminedGroupList
                                         join b in _excel.WorkSheetktResources.ktResourceList on a.GroupType equals b.ResourceResxID into hej
                                         from c in hej.DefaultIfEmpty()
                                         select new GroupType
                                         {
                                             GroupTypeID = a.ID,
                                             ResourceType = a.GroupType
                                         }).ToList();

            for (int i = 0; i < groupOrderList.Count; i++)
            {
                foreach(GroupType group in groupList)
                {
                    if (groupOrderList[i].GroupTypeID.Equals(group.GroupTypeID))
                    {
                        groupOrderList[i].Group = group;
                    }
                }
            }


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
            
            Groups = new ObservableCollection<GroupType>(groupList);
            ObservableCollection<GroupTypeOrder> obsCol = new ObservableCollection<GroupTypeOrder>(groupOrderList);

            return obsCol;
        }

        /// <summary>
        /// Gets all the ItemTypes from Excel
        /// </summary>
        /// <returns>Collection of items</returns>
        private ObservableCollection<ItemType> GetAllItems()
        {
            //Items
            //Sort on language 1=English and on ResourceType 2=DataItemHeading
            var query = (from aa in _excel.WorkSheetktResources.ktResourceList
                          join bb in _excel.WorkSheetktResourceTranslation.ktResourceTranslationList on aa.ResourceID equals bb.ResourceID
                          join cc in _excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("2")) on aa.ResourceTypeID equals cc.ResourceTypeID
                          select new
                          {
                              bb.LanguageID,
                              aa.ResourceResxID,
                              bb.TranslationText
                          }).ToList();

            List<ItemType> itemList = (from a in _excel.WorkSheetktUIOrder.ktUIOrderList.OrderBy(m => m.GroupOrder)
                                       join b in _excel.WorkSheetUIDesign.ktUIDesignList on a.DesignID equals b.DesignID
                                       select new ItemType
                                       {
                                           ResourceType = b.ResxID,
                                           GroupTypeID = a.GroupTypeID,
                                           DesignID = a.DesignID,
                                           ItemOrder = a.GroupOrder,
                                           IncludedTypeID = a.IncludedTypeID
                                       }).ToList();

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

            ObservableCollection<ItemType> obsCol = new ObservableCollection<ItemType>(itemList);

            return obsCol;
        }

        /// <summary>
        /// Puts ItemTypes into GroupTypes, and GroupTypes into PageTypes
        /// </summary>
        private void LinkCollections(ObservableCollection<PageType> pages, ObservableCollection<GroupType> groups, ObservableCollection<GroupTypeOrder> groupOrderTypes, ObservableCollection<ItemType> items)
        {
            //Put items into groups
            for (int i = 0; i < groups.Count; i++)
            {
                for (int k = 0; k < items.Count; k++)
                {
                    if (groups[i].GroupTypeID.Equals(items[k].GroupTypeID))
                    {
                        groups[i].Items.Add(items[k]);
                    }
                }
            }

            //Put groups into pages
            for (int i = 0; i < pages.Count; i++)
            {
                for (int k = 0; k < groupOrderTypes.Count; k++)
                {
                    if (pages[i].PageTypeID.Equals(groupOrderTypes[k].PageTypeID))
                    {
                        pages[i].Groups.Add(groupOrderTypes[k]);
                    }
                }
            }
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
            //GroupTypeOrder --> DepartmentID = 1, set PzgeTypeID, set new GroupTypeID, set groupOrder
            //GroupType --> set new GroupTypeID, danish, english, resourcetype = English name - " ", set new ResourceTypeID

            GroupTypeOrder gtOrder = new GroupTypeOrder();
            GroupType groupType = new GroupType();

            //Create new GroupTypeOrder
            gtOrder.DepartmentID = "-1";
            gtOrder.PageTypeID = pageTypeId;

            int highestId = 0;

            foreach (PageType page in PageList)
            {
                int index = 0;

                while (index < page.Groups.Count)
                {
                    if (Convert.ToInt32(page.Groups[index].GroupTypeID) > highestId)
                    {
                        highestId = Convert.ToInt32(page.Groups[index].GroupTypeID);
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
                while ((from a in page.Groups where a.Group.ResourceType.Equals(hej + i) select a).Any())
                {
                    i++;
                }
            }
            
            hej = hej + i;
            
            groupType.ResourceType = hej;

            groupType.ResourceID = (GroupCounter + 1).ToString();
            GroupCounter++;
            groupType.ResourceTypeID = "1";

            //Reference GroupTypeOrder with GroupType
            gtOrder.Group = groupType;

            
            int hello = 0;
            while (hello < PageList.Count)
            {
                if (PageList[hello].PageTypeID.Equals(pageTypeId))
                {
                    PageList[hello].Groups.Add(gtOrder);
                }
                hello++;
            }
        }

        /// <summary>
        /// Rename an existing group
        /// </summary>
        /// <param name="pageTypeId"></param>
        /// <param name="groupOrder"></param>
        /// <param name="groupTypeID"></param>
        /// <param name="itemTypes"></param>
        /// <param name="englishTranslationText"></param>
        /// <param name="danishTranslationText"></param>
        public void RenameGroup(string pageTypeId, string groupOrder, string groupTypeID, ObservableCollection<ItemType> itemTypes,
            string englishTranslationText, string danishTranslationText)
        {


        }
    }
}
