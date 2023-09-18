using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EverestEditor
{
    [DataContract]
    public class Scene : ViewModelBase
    {
        private string m_name;
        [DataMember]
        public string Name 
        {
            get => m_name;
            set
            { 
                if (m_name != value)
                {
                    m_name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        [DataMember]
        public Project Project { get; private set; }

        // scene will also have a list of game entities, but for now we are just creating a empty scene with no game entities.
        // TODO: Add game entities

        public Scene(Project m_project, string m_name)
        {
            Debug.Assert(m_project != null);
            Project = m_project;
            Name = m_name;
        }
    }
}
