using System.Text.Json;

namespace WorkTrace.WebApp.Helpers
{
    public static class ErrorParser
    {
        public static string Parse(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return "Ocurrió un error desconocido.";

            var contentToParse = errorMessage;
            var separator = " - ";
            var separatorIndex = errorMessage.IndexOf(separator);
            if (separatorIndex >= 0)
            {
                contentToParse = errorMessage.Substring(separatorIndex + separator.Length);
            }
            
            try
            {
                using (var jsonDoc = JsonDocument.Parse(contentToParse))
                {
                    if (jsonDoc.RootElement.TryGetProperty("errors", out var errorsElement))
                    {
                        var errorMessages = new List<string>();
                        foreach (var property in errorsElement.EnumerateObject())
                        {
                            foreach (var error in property.Value.EnumerateArray())
                            {
                                errorMessages.Add(error.GetString());
                            }
                        }
                        if(errorMessages.Count > 0)
                            return string.Join("\n", errorMessages);
                    }

                    if (jsonDoc.RootElement.TryGetProperty("title", out var titleElement))
                    {
                        var title = titleElement.GetString();
                        if(!string.IsNullOrEmpty(title))
                            return title;
                    }
                }
            }
            catch (JsonException)
            {
                // Not a JSON, or malformed. Return the content part.
                return contentToParse;
            }

            return contentToParse; // Fallback to content part
        }
    }
}
