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
                    .Text("MultiTabForm")
                .Close(HtmlTag.H3)
                .Open(HtmlTag.Br)
                .Close(HtmlTag.Br)
                .Text("Just a little spacing")
                .Open(HtmlTag.P)
                    .Text("Ok, Maybe a little more text")
                    .Open(HtmlTag.Div)
                        .Text("And some more")
                            .Open(HtmlTag.Span).Text("Kinda like this!").Close(HtmlTag.Span)
                    .Close(HtmlTag.Div)
                    .Text("Neat, huh?")
                .Close(HtmlTag.P)
                .Open(HtmlTag.Hr).Close()
                .Open(HtmlTag.Div).Classes("user-input")
                    .Open(HtmlTag.P).Classes("user-article")

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
                    .Classes("nav", "nav-tabs")
                    .Id("myTab")
                    .Attributes(("role", "tablist"))

                    // Personal Info tab
                    .Open(HtmlTag.Li)
                        .Classes("nav-item")
                        .Open(HtmlTag.A)
                            .Classes("nav-link", "active")
                            .Id("personal-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "personal-info"))
                            .Text("Personal Info")
                        .Close(HtmlTag.A)
                    .Close(HtmlTag.Li)

                    // Address Info tab
                    .Open(HtmlTag.Li)
                        .Classes("nav-item")
                        .Open(HtmlTag.A)
                            .Classes("nav-link")
                            .Id("address-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "address-info"))
                            .Text("Address Info")
                        .Close(HtmlTag.A)
                    .Close(HtmlTag.Li)

                    // Favorites Info tab
                    .Open(HtmlTag.Li)
                        .Classes("nav-item")
                        .Open(HtmlTag.A)
                            .Classes("nav-link")
                            .Id("favorites-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "favorites-info"))
                            .Text("Favorites Info")
                        .Close(HtmlTag.A)
                    .Close(HtmlTag.Li)

                    // Confirmation tab
                    .Open(HtmlTag.Li)
                        .Classes("nav-item")
                        .Open(HtmlTag.A)
                            .Classes("nav-link")
                            .Id("confirmation-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "confirmation"))
                            .Text("Confirmation")
                        .Close(HtmlTag.A)
                    .Close(HtmlTag.Li)
                .Close(HtmlTag.Ul) // End of UL

            // Tab content div
            .Open(HtmlTag.Div)
                .Id("tabContent")
                .Classes("tab-content")

                // Personal Info Tab Content
                .Open(HtmlTag.Div)
                    .Id("personal-info")
                    .Classes("tab-pane", "fade", "show", "active")

                    .Open(HtmlTag.Div)
                        .Classes("form-group")
                        .Open(HtmlTag.Label)
                            .Attributes(("for", "firstName"))
                            .Text("First Name")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .Classes("form-control")
                            .Id("firstName")
                            .Attributes(("type", "text"), ("name", "PersonalInfo.FirstName"), ("value", model.PersonalInfo.FirstName ?? ""), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .Classes("form-group")
                        .Open(HtmlTag.Label)
                            .Attributes(("for", "lastName"))
                            .Text("Last Name")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .Classes("form-control")
                            .Id("lastName")
                            .Attributes(("type", "text"), ("name", "PersonalInfo.LastName"), ("value", model.PersonalInfo.LastName ?? ""), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .Classes("form-group")
                        .Open(HtmlTag.Label)
                            .Attributes(("for", "email"))
                            .Text("Email")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .Classes("form-control")
                            .Id("email")
                            .Attributes(("type", "email"), ("name", "PersonalInfo.Email"), ("value", model.PersonalInfo.Email ?? ""), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)
                .Close(HtmlTag.Div)

                // Address Info Tab Content
                .Open(HtmlTag.Div)
                    .Id("address-info")
                    .Classes("tab-pane", "fade")

                    .Open(HtmlTag.Div)
                        .Classes("form-group")
                        .Open(HtmlTag.Label)
                            .Attributes(("for", "streetAddress"))
                            .Text("Street")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .Classes("form-control")
                            .Id("streetAddress")
                            .Attributes(("type", "text"), ("name", "AddressInfo.StreetAddress"), ("value", model.AddressInfo.StreetAddress ?? ""), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .Classes("form-group")
                        .Open(HtmlTag.Label)
                            .Attributes(("for", "city"))
                            .Text("City")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .Classes("form-control")
                            .Id("city")
                            .Attributes(("type", "text"), ("name", "AddressInfo.City"), ("value", model.AddressInfo.City ?? ""), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .Classes("form-group")
                        .Open(HtmlTag.Label)
                            .Attributes(("for", "zipCode"))
                            .Text("Zip Code")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .Classes("form-control")
                            .Id("zipCode")
                            .Attributes(("type", "text"), ("name", "AddressInfo.ZipCode"), ("value", model.AddressInfo.ZipCode ?? ""), ("required", "required"))
                        .Close(HtmlTag.Input)
                    .Close(HtmlTag.Div)
                .Close(HtmlTag.Div)

                // Favorites Info Tab Content
                .Open(HtmlTag.Div)
                    .Id("favorites-info")
                    .Classes("tab-pane", "fade")

                    .Open(HtmlTag.Div)
                        .Classes("form-group")
                        .Open(HtmlTag.Label)
                            .Attributes(("for", "favoriteColor"))
                            .Text("Favorite Color")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .Classes("form-control")
                            .Id("favoriteColor")
                            .Attributes(("type", "text"), ("name", "FavoritesInfo.FavoriteColor"), ("value", model.FavoritesInfo.FavoriteColor ?? ""), ("required", "required"))
                        .Close(HtmlTag.Input)
                        .Open(HtmlTag.Div)
                            .Attributes(("style", "margin-top: 10px;"))
                            .Open(HtmlTag.Label)
                                .Text("Search suggestions")
                            .Close(HtmlTag.Label)
                        .RenderBuilderView(typeAheadView, htmxConfigColor)
                        .Close(HtmlTag.Div)
                    .Close(HtmlTag.Div)

                    .Open(HtmlTag.Div)
                        .Classes("form-group")
                        .Open(HtmlTag.Label)
                            .Attributes(("for", "favoriteFood"))
                            .Text("Favorite Food")
                        .Close(HtmlTag.Label)
                        .Open(HtmlTag.Input)
                            .Classes("form-control")
                            .Id("favoriteFood")
                            .Attributes(("type", "text"), ("name", "FavoritesInfo.FavoriteFood"), ("value", model.FavoritesInfo.FavoriteFood ?? ""), ("required", "required"))
                        .Close(HtmlTag.Input)
                        .Open(HtmlTag.Div)
                            .Attributes(("style", "margin-top: 10px;"))
                            .Open(HtmlTag.Label)
                                .Text("Search suggestions")
                            .Close(HtmlTag.Label)
                    .RenderBuilderView(typeAheadView, htmxConfigFood)
                    .Close(HtmlTag.Div)
                .Close(HtmlTag.Div)

                // Confirmation Tab Content
                .Open(HtmlTag.Div)
                    .Id("confirmation")
                    .Classes("tab-pane", "fade")
                    .Open(HtmlTag.P)
                        .Text("Please review your information before submitting.")
                    .Close(HtmlTag.P)
                .Close(HtmlTag.Div)
            .Close(HtmlTag.Div) // End of tab-content div

            .Open(HtmlTag.Div)
                .Id("formResponse")
            .Close(HtmlTag.Div)

            .Open(HtmlTag.Form)
                .Id("multiTabForm")
                .Open(HtmlTag.Button)
                    .Classes("btn", "btn-primary")
                    .Attributes(("type", "submit"))
                    .Text("Submit")
                .Close(HtmlTag.Button)
            .Close(HtmlTag.Form);

            return builder;
        }
    }
}