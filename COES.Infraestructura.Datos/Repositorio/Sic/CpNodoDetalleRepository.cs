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
    /// Clase de acceso a datos de la tabla CP_NODO_DETALLE
    /// </summary>
    public class CpNodoDetalleRepository: RepositoryBase, ICpNodoDetalleRepository
    {
        public CpNodoDetalleRepository(string strConn): base(strConn)
        {
        }

        CpNodoDetalleHelper helper = new CpNodoDetalleHelper();        

        public int Save(CpNodoDetalleDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpndetcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnconcodi, DbType.Int32, entity.Cpnconcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodocodi, DbType.Int32, entity.Cpnodocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpndetvalor, DbType.String, entity.Cpndetvalor));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpNodoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Cpnconcodi, DbType.Int32, entity.Cpnconcodi);
            dbProvider.AddInParameter(command, helper.Cpnodocodi, DbType.Int32, entity.Cpnodocodi);
            dbProvider.AddInParameter(command, helper.Cpndetvalor, DbType.String, entity.Cpndetvalor);
            dbProvider.AddInParameter(command, helper.Cpndetcodi, DbType.Int32, entity.Cpndetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpndetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpndetcodi, DbType.Int32, cpndetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpNodoDetalleDTO GetById(int cpndetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpndetcodi, DbType.Int32, cpndetcodi);
            CpNodoDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpNodoDetalleDTO> List()
        {
            List<CpNodoDetalleDTO> entitys = new List<CpNodoDetalleDTO>();
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

        
        public List<CpNodoDetalleDTO> ListaPorNodo(int codigoNodo)
        {
            
            List<CpNodoDetalleDTO> entitys = new List<CpNodoDetalleDTO>();
            string sql = String.Format(helper.SqlListaPorNodo, codigoNodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CpNodoDetalleDTO> ListaPorArbol(int codigoArbol)
        {

            List<CpNodoDetalleDTO> entitys = new List<CpNodoDetalleDTO>();
            string sql = String.Format(helper.SqlListaPorArbol, codigoArbol);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CpNodoDetalleDTO> GetByCriteria()
        {
            List<CpNodoDetalleDTO> entitys = new List<CpNodoDetalleDTO>();
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
