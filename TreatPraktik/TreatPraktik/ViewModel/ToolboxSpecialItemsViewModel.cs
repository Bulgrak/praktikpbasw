using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DocumentFormat.OpenXml.Office.CustomUI;
using TreatPraktik.Model;
using TreatPraktik.Model.WorkspaceObjects;

namespace TreatPraktik.ViewModel
{
    public class ToolboxSpecialItemsViewModel
    {
        private string filterString;
        private string textSearchDescription;
        public string Language { get; set; }
        public string TextNoToolboxItemsFound { get; set; }
        //public string TextSearchDescription { get; set; } //Beskrivelse
        public ICollectionView SpecialItemsView { get; set; }

        public ToolboxSpecialItemsViewModel()
        {
            CreateSpecialItems();
        }

        public void CreateSpecialItems()
        {
            List<ToolboxItem> toolboxItemList = new List<ToolboxItem>();
            

            ToolboxItem tbiNewLine = new ToolboxItem();
            ItemType itNewLine = new ItemType();
            itNewLine.DesignID = "198";
            itNewLine.Header = "<NewLineItem>";
            tbiNewLine.ItemType = itNewLine;

            ToolboxItem tbiEmptyField = new ToolboxItem();
            ItemType itEmptyField = new ItemType();
            itEmptyField.DesignID = "197";
            itEmptyField.Header = "<EmptyField>";
            tbiEmptyField.ItemType = itEmptyField;

            toolboxItemList.Add(tbiNewLine);
            toolboxItemList.Add(tbiEmptyField);

            SpecialItemsView = CollectionViewSource.GetDefaultView(toolboxItemList);
        }

        public void PopulateLstSpecialItems()
        {
            
        }

    }
}
