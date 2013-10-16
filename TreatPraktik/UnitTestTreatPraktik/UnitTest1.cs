using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreatPraktik.Model;
using TreatPraktik.ViewModel;

namespace UnitTestTreatPraktik
{
    [TestClass]
    public class ExportExcel
    {
        private WorkspaceViewModel _wvm;
        private ImportExcel _impExcel;
        private TreatPraktik.ViewModel.ExportExcel _exExcel;

        private List<ktExaminedGroup> _oldExGroup;
        private List<ktUIGroupOrder> _oldGroupOrder;
        private List<ktUIOrder> _oldOrder;
        private List<ktResources> _oldResources;
        private List<ktResourceTranslation> _oldResTranslation;

        private string _importPath;
        private string _exportPath;

        [TestInitialize]
        public void Initialize()
        {
            _importPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Ressources\Configuration.xlsx");
            _exportPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Ressources\UnitTest.xlsx");

            _wvm = WorkspaceViewModel.Instance;
            _wvm.LoadWorkspace(_importPath);
            _impExcel = ImportExcel.Instance;
            _exExcel = TreatPraktik.ViewModel.ExportExcel.Instance;

            //Get original lists of worksheets
            _oldExGroup = (from a in _impExcel._workSheetktExaminedGroup.ExaminedGroupList 
                           select new ktExaminedGroup
                           {
                               DataQualityScore = a.DataQualityScore, Expanded = a.Expanded, 
                               GroupExpendable = a.GroupExpendable, GroupIdentifier = a.GroupIdentifier, 
                               GroupType = a.GroupType, ID = a.ID, Name = a.Name, RequiredScore = a.RequiredScore
                           }).ToList();

            _oldGroupOrder = (from a in _impExcel._workSheetktUIGroupOrder.ktUIGroupOrderList
                                  select new ktUIGroupOrder
                                  {
                                      DepartmentID = a.DepartmentID, GroupOrder = a.GroupOrder, 
                                      GroupTypeID = a.GroupTypeID, PageTypeID = a.PageTypeID
                                  }).ToList();

            _oldOrder = (from a in _impExcel._workSheetktUIOrder.ktUIOrderList
                             select new ktUIOrder
                             {
                                 DesignID = a.DesignID, GroupOrder = a.GroupOrder, GroupTypeID = a.GroupTypeID,
                                 IncludedTypeID = a.IncludedTypeID, PageTypeID = a.PageTypeID
                             }).ToList();

            _oldResources = (from a in _impExcel._workSheetktResources.ktResourceList
                                 select new ktResources
                                 {
                                     ResourceID = a.ResourceID, ResourceResxID = a.ResourceResxID,
                                     ResourceTypeID = a.ResourceTypeID
                                 }).ToList();

            _oldResTranslation = (from a in _impExcel._workSheetktResourceTranslation.ktResourceTranslationList
                                      select new ktResourceTranslation
                                      {
                                          LanguageID = a.LanguageID, ResourceID = a.ResourceID, 
                                          TranslationText = a.TranslationText
                                      }).ToList();
        }

        #region Create new group

