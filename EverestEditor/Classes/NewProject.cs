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
                checkValidationData();
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
                checkValidationData();
                OnPropertyChanged(nameof(ProjectPath));
            }
        }

        // for validity of path and name
        private bool m_isValid;
        public bool IsValid
        {
            get => m_isValid;
            set
            {
                if (m_isValid != value)
                {
                    m_isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        // for checking the error with the name and path of the choosen project
        private string m_errorMsg;
        public string ErrorMsg
        {
            get => m_errorMsg;
            set
            {
                if (m_errorMsg != value)
                {
                    m_errorMsg = value;
                    OnPropertyChanged(nameof(ErrorMsg));
                }
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
            checkValidationData();
        }

        // create new project on button click
        public string createNewProject(GameTemplate template) // returns path of created project
        {
            if (!checkValidationData())
            {
                return string.Empty;
            }

            if (!Path.EndsInDirectorySeparator(ProjectPath)) ProjectPath += @"\";
            string m_path = $@"{ProjectPath}{ProjectName}\";

            try
            {
                if (!Directory.Exists(m_path)) Directory.CreateDirectory(m_path);
                
                foreach (var folder in template.Folders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(m_path) , folder)));
                }

                // hide the folder meant to be hidden i.e. '.Everest'
                var dirInfo = new DirectoryInfo(m_path + @"\.Everest\");
                dirInfo.Attributes |= FileAttributes.Hidden; //hides the folder

                // copy the icon and splash file to .Everest directory
                File.Copy(template.IconPath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "icon.png")));
                File.Copy(template.SplashPath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "splash.png")));

                
                // create the project file - copy the project file to the new location.
                var m_project = File.ReadAllText(template.ProjectFilePath);
                
                m_project = string.Format(m_project, ProjectName, ProjectPath); // arrange the name and path according to the order as 0 and 1 in xml file.

                var projectPath = Path.GetFullPath(Path.Combine(m_path + $"{ProjectName}{Project.Extension}"));

                File.WriteAllText(projectPath, m_project);

                return m_path;
            }
            catch (Exception m_exception)
            {
                Debug.Write(m_exception.Message);
                // TODO - make a better log error system
                return String.Empty;
            }
        }

        // validate the project path and name
        private bool checkValidationData()
        {
            var m_path = ProjectPath;
            if (!Path.EndsInDirectorySeparator(m_path)) m_path += @"\";
            m_path += $@"{ProjectName}";

            IsValid = false;
            if (string.IsNullOrEmpty(ProjectName.Trim()))
            {
                ErrorMsg = "Invalid Project name!";
            }
            else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1) // or could say >= 0
            {
                ErrorMsg = "Invalid Characters in Project name!";
            }
            else if (string.IsNullOrEmpty(ProjectPath.Trim()))
            {
                ErrorMsg = "Invalid Path!";
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1) // or could say >= 0
            {
                ErrorMsg = "Invalid Characters in Project name!";
            }
            else if (Directory.Exists(m_path) && Directory.EnumerateFileSystemEntries(m_path).Any()) 
            {
                ErrorMsg = "Directory already exists and is not empty!";
            }
            else
            {
                ErrorMsg = string.Empty;
                IsValid = true;
            }

            return IsValid;
        }
    }
}
