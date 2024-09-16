using Domain.Interfaces;
using Orleans.Streams;
using Orleans.Streaming;

namespace Domain.Grains;

public class Person(ILogger<Person> logger) : Grain<PersonGrainState>, IPerson
{
    private readonly ILogger<Person> _logger = logger;
    private IAsyncStream<string> _eventStream;

    public Task<string> GetNameAsync()
    {
        return Task.FromResult(State.Name);
    }

    public async Task SetNameAsync(string name)
    {
        State.Name = name;

        await WriteStateAsync();
    }

    public async Task EatAsync()
    {
        _logger.LogInformation($"{State.Name} is eating.");
        State.Energy = Math.Min(State.Energy + 10, 100);

        EnsureEnergyWithinBounds();
        
        await WriteStateAsync();
    }

    public async Task DrinkAsync()
    {
        if (State.Energy < 5) throw new InvalidOperationException($"{State.Name} does not have enough energy to drink.");
        
        _logger.LogInformation($"{State.Name} is drinking.");        
        State.Energy -= 5;

        EnsureEnergyWithinBounds();
        
        await WriteStateAsync();
    }

    public async Task PartyAsync()
    {
        if (State.Energy < 20) throw new InvalidOperationException($"{State.Name} does not have enough energy to party.");

        _logger.LogInformation($"{State.Name} is partying.");
        State.Energy -= 20;

        EnsureEnergyWithinBounds();
        
        await WriteStateAsync();
    }

    public async Task SleepAsync()
    {
        _logger.LogInformation($"{State.Name} is sleeping.");
        State.Energy = Math.Min(State.Energy + 30, 100);

        EnsureEnergyWithinBounds();
        
        await WriteStateAsync();
    }

    public Task<string> GetStatusAsync()
    {
        return Task.FromResult(ToString());
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken = default)
    {
        var streamProvider = GrainStreamingExtensions.GetStreamProvider(this, "Default");
        
        _eventStream = streamProvider.GetStream<string>(this.GetPrimaryKey());

        await SubscribeToEventsAsync();
        await base.OnActivateAsync(cancellationToken);
    }

    public async Task SubscribeToEventsAsync()
    {
        await _eventStream.SubscribeAsync((data, token) =>
        {
            return HandleEventAsync(data);
        });
    }

    public Task HandleEventAsync(string eventMessage)
    {
        _logger.LogInformation($"Received event: {eventMessage}");
        // Handle the event (e.g., update state, trigger actions)
        return Task.CompletedTask;
    }

    public override string ToString()
    {
        return $"{State.Name} (Energy: {State.Energy})";
    }

    private void EnsureEnergyWithinBounds()
    {
        if (State.Energy > 100) State.Energy = 100;
        if (State.Energy < 0) State.Energy = 0;
    }
}