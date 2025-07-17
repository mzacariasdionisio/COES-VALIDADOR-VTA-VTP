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
    /// Clase de acceso a datos de la tabla AUD_RIESGO
    /// </summary>
    public class AudRiesgoRepository: RepositoryBase, IAudRiesgoRepository
    {
        public AudRiesgoRepository(string strConn): base(strConn)
        {
        }

        AudRiesgoHelper helper = new AudRiesgoHelper();

        public int Save(AudRiesgoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Riesactivo, DbType.String, entity.Riesactivo);
            dbProvider.AddInParameter(command, helper.Rieshistorico, DbType.String, entity.Rieshistorico);
            dbProvider.AddInParameter(command, helper.Riesusucreacion, DbType.String, entity.Riesusucreacion);
            dbProvider.AddInParameter(command, helper.Riesfeccreacion, DbType.DateTime, entity.Riesfeccreacion);
            dbProvider.AddInParameter(command, helper.Riesusumodificacion, DbType.String, entity.Riesusumodificacion);
            dbProvider.AddInParameter(command, helper.Riesfecmodificacion, DbType.DateTime, entity.Riesfecmodificacion);
            dbProvider.AddInParameter(command, helper.Riescodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);
            dbProvider.AddInParameter(command, helper.Tabcdcodivaloracioninherente, DbType.Int32, entity.Tabcdcodivaloracioninherente);
            dbProvider.AddInParameter(command, helper.Tabcdcodivaloracionresidual, DbType.Int32, entity.Tabcdcodivaloracionresidual);
            dbProvider.AddInParameter(command, helper.Riescodigo, DbType.String, entity.Riescodigo);
            dbProvider.AddInParameter(command, helper.Riesdescripcion, DbType.String, entity.Riesdescripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudRiesgoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rieshistorico, DbType.String, entity.Rieshistorico);
            dbProvider.AddInParameter(command, helper.Riesusumodificacion, DbType.String, entity.Riesusumodificacion);
            dbProvider.AddInParameter(command, helper.Riesfecmodificacion, DbType.DateTime, entity.Riesfecmodificacion);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);
            dbProvider.AddInParameter(command, helper.Tabcdcodivaloracioninherente, DbType.Int32, entity.Tabcdcodivaloracioninherente);
            dbProvider.AddInParameter(command, helper.Tabcdcodivaloracionresidual, DbType.Int32, entity.Tabcdcodivaloracionresidual);
            dbProvider.AddInParameter(command, helper.Riescodigo, DbType.String, entity.Riescodigo);
            dbProvider.AddInParameter(command, helper.Riesdescripcion, DbType.String, entity.Riesdescripcion);
            dbProvider.AddInParameter(command, helper.Riescodi, DbType.Int32, entity.Riescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudRiesgoDTO riesgo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Riesusumodificacion, DbType.String, riesgo.Riesusumodificacion);
            dbProvider.AddInParameter(command, helper.Riesfecmodificacion, DbType.DateTime, riesgo.Riesfecmodificacion);
            dbProvider.AddInParameter(command, helper.Riescodi, DbType.Int32, riesgo.Riescodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudRiesgoDTO GetById(int riescodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Riescodi, DbType.Int32, riescodi);
            AudRiesgoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));
                }
            }

            return entity;
        }

        public List<AudRiesgoDTO> List()
        {
            List<AudRiesgoDTO> entitys = new List<AudRiesgoDTO>();
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

        public List<AudRiesgoDTO> GetByCriteria(AudRiesgoDTO riesgo)
        {
            List<AudRiesgoDTO> entitys = new List<AudRiesgoDTO>();

            riesgo.Riesdescripcion = string.IsNullOrEmpty(riesgo.Riesdescripcion) ? "" : riesgo.Riesdescripcion;

            string sql = string.Format(helper.SqlGetByCriteria, riesgo.Tabcdcodivaloracioninherente, riesgo.Tabcdcodivaloracionresidual, riesgo.Riesactivo, riesgo.Riesdescripcion, riesgo.nroPagina, riesgo.nroFilas);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudRiesgoDTO entity = helper.Create(dr);

                    int iValoracioninherente = dr.GetOrdinal(helper.Valoracioninherente);
                    if (!dr.IsDBNull(iValoracioninherente)) entity.Valoracioninherente = dr.GetString(iValoracioninherente);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosBusqueda(AudRiesgoDTO riesgo)
        {
            riesgo.Riesdescripcion = string.IsNullOrEmpty(riesgo.Riesdescripcion) ? "" : riesgo.Riesdescripcion;

            string query = string.Format(helper.SqlObtenerNroRegistroBusqueda, riesgo.Tabcdcodivaloracioninherente, riesgo.Tabcdcodivaloracionresidual, riesgo.Riesactivo, riesgo.Riesdescripcion, riesgo.nroPagina, riesgo.nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }
    }
}
