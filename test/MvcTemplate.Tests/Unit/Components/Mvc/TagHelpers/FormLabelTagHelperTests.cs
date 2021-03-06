﻿using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class FormLabelTagHelperTests
    {
        private EmptyModelMetadataProvider provider;
        private FormLabelTagHelper helper;
        private TagHelperOutput output;

        public FormLabelTagHelperTests()
        {
            output = new TagHelperOutput("label", new TagHelperAttributeList(), _ => null);
            provider = new EmptyModelMetadataProvider();
            helper = new FormLabelTagHelper();
        }

        #region Method: Process(TagHelperContext context, TagHelperOutput output)

        [Fact]
        public void Process_Required()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(String), "Relation.Required", typeof(TagHelperModel)));
            helper.For = new ModelExpression("Relation.Required", new ModelExplorer(provider, metadata, null));
            metadata.ValidatorMetadata.Returns(new[] { new RequiredAttribute() });
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");
            output.Attributes["for"] = "Test";

            helper.Process(null, output);

            TagHelperOutput actual = output;

            Assert.Equal("Title<span class=\"require\">*</span>", actual.Content.GetContent());
            Assert.Equal("Relation_Required", actual.Attributes["for"].Value);
        }

        [Fact]
        public void Process_ValueType()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(Int64), "RequiredValue", typeof(TagHelperModel)));
            helper.For = new ModelExpression("RequiredValue", new ModelExplorer(provider, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");
            output.Attributes["for"] = "Test";

            helper.Process(null, output);

            TagHelperOutput actual = output;

            Assert.Equal("Title<span class=\"require\">*</span>", actual.Content.GetContent());
            Assert.Equal("RequiredValue", actual.Attributes["for"].Value);
        }

        [Fact]
        public void Process_NotRequired()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(String), "NotRequired", typeof(TagHelperModel)));
            helper.For = new ModelExpression("NotRequired", new ModelExplorer(provider, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");
            output.Attributes["for"] = "Test";

            helper.Process(null, output);

            TagHelperOutput actual = output;

            Assert.Equal("Title<span class=\"require\"></span>", actual.Content.GetContent());
            Assert.Equal("NotRequired", actual.Attributes["for"].Value);
        }

        [Fact]
        public void Process_NullableValueType()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(Int64?), "NotRequiredNullableValue", typeof(TagHelperModel)));
            helper.For = new ModelExpression("NotRequiredNullableValue", new ModelExplorer(provider, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");
            output.Attributes["for"] = "Test";

            helper.Process(null, output);

            TagHelperOutput actual = output;

            Assert.Equal("Title<span class=\"require\"></span>", actual.Content.GetContent());
            Assert.Equal("NotRequiredNullableValue", actual.Attributes["for"].Value);
        }

        [Fact]
        public void Process_OverwrittenRequired()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(String), "NotRequired", typeof(TagHelperModel)));
            helper.For = new ModelExpression("NotRequired", new ModelExplorer(provider, metadata, null));
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");
            output.Attributes["for"] = "Test";
            helper.Required = true;

            helper.Process(null, output);

            TagHelperOutput actual = output;

            Assert.Equal("Title<span class=\"require\">*</span>", actual.Content.GetContent());
            Assert.Equal("NotRequired", actual.Attributes["for"].Value);
        }

        [Fact]
        public void Process_OverwrittenNotRequired()
        {
            ModelMetadata metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForProperty(typeof(String), "Required", typeof(TagHelperModel)));
            helper.For = new ModelExpression("Required", new ModelExplorer(provider, metadata, null));
            metadata.ValidatorMetadata.Returns(new[] { new RequiredAttribute() });
            helper.For.ModelExplorer.Metadata.DisplayName.Returns("Title");
            output.Attributes["for"] = "Test";
            helper.Required = false;

            helper.Process(null, output);

            TagHelperOutput actual = output;

            Assert.Equal("Title<span class=\"require\"></span>", actual.Content.GetContent());
            Assert.Equal("Required", actual.Attributes["for"].Value);
        }

        #endregion
    }
}
