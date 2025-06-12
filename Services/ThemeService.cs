using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KalkulatorFO.Services;

public enum AppTheme
{
    Light,
    Dark
}

public class ThemeService : INotifyPropertyChanged
{
    private AppTheme _currentTheme = AppTheme.Light;
    private bool _isDarkMode;
    public string ThemeButtonText => IsDarkMode ? "jasny" : "Ciemny";

    public static ThemeService Instance { get; } = new ThemeService();

    public AppTheme CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                OnPropertyChanged();
                ApplyTheme();
            }
        }
    }

    public bool IsDarkMode
    {
        get => _isDarkMode;
        private set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLightMode));
            }
        }
    }

    public bool IsLightMode => !_isDarkMode;

    // Kolory dla trybu jasnego
    public Color LightBackgroundColor => Color.FromArgb("#FFFFFF");
    public Color LightDisplayBackgroundColor => Color.FromArgb("#FFFFFF");
    public Color LightTextColor => Color.FromArgb("#000000");
    public Color LightSecondaryTextColor => Color.FromArgb("#666666");
    public Color LightButtonBackgroundColor => Color.FromArgb("#F5F5F5");
    public Color LightButtonTextColor => Color.FromArgb("#000000");
    public Color LightOperatorButtonColor => Color.FromArgb("#FF9500");
    public Color LightFunctionButtonColor => Color.FromArgb("#A6A6A6");

    // Kolory dla trybu ciemnego
    public Color DarkBackgroundColor => Color.FromArgb("#000000");
    public Color DarkDisplayBackgroundColor => Color.FromArgb("#1C1C1C");
    public Color DarkTextColor => Color.FromArgb("#FFFFFF");
    public Color DarkSecondaryTextColor => Color.FromArgb("#999999");
    public Color DarkButtonBackgroundColor => Color.FromArgb("#333333");
    public Color DarkButtonTextColor => Color.FromArgb("#FFFFFF");
    public Color DarkOperatorButtonColor => Color.FromArgb("#FF9500");
    public Color DarkFunctionButtonColor => Color.FromArgb("#A6A6A6");

    // Aktualnie używane kolory
    public Color BackgroundColor => IsDarkMode ? DarkBackgroundColor : LightBackgroundColor;
    public Color DisplayBackgroundColor => IsDarkMode ? DarkDisplayBackgroundColor : LightDisplayBackgroundColor;
    public Color TextColor => IsDarkMode ? DarkTextColor : LightTextColor;
    public Color SecondaryTextColor => IsDarkMode ? DarkSecondaryTextColor : LightSecondaryTextColor;
    public Color ButtonBackgroundColor => IsDarkMode ? DarkButtonBackgroundColor : LightButtonBackgroundColor;
    public Color ButtonTextColor => IsDarkMode ? DarkButtonTextColor : LightButtonTextColor;
    public Color OperatorButtonColor => IsDarkMode ? DarkOperatorButtonColor : LightOperatorButtonColor;
    public Color FunctionButtonColor => IsDarkMode ? DarkFunctionButtonColor : LightFunctionButtonColor;

    private ThemeService()
    {
        // Inicjalizacja z motywem jasnym
        IsDarkMode = false;
    }

    private void ApplyTheme()
    {
        switch (CurrentTheme)
        {
            case AppTheme.Light:
                IsDarkMode = false;
                Application.Current.UserAppTheme = Microsoft.Maui.ApplicationModel.AppTheme.Light;
                break;
            case AppTheme.Dark:
                IsDarkMode = true;
                Application.Current.UserAppTheme = Microsoft.Maui.ApplicationModel.AppTheme.Dark;
                break;
        }

        // Powiadom o zmianie wszystkich kolorów
        OnPropertyChanged(nameof(BackgroundColor));
        OnPropertyChanged(nameof(DisplayBackgroundColor));
        OnPropertyChanged(nameof(TextColor));
        OnPropertyChanged(nameof(SecondaryTextColor));
        OnPropertyChanged(nameof(ButtonBackgroundColor));
        OnPropertyChanged(nameof(ButtonTextColor));
        OnPropertyChanged(nameof(OperatorButtonColor));
        OnPropertyChanged(nameof(FunctionButtonColor));
        OnPropertyChanged(nameof(ThemeButtonText));
    }

    public void ToggleTheme()
    {
        CurrentTheme = CurrentTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}