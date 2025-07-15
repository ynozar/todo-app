using Microsoft.EntityFrameworkCore;
using ToDoBackend.DataContext;
using ToDoBackend.Repositories.Interfaces;

namespace ToDoBackend.Repositories.Implementations;

public class GroupRepository: IGroupRepository
{
    private readonly ApplicationDataContext _context;

    public GroupRepository(ApplicationDataContext context)
    {
        _context = context;
    }
    
    public async Task<ToDoItem?> GetByUidAsync(Guid uid)
    {
        return await _context.ToDoItems.SingleOrDefaultAsync(i => i.Uid == uid && !i.Deleted);
    }
}