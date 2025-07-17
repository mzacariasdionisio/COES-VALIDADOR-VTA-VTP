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
    /// Clase de acceso a datos de la tabla RI_ETAPAREVISION
    /// </summary>
    public class RiEtaparevisionRepository: RepositoryBase, IRiEtaparevisionRepository
    {
        public RiEtaparevisionRepository(string strConn): base(strConn)
        {
        }

        RiEtaparevisionHelper helper = new RiEtaparevisionHelper();

        public int Save(RiEtaparevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Etrvcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Etrvnombre, DbType.String, entity.Etrvnombre);
            dbProvider.AddInParameter(command, helper.Etrvestado, DbType.String, entity.Etrvestado);
            dbProvider.AddInParameter(command, helper.Etrvusucreacion, DbType.String, entity.Etrvusucreacion);
            dbProvider.AddInParameter(command, helper.Etrvfeccreacion, DbType.DateTime, entity.Etrvfeccreacion);
            dbProvider.AddInParameter(command, helper.Etrvusumodificacion, DbType.String, entity.Etrvusumodificacion);
            dbProvider.AddInParameter(command, helper.Etrvfecmodificacion, DbType.DateTime, entity.Etrvfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RiEtaparevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Etrvnombre, DbType.String, entity.Etrvnombre);
            dbProvider.AddInParameter(command, helper.Etrvestado, DbType.String, entity.Etrvestado);
            dbProvider.AddInParameter(command, helper.Etrvusucreacion, DbType.String, entity.Etrvusucreacion);
            dbProvider.AddInParameter(command, helper.Etrvfeccreacion, DbType.DateTime, entity.Etrvfeccreacion);
            dbProvider.AddInParameter(command, helper.Etrvusumodificacion, DbType.String, entity.Etrvusumodificacion);
            dbProvider.AddInParameter(command, helper.Etrvfecmodificacion, DbType.DateTime, entity.Etrvfecmodificacion);
            dbProvider.AddInParameter(command, helper.Etrvcodi, DbType.Int32, entity.Etrvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int etrvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Etrvcodi, DbType.Int32, etrvcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RiEtaparevisionDTO GetById(int etrvcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Etrvcodi, DbType.Int32, etrvcodi);
            RiEtaparevisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RiEtaparevisionDTO> List()
        {
            List<RiEtaparevisionDTO> entitys = new List<RiEtaparevisionDTO>();
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

        public List<RiEtaparevisionDTO> GetByCriteria()
        {
            List<RiEtaparevisionDTO> entitys = new List<RiEtaparevisionDTO>();
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
