using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Domain.Interfaces;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(client =>
    {
        client.UseLocalhostClustering();
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime();

using IHost host = builder.Build();

Console.WriteLine("Starting client");

await host.StartAsync();

IClusterClient client = host.Services.GetRequiredService<IClusterClient>();

IEvent @event = client.GetGrain<IEvent>(0);

await @event.SetNameAsync("My Birthday Party");

IPerson person = client.GetGrain<IPerson>(0);

await person.SetNameAsync("Tobias Andersen");

await person.DrinkAsync();

try {
    await person.DrinkAsync();
    await person.PartyAsync();
    await person.DrinkAsync();
    await person.EatAsync();
    await person.PartyAsync();
    await person.DrinkAsync();
    await person.PartyAsync();
    await person.DrinkAsync();
    await person.PartyAsync();
    await person.PartyAsync();
}
catch
{
    await person.SleepAsync();        
}

await @event.AddAttendeeAsync(person);

await @event.StartEventAsync();

Console.WriteLine("Finished partying. Press any key to continue...");
Console.ReadKey();

await host.StopAsync();