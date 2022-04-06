using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Persistence;
using MediaBrowser.Controller.Playlists;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Querying;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Emby.Server.Implementations.Data.Couchdb;

/// <summary>
/// Base of couchdb repositories.
/// </summary>
public sealed class ItemRepository : IItemRepository
{
    private readonly IConnection _couchDb;

    /// <summary>
    ///
    /// </summary>
    /// <param name="couchDb"></param>
    public ItemRepository(IConnection couchDb)
    {
        _couchDb = couchDb;
    }

    /// <inheritdoc />
    public void DeleteItem(Guid id)
    {
        _couchDb.DeleteObject<BaseItem>(id).ConfigureAwait(true).GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public void SaveItems(IEnumerable<BaseItem> items, CancellationToken cancellationToken)
    {
        foreach (var baseItem in items)
        {
            _couchDb.SaveObject(baseItem.Id, baseItem, cancellationToken).ConfigureAwait(true).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc />
    public void SaveImages(BaseItem item)
    {
        throw new NotImplementedException("SaveImages");
    }

    /// <inheritdoc />
    public BaseItem RetrieveItem(Guid id)
    {
        return _couchDb.GetObject<BaseItem>(id).ConfigureAwait(true).GetAwaiter().GetResult()!;
    }

    /// <inheritdoc />
    public List<ChapterInfo> GetChapters(BaseItem item)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public ChapterInfo GetChapter(BaseItem item, int index)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void SaveChapters(Guid id, IReadOnlyList<ChapterInfo> chapters)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<MediaStream> GetMediaStreams(MediaStreamQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void SaveMediaStreams(Guid id, IReadOnlyList<MediaStream> streams, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<MediaAttachment> GetMediaAttachments(MediaAttachmentQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void SaveMediaAttachments(Guid id, IReadOnlyList<MediaAttachment> attachments, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public QueryResult<BaseItem> GetItems(InternalItemsQuery query)
    {
        return new QueryResult<BaseItem>(_couchDb.ListObjects<BaseItem>().ConfigureAwait(true).GetAwaiter().GetResult());
    }

    /// <inheritdoc />
    public List<Guid> GetItemIdsList(InternalItemsQuery query)
    {
        throw new NotImplementedException("GetItemIdsList");
    }

    /// <inheritdoc />
    public List<PersonInfo> GetPeople(InternalPeopleQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void UpdatePeople(Guid itemId, List<PersonInfo> people)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<string> GetPeopleNames(InternalPeopleQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<BaseItem> GetItemList(InternalItemsQuery query)
    {
        return _couchDb.ListObjects<BaseItem>().ConfigureAwait(true).GetAwaiter().GetResult().ToList()!;
    }

    /// <inheritdoc />
    public void UpdateInheritedValues()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public int GetCount(InternalItemsQuery query)
    {
        throw new NotImplementedException("GetCount");
    }

    /// <inheritdoc />
    public QueryResult<(BaseItem Item, ItemCounts ItemCounts)> GetGenres(InternalItemsQuery query)
    {
        throw new NotImplementedException("GetGenres");
    }

    /// <inheritdoc />
    public QueryResult<(BaseItem Item, ItemCounts ItemCounts)> GetMusicGenres(InternalItemsQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public QueryResult<(BaseItem Item, ItemCounts ItemCounts)> GetStudios(InternalItemsQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public QueryResult<(BaseItem Item, ItemCounts ItemCounts)> GetArtists(InternalItemsQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public QueryResult<(BaseItem Item, ItemCounts ItemCounts)> GetAlbumArtists(InternalItemsQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public QueryResult<(BaseItem Item, ItemCounts ItemCounts)> GetAllArtists(InternalItemsQuery query)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<string> GetMusicGenreNames()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<string> GetStudioNames()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<string> GetGenreNames()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<string> GetAllArtistNames()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
    }
}
