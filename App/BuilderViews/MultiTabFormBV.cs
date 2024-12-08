using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmxDotnet.Models;
using HtmxDotnet.utils;

namespace HtmxDotnet.BuilderViews
{
    public class MultiTabFormBV : IBuilderView<MultiTabFormViewModel>
    {
        public HtmlBuilder RenderHtml(HtmlBuilder builder, MultiTabFormViewModel model)
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

            builder
                .Open(HtmlTag.H3)
                    .AddText("MultiTabForm")
                .Close(HtmlTag.H3)
                .Open(HtmlTag.Br)
                .Close(HtmlTag.Br)
                .AddText("Just a little spacing")
                .Open(HtmlTag.P)
                    .AddText("Ok, Maybe a little more text")
                    .Open(HtmlTag.Div)
                        .AddText("And some more")
                            .Open(HtmlTag.Span).AddText("Kinda like this!").Close(HtmlTag.Span)
                    .Close(HtmlTag.Div)
                    .AddText("Neat, huh?")
                .Close(HtmlTag.P)
                .Open(HtmlTag.Hr).Close()
                .Open(HtmlTag.Div).AddCssClasses("user-input")
                    .Open(HtmlTag.P).AddCssClasses("user-article")

                        .SanitizeAndAddText(@"
                            <html>
                                <script>
                                    var str = 'Hello, world!'; // Sample string with special characters: < > & ' "" / \n \r
                                    var dangerous = '<script>alert(""xss"")<\/script>';
                                    var escaped = '&lt;div&gt;escaped&lt;/div&gt;';
                                </script>
                                <body>
                                    <h1>Title</h1>
                                    <p>This is a paragraph with <b>bold</b> text and a newline:\n</p>
                                    <p>Special characters: <, >, &, ', ""</p>
                                    <a href=""javascript:alert('XSS');"">Click me</a>
                                </body>
                            </html>
                            <!-- This comment includes &, <, > -->
                            <div>Nested <div>tags</div> are tricky.</div>
                            <table>
                                <tr><td>Row 1, Cell 1</td><td>Row 1, Cell 2</td></tr>
                                <tr><td>Row 2, Cell 1</td><td>Row 2, Cell 2</td></tr>
                            </table>
                            <script>
                                console.log('Characters: <, >, &, \""', ""\"", /, \\');
                            </script>"
                        )//(°ー°〃)
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
                            .AddAttributes(("type", "text"), ("name", "PersonalInfo.FirstName"), ("value", model.PersonalInfo.FirstName ?? ""), ("required", "required"))
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
                            .AddAttributes(("type", "text"), ("name", "PersonalInfo.LastName"), ("value", model.PersonalInfo.LastName ?? ""), ("required", "required"))
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
                            .AddAttributes(("type", "email"), ("name", "PersonalInfo.Email"), ("value", model.PersonalInfo.Email ?? ""), ("required", "required"))
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
                            .AddAttributes(("type", "text"), ("name", "AddressInfo.StreetAddress"), ("value", model.AddressInfo.StreetAddress ?? ""), ("required", "required"))
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
                            .AddAttributes(("type", "text"), ("name", "AddressInfo.City"), ("value", model.AddressInfo.City ?? ""), ("required", "required"))
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
                            .AddAttributes(("type", "text"), ("name", "AddressInfo.ZipCode"), ("value", model.AddressInfo.ZipCode ?? ""), ("required", "required"))
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
                            .AddAttributes(("type", "text"), ("name", "FavoritesInfo.FavoriteColor"), ("value", model.FavoritesInfo.FavoriteColor ?? ""), ("required", "required"))
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
                            .AddAttributes(("type", "text"), ("name", "FavoritesInfo.FavoriteFood"), ("value", model.FavoritesInfo.FavoriteFood ?? ""), ("required", "required"))
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

            return builder;
        }
    }
}