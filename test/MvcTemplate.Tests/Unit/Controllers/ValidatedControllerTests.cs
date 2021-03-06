﻿using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Controllers;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class ValidatedControllerTests : ControllerTests
    {
        private ValidatedController<IValidator, IService> controller;
        private IValidator validator;
        private IService service;

        public ValidatedControllerTests()
        {
            service = Substitute.For<IService>();
            validator = Substitute.For<IValidator>();
            controller = Substitute.ForPartsOf<ValidatedController<IValidator, IService>>(validator, service);
            
            controller.ActionContext.RouteData = new RouteData();
            controller.ActionContext.HttpContext = Substitute.For<HttpContext>();
            controller.HttpContext.ApplicationServices.GetService<IGlobalizationProvider>().Returns(Substitute.For<IGlobalizationProvider>());
        }

        #region Constructor: ValidatedController(TService service, TValidator validator)

        [Fact]
        public void ValidatedController_SetsValidator()
        {
            Object actual = controller.Validator;
            Object expected = validator;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: OnActionExecuting(ActionExecutingContext filterContext)

        [Fact]
        public void OnActionExecuting_SetsServiceCurrentAccountId()
        {
            ReturnCurrentAccountId(controller, "Test");

            controller.OnActionExecuting(null);

            String expected = controller.CurrentAccountId;
            String actual = service.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OnActionExecuting_SetsValidatorCurrentAccountId()
        {
            ReturnCurrentAccountId(controller, "Test");

            controller.OnActionExecuting(null);

            String expected = controller.CurrentAccountId;
            String actual = validator.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OnActionExecuting_SetsValidatorAlerts()
        {
            ReturnCurrentAccountId(controller, "Test");

            controller.OnActionExecuting(null);

            Object expected = controller.Alerts;
            Object actual = validator.Alerts;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void OnActionExecuting_SetsModelState()
        {
            ReturnCurrentAccountId(controller, "Test");

            controller.OnActionExecuting(null);

            Object expected = controller.ModelState;
            Object actual = validator.ModelState;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_Service()
        {
            controller.Dispose();

            service.Received().Dispose();
        }

        [Fact]
        public void Dispose_Validator()
        {
            controller.Dispose();

            validator.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion
    }
}
