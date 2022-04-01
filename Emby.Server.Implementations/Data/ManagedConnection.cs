#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using SQLitePCL.pretty;

namespace Emby.Server.Implementations.Data
{
    public sealed class ManagedConnection : IDisposable
    {
        private const string AttributeDbStatement = "db.statement";
        private readonly SemaphoreSlim? _writeLock;
        private readonly ActivitySource _activitySource = new("Emby.Server.Implementations.Data.ManagedConnection");

        private SQLiteDatabaseConnection? _db;

        private bool _disposed = false;

        public ManagedConnection(SQLiteDatabaseConnection db, SemaphoreSlim writeLock)
        {
            _db = db;
            _writeLock = writeLock;
        }

        public IStatement PrepareStatement(string sql)
        {
            using var activity = _activitySource.StartActivity();
            activity?.SetTag(AttributeDbStatement, sql);
            return _db.PrepareStatement(sql);
        }

        public IEnumerable<IStatement> PrepareAll(string sql)
        {
            using var activity = _activitySource.StartActivity();
            activity?.SetTag(AttributeDbStatement, sql);
            return _db.PrepareAll(sql);
        }

        public void ExecuteAll(string sql)
        {
            using var activity = _activitySource.StartActivity();
            activity?.SetTag(AttributeDbStatement, sql);
            _db.ExecuteAll(sql);
        }

        public void Execute(string sql, params object[] values)
        {
            using var activity = _activitySource.StartActivity();
            activity?.SetTag(AttributeDbStatement, sql);
            _db.Execute(sql, values);
        }

        public void RunQueries(string[] sql)
        {
            using var activity = _activitySource.StartActivity();
            activity?.SetTag(AttributeDbStatement, sql);
            _db.RunQueries(sql);
        }

        public void RunInTransaction(Action<IDatabaseConnection> action, TransactionMode mode)
        {
            using var activity = _activitySource.StartActivity();
            _db.RunInTransaction(action, mode);
        }

        public T RunInTransaction<T>(Func<IDatabaseConnection, T> action, TransactionMode mode)
        {
            using var activity = _activitySource.StartActivity();
            return _db.RunInTransaction(action, mode);
        }

        public IEnumerable<IReadOnlyList<ResultSetValue>> Query(string sql)
        {
            using var activity = _activitySource.StartActivity();
            activity?.SetTag(AttributeDbStatement, sql);
            return _db.Query(sql);
        }

        public IEnumerable<IReadOnlyList<ResultSetValue>> Query(string sql, params object[] values)
        {
            using var activity = _activitySource.StartActivity();
            activity?.SetTag(AttributeDbStatement, sql);
            return _db.Query(sql, values);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _writeLock?.Release();
            _activitySource.Dispose();

            _db = null; // Don't dispose it
            _disposed = true;
        }
    }
}
