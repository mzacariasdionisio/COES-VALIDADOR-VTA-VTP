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
    /// Clase de acceso a datos de la tabla RER_PARAMETRO_PRIMA
    /// </summary>
    public class RerParametroPrimaRepository : RepositoryBase, IRerParametroPrimaRepository
    {
        public RerParametroPrimaRepository(string strConn)
            : base(strConn)
        {
        }

        RerParametroPrimaHelper helper = new RerParametroPrimaHelper();

        public int Save(RerParametroPrimaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi);
            dbProvider.AddInParameter(command, helper.Rerpprmes, DbType.Int32, entity.Rerpprmes);
            dbProvider.AddInParameter(command, helper.Rerpprmesaniodesc, DbType.String, entity.Rerpprmesaniodesc);
            dbProvider.AddInParameter(command, helper.Rerpprtipocambio, DbType.Decimal, entity.Rerpprtipocambio);
            dbProvider.AddInParameter(command, helper.Rerpprorigen, DbType.String, entity.Rerpprorigen);
            dbProvider.AddInParameter(command, helper.Rerpprrevision, DbType.String, entity.Rerpprrevision);
            dbProvider.AddInParameter(command, helper.Rerpprusucreacion, DbType.String, entity.Rerpprusucreacion);
            dbProvider.AddInParameter(command, helper.Rerpprfeccreacion, DbType.DateTime, entity.Rerpprfeccreacion);
            dbProvider.AddInParameter(command, helper.Rerpprusumodificacion, DbType.String, entity.Rerpprusumodificacion);
            dbProvider.AddInParameter(command, helper.Rerpprfecmodificacion, DbType.DateTime, entity.Rerpprfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerParametroPrimaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi);
            dbProvider.AddInParameter(command, helper.Rerpprmes, DbType.Int32, entity.Rerpprmes);
            dbProvider.AddInParameter(command, helper.Rerpprmesaniodesc, DbType.String, entity.Rerpprmesaniodesc);
            dbProvider.AddInParameter(command, helper.Rerpprtipocambio, DbType.Decimal, entity.Rerpprtipocambio);
            dbProvider.AddInParameter(command, helper.Rerpprorigen, DbType.String, entity.Rerpprorigen);
            dbProvider.AddInParameter(command, helper.Rerpprrevision, DbType.String, entity.Rerpprrevision);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Rerpprusucreacion, DbType.String, entity.Rerpprusucreacion);
            dbProvider.AddInParameter(command, helper.Rerpprfeccreacion, DbType.DateTime, entity.Rerpprfeccreacion);
            dbProvider.AddInParameter(command, helper.Rerpprusumodificacion, DbType.String, entity.Rerpprusumodificacion);
            dbProvider.AddInParameter(command, helper.Rerpprfecmodificacion, DbType.DateTime, entity.Rerpprfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerPprCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, rerPprCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerParametroPrimaDTO GetById(int rerPprCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, rerPprCodi);
            RerParametroPrimaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerParametroPrimaDTO> GetByCriteria(string sAnio, int iMes)
        {
            List<RerParametroPrimaDTO> entities = new List<RerParametroPrimaDTO>();
            string query = string.Format(helper.SqlGetByCriteria, iMes, sAnio);
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

        public List<RerParametroPrimaDTO> List()
        {
            List<RerParametroPrimaDTO> entities = new List<RerParametroPrimaDTO>();
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

        public List<RerParametroPrimaDTO> GetByAnioVersion(int reravcodi)
        {
            List<RerParametroPrimaDTO> entities = new List<RerParametroPrimaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByAnioVersion);
            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, reravcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<RerParametroPrimaDTO> GetByAnioVersionByMes(int reravcodi, string rerpprmes)
        {
            string query = string.Format(helper.SqlGetByAnioVersionByMes, reravcodi, rerpprmes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerParametroPrimaDTO> entities = new List<RerParametroPrimaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<RerParametroPrimaDTO> listaParametroPrimaRerByAnio(int Reravaniotarif)
        {
            List<RerParametroPrimaDTO> entities = new List<RerParametroPrimaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqllistaParametroPrimaRerByAnio);
            dbProvider.AddInParameter(command, helper.Reravaniotarif, DbType.Int32, Reravaniotarif);
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
