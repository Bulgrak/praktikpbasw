using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreatPraktik.Model.WorkflowObjects;

namespace TreatPraktik.ViewModel
{
    class WorkflowViewModel
    {
        public List<PageType> GetAllPages()
        {
            ImportExcel excel = ImportExcel.Instance;

            List<PageType> pages = (from a in excel.WorkSheetktUIPageType.ktUIPageTypeList
                                    select new PageType
                                    {
                                        PageTypeID = a.PageTypeID,
                                        PageName = a.PageType,
                                        Groups = new List<GroupType> (from b in excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.
                                                                          Where( b => b.PageTypeID.Equals(a.PageTypeID))
                                                  join c in excel.WorkSheetExaminedGroup.ExaminedGroupList on b.GroupTypeID equals c.ID
                                                  select new GroupType
                                                  {
                                                      GroupTypeID = b.GroupTypeID,
                                                      GroupName = c.GroupType,
                                                      GroupOrder = b.GroupOrder,
                                                      Items = new List<ItemType> (from d in excel.WorkSheetktUIOrder.ktUIOrderList.Where(d => d.GroupTypeID.Equals(b.GroupTypeID))
                                                                                  join e in excel.WorkSheetUIDesign.ktUIDesignList on d.DesignID equals e.DesignID
                                                                select new ItemType
                                                                {
                                                                    DesignID = d.DesignID,
                                                                    GroupOrder = d.GroupOrder,
                                                                    DatabaseFieldName = e.DatabaseFieldName
                                                                }).ToList(),
                                                  }).ToList(),
                                     }).ToList();

            return pages;
        }

        public List<PageType> GetAllPages2()
        {
            ImportExcel excel = ImportExcel.Instance;
            
            List<PageType> pages = (from a in excel.WorkSheetktUIPageType.ktUIPageTypeList
                                    select new PageType
                                    {
                                        PageTypeID = a.PageTypeID,
                                        PageName = a.PageType
                                    }).ToList();


            foreach (var item in pages)
            {
                item.Groups = (from c in excel.WorkSheetExaminedGroup.ExaminedGroupList
                               join b in excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.
                                    Where(b => b.PageTypeID.Equals(item.PageTypeID)) on c.ID equals b.GroupTypeID
                               select new GroupType
                               {
                                   GroupTypeID = b.GroupTypeID,
                                   GroupName = c.GroupType,
                                   GroupOrder = b.GroupOrder
                               }).ToList();

            }
            return pages;
        }
            
        
            



        //public List<AktivKoersel> GetAktiveKoersler()
        //{
        //    db = new amPhiDataContext();

        //    //Nu tjekker den efter kørsler der er fra et år tilbage. Pga testdata.. --> Skal kun være fra en dag tilbage.
        //    List<AktivKoersel> koersler = (from a in db.dtEVAs.Where(d => d.TurModtaget > DateTime.Now.AddYears(-1) && d.TurModtaget < DateTime.Now && d.TurAfsluttet == null)
        //                                   join b in db.stJournalInfos on a.KID equals b.KID
        //                                   join c in db.ktAmbulanceTypes on b.AmbulanceTypeID equals c.ID
        //                                   select new AktivKoersel
        //                                   {
        //                                       TurModtaget = a.TurModtaget,
        //                                       Description = c.Description,
        //                                       VognNr = b.VognNr,
        //                                       KID = a.KID,
        //                                       Prioritet = a.Koerselsform
        //                                   }).OrderBy(e => e.TurModtaget).OrderByDescending(f => f.TurModtaget).ToList();
        //    return koersler;
        //}
    }
}
