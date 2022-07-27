using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace WorldServer.Handlers.Foo;

public sealed record GetFooQuery(long Seq) : IQuery;

public class GetFooHandler : IQueryHandler<GetFooQuery, List<string>>
{
    private readonly WorldDbContext dbContext;

    public GetFooHandler(WorldDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<string>> QueryAsync(GetFooQuery query)
    {
        return await dbContext.Foos
            .Where(row => query.Seq < row.Seq)
            .Select(row => row.Data)
            .ToListAsync();
    }
}