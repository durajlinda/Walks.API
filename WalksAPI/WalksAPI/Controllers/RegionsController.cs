using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.Data;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;

namespace WalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private WalksDbContext dbContext;

        public RegionsController(WalksDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        [HttpGet]
        [Route("api/GetALLRegions")]
        public IActionResult GetAll()
        {
            // Get Data From Database - Domain models
            var regionsDomain = dbContext.Regions.ToList();

            // Map Domain Models to DTOs
            var regionsDto = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            // Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("api/GetRegionById")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            // Get Region Domain Model From Database
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO
            // 
            var regionDto = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // Return DTO back to client
            return Ok(regionDto);
        }

        [HttpPost]
        [Route("api/AddRegion")]
        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionRequest)
        {
            // Map/Convert Region DTO to Region Domain Model
            var regionDomain = new Region
            {
           
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                RegionImageUrl = addRegionRequest.RegionImageUrl
            };
            // Add Region Domain Model to Database
            dbContext.Regions.Add(regionDomain);
            dbContext.SaveChanges();

            var regionDto = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            // Return Region Domain Model back to client
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("api/UpdateRegion/{id}")]
        public IActionResult Update(Guid id, [FromBody] UpdateReionsRequestDTO updateRegionRequest)
        {
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound($"Region with id {id} not found.");
            }

            regionDomain.Code = updateRegionRequest.Code;
            regionDomain.Name = updateRegionRequest.Name;
            regionDomain.RegionImageUrl = updateRegionRequest.RegionImageUrl;

            dbContext.SaveChanges();

            var regionDto = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("api/DeleteRegion/{id}")]
        public IActionResult Delete(Guid id)
        {
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomain == null)
            {
                return NotFound($"Region with id {id} not found.");
            }
            dbContext.Regions.Remove(regionDomain);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
