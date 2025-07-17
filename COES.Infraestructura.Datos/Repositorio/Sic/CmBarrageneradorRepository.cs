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
    /// Clase de acceso a datos de la tabla CM_BARRAGENERADOR
    /// </summary>
    public class CmBarrageneradorRepository: RepositoryBase, ICmBarrageneradorRepository
    {
        public CmBarrageneradorRepository(string strConn): base(strConn)
        {
        }

        CmBarrageneradorHelper helper = new CmBarrageneradorHelper();

        public int Save(CmBarrageneradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Bargercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Bargerfecha, DbType.DateTime, entity.Bargerfecha);
            dbProvider.AddInParameter(command, helper.Bargerusucreacion, DbType.String, entity.Bargerusucreacion);
            dbProvider.AddInParameter(command, helper.Bargerfeccreacion, DbType.DateTime, entity.Bargerfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmBarrageneradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Bargerfecha, DbType.DateTime, entity.Bargerfecha);
            dbProvider.AddInParameter(command, helper.Bargerusucreacion, DbType.String, entity.Bargerusucreacion);
            dbProvider.AddInParameter(command, helper.Bargerfeccreacion, DbType.DateTime, entity.Bargerfeccreacion);
            dbProvider.AddInParameter(command, helper.Bargercodi, DbType.Int32, entity.Bargercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int bargercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Bargercodi, DbType.Int32, bargercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmBarrageneradorDTO GetById(int bargercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Bargercodi, DbType.Int32, bargercodi);
            CmBarrageneradorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmBarrageneradorDTO> List()
        {
            List<CmBarrageneradorDTO> entitys = new List<CmBarrageneradorDTO>();
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

        public List<CmBarrageneradorDTO> GetByCriteria()
        {
            List<CmBarrageneradorDTO> entitys = new List<CmBarrageneradorDTO>();
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
