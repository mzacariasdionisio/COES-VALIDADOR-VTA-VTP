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
    /// Clase de acceso a datos de la tabla RE_PERIODO_ETAPA
    /// </summary>
    public class RePeriodoEtapaRepository: RepositoryBase, IRePeriodoEtapaRepository
    {
        public RePeriodoEtapaRepository(string strConn): base(strConn)
        {
        }

        RePeriodoEtapaHelper helper = new RePeriodoEtapaHelper();

        public int Save(RePeriodoEtapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Repeetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reetacodi, DbType.Int32, entity.Reetacodi);
            dbProvider.AddInParameter(command, helper.Repeetfecha, DbType.DateTime, entity.Repeetfecha);
            dbProvider.AddInParameter(command, helper.Repeetestado, DbType.String, entity.Repeetestado);
            dbProvider.AddInParameter(command, helper.Repeetusucreacion, DbType.String, entity.Repeetusucreacion);
            dbProvider.AddInParameter(command, helper.Repeetfeccreacion, DbType.DateTime, entity.Repeetfeccreacion);
            dbProvider.AddInParameter(command, helper.Repeetusumodificacion, DbType.String, entity.Repeetusumodificacion);
            dbProvider.AddInParameter(command, helper.Repeetfecmodificacion, DbType.DateTime, entity.Repeetfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RePeriodoEtapaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reetacodi, DbType.Int32, entity.Reetacodi);
            dbProvider.AddInParameter(command, helper.Repeetfecha, DbType.DateTime, entity.Repeetfecha);
            dbProvider.AddInParameter(command, helper.Repeetestado, DbType.String, entity.Repeetestado);
            dbProvider.AddInParameter(command, helper.Repeetusucreacion, DbType.String, entity.Repeetusucreacion);
            dbProvider.AddInParameter(command, helper.Repeetfeccreacion, DbType.DateTime, entity.Repeetfeccreacion);
            dbProvider.AddInParameter(command, helper.Repeetusumodificacion, DbType.String, entity.Repeetusumodificacion);
            dbProvider.AddInParameter(command, helper.Repeetfecmodificacion, DbType.DateTime, entity.Repeetfecmodificacion);
            dbProvider.AddInParameter(command, helper.Repeetcodi, DbType.Int32, entity.Repeetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int repeetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, repeetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RePeriodoEtapaDTO GetById(int repeetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Repeetcodi, DbType.Int32, repeetcodi);
            RePeriodoEtapaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RePeriodoEtapaDTO> List()
        {
            List<RePeriodoEtapaDTO> entitys = new List<RePeriodoEtapaDTO>();
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

        public List<RePeriodoEtapaDTO> GetByCriteria()
        {
            List<RePeriodoEtapaDTO> entitys = new List<RePeriodoEtapaDTO>();
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

        public List<RePeriodoEtapaDTO> GetByPeriodo(int repercodi)
        {
            List<RePeriodoEtapaDTO> entitys = new List<RePeriodoEtapaDTO>();
            string query = string.Format(helper.SqlGetByPeriodo, repercodi);
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
