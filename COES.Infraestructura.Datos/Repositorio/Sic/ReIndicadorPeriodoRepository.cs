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
    /// Clase de acceso a datos de la tabla RE_INDICADOR_PERIODO
    /// </summary>
    public class ReIndicadorPeriodoRepository: RepositoryBase, IReIndicadorPeriodoRepository
    {
        public ReIndicadorPeriodoRepository(string strConn): base(strConn)
        {
        }

        ReIndicadorPeriodoHelper helper = new ReIndicadorPeriodoHelper();

        public int Save(ReIndicadorPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reindcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, entity.Recintcodi);
            dbProvider.AddInParameter(command, helper.Reindki, DbType.Decimal, entity.Reindki);
            dbProvider.AddInParameter(command, helper.Reindni, DbType.Decimal, entity.Reindni);
            dbProvider.AddInParameter(command, helper.Reindusucreacion, DbType.String, entity.Reindusucreacion);
            dbProvider.AddInParameter(command, helper.Reindfeccreacion, DbType.DateTime, entity.Reindfeccreacion);
            dbProvider.AddInParameter(command, helper.Reindusumodificacion, DbType.String, entity.Reindusumodificacion);
            dbProvider.AddInParameter(command, helper.Reindfecmodificacion, DbType.DateTime, entity.Reindfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReIndicadorPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, entity.Recintcodi);
            dbProvider.AddInParameter(command, helper.Reindki, DbType.Decimal, entity.Reindki);
            dbProvider.AddInParameter(command, helper.Reindni, DbType.Decimal, entity.Reindni);
            dbProvider.AddInParameter(command, helper.Reindusucreacion, DbType.String, entity.Reindusucreacion);
            dbProvider.AddInParameter(command, helper.Reindfeccreacion, DbType.DateTime, entity.Reindfeccreacion);
            dbProvider.AddInParameter(command, helper.Reindusumodificacion, DbType.String, entity.Reindusumodificacion);
            dbProvider.AddInParameter(command, helper.Reindfecmodificacion, DbType.DateTime, entity.Reindfecmodificacion);
            dbProvider.AddInParameter(command, helper.Reindcodi, DbType.Int32, entity.Reindcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reindcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reindcodi, DbType.Int32, reindcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReIndicadorPeriodoDTO GetById(int reindcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reindcodi, DbType.Int32, reindcodi);
            ReIndicadorPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReIndicadorPeriodoDTO> List()
        {
            List<ReIndicadorPeriodoDTO> entitys = new List<ReIndicadorPeriodoDTO>();
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

        public List<ReIndicadorPeriodoDTO> GetByCriteria()
        {
            List<ReIndicadorPeriodoDTO> entitys = new List<ReIndicadorPeriodoDTO>();
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

        public List<ReIndicadorPeriodoDTO> ObtenerParaImportar(int idPeriodo)
        {
            List<ReIndicadorPeriodoDTO> entitys = new List<ReIndicadorPeriodoDTO>();
            string query = string.Format(helper.SqlObtenerParaImportar, idPeriodo);
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
