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
    /// Clase de acceso a datos de la tabla RER_PARAMETRO_REVISION
    /// </summary>
    public class RerParametroRevisionRepository : RepositoryBase, IRerParametroRevisionRepository
    {
        public RerParametroRevisionRepository(string strConn)
            : base(strConn)
        {
        }

        RerParametroRevisionHelper helper = new RerParametroRevisionHelper();

        public int Save(RerParametroRevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerprecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Perinombre, DbType.String, entity.Perinombre);
            dbProvider.AddInParameter(command, helper.Recanombre, DbType.String, entity.Recanombre);
            dbProvider.AddInParameter(command, helper.Rerpretipo, DbType.String, entity.Rerpretipo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Rerpreusucreacion, DbType.String, entity.Rerpreusucreacion);
            dbProvider.AddInParameter(command, helper.Rerprefeccreacion, DbType.DateTime, entity.Rerprefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerParametroRevisionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Perinombre, DbType.String, entity.Perinombre);
            dbProvider.AddInParameter(command, helper.Recanombre, DbType.String, entity.Recanombre);
            dbProvider.AddInParameter(command, helper.Rerpretipo, DbType.String, entity.Rerpretipo);
            dbProvider.AddInParameter(command, helper.Rerpreusucreacion, DbType.String, entity.Rerpreusucreacion);
            dbProvider.AddInParameter(command, helper.Rerprefeccreacion, DbType.DateTime, entity.Rerprefeccreacion);
            dbProvider.AddInParameter(command, helper.Rerprecodi, DbType.Int32, entity.Rerprecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerPreCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerprecodi, DbType.Int32, rerPreCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerParametroRevisionDTO GetById(int rerPreCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerprecodi, DbType.Int32, rerPreCodi);
            RerParametroRevisionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerParametroRevisionDTO> List()
        {
            List<RerParametroRevisionDTO> entities = new List<RerParametroRevisionDTO>();
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
        public List<RerParametroRevisionDTO> ListByRerpprcodiByTipo(int Rerpprcodi, string Rerpretipo)
        {
            List<RerParametroRevisionDTO> entities = new List<RerParametroRevisionDTO>();

            string query = string.Format(helper.SqlListByRerpprcodiByTipo, Rerpprcodi, Rerpretipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public void DeleteAllByRerpprcodi(int Rerpprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAllByRerpprcodi);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, Rerpprcodi);

            dbProvider.ExecuteNonQuery(command);
        }


    }
}

