using System.Windows;
using CHWGameEngine;

namespace WPFTestEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        private GameWorld gw = new GameWorld(@"C:\Users\Chris\Google Drev\maptest.txt");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
