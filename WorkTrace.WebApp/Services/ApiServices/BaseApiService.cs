using System.Text.Json;
﻿using System.Text.RegularExpressions;
﻿
﻿namespace WorkTrace.WebApp.Services;
﻿
﻿public abstract class BaseApiService
﻿{
﻿    protected readonly HttpClient _client;
﻿
﻿    protected BaseApiService(HttpClient client)
﻿    {
﻿        _client = client;
﻿    }
﻿    
﻿    protected async Task<T?> ReadResponse<T>(HttpResponseMessage response)
﻿    {
﻿        if (!response.IsSuccessStatusCode)
﻿        {
﻿            var errorContent = await response.Content.ReadAsStringAsync();
﻿            throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
﻿        }
﻿
﻿        var content = await response.Content.ReadAsStringAsync();
﻿        if (string.IsNullOrEmpty(content))
﻿        {
﻿            return default;
﻿        }
﻿        
﻿        // Replace ObjectId format {"$oid": "..."} with just the string value
﻿        content = Regex.Replace(content, @"""\$oid"":\s*""([^""]+)""", @"""﻿""");
﻿
﻿        return JsonSerializer.Deserialize<T>(
﻿            content,
﻿            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
﻿    }
﻿}
﻿