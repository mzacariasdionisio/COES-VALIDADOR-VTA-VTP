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
    /// Clase de acceso a datos de la tabla SMA_AMPLIACION_PLAZO
    /// </summary>
    public class SmaAmpliacionPlazoRepository : RepositoryBase, ISmaAmpliacionPlazoRepository
    {
        public SmaAmpliacionPlazoRepository(string strConn) : base(strConn)
        {
        }

        SmaAmpliacionPlazoHelper helper = new SmaAmpliacionPlazoHelper();

        public int Save(SmaAmpliacionPlazoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Smaapcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Smaapaniomes, DbType.DateTime, entity.Smaapaniomes);
            dbProvider.AddInParameter(command, helper.Smaapplazodefecto, DbType.DateTime, entity.Smaapplazodefecto);
            dbProvider.AddInParameter(command, helper.Smaapnuevoplazo, DbType.DateTime, entity.Smaapnuevoplazo);
            dbProvider.AddInParameter(command, helper.Smaapestado, DbType.String, entity.Smaapestado);
            dbProvider.AddInParameter(command, helper.Smaapusucreacion, DbType.String, entity.Smaapusucreacion);
            dbProvider.AddInParameter(command, helper.Smaapfeccreacion, DbType.DateTime, entity.Smaapfeccreacion);
            dbProvider.AddInParameter(command, helper.Smaapusumodificacion, DbType.String, entity.Smaapusumodificacion);
            dbProvider.AddInParameter(command, helper.Smaapfecmodificacion, DbType.DateTime, entity.Smaapfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SmaAmpliacionPlazoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Smaapaniomes, DbType.DateTime, entity.Smaapaniomes);
            dbProvider.AddInParameter(command, helper.Smaapplazodefecto, DbType.DateTime, entity.Smaapplazodefecto);
            dbProvider.AddInParameter(command, helper.Smaapnuevoplazo, DbType.DateTime, entity.Smaapnuevoplazo);
            dbProvider.AddInParameter(command, helper.Smaapestado, DbType.String, entity.Smaapestado);
            dbProvider.AddInParameter(command, helper.Smaapusucreacion, DbType.String, entity.Smaapusucreacion);
            dbProvider.AddInParameter(command, helper.Smaapfeccreacion, DbType.DateTime, entity.Smaapfeccreacion);
            dbProvider.AddInParameter(command, helper.Smaapusumodificacion, DbType.String, entity.Smaapusumodificacion);
            dbProvider.AddInParameter(command, helper.Smaapfecmodificacion, DbType.DateTime, entity.Smaapfecmodificacion);

            dbProvider.AddInParameter(command, helper.Smaapcodi, DbType.Int32, entity.Smaapcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int smaapcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Smaapcodi, DbType.Int32, smaapcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaAmpliacionPlazoDTO GetById(int smaapcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Smaapcodi, DbType.Int32, smaapcodi);
            SmaAmpliacionPlazoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaAmpliacionPlazoDTO> List()
        {
            List<SmaAmpliacionPlazoDTO> entitys = new List<SmaAmpliacionPlazoDTO>();
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

        public List<SmaAmpliacionPlazoDTO> GetByCriteria()
        {
            List<SmaAmpliacionPlazoDTO> entitys = new List<SmaAmpliacionPlazoDTO>();
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
    }
}
