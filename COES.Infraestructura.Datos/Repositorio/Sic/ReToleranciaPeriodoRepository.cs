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
    /// Clase de acceso a datos de la tabla RE_TOLERANCIA_PERIODO
    /// </summary>
    public class ReToleranciaPeriodoRepository: RepositoryBase, IReToleranciaPeriodoRepository
    {
        public ReToleranciaPeriodoRepository(string strConn): base(strConn)
        {
        }

        ReToleranciaPeriodoHelper helper = new ReToleranciaPeriodoHelper();

        public int Save(ReToleranciaPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Retolcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Retolninf, DbType.Int32, entity.Retolninf);
            dbProvider.AddInParameter(command, helper.Retoldinf, DbType.Int32, entity.Retoldinf);
            dbProvider.AddInParameter(command, helper.Retolnsup, DbType.Int32, entity.Retolnsup);
            dbProvider.AddInParameter(command, helper.Retoldsup, DbType.Int32, entity.Retoldsup);
            dbProvider.AddInParameter(command, helper.Retolusucreacion, DbType.String, entity.Retolusucreacion);
            dbProvider.AddInParameter(command, helper.Retolfeccreacion, DbType.DateTime, entity.Retolfeccreacion);
            dbProvider.AddInParameter(command, helper.Retolusumodificacion, DbType.String, entity.Retolusumodificacion);
            dbProvider.AddInParameter(command, helper.Retolfecmodificacion, DbType.DateTime, entity.Retolfecmodificacion);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, entity.Rentcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReToleranciaPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Retolninf, DbType.Int32, entity.Retolninf);
            dbProvider.AddInParameter(command, helper.Retoldinf, DbType.Int32, entity.Retoldinf);
            dbProvider.AddInParameter(command, helper.Retolnsup, DbType.Int32, entity.Retolnsup);
            dbProvider.AddInParameter(command, helper.Retoldsup, DbType.Int32, entity.Retoldsup);
            dbProvider.AddInParameter(command, helper.Retolusucreacion, DbType.String, entity.Retolusucreacion);
            dbProvider.AddInParameter(command, helper.Retolfeccreacion, DbType.DateTime, entity.Retolfeccreacion);
            dbProvider.AddInParameter(command, helper.Retolusumodificacion, DbType.String, entity.Retolusumodificacion);
            dbProvider.AddInParameter(command, helper.Retolfecmodificacion, DbType.DateTime, entity.Retolfecmodificacion);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Rentcodi, DbType.Int32, entity.Rentcodi);
            dbProvider.AddInParameter(command, helper.Retolcodi, DbType.Int32, entity.Retolcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int retolcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Retolcodi, DbType.Int32, retolcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReToleranciaPeriodoDTO GetById(int retolcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Retolcodi, DbType.Int32, retolcodi);
            ReToleranciaPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReToleranciaPeriodoDTO> List()
        {
            List<ReToleranciaPeriodoDTO> entitys = new List<ReToleranciaPeriodoDTO>();
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

        public List<ReToleranciaPeriodoDTO> GetByCriteria(int periodo)
        {
            List<ReToleranciaPeriodoDTO> entitys = new List<ReToleranciaPeriodoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReToleranciaPeriodoDTO entity = helper.Create(dr);
                    
                    int iRentabrev = dr.GetOrdinal(helper.Rentabrev);
                    if (!dr.IsDBNull(iRentabrev)) entity.Rentabrev = dr.GetString(iRentabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReToleranciaPeriodoDTO> ObtenerParaImportar(int idPeriodo)
        {
            List<ReToleranciaPeriodoDTO> entitys = new List<ReToleranciaPeriodoDTO>();
            string sql = string.Format(helper.SqlObtenerParaImportar, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
