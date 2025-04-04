using System.Diagnostics;

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace ConsoleApp.Processes.IO;

public class UploadToCloudStep : KernelProcessStep
{
    [KernelFunction]
    public async Task UploadToCloudAsync(Kernel kernel, string? filePath)
    {
        var logger = kernel.GetRequiredService<ILogger<UploadToCloudStep>>();

        logger.LogInformation("Validating file path...");

        var filePathIsNullOrEmpty = string.IsNullOrWhiteSpace(filePath);

        if (filePathIsNullOrEmpty)
        {
            logger.LogError("File path is null or empty.");
            return;
        }

        var fileExists = File.Exists(filePath);

        if (!fileExists)
        {
            logger.LogError("File does not exist: {filePath}", filePath);
            return;
        }

        logger.LogInformation("Uploading file to cloud...");

        var totalUploadTime = 10; // total upload time in seconds
        var stopwatch = Stopwatch.StartNew();

        for (int i = 0; i <= totalUploadTime; i++)
        {
            await Task.Delay(1000); // simulate 1 second upload time per iteration
            var elapsed = stopwatch.Elapsed.Seconds;
            var eta = totalUploadTime - elapsed;
            logger.LogInformation("Upload progress: {progress}%, ETA: {eta} seconds", (i * 100) / totalUploadTime, eta);
        }

        stopwatch.Stop();

        logger.LogInformation("File upload completed.");
    }
}
