using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Infotecs.MiniJournal.Application.Views
{
    /// <summary>
    /// Interaction logic for AddCommentView.xaml
    /// </summary>
    public partial class AddCommentView : Window
    {
        public AddCommentView()
        {
            InitializeComponent();
        }

        private void CloseAfterButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
