using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EverestEditor
{
    [DataContract(Name ="Game")]
    public class Project : ViewModelBase
    {
        public static string Extension { get; } = ".ev";
        [DataMember]
        public string Name { get; private set; } // name of the project
        [DataMember]
        public string Path { get; private set; }
        public string FullPath => $"{Path}{Name}{Extension}";
        
        [DataMember(Name = "Scenes")]
        private ObservableCollection<Scene> m_scenes = new ObservableCollection<Scene> ();
        public ReadOnlyObservableCollection<Scene> Scenes { get; }

        public Project(string m_name, string m_path)
        {
            Name = m_name;
            Path = m_path;

            m_scenes.Add(new Scene(this, "Default Scene"));
        }

    }
}
