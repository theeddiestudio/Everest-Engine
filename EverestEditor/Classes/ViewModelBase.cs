using System.ComponentModel;
using System.Runtime.Serialization;

namespace EverestEditor
{
    [DataContract(IsReference = true)] // isReference makes it so that the data members are not duplicated when one of its instance is created on a DataContract
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}