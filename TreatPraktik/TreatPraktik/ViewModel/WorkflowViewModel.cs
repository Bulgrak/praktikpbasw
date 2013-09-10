using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreatPraktik.Model.WorkflowObjects;

namespace TreatPraktik.ViewModel
{
    class WorkflowViewModel
    {
        public IEnumerable<PageType> GetAllPages()
        {
            ImportExcel excel = ImportExcel.Instance;

            IEnumerable<PageType> pages = (from a in excel.WorkSheetktUIPageType.ktUIPageTypeList
                                    select new PageType
                                    {
                                        PageTypeID = a.PageTypeID,
                                        PageName = a.PageType,
                                        Groups = (from b in excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.Where( b => b.PageTypeID == a.PageTypeID)
                                                  join c in excel.WorkSheetExaminedGroup.ExaminedGroupList on b.GroupTypeID equals c.ID
                                                  select new GroupType
                                                  {
                                                      GroupTypeID = b.GroupTypeID,
                                                      GroupName = c.GroupType,
                                                      GroupOrder = b.GroupOrder
                                                  }).ToList(),
                                     }).ToList();

            return pages;
        }


                                    //}).ToList();
            //foreach (var item in pages)
            //{
            //    item.Groups = (from c in excel.WorkSheetExaminedGroup.ExaminedGroupList 
            //        join b in excel.WorkSheetktUIGroupOrder.ktUIGroupOrderList.Where(b => b.PageTypeID == item.PageTypeID) on c.ID equals b.GroupTypeID
            //                   //join c in excel.WorkSheetExaminedGroup.ExaminedGroupList on b.GroupTypeID equals c.ID
            //                   //where item.PageTypeID == b.PageTypeID
            //                   select new GroupType
            //                   {
            //                       GroupTypeID = b.GroupTypeID,
            //                       GroupName = c.GroupType,
            //                       GroupOrder = b.GroupOrder
            //                   }).ToList();
            
        
            



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
