using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_CENTRAL_PMPO
    /// </summary>
    public class CpaCentralPmpoRepository : RepositoryBase, ICpaCentralPmpoRepository
    {
        public CpaCentralPmpoRepository(string strConn)
            : base(strConn)
        {
        }

        CpaCentralPmpoHelper helper = new CpaCentralPmpoHelper();

        public int Save(CpaCentralPmpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpacnpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, entity.Cpacntcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Cpacnpusumodificacion, DbType.String, entity.Cpacnpusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpacnpfecmodificacion, DbType.DateTime, entity.Cpacnpfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaCentralPmpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpacnpcodi, DbType.Int32, entity.Cpacnpcodi);
            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, entity.Cpacntcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Cpacnpusumodificacion, DbType.String, entity.Cpacnpusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpacnpfecmodificacion, DbType.DateTime, entity.Cpacnpfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpaCnpCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpacnpcodi, DbType.Int32, cpaCnpCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaCentralPmpoDTO GetById(int cpaCnpCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpacnpcodi, DbType.Int32, cpaCnpCodi);
            CpaCentralPmpoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaCentralPmpoDTO> List(int cpacntcodi)
        {
            List<CpaCentralPmpoDTO> entities = new List<CpaCentralPmpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, cpacntcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        //CU04
        public List<CpaCentralPmpoDTO> ListCpaCentralPmpobyCentral(int id)
        {
            CpaCentralPmpoDTO entity = new CpaCentralPmpoDTO();
            List<CpaCentralPmpoDTO> entities = new List<CpaCentralPmpoDTO>();
            string query = string.Format(helper.SqlListCpaCentralPmpobyCentral, id);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaCentralPmpoDTO();

                    int iCpacnpcodi = dr.GetOrdinal(helper.Cpacnpcodi);
                    if (!dr.IsDBNull(iCpacnpcodi)) entity.Cpacnpcodi = dr.GetInt32(iCpacnpcodi);

                    int iCpacntcodi = dr.GetOrdinal(helper.Cpacntcodi);
                    if (!dr.IsDBNull(iCpacntcodi)) entity.Cpacntcodi = dr.GetInt32(iCpacntcodi);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iCpacnpusumodificacion = dr.GetOrdinal(helper.Cpacnpusumodificacion);
                    if (!dr.IsDBNull(iCpacnpusumodificacion)) entity.Cpacnpusumodificacion = dr.GetString(iCpacnpusumodificacion);

                    int iCpacnpfecmodificacion = dr.GetOrdinal(helper.Cpacnpfecmodificacion);
                    if (!dr.IsDBNull(iCpacnpfecmodificacion)) entity.Cpacnpfecmodificacion = dr.GetDateTime(iCpacnpfecmodificacion);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        //INICIO: CU011
        public List<CpaCentralPmpoDTO> GetByCentral(string cpacntcodi)
        {
            string query = string.Format(helper.SqlGetByCentral, cpacntcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaCentralPmpoDTO> entitys = new List<CpaCentralPmpoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaCentralPmpoDTO entity = helper.Create(dr);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpaCentralPmpoDTO> GetByRevision(int cparcodi)
        {
            string query = string.Format(helper.SqlGetByRevision, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaCentralPmpoDTO> entitys = new List<CpaCentralPmpoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaCentralPmpoDTO entity = helper.Create(dr);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //FIN: CU011
    }
}