        [TestMethod]
        public void AddOneGroup()
        {
            //Create a new group
            string pageTypeID = "15";
            string languageID = "1";
            double groupOrder = 35.00;
            string engTransText = "New group";
            string danTransText = "Ny gruppe";
            _wvm.CreateGroup(pageTypeID, languageID, groupOrder, engTransText, danTransText);

            //Export excel with the new group in it
            _exExcel.CreateNewExcel(_exportPath);

            //Import the new excel file
            _impExcel.ImportExcelConfiguration(_exportPath);

            #region ktExaminedGroup tests

            //Get the new group in ktExaminedGroup
            List<ktExaminedGroup> examinedGroups =
                _impExcel._workSheetktExaminedGroup.ExaminedGroupList.Where(
                    x => x.GroupType.Equals("Newgroup1")).ToList();

            //Check if there only is one item of "New Group" in ktExaminedGroup
            Assert.AreEqual(examinedGroups.Count(), 1);
            ktExaminedGroup newGroup = examinedGroups[0];

            //Does the old list contain the new group?
            CollectionAssert.DoesNotContain(_oldExGroup, newGroup);

            #endregion

            #region ktUIOrder tests
            
            //Get the new group in ktUIOrder
            List<ktUIOrder> orders =
                _impExcel._workSheetktUIOrder.ktUIOrderList.Where(
                    x => x.GroupTypeID.Equals(newGroup.ID)).ToList();

            //Check if there are no items of "New Group" in ktExaminedGroup
            //There should be 0, because there are no items in the group
            Assert.AreEqual(orders.Count(), 0);

            #endregion

            #region ktUIGroupOrder tests

            //Get the new group in ktUIGroupOrder
            List<ktUIGroupOrder> groupOrders =
                _impExcel._workSheetktUIGroupOrder.ktUIGroupOrderList.Where(
                    x => x.GroupTypeID.Equals(newGroup.ID)).ToList();

            //Check if there only is one item of "New Group" in ktUIGroupOrder
            Assert.AreEqual(groupOrders.Count(), 1);
            ktUIGroupOrder newGroupOrder = groupOrders[0];

            //Does the old list contain the new group?
            CollectionAssert.DoesNotContain(_oldGroupOrder, newGroupOrder);

            //Does the new group have the right parameters on ktUIGroupOrder?
            Assert.AreEqual(newGroupOrder.PageTypeID, pageTypeID);
            Assert.AreEqual(newGroupOrder.GroupOrder, groupOrder);

            #endregion

            #region ktResources tests

            //Get the new group in ktResources
            List<ktResources> resources =
                _impExcel._workSheetktResources.ktResourceList.Where(
                    x => x.ResourceResxID.Equals("Newgroup1")).ToList();

            //Check if there only is one item of "New Group" in ktResources
            Assert.AreEqual(resources.Count(), 1);
            ktResources newResource = resources[0];

            //Check if new group has a ResourceTypeID equal to 1.
            //Means that the group is a group
            Assert.AreEqual(newResource.ResourceTypeID, "1");

            //Does the old list contain the new group?
            CollectionAssert.DoesNotContain(_oldResources, newResource);

            #endregion

            #region ktUIResourceTranslation tests

            //Get the new group in ktResourceTranslation
            List<ktResourceTranslation> resourceTranslations =
                _impExcel._workSheetktResourceTranslation.ktResourceTranslationList.Where(
                    x => x.ResourceID.Equals(newResource.ResourceID)).ToList();

            //Check if there only is two item of "New Group" in ktResourceTranslation
            Assert.AreEqual(resourceTranslations.Count(), 2);

            //Check if the translation texts are right for the "New Group"
            foreach (ktResourceTranslation rt in resourceTranslations)
            {
                if (rt.LanguageID.Equals("1"))
                {
                    Assert.AreEqual(rt.TranslationText, engTransText);
                }
                else
                {
                    Assert.AreEqual(rt.TranslationText, danTransText);
                }

                //Does the "New Group" exist in the old list
                CollectionAssert.DoesNotContain(_oldResTranslation, rt);
            }

            #endregion
        }

