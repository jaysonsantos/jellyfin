using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;

namespace Emby.Server.Implementations.Data.Couchdb;

public interface IConnection
{
    /// <summary>
    /// Gets an item from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> GetObject<T>(Guid id);

    /// <summary>
    /// List all items of a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<List<T>> ListObjects<T>();

    Task DeleteObject<T>(Guid id);

    Task SaveObject<T>(Guid id, T data, CancellationToken cancellationToken);
}
