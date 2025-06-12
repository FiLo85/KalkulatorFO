using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KalkulatorFO.Models;
using KalkulatorFO.Services;

namespace KalkulatorFO.ViewModels;

public class CalculatorViewModel : INotifyPropertyChanged
{
    private readonly CalculatorService _calculatorService;
    private string _display = "0";
    private string _currentExpression = "";
    private bool _isNewNumber = true;
    private bool _showHistory = false;

    public CalculatorViewModel()
    {
        _calculatorService = new CalculatorService();

        // Komendy
        NumberCommand = new Command<string>(OnNumberPressed);
        OperatorCommand = new Command<string>(OnOperatorPressed);
        EqualsCommand = new Command(OnEqualsPressed);
        ClearCommand = new Command(OnClearPressed);
        ClearEntryCommand = new Command(OnClearEntryPressed);
        BackspaceCommand = new Command(OnBackspacePressed);
        ToggleHistoryCommand = new Command(OnToggleHistory);
        ClearHistoryCommand = new Command(OnClearHistory);
        UseHistoryItemCommand = new Command<CalculationHistory>(OnUseHistoryItem);
    }

    public string Display
    {
        get => _display;
        set
        {
            _display = value;
            OnPropertyChanged();
        }
    }

    public string CurrentExpression
    {
        get => _currentExpression;
        set
        {
            _currentExpression = value;
            OnPropertyChanged();
        }
    }

    public bool ShowHistory
    {
        get => _showHistory;
        set
        {
            _showHistory = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<CalculationHistory> History => _calculatorService.History;

    // Komendy
    public ICommand NumberCommand { get; }
    public ICommand OperatorCommand { get; }
    public ICommand EqualsCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand ClearEntryCommand { get; }
    public ICommand BackspaceCommand { get; }
    public ICommand ToggleHistoryCommand { get; }
    public ICommand ClearHistoryCommand { get; }
    public ICommand UseHistoryItemCommand { get; }

    private void OnNumberPressed(string number)
    {
        if (number == "." && Display.Contains("."))
            return;

        if (_isNewNumber || Display == "0")
        {
            Display = number == "." ? "0." : number;
            _isNewNumber = false;
        }
        else
        {
            Display += number;
        }
    }

    private void OnOperatorPressed(string operatorSymbol)
    {
        if (!string.IsNullOrEmpty(CurrentExpression) && !_isNewNumber)
        {
            // Jeśli mamy już wyrażenie i nową liczbę, najpierw oblicz
            CurrentExpression += Display;
            try
            {
                var result = _calculatorService.Calculate(CurrentExpression);
                Display = result.ToString();
                CurrentExpression = Display + " " + operatorSymbol + " ";
            }
            catch
            {
                CurrentExpression = Display + " " + operatorSymbol + " ";
            }
        }
        else
        {
            CurrentExpression = Display + " " + operatorSymbol + " ";
        }

        _isNewNumber = true;
    }

    private void OnEqualsPressed()
    {
        if (string.IsNullOrEmpty(CurrentExpression) || _isNewNumber)
            return;

        try
        {
            var fullExpression = CurrentExpression + Display;
            var result = _calculatorService.Calculate(fullExpression);
            Display = result.ToString();
            CurrentExpression = "";
            _isNewNumber = true;
        }
        catch (Exception ex)
        {
            Display = "Błąd";
            CurrentExpression = "";
            _isNewNumber = true;
        }
    }

    private void OnClearPressed()
    {
        Display = "0";
        CurrentExpression = "";
        _isNewNumber = true;
    }

    private void OnClearEntryPressed()
    {
        Display = "0";
        _isNewNumber = true;
    }

    private void OnBackspacePressed()
    {
        if (Display.Length > 1)
        {
            Display = Display.Substring(0, Display.Length - 1);
        }
        else
        {
            Display = "0";
            _isNewNumber = true;
        }
    }

    private void OnToggleHistory()
    {
        ShowHistory = !ShowHistory;
    }

    private void OnClearHistory()
    {
        _calculatorService.ClearHistory();
    }

    private void OnUseHistoryItem(CalculationHistory historyItem)
    {
        if (historyItem != null)
        {
            Display = historyItem.Result;
            CurrentExpression = "";
            _isNewNumber = true;
            ShowHistory = false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
