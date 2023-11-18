using Microsoft.VisualBasic;
using WalksAPI.Data;
using WalksAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace WalksAPI.Repositories
{
    public class RegionRepository : IRegion
    {
        private WalksDbContext dbContext;

        public RegionRepository(WalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingregion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingregion == null)
            {
                return null;
            }
            dbContext.Regions.Remove(existingregion);
            await dbContext.SaveChangesAsync();
            return existingregion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();

        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingregion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingregion == null)
            {
                return null;
            }
            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return existingregion;
        }
    }
}
