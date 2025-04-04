using ConsoleApp;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton(provider => new Kernel(provider));
builder.Services.AddHostedService<CreateFileAndUploadToCloudWorker>();

using var host = builder.Build();

await host.RunAsync();