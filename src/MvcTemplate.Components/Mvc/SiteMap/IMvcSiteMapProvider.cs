﻿using Microsoft.AspNet.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcTemplate.Components.Mvc
{
    public interface IMvcSiteMapProvider
    {
        IEnumerable<MvcSiteMapNode> GetAuthorizedMenus(ViewContext context);
        IEnumerable<MvcSiteMapNode> GetBreadcrumb(ViewContext context);
    }
}
