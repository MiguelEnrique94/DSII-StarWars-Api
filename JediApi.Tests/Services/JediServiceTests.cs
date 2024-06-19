using JediApi.Models;
using JediApi.Repositories;
using JediApi.Services;
using Moq;

namespace JediApi.Tests.Services
{
    public class JediServiceTests
    {
        // não mexer
        private readonly JediService _service;
        private readonly Mock<IJediRepository> _repositoryMock;

        public JediServiceTests()
        {
            // não mexer
            _repositoryMock = new Mock<IJediRepository>();
            _service = new JediService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetById_Success()
        { 
            int jediId = 1;
            var expectedJedi = new Jedi { Id = jediId, Name = "Luke Skywalker" };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(jediId))
                           .ReturnsAsync(expectedJedi);

            var result = await _service.GetByIdAsync(jediId);

            Assert.NotNull(result);
            Assert.Equal(jediId, result.Id);
            Assert.Equal("Luke Skywalker", result.Name);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(jediId), Times.Once);
        }


        [Fact]
        public async Task GetById_NotFound()
        {
            int nonExistingJediId = 999;
            _repositoryMock.Setup(repo => repo.GetByIdAsync(nonExistingJediId))
                           .ReturnsAsync(() => null);

            var result = await _service.GetByIdAsync(nonExistingJediId);

            Assert.Null(result);

            _repositoryMock.Verify(repo => repo.GetByIdAsync(nonExistingJediId), Times.Once);
        }

        [Fact]
        public async Task GetAll()
        {
            var expectedJedis = new List<Jedi>
    {
        new Jedi { Id = 1, Name = "Luke Skywalker" },
        new Jedi { Id = 2, Name = "Obi-Wan Kenobi" },
        new Jedi { Id = 3, Name = "Yoda" }
    };
            _repositoryMock.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(expectedJedis);
            
            var results = await _service.GetAllAsync();
            
            Assert.NotNull(results);
            Assert.Equal(expectedJedis.Count, results.Count);

            foreach (var expectedJedi in expectedJedis)
            {
                var result = results.FirstOrDefault(r => r.Id == expectedJedi.Id);
                Assert.NotNull(result);
                Assert.Equal(expectedJedi.Name, result.Name);
            }

            _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

    }
}
