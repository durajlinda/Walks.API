﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.CustomActionFilters;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegion region;
        private readonly IMapper mapper;

        public RegionsController(IRegion region, IMapper mapper)
        {
            this.region = region;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("api/GetALLRegions")]
        public async Task<IActionResult> GetAll()
        {
           var regionsDomain = await region.GetAllAsync();
           return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
        }


        [HttpGet]
        [Route("api/GetRegionById/{id}")]
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