        [TestMethod]
        public void AddTwoGroups()
        {
            //Create a new group
            string pageTypeID1 = "15";
            string languageID1 = "1";
            double groupOrder1 = 35.00;
            string engTransText1 = "New group one";
            string danTransText1 = "Ny gruppe one";
            _wvm.CreateGroup(pageTypeID1, languageID1, groupOrder1, engTransText1, danTransText1);

            //Create a new group
            string pageTypeID2 = "15";
            string languageID2 = "1";
            double groupOrder2 = 36.00;
            string engTransText2 = "New group two";
            string danTransText2 = "Ny gruppe two";
            _wvm.CreateGroup(pageTypeID2, languageID2, groupOrder2, engTransText2, danTransText2);

            //Export excel with the new groups in it
            _exExcel.CreateNewExcel(_exportPath);

            //Import the new excel file
            _impExcel.ImportExcelConfiguration(_exportPath);

            #region ktExaminedGroup tests

            //Get the new group in ktExaminedGroup
            List<ktExaminedGroup> examinedGroups =
                _impExcel._workSheetktExaminedGroup.ExaminedGroupList.Where(
                    x => x.GroupType.Equals("Newgroupone1") || x.GroupType.Equals("Newgrouptwo1")).ToList();

            //Check if there are two items of "New Group" in ktExaminedGroup
            Assert.AreEqual(examinedGroups.Count(), 2);
            ktExaminedGroup newGroupOne = examinedGroups[0];
            ktExaminedGroup newGroupTwo = examinedGroups[1];

            //Does the old list contain the new groups?
            CollectionAssert.DoesNotContain(_oldExGroup, newGroupOne);
            CollectionAssert.DoesNotContain(_oldExGroup, newGroupTwo);

            #endregion

            #region ktUIOrder tests

            //Get the new groups in ktUIOrder
            List<ktUIOrder> orders =
                _impExcel._workSheetktUIOrder.ktUIOrderList.Where(
                    x => x.GroupTypeID.Equals(newGroupOne.ID) || 
                        x.GroupTypeID.Equals(newGroupTwo.ID)).ToList();

            //Check if there are no items of "New Group" in ktExaminedGroup
            //There should be 0, because there are no items in the groups
            Assert.AreEqual(orders.Count(), 0);

            #endregion

            #region ktUIGroupOrder tests

            //Get the new groups in ktUIGroupOrder
            List<ktUIGroupOrder> groupOrders =
                _impExcel._workSheetktUIGroupOrder.ktUIGroupOrderList.Where(
                    x => x.GroupTypeID.Equals(newGroupOne.ID) ||
                        x.GroupTypeID.Equals(newGroupTwo.ID)).ToList();

            //Check if there are two items of "New Group" in ktUIGroupOrder
            Assert.AreEqual(groupOrders.Count(), 2);
            ktUIGroupOrder newGroupOrderOne = groupOrders[0];
            ktUIGroupOrder newGroupOrderTwo = groupOrders[1];

            //Does the old list contain the new groups?
            CollectionAssert.DoesNotContain(_oldGroupOrder, newGroupOrderOne);
            CollectionAssert.DoesNotContain(_oldGroupOrder, newGroupOrderTwo);

            //Does the new groups have the right parameters on ktUIGroupOrder?
            foreach (ktUIGroupOrder go in groupOrders)
            {
                if (go.PageTypeID.Equals(pageTypeID1))
                {
                    Assert.AreEqual(newGroupOrderOne.PageTypeID, pageTypeID1);
                    Assert.AreEqual(newGroupOrderOne.GroupOrder, groupOrder1);
                }
                else
                {
                    Assert.AreEqual(newGroupOrderTwo.PageTypeID, pageTypeID2);
                    Assert.AreEqual(newGroupOrderTwo.GroupOrder, groupOrder2);
                }
            }

            #endregion

            #region ktResources tests

            //Get the new groups in ktResources
            List<ktResources> resources =
                _impExcel._workSheetktResources.ktResourceList.Where(
                    x => x.ResourceResxID.Equals("Newgroupone1") ||
                        x.ResourceResxID.Equals("Newgrouptwo1")).ToList();

            //Check if there only is one item of "New Group" in ktResources
            Assert.AreEqual(resources.Count(), 2);
            ktResources newResourceOne = resources[0];
            ktResources newResourceTwo = resources[1];

            //Check if new group has a ResourceTypeID equal to 1.
            //Means that the group is a group
            Assert.AreEqual(newResourceOne.ResourceTypeID, "1");
            Assert.AreEqual(newResourceTwo.ResourceTypeID, "1");

            //Does the old list contain the new groups?
            CollectionAssert.DoesNotContain(_oldResources, newResourceOne);
            CollectionAssert.DoesNotContain(_oldResources, newResourceTwo);

            #endregion

            #region ktUIResourceTranslation tests

            //Get the new groups in ktResourceTranslation
            List<ktResourceTranslation> resourceTranslationsOne =
                _impExcel._workSheetktResourceTranslation.ktResourceTranslationList.Where(
                    x => x.ResourceID.Equals(newResourceOne.ResourceID)).ToList();

            List<ktResourceTranslation> resourceTranslationsTwo =
                _impExcel._workSheetktResourceTranslation.ktResourceTranslationList.Where(
                    x => x.ResourceID.Equals(newResourceTwo.ResourceID)).ToList();

            //Check if there are four items of "New Group" in ktResourceTranslation
            Assert.AreEqual(resourceTranslationsOne.Count() + resourceTranslationsTwo.Count(), 4);

            //Check if the translation texts are right for the two "New Groups"
            foreach (ktResourceTranslation rt in resourceTranslationsOne)
            {
                if (rt.LanguageID.Equals("1"))
                {
                    Assert.AreEqual(rt.TranslationText, engTransText1);
                }
                else
                {
                    Assert.AreEqual(rt.TranslationText, danTransText1);
                }

                //Does the "New Group" exist in the old list
                CollectionAssert.DoesNotContain(_oldResTranslation, rt);
            }

            foreach (ktResourceTranslation rt in resourceTranslationsTwo)
            {
                if (rt.LanguageID.Equals("1"))
                {
                    Assert.AreEqual(rt.TranslationText, engTransText2);
                }
                else
                {
                    Assert.AreEqual(rt.TranslationText, danTransText2);
                }

                //Does the "New Group" exist in the old list
                CollectionAssert.DoesNotContain(_oldResTranslation, rt);
            }

            #endregion
        }

