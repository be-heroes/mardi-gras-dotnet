namespace Domain.Grains;

public class PersonGrainState
{
    public string Name { get; set; } = string.Empty;

    public int Energy { get; set; } = 100;
}