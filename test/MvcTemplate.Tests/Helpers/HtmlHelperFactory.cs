using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Components.Security;
using NSubstitute;
using System;

namespace MvcTemplate.Tests
{
    //Test For Me
    public static class HtmlHelperFactory
    {
        public static IHtmlHelper CreateHtmlHelper()
        {
            return CreateHtmlHelper<Object>(null);
        }
        public static IHtmlHelper<T> CreateHtmlHelper<T>(T model)
        {
            IHtmlHelper<T> html = Substitute.For<IHtmlHelper<T>>();

            html.ViewContext.Returns(new ViewContext());
            html.ViewContext.RouteData = new RouteData();
            html.ViewContext.HttpContext = Substitute.For<HttpContext>();
            html.MetadataProvider.Returns(new EmptyModelMetadataProvider());
            html.ViewContext.HttpContext.ApplicationServices
                .GetService<IAuthorizationProvider>()
                .Returns(Substitute.For<IAuthorizationProvider>());
            html.ViewContext.ViewData.Model = model;

            return html;
        }
    }
}