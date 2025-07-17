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
    /// Clase de acceso a datos de la tabla CPA_TOTAL_TRANSMISORESDET
    /// </summary>
    public class CpaTotalTransmisoresDetRepository : RepositoryBase, ICpaTotalTransmisoresDetRepository
    {
        public CpaTotalTransmisoresDetRepository(string strConn) : base(strConn)
        {
        }

        CpaTotalTransmisoresDetHelper helper = new CpaTotalTransmisoresDetHelper();

        #region Métodos Tabla CPA_TOTAL_TRANSMISORESDET
        public int Save(CpaTotalTransmisoresDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpattdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpattcodi, DbType.Int32, entity.Cpattcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes01, DbType.Decimal, entity.Cpattdtotmes01);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes02, DbType.Decimal, entity.Cpattdtotmes02);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes03, DbType.Decimal, entity.Cpattdtotmes03);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes04, DbType.Decimal, entity.Cpattdtotmes04);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes05, DbType.Decimal, entity.Cpattdtotmes05);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes06, DbType.Decimal, entity.Cpattdtotmes06);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes07, DbType.Decimal, entity.Cpattdtotmes07);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes08, DbType.Decimal, entity.Cpattdtotmes08);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes09, DbType.Decimal, entity.Cpattdtotmes09);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes10, DbType.Decimal, entity.Cpattdtotmes10);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes11, DbType.Decimal, entity.Cpattdtotmes11);
            dbProvider.AddInParameter(command, helper.Cpattdtotmes12, DbType.Decimal, entity.Cpattdtotmes12);
            dbProvider.AddInParameter(command, helper.Cpattdusucreacion, DbType.String, entity.Cpattdusucreacion);
            dbProvider.AddInParameter(command, helper.Cpattdfeccreacion, DbType.DateTime, entity.Cpattdfeccreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Delete(int cpaTtdCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpattdcodi, DbType.Int32, cpaTtdCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaTotalTransmisoresDetDTO> List()
        {
            List<CpaTotalTransmisoresDetDTO> entities = new List<CpaTotalTransmisoresDetDTO>();
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

        public CpaTotalTransmisoresDetDTO GetById(int cpaTtdCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpattdcodi, DbType.Int32, cpaTtdCodi);
            CpaTotalTransmisoresDetDTO entity = null;

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
        public List<CpaTotalTransmisoresDetDTO> GetByIdTransmisores(int cpatdcodi)
        {
            List<CpaTotalTransmisoresDetDTO> entitys = new List<CpaTotalTransmisoresDetDTO>();

            string query = string.Format(helper.SqlGetByIdTransmisores, cpatdcodi);
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

        public List<CpaTotalTransmisoresDetDTO> GetLastEnvio(int IdRevision)
        {
            List<CpaTotalTransmisoresDetDTO> entities = new List<CpaTotalTransmisoresDetDTO>();

            string query = string.Format(helper.SqlGetLastEnvio, IdRevision);
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
        
        public List<CpaTotalTransmisoresDetDTO> EnvioVacio(int IdRevision)
        {
            List<CpaTotalTransmisoresDetDTO> entitys = new List<CpaTotalTransmisoresDetDTO>();

            int parRevision = (IdRevision == 0 ? 0 : IdRevision);

            string query = string.Format(helper.SqlGetEnvioVacio, parRevision);
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
        public List<CpaTotalTransmisoresDetDTO> ListLastByRevision(int cparcodi)
        {
            string query = string.Format(helper.SqlListLastByRevision, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaTotalTransmisoresDetDTO> entitys = new List<CpaTotalTransmisoresDetDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaTotalTransmisoresDetDTO entity = helper.Create(dr);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = Convert.ToInt32(dr.GetValue(iCparcodi));

                    int iCpattdtotal = dr.GetOrdinal(helper.Cpattdtotal);
                    if (!dr.IsDBNull(iCpattdtotal)) entity.Cpattdtotal = dr.GetDecimal(iCpattdtotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpaTotalTransmisoresDetDTO> ListByCpattcodi(int cpattcodi)
        {
            string query = string.Format(helper.SqlListByCpattcodi, cpattcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaTotalTransmisoresDetDTO> entitys = new List<CpaTotalTransmisoresDetDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaTotalTransmisoresDetDTO entity = helper.Create(dr);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = Convert.ToInt32(dr.GetValue(iCparcodi));

                    int iCpattdtotal = dr.GetOrdinal(helper.Cpattdtotal);
                    if (!dr.IsDBNull(iCpattdtotal)) entity.Cpattdtotal = dr.GetDecimal(iCpattdtotal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        /* CU17:FIN */
        #endregion
    }
}
