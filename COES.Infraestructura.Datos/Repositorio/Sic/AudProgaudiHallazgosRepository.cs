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
    /// Clase de acceso a datos de la tabla AUD_PROGAUDI_HALLAZGOS
    /// </summary>
    public class AudProgaudiHallazgosRepository : RepositoryBase, IAudProgaudiHallazgosRepository
    {
        public AudProgaudiHallazgosRepository(string strConn) : base(strConn)
        {
        }

        AudProgaudiHallazgosHelper helper = new AudProgaudiHallazgosHelper();

        public int Save(AudProgaudiHallazgosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Progahcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, entity.Progaecodi);
            dbProvider.AddInParameter(command, helper.Archcodianalisiscausa, DbType.Int32, entity.Archcodianalisiscausa);
            dbProvider.AddInParameter(command, helper.Archcodievidencia, DbType.Int32, entity.Archcodievidencia);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipohallazgo, DbType.Int32, entity.Tabcdcoditipohallazgo);
            dbProvider.AddInParameter(command, helper.Progaicodiresponsable, DbType.Int32, entity.Progaicodiresponsable);
            dbProvider.AddInParameter(command, helper.Progahdescripcion, DbType.String, entity.Progahdescripcion);
            dbProvider.AddInParameter(command, helper.Progahplanaccion, DbType.String, entity.Progahplanaccion);
            dbProvider.AddInParameter(command, helper.Progahaccionmejora, DbType.String, entity.Progahaccionmejora);
            dbProvider.AddInParameter(command, helper.Progahaccionmejoraplazo, DbType.DateTime, entity.Progahaccionmejoraplazo);
            dbProvider.AddInParameter(command, helper.Tabcdestadocodi, DbType.Int32, entity.Tabcdestadocodi);
            dbProvider.AddInParameter(command, helper.Progahactivo, DbType.String, entity.Progahactivo);
            dbProvider.AddInParameter(command, helper.Progahhistorico, DbType.String, entity.Progahhistorico);
            dbProvider.AddInParameter(command, helper.Progahusucreacion, DbType.String, entity.Progahusucreacion);
            dbProvider.AddInParameter(command, helper.Progahfeccreacion, DbType.DateTime, entity.Progahfeccreacion);
            dbProvider.AddInParameter(command, helper.Progahusumodificacion, DbType.String, entity.Progahusumodificacion);
            dbProvider.AddInParameter(command, helper.Progahfecmodificacion, DbType.DateTime, entity.Progahfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudProgaudiHallazgosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, entity.Progaecodi);
            dbProvider.AddInParameter(command, helper.Archcodianalisiscausa, DbType.Int32, entity.Archcodianalisiscausa);
            dbProvider.AddInParameter(command, helper.Archcodievidencia, DbType.Int32, entity.Archcodievidencia);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipohallazgo, DbType.Int32, entity.Tabcdcoditipohallazgo);
            dbProvider.AddInParameter(command, helper.Progaicodiresponsable, DbType.Int32, entity.Progaicodiresponsable);
            dbProvider.AddInParameter(command, helper.Progahdescripcion, DbType.String, entity.Progahdescripcion);
            dbProvider.AddInParameter(command, helper.Progahplanaccion, DbType.String, entity.Progahplanaccion);
            dbProvider.AddInParameter(command, helper.Progahaccionmejora, DbType.String, entity.Progahaccionmejora);
            dbProvider.AddInParameter(command, helper.Progahaccionmejoraplazo, DbType.DateTime, entity.Progahaccionmejoraplazo);
            dbProvider.AddInParameter(command, helper.Tabcdestadocodi, DbType.Int32, entity.Tabcdestadocodi);
            dbProvider.AddInParameter(command, helper.Progahhistorico, DbType.String, entity.Progahhistorico);
            dbProvider.AddInParameter(command, helper.Progahusumodificacion, DbType.String, entity.Progahusumodificacion);
            dbProvider.AddInParameter(command, helper.Progahfecmodificacion, DbType.DateTime, entity.Progahfecmodificacion);
            dbProvider.AddInParameter(command, helper.Progahcodi, DbType.Int32, entity.Progahcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudProgaudiHallazgosDTO progaudiHallazgo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Progahusumodificacion, DbType.String, progaudiHallazgo.Progahusumodificacion);
            dbProvider.AddInParameter(command, helper.Progahfecmodificacion, DbType.DateTime, progaudiHallazgo.Progahfecmodificacion);
            dbProvider.AddInParameter(command, helper.Progahcodi, DbType.Int32, progaudiHallazgo.Progahcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudProgaudiHallazgosDTO GetById(int progahcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Progahcodi, DbType.Int32, progahcodi);
            AudProgaudiHallazgosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iElemcodi = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodi)) entity.Elemcodigo = dr.GetString(iElemcodi);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);
                }
            }

            return entity;
        }

        public List<AudProgaudiHallazgosDTO> List()
        {
            List<AudProgaudiHallazgosDTO> entitys = new List<AudProgaudiHallazgosDTO>();
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

        public List<AudProgaudiHallazgosDTO> GetByCriteria(int progaecodi)
        {
            List<AudProgaudiHallazgosDTO> entitys = new List<AudProgaudiHallazgosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, progaecodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgaudiHallazgosDTO entity = helper.Create(dr);

                    int iTipoHallazgo = dr.GetOrdinal(helper.TipoHallazgo);
                    if (!dr.IsDBNull(iTipoHallazgo)) entity.Tipohallazgo = dr.GetString(iTipoHallazgo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosBusqueda(AudProgaudiHallazgosDTO progaudiHallazgo)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroBusquedaByAuditoria, progaudiHallazgo.Audicodi, progaudiHallazgo.Usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<AudProgaudiHallazgosDTO> GetByCriteriaPorAudi(AudProgaudiHallazgosDTO progaudiHallazgo)
        {
            List<AudProgaudiHallazgosDTO> entitys = new List<AudProgaudiHallazgosDTO>();

            string query = string.Format(helper.SqlGetByCriteriaPorAudi, progaudiHallazgo.Audicodi, string.Format(" And (tabcdestadocodi = {0} or {0} = 0)", progaudiHallazgo.Tabcdcodiestado));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgaudiHallazgosDTO entity = helper.Create(dr);

                    int iTipoHallazgo = dr.GetOrdinal(helper.TipoHallazgo);
                    if (!dr.IsDBNull(iTipoHallazgo)) entity.Tipohallazgo = dr.GetString(iTipoHallazgo);

                    int iElemcodi = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodi)) entity.Elemcodigo = dr.GetString(iElemcodi);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iProgacodi = dr.GetOrdinal(helper.Progacodi);
                    if (!dr.IsDBNull(iProgacodi)) entity.Progacodi = Convert.ToInt32(dr.GetValue(iProgacodi));

                    int iUsercode = dr.GetOrdinal(helper.Usercode);
                    if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

                    int iEstadohallazgo = dr.GetOrdinal(helper.Estadohallazgo);
                    if (!dr.IsDBNull(iEstadohallazgo)) entity.Estadohallazgo = dr.GetString(iEstadohallazgo);
                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
