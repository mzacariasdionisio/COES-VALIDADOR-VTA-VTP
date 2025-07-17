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
    /// Clase de acceso a datos de la tabla PFR_PERIODO
    /// </summary>
    public class PfrPeriodoRepository: RepositoryBase, IPfrPeriodoRepository
    {
        public PfrPeriodoRepository(string strConn): base(strConn)
        {
        }

        PfrPeriodoHelper helper = new PfrPeriodoHelper();

        public int Save(PfrPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfrpercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfrpernombre, DbType.String, entity.Pfrpernombre);
            dbProvider.AddInParameter(command, helper.Pfrperanio, DbType.Int32, entity.Pfrperanio);
            dbProvider.AddInParameter(command, helper.Pfrpermes, DbType.Int32, entity.Pfrpermes);
            dbProvider.AddInParameter(command, helper.Pfrperaniomes, DbType.Int32, entity.Pfrperaniomes);
            dbProvider.AddInParameter(command, helper.Pfrperusucreacion, DbType.String, entity.Pfrperusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrperfeccreacion, DbType.DateTime, entity.Pfrperfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrperfecmodificacion, DbType.DateTime, entity.Pfrperfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfrperusumodificacion, DbType.String, entity.Pfrperusumodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrpercodi, DbType.Int32, entity.Pfrpercodi);
            dbProvider.AddInParameter(command, helper.Pfrpernombre, DbType.String, entity.Pfrpernombre);
            dbProvider.AddInParameter(command, helper.Pfrperanio, DbType.Int32, entity.Pfrperanio);
            dbProvider.AddInParameter(command, helper.Pfrpermes, DbType.Int32, entity.Pfrpermes);
            dbProvider.AddInParameter(command, helper.Pfrperaniomes, DbType.Int32, entity.Pfrperaniomes);
            dbProvider.AddInParameter(command, helper.Pfrperusucreacion, DbType.String, entity.Pfrperusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrperfeccreacion, DbType.DateTime, entity.Pfrperfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrperfecmodificacion, DbType.DateTime, entity.Pfrperfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfrperusumodificacion, DbType.String, entity.Pfrperusumodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrpercodi, DbType.Int32, pfrpercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrPeriodoDTO GetById(int pfrpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrpercodi, DbType.Int32, pfrpercodi);
            PfrPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrPeriodoDTO> List()
        {
            List<PfrPeriodoDTO> entitys = new List<PfrPeriodoDTO>();
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

        public List<PfrPeriodoDTO> GetByCriteria(int anio)
        {
            List<PfrPeriodoDTO> entitys = new List<PfrPeriodoDTO>();

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
