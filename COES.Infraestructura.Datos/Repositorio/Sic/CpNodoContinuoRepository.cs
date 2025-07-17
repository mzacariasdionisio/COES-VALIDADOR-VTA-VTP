using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_NODO_CONTINUO
    /// </summary>
    public class CpNodoContinuoRepository : RepositoryBase, ICpNodoContinuoRepository
    {
        public CpNodoContinuoRepository(string strConn) : base(strConn)
        {
        }

        CpNodoContinuoHelper helper = new CpNodoContinuoHelper();        


        public int Save(CpNodoContinuoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodocodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodoestado, DbType.String, entity.Cpnodoestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cparbcodi, DbType.Int32, entity.Cparbcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodoconverge, DbType.String, entity.Cpnodoconverge));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodoflagcondterm, DbType.String, entity.Cpnodoflagcondterm));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodoflagconcompr, DbType.String, entity.Cpnodoflagconcompr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodoflagsincompr, DbType.String, entity.Cpnodoflagsincompr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodoflagrer, DbType.String, entity.Cpnodoflagrer));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodocarpeta, DbType.String, entity.Cpnodocarpeta));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodofeciniproceso, DbType.DateTime, entity.Cpnodofeciniproceso));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodofecfinproceso, DbType.DateTime, entity.Cpnodofecfinproceso));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodomsjproceso, DbType.String, entity.Cpnodomsjproceso));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodoidtopguardado, DbType.Int32, entity.Cpnodoidtopguardado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cpnodonumero, DbType.Int32, entity.Cpnodonumero));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }


        public void Update(CpNodoContinuoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Cpnodoestado, DbType.String, entity.Cpnodoestado);
            dbProvider.AddInParameter(command, helper.Cparbcodi, DbType.Int32, entity.Cparbcodi);
            dbProvider.AddInParameter(command, helper.Cpnodoconverge, DbType.String, entity.Cpnodoconverge);
            dbProvider.AddInParameter(command, helper.Cpnodoflagcondterm, DbType.String, entity.Cpnodoflagcondterm);
            dbProvider.AddInParameter(command, helper.Cpnodoflagconcompr, DbType.String, entity.Cpnodoflagconcompr);
            dbProvider.AddInParameter(command, helper.Cpnodoflagsincompr, DbType.String, entity.Cpnodoflagsincompr);
            dbProvider.AddInParameter(command, helper.Cpnodoflagrer, DbType.String, entity.Cpnodoflagrer);
            dbProvider.AddInParameter(command, helper.Cpnodocarpeta, DbType.String, entity.Cpnodocarpeta);
            dbProvider.AddInParameter(command, helper.Cpnodofeciniproceso, DbType.DateTime, entity.Cpnodofeciniproceso);
            dbProvider.AddInParameter(command, helper.Cpnodofecfinproceso, DbType.DateTime, entity.Cpnodofecfinproceso);
            dbProvider.AddInParameter(command, helper.Cpnodomsjproceso, DbType.String, entity.Cpnodomsjproceso);
            dbProvider.AddInParameter(command, helper.Cpnodoidtopguardado, DbType.Int32, entity.Cpnodoidtopguardado);
            dbProvider.AddInParameter(command, helper.Cpnodonumero, DbType.Int32, entity.Cpnodonumero);
            dbProvider.AddInParameter(command, helper.Cpnodocodi, DbType.Int32, entity.Cpnodocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpnodocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpnodocodi, DbType.Int32, cpnodocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpNodoContinuoDTO GetById(int cpnodocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpnodocodi, DbType.Int32, cpnodocodi);
            CpNodoContinuoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iTopiniciohora = dr.GetOrdinal(helper.Topiniciohora);
                    if (!dr.IsDBNull(iTopiniciohora)) entity.Topiniciohora = Convert.ToInt16(dr.GetValue(iTopiniciohora));

                }
            }

            return entity;
        }

        public CpNodoContinuoDTO GetByNumero(int cparbcodi, int cpnodonumero)
        {
            string sql = string.Format(helper.SqlGetByNumero, cparbcodi, cpnodonumero);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CpNodoContinuoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    int iTopiniciohora = dr.GetOrdinal(helper.Topiniciohora);
                    if (!dr.IsDBNull(iTopiniciohora)) entity.Topiniciohora = Convert.ToInt16(dr.GetValue(iTopiniciohora));
                }
            }

            return entity;
        }

        public List<CpNodoContinuoDTO> List()
        {
            List<CpNodoContinuoDTO> entitys = new List<CpNodoContinuoDTO>();
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

        public List<CpNodoContinuoDTO> GetByCriteria()
        {
            List<CpNodoContinuoDTO> entitys = new List<CpNodoContinuoDTO>();
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

        
        public List<CpNodoContinuoDTO> ListaPorArbol(int arbolcodi)
        {
            List<CpNodoContinuoDTO> entitys = new List<CpNodoContinuoDTO>();
            string sql = String.Format(helper.SqlListaPorArbol, arbolcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iTopnombre = dr.GetOrdinal(helper.Topnombre);
                    if (!dr.IsDBNull(iTopnombre)) entity.Topnombre = dr.GetString(iTopnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        
    }
}
