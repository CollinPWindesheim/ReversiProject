#pragma checksum "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d78c6c90b0067be7a9bb7a4fded502a596bfd4c8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Score_Index), @"mvc.1.0.view", @"/Views/Score/Index.cshtml")]
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
#line 1 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\_ViewImports.cshtml"
using ReversiMvcApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml"
using ReversiMvcApp.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d78c6c90b0067be7a9bb7a4fded502a596bfd4c8", @"/Views/Score/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"58e39a8f26747ecb82384945e566556928af36c7", @"/Views/_ViewImports.cshtml")]
    public class Views_Score_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ScoreViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("<h1>Score bord</h1>\r\n<table class=\"table table-striped\">\r\n    <thead>\r\n        <tr>\r\n            <th>Naam</th>\r\n            <th>Gewonnen</th>\r\n            <th>Verloren</th>\r\n            <th>Gelijk gepspeeld</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 18 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml"
         foreach (var user in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>");
#nullable restore
#line 21 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml"
           Write(user.Speler);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 22 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml"
           Write(user.AantalGewonnen);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 23 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml"
           Write(user.AantalVerloren);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 24 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml"
           Write(user.AantalGelijk);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n");
#nullable restore
#line 26 "C:\Users\colli\Desktop\test\ReversiDone\Reversi\Views\Score\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ScoreViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
