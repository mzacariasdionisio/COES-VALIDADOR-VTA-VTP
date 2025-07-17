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
    /// Clase de acceso a datos de la tabla CPA_INSUMO
    /// </summary>
    public class CpaInsumoRepository : RepositoryBase, ICpaInsumoRepository
    {
        public CpaInsumoRepository(string strConn)
            : base(strConn)
        {
        }

        CpaInsumoHelper helper = new CpaInsumoHelper();

        public int Save(CpaInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpainscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpainstipinsumo, DbType.String, entity.Cpainstipinsumo);
            dbProvider.AddInParameter(command, helper.Cpainstipproceso, DbType.String, entity.Cpainstipproceso);
            dbProvider.AddInParameter(command, helper.Cpainslog, DbType.String, entity.Cpainslog);
            dbProvider.AddInParameter(command, helper.Cpainsusucreacion, DbType.String, entity.Cpainsusucreacion);
            dbProvider.AddInParameter(command, helper.Cpainsfeccreacion, DbType.DateTime, entity.Cpainsfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaInsumoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpainscodi, DbType.Int32, entity.Cpainscodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpainstipinsumo, DbType.String, entity.Cpainstipinsumo);
            dbProvider.AddInParameter(command, helper.Cpainstipproceso, DbType.String, entity.Cpainstipproceso);
            dbProvider.AddInParameter(command, helper.Cpainslog, DbType.String, entity.Cpainslog);
            dbProvider.AddInParameter(command, helper.Cpainsusucreacion, DbType.String, entity.Cpainsusucreacion);
            dbProvider.AddInParameter(command, helper.Cpainsfeccreacion, DbType.DateTime, entity.Cpainsfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpainscodi, DbType.Int32, entity.Cpainscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpainscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpainscodi, DbType.Int32, cpainscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaInsumoDTO GetById(int cpainscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpainscodi, DbType.Int32, cpainscodi);
            CpaInsumoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaInsumoDTO> List()
        {
            List<CpaInsumoDTO> entities = new List<CpaInsumoDTO>();
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

        public CpaInsumoDTO GetByCparcodiByCpainstipinsumo(int Cparcodi, string Cpainstipinsumo)
        {
            string query = string.Format(helper.SqlGetByCparcodiByCpainstipinsumo, Cparcodi, Cpainstipinsumo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            CpaInsumoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaInsumoDTO> GetAllByCparcodiByCpainstipinsumo(int Cparcodi, string Cpainstipinsumo)
        {
            List<CpaInsumoDTO> entities = new List<CpaInsumoDTO>();
            string query = string.Format(helper.SqlGetByCparcodiByCpainstipinsumo, Cparcodi, Cpainstipinsumo);

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

    }
}

