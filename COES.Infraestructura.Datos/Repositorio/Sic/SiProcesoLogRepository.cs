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
    /// Clase de acceso a datos de la tabla SI_PROCESO_LOG
    /// </summary>
    public class SiProcesoLogRepository : RepositoryBase, ISiProcesoLogRepository
    {
        public SiProcesoLogRepository(string strConn) : base(strConn)
        {
        }

        SiProcesoLogHelper helper = new SiProcesoLogHelper();

        public int Save(SiProcesoLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prcslgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prcscodi, DbType.Int32, entity.Prcscodi);
            dbProvider.AddInParameter(command, helper.Prcslgfecha, DbType.DateTime, entity.Prcslgfecha);
            dbProvider.AddInParameter(command, helper.Prcslginicio, DbType.DateTime, entity.Prcslginicio);
            dbProvider.AddInParameter(command, helper.Prcslgfin, DbType.DateTime, entity.Prcslgfin);
            dbProvider.AddInParameter(command, helper.Prcslgestado, DbType.String, entity.Prcslgestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiProcesoLogDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Prcscodi, DbType.Int32, entity.Prcscodi);
            dbProvider.AddInParameter(command, helper.Prcslgfecha, DbType.DateTime, entity.Prcslgfecha);
            dbProvider.AddInParameter(command, helper.Prcslginicio, DbType.DateTime, entity.Prcslginicio);
            dbProvider.AddInParameter(command, helper.Prcslgfin, DbType.DateTime, entity.Prcslgfin);
            dbProvider.AddInParameter(command, helper.Prcslgestado, DbType.String, entity.Prcslgestado);
            dbProvider.AddInParameter(command, helper.Prcslgcodi, DbType.Int32, entity.Prcslgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int prcslgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Prcslgcodi, DbType.Int32, prcslgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiProcesoLogDTO GetById(int prcslgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Prcslgcodi, DbType.Int32, prcslgcodi);
            SiProcesoLogDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiProcesoLogDTO> List()
        {
            List<SiProcesoLogDTO> entitys = new List<SiProcesoLogDTO>();
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

        public List<SiProcesoLogDTO> GetByCriteria()
        {
            List<SiProcesoLogDTO> entitys = new List<SiProcesoLogDTO>();
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
