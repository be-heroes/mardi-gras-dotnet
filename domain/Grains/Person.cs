using Domain.Interfaces;

namespace Domain.Grains;

public class Person(ILogger<Person> logger) : IPerson
{
    private readonly ILogger _logger = logger;

    public string Name { get; private set; } = string.Empty;

    public int Energy { get; private set; } = 100;

    ValueTask<string> IPerson.Do(ActionType action)
    {
        switch (action)
        {
            case ActionType.Eat:
                Eat();
                break;
            case ActionType.Drink:
                Drink();
                break;
            case ActionType.Party:
                Party();
                break;
            case ActionType.Sleep:
                Sleep();
                break;
            default:
                throw new ArgumentException("Invalid action type.");
        }

        return ValueTask.FromResult($"{Name} has {Energy} energy left after {action}.");
    }

    ValueTask<string> IPerson.GetName()
    {
        return ValueTask.FromResult(Name);
    }

    Task IPerson.SetName(string value)
    {
        Name = value;

        return Task.CompletedTask;
    }

    void Eat()
    {
        _logger.LogInformation($"{Name} is eating.");

        Energy += 10;
        
        if (Energy > 100) Energy = 100;
    }

    void Drink()
    {
        if(Energy - 5 <= 0) throw new InvalidOperationException($"{Name} does not have enough energy to drink.");
        
        _logger.LogInformation($"{Name} is drinking.");
        
        Energy -= 5;
    }

    void Party()
    {
        if(Energy - 20 <= 0) throw new InvalidOperationException($"{Name} does not have enough energy to party.");

        _logger.LogInformation($"{Name} is partying.");
        
        Energy -= 20;
    }

    void Sleep()
    {
        _logger.LogInformation($"{Name} is sleeping.");
        
        Energy += 30;

        if (Energy > 100) Energy = 100;
    }

    public override string ToString()
    {
        return $"{Name} (Energy: {Energy})";
    }
}