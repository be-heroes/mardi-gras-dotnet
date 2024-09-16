using Domain.Interfaces;

namespace Domain.Grains;

public class Event(ILogger<Event> logger) : Grain<EventGrainState>, IEvent
{
    private readonly ILogger _logger = logger;
    
    public IEnumerable<IPerson> Attendees => State.Attendees.AsReadOnly();

    public async ValueTask<bool> AddAttendeeAsync(IPerson attendee)
    {
        State.Attendees.Add(attendee);
        
        await WriteStateAsync();

        _logger.LogInformation($"{await attendee.GetNameAsync()} has joined the event: {State.Name}");

        return true;
    }

    public Task<string> GetNameAsync()
    {
        return Task.FromResult(State.Name);
    }

    public async Task SetNameAsync(string value)
    {        
        State.Name = value;

        await WriteStateAsync();
    }

    public async Task StartEventAsync()
    {
        _logger.LogInformation($"Event {State.Name} is starting!");

        var drinkTasks = State.Attendees.Select(async attendee =>
        {
            await attendee.DrinkAsync();

            _logger.LogInformation($"{await attendee.GetNameAsync()} had his welcome drink and is still sober.");
        });

        await Task.WhenAll(drinkTasks);
    }
}