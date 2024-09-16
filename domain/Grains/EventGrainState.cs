using Domain.Interfaces;

namespace Domain.Grains;

public class EventGrainState
{
    public string Name { get; set; } = string.Empty;
    
    public List<IPerson> Attendees { get; set; } = [];
}