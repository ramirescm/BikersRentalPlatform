namespace Motorcycle.Consumer;

public class MotorcycleRegisteredEvent
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
}