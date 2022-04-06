using System;
using Emby.Server.Implementations.Data.Couchdb;

namespace Emby.Server.Implementations.Tests.Data.Couchdb;

public class ConnectionTest
{
    private Connection _connection;

    public void CreateDatabase()
    {
        client.BaseAddress = new Uri("http://localhost:5984/");
        _connection = new Connection(client);
    }
}
