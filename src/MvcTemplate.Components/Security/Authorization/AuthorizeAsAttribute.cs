﻿using System;

namespace MvcTemplate.Components.Security
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAsAttribute : Attribute
    {
        public String Area { get; set; }
        public String Controller { get; set; }
        public String Action { get; private set; }

        public AuthorizeAsAttribute(String action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            Action = action;
        }
    }
}
