using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MasFinalProj.API.Abstractions;

/// <summary>
/// Abstrakcyjna klasa kontrolera.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public abstract class AbstractController<TKey, TEntity, TInputDto, TOutputDto> : ControllerBase where TEntity : BaseEntity<TKey>
{
    private readonly IGenericRepository<TKey, TEntity> _repository;
    
    /// <summary>
    /// Konstruktor kontrolera.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="logger"></param>
    protected AbstractController(IGenericRepository<TKey, TEntity> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Pobiera wszystkie encje typu <see cref="TEntity"/>.
    /// </summary>
    /// <returns>
    /// Lista encji typu <see cref="TEntity"/>.
    /// </returns>
    [HttpGet]
    public abstract Task<ActionResult<List<TOutputDto>>> GetAllAsync();

    /// <summary>
    /// Pobiera encję typu <see cref="TEntity"/> o podanym identyfikatorze.
    /// </summary>
    /// <param name="id">
    /// Identyfikator encji.
    /// </param>
    /// <returns>
    /// Encja typu <see cref="TEntity"/>.
    /// </returns>
    [HttpGet("{id}")]
    public abstract Task<ActionResult<TOutputDto>> GetByIdAsync([FromRoute] TKey id);
    
    
    /// <summary>
    /// Tworzy nową encję typu <see cref="TEntity"/>.
    /// </summary>
    /// <param name="entity">
    /// Encja do utworzenia.
    /// </param>
    /// <returns>
    /// Encja typu <see cref="TEntity"/>.
    /// </returns>
    [HttpPost]
    public abstract Task<ActionResult<TOutputDto>> CreateAsync([FromBody] TInputDto entity);
    
    
    /// <summary>
    /// Aktualizuje encję typu <see cref="TEntity"/>.
    /// </summary>
    /// <param name="entity">
    /// Encja do aktualizacji.
    /// </param>
    /// <returns>
    /// Encja typu <see cref="TEntity"/>.
    /// </returns>
    [HttpPut]
    public abstract Task<ActionResult<TOutputDto>> UpdateAsync([FromBody] TEntity entity);
    
    /// <summary>
    /// Usuwa encję typu <see cref="TEntity"/> o podanym identyfikatorze.
    /// </summary>
    /// <param name="id">
    /// Identyfikator encji.
    /// </param>
    /// <returns>
    /// Ilość usuniętych encji.
    /// </returns>
    [HttpDelete("{id}")]
    public abstract Task<ActionResult<int>> DeleteAsync([FromRoute] TKey id);
}