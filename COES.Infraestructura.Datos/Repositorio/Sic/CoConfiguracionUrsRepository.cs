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
    /// Clase de acceso a datos de la tabla CO_CONFIGURACION_URS
    /// </summary>
    public class CoConfiguracionUrsRepository: RepositoryBase, ICoConfiguracionUrsRepository
    {
        public CoConfiguracionUrsRepository(string strConn): base(strConn)
        {
        }

        CoConfiguracionUrsHelper helper = new CoConfiguracionUrsHelper();

        public int Save(CoConfiguracionUrsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Conurscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Conursfecinicio, DbType.DateTime, entity.Conursfecinicio);
            dbProvider.AddInParameter(command, helper.Conursfecfin, DbType.DateTime, entity.Conursfecfin);
            dbProvider.AddInParameter(command, helper.Conursusucreacion, DbType.String, entity.Conursusucreacion);
            dbProvider.AddInParameter(command, helper.Conursfeccreacion, DbType.DateTime, entity.Conursfeccreacion);
            dbProvider.AddInParameter(command, helper.Conursusumodificacion, DbType.String, entity.Conursusumodificacion);
            dbProvider.AddInParameter(command, helper.Conursfecmodificacion, DbType.DateTime, entity.Conursfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoConfiguracionUrsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Conursfecinicio, DbType.DateTime, entity.Conursfecinicio);
            dbProvider.AddInParameter(command, helper.Conursfecfin, DbType.DateTime, entity.Conursfecfin);
            dbProvider.AddInParameter(command, helper.Conursusucreacion, DbType.String, entity.Conursusucreacion);
            dbProvider.AddInParameter(command, helper.Conursfeccreacion, DbType.DateTime, entity.Conursfeccreacion);
            dbProvider.AddInParameter(command, helper.Conursusumodificacion, DbType.String, entity.Conursusumodificacion);
            dbProvider.AddInParameter(command, helper.Conursfecmodificacion, DbType.DateTime, entity.Conursfecmodificacion);
            dbProvider.AddInParameter(command, helper.Conurscodi, DbType.Int32, entity.Conurscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int conurscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Conurscodi, DbType.Int32, conurscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoConfiguracionUrsDTO GetById(int idPeriodo, int idVersion, int idUrs)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, idPeriodo);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, idVersion);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, idUrs);
            CoConfiguracionUrsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoConfiguracionUrsDTO> List()
        {
            List<CoConfiguracionUrsDTO> entitys = new List<CoConfiguracionUrsDTO>();
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

        public List<CoConfiguracionUrsDTO> GetByCriteria()
        {
            List<CoConfiguracionUrsDTO> entitys = new List<CoConfiguracionUrsDTO>();
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

        public List<CoConfiguracionUrsDTO> GetPorVersion(int idVersion)
        {
            List<CoConfiguracionUrsDTO> entitys = new List<CoConfiguracionUrsDTO>();
            string sql = string.Format(helper.SqlGetPorVersion, idVersion);
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
