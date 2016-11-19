﻿namespace AngleSharp.Css.Tests.Values
{
    using AngleSharp.Css.Dom;
    using AngleSharp.Css.Parser;
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using NUnit.Framework;
    using static CssConstructionFunctions;

    [TestFixture]
    public class ErrorHandlingTests
    {
        [Test]
        public void ParseInlineStyleWithToleratedInvalidValueShouldReturnThatValue()
        {
            var source = "<div style=\"background-image: url(javascript:alert(1))\"></div>";
            var document = ParseDocument(source, new CssParserOptions
            {
                IsIncludingUnknownDeclarations = true,
                IsIncludingUnknownRules = true,
                IsToleratingInvalidValues = true
            });
            var div = document.QuerySelector<IHtmlElement>("div");
            Assert.AreEqual(1, div.Style.Length);
            Assert.AreEqual("background-image", div.Style[0]);
            Assert.AreEqual("url(javascript:alert(1))", div.Style.GetBackgroundImage());
        }

        [Test]
        public void ParseInlineStyleWithUnknownDeclarationShouldBeAbleToRemoveThatDeclaration()
        {
            var source = @"<DIV STYLE='background: url(""javascript:alert(foo)"")'>";
            var document = ParseDocument(source, new CssParserOptions
            {
                IsIncludingUnknownDeclarations = true,
                IsIncludingUnknownRules = true,
                IsToleratingInvalidValues = true
            });
            var div = document.QuerySelector<IHtmlElement>("div");
            Assert.AreEqual(1, div.Style.Length);
            Assert.AreEqual("background", div.Style[0]);
            div.Style.RemoveProperty("background");
            Assert.AreEqual(0, div.Style.Length);
        }
    }
}
