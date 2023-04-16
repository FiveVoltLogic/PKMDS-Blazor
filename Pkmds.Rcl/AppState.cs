﻿namespace Pkmds.Rcl;

public record AppState : IAppState
{
    public AppState()
    {
        GameInfo.Strings = GameInfo.GetStrings(CurrentLanguage);
    }

    public string CurrentLanguage
    {
        get => currentLanguage;
        set
        {
            currentLanguage = value;
            LocalizeUtil.InitializeStrings(CurrentLanguage, SaveFile);
        }
    }

    public int CurrentLanguageId { get; set; } = (int)LanguageID.English;

    public string[] GenderForms => new[] { string.Empty, "F", string.Empty };

    public event Action? OnAppStateChanged;

    public SaveFile? SaveFile
    {
        get => saveFile;
        set
        {
            saveFile = value;
            LocalizeUtil.InitializeStrings(CurrentLanguage, SaveFile);
        }
    }

    private string currentLanguage = GameLanguage.DefaultLanguage;
    private SaveFile? saveFile;
    private PKM? _selectedPokemon;

    public PKM? SelectedPokemon
    {
        get => _selectedPokemon;
        set
        {
            _selectedPokemon = value;
            LoadPokemonStats();
        }
    }

    public int? SelectedBoxSlot { get; set; }

    public bool ShowProgressIndicator { get; set; }

    public string FileDisplayName { get; set; } = string.Empty;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IEnumerable<ushort>> SearchPokemonNames(string searchString) => SaveFile is null
        ? Enumerable.Empty<ushort>()
        : GameInfo.FilteredSources.Species
            .Where(species => species.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            .OrderBy(species => species.Text)
            .Select(species => (ushort)species.Value);
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    public string ConvertSpeciesToName(ushort species) =>
        SpeciesName.GetSpeciesName(species, CurrentLanguageId);

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IEnumerable<int>> SearchItemNames(string searchString) => SaveFile is null
        ? Enumerable.Empty<int>()
        : GameInfo.FilteredSources.Items
            .Where(item => item.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            .OrderBy(item => item.Text)
            .Select(item => item.Value);
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    public string ConvertItemToName(int itemId) => GameInfo.FilteredSources.Items
        .FirstOrDefault(item => item.Value == itemId)?.Text ?? string.Empty;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IEnumerable<int>> SearchAbilityNames(string searchString) => SaveFile is null
        ? Enumerable.Empty<int>()
        : GameInfo.FilteredSources.Abilities
            .Where(ability => ability.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            .OrderBy(ability => ability.Text)
            .Select(ability => ability.Value);
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    public string ConvertAbilityToName(int abilityId) => GameInfo.FilteredSources.Abilities
        .FirstOrDefault(ability => ability.Value == abilityId)?.Text ?? string.Empty;

    public void Refresh() => OnAppStateChanged?.Invoke();

    public void ClearSelection()
    {
        SelectedPokemon = null;
        SelectedBoxSlot = null;
        Refresh();
    }

    public string[] NatureStatShortNames => new[] { "Atk", "Def", "Spe", "SpA", "SpD" };

    public string GetStatModifierString(int nature)
    {
        var (up, down) = NatureAmp.GetNatureModification(nature);
        return up == down ? string.Empty : $"({NatureStatShortNames[up]} ↑, {NatureStatShortNames[down]} ↓)";
    }

    public void LoadPokemonStats()
    {
        if (SaveFile is null || _selectedPokemon is null)
        {
            return;
        }

        var pt = SaveFile.Personal;
        var pi = pt.GetFormEntry(_selectedPokemon.Species, _selectedPokemon.Form);
        Span<ushort> stats = stackalloc ushort[6];
        _selectedPokemon.LoadStats(pi, stats);
        _selectedPokemon.SetStats(stats);
    }
}
