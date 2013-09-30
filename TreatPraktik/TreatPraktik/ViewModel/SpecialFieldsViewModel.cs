using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TreatPraktik.Model;

namespace TreatPraktik.ViewModel
{
    public class SpecialFieldsViewModel
    {
        private string filterString;
        private string textSearchDescription;
        public string Language { get; set; }
        public string TextNoToolboxItemsFound { get; set; }
        //public string TextSearchDescription { get; set; } //Beskrivelse
        public ICollectionView SpecialItemsView { get; set; }

        public SpecialFieldsViewModel()
        {
            CreateSpecialItems();
        }

        public void CreateSpecialItems()
        {
            List<ToolboxItem> toolboxItemList = new List<ToolboxItem>();
            

            ToolboxItem tbiNewLine = new ToolboxItem();
            tbiNewLine.DesignID = "198";
            tbiNewLine.Header = "<NewLineItem>";

            ToolboxItem tbiEmptyField = new ToolboxItem();
            tbiEmptyField.DesignID = "197";
            tbiEmptyField.Header = "<EmptyField>";

            toolboxItemList.Add(tbiNewLine);
            toolboxItemList.Add(tbiEmptyField);

            SpecialItemsView = CollectionViewSource.GetDefaultView(toolboxItemList);
        }

        public void PopulateLstSpecialItems()
        {
            
        }

    }
}
