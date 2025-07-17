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
    /// Clase de acceso a datos de la tabla RER_INSUMO_MES
    /// </summary>
    public class RerInsumoMesRepository : RepositoryBase, IRerInsumoMesRepository
    {
        public RerInsumoMesRepository(string strConn)
            : base(strConn)
        {
        }

        RerInsumoMesHelper helper = new RerInsumoMesHelper();

        public int Save(RerInsumoMesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerinmmescodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, entity.Rerinscodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerinmanio, DbType.Int32, entity.Rerinmanio);
            dbProvider.AddInParameter(command, helper.Rerinmmes, DbType.Int32, entity.Rerinmmes);
            dbProvider.AddInParameter(command, helper.Rerinmtipresultado, DbType.String, entity.Rerinmtipresultado);
            dbProvider.AddInParameter(command, helper.Rerinmtipinformacion, DbType.String, entity.Rerinmtipinformacion);
            dbProvider.AddInParameter(command, helper.Rerinmdetalle, DbType.String, entity.Rerinmdetalle);
            dbProvider.AddInParameter(command, helper.Rerinmmestotal, DbType.Decimal, entity.Rerinmmestotal);
            dbProvider.AddInParameter(command, helper.Rerinmmesusucreacion, DbType.String, entity.Rerinmmesusucreacion);
            dbProvider.AddInParameter(command, helper.Rerinmmesfeccreacion, DbType.DateTime, entity.Rerinmmesfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerInsumoMesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerinmmescodi, DbType.Int32, entity.Rerinmmescodi);
            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, entity.Rerinscodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerinmanio, DbType.Int32, entity.Rerinmanio);
            dbProvider.AddInParameter(command, helper.Rerinmmes, DbType.Int32, entity.Rerinmmes);
            dbProvider.AddInParameter(command, helper.Rerinmtipresultado, DbType.String, entity.Rerinmtipresultado);
            dbProvider.AddInParameter(command, helper.Rerinmtipinformacion, DbType.String, entity.Rerinmtipinformacion);
            dbProvider.AddInParameter(command, helper.Rerinmdetalle, DbType.String, entity.Rerinmdetalle);
            dbProvider.AddInParameter(command, helper.Rerinmmestotal, DbType.Decimal, entity.Rerinmmestotal);
            dbProvider.AddInParameter(command, helper.Rerinmmesusucreacion, DbType.String, entity.Rerinmmesusucreacion);
            dbProvider.AddInParameter(command, helper.Rerinmmesfeccreacion, DbType.DateTime, entity.Rerinmmesfeccreacion);
            dbProvider.AddInParameter(command, helper.Rerinmmescodi, DbType.Int32, entity.Rerinmmescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Rerinmmescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerinmmescodi, DbType.Int32, Rerinmmescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerInsumoMesDTO GetById(int Rerinmmescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerinmmescodi, DbType.Int32, Rerinmmescodi);
            RerInsumoMesDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerInsumoMesDTO> GetByAnioTarifario(int reravcodi, string rerinmtipresultado)
        {
            string query = string.Format(helper.SqlGetByAnioTarifario, reravcodi, rerinmtipresultado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerInsumoMesDTO> entities = new List<RerInsumoMesDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }

            }

            return entities;
        }

        public List<RerInsumoMesDTO> List()
        {
            List<RerInsumoMesDTO> entities = new List<RerInsumoMesDTO>();
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

        #region CU21
        public void DeleteByParametroPrimaAndTipo(int iRerpprcodi, int iRerpprmes, string sRerindtipresultado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByParametroPrimaAndTipo);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iRerpprcodi);
            dbProvider.AddInParameter(command, helper.Rerinmmes, DbType.Int32, iRerpprmes);
            dbProvider.AddInParameter(command, helper.Rerinmtipresultado, DbType.String, sRerindtipresultado);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<RerInsumoMesDTO> GetByTipoResultadoByPeriodo(string rerinmtipresultado, int reravcodi, string rerpprmes)
        {
            string query = string.Format(helper.SqlGetByTipoResultadoByPeriodo, rerinmtipresultado, reravcodi, rerpprmes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerInsumoMesDTO> entitys = new List<RerInsumoMesDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateByTipoResultadoByPeriodo(dr));
                }
            }

            return entitys;
        }
        #endregion
    }
}

