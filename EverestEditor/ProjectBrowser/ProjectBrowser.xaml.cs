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
using System.Windows.Shapes;

namespace EverestEditor.ProjectBrowser
{
    /// <summary>
    /// Interaction logic for ProjectBrowser.xaml
    /// </summary>
    public partial class ProjectBrowser : Window
    {
        public ProjectBrowser()
        {
            InitializeComponent();
            Activated += MainWindow_Activated; // used to put the window on focus everytime it is initialized.
        }

        private void MainWindow_Activated(object sender, System.EventArgs e)
        {
            // Bring the window into focus when it is activated.
            Activate();
        }

        private void browseProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            // check for the last time if correct button is clicked
            if (sender != browseProjectBtn)
            {
                return;
            }

            // if click is initiated from createProjectBtn
            if (createProjectBtn.IsChecked == true)
            {
                createProjectBtn.IsChecked = false;
                browserStack.Margin = new Thickness(0);
            }
            browseProjectBtn.IsChecked = true;

        }

        private void createProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            // check for the last time if correct button is clicked
            if (sender != createProjectBtn)
            {
                return;
            }

            // if click is initiated from browseProjectBtn
            if (browseProjectBtn.IsChecked == true)
            {
                browseProjectBtn.IsChecked = false;
                browserStack.Margin = new Thickness(0, -450, 0, 0);
            }
            createProjectBtn.IsChecked = true;

        }
    }
}