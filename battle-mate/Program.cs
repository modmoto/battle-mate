using System.Threading.Tasks;
using battle_mate.Pages.BattleResult;
using battle_mate.Pages.ProbbilityCalculator;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace battle_mate;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        builder.Services.AddSingleton<Dice>();
        builder.Services.AddSingleton<DiceState>();
        builder.Services.AddSingleton<CombatResultState>();
        builder.Services.AddSingleton<ProbabilityState>();
        builder.Services.AddTransient<LocalStorageRepository>();
        builder.Services.AddBlazoredLocalStorage();

        await builder.Build().RunAsync();
    }
}