        [TestMethod]
        public void AddTwoGroupsWithSameName()
        {
            //Create a new group
            string pageTypeID1 = "15";
            string languageID1 = "1";
            double groupOrder1 = 35.00;
            string engTransText1 = "New group";
            string danTransText1 = "Ny gruppe";
            _wvm.CreateGroup(pageTypeID1, languageID1, groupOrder1, engTransText1, danTransText1);

            //Create a new group
            string pageTypeID2 = "15";
            string languageID2 = "1";
            double groupOrder2 = 36.00;
            string engTransText2 = "New group";
            string danTransText2 = "Ny gruppe";
            _wvm.CreateGroup(pageTypeID2, languageID2, groupOrder2, engTransText2, danTransText2);

            //Export excel with the new groups in it
            _exExcel.CreateNewExcel(_exportPath);

            //Import the new excel file
            _impExcel.ImportExcelConfiguration(_exportPath);

            #region ktExaminedGroup tests

            //Get the new group in ktExaminedGroup
            List<ktExaminedGroup> examinedGroups =
                _impExcel._workSheetktExaminedGroup.ExaminedGroupList.Where(
                    x => x.GroupType.Equals("Newgroup1") || x.GroupType.Equals("Newgroup2")).ToList();

            //Check if there are two items of "New Group" in ktExaminedGroup
            Assert.AreEqual(examinedGroups.Count(), 2);
            ktExaminedGroup newGroupOne = examinedGroups[0];
            ktExaminedGroup newGroupTwo = examinedGroups[1];

            //Does the old list contain the new groups?
            CollectionAssert.DoesNotContain(_oldExGroup, newGroupOne);
            CollectionAssert.DoesNotContain(_oldExGroup, newGroupTwo);

            #endregion

            #region ktUIOrder tests

            //Get the new groups in ktUIOrder
            List<ktUIOrder> orders =
                _impExcel._workSheetktUIOrder.ktUIOrderList.Where(
                    x => x.GroupTypeID.Equals(newGroupOne.ID) ||
                        x.GroupTypeID.Equals(newGroupTwo.ID)).ToList();

            //Check if there are no items of "New Group" in ktExaminedGroup
            //There should be 0, because there are no items in the groups
            Assert.AreEqual(orders.Count(), 0);

            #endregion

            #region ktUIGroupOrder tests

            //Get the new groups in ktUIGroupOrder
            List<ktUIGroupOrder> groupOrders =
                _impExcel._workSheetktUIGroupOrder.ktUIGroupOrderList.Where(
                    x => x.GroupTypeID.Equals(newGroupOne.ID) ||
                        x.GroupTypeID.Equals(newGroupTwo.ID)).ToList();

            //Check if there are two items of "New Group" in ktUIGroupOrder
            Assert.AreEqual(groupOrders.Count(), 2);
            ktUIGroupOrder newGroupOrderOne = groupOrders[0];
            ktUIGroupOrder newGroupOrderTwo = groupOrders[1];

            //Does the old list contain the new groups?
            CollectionAssert.DoesNotContain(_oldGroupOrder, newGroupOrderOne);
            CollectionAssert.DoesNotContain(_oldGroupOrder, newGroupOrderTwo);

            //Does the new groups have the right parameters on ktUIGroupOrder?
            foreach (ktUIGroupOrder go in groupOrders)
            {
                if (go.PageTypeID.Equals(pageTypeID1))
                {
                    Assert.AreEqual(newGroupOrderOne.PageTypeID, pageTypeID1);
                    Assert.AreEqual(newGroupOrderOne.GroupOrder, groupOrder1);
                }
                else
                {
                    Assert.AreEqual(newGroupOrderTwo.PageTypeID, pageTypeID2);
                    Assert.AreEqual(newGroupOrderTwo.GroupOrder, groupOrder2);
                }
            }

            #endregion

            #region ktResources tests

            //Get the new groups in ktResources
            List<ktResources> resources =
                _impExcel._workSheetktResources.ktResourceList.Where(
                    x => x.ResourceResxID.Equals("Newgroup1") ||
                        x.ResourceResxID.Equals("Newgroup2")).ToList();

            //Check if there only is one item of "New Group" in ktResources
            Assert.AreEqual(resources.Count(), 2);
            ktResources newResourceOne = resources[0];
            ktResources newResourceTwo = resources[1];

            //Check if new group has a ResourceTypeID equal to 1.
            //Means that the group is a group
            Assert.AreEqual(newResourceOne.ResourceTypeID, "1");
            Assert.AreEqual(newResourceTwo.ResourceTypeID, "1");

            //Does the old list contain the new groups?
            CollectionAssert.DoesNotContain(_oldResources, newResourceOne);
            CollectionAssert.DoesNotContain(_oldResources, newResourceTwo);

            #endregion

            #region ktUIResourceTranslation tests

            //Get the new groups in ktResourceTranslation
            List<ktResourceTranslation> resourceTranslationsOne =
                _impExcel._workSheetktResourceTranslation.ktResourceTranslationList.Where(
                    x => x.ResourceID.Equals(newResourceOne.ResourceID)).ToList();

            List<ktResourceTranslation> resourceTranslationsTwo =
                _impExcel._workSheetktResourceTranslation.ktResourceTranslationList.Where(
                    x => x.ResourceID.Equals(newResourceTwo.ResourceID)).ToList();

            //Check if there are four items of "New Group" in ktResourceTranslation
            Assert.AreEqual(resourceTranslationsOne.Count() + resourceTranslationsTwo.Count(), 4);

            //Check if the translation texts are right for the two "New Groups"
            foreach (ktResourceTranslation rt in resourceTranslationsOne)
            {
                if (rt.LanguageID.Equals("1"))
                {
                    Assert.AreEqual(rt.TranslationText, engTransText1);
                }
                else
                {
                    Assert.AreEqual(rt.TranslationText, danTransText1);
                }

                //Does the "New Group" exist in the old list
                CollectionAssert.DoesNotContain(_oldResTranslation, rt);
            }

            foreach (ktResourceTranslation rt in resourceTranslationsTwo)
            {
                if (rt.LanguageID.Equals("1"))
                {
                    Assert.AreEqual(rt.TranslationText, engTransText2);
                }
                else
                {
                    Assert.AreEqual(rt.TranslationText, danTransText2);
                }

                //Does the "New Group" exist in the old list
                CollectionAssert.DoesNotContain(_oldResTranslation, rt);
            }

            #endregion
        }

        #endregion

        #region Rename a group

        [TestMethod]
        public void RenameGroup()
        {
            //Rename a group
            string pageTypeId = "15";
            string groupTypeId = "0";
            string engTransText = "Rename group";
            string danTransText = "Omdøb gruppe";
            _wvm.RenameGroup(pageTypeId, groupTypeId, engTransText, danTransText);

            //Export to excel with the group in it
            _exExcel.CreateNewExcel(_exportPath);

            //Import the new excel file
            _impExcel.ImportExcelConfiguration(_exportPath);


        }

        #endregion

        [TestCleanup]
        public void CleanUp()
        {
            File.Delete(_exportPath);
        }
    }
}
