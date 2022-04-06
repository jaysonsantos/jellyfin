using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Jellyfin.Data.Enums;
using MediaBrowser.Controller.Channels;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Controller.Playlists;

namespace MediaBrowser.Controller.Entities.Json;

/// <inheritdoc />
public class JsonBaseItemConverter : JsonConverter<BaseItem?>
{
    /// <inheritdoc />
    public override BaseItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<JsonObject>(ref reader, Jellyfin.Extensions.Json.JsonDefaults.PascalCaseOptions);
        if (value == null)
        {
            return default;
        }

        var kind = value["BaseItemKind"];
        var type = kind?.Deserialize<BaseItemKind>(Jellyfin.Extensions.Json.JsonDefaults.PascalCaseOptions) switch
        {
            BaseItemKind.Playlist => typeof(Playlist),
            BaseItemKind.UserView => typeof(UserView),
            BaseItemKind.Audio => typeof(Audio.Audio),
            BaseItemKind.MusicAlbum => typeof(MusicAlbum),
            BaseItemKind.MusicArtist => typeof(MusicArtist),
            BaseItemKind.MusicGenre => typeof(MusicGenre),
            BaseItemKind.MusicVideo => typeof(MusicVideo),
            BaseItemKind.Movie => typeof(Movie),
            BaseItemKind.Trailer => typeof(Trailer),
            BaseItemKind.Series => typeof(Series),
            BaseItemKind.Season => typeof(Season),
            BaseItemKind.Episode => typeof(Episode),
            BaseItemKind.Program => typeof(ProgramInfo),
            BaseItemKind.Book => typeof(Book),
            BaseItemKind.BoxSet => typeof(BoxSet),
            BaseItemKind.AggregateFolder => typeof(AggregateFolder),
            BaseItemKind.Folder => typeof(Folder),
            BaseItemKind.CollectionFolder => typeof(CollectionFolder),
            BaseItemKind.Photo => typeof(Photo),
            BaseItemKind.PhotoAlbum => typeof(PhotoAlbum),
            BaseItemKind.AudioBook => typeof(AudioBook),
            BaseItemKind.Channel => typeof(Channel),
            BaseItemKind.LiveTvChannel => typeof(LiveTvChannel),
            BaseItemKind.LiveTvProgram => typeof(LiveTvProgram),
            BaseItemKind.Video => typeof(Video),
            BaseItemKind.ManualPlaylistsFolder => typeof(Playlist),
            _ => throw new Exception($"Unknown item type '{kind}'")
        };
        return value.Deserialize(type, Jellyfin.Extensions.Json.JsonDefaults.PascalCaseOptions) as BaseItem;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, BaseItem? value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, Jellyfin.Extensions.Json.JsonDefaults.PascalCaseOptions);
    }
}
