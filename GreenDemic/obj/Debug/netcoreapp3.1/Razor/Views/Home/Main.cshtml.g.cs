#pragma checksum "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5139a4284ed9bbd4ec09eeb37d32b7918c6cca56"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Main), @"mvc.1.0.view", @"/Views/Home/Main.cshtml")]
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
#line 1 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\_ViewImports.cshtml"
using GreenDemic;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\_ViewImports.cshtml"
using GreenDemic.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5139a4284ed9bbd4ec09eeb37d32b7918c6cca56", @"/Views/Home/Main.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"651a7dca8d4e0d7b2afc66133ee976b2ab8d58e7", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Main : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
  
    ViewData["Title"] = "Main";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
  
    string name = "Unknown";

    if (Context.Session.GetString("AccName") != null)
    {
        name = Context.Session.GetString("AccName");
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h5 style=\"text-align: center; font-weight: bold;\">Hello, ");
#nullable restore
#line 14 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                     Write(name);

#line default
#line hidden
#nullable disable
            WriteLiteral(" :)</h5>\r\n\r\n");
#nullable restore
#line 16 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
 if (ViewData["ThisMonthCals"] == null)
{
    ViewData["ThisMonthCals"] = 0;
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 21 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
  
    int calPercentage = 0;
    if (Context.Session.GetInt32("TotalCal") == 0)
    {
        calPercentage = 0;
    }
    else
    {
        calPercentage = ((int)@ViewData["ThisMonthCals"] * 100 / (int)@Context.Session.GetInt32("TotalCal"));
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
  List<int> thisYearCalList = (List<int>)ViewData["ThisYearCals"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<p>Total calories of all family members: ");
#nullable restore
#line 34 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                    Write(Context.Session.GetInt32("TotalCal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(" calories</p>\r\n<p>Total calories purchased this month: ");
#nullable restore
#line 35 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                   Write(ViewData["ThisMonthCals"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" calories</p>\r\n\r\n\r\n\r\n");
#nullable restore
#line 39 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
 if ((int)ViewData["ThisMonthCals"] > Context.Session.GetInt32("TotalCal"))
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>😲</h1>\r\n    <div class=\"alert alert-danger\">\r\n        <strong>Uh Oh!</strong> You have exceeded the recommended maximum calories this month.\r\n    </div>\r\n");
#nullable restore
#line 45 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>");
#nullable restore
#line 48 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
   Write(calPercentage);

#line default
#line hidden
#nullable disable
            WriteLiteral(" %</h1>\r\n");
#nullable restore
#line 49 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"progress\">\r\n    <div class=\"progress-bar\" role=\"progressbar\"");
            BeginWriteAttribute("style", " style=\"", 1244, "\"", 1252, 0);
            EndWriteAttribute();
            WriteLiteral(" aria-valuenow=\"0\" aria-valuemin=\"0\" aria-valuemax=\"100\"></div>\r\n</div>\r\n<div>\r\n    <canvas id=\"myChart\" width=\"800\" height=\"450\"></canvas>\r\n</div>\r\n<div>\r\n    <canvas id=\"line-chart\" width=\"800\" height=\"450\"></canvas>\r\n</div>\r\n\r\n\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    <script>\r\n        $(document).ready(function () {\r\n            var newprogress = ");
#nullable restore
#line 67 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                         Write(calPercentage);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            var snack = ");
#nullable restore
#line 68 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                   Write(ViewData["Snack"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            var meat = ");
#nullable restore
#line 69 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                  Write(ViewData["Meat"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            var seafood = ");
#nullable restore
#line 70 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                     Write(ViewData["Seafood"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            var dairy = ");
#nullable restore
#line 71 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                   Write(ViewData["Dairy"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            var grains = ");
#nullable restore
#line 72 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                    Write(ViewData["Grains"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            var fruit = ");
#nullable restore
#line 73 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                   Write(ViewData["Fruit"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            var vegetable = ");
#nullable restore
#line 74 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                       Write(ViewData["Vegetable"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            var others = ");
#nullable restore
#line 75 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                    Write(ViewData["Others"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n\r\n            $(\'.progress-bar\').attr(\'aria-valuenow\', newprogress).css(\'width\', newprogress + \'%\');\r\n            if (");
#nullable restore
#line 78 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
           Write(ViewData["ThisMonthCals"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" < ");
#nullable restore
#line 78 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                        Write(Context.Session.GetInt32("TotalCal"));

#line default
#line hidden
#nullable disable
                WriteLiteral(") {\r\n                if (");
#nullable restore
#line 79 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
               Write(calPercentage);

#line default
#line hidden
#nullable disable
                WriteLiteral(" < 50) {\r\n                    $(\".progress-bar\").addClass(\"bg-success\");\r\n                }\r\n                else if (50 <= ");
#nullable restore
#line 82 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                          Write(calPercentage);

#line default
#line hidden
#nullable disable
                WriteLiteral(" < 75) {\r\n                    $(\".progress-bar\").addClass(\"bg-warning\");\r\n                }\r\n                else if (");
#nullable restore
#line 85 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                    Write(calPercentage);

#line default
#line hidden
#nullable disable
                WriteLiteral(@" >= 75) {
                    $("".progress-bar"").addClass(""bg-danger"");
                }
            }
            else {
                $("".progress-bar"").addClass(""bg-danger"");
            }

            var pieChart = new Chart(document.getElementById(""myChart""), {
                type: 'doughnut',
                data: {
                    labels: [""Snack"", ""Meat"", ""Seafood"", ""Dairy"", ""Grains"", ""Fruit"", ""Vegetable"", ""Others""],
                    datasets: [
                        {
                            label: ""Percentage (%)"",
                            backgroundColor: [""#C6D57E"", ""#D57E7E"", ""#A2CDCD"", ""#FFE1AF"", ""#CEE5D0"", ""#F3F0D7"", ""#FED2AA"", ""#FFBF86""],
                            data: [snack, meat, seafood, dairy, grains, fruit, vegetable, others]
                        }
                    ]
                },
                options: {
                    responsive: true,
                    legend: {
                        display: true
                  ");
                WriteLiteral(@"  },
                    title: {
                        display: true,
                        text: 'Total calories by categories'
                    }
                }
            });
            handleZeroData(pieChart);

            var lineChart = new Chart(document.getElementById(""line-chart""), {
                type: 'line',
                data: {
                    labels: [""Jan"", ""Feb"", ""Mar"", ""Apr"", ""May"", ""Jun"", ""Jul"", ""Aug"", ""Sep"", ""Oct"",""Nov"",""Dec""],
                    datasets: [{
                        data: [");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                          Write(thisYearCalList[0]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                              Write(thisYearCalList[1]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                  Write(thisYearCalList[2]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                      Write(thisYearCalList[3]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                                          Write(thisYearCalList[4]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                                                              Write(thisYearCalList[5]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                                                                                  Write(thisYearCalList[6]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                                                                                                      Write(thisYearCalList[7]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                                                                                                                          Write(thisYearCalList[8]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                                                                                                                                              Write(thisYearCalList[9]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                                                                                                                                                                  Write(thisYearCalList[10]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 123 "C:\Users\The Tay Family\Documents\Yik Yong\PFD\Assignment 1 Latest\GreenDemic\Views\Home\Main.cshtml"
                                                                                                                                                                                                                                                       Write(thisYearCalList[11]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"],
                        label: ""This month calories"",
                        borderColor: ""#3e95cd"",
                        fill: false
                    }
                    ]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Total calories by month'
                    }
                }
            });
            handleZeroData(lineChart);
        });

        function handleZeroData(myChart) {
            var dataArray = myChart.data.datasets[0].data;
            var sum = dataArray.reduce((a, b) => a + b, 0);
            console.log(sum);
            Chart.plugins.register({
                afterDraw: function (chart) {
                    if (sum == 0) {
                        myChart.data.datasets[0].hidden = true;
                        var ctx = myChart.chart.ctx;
                        var width = myChart.chart.width;
                        var height = myChart.char");
                WriteLiteral(@"t.height

                        ctx.textAlign = 'center';
                        ctx.textBaseline = 'middle';
                        ctx.font = ""20px 'Helvetica'"";
                        ctx.fillText('No data to display', width / 2, height / 2);
                    }
                }
            });
        }
    </script>
");
            }
            );
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
