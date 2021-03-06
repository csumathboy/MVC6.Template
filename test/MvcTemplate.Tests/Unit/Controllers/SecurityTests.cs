﻿using Microsoft.AspNet.Mvc;
using MvcTemplate.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class SecurityTests
    {
        #region ValidateAntiForgeryToken

        [Fact]
        public void PostMethods_HasValidateAntiForgeryToken()
        {
            IEnumerable<MethodInfo> postMethods = typeof(BaseController)
                .Assembly
                .GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsDefined(typeof(HttpPostAttribute), false));

            foreach (MethodInfo method in postMethods)
                Assert.True(method.IsDefined(typeof(ValidateAntiForgeryTokenAttribute), false),
                    String.Format("{0}.{1} method does not have ValidateAntiForgeryToken attribute specified.",
                        method.ReflectedType.Name,
                        method.Name));
        }

        #endregion
    }
}
