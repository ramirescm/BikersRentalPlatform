namespace Motorcycle.Core.Entities;

public class Deliverer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateOnly BirthDate { get; set; }
    public string CnhNumber { get; set; }
    public string CnhType { get; set; }
    public string CnhPathImage { get; set; }
}