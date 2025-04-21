using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

// CodeFirstApi
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
builder.Services.AddHttpClient("GeneratedClients", client =>client.BaseAddress =baseAddress );
builder.Services.AddGeneratedClients();

await builder.Build().RunAsync();