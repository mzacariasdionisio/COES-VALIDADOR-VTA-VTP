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
    /// Clase de acceso a datos de la tabla CAI_AJUSTECMARGINAL
    /// </summary>
    public class CaiAjustecmarginalRepository: RepositoryBase, ICaiAjustecmarginalRepository
    {
        public CaiAjustecmarginalRepository(string strConn): base(strConn)
        {
        }

        CaiAjustecmarginalHelper helper = new CaiAjustecmarginalHelper();

        public int Save(CaiAjustecmarginalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caajcmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Caajcmmes, DbType.Int32, entity.Caajcmmes);
            dbProvider.AddInParameter(command, helper.Caajcmusucreacion, DbType.String, entity.Caajcmusucreacion);
            dbProvider.AddInParameter(command, helper.Caajcmfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiAjustecmarginalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Caajcmmes, DbType.Int32, entity.Caajcmmes);
            dbProvider.AddInParameter(command, helper.Caajcmusucreacion, DbType.String, entity.Caajcmusucreacion);
            dbProvider.AddInParameter(command, helper.Caajcmfeccreacion, DbType.DateTime, entity.Caajcmfeccreacion);
            dbProvider.AddInParameter(command, helper.Caajcmcodi, DbType.Int32, entity.Caajcmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int caiajcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiAjustecmarginalDTO GetById(int caajcmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Caajcmcodi, DbType.Int32, caajcmcodi);
            CaiAjustecmarginalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiAjustecmarginalDTO> List(int caiajcodi)
        {
            List<CaiAjustecmarginalDTO> entitys = new List<CaiAjustecmarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CaiAjustecmarginalDTO> GetByCriteria()
        {
            List<CaiAjustecmarginalDTO> entitys = new List<CaiAjustecmarginalDTO>();
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
