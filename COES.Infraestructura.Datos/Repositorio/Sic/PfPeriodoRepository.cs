using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PF_PERIODO
    /// </summary>
    public class PfPeriodoRepository : RepositoryBase, IPfPeriodoRepository
    {
        public PfPeriodoRepository(string strConn) : base(strConn)
        {
        }

        PfPeriodoHelper helper = new PfPeriodoHelper();

        public int Save(PfPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfperinombre, DbType.String, entity.Pfperinombre);
            dbProvider.AddInParameter(command, helper.Pfperianio, DbType.Int32, entity.Pfperianio);
            dbProvider.AddInParameter(command, helper.Pfperimes, DbType.Int32, entity.Pfperimes);
            dbProvider.AddInParameter(command, helper.Pfperianiomes, DbType.Int32, entity.Pfperianiomes);
            dbProvider.AddInParameter(command, helper.Pfperiusucreacion, DbType.String, entity.Pfperiusucreacion);
            dbProvider.AddInParameter(command, helper.Pfperifeccreacion, DbType.DateTime, entity.Pfperifeccreacion);
            dbProvider.AddInParameter(command, helper.Pfperiusumodificacion, DbType.String, entity.Pfperiusumodificacion);
            dbProvider.AddInParameter(command, helper.Pfperifecmodificacion, DbType.DateTime, entity.Pfperifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfperinombre, DbType.String, entity.Pfperinombre);
            dbProvider.AddInParameter(command, helper.Pfperianio, DbType.Int32, entity.Pfperianio);
            dbProvider.AddInParameter(command, helper.Pfperimes, DbType.Int32, entity.Pfperimes);
            dbProvider.AddInParameter(command, helper.Pfperianiomes, DbType.Int32, entity.Pfperianiomes);
            dbProvider.AddInParameter(command, helper.Pfperiusucreacion, DbType.String, entity.Pfperiusucreacion);
            dbProvider.AddInParameter(command, helper.Pfperifeccreacion, DbType.DateTime, entity.Pfperifeccreacion);
            dbProvider.AddInParameter(command, helper.Pfperiusumodificacion, DbType.String, entity.Pfperiusumodificacion);
            dbProvider.AddInParameter(command, helper.Pfperifecmodificacion, DbType.DateTime, entity.Pfperifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfpericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, pfpericodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfPeriodoDTO GetById(int pfpericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, pfpericodi);
            PfPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfPeriodoDTO> List()
        {
            List<PfPeriodoDTO> entitys = new List<PfPeriodoDTO>();
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

        public List<PfPeriodoDTO> GetByCriteria(int anio)
        {
            List<PfPeriodoDTO> entitys = new List<PfPeriodoDTO>();

            string query = string.Format(helper.SqlGetByCriteria, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
