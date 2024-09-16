using Domain.Interfaces;

namespace Domain.Grains;

public class Event(ILogger<Event> logger) : IEvent
{
    private readonly ILogger _logger = logger;

    private readonly List<IPerson> _attendees = [];

    public string Name { get; private set; } = string.Empty;
    
    public IEnumerable<IPerson> Attendees => _attendees.AsReadOnly();

    async ValueTask<bool> IEvent.AddAttendee(IPerson attendee)
    {
        _attendees.Add(attendee);

        _logger.LogInformation($"{await attendee.GetNameAsync()} has joined the event: {Name}");

        return true;
    }

    ValueTask<string> IEvent.GetName()
    {
        return ValueTask.FromResult(Name);
    }

    Task IEvent.SetName(string value)
    {        
        Name = value;

        return Task.CompletedTask;
    }

    public async Task StartEvent()
    {
        _logger.LogInformation($"Event {Name} is starting!");

        foreach (var attendee in Attendees)
        {
            await attendee.DrinkAsync();

            _logger.LogInformation($"{await attendee.GetNameAsync()} had his welcome drink and is still sober.");
        }
    }
}