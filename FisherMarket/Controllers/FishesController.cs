using AutoMapper;
using FisherMarket.Contracts;
using FisherMarket.DTOs;
using FisherMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FisherMarket.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FishesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public FishesController(IRepositoryWrapper repo, IMapper mapper, ILoggerManager logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/fishes
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetFishes()
        {
            _logger.LogInfo("GET api/fishes");

            var fishesFromRepo = _repo.Fish.FindAll();
            var fishes = _mapper.Map<IEnumerable<FishDto>>(fishesFromRepo);

            return Ok(fishes);
        }

        // GET api/fishes/5
        [Authorize(Roles = "Member")]
        [HttpGet("{id}")]
        public IActionResult GetFish(int id)
        {
            _logger.LogInfo("GET api/fishes/{id} where id=" + id);

            var fishFromRepo = _repo.Fish.FindByCondition(f => f.Id == id).FirstOrDefault();
            if(fishFromRepo == null)
            {
                throw new Exception("Fish with this id : " + id + " not found.");
            }
            else
            {
                var fish = _mapper.Map<FishDto>(fishFromRepo);
                return Ok(fish);
            }
        }

        // POST api/fishes/add
        [HttpPost("add")]
        public IActionResult Add([FromBody] FishDto fishDto)
        {
            _logger.LogInfo("GET api/fishes/add");

            var fish = _mapper.Map<Fish>(fishDto);
            _repo.Fish.Create(fish);
            _repo.Save();

            return CreatedAtAction(actionName: "Add",
                routeValues: new { id = fish.Id },
                value: fish);
        }

        // POST api/fishes/edit/5
        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, [FromBody] FishDto fishDto)
        {
            _logger.LogInfo("GET api/fishes/edit/{id} where id=" + id);

            var fish = _repo.Fish.FindByCondition(f => f.Id == id).FirstOrDefault();
            if (fish == null)
            {
                throw new Exception("Fish with this id : " + id + " not found.");
            }
            else
            {
                _mapper.Map(fishDto, fish);
                _repo.Fish.Update(fish);
                _repo.Save();

                return Ok();
            }
        }   

        // DELETE api/fishes/delete/5
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInfo("GET api/fishes/delete/{id} where id=" + id);

            var fish = _repo.Fish.FindByCondition(f => f.Id == id).FirstOrDefault();
            if (fish == null)
            {
                throw new Exception("Fish with this id : " + id + " not found.");
            }
            else
            {
                _repo.Fish.Delete(fish);
                _repo.Save();

                return Ok();
            }
        }
    }
}
