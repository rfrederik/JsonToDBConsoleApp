using JsonToDBConsoleApp.Clients;
using JsonToDBConsoleApp.DTOs;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
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
            var expectedUsers = new List<UserDto>
            {
                new UserDto { Id = "1", Name = "Roddie Frederik", Bio = "cool guy", Language = "English" },
                new UserDto { Id = "2", Name = "Fake Name", Bio = "not so cool", Language = "PHP" }
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

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

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var userClient = new UserClient(httpClient);

            // Act
            var result = await userClient.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsers.Count, result.Count);
            Assert.Equal(expectedUsers[0].Name, result[0].Name);
        }
    }
}
