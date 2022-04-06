using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Persistence;

namespace Emby.Server.Implementations.Data.Couchdb;

public class UserDataRepository: IUserDataRepository
{
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {}

    public void SaveUserData(long userId, string key, UserItemData userData, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public UserItemData GetUserData(long userId, string key)
    {
        throw new System.NotImplementedException();
    }

    public UserItemData GetUserData(long userId, List<string> keys)
    {
        // TODO: Implement
        return new UserItemData()
        {
            // UserId = new Guid(userId.ToString(CultureInfo.InvariantCulture))
        };
    }

    public List<UserItemData> GetAllUserData(long userId)
    {
        throw new System.NotImplementedException();
    }

    public void SaveAllUserData(long userId, UserItemData[] userData, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
