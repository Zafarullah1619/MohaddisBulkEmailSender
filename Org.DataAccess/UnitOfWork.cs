using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace Org.DataAccess
{


    public interface IUnitOfWork : IDisposable
    {
        IDatabaseContext DataContext { get; }
        void BeginTransaction();
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public SqlTransaction Transaction { get; private set; }
        private DatabaseContext _dataContext;
        public UnitOfWork(string ConnectionStringName = "DefaultConnection")
        {
            _dataContext = new DatabaseContext(ConnectionStringName);
        }

        public UnitOfWork(string ConnectionStringName, string CustomConnection)
        {
            _dataContext = new DatabaseContext(ConnectionStringName, CustomConnection);
        }

        /// <summary>
        /// Define a property of context class
        /// </summary>
        public IDatabaseContext DataContext
        {
            get { return _dataContext; }
        }




        public void BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new NullReferenceException("Not finished previous transaction");
            }
            Transaction = DataContext.Connection.BeginTransaction();
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                try
                {
                    Transaction.Commit();
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                }
                Transaction.Dispose();
                Transaction = null;
            }
            else
            {
                throw new NullReferenceException("Tryed commit not opened transaction");
            }
        }

        /// <summary>
        /// Define a property of context class
        /// </summary>



        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
            }
            if (DataContext != null)
            {
                DataContext.Dispose();
            }
        }


    }
}
