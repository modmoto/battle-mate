using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace battle_mate.Pages.BattleResult;

public class CombatOptionsRepository
{
    private readonly ILocalStorageService _localStorageService;
    private string _optionKey = "combat-options";
    private string _probabilityKey = "probability-chains";

    public CombatOptionsRepository(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
        
    public async Task<CombatOptions> LoadOptions()
    {
        var itemAsync = await _localStorageService.GetItemAsync<CombatOptions>(_optionKey);
        return itemAsync ?? new CombatOptions();
    }
        
    public Task SetOptions(CombatOptions options)
    {
        return _localStorageService.SetItemAsync(_optionKey, options).AsTask();
    }
        
    public async Task<List<ProbabilityChain>> LoadProbabilities()
    {
        var itemAsync = await _localStorageService.GetItemAsync<List<ProbabilityChain>>(_probabilityKey);
        return itemAsync ?? new List<ProbabilityChain>();
    }
        
    public Task SetOptions(List<ProbabilityChain> options)
    {
        return _localStorageService.SetItemAsync(_probabilityKey, options).AsTask();
    }
}