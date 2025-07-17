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
    /// Clase de acceso a datos de la tabla ST_PERIODO
    /// </summary>
    public class StPeriodoRepository : RepositoryBase, IStPeriodoRepository
    {
        public StPeriodoRepository(string strConn)
            : base(strConn)
        {
        }

        StPeriodoHelper helper = new StPeriodoHelper();

        public int Save(StPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Stperanio, DbType.Int32, entity.Stperanio);
            dbProvider.AddInParameter(command, helper.Stpermes, DbType.Int32, entity.Stpermes);
            dbProvider.AddInParameter(command, helper.Stperaniomes, DbType.Int32, entity.Stperaniomes);
            dbProvider.AddInParameter(command, helper.Stpernombre, DbType.String, entity.Stpernombre);
            dbProvider.AddInParameter(command, helper.Stperusucreacion, DbType.String, entity.Stperusucreacion);
            dbProvider.AddInParameter(command, helper.Stperfeccreacion, DbType.DateTime, entity.Stperfeccreacion);
            dbProvider.AddInParameter(command, helper.Stperusumodificacion, DbType.String, entity.Stperusumodificacion);
            dbProvider.AddInParameter(command, helper.Stperfecmodificacion, DbType.DateTime, entity.Stperfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Stperanio, DbType.Int32, entity.Stperanio);
            dbProvider.AddInParameter(command, helper.Stpermes, DbType.Int32, entity.Stpermes);
            dbProvider.AddInParameter(command, helper.Stperaniomes, DbType.Int32, entity.Stperaniomes);
            dbProvider.AddInParameter(command, helper.Stpernombre, DbType.String, entity.Stpernombre);
            dbProvider.AddInParameter(command, helper.Stperusucreacion, DbType.String, entity.Stperusucreacion);
            dbProvider.AddInParameter(command, helper.Stperfeccreacion, DbType.DateTime, entity.Stperfeccreacion);
            dbProvider.AddInParameter(command, helper.Stperusumodificacion, DbType.String, entity.Stperusumodificacion);
            dbProvider.AddInParameter(command, helper.Stperfecmodificacion, DbType.DateTime, entity.Stperfecmodificacion);
            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, entity.Stpercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int stpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, stpercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StPeriodoDTO GetById(int stpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, stpercodi);
            StPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StPeriodoDTO> List()
        {
            List<StPeriodoDTO> entitys = new List<StPeriodoDTO>();
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

        public List<StPeriodoDTO> GetByCriteria(string nombre)
        {
            List<StPeriodoDTO> entitys = new List<StPeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Stpernombre, DbType.String, nombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public StPeriodoDTO GetByIdPeriodoAnterior(int stpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdPeriodoAnterior);

            dbProvider.AddInParameter(command, helper.Stpercodi, DbType.Int32, stpercodi);
            StPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

    }
}
