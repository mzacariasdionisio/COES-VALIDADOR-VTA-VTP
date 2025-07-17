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
    /// Clase de acceso a datos de la tabla CO_ENVIOLIQUIDACION
    /// </summary>
    public class CoEnvioliquidacionRepository: RepositoryBase, ICoEnvioliquidacionRepository
    {
        public CoEnvioliquidacionRepository(string strConn): base(strConn)
        {
        }

        CoEnvioliquidacionHelper helper = new CoEnvioliquidacionHelper();

        public int Save(CoEnvioliquidacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Coenlicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Coenlifecha, DbType.DateTime, entity.Coenlifecha);
            dbProvider.AddInParameter(command, helper.Coenliusuario, DbType.String, entity.Coenliusuario);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoEnvioliquidacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Coenlifecha, DbType.DateTime, entity.Coenlifecha);
            dbProvider.AddInParameter(command, helper.Coenliusuario, DbType.String, entity.Coenliusuario);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Coenlicodi, DbType.Int32, entity.Coenlicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int coenlicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Coenlicodi, DbType.Int32, coenlicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoEnvioliquidacionDTO GetById(int coenlicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Coenlicodi, DbType.Int32, coenlicodi);
            CoEnvioliquidacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoEnvioliquidacionDTO> List()
        {
            List<CoEnvioliquidacionDTO> entitys = new List<CoEnvioliquidacionDTO>();
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

        public List<CoEnvioliquidacionDTO> GetByCriteria(int idVersion)
        {
            List<CoEnvioliquidacionDTO> entitys = new List<CoEnvioliquidacionDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, idVersion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoEnvioliquidacionDTO entity = helper.Create(dr);

                    int iPeriodonomb = dr.GetOrdinal(helper.Periodonomb);
                    if (!dr.IsDBNull(iPeriodonomb)) entity.Periodonomb = dr.GetString(iPeriodonomb);

                    int iVersionnomb = dr.GetOrdinal(helper.Versionnomb);
                    if (!dr.IsDBNull(iVersionnomb)) entity.Versionnomb = dr.GetString(iVersionnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoEnvioliquidacionDTO> ObtenerEnviosPorPeriodo(int idPeriodo)
        {
            List<CoEnvioliquidacionDTO> entitys = new List<CoEnvioliquidacionDTO>();

            string sql = string.Format(helper.SqlObtenerEnviosPorPeriodo, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoEnvioliquidacionDTO entity = helper.Create(dr);

                    int iPeriodonomb = dr.GetOrdinal(helper.Periodonomb);
                    if (!dr.IsDBNull(iPeriodonomb)) entity.Periodonomb = dr.GetString(iPeriodonomb);

                    int iVersionnomb = dr.GetOrdinal(helper.Versionnomb);
                    if (!dr.IsDBNull(iVersionnomb)) entity.Versionnomb = dr.GetString(iVersionnomb);

                    int iPeriododesc = dr.GetOrdinal(helper.Periododesc);
                    if (!dr.IsDBNull(iPeriododesc)) entity.Periododesc = dr.GetString(iPeriododesc);

                    int iVersiondesc = dr.GetOrdinal(helper.Versiondesc);
                    if (!dr.IsDBNull(iVersiondesc)) entity.Versiondesc = dr.GetString(iVersiondesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
