#pragma checksum "C:\Users\colli\Desktop\test\Reversi\Reversi\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "624f46ccc81b01df1376df541ce69a8847161bd8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
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
#line 1 "C:\Users\colli\Desktop\test\Reversi\Reversi\Views\_ViewImports.cshtml"
using Reversi;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\colli\Desktop\test\Reversi\Reversi\Views\_ViewImports.cshtml"
using Reversi.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"624f46ccc81b01df1376df541ce69a8847161bd8", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8cd8a9c6598423fec204e139ce3212675bca1bcc", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_HelpPartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\colli\Desktop\test\Reversi\Reversi\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            DefineSection("Scripts", async() => {
                WriteLiteral("\n    <script");
                BeginWriteAttribute("src", " src=\"", 72, "\"", 110, 1);
#nullable restore
#line 6 "C:\Users\colli\Desktop\test\Reversi\Reversi\Views\Home\Index.cshtml"
WriteAttributeValue("", 78, Url.Content("~/js/site.min.js"), 78, 32, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral("></script>\n");
            }
            );
            WriteLiteral(@"

<div id=""container"">
    <div id=""loading"">
        <div class=""spinner"">
            <svg width=""120"" height=""120"" viewBox=""0 0 30 30"" xmlns=""http://www.w3.org/2000/svg"">
                <circle fill=""none"" stroke=""#000"" cx=""15"" cy=""15"" r=""14""></circle>
            </svg>
        </div>
    </div>
    <div id=""layout"" class=""fade bg-white"">
        <div id=""lobby"" class=""row d-none"">
            <div class=""col"">
                <div class=""d-flex"">
                    <button id=""add"" class=""btn btn-primary d-flex"" data-toggle=""modal"" data-target=""#modal-add"">
                        <i class=""material-icons mr-2"">add</i>
                        <span class=""m-auto"">Nieuw spel</span>
                    </button>
                    <button class=""btn btn-info d-flex d-md-none ml-2"" data-toggle=""modal"" data-target=""#modal-help"">
                        <i class=""material-icons mr-2"">help</i>
                        <span class=""m-auto"">Help</span>
                    </button>
                </div>
     ");
            WriteLiteral("           <ul id=\"list\" class=\"list-group mt-3\"></ul>\n            </div>\n            <div class=\"col d-none d-md-block\">\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "624f46ccc81b01df1376df541ce69a8847161bd85288", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
            </div>
        </div>
        <div id=""spel"" class=""row d-none"">
            <div class=""col col-md-5 col-lg-4 col-xl-3 mb-3 mb-md-0"">
                <div id=""settings"" class=""card"">
                    <div id=""settings-toggle"" class=""card-header"">
                        <span id=""unread"" class=""badge badge-primary d-none mr-3""></span>
                        <span id=""status""></span>
                        <i class=""material-icons float-right"">expand_more</i>
                    </div>
                    <div id=""settings-expand"">
                        <div id=""settings-waiting"" style=""display:none"">
                            <div class=""card-body"">
                                <div class=""row"">
                                    <div class=""col-auto mb-3"">Wachten op een tegenstander...</div>
                                    <div class=""col text-right"">
                                        <button class=""btn btn-danger"" id=""leave"">Spel verlaten</button>
                       ");
            WriteLiteral(@"             </div>
                                </div>
                            </div>
                        </div>
                        <div id=""settings-playing"" style=""display:none"">
                            <div class=""d-flex flex-column"">
                                <div class=""border-bottom p-4"">
                                    <div id=""turn"">
                                        <div class=""row"">
                                            <div class=""col-auto d-flex"">
                                                <div class=""fiche black m-auto""></div>
                                                <div class=""position-absolute text-white score-black""></div>
                                            </div>
                                            <div class=""col d-flex flex-column"">
                                                <div class=""mt-auto"" id=""name-black""></div>
                                                <div class=""mb-auto text-muted"">Zwart</div>
     ");
            WriteLiteral(@"                                       </div>
                                        </div>
                                        <div class=""mt-3 row"">
                                            <div class=""col-auto d-flex"">
                                                <div class=""fiche white m-auto""></div>
                                                <div class=""position-absolute text-black score-white""></div>
                                            </div>
                                            <div class=""col d-flex flex-column"">
                                                <div class=""mt-auto"" id=""name-white""></div>
                                                <div class=""mb-auto text-muted"">Wit</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class=""mt-4 text-right"">
                                        <button class=""btn btn-danger forfeit");
            WriteLiteral(@""" data-toggle=""modal"" data-target=""#modal-forfeit"">Opgeven</button>
                                    </div>
                                </div>
                                <div id=""chat"" class=""chat""></div>
                                <div class=""card-footer text-muted"">
                                    <div class=""input-group"">
                                        <textarea id=""chat-box"" class=""chat-box form-control"" placeholder=""Typ een bericht..."" max=""512""></textarea>
                                        <div class=""input-group-append d-flex"">
                                            <button type=""submit"" id=""chat-send"" class=""btn chat-send material-icons"">send</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class=""w-100 d-md-none""></div>
   ");
            WriteLiteral(@"         <div class=""col"">
                <div id=""bord"" class=""card m-auto position-relative""></div>
                <div id=""pas"" class=""fade d-none"">
                    <div class=""card m-auto p-4"">
                        <span class=""mb-3"">Er zijn geen zetten mogelijk...</span>
                        <button class=""btn btn-primary"">Pas</button>
                    </div>
                </div>
                <div id=""finish"" class=""fade d-none"">
                    <div class=""card m-auto"">
                        <div class=""card-header"" id=""finish-text""></div>
                        <div class=""d-flex flex-column p-3"">
                            <div class=""p-3"" id=""finish-result"">
                                <div class=""row"">
                                    <div class=""col"">
                                        <div class=""text-center"">Zwart</div>
                                        <div class=""display-1 text-center score-black""></div>
                                    </div>
  ");
            WriteLiteral(@"                                  <div class=""col d-flex"">
                                        <div class=""display-1 m-auto text-center"">-</div>
                                    </div>
                                    <div class=""col"">
                                        <div class=""text-center"">Wit</div>
                                        <div class=""display-1 text-center score-white""></div>
                                    </div>
                                </div>
                            </div>
                            <button class=""btn btn-primary ml-auto mt-3"">Terug naar de lobby</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<div class=""modal fade"" id=""modal-add"" tabindex=""-1"" role=""dialog"" aria-labelledby=""modal-add-label"" aria-hidden=""true"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
            ");
            WriteLiteral(@"    <h5 class=""modal-title"" id=""modal-add-label"">Nieuw spel aanmaken</h5>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                    <span aria-hidden=""true"">&times;</span>
                </button>
            </div>
            <div class=""modal-body"">
                <div class=""form-group"">
                    <label for=""modal-add-description"">Omschrijving</label>
                    <textarea class=""form-control"" id=""modal-add-description"" rows=""3""></textarea>
                </div>
                <div class=""form-group"">
                    <label for=""modal-add-size"">Grootte van het bord</label>
                    <input type=""number"" min=""6"" max=""24"" step=""2"" value=""8"" class=""form-control"" id=""modal-add-size"" placeholder=""8"">
                    <span class=""form-validator text-muted"">Kies een even getal tussen 6 en 24</span>
                </div>
            </div>
            <div class=""modal-footer"">
                <button type=""button"" cl");
            WriteLiteral(@"ass=""btn btn-secondary"" data-dismiss=""modal"">Annuleren</button>
                <button type=""submit"" class=""btn btn-primary"" id=""modal-add-submit"">Aanmaken!</button>
            </div>
        </div>
    </div>
</div>

<div class=""modal fade"" id=""modal-help"" tabindex=""-1"" role=""dialog"" aria-labelledby=""modal-help-label"" aria-hidden=""true"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h5 class=""modal-title"" id=""modal-help-label"">Help</h5>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                    <span aria-hidden=""true"">&times;</span>
                </button>
            </div>
            <div class=""modal-body"">
                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "624f46ccc81b01df1376df541ce69a8847161bd814867", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
            </div>
        </div>
    </div>
</div>

<div class=""modal fade"" id=""modal-forfeit"" tabindex=""-1"" role=""dialog"" aria-hidden=""true"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-body"">
                Weet je zeker dat je op wilt geven?
            </div>
            <div class=""modal-footer"">
                <button type=""button"" class=""btn btn-outline-dark"" data-dismiss=""modal"">Nee</button>
                <button type=""button"" class=""btn btn-danger forfeit"" data-dismiss=""modal"" id=""forfeit"">Ja</button>
            </div>
        </div>
    </div>
</div>");
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
