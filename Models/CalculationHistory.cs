using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorFO.Models;

public class CalculationHistory : INotifyPropertyChanged
{
    private string _expression;
    private string _result;
    private DateTime _timestamp;

    public string Expression
    {
        get => _expression;
        set
        {
            _expression = value;
            OnPropertyChanged();
        }
    }

    public string Result
    {
        get => _result;
        set
        {
            _result = value;
            OnPropertyChanged();
        }
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        set
        {
            _timestamp = value;
            OnPropertyChanged();
        }
    }

    public string DisplayText => $"{Expression} = {Result}";
    public string TimeDisplay => Timestamp.ToString("HH:mm:ss");

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
