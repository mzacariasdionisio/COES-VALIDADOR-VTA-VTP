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
    /// Clase de acceso a datos de la tabla RER_INSUMO
    /// </summary>
    public class RerInsumoRepository : RepositoryBase, IRerInsumoRepository
    {
        public RerInsumoRepository(string strConn)
            : base(strConn)
        {
        }

        RerInsumoHelper helper = new RerInsumoHelper();

        public int Save(RerInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi);
            dbProvider.AddInParameter(command, helper.Rerinstipinsumo, DbType.String, entity.Rerinstipinsumo);
            dbProvider.AddInParameter(command, helper.Rerinstipproceso, DbType.String, entity.Rerinstipproceso);
            dbProvider.AddInParameter(command, helper.Rerinslog, DbType.String, entity.Rerinslog);
            dbProvider.AddInParameter(command, helper.Rerinsusucreacion, DbType.String, entity.Rerinsusucreacion);
            dbProvider.AddInParameter(command, helper.Rerinsfeccreacion, DbType.DateTime, entity.Rerinsfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, entity.Rerinscodi);
            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi);
            dbProvider.AddInParameter(command, helper.Rerinstipinsumo, DbType.String, entity.Rerinstipinsumo);
            dbProvider.AddInParameter(command, helper.Rerinstipproceso, DbType.String, entity.Rerinstipproceso);
            dbProvider.AddInParameter(command, helper.Rerinslog, DbType.String, entity.Rerinslog);
            dbProvider.AddInParameter(command, helper.Rerinsusucreacion, DbType.String, entity.Rerinsusucreacion);
            dbProvider.AddInParameter(command, helper.Rerinsfeccreacion, DbType.DateTime, entity.Rerinsfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerInsCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, rerInsCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerInsumoDTO GetById(int rerInsCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, rerInsCodi);
            RerInsumoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public RerInsumoDTO GetByReravcodiByRerinstipinsumo(int Reravcodi, string Rerinstipinsumo)
        {
            string query = string.Format(helper.SqlGetByReravcodiByRerinstipinsumo, Reravcodi, Rerinstipinsumo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            RerInsumoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerInsumoDTO> List()
        {
            List<RerInsumoDTO> entities = new List<RerInsumoDTO>();
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



        public int GetIdPeriodoPmpoByAnioMes(int iPeriAnioMes)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetIdPeriodoPmpoByAnioMes);
            dbProvider.AddInParameter(command, helper.Perianiomes, DbType.Int32, iPeriAnioMes);

            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }



        public void TruncateTablaTemporal(string nombreTabla)
        {
            string query = string.Format(helper.SqlTruncateTablaTemporal, nombreTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveTablaTemporal(RerInsumoTemporalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInsertTablaTemporal);

            dbProvider.AddInParameter(command, helper.Rerfecinicio, DbType.DateTime, entity.Rerfecinicio);
            dbProvider.AddInParameter(command, helper.Reretapa, DbType.Int32, entity.Reretapa);
            dbProvider.AddInParameter(command, helper.Rerbloque, DbType.Int32, entity.Rerbloque);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, entity.Ptomedidesc);
            dbProvider.AddInParameter(command, helper.Rervalor, DbType.Decimal, entity.Rervalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void BulkInsertTablaTemporal(List<RerInsumoTemporalDTO> entitys, string nombreTabla)
        {
            dbProvider.AddColumnMapping(helper.Rerfecinicio, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Reretapa, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerbloque, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ptomedidesc, DbType.String);
            dbProvider.AddColumnMapping(helper.Rervalor, DbType.Decimal);

            dbProvider.BulkInsert<RerInsumoTemporalDTO>(entitys, nombreTabla);
        }

        public List<RerInsumoTemporalDTO> ListTablaTemporal()
        {
            List<RerInsumoTemporalDTO> entitys = new List<RerInsumoTemporalDTO>();

            string query = helper.SqlListTablaTemporal;
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerInsumoTemporalDTO entity = new RerInsumoTemporalDTO();

                    int iRerfecinicio = dr.GetOrdinal(helper.Rerfecinicio);
                    if (!dr.IsDBNull(iRerfecinicio)) entity.Rerfecinicio = dr.GetDateTime(iRerfecinicio);

                    int iReretapa = dr.GetOrdinal(helper.Reretapa);
                    if (!dr.IsDBNull(iReretapa)) entity.Reretapa = Convert.ToInt32(dr.GetValue(iReretapa));

                    int iRerbloque = dr.GetOrdinal(helper.Rerbloque);
                    if (!dr.IsDBNull(iRerbloque)) entity.Rerbloque = Convert.ToInt32(dr.GetValue(iRerbloque));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iRervalor = dr.GetOrdinal(helper.Rervalor);
                    if (!dr.IsDBNull(iRervalor)) entity.Rervalor = dr.GetDecimal(iRervalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}

