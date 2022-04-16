
namespace ReactServerAPI.Services
{
    public class MarkService : IService<Mark, int>
    {
        private readonly ReactAPIContext ctx;

        public MarkService(ReactAPIContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<Mark> IService<Mark, int>.CreateAsync(Mark entity)
        {
            try
            {
                var result = await ctx.Marks.AddAsync(entity);
                await ctx.SaveChangesAsync();
                return result.Entity; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        async Task<Mark> IService<Mark, int>.DeleteAsync(int id)
        {
            try
            {
                var deptToFind = await ctx.Marks.FindAsync(id);
                if (deptToFind == null)
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return null;
#pragma warning restore CS8603 // Possible null reference return.
                }
                ctx.Marks.Remove(deptToFind);
                await ctx.SaveChangesAsync();
                return deptToFind;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        async Task<IEnumerable<Mark>> IService<Mark, int>.GetAsync()
        {
            try
            {
                var result = await ctx.Marks.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        async Task<Mark> IService<Mark, int>.GetAsync(int id)
        {
            try
            {
                var result = await ctx.Marks.FindAsync(id);
#pragma warning disable CS8603 // Possible null reference return.
                return result;
#pragma warning restore CS8603 // Possible null reference return.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        Task<Mark> IService<Mark, int>.UpdateAsync(int id, Mark entity)
        {
            throw new NotImplementedException();
        }
    }
}
