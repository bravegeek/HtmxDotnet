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
        [MaxIterationCount(50)] // Maximum number of iterations
        [BenchmarkCategory("Sani")]
        [Benchmark]
        public string BuilderBenchSanitizer()
        {
            using var hb = new
            HtmlBuilder(Tag.Div)
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
            .Close();

            var res = hb.Build();
            return res;
        }

        [MinIterationCount(30)] // Minimum number of iterations
        [MaxIterationCount(50)] // Maximum number of iterations
        [Benchmark]
        public string BuilderBenchSmallRender()
        {
            using var hb = new HtmlBuilder(Tag.Div);
            hb.Attributes(("article-type", "explination")).Class("article")
                .Open(Tag.P).Attributes(("Auther", "Will")).Class("paragraph", "fancy-text", "document")
                    .Open(Tag.Strong)
                        .Text("Lorem Ipsum")
                    .Close()
                    .Text("Hello")
                    .Open(Tag.Strong).Class("cursive", "drop-cap")
                        .Text("Where does it come from?")
                    .Close()
                    .Text("World")
                .Close()
            .Close();

            return hb.Build();

        }

        [MinIterationCount(10)] // Minimum number of iterations
        [MaxIterationCount(15)] // Maximum number of iterations
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

            using var builder = new HtmlBuilder();

            builder
                .Open(Tag.H3)
                    .Text("MultiTabForm")
                .Close()

                .Open(Tag.Div)
                    .Class("instructions")
                    .Open(Tag.P).Id("Instructions")
                        .Text(
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
                    .Close()
                .Close()

                .Open(Tag.Div).Class("user-input")
                    .Open(Tag.P).Class("user-article")
                        .SanitizeAndAddText("<script>alert('Hello!');</script> <b>bold</b>")//(°ー°〃)
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
                        .Close()
                    .Close()

                    // Address Info tab
                    .Open(Tag.Li)
                        .Class("nav-item")
                        .Open(Tag.A)
                            .Class("nav-link")
                            .Id("address-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "address-info"))
                            .Text("Address Info")
                        .Close()
                    .Close()

                    // Favorites Info tab
                    .Open(Tag.Li)
                        .Class("nav-item")
                        .Open(Tag.A)
                            .Class("nav-link")
                            .Id("favorites-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "favorites-info"))
                            .Text("Favorites Info")
                        .Close()
                    .Close()

                    // Confirmation tab
                    .Open(Tag.Li)
                        .Class("nav-item")
                        .Open(Tag.A)
                            .Class("nav-link")
                            .Id("confirmation-tab")
                            .Attributes(("href", "#"))
                            .DataAttributes(("tab", "confirmation"))
                            .Text("Confirmation")
                        .Close()
                    .Close()
                .Close() // End of UL

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
                            .Close()
                            .Open(Tag.Input)
                                .Class("form-control")
                                .Id("firstName")
                                .Attributes(("type", "text"), ("name", "PersonalInfo.FirstName"), ("value", "John"), ("required", "required"))
                            .Close()
                        .Close()

                        .Open(Tag.Div)
                            .Class("form-group")
                            .Open(Tag.Label)
                                .Attributes(("for", "lastName"))
                                .Text("Last Name")
                            .Close()
                            .Open(Tag.Input)
                                .Class("form-control")
                                .Id("lastName")
                                .Attributes(("type", "text"), ("name", "PersonalInfo.LastName"), ("value", "Doe"), ("required", "required"))
                            .Close()
                        .Close()

                        .Open(Tag.Div)
                            .Class("form-group")
                            .Open(Tag.Label)
                                .Attributes(("for", "email"))
                                .Text("Email")
                            .Close()
                            .Open(Tag.Input)
                                .Class("form-control")
                                .Id("email")
                                .Attributes(("type", "email"), ("name", "PersonalInfo.Email"), ("value", "jdoe@mail.com"), ("required", "required"))
                            .Close()
                        .Close()
                    .Close()

                    // Address Info Tab Content
                    .Open(Tag.Div)
                        .Id("address-info")
                        .Class("tab-pane", "fade")

                        .Open(Tag.Div)
                            .Class("form-group")
                            .Open(Tag.Label)
                                .Attributes(("for", "streetAddress"))
                                .Text("Street")
                            .Close()
                            .Open(Tag.Input)
                                .Class("form-control")
                                .Id("streetAddress")
                                .Attributes(("type", "text"), ("name", "AddressInfo.StreetAddress"), ("value", "123 Apple Court Drive"), ("required", "required"))
                            .Close()
                        .Close()

                        .Open(Tag.Div)
                            .Class("form-group")
                            .Open(Tag.Label)
                                .Attributes(("for", "city"))
                                .Text("City")
                            .Close()
                            .Open(Tag.Input)
                                .Class("form-control")
                                .Id("city")
                                .Attributes(("type", "text"), ("name", "AddressInfo.City"), ("value", "Las Vegas"), ("required", "required"))
                            .Close()
                        .Close()

                        .Open(Tag.Div)
                            .Class("form-group")
                            .Open(Tag.Label)
                                .Attributes(("for", "zipCode"))
                                .Text("Zip Code")
                            .Close()
                            .Open(Tag.Input)
                                .Class("form-control")
                                .Id("zipCode")
                                .Attributes(("type", "text"), ("name", "AddressInfo.ZipCode"), ("value", "77777"), ("required", "required"))
                            .Close()
                        .Close()
                    .Close()

                    // Favorites Info Tab Content
                    .Open(Tag.Div)
                        .Id("favorites-info")
                        .Class("tab-pane", "fade")

                        .Open(Tag.Div)
                            .Class("form-group")
                            .Open(Tag.Label)
                                .Attributes(("for", "favoriteColor"))
                                .Text("Favorite Color")
                            .Close()
                            .Open(Tag.Input)
                                .Class("form-control")
                                .Id("favoriteColor")
                                .Attributes(("type", "text"), ("name", "FavoritesInfo.FavoriteColor"), ("value", "Blue"), ("required", "required"))
                            .Close()
                            .Open(Tag.Div)
                                .Attributes(("style", "margin-top: 10px;"))
                                .Open(Tag.Label)
                                    .Text("Search suggestions")
                                .Close()
                            .RenderBuilderView(typeAheadView, htmxConfigColor)
                            .Close()
                        .Close()

                        .Open(Tag.Div)
                            .Class("form-group")
                            .Open(Tag.Label)
                                .Attributes(("for", "favoriteFood"))
                                .Text("Favorite Food")
                            .Close()
                            .Open(Tag.Input)
                                .Class("form-control")
                                .Id("favoriteFood")
                                .Attributes(("type", "text"), ("name", "FavoritesInfo.FavoriteFood"), ("value", "Quiche"), ("required", "required"))
                            .Close()
                            .Open(Tag.Div)
                                .Attributes(("style", "margin-top: 10px;"))
                                .Open(Tag.Label)
                                    .Text("Search suggestions")
                                .Close()
                                .RenderBuilderView(typeAheadView, htmxConfigFood)
                            .Close()
                        .Close()

                    // Confirmation Tab Content
                    .Open(Tag.Div)
                        .Id("confirmation")
                        .Class("tab-pane", "fade")
                        .Open(Tag.P)
                            .Text("Please review your information before submitting.")
                        .Close()
                    .Close()
                .Close() // End of tab-content div

                .Open(Tag.Div)
                    .Id("formResponse")
                .Close()

                .Open(Tag.Form)
                    .Id("multiTabForm")
                    .Open(Tag.Button)
                        .Class("btn", "btn-primary")
                        .Attributes(("type", "submit"))
                        .Text("Submit")
                    .Close()
                .Close();

            var res = builder.BuildPretty();
            return res;
        }
    }
}

// to run, dotnet run -c Release