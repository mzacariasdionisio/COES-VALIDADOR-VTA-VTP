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
    /// Clase de acceso a datos de la tabla AUD_REQUERIMIENTO_INFORM
    /// </summary>
    public class AudRequerimientoInformRepository: RepositoryBase, IAudRequerimientoInformRepository
    {
        public AudRequerimientoInformRepository(string strConn): base(strConn)
        {
        }

        AudRequerimientoInformHelper helper = new AudRequerimientoInformHelper();

        public int Save(AudRequerimientoInformDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reqicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, entity.Progaecodi);
            dbProvider.AddInParameter(command, helper.Tabcdcodiestado, DbType.Int32, entity.Tabcdcodiestado);
            dbProvider.AddInParameter(command, helper.Percodiresponsable, DbType.Int32, entity.Percodiresponsable);
            dbProvider.AddInParameter(command, helper.Archcodirequerimiento, DbType.Int32, entity.Archcodirequerimiento);
            dbProvider.AddInParameter(command, helper.Reqiplazo, DbType.DateTime, entity.Reqiplazo);
            dbProvider.AddInParameter(command, helper.Reqirequerimiento, DbType.String, entity.Reqirequerimiento);
            dbProvider.AddInParameter(command, helper.Reqifechasolicitada, DbType.DateTime, entity.Reqifechasolicitada);
            dbProvider.AddInParameter(command, helper.Reqifechapresentada, DbType.DateTime, entity.Reqifechapresentada);
            dbProvider.AddInParameter(command, helper.Reqiactivo, DbType.String, entity.Reqiactivo);
            dbProvider.AddInParameter(command, helper.Reqihistorico, DbType.String, entity.Reqihistorico);
            dbProvider.AddInParameter(command, helper.Reqiusuregistro, DbType.String, entity.Reqiusuregistro);
            dbProvider.AddInParameter(command, helper.Reqifecregistro, DbType.DateTime, entity.Reqifecregistro);
            dbProvider.AddInParameter(command, helper.Reqiusumodificacion, DbType.String, entity.Reqiusumodificacion);
            dbProvider.AddInParameter(command, helper.Reqifecmodificacion, DbType.DateTime, entity.Reqifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudRequerimientoInformDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, entity.Progaecodi);
            dbProvider.AddInParameter(command, helper.Tabcdcodiestado, DbType.Int32, entity.Tabcdcodiestado);
            dbProvider.AddInParameter(command, helper.Percodiresponsable, DbType.Int32, entity.Percodiresponsable);
            dbProvider.AddInParameter(command, helper.Archcodirequerimiento, DbType.Int32, entity.Archcodirequerimiento);
            dbProvider.AddInParameter(command, helper.Reqiplazo, DbType.DateTime, entity.Reqiplazo);
            dbProvider.AddInParameter(command, helper.Reqirequerimiento, DbType.String, entity.Reqirequerimiento);
            dbProvider.AddInParameter(command, helper.Reqifechasolicitada, DbType.DateTime, entity.Reqifechasolicitada);
            dbProvider.AddInParameter(command, helper.Reqifechapresentada, DbType.DateTime, entity.Reqifechapresentada);
            dbProvider.AddInParameter(command, helper.Reqiactivo, DbType.String, entity.Reqiactivo);
            dbProvider.AddInParameter(command, helper.Reqihistorico, DbType.String, entity.Reqihistorico);
            dbProvider.AddInParameter(command, helper.Reqiusuregistro, DbType.String, entity.Reqiusuregistro);
            dbProvider.AddInParameter(command, helper.Reqifecregistro, DbType.DateTime, entity.Reqifecregistro);
            dbProvider.AddInParameter(command, helper.Reqiusumodificacion, DbType.String, entity.Reqiusumodificacion);
            dbProvider.AddInParameter(command, helper.Reqifecmodificacion, DbType.DateTime, entity.Reqifecmodificacion);
            dbProvider.AddInParameter(command, helper.Reqicodi, DbType.Int32, entity.Reqicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudRequerimientoInformDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reqiusumodificacion, DbType.String, entity.Reqiusumodificacion);
            dbProvider.AddInParameter(command, helper.Reqifecmodificacion, DbType.DateTime, entity.Reqifecmodificacion);
            dbProvider.AddInParameter(command, helper.Reqicodi, DbType.Int32, entity.Reqicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudRequerimientoInformDTO GetById(int reqicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reqicodi, DbType.Int32, reqicodi);
            AudRequerimientoInformDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudRequerimientoInformDTO> List()
        {
            List<AudRequerimientoInformDTO> entitys = new List<AudRequerimientoInformDTO>();
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

        public List<AudRequerimientoInformDTO> GetByCriteria(int progaecodi)
        {
            List<AudRequerimientoInformDTO> entitys = new List<AudRequerimientoInformDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, progaecodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudRequerimientoInformDTO requerimientoInform = helper.Create(dr);

                    int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodigo)) requerimientoInform.Elemcodigo = dr.GetString(iElemcodigo);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) requerimientoInform.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iEstadodescripcion = dr.GetOrdinal(helper.Estadodescripcion);
                    if (!dr.IsDBNull(iEstadodescripcion)) requerimientoInform.Estadodescripcion = dr.GetString(iEstadodescripcion);

                    int iTienearchivo = dr.GetOrdinal(helper.Tienearchivo);
                    if (!dr.IsDBNull(iTienearchivo)) requerimientoInform.Tienearchivo = Convert.ToInt32(dr.GetValue(iTienearchivo));
                    
                    entitys.Add(requerimientoInform);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosBusqueda(AudRequerimientoInformDTO requerimientoInformacion)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroBusquedaByAuditoria, requerimientoInformacion.Audicodi, requerimientoInformacion.Tabcdcodiestado, requerimientoInformacion.Usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<AudRequerimientoInformDTO> GetByCriteriaByAuditoria(AudRequerimientoInformDTO requerimientoInformacion)
        {
            List<AudRequerimientoInformDTO> entitys = new List<AudRequerimientoInformDTO>();

            string query = string.Format(helper.SqlGetByCriteriaByAuditoria, requerimientoInformacion.Audicodi, requerimientoInformacion.Tabcdcodiestado, requerimientoInformacion.nroPagina, requerimientoInformacion.nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudRequerimientoInformDTO requerimientoInform = helper.Create(dr);

                    int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodigo)) requerimientoInform.Elemcodigo = dr.GetString(iElemcodigo);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) requerimientoInform.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iEstadodescripcion = dr.GetOrdinal(helper.Estadodescripcion);
                    if (!dr.IsDBNull(iEstadodescripcion)) requerimientoInform.Estadodescripcion = dr.GetString(iEstadodescripcion);

                    int iUsercode = dr.GetOrdinal(helper.Usercode);
                    if (!dr.IsDBNull(iUsercode)) requerimientoInform.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));


                    entitys.Add(requerimientoInform);
                }
            }

            return entitys;
        }
    }
}
