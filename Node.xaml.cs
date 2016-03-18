using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkyView {
    /// <summary>
    /// Logique d'interaction pour Node.xaml
    /// </summary>
    public partial class Node : UserControl {
        public Node() {
            InitializeComponent();
        }

        private void TitleContainer_TextChanged(object sender, TextChangedEventArgs e) {
            Size size = TextRenderer.MeasureText(TitleContainer.Text, TitleContainer.Font);
            TitleContainer.Width = size.Width;
            TitleContainer.Heigth = size.Height;
            RectTitle.Width = size.Width + 4;
            RectTitle.Height = size.Height + 4;
        }
    }
}
