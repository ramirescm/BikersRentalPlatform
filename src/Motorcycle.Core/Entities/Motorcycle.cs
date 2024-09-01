namespace Motorcycle.Core.Entities;

public class Motorcycle : Vehicle
{
    public Motorcycle()
    {
    }

    public Motorcycle(Guid id, int year, string model, string licensePlate)
        : base(id, year, model, licensePlate)
    {
    }
}