using System;
using System.Collections.Generic;
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

namespace TreatPraktik
{
    /// <summary>
    /// Interaction logic for ucListOfItemsFilter.xaml
    /// </summary>
    public partial class ItemFilter : UserControl
    {
        public ItemFilter()
        {
            InitializeComponent();
            DataContext = new ItemFilterViewModel();
        }
    }
}
