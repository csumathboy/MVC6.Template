﻿using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using MvcTemplate.Resources.Form;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Components.Mvc
{
    public class MinLengthAdapter : MinLengthAttributeAdapter
    {
        public MinLengthAdapter(MinLengthAttribute attribute)
            : base(attribute, null)
        {
            Attribute.ErrorMessage = Validations.FieldMustBeWithMinLengthOf;
        }
    }
}
