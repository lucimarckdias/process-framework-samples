using Microsoft.SemanticKernel;

namespace ConsoleApp.Processes.IO;

public class CreateFileStep : KernelProcessStep
{
    [KernelFunction]
    public string CreateFile(string? fileName, string? content = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(fileName);

        Console.WriteLine($"Creating file: {fileName}");

        var directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var path = Path.Combine(directory, $"{DateTimeOffset.UtcNow:yyyyMMddTHHmmss}-{fileName}");

        if (File.Exists(path))
        {
            Console.WriteLine($"File already exists: {path}");
            return path;
        }

        using var fileStream = File.Create(path);
        using var writer = new StreamWriter(fileStream);

        writer.WriteLine(content ?? "Hello, world!");

        return path;
    }
}