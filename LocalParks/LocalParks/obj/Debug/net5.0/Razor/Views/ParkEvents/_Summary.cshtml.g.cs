#pragma checksum "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d2bf59dcfd46c8d9948a407dab1c26c2df9e948d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ParkEvents__Summary), @"mvc.1.0.view", @"/Views/ParkEvents/_Summary.cshtml")]
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
#line 1 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
using LocalParks.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d2bf59dcfd46c8d9948a407dab1c26c2df9e948d", @"/Views/ParkEvents/_Summary.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1358598ebd74b86c97ada2e51e73a0b681c39b20", @"/Views/_ViewImports.cshtml")]
    public class Views_ParkEvents__Summary : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<EventModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"panel panel-default\">\r\n    <div class=\"panel-heading\">\r\n        <h2>");
#nullable restore
#line 6 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
       Write(Model.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n\r\n        <input class=\"btn-dark w3-hover-border-amber w3-hover-text-amber rounded\" \r\n               type=\"button\" \r\n               value=\"See Details\"");
            BeginWriteAttribute("onclick", " \r\n               onclick=\"", 296, "\"", 470, 3);
            WriteAttributeValue("", 323, "location.href=\'", 323, 15, true);
#nullable restore
#line 11 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
WriteAttributeValue("", 338, Url.Action("Details", "ParkEvents", 
                                      new { parkId = Model.Park.ParkId, date = Model.Date }), 338, 131, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 469, "\'", 469, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n    </div>\r\n\r\n    <table class=\"table table-striped table-bordered dt-responsive nowrap w3-center btn rounded\">\r\n            <tr>\r\n                <th class=\"thead-light\">");
#nullable restore
#line 17 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
                                   Write(Html.DisplayNameFor(model => model.Park));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th class=\"thead-light\">");
#nullable restore
#line 18 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
                                   Write(Html.DisplayNameFor(model => model.Date));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n            </tr>\r\n            <tr>\r\n                <td>");
#nullable restore
#line 21 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
               Write(Model.Park.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 22 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
               Write(Model.Date.ToShortDateString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n            <tr>\r\n                <th class=\"thead-light\" colspan=\"2\">");
#nullable restore
#line 25 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
                                               Write(Html.DisplayNameFor(model => model.Description));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n            </tr>\r\n            <tr>\r\n                <td colspan=\"2\">");
#nullable restore
#line 28 "C:\Users\owenf\source\Projects\LocalParks\LocalParks\LocalParks\Views\ParkEvents\_Summary.cshtml"
                           Write(Model.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n    </table>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<EventModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
