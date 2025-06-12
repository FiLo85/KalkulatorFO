using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KalkulatorFO.Models;

namespace KalkulatorFO.Services;

public class CalculatorService
{
    private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
    private static readonly CultureInfo DisplayCulture = new CultureInfo("en-US");
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
            // Normalizuj wyrażenie - zamień przecinki na kropki
            string normalizedExpression = NormalizeExpression(expression);

            var table = new DataTable();
            // Ustaw kulturę dla DataTable na InvariantCulture
            table.Locale = InvariantCulture;

            var result = table.Compute(normalizedExpression, null);
            if (result == DBNull.Value)
                throw new InvalidOperationException("Nieprawidłowe wyrażenie");

            double numericResult = Convert.ToDouble(result, InvariantCulture);

            // Formatuj wynik dla wyświetlenia (zawsze z kropką)
            string formattedResult = FormatResult(numericResult);

            AddToHistory(expression, formattedResult);
            return numericResult;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Błąd obliczenia: {ex.Message}");
        }
    }

    private string NormalizeExpression(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            return expression;

        // Zamień wszystkie przecinki na kropki dla obliczeń
        return expression.Replace(',', '.');
    }

    private string FormatResult(double result)
    {
        // Formatuj wynik zawsze z kropką jako separatorem dziesiętnym
        return result.ToString(DisplayCulture);
    }

    public string FormatNumber(double number)
    {
        return number.ToString(DisplayCulture);
    }

    public double ParseNumber(string input)
    {
        if (string.IsNullOrEmpty(input))
            return 0;

        // Normalizuj input - zamień przecinki na kropki
        string normalizedInput = input.Replace(',', '.');

        if (double.TryParse(normalizedInput, NumberStyles.Float, InvariantCulture, out double result))
        {
            return result;
        }

        return 0;
    }

    private void AddToHistory(string expression, string result)
    {
        var historyItem = new CalculationHistory
        {
            Expression = expression,
            Result = result,
            Timestamp = DateTime.Now
        };

        _history.Insert(0, historyItem);

        while (_history.Count > MaxHistoryItems)
        {
            _history.RemoveAt(_history.Count - 1); // Poprawka błędu składniowego
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