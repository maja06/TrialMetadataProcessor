using Microsoft.AspNetCore.Http;

namespace TrialMetadataProcessor.Application.Common.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<string> ReadAsStringAsync(this IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
