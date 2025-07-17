using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CM_ALERTAVALOR
    /// </summary>
    public class CmAlertavalorRepository: RepositoryBase, ICmAlertavalorRepository
    {
        public CmAlertavalorRepository(string strConn): base(strConn)
        {
        }

        CmAlertavalorHelper helper = new CmAlertavalorHelper();

        public int Save(CmAlertavalorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Alevalcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Alevalindicador, DbType.String, entity.Alevalindicador);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(string indicador, decimal maxCM, decimal maxCMCongestion, decimal maxCMSinCongestion, 
            decimal maxCICongestion, decimal maxCISinCongestion, DateTime fechaProceso)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Alevalindicador, DbType.String, indicador);
            dbProvider.AddInParameter(command, helper.Alevalmax, DbType.Decimal, maxCM);
            dbProvider.AddInParameter(command, helper.Alevalmaxconconge, DbType.Decimal, maxCMCongestion);
            dbProvider.AddInParameter(command, helper.Alevalmaxsinconge, DbType.Decimal, maxCMSinCongestion);
            dbProvider.AddInParameter(command, helper.Alevalciconconge, DbType.Decimal, maxCICongestion);
            dbProvider.AddInParameter(command, helper.Alevalcisinconge, DbType.Decimal, maxCISinCongestion);
            dbProvider.AddInParameter(command, helper.Alevalfechaproceso, DbType.DateTime, fechaProceso);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int alevalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Alevalcodi, DbType.Int32, alevalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmAlertavalorDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            
            CmAlertavalorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmAlertavalorDTO> List()
        {
            List<CmAlertavalorDTO> entitys = new List<CmAlertavalorDTO>();
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

        public List<CmAlertavalorDTO> GetByCriteria()
        {
            List<CmAlertavalorDTO> entitys = new List<CmAlertavalorDTO>();
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
    }
}
