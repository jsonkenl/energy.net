using Moq;
using Xunit;
using System;
using Energy.Core;
using Energy.Core.Interfaces;
using Microsoft.Extensions.Options;
using Energy.Net.Features.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.RegularExpressions;
using System.Linq;

namespace Energy.Test.UnitTests.Controllers
{
    public class AdminControllerShould
    {
        private Mock<ITempDataDictionary> _mockTempData;
        private Mock<IOptions<ApplicationOptions>> _mockAppSettings;
        private Mock<IAdministratorRepository> _mockAdminRepo;
        private ApplicationOptions _options;
        private AdminController _sut;

        public AdminControllerShould()
        {
            _mockTempData = new Mock<ITempDataDictionary>();
            _mockAdminRepo = new Mock<IAdministratorRepository>();
            _mockAppSettings = new Mock<IOptions<ApplicationOptions>>();

            _options = new ApplicationOptions
            {
                AdministratorEmail = "AdminEmail@EnergyCo.net",
                AdministratorDistinguishedName = "EnergyCo Administrator"
            };

            _mockAppSettings.Setup(x => x.Value).Returns(_options);

            _sut = new AdminController(_mockAppSettings.Object, _mockAdminRepo.Object);
            _sut.TempData = _mockTempData.Object;
        }

        [Fact]
        public void ReturnViewForCreatePassword()
        {

            IActionResult result = _sut.CreatePassword();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData("ValidPassword1")]
        [InlineData("ValidPassword!")]
        [InlineData("validpassword1!")]
        [InlineData("VALIDPASSWORD1!")]
        public void RedirectToHomeGivenValidPasswordScheme(string password)
        {
            var viewModel = new AdminViewModel
            {
                Password = password,
                ConfirmPassword = password
            };

            if (IsInvalid(password))
            {
                _sut.ModelState.AddModelError("x", "Test Error");
            }

            IActionResult result = _sut.CreatePassword(viewModel);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("/Home/Index", $"/{redirectResult.ControllerName}/{redirectResult.ActionName}");
        }

        [Theory]
        [InlineData("Invalid")]
        [InlineData("InvalidPassword")]
        [InlineData("invalidpassword1")]
        [InlineData("invalidpassword!")]
        [InlineData("INVALIDPASSWORD")]
        [InlineData("1^@|1[]$@22")]
        public void ReturnViewWithModelErrorGivenInvalidPasswordScheme(string password)
        {
            var viewModel = new AdminViewModel
            {
                Password = password,
                ConfirmPassword = password
            };

            if (IsInvalid(password))
            {
                _sut.ModelState.AddModelError("x", "Test Error");
            }

            IActionResult result = _sut.CreatePassword(viewModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ModelState.ErrorCount == 1);
        }

        

        private bool IsInvalid(string password)
        { 
            // Passwords must be between 8 and 50 characters and contain at least 3 of the following: 
            // upper case (A-Z), lower case (a-z), number(0-9) and special character(e.g. !@#$%^&*)
            var option1 = "^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])";
            var option2 = "(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])";
            var option3 = "(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])";
            var option4 = "(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$";
            Regex validationRegex = new Regex($"{option1}|{option2}|{option3}|{option4}");

            return (Enumerable.Range(8,50).Contains(password.Length) 
                && validationRegex.IsMatch(password)) ? false : true;
        }
    }
}
