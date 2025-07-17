using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RI_DETALLE_REVISION
    /// </summary>
    public class RiDetalleRevisionRepository : RepositoryBase, IRiDetalleRevisionRepository
    {
        public RiDetalleRevisionRepository(string strConn) : base(strConn)
        {
        }

        RiDetalleRevisionHelper helper = new RiDetalleRevisionHelper();

        public int Save(RiDetalleRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {

            int id = 1;
            if (entity.Dervcodi == 0)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                if (result != null) id = Convert.ToInt32(result);
            }
            else
            {
                id = entity.Dervcodi + 1;
            }

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;


            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Dervcodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.Dervcampo; param.Value = entity.Dervcampo; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervvalor; param.Value = entity.Dervvalor; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervobservacion; param.Value = entity.Dervobservacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervadjunto; param.Value = entity.Dervadjunto; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervvaloradjunto; param.Value = entity.Dervvaloradjunto; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Revicodi; param.Value = entity.Revicodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervusucreacion; param.Value = entity.Dervusucreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervfeccreacion; param.Value = entity.Dervfeccreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervusumoficicacion; param.Value = entity.Dervusumoficicacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervfecmodificacion; param.Value = entity.Dervfecmodificacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervestado; param.Value = entity.Dervestado; command2.Parameters.Add(param);

            command2.ExecuteNonQuery();
            return id;
        }

        public void Update(RiDetalleRevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dervcampo, DbType.String, entity.Dervcampo);
            dbProvider.AddInParameter(command, helper.Dervvalor, DbType.String, entity.Dervvalor);
            dbProvider.AddInParameter(command, helper.Dervobservacion, DbType.String, entity.Dervobservacion);
            dbProvider.AddInParameter(command, helper.Dervadjunto, DbType.String, entity.Dervadjunto);
            dbProvider.AddInParameter(command, helper.Dervvaloradjunto, DbType.String, entity.Dervvaloradjunto);
            dbProvider.AddInParameter(command, helper.Revicodi, DbType.Int32, entity.Revicodi);
            dbProvider.AddInParameter(command, helper.Dervusucreacion, DbType.String, entity.Dervusucreacion);
            dbProvider.AddInParameter(command, helper.Dervfeccreacion, DbType.DateTime, entity.Dervfeccreacion);
            dbProvider.AddInParameter(command, helper.Dervusumoficicacion, DbType.String, entity.Dervusumoficicacion);
            dbProvider.AddInParameter(command, helper.Dervfecmodificacion, DbType.DateTime, entity.Dervfecmodificacion);
            dbProvider.AddInParameter(command, helper.Dervcodi, DbType.Int32, entity.Dervcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        public void UpdateEstado(RiDetalleRevisionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlUpdateEstado;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = command2.CreateParameter(); param.ParameterName = helper.Dervestado; param.Value = entity.Dervestado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervusumoficicacion; param.Value = entity.Dervusumoficicacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Dervfecmodificacion; param.Value = entity.Dervfecmodificacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Revicodi; param.Value = entity.Revicodi;command2.Parameters.Add(param);
            

            int filas = command2.ExecuteNonQuery();
        }

        public void Delete(int dervcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dervcodi, DbType.Int32, dervcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RiDetalleRevisionDTO GetById(int dervcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dervcodi, DbType.Int32, dervcodi);
            RiDetalleRevisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RiDetalleRevisionDTO> List()
        {
            List<RiDetalleRevisionDTO> entitys = new List<RiDetalleRevisionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite listar los detalles de una revisión
        /// </summary>
        /// <param name="revicodi"></param>
        /// <returns>Listado de Detalle de Revision</returns>
        public List<RiDetalleRevisionDTO> ListByRevicodi(int revicodi)
        {
            List<RiDetalleRevisionDTO> entitys = new List<RiDetalleRevisionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(String.Format(helper.SqlListByRevicodi, revicodi));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RiDetalleRevisionDTO> GetByCriteria()
        {
            List<RiDetalleRevisionDTO> entitys = new List<RiDetalleRevisionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
