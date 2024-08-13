using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Luthetus.Common.RazorLib.Installations.Models;
using Luthetus.Common.RazorLib.BackgroundTasks.Models;
using Luthetus.Tutorials.RazorLib;
using Luthetus.Tutorials.Wasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var hostingInformation = new LuthetusHostingInformation(
    LuthetusHostingKind.Wasm,
    new BackgroundTaskService());

builder.Services.AddLuthetusTutorialsRazorLibServices(hostingInformation);

var host = builder.Build();
hostingInformation.StartBackgroundTaskWorkers(host.Services);

await host.RunAsync();