using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TreatPraktik.ViewModel;

namespace TreatPraktik.View
{
    /// <summary>
    /// Interaction logic for LanguageUserControl.xaml
    /// </summary>
    public partial class LanguageUserControl : UserControl, INotifyPropertyChanged
    {
        private string languageID;
        private LanguageViewModel languageWM;

        public LanguageUserControl()
        {
            InitializeComponent();
            languageWM = new LanguageViewModel();
        }

        private void Button_Click_UK(object sender, RoutedEventArgs e)
        {
            LanguageID = "1";
            //OnPropertyChanged("LanguageID");
        }

        private void Button_Click_DK(object sender, RoutedEventArgs e)
        {
            LanguageID = "2";
            //OnPropertyChanged("LanguageID");
        }

        public string LanguageID
        {
            set
            {
                languageID = value;
                languageWM.ChangeLanguage(LanguageID);
            }
            get
            {
                return languageID;
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

    }
}
