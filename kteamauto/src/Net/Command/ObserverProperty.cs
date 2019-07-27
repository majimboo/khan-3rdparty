using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KhanEngine
{
    public class ObserverProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged<T>(ref T property, T value, [CallerMemberName] string name = "")
        {
            property = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CallerMemberNameAttribute : Attribute
    {
    }
}