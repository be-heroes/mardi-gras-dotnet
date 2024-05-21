using Domain.Grains;

namespace Domain.Interfaces;

public interface IPerson : IGrainWithIntegerKey
{
    ValueTask<string> Do(ActionType action);

    ValueTask<string> GetName();

    Task SetName(string value);
}