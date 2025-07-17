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
    /// Clase de acceso a datos de la tabla CO_CONFIGURACION_GEN
    /// </summary>
    public class CoConfiguracionGenRepository: RepositoryBase, ICoConfiguracionGenRepository
    {
        public CoConfiguracionGenRepository(string strConn): base(strConn)
        {
        }

        CoConfiguracionGenHelper helper = new CoConfiguracionGenHelper();

        public int Save(CoConfiguracionGenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Courgecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Courdecodi, DbType.Int32, entity.Courdecodi);
            dbProvider.AddInParameter(command, helper.Courgeusucreacion, DbType.String, entity.Courgeusucreacion);
            dbProvider.AddInParameter(command, helper.Courgefeccreacion, DbType.DateTime, entity.Courgefeccreacion);
            dbProvider.AddInParameter(command, helper.Courgeusumodificacion, DbType.String, entity.Courgeusumodificacion);
            dbProvider.AddInParameter(command, helper.Courgefecmodificacion, DbType.DateTime, entity.Courgefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoConfiguracionGenDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Courdecodi, DbType.Int32, entity.Courdecodi);
            dbProvider.AddInParameter(command, helper.Courgeusucreacion, DbType.String, entity.Courgeusucreacion);
            dbProvider.AddInParameter(command, helper.Courgefeccreacion, DbType.DateTime, entity.Courgefeccreacion);
            dbProvider.AddInParameter(command, helper.Courgeusumodificacion, DbType.String, entity.Courgeusumodificacion);
            dbProvider.AddInParameter(command, helper.Courgefecmodificacion, DbType.DateTime, entity.Courgefecmodificacion);
            dbProvider.AddInParameter(command, helper.Courgecodi, DbType.Int32, entity.Courgecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int courgecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Courgecodi, DbType.Int32, courgecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoConfiguracionGenDTO GetById(int courgecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Courgecodi, DbType.Int32, courgecodi);
            CoConfiguracionGenDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoConfiguracionGenDTO> List()
        {
            List<CoConfiguracionGenDTO> entitys = new List<CoConfiguracionGenDTO>();
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

        public List<CoConfiguracionGenDTO> GetByCriteria(int idConfiguracionDet)
        {
            List<CoConfiguracionGenDTO> entitys = new List<CoConfiguracionGenDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idConfiguracionDet);
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

        public List<CoConfiguracionGenDTO> GetUnidadesSeleccionadas(string strCourdecodis)
        {
            List<CoConfiguracionGenDTO> entitys = new List<CoConfiguracionGenDTO>();
            string sql = string.Format(helper.SqlGetUnidadesSeleccionadas, strCourdecodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CoConfiguracionGenDTO entity = new CoConfiguracionGenDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
