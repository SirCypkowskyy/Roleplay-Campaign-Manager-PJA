using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasFinalProj.Persistence.Repositories;

/// <summary>
/// Generyczne repozytorium
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public class GenericRepository<TKey, TEntity> : IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GenericRepository<TKey, TEntity>> _logger;
    
    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public GenericRepository(DatabaseContext context, ILogger<GenericRepository<TKey, TEntity>> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdAsync(TKey id, bool asNoTracking = false)
    {
        if (asNoTracking)
            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id != null && e.Id.Equals(id));

        return await _context.Set<TEntity>().FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteByIdAsync(TKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("Entity with id {id} not found", id);
            throw new InvalidOperationException($"Entity with id {id} not found");
        }

        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}