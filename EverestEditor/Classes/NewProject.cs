using EverestEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
        public List<string> Folders { get; set; } // strings for foldernames to load from # assets and kinds
    }

    class NewProject : ViewModelBase // uses MVVM
    {
        // TODO : Use a path from Installation location instead of the below.
        private readonly string m_templatePath = @"..\..\EverestEditor\Templates\";

        // for name of the project
        private string m_name = "New Project";
        public string Name
        {
            get => m_name;
            set
            {
                m_name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        // for path
        private string m_path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\The Eddie Studio\Everest Projects";
        public string Path
        {
            get => m_path;
            set
            {
                m_path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        //constructer
        public NewProject()
        {
            try
            {
                var templates = Directory.GetFiles(m_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.WriteLine(templates.Length);
                Debug.Assert(templates.Any());
                foreach (var file in templates)
                {
                    var newTemplate = new GameTemplate()
                    {
                        ProjectFile = "project.ev",
                        ProjectType = "Empty Project",
                        Folders = new List<string> { ".Everest", "Content", "GameCode" }
                    };

                    Serializer.ToFile(newTemplate, file);

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
