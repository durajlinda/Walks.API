using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.CustomActionFilters;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegion region;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegion region, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.region = region;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Route("api/GetALLRegions")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
           var regionsDomain = await region.GetAllAsync();
           return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
        }


        [HttpGet]
        [Route("api/GetRegionById/{id}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await region.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpPost]
        [ValidateModel]
        [Route("api/AddRegion")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequest)
        {  
            var regionDomain = new Region
            {

                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                RegionImageUrl = addRegionRequest.RegionImageUrl
            };
            regionDomain = await region.CreateAsync(regionDomain);
            var regionDto = mapper.Map<RegionDTO>(regionDomain);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [ValidateModel]
        [Route("api/UpdateRegion/{id}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReionsRequestDTO updateRegionRequest)
        {

            var regionDomain = new Region
            {
            
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                RegionImageUrl = updateRegionRequest.RegionImageUrl
            };
           regionDomain = await region.UpdateAsync(id, regionDomain);

            if (regionDomain == null)
            {
                return NotFound($"Region with id {id} not found.");
            }



            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpDelete]
        [Route("api/DeleteRegion/{id}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var regionDomain = await region.DeleteAsync(id);
            if (regionDomain == null)
            {
                return NotFound($"Region with id {id} not found.");
            }


            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }
    }
}
