namespace Domain.Interfaces;

[Alias("Domain.Interfaces.IEvent")]
public interface IEvent : IGrainWithIntegerKey
{
    [Alias("GetNameAsync")]
    Task<string> GetNameAsync();
    
    [Alias("SetNameAsync")]
    Task SetNameAsync(string name);

    [Alias("AddAttendeeAsync")]
    ValueTask<bool> AddAttendeeAsync(IPerson attendee);
    
    [Alias("StartEventAsync")]
    Task StartEventAsync();
}