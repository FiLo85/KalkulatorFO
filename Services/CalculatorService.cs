using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KalkulatorFO.Models;

namespace KalkulatorFO.Services;

public class CalculatorService
{
    private readonly ObservableCollection<CalculationHistory> _history;
    private const int MaxHistoryItems = 50;

    public ObservableCollection<CalculationHistory> History => _history;

    public CalculatorService()
    {
        _history = new ObservableCollection<CalculationHistory>();
    }

    public double Calculate(string expression)
    {
        try
        {
            // Używamy DataTable.Compute do bezpiecznego obliczania wyrażeń
            var table = new DataTable();
            var result = table.Compute(expression, null);

            if (result == DBNull.Value)
                throw new InvalidOperationException("Nieprawidłowe wyrażenie");

            double numericResult = Convert.ToDouble(result);

            // Dodaj do historii
            AddToHistory(expression, numericResult.ToString());

            return numericResult;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Błąd obliczenia: {ex.Message}");
        }
    }

    private void AddToHistory(string expression, string result)
    {
        var historyItem = new CalculationHistory
        {
            Expression = expression,
            Result = result,
            Timestamp = DateTime.Now
        };

        // Dodaj na początek listy
        _history.Insert(0, historyItem);

        // Ogranicz liczbę elementów w historii
        while (_history.Count > MaxHistoryItems)
        {
            _history.RemoveAt(_history.Count - 1);
        }
    }

    public void ClearHistory()
    {
        _history.Clear();
    }

    public string GetHistoryItem(int index)
    {
        if (index >= 0 && index < _history.Count)
        {
            return _history[index].Result;
        }
        return string.Empty;
    }
}