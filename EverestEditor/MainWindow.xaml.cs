using EverestEditor.ProjectBrowser;
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

namespace EverestEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded; // load the function when the window is loaded
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded; // detach the loaded so that the window is not loaded again
            OpenProjectBrowserWindow();
        }
        private void OpenProjectBrowserWindow()
        {
            var projectBrowser = new ProjectBrowser.ProjectBrowser();
            if (projectBrowser.ShowDialog() == false)
            {
                Application.Current.Shutdown(); // close the main window if dialog window is closed too.
            }
            else
            {
                // import the project newly created or existing ones
            }
        }
    }
}
