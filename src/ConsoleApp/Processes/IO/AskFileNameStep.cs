using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace ConsoleApp.Processes.IO;

public class AskFileNameStep : KernelProcessStep
{
    [KernelFunction]
    public string? AskFileName(Kernel kernel)
    {
        var logger = kernel.GetRequiredService<ILogger<AskFileNameStep>>();

        logger.LogInformation("Asking for file name...");

        Console.WriteLine("Please enter the file name:");

        var fileName = Console.ReadLine();
        
        return string.IsNullOrWhiteSpace(fileName) 
            ? Path.GetRandomFileName()
            : fileName;
    }
}
