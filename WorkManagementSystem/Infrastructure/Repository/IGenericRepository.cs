﻿namespace WorkManagementSystem.Infrastructure.Repository;

public interface IGenericRepository<T>
{
    /// <returns>The Entity's state</returns>
    EntityState Add(T entity);
    Task AddAsync(T entity);
    /// <returns>The Entity's state</returns>
    EntityState Update(T entity);
    /// <returns>Entity</returns>
    T Get<TKey>(TKey id);
    Task AddRangeAsync(IEnumerable<T> lst);
    /// <returns>Task Entity</returns>
    Task<T> GetAsync<TKey>(TKey id);

    /// <returns>The requested Entity</returns>
    T Get(params object[] keyValues);

    /// <returns>Entity</returns>
    IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

    /// <returns>Queryable</returns>
    IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include);

    /// <returns>List of entities</returns>
    IQueryable<T> GetAll();

    /// <returns>Queryable</returns>
    IQueryable<T> GetAll(int page, int pageCount);

    /// <returns>List of entities</returns>
    IQueryable<T> GetAll(string include);

    /// <returns>List of entities</returns>
    IQueryable<T> RawSql(string sql, params object[] parameters);

    /// <returns>List of entities</returns>
    IQueryable<T> GetAll(string include, string include2);

    /// <summary>
    /// Soft delete with using IsActive flag for entity
    /// </summary>
    /// <returns>The Entity's state</returns>
    EntityState SoftDelete(T entity);

    /// <summary>
    /// Deletes the specified entity
    /// </summary>
    /// <returns>The Entity's state</returns>
    EntityState HardDelete(T entity);
    void HardDeletes(List<T> entity);

    bool Exists(Expression<Func<T, bool>> predicate);


}
