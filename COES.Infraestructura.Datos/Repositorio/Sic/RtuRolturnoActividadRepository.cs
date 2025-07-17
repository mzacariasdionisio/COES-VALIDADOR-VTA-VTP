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
    /// Clase de acceso a datos de la tabla RTU_ROLTURNO_ACTIVIDAD
    /// </summary>
    public class RtuRolturnoActividadRepository : RepositoryBase, IRtuRolturnoActividadRepository
    {
        public RtuRolturnoActividadRepository(string strConn) : base(strConn)
        {
        }

        RtuRolturnoActividadHelper helper = new RtuRolturnoActividadHelper();

        public int Save(RtuRolturnoActividadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rturaccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rtudetcodi, DbType.Int32, entity.Rtudetcodi);
            dbProvider.AddInParameter(command, helper.Rtuactcodi, DbType.Int32, entity.Rtuactcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RtuRolturnoActividadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rtuactcodi, DbType.Int32, entity.Rtuactcodi);
            dbProvider.AddInParameter(command, helper.Rturaccodi, DbType.Int32, entity.Rturaccodi);
            dbProvider.AddInParameter(command, helper.Rtudetcodi, DbType.Int32, entity.Rtudetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rturaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rturaccodi, DbType.Int32, rturaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RtuRolturnoActividadDTO GetById(int rturaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rturaccodi, DbType.Int32, rturaccodi);
            RtuRolturnoActividadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RtuRolturnoActividadDTO> List()
        {
            List<RtuRolturnoActividadDTO> entitys = new List<RtuRolturnoActividadDTO>();
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

        public List<RtuRolturnoActividadDTO> GetByCriteria()
        {
            List<RtuRolturnoActividadDTO> entitys = new List<RtuRolturnoActividadDTO>();
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
