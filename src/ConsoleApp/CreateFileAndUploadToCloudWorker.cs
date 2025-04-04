using ConsoleApp.Processes.IO;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace ConsoleApp;

public class CreateFileAndUploadToCloudWorker(
    Kernel kernel,
    ILogger<CreateFileAndUploadToCloudWorker> logger) : BackgroundService
{
    private const string InitialEvent = "Start";

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var processBuilder = new ProcessBuilder("CreateFileAndUploadToCloud");

            var askFileNameStep = processBuilder.AddStepFromType<AskFileNameStep>();
            var createFileStep = processBuilder.AddStepFromType<CreateFileStep>();
            var getContentStep = processBuilder.AddStepFromType<GetContentStep>();
            var uploadToCloudStep = processBuilder.AddStepFromType<UploadToCloudStep>();

            processBuilder
                .OnInputEvent(InitialEvent)
                .SendEventTo(new(getContentStep));

            getContentStep
                .OnFunctionResult()
                .SendEventTo(new(askFileNameStep));

            getContentStep
                .OnFunctionResult()
                .SendEventTo(new(createFileStep, parameterName: "content"));

            askFileNameStep
                .OnFunctionResult()
                .SendEventTo(new(createFileStep, parameterName: "fileName"));

            createFileStep
                .OnFunctionResult()
                .SendEventTo(new(uploadToCloudStep));

            var process = processBuilder.Build();

            await process.StartAsync(kernel, new() { Id = InitialEvent });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while running the process.");
        }
    }
}