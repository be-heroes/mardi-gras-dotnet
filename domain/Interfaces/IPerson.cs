namespace Domain.Interfaces;

[Alias("Domain.Interfaces.IPerson")]
public interface IPerson : IGrainWithIntegerKey
{
    [Alias("GetNameAsync")]
    Task<string> GetNameAsync();
    
    [Alias("SetNameAsync")]
    Task SetNameAsync(string name);

    [Alias("EatAsync")]
    Task EatAsync();

    [Alias("DrinkAsync")]
    Task DrinkAsync();

    [Alias("PartyAsync")]
    Task PartyAsync();

    [Alias("SleepAsync")]
    Task SleepAsync();

    [Alias("GetStatusAsync")]
    Task<string> GetStatusAsync();

    [Alias("SubscribeToEventsAsync")]
    Task SubscribeToEventsAsync();

    [Alias("HandleEventAsync")]
    Task HandleEventAsync(string eventMessage);
}