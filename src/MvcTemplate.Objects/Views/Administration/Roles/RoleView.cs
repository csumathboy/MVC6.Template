﻿using MvcTemplate.Components.Html;
using NonFactors.Mvc.Lookup;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class RoleView : BaseView
    {
        [Required]
        [LookupColumn]
        [StringLength(128)]
        public String Title { get; set; }

        public JsTree PrivilegesTree { get; set; }

        public RoleView()
        {
            PrivilegesTree = new JsTree();
        }
    }
}
