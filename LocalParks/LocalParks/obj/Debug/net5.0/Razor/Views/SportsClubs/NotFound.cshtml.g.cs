#pragma checksum "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\SportsClubs\NotFound.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6776e5c4a9cfd7905edab1bf773b0cdd29f1203c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SportsClubs_NotFound), @"mvc.1.0.view", @"/Views/SportsClubs/NotFound.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\_ViewImports.cshtml"
using LocalParks;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\_ViewImports.cshtml"
using LocalParks.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6776e5c4a9cfd7905edab1bf773b0cdd29f1203c", @"/Views/SportsClubs/NotFound.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1358598ebd74b86c97ada2e51e73a0b681c39b20", @"/Views/_ViewImports.cshtml")]
    public class Views_SportsClubs_NotFound : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<title>Error 404 - Page not found</title>\r\n\r\n<h2>Not Found.</h2>\r\n\r\n<input class=\"btn-dark w3-hover-border-amber w3-hover-text-amber rounded btn-sm\" \r\n       type=\"button\" \r\n       value=\"Go back to Sports Clubs\"");
            BeginWriteAttribute("onclick", " \r\n       onclick=\"", 212, "\"", 282, 3);
            WriteAttributeValue("", 231, "location.href=\'", 231, 15, true);
#nullable restore
#line 8 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\SportsClubs\NotFound.cshtml"
WriteAttributeValue("", 246, Url.Action("Index", "SportsClubs"), 246, 35, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 281, "\'", 281, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
