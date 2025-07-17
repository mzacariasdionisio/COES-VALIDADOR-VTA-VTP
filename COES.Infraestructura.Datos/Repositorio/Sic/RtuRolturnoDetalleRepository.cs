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
    /// Clase de acceso a datos de la tabla RTU_ROLTURNO_DETALLE
    /// </summary>
    public class RtuRolturnoDetalleRepository : RepositoryBase, IRtuRolturnoDetalleRepository
    {
        public RtuRolturnoDetalleRepository(string strConn) : base(strConn)
        {
        }

        RtuRolturnoDetalleHelper helper = new RtuRolturnoDetalleHelper();

        public int Save(RtuRolturnoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rtudetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rtudetnrodia, DbType.Int32, entity.Rtudetnrodia);
            dbProvider.AddInParameter(command, helper.Rtudetmodtrabajo, DbType.String, entity.Rtudetmodtrabajo);
            dbProvider.AddInParameter(command, helper.Rturolcodi, DbType.Int32, entity.Rturolcodi);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RtuRolturnoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rtudetnrodia, DbType.Int32, entity.Rtudetnrodia);
            dbProvider.AddInParameter(command, helper.Rtudetmodtrabajo, DbType.String, entity.Rtudetmodtrabajo);
            dbProvider.AddInParameter(command, helper.Rturolcodi, DbType.Int32, entity.Rturolcodi);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);
            dbProvider.AddInParameter(command, helper.Rtudetcodi, DbType.Int32, entity.Rtudetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rtudetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rtudetcodi, DbType.Int32, rtudetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RtuRolturnoDetalleDTO GetById(int rtudetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rtudetcodi, DbType.Int32, rtudetcodi);
            RtuRolturnoDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RtuRolturnoDetalleDTO> List()
        {
            List<RtuRolturnoDetalleDTO> entitys = new List<RtuRolturnoDetalleDTO>();
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

        public List<RtuRolturnoDetalleDTO> GetByCriteria()
        {
            List<RtuRolturnoDetalleDTO> entitys = new List<RtuRolturnoDetalleDTO>();
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
