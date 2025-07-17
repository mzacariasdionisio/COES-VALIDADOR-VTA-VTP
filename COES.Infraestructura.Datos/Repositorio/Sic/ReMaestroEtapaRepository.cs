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
    /// Clase de acceso a datos de la tabla RE_MAESTRO_ETAPA
    /// </summary>
    public class ReMaestroEtapaRepository: RepositoryBase, IReMaestroEtapaRepository
    {
        public ReMaestroEtapaRepository(string strConn): base(strConn)
        {
        }

        ReMaestroEtapaHelper helper = new ReMaestroEtapaHelper();

        public int Save(ReMaestroEtapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reetacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reetanombre, DbType.String, entity.Reetanombre);
            dbProvider.AddInParameter(command, helper.Reetaorden, DbType.Int32, entity.Reetaorden);
            dbProvider.AddInParameter(command, helper.Reetaregistro, DbType.String, entity.Reetaregistro);
            dbProvider.AddInParameter(command, helper.Reetausucreacion, DbType.String, entity.Reetausucreacion);
            dbProvider.AddInParameter(command, helper.Reetafeccreacion, DbType.DateTime, entity.Reetafeccreacion);
            dbProvider.AddInParameter(command, helper.Reetausumodificacion, DbType.String, entity.Reetausumodificacion);
            dbProvider.AddInParameter(command, helper.Reetafecmodificacion, DbType.DateTime, entity.Reetafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReMaestroEtapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reetanombre, DbType.String, entity.Reetanombre);
            dbProvider.AddInParameter(command, helper.Reetaorden, DbType.Int32, entity.Reetaorden);
            dbProvider.AddInParameter(command, helper.Reetaregistro, DbType.String, entity.Reetaregistro);
            dbProvider.AddInParameter(command, helper.Reetausucreacion, DbType.String, entity.Reetausucreacion);
            dbProvider.AddInParameter(command, helper.Reetafeccreacion, DbType.DateTime, entity.Reetafeccreacion);
            dbProvider.AddInParameter(command, helper.Reetausumodificacion, DbType.String, entity.Reetausumodificacion);
            dbProvider.AddInParameter(command, helper.Reetafecmodificacion, DbType.DateTime, entity.Reetafecmodificacion);
            dbProvider.AddInParameter(command, helper.Reetacodi, DbType.Int32, entity.Reetacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reetacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reetacodi, DbType.Int32, reetacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReMaestroEtapaDTO GetById(int reetacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reetacodi, DbType.Int32, reetacodi);
            ReMaestroEtapaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReMaestroEtapaDTO> List()
        {
            List<ReMaestroEtapaDTO> entitys = new List<ReMaestroEtapaDTO>();
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

        public List<ReMaestroEtapaDTO> GetByCriteria(int idPeriodo)
        {
            List<ReMaestroEtapaDTO> entitys = new List<ReMaestroEtapaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, idPeriodo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReMaestroEtapaDTO entity = helper.Create(dr);

                    int iFechaFinal = dr.GetOrdinal(helper.Repeetfecha);
                    if (!dr.IsDBNull(iFechaFinal)) entity.FechaFinal = dr.GetDateTime(iFechaFinal);

                    int iEstado = dr.GetOrdinal(helper.Repeetestado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
