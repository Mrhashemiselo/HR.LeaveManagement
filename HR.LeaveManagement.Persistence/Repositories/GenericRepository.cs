using HR.LeaveManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories;

public class GenericRepository<T>(LeaveManagementDbContext dbContext) : IGenericRepository<T>
    where T : class
{
    public async Task<T> Get(int id) =>
        await dbContext.Set<T>().FindAsync(id);

    public async Task<IReadOnlyList<T>> GetAll() =>
        await dbContext.Set<T>().ToListAsync();

    public async Task<T> Add(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task Update(T entity) =>
        dbContext.Entry(entity).State = EntityState.Modified;

    public async Task Delete(T entity) =>
        dbContext.Set<T>().Remove(entity);

    public async Task<bool> Exists(int id)
    {
        var entity = await Get(id);
        return entity != null;
    }
}