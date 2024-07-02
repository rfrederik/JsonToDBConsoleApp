using JsonToDBConsoleApp.Clients;
using JsonToDBConsoleApp.DTOs;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace JsonToDBConsoleApp.Tests
{
    public class UserClientTests
    {
        [Fact]
        public async Task GetUsers_ReturnsUserList_WhenResponseIsSuccessful()
        {
            // Collect
            List<UserDto> expectedUsers = new List<UserDto>
            {
                new UserDto { Id = "1", Name = "Roddie Frederik", Bio = "cool guy", Language = "English" },
                new UserDto { Id = "2", Name = "Fake Name", Bio = "not so cool", Language = "PHP" }
            };

            Mock<HttpMessageHandler> mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedUsers)
                });

            HttpClient httpClient = new HttpClient(mockHttpMessageHandler.Object);

            UserClient userClient = new UserClient(httpClient);

            // Act
            List<UserDto>? result = await userClient.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expectedUsers, result);
        }
    }
}
