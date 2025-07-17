using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_TOTAL_DEMANDADET
    /// </summary>
    public class CpaTotalDemandaDetRepository : RepositoryBase, ICpaTotalDemandaDetRepository
    {
        public CpaTotalDemandaDetRepository(string strConn) : base(strConn)
        {
        }

        CpaTotalDemandaDetHelper helper = new CpaTotalDemandaDetHelper();

        #region Métodos Tabla CPA_TOTAL_DEMANDADET
        public int Save(CpaTotalDemandaDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave); 

            dbProvider.AddInParameter(command, helper.Cpatddcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpatdcodi, DbType.Int32, entity.Cpatdcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpatddtotenemwh, DbType.Decimal, entity.Cpatddtotenemwh);
            dbProvider.AddInParameter(command, helper.Cpatddtotenesoles, DbType.Decimal, entity.Cpatddtotenesoles);
            dbProvider.AddInParameter(command, helper.Cpatddtotpotmw, DbType.Decimal, entity.Cpatddtotpotmw);
            dbProvider.AddInParameter(command, helper.Cpatddtotpotsoles, DbType.Decimal, entity.Cpatddtotpotsoles);
            dbProvider.AddInParameter(command, helper.Cpatddusucreacion, DbType.String, entity.Cpatddusucreacion);
            dbProvider.AddInParameter(command, helper.Cpatddfeccreacion, DbType.DateTime, entity.Cpatddfeccreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Delete(int cpaTddCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpatddcodi, DbType.Int32, cpaTddCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaTotalDemandaDetDTO> List()
        {
            List<CpaTotalDemandaDetDTO> entities = new List<CpaTotalDemandaDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public CpaTotalDemandaDetDTO GetById(int cpaTddCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpatddcodi, DbType.Int32, cpaTddCodi);
            CpaTotalDemandaDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        #endregion

        #region Métodos Adicionales
        public List<CpaTotalDemandaDetDTO> GetByIdDemanda(int cpaTdCodi)
        {
            List<CpaTotalDemandaDetDTO> entitys = new List<CpaTotalDemandaDetDTO>();

            string query = string.Format(helper.SqlGetByIdDemanda, cpaTdCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CpaTotalDemandaDetDTO> GetLastEnvio(int IdRevision, string Tipo, int Mes)
        {
            List<CpaTotalDemandaDetDTO> entities = new List<CpaTotalDemandaDetDTO>();

            string query = string.Format(helper.SqlGetLastEnvio, IdRevision, Tipo, Mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<CpaTotalDemandaDetDTO> EnvioVacio(int revision, string tipo, int mes)
        {
            List<CpaTotalDemandaDetDTO> entitys = new List<CpaTotalDemandaDetDTO>();

            int parRevision = (revision == 0 ? 0 : revision);
            string parTipo = (tipo == "" ? "" : tipo);
            int parMes = (mes == 0 ? 0 : mes);

            string query = string.Format(helper.SqlGetEnvioVacio, parRevision, parTipo, parMes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /* CU17: INICIO */
        public List<CpaTotalDemandaDetDTO> ListLastByRevision(int cparcodi)
        {
            List<CpaTotalDemandaDetDTO> entitys = new List<CpaTotalDemandaDetDTO>();

            string query = string.Format(helper.SqlListLastByRevision, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaTotalDemandaDetDTO entity = helper.Create(dr);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = Convert.ToInt32(dr.GetValue(iCparcodi));

                    int iCpatdtipo = dr.GetOrdinal(helper.Cpatdtipo);
                    if (!dr.IsDBNull(iCpatdtipo)) entity.Cpatdtipo = dr.GetString(iCpatdtipo); 

                    int iCpatdmes = dr.GetOrdinal(helper.Cpatdmes);
                    if (!dr.IsDBNull(iCpatdmes)) entity.Cpatdmes = Convert.ToInt32(dr.GetValue(iCpatdmes));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpaTotalDemandaDetDTO> ListByCpatdcodi(int cpatdcodi)
        {
            string query = string.Format(helper.SqlListByCpatdcodi, cpatdcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaTotalDemandaDetDTO> entitys = new List<CpaTotalDemandaDetDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaTotalDemandaDetDTO entity = helper.Create(dr);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = Convert.ToInt32(dr.GetValue(iCparcodi));

                    int iCpatdtipo = dr.GetOrdinal(helper.Cpatdtipo);
                    if (!dr.IsDBNull(iCpatdtipo)) entity.Cpatdtipo = dr.GetString(iCpatdtipo);

                    int iCpatdmes = dr.GetOrdinal(helper.Cpatdmes);
                    if (!dr.IsDBNull(iCpatdmes)) entity.Cpatdmes = Convert.ToInt32(dr.GetValue(iCpatdmes));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        /* CU17: FIN */
        #endregion
    }
}
