﻿using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MongoDB.Bson;
using MongoDB.Driver.Core.Bindings;

// ReSharper disable once CheckNamespace
namespace MongoDB.Driver
{
    internal class CapMongoDbClientSessionHandle : IClientSessionHandle
    {
        private readonly ICapTransaction _transaction;
        private readonly IClientSessionHandle _sessionHandle;

        public CapMongoDbClientSessionHandle(ICapTransaction transaction)
        {
            _transaction = transaction;
            _sessionHandle = (IClientSessionHandle)_transaction.DbTransaction;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void AbortTransaction(CancellationToken cancellationToken = default(CancellationToken))
        {
            _transaction.Rollback();
        }

        public Task AbortTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _transaction.Rollback();
            return Task.CompletedTask;
        }

        public void AdvanceClusterTime(BsonDocument newClusterTime)
        {
            _sessionHandle.AdvanceClusterTime(newClusterTime);
        }

        public void AdvanceOperationTime(BsonTimestamp newOperationTime)
        {
            _sessionHandle.AdvanceOperationTime(newOperationTime);
        }

        public void CommitTransaction(CancellationToken cancellationToken = default(CancellationToken))
        {
            _transaction.Commit();
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _transaction.Commit();
            return Task.CompletedTask;
        }

        public void StartTransaction(TransactionOptions transactionOptions = null)
        {
            _sessionHandle.StartTransaction(transactionOptions);
        }

        public IMongoClient Client => _sessionHandle.Client;
        public BsonDocument ClusterTime => _sessionHandle.ClusterTime;
        public bool IsImplicit => _sessionHandle.IsImplicit;
        public bool IsInTransaction => _sessionHandle.IsInTransaction;
        public BsonTimestamp OperationTime => _sessionHandle.OperationTime;
        public ClientSessionOptions Options => _sessionHandle.Options;
        public IServerSession ServerSession => _sessionHandle.ServerSession;
        public ICoreSessionHandle WrappedCoreSession => _sessionHandle.WrappedCoreSession;
        public IClientSessionHandle Fork()
        {
            return _sessionHandle.Fork();
        }
    }
}
