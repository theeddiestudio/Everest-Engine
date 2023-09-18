using EverestEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EverestEditor.ProjectBrowser
{
    [DataContract]
    public class GameTemplate
    {
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public List<string> Folders { get; set; } // strings for foldernames to load from # assets and kinds
        public string ProjectFilePath { get; set; } // paths are used for after the engine is released to hold template file path
        public byte[] Icon { get; set; } // binary array for icons
        public string IconPath { get; set; }
        public byte[] Splash { get; set; }
        public string SplashPath { get; set; }
    }

    class NewProject : ViewModelBase // uses MVVM
    {
        // TODO : Use a path from Installation location instead of the below.
        private readonly string m_templatePath = @"..\..\EverestEditor\Templates\";

        // for name of the project
        private string m_name = "Project Name";
        public string ProjectName
        {
            get => m_name;
            set
            {
                m_name = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        // for path
        private string m_path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\The Eddie Studio\Everest Projects";
        public string ProjectPath
        {
            get => m_path;
            set
            {
                m_path = value;
                OnPropertyChanged(nameof(ProjectPath));
            }
        }

        // special kind of list that can be used to bind to objects is known as observable collection
        private ObservableCollection<GameTemplate> m_templates = new ObservableCollection<GameTemplate>();

        public ReadOnlyObservableCollection<GameTemplate> Templates { get; } // outer world cannot change it.

        //constructer
        public NewProject()
        {
            Templates = new ReadOnlyObservableCollection<GameTemplate>(m_templates);
            try
            {
                var templates = Directory.GetFiles(m_templatePath, "template.xml", SearchOption.AllDirectories);
                
                Debug.Assert(templates.Any());
                foreach (var file in templates)
                {
                    // old code generates Directory Not found exception
                    // var template = Serializer.ReadFiles<GameTemplate>(m_templatePath);
         
                    var template = Serializer.ReadFiles<GameTemplate>(file);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));
                    template.IconPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconPath);
                    template.SplashPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "splash.png"));
                    template.Splash = File.ReadAllBytes(template.SplashPath);

                    m_templates.Add(template);
                }
            }
            catch (Exception m_exception)
            {
                Debug.Write(m_exception.Message);
                // TODO - make a better log error system
            }
        }
    }
}
