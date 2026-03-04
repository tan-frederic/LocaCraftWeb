using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Xunit;

namespace LocaCraftAPI.Tests.Services;

public class LoginUserTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly IConfiguration _config;

    public LoginUserTests()
    {
        var store = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            store.Object, null, null, null, null, null, null, null, null);

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "TEST_SECRET_KEY_FOR_UNIT_TESTS_MIN_32_CHARS!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:ExpirationInMinutes"] = "60"
            })
            .Build();
    }

    [Fact]
    public async Task Login_Success_ReturnsOkWithValidToken()
    {
        var user = new AppUser { Id = "user-id", Email = "user@test.com", UserName = "user@test.com" };
        _userManagerMock.Setup(m => m.FindByEmailAsync("user@test.com")).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, "Password1!")).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

        var result = await LoginUser.Handle(
            new LoginUser.Request("user@test.com", "Password1!"),
            _userManagerMock.Object, _config);

        var statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, statusResult.StatusCode);

        var valueResult = Assert.IsAssignableFrom<IValueHttpResult>(result);
        var json = JsonSerializer.Serialize(valueResult.Value);
        var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("token", out var tokenProp));
        var tokenString = tokenProp.GetString();
        Assert.NotNull(tokenString);

        var handler = new JwtSecurityTokenHandler();
        handler.ValidateToken(tokenString, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "TestIssuer",
            ValidAudience = "TestAudience",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("TEST_SECRET_KEY_FOR_UNIT_TESTS_MIN_32_CHARS!"))
        }, out var validatedToken);

        var jwt = Assert.IsType<JwtSecurityToken>(validatedToken);
        Assert.Equal("user-id", jwt.Subject);
        Assert.Contains(jwt.Claims, c => c.Value == "User");
    }

    [Fact]
    public async Task Login_UserNotFound_ReturnsUnauthorized()
    {
        _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((AppUser?)null);

        var result = await LoginUser.Handle(
            new LoginUser.Request("unknown@test.com", "Password1!"),
            _userManagerMock.Object, _config);

        var statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, statusResult.StatusCode);
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsUnauthorized()
    {
        var user = new AppUser { Id = "user-id", Email = "user@test.com", UserName = "user@test.com" };
        _userManagerMock.Setup(m => m.FindByEmailAsync("user@test.com")).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(false);

        var result = await LoginUser.Handle(
            new LoginUser.Request("user@test.com", "WrongPassword!"),
            _userManagerMock.Object, _config);

        var statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, statusResult.StatusCode);
    }
}
