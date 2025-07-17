/*****************************************************************************************
* Fecha de Creación: 29-05-2014
* Creado por: COES SINAC
* Descripción: Clase que implementa la interface de acceso a datos
* haciendo uso del Data Access Application Block
*****************************************************************************************/

using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using COES.Framework.Base.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Oracle.ManagedDataAccess.Types;
using COES.Framework.Base.Core;
using COES.Base.Tools;
using System.Configuration;

namespace COES.Base.DataHelper
{
    public class DbProvider : IDataBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Database dataBase;
        private string dbName;
        private List<ColumnMaper> columnsMaper;

        /// <summary>
        /// Contructor de la clase
        /// </summary>
        /// <param name="dbName"></param>
        public DbProvider(string dbName)
        {
            dataBase = DatabaseFactory.CreateDatabase(dbName);
            this.dbName = dbName;
            columnsMaper = new List<ColumnMaper>();
        }

        /// <summary>
        /// Devuelve un objeto DbCommand encargado de ejecutar el store procedure
        /// especificado
        /// </summary>
        /// <param name="storedProcedureName">Nombre del Store Procedure</param>
        /// <returns></returns>
        public DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            return dataBase.GetStoredProcCommand(storedProcedureName);
        }

        /// <summary>
        /// Devuelve un objeto DbCommand encargado de ejecutar el query especificado
        /// </summary>
        /// <param name="query">Query a ejecutar</param>
        /// <returns></returns>
        public DbCommand GetSqlStringCommand(string query)
        {
            return dataBase.GetSqlStringCommand(query);
        }

        /// <summary>
        /// Agrega parámetros de entrada que son necesarios para ejecutar un comando
        /// </summary>
        /// <param name="command">Objeto DbCommnand que requiere el parámetro</param>
        /// <param name="name">Nombre del parámetro</param>
        /// <param name="dbType">Tipo de dato del parámetro</param>
        /// <param name="value">Valor del parámetro</param>
        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            dataBase.AddInParameter(command, name, dbType, value);
        }

        /// <summary>
        /// Agrega parámetros de salida al comando especificado
        /// </summary>
        /// <param name="command">Objeto DbCommand que contiene el parámetro</param>
        /// <param name="name">Nombre del parámetro</param>
        /// <param name="dbType">Tipo de dato del parámetro</param>
        /// <param name="size">Longitud del parámetro</param>
        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            dataBase.AddOutParameter(command, name, dbType, size);
        }

        /// <summary>
        /// Ejecuta el comando y devuelve un objeto IDataReader para la lectura de datos
        /// </summary>
        /// <param name="command">Comando a ejecutar</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand command)
        {
            return dataBase.ExecuteReader(command);
        }

        /// <summary>
        /// Obtiene el valor de un parametro despues de haber ejecutado del comando
        /// </summary>
        /// <param name="command">Objeto DbCommand a ejecutar</param>
        /// <param name="name">Nombre del parámetro</param>
        /// <returns></returns>
        public object GetParameterValue(DbCommand command, string name)
        {
            return dataBase.GetParameterValue(command, name);
        }

        /// <summary>
        /// Obtiene el valor de la primera celda en la primera fila del resultado
        /// tras haber ejecutado el comando
        /// </summary>
        /// <param name="command">Objeto DbCommand a ejecutar</param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand command)
        {
            return dataBase.ExecuteScalar(command);
        }

        /// <summary>
        /// Retorna el número de filas afectadas despues de haber ejecutado un comando
        /// </summary>
        /// <param name="command">Objeto DbCommand a ajecutar</param>
        public int ExecuteNonQuery(DbCommand command)
        {
            return dataBase.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Retorna el número de filas afectadas despues de haber ejecutado el comando y genera auditoria de la tabla
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int ExecuteNonQueryAudit(DbCommand command, string user)
        {
            int result = dataBase.ExecuteNonQuery(command);
            string query = DbUtil.Audit(this.dataBase, command, user);

            if (!string.IsNullOrEmpty(query))
            {
                command.CommandText = query;
                this.dataBase.ExecuteNonQuery(command);
            }

            return result;
        }

        /// <summary>
        /// Carga los datos obtenidos tras ejecutar un comando a un dataset
        /// </summary>
        /// <param name="command">Objeto DbCommand a ejecutar</param>
        /// <param name="dataSet">DataSet donde se carga el resultado</param>
        /// <param name="tableName">Nombre de la tabla cargada</param>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName)
        {
            dataBase.LoadDataSet(command, dataSet, tableName);
        }
        /// <summary>
        /// Permite agregar columnas de mapeo
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        public void AddColumnMapping(string name, DbType dbType)
        {
            this.columnsMaper.Add(new ColumnMaper { DbTipo = dbType, Nombre = name });
        }


        /// <summary>
        /// Permite crear un parametro para el command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDbDataParameter CreateParameter(DbCommand command, string name, DbType dbType, object value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = dbType;
            return parameter;
        }

        /// <summary>
        /// Permite realizar el grabado en bulk a oracle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="copy"></param>
        /// <param name="entities"></param>
        public void BulkInsert<T>(IEnumerable<T> entitys, string tableName)
        {
            string conn = ConfigurationManager.ConnectionStrings[this.dbName].ConnectionString;
            OracleConnection connection = new OracleConnection(conn);
            connection.Open();
            OracleBulkCopy copy = new OracleBulkCopy(connection);
            copy.DestinationTableName = tableName;
            copy.BulkCopyTimeout = 3600;
            copy.BatchSize = 15000;


            DataTable dt = new DataTable(tableName);

            foreach (ColumnMaper item in this.columnsMaper)
            {
                switch (item.DbTipo)
                {
                    case DbType.Int32:
                        {
                            dt.Columns.Add(item.Nombre, typeof(decimal));
                            break;
                        }
                    case DbType.String:
                        {
                            dt.Columns.Add(item.Nombre, typeof(string));
                            break;
                        }
                    case DbType.DateTime:
                        {
                            dt.Columns.Add(item.Nombre, typeof(DateTime));
                            break;
                        }
                    case DbType.Decimal:
                        {
                            dt.Columns.Add(item.Nombre, typeof(decimal));
                            break;
                        }
                    case DbType.Int16:
                        {
                            dt.Columns.Add(item.Nombre, typeof(int));
                            break;
                        }
                }
            }

            foreach (var entity in entitys)
            {
                var row = dt.NewRow();

                foreach (ColumnMaper maper in this.columnsMaper)
                {
                    string campo = Util.ToPascalCase(maper.Nombre);
                    var valor = entity.GetType().GetProperty(Util.ToPascalCase(maper.Nombre)).GetValue(entity, null);

                    if (valor != null)
                    {
                        switch (maper.DbTipo)
                        {
                            case DbType.Int32:
                                {
                                    valor = Convert.ToDecimal(valor);
                                    break;
                                }
                            case DbType.String:
                                {
                                    valor = Convert.ToString(valor);
                                    break;
                                }
                            case DbType.DateTime:
                                {
                                    valor = (DateTime)valor;
                                    break;
                                }
                            case DbType.Decimal:
                                {
                                    valor = Convert.ToDecimal(valor);
                                    break;
                                }
                            case DbType.Int16:
                                {
                                    valor = Convert.ToInt16(valor);
                                    break;
                                }
                        }
                    }

                    row[maper.Nombre] = (valor != null) ? valor : DBNull.Value;
                }

                dt.Rows.Add(row);
            }

            copy.WriteToServer(dt);
            connection.Close();
        }


        /// <summary>
        /// Permite realizar el grabado en bulk a oracle para el aplicativo RSF_PR22
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="copy"></param>
        /// <param name="entities"></param>
        public void BulkInsertRSF<T>(IEnumerable<T> entitys, string tableName)
        {
            string conn = ConfigurationManager.ConnectionStrings[this.dbName].ConnectionString;
            OracleConnection connection = new OracleConnection(conn);
            connection.Open();
            OracleBulkCopy copy = new OracleBulkCopy(connection);
            copy.DestinationTableName = tableName;
            copy.BulkCopyTimeout = 4000;
            copy.BatchSize = 60000;


            DataTable dt = new DataTable(tableName);

            foreach (ColumnMaper item in this.columnsMaper)
            {
                switch (item.DbTipo)
                {
                    case DbType.Int32:
                        {
                            dt.Columns.Add(item.Nombre, typeof(decimal));
                            break;
                        }
                    case DbType.String:
                        {
                            dt.Columns.Add(item.Nombre, typeof(string));
                            break;
                        }
                    case DbType.DateTime:
                        {
                            dt.Columns.Add(item.Nombre, typeof(DateTime));
                            break;
                        }
                    case DbType.Decimal:
                        {
                            dt.Columns.Add(item.Nombre, typeof(decimal));
                            break;
                        }
                    case DbType.Int16:
                        {
                            dt.Columns.Add(item.Nombre, typeof(int));
                            break;
                        }
                }
            }

            foreach (var entity in entitys)
            {
                var row = dt.NewRow();

                foreach (ColumnMaper maper in this.columnsMaper)
                {
                    string campo = Util.ToPascalCase(maper.Nombre);
                    var valor = entity.GetType().GetProperty(Util.ToPascalCase(maper.Nombre)).GetValue(entity, null);

                    if (valor != null)
                    {
                        switch (maper.DbTipo)
                        {
                            case DbType.Int32:
                                {
                                    valor = Convert.ToDecimal(valor);
                                    break;
                                }
                            case DbType.String:
                                {
                                    valor = Convert.ToString(valor);
                                    break;
                                }
                            case DbType.DateTime:
                                {
                                    valor = (DateTime)valor;
                                    break;
                                }
                            case DbType.Decimal:
                                {
                                    valor = Convert.ToDecimal(valor);
                                    break;
                                }
                            case DbType.Int16:
                                {
                                    valor = Convert.ToInt16(valor);
                                    break;
                                }
                        }
                    }

                    row[maper.Nombre] = (valor != null) ? valor : DBNull.Value;
                }

                dt.Rows.Add(row);
            }

            copy.WriteToServer(dt);
            connection.Close();
        }
    }
}
