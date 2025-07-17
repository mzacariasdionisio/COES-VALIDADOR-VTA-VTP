/*********************************************************************************
* Fecha de Creación: 29-05-2014
* Creado por: COES SINAC
* Fecha de Modificación:
* Modificado por:
* Descripción: Interface que define los métodos de acceso a datos.
*********************************************************************************/

using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Base.DataHelper
{
    public interface IDataBase
    {
        DbCommand GetStoredProcCommand(string storedProcedureName);
        DbCommand GetSqlStringCommand(string query);
        void AddInParameter(DbCommand command, string name, DbType dbType, object value);
        IDataReader ExecuteReader(DbCommand command);
        void AddOutParameter(DbCommand command, string name, DbType dbType, int size);
        object GetParameterValue(DbCommand command, string name);
        object ExecuteScalar(DbCommand command);
        int ExecuteNonQuery(DbCommand command);
        void LoadDataSet(DbCommand command, DataSet dataSet, string tableName);
        void AddColumnMapping(string name, DbType dbType);
        void BulkInsert<T>(IEnumerable<T> entitys, string tableName);
    }
}
