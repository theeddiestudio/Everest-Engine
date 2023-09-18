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
using EverestEditor.ProjectBrowser;

namespace EverestEditor.ProjectBrowser
{
    /// <summary>
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    public partial class NewProjectView : UserControl
    {
        public NewProjectView()
        {
            InitializeComponent();

            // give a default name to the project when no template is selected
            // not going to be used cause blank project is made default.
            /*
            if (templateName.Text == "")
            {
                templateName.Text = "ProjectApp";
            } */
        }

        private void onButtonClickCreateProject(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as NewProject; // a data context meant for this control
            var projectPath = vm.createNewProject(templatesListBox.SelectedItem as GameTemplate);
            bool dialogResult = false;
            var win = Window.GetWindow(this);

            if (!string.IsNullOrEmpty(projectPath) )
            {
                dialogResult = true;
            }

            win.DialogResult = dialogResult;
            win.Close();
        }
    }
}
