using System.Data;
using Dapper;
using Npgsql;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Motorcycle.Consumer;

public class MotorcycleEventService
{
    private readonly IConfiguration _configuration;

    public MotorcycleEventService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_configuration["ConnectionStrings:DatabaseSettings"]);
    }

    public void SaveEventToDatabase(MotorcycleRegisteredEvent motorcycleEvent)
    {
        using (var connection = GetConnection())
        {
            var query = @"
                INSERT INTO motorcycle_notifications (motorcycle_id, year, model, license_plate, created_at)
                VALUES (@MotorcycleId, @Year, @Model, @LicensePlate, @CreatedAt)";
            
            var parameters = new
            {
                MotorcycleId = motorcycleEvent.Id,
                Year = motorcycleEvent.Year,
                Model = motorcycleEvent.Model,
                LicensePlate = motorcycleEvent.LicensePlate,
                CreatedAt = DateTime.UtcNow
            };

            connection.Execute(query, parameters);
        }
    }
}