using Moq;
using Xunit;
using System;
using Energy.Core;
using Energy.Core.Interfaces;
using Energy.Net.Features.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Energy.Test
{
    public class SessionControllerShould
    {
        private string _validPassword;
        private string _invalidPassword;
        private Mock<IEmployeeRepository> _mockEmployeeRepo;
        private Mock<IAdministratorRepository> _mockAdminRepo;
        private Mock<IOptions<ApplicationOptions>> _mockAppSettings;
        private Mock<ITempDataDictionary> _mockTempData;
        private ApplicationOptions _options;

        public SessionControllerShould()
        {
            _validPassword = "validPassword1!";
            _invalidPassword = "invalidPassword1!";
            _mockTempData = new Mock<ITempDataDictionary>();
            _mockEmployeeRepo = new Mock<IEmployeeRepository>();
            _mockAdminRepo = new Mock<IAdministratorRepository>();
            _mockAppSettings = new Mock<IOptions<ApplicationOptions>>();

            _options = new ApplicationOptions
            {
                AdministratorEmail = "AdminEmail@EnergyCo.net",
                AdministratorDistinguishedName = "EnergyCo Administrator"
            };

            _mockAppSettings.Setup(x => x.Value).Returns(_options);
        }

        [Fact]
        public void ReturnViewForLogin()
        {
            var sut = SetUpTestSessionController(); 

            IActionResult result = sut.Login();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task RedirectToHomeViewWhenValidAdminLogin()
        {
            var sut = SetUpTestSessionController();

            var loginModel = new LoginViewModel()
            {
                Email = _options.AdministratorEmail,
                Password = _validPassword
            };

            IActionResult result = await sut.Login(loginModel);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("/Home/Index", $"/{redirectResult.ControllerName}/{redirectResult.ActionName}");
        }

        [Fact]
        public async Task ReturnLoginViewWithModelErrorWhenInvalidAdminLogin()
        {
            var sut = SetUpTestSessionController();

            var loginModel = new LoginViewModel()
            {
                Email = _options.AdministratorEmail,
                Password = _invalidPassword
            };

            IActionResult result = await sut.Login(loginModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ModelState.ErrorCount == 1);
        }
        
        [Fact]
        public void ReturnViewForForbidden()
        {
            var sut = SetUpTestSessionController();

            IActionResult result = sut.Forbidden();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task RedirectToHomeAfterSuccessfulLogout()
        {
            var sut = SetUpTestSessionController();

            var loginModel = new LoginViewModel()
            {
                Email = _options.AdministratorEmail,
                Password = _validPassword
            };

            await sut.Login(loginModel);

            IActionResult result = await sut.Logout();

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("/Home/Index", $"/{redirectResult.ControllerName}/{redirectResult.ActionName}");
        }

        [Fact]
        public async Task ReturnViewWithErrorWhenUnsuccessfulLogout()
        {
            var sut = new SessionController(_mockAppSettings.Object, _mockEmployeeRepo.Object, _mockAdminRepo.Object);

            IActionResult result = await sut.Logout();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ModelState.ErrorCount == 1);
        }

        private SessionController SetUpTestSessionController()
        {
            var admin = new Administrator
            {
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(_validPassword)
            };

            _mockAdminRepo.Setup(x => x.Get()).Returns(admin);

            var employee = new Employee
            {
                FirstName = "EnergyCo",
                LastName = "Administrator",
                Email = _options.AdministratorEmail,
                DistinguishedName = _options.AdministratorDistinguishedName,
                Role = Role.Administrator
            };

            _mockEmployeeRepo.Setup(x => x.GetByEmail(It.IsAny<string>())).Returns(employee);

            return new SessionController(_mockAppSettings.Object, _mockEmployeeRepo.Object, _mockAdminRepo.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        RequestServices = CreateServiceProviderMock()
                    }
                },

                TempData = _mockTempData.Object
            };
        }

        private IServiceProvider CreateServiceProviderMock()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            authenticationServiceMock
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(),
                                            It.IsAny<string>(),
                                            It.IsAny<ClaimsPrincipal>(),
                                            It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);

            var urlHelperFactory = new Mock<IUrlHelperFactory>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);

            return serviceProviderMock.Object;
        }
    }
}
