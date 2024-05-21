namespace Domain.Interfaces;

public interface IEvent : IGrainWithIntegerKey
{
    ValueTask<bool> AddAttendee(IPerson attendee);

    ValueTask<string> GetName();

    Task SetName(string value);

    Task StartEvent();
}