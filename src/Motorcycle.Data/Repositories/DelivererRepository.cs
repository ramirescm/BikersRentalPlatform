using Microsoft.EntityFrameworkCore;
using Motorcycle.Core.Entities;
using Motorcycle.Core.Repositories;
using Motorcycle.Shared.Dtos;
using Motorcycle.Shared.Exceptions;

namespace Motorcycle.Data.Repositories;

public class DelivererRepository : Repository<Deliverer>, IDelivererRepository
{
    public DelivererRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Guid> Add(Deliverer deliverer, CancellationToken cancellationToken)
    {
        await _context.Deliverers.AddAsync(deliverer, cancellationToken);
        return deliverer.Id;
    }

    public async Task<bool> CnhExistsAsync(string cnh, CancellationToken cancellationToken)
    {
        return await _context.Deliverers
            .Where(d => d.CnhNumber == cnh)
            .AsNoTracking()
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> CnhAllowedExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Deliverers
            .Where(d => d.Id == id && (d.CnhType == "A" || d.CnhType == "AB"))
            .AsNoTracking()
            .AnyAsync(cancellationToken);
    }

    public async Task<Deliverer> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Deliverers
            .AsNoTracking()
            .SingleAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> CnpjExistsAsync(string cnpj, CancellationToken cancellationToken)
    {
        return await _context.Deliverers
            .Where(d => d.Cnpj == cnpj)
            .AsNoTracking()
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> UpdateImageCnhAsync(Guid id, string newImagePath)
    {
        var deliverer = await _context.Deliverers
            .FirstOrDefaultAsync(d => d.Id == id);

        if (deliverer is null) throw new NotFoundException("Deliverer not found.");

        deliverer.CnhPathImage = newImagePath;
        _context.Deliverers.Update(deliverer);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<DelivererDto>> GetAll(string? cnh, CancellationToken cancellationToken)
    {
        var query = _context.Deliverers.AsQueryable();
        if (!string.IsNullOrEmpty(cnh)) query = query.Where(v => v.CnhNumber == cnh);

        return await query
            .Select(e => new DelivererDto
            {
                Id = e.Id,
                Name = e.Name,
                Cnpj = e.Cnpj,
                BirthDate = e.BirthDate,
                CnhNumber = e.CnhNumber,
                CnhType = e.CnhType,
                CnhPathImage = e.CnhPathImage
            })
            .ToListAsync(cancellationToken);
    }
}