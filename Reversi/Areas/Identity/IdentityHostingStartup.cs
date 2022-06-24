using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;

[assembly: HostingStartup(typeof(ReversiMvcApp.Areas.Identity.IdentityHostingStartup))]
namespace ReversiMvcApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) { }
    }
}