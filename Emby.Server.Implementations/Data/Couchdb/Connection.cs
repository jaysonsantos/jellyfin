using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Emby.Server.Implementations.Data.Couchdb;

/// <summary>
///
/// </summary>
public abstract class BaseConnection
{
    /// <summary>
    /// Default http client to be used in configuration.
    /// </summary>
    public const string HttpClientName = "CouchDB";
}

internal class CouchDbListResponse
{
    [JsonPropertyName("total_rows")] public int TotalRows { get; set; }

    [JsonPropertyName("offset")] public int Offset { get; set; }

    [JsonPropertyName("rows")] public List<CouchDbRow>? Rows { get; set; }
}

internal class CouchDbRow
{
    [JsonPropertyName("doc")] public JsonValue? Document { get; set; }
}

/// <inheritdoc cref="Emby.Server.Implementations.Data.Couchdb.IConnection" />
public class Connection : BaseConnection, IConnection
{
    private readonly IHttpClientFactory _httpClientFactory;

    private static readonly Regex _databaseName = new(@"([a-z])([A-Z])", RegexOptions.Compiled);
    private static readonly ConcurrentDictionary<string, string> _createdDatabases = new();

    private JsonSerializerOptions _jsonSerializerOptions;

    private HttpClient Client
    {
        get
        {
            return _httpClientFactory.CreateClient(HttpClientName);
        }
    }

    /// <inheritdoc />
    public Connection(IHttpClientFactory httpClientFactory, IConfigureOptions<JsonOptions> defaultJsonOptions)
    {
        _httpClientFactory = httpClientFactory;
        JsonOptions options = new();
        defaultJsonOptions.Configure(options);
        options.JsonSerializerOptions.Converters.Add(new JsonBaseItemConverter());
        _jsonSerializerOptions = options.JsonSerializerOptions;
    }

    /// <summary>
    /// Get the specified item by an id.
    /// </summary>
    /// <param name="id">Unique id that identifies an given item.</param>
    /// <typeparam name="T">The type that you want to get from the storage.</typeparam>
    /// <returns>Returns an instance of the given type or null.</returns>
    public async Task<T?> GetObject<T>(Guid id)
    {
        var databaseName = CreateOrCreateDatabase<T>();
        try
        {
            return await Client.GetFromJsonAsync<T>($"{databaseName}/{id:N}", _jsonSerializerOptions).ConfigureAwait(true);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode != HttpStatusCode.NotFound)
            {
                throw;
            }

            return default;
        }
    }

    /// <inheritdoc />
    public async Task DeleteObject<T>(Guid id)
    {
        var databaseName = CreateOrCreateDatabase<T>();
        await Client.DeleteAsync($"{databaseName}/{id:N}").ConfigureAwait(true);
    }

    /// <inheritdoc />
    public async Task SaveObject<T>(Guid id, T data, CancellationToken cancellationToken)
    {
        var databaseName = CreateOrCreateDatabase<T>();
        await Client.PutAsJsonAsync($"{databaseName}/{id:N}", data, _jsonSerializerOptions, cancellationToken).ConfigureAwait(true);
    }

    /// <inheritdoc />
    public async Task<List<T>> ListObjects<T>()
    {
        var databaseName = CreateOrCreateDatabase<T>();
        var requestUri = $"{databaseName}/_all_docs?include_docs=true";
        var response = await Client.GetFromJsonAsync<CouchDbListResponse>(requestUri, _jsonSerializerOptions).ConfigureAwait(true);
        var objects = response?.Rows?.Select(item => item.Document.Deserialize<T>(_jsonSerializerOptions)).ToList();
        return (objects ?? new())!;
    }

    private string CreateOrCreateDatabase<T>()
    {
        return _createdDatabases.GetOrAdd(typeof(T).Name, databaseName =>
        {
            databaseName = GetDatabaseName(databaseName);
            var databaseExists = DatabaseExists(databaseName).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!databaseExists)
            {
                CreateDatabase(databaseName);
            }

            return databaseName;
        });
    }

    private async void CreateDatabase(string databaseName)
    {
        var response = await Client.PutAsync(databaseName, null).ConfigureAwait(true);
        response.EnsureSuccessStatusCode();
    }

    private async Task<bool> DatabaseExists(string databaseName)
    {
        var request = new HttpRequestMessage(HttpMethod.Head, databaseName);
        var response = await Client.SendAsync(request).ConfigureAwait(true);
        return response.IsSuccessStatusCode;
    }

    private string GetDatabaseName(string databaseName)
    {
        var name = _databaseName.Replace(databaseName, "$1_$2").ToLowerInvariant();
        return $"jellyfin_{name}";
    }
}
