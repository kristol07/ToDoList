using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoList.API;
using ToDoList.API.Services;
using Xunit;

namespace ToDoList.IntegrationTests
{
    public class ToDoListTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public ToDoListTest(WebApplicationFactory<Startup> factory)
        {
            //_httpClient = factory.CreateDefaultClient();

            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IRepository, FakeToDoListRepository>();
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetTodoList_Should_ReturnOk()
        {
            var response = await _httpClient.GetAsync("api/ToDoList");

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
