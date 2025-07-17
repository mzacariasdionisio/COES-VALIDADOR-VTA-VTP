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
    /// Clase de acceso a datos de la tabla RTU_CONFIGURACION_PERSONA
    /// </summary>
    public class RtuConfiguracionPersonaRepository : RepositoryBase, IRtuConfiguracionPersonaRepository
    {
        public RtuConfiguracionPersonaRepository(string strConn) : base(strConn)
        {
        }

        RtuConfiguracionPersonaHelper helper = new RtuConfiguracionPersonaHelper();

        public int Save(RtuConfiguracionPersonaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rtugrucodi, DbType.Int32, entity.Rtugrucodi);
            dbProvider.AddInParameter(command, helper.Rtupercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rtuperorden, DbType.Int32, entity.Rtuperorden);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RtuConfiguracionPersonaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rtupercodi, DbType.Int32, entity.Rtupercodi);
            dbProvider.AddInParameter(command, helper.Rtuperorden, DbType.Int32, entity.Rtuperorden);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);
            dbProvider.AddInParameter(command, helper.Rtugrucodi, DbType.Int32, entity.Rtugrucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rtupercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rtupercodi, DbType.Int32, rtupercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RtuConfiguracionPersonaDTO GetById(int rtupercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rtupercodi, DbType.Int32, rtupercodi);
            RtuConfiguracionPersonaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RtuConfiguracionPersonaDTO> List()
        {
            List<RtuConfiguracionPersonaDTO> entitys = new List<RtuConfiguracionPersonaDTO>();
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

        public List<RtuConfiguracionPersonaDTO> GetByCriteria()
        {
            List<RtuConfiguracionPersonaDTO> entitys = new List<RtuConfiguracionPersonaDTO>();
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
