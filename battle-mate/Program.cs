using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace battle_mate
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton<Dice>();

            await builder.Build().RunAsync();
        }
    }

    public class Dice
    {
        private Random _random;
        
        public Dice(int seed = 0)
        {
            _random = seed == default ? new Random() : new Random(seed);
        }        
        
        public List<int> Roll(int count, int sides)
        {
            var rolls = new List<int>();
            for (int i = 0; i < count; i++)
            {
                rolls.Add(_random.Next(1, sides));
            }
            
            rolls.Sort();
            return rolls;
        }
    }
}