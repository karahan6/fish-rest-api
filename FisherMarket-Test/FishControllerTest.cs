using AutoMapper;
using FisherMarket.Contracts;
using FisherMarket.Controllers;
using FisherMarket.DTOs;
using FisherMarket.Helpers;
using FisherMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace FisherMarket_Test
{

    public class FishControllerTest
    {
        private readonly FishesController _controller;
        private readonly IMapper _mapper;
        private readonly Mock<ILoggerManager> _logger;
        public FishControllerTest()
        {

            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _logger = new Mock<ILoggerManager>();
            _controller = new FishesController(new FakeRepositoryWrapper(), _mapper, _logger.Object);

        }

        [Fact]
        public void GetAll_WhenCalled_ReturnsOkResult()
        {
            var result = _controller.GetFishes();
            var okResult = result as OkObjectResult;

            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public void GetAll_WhenCalled_ReturnsAllItems()
        {
            var okResult = _controller.GetFishes() as OkObjectResult;

            var items = Assert.IsType<List<FishDto>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void GetById_UnknownIdPassed_ReturnsException()
        {
            Exception expectedException = null;
            try
            {
                var notFoundResult = _controller.GetFish(Guid.NewGuid().GetHashCode()) as ObjectResult;
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            Assert.NotNull(expectedException);
            Assert.IsType<Exception>(expectedException);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsOkResult()
        {
            var okResult = _controller.GetFish(222925694);
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetById_Then_Check_Name()
        {
            var item = _controller.GetFish(222925694) as OkObjectResult;
            Assert.Equal("Fish1", ((FishDto)item.Value).Name);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            FishDto newFishDto = new FishDto()
            {
                Name = "Fish4",
                Definition = "Definition4",
                Price = 12.0
            };

            var createdResponse = _controller.Add(newFishDto);

            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            FishDto newFishDto = new FishDto()
            {
                Name = "Fish4",
                Definition = "Definition4",
                Price = 12.0
            };

            var createdResponse = _controller.Add(newFishDto) as CreatedAtActionResult;
            var item = createdResponse.Value as Fish;

            Assert.IsType<Fish>(item);
            Assert.Equal("Fish4", item.Name);
        }

        [Fact]
        public void Delete_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            Exception expectedException = null;
            try
            {
                var notExistingId = Guid.NewGuid().GetHashCode();
                var badResponse = _controller.Delete(notExistingId);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            Assert.NotNull(expectedException);
            Assert.IsType<Exception>(expectedException);
        }

        [Fact]
        public void Delete_ExistingIdPassed_ReturnsOkResult()
        {
            var existingId = 222925694;
            var okResponse = _controller.Delete(existingId);
            Assert.IsType<OkResult>(okResponse);
        }

        [Fact]
        public void UpdateById_ExistingId_ReturnsOkResult()
        {
            var fishDto = new FishDto()
            {
                Id = 222925694,
                Name = "Fish1Edited",
                Definition = "Definition1",
                Price = 101.0
            };
            var response = _controller.Edit(fishDto.Id,  fishDto);
            Assert.IsType<OkResult>(response);

        }


        [Fact]
        public void Update_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            Exception expectedException = null;
            try
            {
                var notExistingId = Guid.NewGuid().GetHashCode();
                var editFish = new FishDto
                {
                    Id = notExistingId,
                    Name = "Fish1Edited",
                    Definition = "Definition1",
                    Price = 101.0

                };
                var badResponse = _controller.Edit(notExistingId, editFish);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            Assert.NotNull(expectedException);
            Assert.IsType<Exception>(expectedException);
        }
    }
}
