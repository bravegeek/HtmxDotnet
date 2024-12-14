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
                .Open(Tag.H3)
                    .Text("MultiTabForm")
                .Close(Tag.H3)
                .Open(Tag.Br)
                .Close(Tag.Br)
                .Text("Just a little spacing")
                .Open(Tag.P)
                    .Text("Ok, Maybe a little more text")
                    .Open(Tag.Div)
                        .Text("And some more")
                            .Open(Tag.Span).Text("Kinda like this!").Close(Tag.Span)
                    .Close(Tag.Div)
                    .Text("Neat, huh?")
                .Close(Tag.P)
                .Open(Tag.Hr).Close()
                .Open(Tag.Div).Class("user-input")
                    .Open(Tag.P).Class("user-article")

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

                .Open(Tag.Ul)
                    .Class("nav", "nav-tabs")
                    .Id("myTab")
                    .Attributes(("role", "tablist"))

                    // Personal Info tab
                    .Open(Tag.Li)
                        .Class("nav-item")
                        .Open(Tag.A)
                            .Class("nav-link", "active")
                            .Id("personal-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "personal-info"))
                            .Text("Personal Info")
                        .Close(Tag.A)
                    .Close(Tag.Li)

                    // Address Info tab
                    .Open(Tag.Li)
                        .Class("nav-item")
                        .Open(Tag.A)
                            .Class("nav-link")
                            .Id("address-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "address-info"))
                            .Text("Address Info")
                        .Close(Tag.A)
                    .Close(Tag.Li)

                    // Favorites Info tab
                    .Open(Tag.Li)
                        .Class("nav-item")
                        .Open(Tag.A)
                            .Class("nav-link")
                            .Id("favorites-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "favorites-info"))
                            .Text("Favorites Info")
                        .Close(Tag.A)
                    .Close(Tag.Li)

                    // Confirmation tab
                    .Open(Tag.Li)
                        .Class("nav-item")
                        .Open(Tag.A)
                            .Class("nav-link")
                            .Id("confirmation-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "confirmation"))
                            .Text("Confirmation")
                        .Close(Tag.A)
                    .Close(Tag.Li)
                .Close(Tag.Ul) // End of UL

            // Tab content div
            .Open(Tag.Div)
                .Id("tabContent")
                .Class("tab-content")

                // Personal Info Tab Content
                .Open(Tag.Div)
                    .Id("personal-info")
                    .Class("tab-pane", "fade", "show", "active")

                    .Open(Tag.Div)
                        .Class("form-group")
                        .Open(Tag.Label)
                            .Attributes(("for", "firstName"))
                            .Text("First Name")
                        .Close(Tag.Label)
                        .Open(Tag.Input)
                            .Class("form-control")
                            .Id("firstName")
                            .Attributes(("type", "text"), ("name", "PersonalInfo.FirstName"), ("value", model.PersonalInfo.FirstName ?? ""), ("required", "required"))
                        .Close(Tag.Input)
                    .Close(Tag.Div)

                    .Open(Tag.Div)
                        .Class("form-group")
                        .Open(Tag.Label)
                            .Attributes(("for", "lastName"))
                            .Text("Last Name")
                        .Close(Tag.Label)
                        .Open(Tag.Input)
                            .Class("form-control")
                            .Id("lastName")
                            .Attributes(("type", "text"), ("name", "PersonalInfo.LastName"), ("value", model.PersonalInfo.LastName ?? ""), ("required", "required"))
                        .Close(Tag.Input)
                    .Close(Tag.Div)

                    .Open(Tag.Div)
                        .Class("form-group")
                        .Open(Tag.Label)
                            .Attributes(("for", "email"))
                            .Text("Email")
                        .Close(Tag.Label)
                        .Open(Tag.Input)
                            .Class("form-control")
                            .Id("email")
                            .Attributes(("type", "email"), ("name", "PersonalInfo.Email"), ("value", model.PersonalInfo.Email ?? ""), ("required", "required"))
                        .Close(Tag.Input)
                    .Close(Tag.Div)
                .Close(Tag.Div)

                // Address Info Tab Content
                .Open(Tag.Div)
                    .Id("address-info")
                    .Class("tab-pane", "fade")

                    .Open(Tag.Div)
                        .Class("form-group")
                        .Open(Tag.Label)
                            .Attributes(("for", "streetAddress"))
                            .Text("Street")
                        .Close(Tag.Label)
                        .Open(Tag.Input)
                            .Class("form-control")
                            .Id("streetAddress")
                            .Attributes(("type", "text"), ("name", "AddressInfo.StreetAddress"), ("value", model.AddressInfo.StreetAddress ?? ""), ("required", "required"))
                        .Close(Tag.Input)
                    .Close(Tag.Div)

                    .Open(Tag.Div)
                        .Class("form-group")
                        .Open(Tag.Label)
                            .Attributes(("for", "city"))
                            .Text("City")
                        .Close(Tag.Label)
                        .Open(Tag.Input)
                            .Class("form-control")
                            .Id("city")
                            .Attributes(("type", "text"), ("name", "AddressInfo.City"), ("value", model.AddressInfo.City ?? ""), ("required", "required"))
                        .Close(Tag.Input)
                    .Close(Tag.Div)

                    .Open(Tag.Div)
                        .Class("form-group")
                        .Open(Tag.Label)
                            .Attributes(("for", "zipCode"))
                            .Text("Zip Code")
                        .Close(Tag.Label)
                        .Open(Tag.Input)
                            .Class("form-control")
                            .Id("zipCode")
                            .Attributes(("type", "text"), ("name", "AddressInfo.ZipCode"), ("value", model.AddressInfo.ZipCode ?? ""), ("required", "required"))
                        .Close(Tag.Input)
                    .Close(Tag.Div)
                .Close(Tag.Div)

                // Favorites Info Tab Content
                .Open(Tag.Div)
                    .Id("favorites-info")
                    .Class("tab-pane", "fade")

                    .Open(Tag.Div)
                        .Class("form-group")
                        .Open(Tag.Label)
                            .Attributes(("for", "favoriteColor"))
                            .Text("Favorite Color")
                        .Close(Tag.Label)
                        .Open(Tag.Input)
                            .Class("form-control")
                            .Id("favoriteColor")
                            .Attributes(("type", "text"), ("name", "FavoritesInfo.FavoriteColor"), ("value", model.FavoritesInfo.FavoriteColor ?? ""), ("required", "required"))
                        .Close(Tag.Input)
                        .Open(Tag.Div)
                            .Attributes(("style", "margin-top: 10px;"))
                            .Open(Tag.Label)
                                .Text("Search suggestions")
                            .Close(Tag.Label)
                        .RenderBuilderView(typeAheadView, htmxConfigColor)
                        .Close(Tag.Div)
                    .Close(Tag.Div)

                    .Open(Tag.Div)
                        .Class("form-group")
                        .Open(Tag.Label)
                            .Attributes(("for", "favoriteFood"))
                            .Text("Favorite Food")
                        .Close(Tag.Label)
                        .Open(Tag.Input)
                            .Class("form-control")
                            .Id("favoriteFood")
                            .Attributes(("type", "text"), ("name", "FavoritesInfo.FavoriteFood"), ("value", model.FavoritesInfo.FavoriteFood ?? ""), ("required", "required"))
                        .Close(Tag.Input)
                        .Open(Tag.Div)
                            .Attributes(("style", "margin-top: 10px;"))
                            .Open(Tag.Label)
                                .Text("Search suggestions")
                            .Close(Tag.Label)
                    .RenderBuilderView(typeAheadView, htmxConfigFood)
                    .Close(Tag.Div)
                .Close(Tag.Div)

                // Confirmation Tab Content
                .Open(Tag.Div)
                    .Id("confirmation")
                    .Class("tab-pane", "fade")
                    .Open(Tag.P)
                        .Text("Please review your information before submitting.")
                    .Close(Tag.P)
                .Close(Tag.Div)
            .Close(Tag.Div) // End of tab-content div

            .Open(Tag.Div)
                .Id("formResponse")
            .Close(Tag.Div)

            .Open(Tag.Form)
                .Id("multiTabForm")
                .Open(Tag.Button)
                    .Class("btn", "btn-primary")
                    .Attributes(("type", "submit"))
                    .Text("Submit")
                .Close(Tag.Button)
            .Close(Tag.Form);

            return builder;
        }
    }
}