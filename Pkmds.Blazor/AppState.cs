﻿namespace Pkmds.Blazor;

public class AppState : IAppState
{
    public event Action? OnAppStateChanged;

    public SaveFile? SaveFile { get; set; }

    public PKM? SelectedPokemon { get; set; }
    public void Refresh() => OnAppStateChanged?.Invoke();
}
