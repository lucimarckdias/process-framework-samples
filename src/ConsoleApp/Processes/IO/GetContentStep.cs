using Microsoft.SemanticKernel;

namespace ConsoleApp.Processes.IO;

public class GetContentStep : KernelProcessStep
{
    [KernelFunction]
    public async Task<string?> GetContentAsync(Kernel kernel)
    {
        string? url = null;

        ArgumentNullException.ThrowIfNull(kernel);

        while (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            Console.WriteLine($"Please enter a valid URL:");
            url = Console.ReadLine();
        }

        var httpClient = kernel.GetRequiredService<HttpClient>();
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        return null;
    }
}
