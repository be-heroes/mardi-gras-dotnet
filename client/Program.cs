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
await host.StartAsync();

IClusterClient client = host.Services.GetRequiredService<IClusterClient>();

IEvent @event = client.GetGrain<IEvent>(0);

await @event.SetName("My Birthday Party");

IPerson person = client.GetGrain<IPerson>(0);

await person.SetName("Tobias Andersen");

string response = await person.Do(Domain.Grains.ActionType.Drink);

try{
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Drink)}""");
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Party)}""");
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Drink)}""");
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Eat)}""");
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Party)}""");
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Drink)}""");
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Party)}""");
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Drink)}""");
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Party)}""");

    // I will run out of energy here and need sleep :)
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Party)}""");
}
catch
{
    Console.WriteLine($"""{await person.Do(Domain.Grains.ActionType.Sleep)}""");        
}

await @event.AddAttendee(person);

await @event.StartEvent();

Console.ReadKey();

await host.StopAsync();