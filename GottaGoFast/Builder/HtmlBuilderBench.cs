using BenchmarkDotNet.Attributes;
using HtmxDotnet.BuilderViews;
using HtmxDotnet.Models;
using HtmxDotnet.utils;

namespace GottaGoFast.BuilderBench
{
    [MemoryDiagnoser]
    public class HtmlBuilderBench
    {

        [MinIterationCount(30)] // Minimum number of iterations
        [MaxIterationCount(100)] // Maximum number of iterations
        [Benchmark]
        public string BuilderBenchTest()
        {
            var hb = new
            HtmlBuilder(HtmlTag.Div).AddAttributes(("article-type", "explination")).AddCssClasses("article")
                .Open(HtmlTag.P).AddAttributes(("Auther", "Will")).AddCssClasses("paragraph", "fancy-text", "document")
                    .Open(HtmlTag.Strong)
                        .AddText("Lorem Ipsum")
                    .Close()
                    .AddText("Hello")
                    .Open(HtmlTag.Strong).AddCssClasses("cursive", "drop-cap")
                        .AddText("Where does it come from?")
                    .Close()
                    .AddText("World")
                .Close()
            .Close();

            return hb.Build();
        }

        [MinIterationCount(5)] // Minimum number of iterations
        [MaxIterationCount(10)] // Maximum number of iterations
        [Benchmark]
        public string BuilderBenchLargeRender()
        {
            var htmxConfigFood = new HtmxConfig
            {
                HxAction = "hx-get", // Set to hx-get for typeahead
                HxUrl = "kitchensink/TypeAheadSearch", // The URL endpoint for the search action
                HxTrigger = "keyup changed delay:500ms", // Trigger on keyup with debounce
                HxTarget = "resultsFood", // Target the div where results will be displayed
            };

            var htmxConfigColor = new HtmxConfig
            {
                HxAction = "hx-get", // Set to hx-get for typeahead
                HxUrl = "kitchensink/TypeAheadSearchColor", // The URL endpoint for the search action
                HxTrigger = "keyup changed delay:500ms", // Trigger on keyup with debounce
                HxTarget = "resultsColor", // Target the div where results will be displayed
            };

            var typeAheadView = new TypeAheadBV();

            var builder = new HtmlBuilder();

            builder
                .Open(HtmlTag.H3)
                    .AddText("MultiTabForm")
                .Close(HtmlTag.H3)

                .Open(HtmlTag.Div)
                    .AddCssClasses("instructions")
                    .Open(HtmlTag.P).WithId("Instructions")
                        .AddText(
                            @"
                                Lorem Ipsum is simply dummy text of the printing and typesetting industry. 
                                Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, 
                                when an unknown printer took a galley of type and scrambled it to make a type 
                                specimen book. It has survived not only five centuries, but also the leap 
                                into electronic typesetting, remaining essentially unchanged. It was 
                                popularised in the 1960s with the release of Letraset sheets containing Lorem 
                                Ipsum passages, and more recently with desktop publishing software like Aldus 
                                PageMaker including versions of Lorem Ipsum.
                            "
                            )
                    .Close(HtmlTag.P)
                .Close(HtmlTag.Div)

                .Open(HtmlTag.Div).AddCssClasses("user-input")
                    .Open(HtmlTag.P).AddCssClasses("user-article")
                        .SanitizeAndAddText("<script>alert('Hello!');</script> <b>bold</b>")//(°ー°〃)
                    .Close()
                .Close()

                .Open(HtmlTag.Ul)
                    .AddCssClasses("nav", "nav-tabs")
                    .WithId("myTab")
                    .AddAttributes(("role", "tablist"))

                    // Personal Info tab
                    .Open(HtmlTag.Li)
                        .AddCssClasses("nav-item")
                        .Open(HtmlTag.A)
                            .AddCssClasses("nav-link", "active")
                            .WithId("personal-tab")
                            .AddAttributes(("href", "#"))
                            .AddDataAttributes(("tab", "personal-info"))
                            .AddText("Personal Info")
                        .Close(HtmlTag.A)
                    .Close(HtmlTag.Li)

                    // Address Info tab
                    .Open(HtmlTag.Li)
                        .AddCssClasses("nav-item")
                        .Open(HtmlTag.A)
                            .AddCssClasses("nav-link")
                            .WithId("address-tab")
                            .AddAttributes(("href", "#"))
                            .AddDataAttributes(("tab", "address-info"))
                            .AddText("Address Info")
                        .Close(HtmlTag.A)
                    .Close(HtmlTag.Li)

                    // Favorites Info tab
                    .Open(HtmlTag.Li)
                        .AddCssClasses("nav-item")
                        .Open(HtmlTag.A)
                            .AddCssClasses("nav-link")
                            .WithId("favorites-tab")
                            .AddAttributes(("href", "#"))
                            .AddDataAttributes(("tab", "favorites-info"))
                            .AddText("Favorites Info")
                        .Close(HtmlTag.A)
                    .Close(HtmlTag.Li)

                    // Confirmation tab
                    .Open(HtmlTag.Li)
                        .AddCssClasses("nav-item")
                        .Open(HtmlTag.A)
                            .AddCssClasses("nav-link")
                            .WithId("confirmation-tab")
                            .AddAttributes(("href", "#"))
                            .AddDataAttributes(("tab", "confirmation"))
                            .AddText("Confirmation")
                        .Close(HtmlTag.A)
                    .Close(HtmlTag.Li)
                .Close(HtmlTag.Ul) // End of UL

            // Tab content div
            .Open(HtmlTag.Div)
                .WithId("tabContent")
                .AddCssClasses("tab-content")

                // Personal Info Tab Content
                .Open(HtmlTag.Div)
                    .WithId("personal-info")
                    .AddCssClasses("tab-pane", "fade", "show", "active")

                    .Open(HtmlTag.Div)
                        .AddCssClasses("form-group")
                        .Open(HtmlTag.Label)
                            .AddAttributes(("for", "firstName"))
                            .AddText("First Name")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .AddCssClasses("form-control")
                            .WithId("firstName")
                            .AddAttributes(("type", "text"), ("name", "PersonalInfo.FirstName"), ("value", "John"), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .AddCssClasses("form-group")
                        .Open(HtmlTag.Label)
                            .AddAttributes(("for", "lastName"))
                            .AddText("Last Name")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .AddCssClasses("form-control")
                            .WithId("lastName")
                            .AddAttributes(("type", "text"), ("name", "PersonalInfo.LastName"), ("value", "Doe"), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .AddCssClasses("form-group")
                        .Open(HtmlTag.Label)
                            .AddAttributes(("for", "email"))
                            .AddText("Email")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .AddCssClasses("form-control")
                            .WithId("email")
                            .AddAttributes(("type", "email"), ("name", "PersonalInfo.Email"), ("value", "jdoe@mail.com"), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)
                .Close(HtmlTag.Div)

                // Address Info Tab Content
                .Open(HtmlTag.Div)
                    .WithId("address-info")
                    .AddCssClasses("tab-pane", "fade")

                    .Open(HtmlTag.Div)
                        .AddCssClasses("form-group")
                        .Open(HtmlTag.Label)
                            .AddAttributes(("for", "streetAddress"))
                            .AddText("Street")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .AddCssClasses("form-control")
                            .WithId("streetAddress")
                            .AddAttributes(("type", "text"), ("name", "AddressInfo.StreetAddress"), ("value", "123 Apple Court Drive"), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .AddCssClasses("form-group")
                        .Open(HtmlTag.Label)
                            .AddAttributes(("for", "city"))
                            .AddText("City")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .AddCssClasses("form-control")
                            .WithId("city")
                            .AddAttributes(("type", "text"), ("name", "AddressInfo.City"), ("value", "Las Vegas"), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .AddCssClasses("form-group")
                        .Open(HtmlTag.Label)
                            .AddAttributes(("for", "zipCode"))
                            .AddText("Zip Code")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .AddCssClasses("form-control")
                            .WithId("zipCode")
                            .AddAttributes(("type", "text"), ("name", "AddressInfo.ZipCode"), ("value", "77777"), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)
                .Close(HtmlTag.Div)

                // Favorites Info Tab Content
                .Open(HtmlTag.Div)
                    .WithId("favorites-info")
                    .AddCssClasses("tab-pane", "fade")

                    .Open(HtmlTag.Div)
                        .AddCssClasses("form-group")
                        .Open(HtmlTag.Label)
                            .AddAttributes(("for", "favoriteColor"))
                            .AddText("Favorite Color")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .AddCssClasses("form-control")
                            .WithId("favoriteColor")
                            .AddAttributes(("type", "text"), ("name", "FavoritesInfo.FavoriteColor"), ("value", "Blue"), ("required", "required"))
                        .Close(HtmlTag.Input)
                        .Open(HtmlTag.Div)
                            .AddAttributes(("style", "margin-top: 10px;"))
                            .Open(HtmlTag.Label)
                                .AddText("Search suggestions")
                            .Close(HtmlTag.Label)
                        .RenderBuilderView(typeAheadView, htmxConfigColor)
                        .Close(HtmlTag.Div)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .AddCssClasses("form-group")
                        .Open(HtmlTag.Label)
                            .AddAttributes(("for", "favoriteFood"))
                            .AddText("Favorite Food")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .AddCssClasses("form-control")
                            .WithId("favoriteFood")
                            .AddAttributes(("type", "text"), ("name", "FavoritesInfo.FavoriteFood"), ("value", "Quiche"), ("required", "required"))
                        .Close(HtmlTag.Input)
                        .Open(HtmlTag.Div)
                            .AddAttributes(("style", "margin-top: 10px;"))
                            .Open(HtmlTag.Label)
                                .AddText("Search suggestions")
                            .Close(HtmlTag.Label)
                    .RenderBuilderView(typeAheadView, htmxConfigFood)
                    .Close(HtmlTag.Div)
                .Close(HtmlTag.Div)

                // Confirmation Tab Content
                .Open(HtmlTag.Div)
                    .WithId("confirmation")
                    .AddCssClasses("tab-pane", "fade")
                    .Open(HtmlTag.P)
                        .AddText("Please review your information before submitting.")
                    .Close(HtmlTag.P)
                .Close(HtmlTag.Div)
            .Close(HtmlTag.Div) // End of tab-content div

            .Open(HtmlTag.Div)
                .WithId("formResponse")
            .Close(HtmlTag.Div)

            .Open(HtmlTag.Form)
                .WithId("multiTabForm")
                .Open(HtmlTag.Button)
                    .AddCssClasses("btn", "btn-primary")
                    .AddAttributes(("type", "submit"))
                    .AddText("Submit")
                .Close(HtmlTag.Button)
            .Close(HtmlTag.Form);

            var res = builder.Build();

            return res;
        }
    }
}