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
    /// Clase de acceso a datos de la tabla RER_INSUMO_VTP
    /// </summary>
    public class RerInsumoVtpRepository : RepositoryBase, IRerInsumoVtpRepository
    {
        public RerInsumoVtpRepository(string strConn)
            : base(strConn)
        {
        }

        RerInsumoVtpHelper helper = new RerInsumoVtpHelper();

        public int Save(RerInsumoVtpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerinpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, entity.Rerinscodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerinpanio, DbType.Int32, entity.Rerinpanio);
            dbProvider.AddInParameter(command, helper.Rerinpmes, DbType.Int32, entity.Rerinpmes);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Rerinpmestotal, DbType.Decimal, entity.Rerinpmestotal);
            dbProvider.AddInParameter(command, helper.Rerinpmesusucreacion, DbType.String, entity.Rerinpmesusucreacion);
            dbProvider.AddInParameter(command, helper.Rerinpmesfeccreacion, DbType.DateTime, entity.Rerinpmesfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerInsumoVtpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, entity.Rerinscodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerinpanio, DbType.Int32, entity.Rerinpanio);
            dbProvider.AddInParameter(command, helper.Rerinpmes, DbType.Int32, entity.Rerinpmes);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Rerinpmestotal, DbType.Decimal, entity.Rerinpmestotal);
            dbProvider.AddInParameter(command, helper.Rerinpmesusucreacion, DbType.String, entity.Rerinpmesusucreacion);
            dbProvider.AddInParameter(command, helper.Rerinpmesfeccreacion, DbType.DateTime, entity.Rerinpmesfeccreacion);
            dbProvider.AddInParameter(command, helper.Rerinpcodi, DbType.Int32, entity.Rerinpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerInpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerinpcodi, DbType.Int32, rerInpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerInsumoVtpDTO GetById(int rerInpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerinpcodi, DbType.Int32, rerInpcodi);
            RerInsumoVtpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerInsumoVtpDTO> List()
        {
            List<RerInsumoVtpDTO> entities = new List<RerInsumoVtpDTO>();
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

        public void DeleteByParametroPrimaAndMes(int iRerpprcodi, int iRerinpmes)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByParametroPrimaAndMes);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iRerpprcodi);
            dbProvider.AddInParameter(command, helper.Rerinpmes, DbType.Int32, iRerinpmes);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<RerInsumoVtpDTO> GetByPeriodo(int reravcodi, string rerpprmes)
        {
            string query = string.Format(helper.SqlGetByPeriodo, reravcodi, rerpprmes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerInsumoVtpDTO> entitys = new List<RerInsumoVtpDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateByPeriodo(dr));
                }
            }

            return entitys;
        }

        public decimal ObtenerSaldoVtpByInsumoVTP(int iRerpprcodi, int iEmprcodi, int iEquicodi)
        {
            decimal dResultado = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerSaldoVtpByInsumoVTP);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iRerpprcodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iEmprcodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iEquicodi);
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) dResultado = Convert.ToDecimal(result);

            return dResultado;
        }
    }
}

