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
    /// Clase de acceso a datos de la tabla RER_SDDP
    /// </summary>
    public class RerSddpRepository : RepositoryBase, IRerSddpRepository
    {
        public RerSddpRepository(string strConn) : base(strConn)
        {
        }

        readonly RerSddpHelper helper = new RerSddpHelper();

        public int Save(RerSddpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Resddpcodi, DbType.Int32, id); 
            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi);
            dbProvider.AddInParameter(command, helper.Resddpnomarchivo, DbType.String, entity.Resddpnomarchivo);
            dbProvider.AddInParameter(command, helper.Resddpsemanaini, DbType.Int32, entity.Resddpsemanaini);
            dbProvider.AddInParameter(command, helper.Resddpanioini, DbType.Int32, entity.Resddpanioini);
            dbProvider.AddInParameter(command, helper.Resddpnroseries, DbType.Int32, entity.Resddpnroseries);
            dbProvider.AddInParameter(command, helper.Resddpdiainicio, DbType.DateTime, entity.Resddpdiainicio);
            dbProvider.AddInParameter(command, helper.Resddpusucreacion, DbType.String, entity.Resddpusucreacion);
            dbProvider.AddInParameter(command, helper.Resddpfeccreacion, DbType.DateTime, entity.Resddpfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerSddpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi);
            dbProvider.AddInParameter(command, helper.Resddpnomarchivo, DbType.String, entity.Resddpnomarchivo);
            dbProvider.AddInParameter(command, helper.Resddpsemanaini, DbType.Int32, entity.Resddpsemanaini);
            dbProvider.AddInParameter(command, helper.Resddpanioini, DbType.Int32, entity.Resddpanioini);
            dbProvider.AddInParameter(command, helper.Resddpnroseries, DbType.Int32, entity.Resddpnroseries);
            dbProvider.AddInParameter(command, helper.Resddpdiainicio, DbType.DateTime, entity.Resddpdiainicio);
            dbProvider.AddInParameter(command, helper.Resddpusucreacion, DbType.String, entity.Resddpusucreacion);
            dbProvider.AddInParameter(command, helper.Resddpfeccreacion, DbType.DateTime, entity.Resddpfeccreacion);
            dbProvider.AddInParameter(command, helper.Resddpcodi, DbType.Int32, entity.Resddpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerSddpId)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Resddpcodi, DbType.Int32, rerSddpId);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerSddpDTO GetById(int rerSddpId)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Resddpcodi, DbType.Int32, rerSddpId);
            RerSddpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerSddpDTO> List()
        {
            List<RerSddpDTO> entities = new List<RerSddpDTO>();
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


    }
}

