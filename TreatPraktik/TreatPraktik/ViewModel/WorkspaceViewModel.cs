using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    public class WorkspaceViewModel : INotifyPropertyChanged
    {
        private static WorkspaceViewModel instance;

        public ImportExcel excel;

        public ObservableCollection<PageType> PageList { get; set; }
        private ObservableCollection<GroupTypeOrder> GroupList { get; set; }
        private ObservableCollection<GroupType> Groups { get; set; }
        private ObservableCollection<ItemType> ItemList { get; set; }

        //private ObservableCollection<GroupType> tempList { get; set; }

        public WorkspaceViewModel()
        {
            excel = ImportExcel.Instance;

            PageList = GetAllPages();
            GroupList = GetAllGroups();
            ItemList = GetAllItems();

            LinkCollections(PageList, Groups, GroupList,ItemList);
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
            var query = (from aaa in excel.WorkSheetktResources.ktResourceList
                         join bbb in excel.WorkSheetktResourceTranslation.ktResourceTranslationList on aaa.ResourceID equals bbb.ResourceID
                         join ccc in excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("7")) on aaa.ResourceTypeID equals ccc.ResourceTypeID
                         select new
                          {
                              bbb.LanguageID,
                              aaa.ResourceResxID,
                              bbb.TranslationText
                          }).ToList();

            List<PageType> pageList = (from a in excel.WorkSheetktUIPageType.ktUIPageTypeList
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
            var query = (from aaa in excel.WorkSheetktResources.ktResourceList
                          join bbb in excel.WorkSheetktResourceTranslation.ktResourceTranslationList on aaa.ResourceID equals bbb.ResourceID
                          join ccc in excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("1")) on aaa.ResourceTypeID equals ccc.ResourceTypeID
                          select new
                          {
                              bbb.LanguageID,
                              aaa.ResourceResxID,
                              bbb.TranslationText,
                              aaa.ResourceTypeID
                          }).ToList();

            List<GroupTypeOrder> groupOrderList = (from a in excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.OrderBy(m => m.GroupOrder)
                                         //join b in excel.WorkSheetExaminedGroup.ExaminedGroupList on a.GroupTypeID equals b.ID
                                         select new GroupTypeOrder
                                         {
                                             DepartmentID = a.DepartmentID,
                                             PageTypeID = a.PageTypeID,
                                             GroupTypeID = a.GroupTypeID,
                                             GroupOrder = a.GroupOrder
                                         }).ToList();

            List<GroupType> groupList = (from a in excel.WorkSheetExaminedGroup.ExaminedGroupList
                                         join b in excel.WorkSheetktResources.ktResourceList on a.GroupType equals b.ResourceResxID into hej
                                         from c in hej.DefaultIfEmpty()
                                         select new GroupType
                                         {
                                             GroupTypeID = a.ID,
                                             ResourceType = a.GroupType
                                         }).ToList();
                
                //from a in excel.WorkSheetktResources.ktResourceList.Where(x => x.ResourceTypeID.Equals("1"))
                //                         join b in excel.WorkSheetExaminedGroup.ExaminedGroupList on a.ResourceResxID equals b.GroupType into yay
                //                         from b in yay.DefaultIfEmpty()
                //                            select new GroupType
                //                            {
                //                                GroupTypeID = b.ID,
                //                                //GroupTypeID = (from b in excel.WorkSheetExaminedGroup.ExaminedGroupList.Where(x => x.GroupType.Equals(a.ResourceResxID))
                //                                //               select b.ID).FirstOrDefault(),
                //                                ResourceType = a.ResourceResxID,
                //                                ResourceTypeID = a.ResourceTypeID
                //                            }).ToList();

            //foreach (GroupType groupType in groupList)
            //{
            //    if (groupType.GroupTypeID.Equals("58") || groupType.GroupTypeID.Equals("60"))
            //    {
            //        groupList.Remove(groupType);
            //    }
                
            //    //Console.WriteLine(groupType.ResourceType + " " + groupType.GroupTypeID);
            //}


            //for (int i = 0; i < groupList.Count; i++)
            //{
            //    if (groupList[i].GroupTypeID.Equals("58") || groupList[i].GroupTypeID.Equals("60"))
            //    {
            //        groupList.Remove(groupList[i]);
            //        i--;
            //    }
            //}

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
            var query = (from aa in excel.WorkSheetktResources.ktResourceList
                          join bb in excel.WorkSheetktResourceTranslation.ktResourceTranslationList on aa.ResourceID equals bb.ResourceID
                          join cc in excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("2")) on aa.ResourceTypeID equals cc.ResourceTypeID
                          select new
                          {
                              bb.LanguageID,
                              aa.ResourceResxID,
                              bb.TranslationText
                          }).ToList();

            List<ItemType> itemList = (from a in excel.WorkSheetktUIOrder.ktUIOrderList.OrderBy(m => m.GroupOrder)
                                       join b in excel.WorkSheetUIDesign.ktUIDesignList on a.DesignID equals b.DesignID
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

            ////Put a GroupTypeID reference into pages
            //for (int i = 0; i < pages.Count; i++)
            //{
            //    for (int k = 0; k < groups.Count; k++)
            //    {
            //        if (pages[i].PageTypeID.Equals(groups[k].PageTypeID))
            //        {
            //            pages[i].GroupTypeIDs.Add(groups[k].GroupTypeID);
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
                        pages[i].Groups.Add(groupOrderTypes[k]);
                    }
                }
            }
        }

        //public ObservableCollection<GroupType> GetGroupTypeFromID(List<string> groupTypeIDs)
        //{
        //    tempList = new ObservableCollection<GroupType>();

        //    foreach (GroupType group in GroupList)
        //    {
        //        foreach (string id in groupTypeIDs)
        //        {
        //            if (group.GroupTypeID.Equals(id))
        //            {
        //                tempList.Add(group);
        //            }
        //        }
        //    }
        //    return tempList;
        //}

        //public ObservableCollection<PageType> GetAllPages()
        //{
        //    List<LanguageType> tempList = (from aa in excel.WorkSheetktResources.ktResourceList
        //                                   join bb in excel.WorkSheetktResourceTranslation.ktResourceTranslationList.Where(d => d.LanguageID.Equals("1")) on aa.ResourceID equals bb.ResourceID
        //                                   join cc in excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("2")) on aa.ResourceTypeID equals cc.ResourceTypeID
        //                                   select new LanguageType
        //                                   {
        //                                       ResourceID = aa.ResourceID,
        //                                       ResourceResxID = aa.ResourceResxID,
        //                                       ResourceTypeID = aa.ResourceTypeID,
        //                                       TranslationText = bb.TranslationText
        //                                   }
        //                                      ).ToList();

        //    List<LanguageType> tempList2 = (from aaa in excel.WorkSheetktResources.ktResourceList
        //                                    join bbb in excel.WorkSheetktResourceTranslation.ktResourceTranslationList.Where(d => d.LanguageID.Equals("1")) on aaa.ResourceID equals bbb.ResourceID
        //                                    join ccc in excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("1")) on aaa.ResourceTypeID equals ccc.ResourceTypeID
        //                                    select new LanguageType
        //                                    {
        //                                        ResourceID = aaa.ResourceID,
        //                                        ResourceResxID = aaa.ResourceResxID,
        //                                        ResourceTypeID = aaa.ResourceTypeID,
        //                                        TranslationText = bbb.TranslationText
        //                                    }
        //                                  ).ToList();


        //    List<PageType> pages = (from a in excel.WorkSheetktUIPageType.ktUIPageTypeList
        //                            select new PageType
        //                            {
        //                                PageTypeID = a.PageTypeID,
        //                                PageName = a.PageType,
        //                                Groups = new ObservableCollection<GroupType>(from b in excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.OrderBy(m => m.GroupOrder)
        //                                                                             //Where(b => b.PageTypeID.Equals(a.PageTypeID))
        //                                                                             join c in excel.WorkSheetExaminedGroup.ExaminedGroupList on b.GroupTypeID equals c.ID
        //                                                                             join i in tempList2 on c.GroupType equals i.ResourceResxID
        //                                                                             //join i in excel.WorkSheetktResources.ktResourceList on c.GroupType equals i.ResourceResxID
        //                                                                             //join j in excel.WorkSheetktResourceTranslation.ktResourceTranslationList.Where(d => d.LanguageID.Equals("1")) on i.ResourceID equals j.ResourceID
        //                                                                             //join k in excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("1")) on i.ResourceTypeID equals k.ResourceTypeID
        //                                                                             where b.PageTypeID.Equals(a.PageTypeID)
        //                                                                             select new GroupType
        //                                                                             {
        //                                                                                 DepartmentID = b.DepartmentID,
        //                                                                                 GroupTypeID = b.GroupTypeID,
        //                                                                                 GroupName = c.GroupType,
        //                                                                                 GroupOrder = b.GroupOrder,
        //                                                                                 GroupHeader = i.TranslationText,
        //                                                                                 Items = new ObservableCollection<ItemType>(from d in excel.WorkSheetktUIOrder.ktUIOrderList.OrderBy(n => n.GroupOrder)
        //                                                                                                                            join e in excel.WorkSheetUIDesign.ktUIDesignList on d.DesignID equals e.DesignID
        //                                                                                                                            join f in tempList on e.ResxID equals f.ResourceResxID into gj
        //                                                                                                                            from f in gj.DefaultIfEmpty()
        //                                                                                                                            //join g in excel.WorkSheetktResourceTranslation.ktResourceTranslationList.Where(d => d.LanguageID.Equals("1")) on f.ResourceID equals g.ResourceID
        //                                                                                                                            //join h in excel.WorkSheetktResourceType.ktResourceTypeList.Where(d => d.ResourceTypeID.Equals("2")) on f.ResourceTypeID equals h.ResourceTypeID
        //                                                                                                                            where d.GroupTypeID.Equals(b.GroupTypeID)
        //                                                                                                                            select new ItemType
        //                                                                                                                            {
        //                                                                                                                                DesignID = d.DesignID,
        //                                                                                                                                ItemOrder = d.GroupOrder,
        //                                                                                                                                DatabaseFieldName = e.DatabaseFieldName,
        //                                                                                                                                Header = (f == null) ? null : f.TranslationText,
        //                                                                                                                                IncludedTypeID = d.IncludedTypeID
        //                                                                                                                            }),
        //                                                                             }),
        //                            }).ToList();

        //    ObservableCollection<PageType> obsCol = new ObservableCollection<PageType>(pages);

        //    return obsCol;
        //}

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static WorkspaceViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorkspaceViewModel();
                }
                return instance;
            }
        }
    }
}
