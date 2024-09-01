namespace Motorcycle.Core.Entities;

public abstract class Vehicle
{
    public Vehicle()
    {
    }

    protected Vehicle(Guid id, int year, string model, string licensePlate)
    {
        Id = id;
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
    }

    public Guid Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
}