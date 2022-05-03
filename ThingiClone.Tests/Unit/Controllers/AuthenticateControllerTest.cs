using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThingiClone.Controllers;
using ThingiClone.Models;
using ThingiClone.Models.Auth;
using Xunit;

namespace ThingiClone.Tests.Unit.Controllers
{
    public class AuthenticateControllerTest
    {

        [Fact]
        public async Task LoginUserFailedTest()
        {
            IUserStore<IdentityUser> mockUserStore = Mock.Of<IUserStore<IdentityUser>>();
            IRoleStore<IdentityRole> mockRoleStore = Mock.Of<IRoleStore<IdentityRole>>();
            Mock<UserManager<IdentityUser>> mockUserManager = new Mock<UserManager<IdentityUser>>(mockUserStore, null, null, null, null, null, null, null, null);
            Mock<RoleManager<IdentityRole>> mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore, null, null, null, null);
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

            var controller = new AuthenticateController(mockUserManager.Object, mockRoleManager.Object, mockConfiguration.Object);
            var loginData = new LoginModel { Username = "testuser", Password = "test"};
            var result = await controller.Login(loginData);

            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task LoginUserSuccessTest()
        {
            IUserStore<IdentityUser> mockUserStore = Mock.Of<IUserStore<IdentityUser>>();
            IRoleStore<IdentityRole> mockRoleStore = Mock.Of<IRoleStore<IdentityRole>>();
            Mock<UserManager<IdentityUser>> mockUserManager = new Mock<UserManager<IdentityUser>>(mockUserStore, null, null, null, null, null, null, null, null);
            Mock<RoleManager<IdentityRole>> mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore, null, null, null, null);
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "JWT:Secret")]).Returns("thisisa512bitsecretwheeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");

            var loginData = new LoginModel { Username = "testuser", Password = "test" };
            IdentityUser testUser = new IdentityUser("testuser");
            mockUserManager.Setup(mock => mock.FindByNameAsync(loginData.Username)).ReturnsAsync(testUser);
            mockUserManager.Setup(mock => mock.CheckPasswordAsync(testUser, loginData.Password)).ReturnsAsync(true);
            mockUserManager.Setup(mock => mock.GetRolesAsync(testUser)).ReturnsAsync(new List<string> { "user" });

            var controller = new AuthenticateController(mockUserManager.Object, mockRoleManager.Object, mockConfiguration.Object);
            
            var result = await controller.Login(loginData) as OkObjectResult;

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var resultObject = (AuthResponse)result.Value;
            Assert.NotNull(resultObject.Token);
            Assert.NotNull(resultObject.Expiration);
        }

        [Fact]
        public async Task RegisterFailedUserAlreadyExistsTest()
        {
            IUserStore<IdentityUser> mockUserStore = Mock.Of<IUserStore<IdentityUser>>();
            IRoleStore<IdentityRole> mockRoleStore = Mock.Of<IRoleStore<IdentityRole>>();
            Mock<UserManager<IdentityUser>> mockUserManager = new Mock<UserManager<IdentityUser>>(mockUserStore, null, null, null, null, null, null, null, null);
            Mock<RoleManager<IdentityRole>> mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore, null, null, null, null);
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

            var registerData = new RegisterModel { EmailAddress = "test@test.com", Password = "testpassword", Username = "testuser" };
            var userData = new IdentityUser { UserName = "testuser" };

            mockUserManager.Setup(mock => mock.FindByNameAsync(registerData.Username)).ReturnsAsync(userData);

            var controller = new AuthenticateController(mockUserManager.Object, mockRoleManager.Object, mockConfiguration.Object);

            var result = await controller.Register(registerData) as ObjectResult;

            Assert.Equal(500, result.StatusCode);

            Response resultObject = (Response)result.Value;
            Assert.Equal("Error", resultObject.Status);
        }

        [Fact]
        public async Task RegisterFailedInvalidModelTest()
        {
            IUserStore<IdentityUser> mockUserStore = Mock.Of<IUserStore<IdentityUser>>();
            IRoleStore<IdentityRole> mockRoleStore = Mock.Of<IRoleStore<IdentityRole>>();
            Mock<UserManager<IdentityUser>> mockUserManager = new Mock<UserManager<IdentityUser>>(mockUserStore, null, null, null, null, null, null, null, null);
            Mock<RoleManager<IdentityRole>> mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore, null, null, null, null);
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

            var registerData = new RegisterModel { EmailAddress = "test@test.com", Password = "testpassword"};

            var controller = new AuthenticateController(mockUserManager.Object, mockRoleManager.Object, mockConfiguration.Object);
            controller.ModelState.AddModelError("username", "required");

            var result = await controller.Register(registerData) as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task RegisterSuccessfulTest()
        {
            IUserStore<IdentityUser> mockUserStore = Mock.Of<IUserStore<IdentityUser>>();
            IRoleStore<IdentityRole> mockRoleStore = Mock.Of<IRoleStore<IdentityRole>>();
            Mock<UserManager<IdentityUser>> mockUserManager = new Mock<UserManager<IdentityUser>>(mockUserStore, null, null, null, null, null, null, null, null);
            Mock<RoleManager<IdentityRole>> mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore, null, null, null, null);
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

            var registerData = new RegisterModel { EmailAddress = "test@test.com", Password = "testpassword", Username = "testuser" };
            mockUserManager.Setup(mock => mock.FindByNameAsync(registerData.Username)).ReturnsAsync(() => null);
            mockUserManager.Setup(mock => mock.CreateAsync(It.IsAny<IdentityUser>(), registerData.Password)).ReturnsAsync(IdentityResult.Success);

            var controller = new AuthenticateController(mockUserManager.Object, mockRoleManager.Object, mockConfiguration.Object);

            var result = await controller.Register(registerData) as OkObjectResult;

            Assert.IsType<OkObjectResult>(result);

            Response resultObject = (Response)result.Value;
            Assert.Equal("Success", resultObject.Status);
        }
    }
}