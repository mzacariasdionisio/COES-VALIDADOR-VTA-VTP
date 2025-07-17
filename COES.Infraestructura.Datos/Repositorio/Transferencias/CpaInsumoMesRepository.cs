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
    /// Clase de acceso a datos de la tabla CPA_INSUMO_MES
    /// </summary>
    public class CpaInsumoMesRepository : RepositoryBase, ICpaInsumoMesRepository
    {
        public CpaInsumoMesRepository(string strConn)
            : base(strConn)
        {
        }

        CpaInsumoMesHelper helper = new CpaInsumoMesHelper();

        public int Save(CpaInsumoMesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpainscodi, DbType.Int32, entity.Cpainscodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cpainmtipinsumo, DbType.String, entity.Cpainmtipinsumo);
            dbProvider.AddInParameter(command, helper.Cpainmtipproceso, DbType.String, entity.Cpainmtipproceso);
            dbProvider.AddInParameter(command, helper.Cpainmmes, DbType.Int32, entity.Cpainmmes);
            dbProvider.AddInParameter(command, helper.Cpainmtotal, DbType.Decimal, entity.Cpainmtotal);
            dbProvider.AddInParameter(command, helper.Cpainmusucreacion, DbType.String, entity.Cpainmusucreacion);
            dbProvider.AddInParameter(command, helper.Cpainmfeccreacion, DbType.DateTime, entity.Cpainmfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaInsumoMesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, entity.Cpainmcodi);
            dbProvider.AddInParameter(command, helper.Cpainscodi, DbType.Int32, entity.Cpainscodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cpainmtipinsumo, DbType.String, entity.Cpainmtipinsumo);
            dbProvider.AddInParameter(command, helper.Cpainmtipproceso, DbType.String, entity.Cpainmtipproceso);
            dbProvider.AddInParameter(command, helper.Cpainmmes, DbType.Int32, entity.Cpainmmes);
            dbProvider.AddInParameter(command, helper.Cpainmtotal, DbType.Decimal, entity.Cpainmtotal);
            dbProvider.AddInParameter(command, helper.Cpainmusucreacion, DbType.String, entity.Cpainmusucreacion);
            dbProvider.AddInParameter(command, helper.Cpainmfeccreacion, DbType.DateTime, entity.Cpainmfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, entity.Cpainmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpainmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaInsumoMesDTO GetById(int cpainmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);
            CpaInsumoMesDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaInsumoMesDTO> List()
        {
            List<CpaInsumoMesDTO> entities = new List<CpaInsumoMesDTO>();
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
        public CpaInsumoMesDTO GetByCriteria(int cparcodi, int emprcodi, int equicodi, string cpainmtipinsumo, int cpainmmes)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Cpainmtipinsumo, DbType.Int32, cpainmtipinsumo);
            dbProvider.AddInParameter(command, helper.Cpainmmes, DbType.Int32, cpainmmes);

            CpaInsumoMesDTO entity = new CpaInsumoMesDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        public void DeleteByCentral(int cparcodi, int equicodi, string cpainmtipinsumo, int cpainmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCentral);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Cpainmtipinsumo, DbType.Int32, cpainmtipinsumo);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, cpainmcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByRevision(int cparcodi, string cpainmtipinsumo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByRevision);

            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);
            dbProvider.AddInParameter(command, helper.Cpainmtipinsumo, DbType.Int32, cpainmtipinsumo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateInsumoMesTotal(int Cpainmcodi, int Equicodi, string Cpainmtipinsumo, DateTime dFecInicio, DateTime dFecFin)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateInsumoMesTotal);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, Equicodi);
            dbProvider.AddInParameter(command, helper.Cpainmtipinsumo, DbType.Int32, Cpainmtipinsumo);
            dbProvider.AddInParameter(command, helper.Cpainmfeccreacion, DbType.DateTime, dFecInicio);
            dbProvider.AddInParameter(command, helper.Cpainmfeccreacion, DbType.DateTime, dFecFin);
            dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, Cpainmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        //public void UpdateByInsumoMes(int Cpainmcodi, decimal Cpainmtotal, DateTime sFechaModificacion, string sUserModificacion)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateInsumoMesTotal);

        //    dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, Cpainmcodi);
        //    dbProvider.AddInParameter(command, helper.Cpainmtotal, DbType.Decimal, Cpainmtotal);
        //    dbProvider.AddInParameter(command, helper.Cpainmfeccreacion, DbType.DateTime, dFecInicio);
        //    dbProvider.AddInParameter(command, helper.Cpainmfeccreacion, DbType.DateTime, dFecFin);
        //    dbProvider.AddInParameter(command, helper.Cpainmcodi, DbType.Int32, Cpainmcodi);

        //    dbProvider.ExecuteNonQuery(command);
        //}
    }
}
