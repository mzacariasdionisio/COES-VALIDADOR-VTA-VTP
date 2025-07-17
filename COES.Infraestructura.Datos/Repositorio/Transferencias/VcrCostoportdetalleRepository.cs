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
    /// Clase de acceso a datos de la tabla VCR_COSTOPORTDETALLE
    /// </summary>
    public class VcrCostoportdetalleRepository: RepositoryBase, IVcrCostoportdetalleRepository
    {
        public VcrCostoportdetalleRepository(string strConn): base(strConn)
        {
        }

        VcrCostoportdetalleHelper helper = new VcrCostoportdetalleHelper();

        public int Save(VcrCostoportdetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrcodcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrcodfecha, DbType.DateTime, entity.Vcrcodfecha);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Vcrcodinterv, DbType.Int32, entity.Vcrcodinterv);
            dbProvider.AddInParameter(command, helper.Vcrcodpdo, DbType.Decimal, entity.Vcrcodpdo);
            dbProvider.AddInParameter(command, helper.Vcrcodcmgcp, DbType.Decimal, entity.Vcrcodcmgcp);
            dbProvider.AddInParameter(command, helper.Vcrcodcv, DbType.Decimal, entity.Vcrcodcv);
            dbProvider.AddInParameter(command, helper.Vcrcodcostoportun, DbType.Decimal, entity.Vcrcodcostoportun);
            dbProvider.AddInParameter(command, helper.Vcrcodusucreacion, DbType.String, entity.Vcrcodusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrcodfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrCostoportdetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrcodfecha, DbType.DateTime, entity.Vcrcodfecha);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Vcrcodinterv, DbType.Int32, entity.Vcrcodinterv);
            dbProvider.AddInParameter(command, helper.Vcrcodpdo, DbType.Decimal, entity.Vcrcodpdo);
            dbProvider.AddInParameter(command, helper.Vcrcodcmgcp, DbType.Decimal, entity.Vcrcodcmgcp);
            dbProvider.AddInParameter(command, helper.Vcrcodcv, DbType.Decimal, entity.Vcrcodcv);
            dbProvider.AddInParameter(command, helper.Vcrcodcostoportun, DbType.Decimal, entity.Vcrcodcostoportun);
            dbProvider.AddInParameter(command, helper.Vcrcodusucreacion, DbType.String, entity.Vcrcodusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrcodfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrcodcodi, DbType.Int32, entity.Vcrcodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrCostoportdetalleDTO GetById(int vcrcopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrcodcodi, DbType.Int32, vcrcopcodi);
            VcrCostoportdetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrCostoportdetalleDTO> List()
        {
            List<VcrCostoportdetalleDTO> entitys = new List<VcrCostoportdetalleDTO>();
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

        public List<VcrCostoportdetalleDTO> GetByCriteria()
        {
            List<VcrCostoportdetalleDTO> entitys = new List<VcrCostoportdetalleDTO>();
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

        public List<VcrCostoportdetalleDTO> ListPorMesURS(int vcrecacodi, int grupocodi, int equicodi)
        {
            List<VcrCostoportdetalleDTO> entitys = new List<VcrCostoportdetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPorMesURS);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            
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
