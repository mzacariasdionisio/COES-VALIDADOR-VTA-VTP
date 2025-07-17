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
    /// Clase de acceso a datos de la tabla AUD_AUDITORIA
    /// </summary>
    public class AudAuditoriaRepository: RepositoryBase, IAudAuditoriaRepository
    {
        public AudAuditoriaRepository(string strConn): base(strConn)
        {
        }

        AudAuditoriaHelper helper = new AudAuditoriaHelper();

        public int Save(AudAuditoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tabcdestadocodi, DbType.Int32, entity.Tabcdestadocodi);
            dbProvider.AddInParameter(command, helper.Audinombre, DbType.String, entity.Audinombre);
            dbProvider.AddInParameter(command, helper.Audiobjetivo, DbType.String, entity.Audiobjetivo);
            dbProvider.AddInParameter(command, helper.Audifechainicio, DbType.DateTime, entity.Audifechainicio);
            dbProvider.AddInParameter(command, helper.Audifechafin, DbType.DateTime, entity.Audifechafin);
            dbProvider.AddInParameter(command, helper.Audiactivo, DbType.String, entity.Audiactivo);
            dbProvider.AddInParameter(command, helper.Audihistorico, DbType.String, entity.Audihistorico);
            dbProvider.AddInParameter(command, helper.Audiusucreacion, DbType.String, entity.Audiusucreacion);
            dbProvider.AddInParameter(command, helper.Audifeccreacion, DbType.DateTime, entity.Audifeccreacion);
            dbProvider.AddInParameter(command, helper.Audiusumodificacion, DbType.String, entity.Audiusumodificacion);
            dbProvider.AddInParameter(command, helper.Audifecmodificacion, DbType.DateTime, entity.Audifecmodificacion);
            

            string codigo = string.Format("AUD-{0}-{1}", entity.Audicodigenerado.ToString().PadLeft(2, '0'), DateTime.Now.Year);
            dbProvider.AddInParameter(command, helper.Audicodigo, DbType.String, codigo);

            dbProvider.AddInParameter(command, helper.Audialcance, DbType.String, entity.Audialcance);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudAuditoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tabcdestadocodi, DbType.Int32, entity.Tabcdestadocodi);
            dbProvider.AddInParameter(command, helper.Audinombre, DbType.String, entity.Audinombre);
            dbProvider.AddInParameter(command, helper.Audiobjetivo, DbType.String, entity.Audiobjetivo);
            dbProvider.AddInParameter(command, helper.Audifechainicio, DbType.DateTime, entity.Audifechainicio);
            dbProvider.AddInParameter(command, helper.Audifechafin, DbType.DateTime, entity.Audifechafin);
            dbProvider.AddInParameter(command, helper.Audiusumodificacion, DbType.String, entity.Audiusumodificacion);
            dbProvider.AddInParameter(command, helper.Audifecmodificacion, DbType.DateTime, entity.Audifecmodificacion);
            dbProvider.AddInParameter(command, helper.Audialcance, DbType.String, entity.Audialcance);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, entity.Audicodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudAuditoriaDTO auditoria)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Audiusumodificacion, DbType.String, auditoria.Audiusumodificacion);
            dbProvider.AddInParameter(command, helper.Audifecmodificacion, DbType.DateTime, auditoria.Audifecmodificacion);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, auditoria.Audicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudAuditoriaDTO GetById(int audicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, audicodi);
            AudAuditoriaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iAudiesplanificado = dr.GetOrdinal(helper.Audipsplanificado);
                    if (!dr.IsDBNull(iAudiesplanificado)) entity.Audiesplanificado = dr.GetString(iAudiesplanificado);

                    //int iAudppcodi = dr.GetOrdinal(helper.Audppcodi);
                    //if (!dr.IsDBNull(iAudppcodi)) entity.Audppcodi = Convert.ToInt32(dr.GetValue(iAudppcodi));

                    //int iProccodi = dr.GetOrdinal(helper.Proccodi);
                    //if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));
                    int iAudialcance = dr.GetOrdinal(helper.Audialcance);
                    if (!dr.IsDBNull(iAudialcance)) entity.Audialcance = dr.GetString(iAudialcance);

                    int iAudpcodi = dr.GetOrdinal(helper.Audpcodi);
                    if (!dr.IsDBNull(iAudpcodi)) entity.Audpcodi = Convert.ToInt32(dr.GetValue(iAudpcodi));

                    int iPlancodi = dr.GetOrdinal(helper.Plancodi);
                    if (!dr.IsDBNull(iPlancodi)) entity.Plancodi = Convert.ToInt32(dr.GetValue(iPlancodi));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);
                }
            }

            return entity;
        }

        public List<AudAuditoriaDTO> List()
        {
            List<AudAuditoriaDTO> entitys = new List<AudAuditoriaDTO>();
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

        public List<AudAuditoriaDTO> GetByCriteria(AudAuditoriaDTO auditoria)
        {
            List<AudAuditoriaDTO> entitys = new List<AudAuditoriaDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, auditoria.Tabcdestadocodi, auditoria.AnioVigencia, auditoria.Audinombre, auditoria.nroPagina, auditoria.nroFilas);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAuditoriaDTO entity = helper.Create(dr);

                    int iEstadodescripcion = dr.GetOrdinal(helper.Estadodescripcion);
                    if (!dr.IsDBNull(iEstadodescripcion)) entity.Estadodescripcion = dr.GetString(iEstadodescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<string> MostrarAnios()
        {
            List<string> entitys = new List<string>();

            string sql = string.Format(helper.SqlMostrarAnios);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            string anio="";
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iAudicodi = dr.GetOrdinal("anio");
                    if (!dr.IsDBNull(iAudicodi)) anio = dr.GetValue(iAudicodi).ToString();
                    entitys.Add(anio);
                }
            }

            return entitys;
        }

        public List<AudAuditoriaDTO> MostrarAuditoriasEjecutar(AudAuditoriaDTO auditoria)
        {
            List<AudAuditoriaDTO> entitys = new List<AudAuditoriaDTO>();

            string sql = string.Format(helper.SqlMostrarAuditoriasEjectuar,auditoria.Tabcdestadocodi,auditoria.AnioInicio,auditoria.Audinombre,auditoria.Audiactivo);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
        
          
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAuditoriaDTO entity = helper.Create(dr);

                    int iEstadodescripcion = dr.GetOrdinal(helper.Estadodescripcion);
                    if (!dr.IsDBNull(iEstadodescripcion)) entity.Estadodescripcion = dr.GetString(iEstadodescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosBusqueda(AudAuditoriaDTO auditoria)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroBusqueda, auditoria.Audiactivo, auditoria.AnioVigencia, auditoria.Audinombre);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<AudAuditoriaDTO> VerResultados(AudAuditoriaDTO auditoria)
        {
            List<AudAuditoriaDTO> entitys = new List<AudAuditoriaDTO>();

            string query = string.Format(helper.SqlVerResultados, auditoria.Audicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudAuditoriaDTO entity = new AudAuditoriaDTO();

                    int iProcdescripcion = dr.GetOrdinal(helper.Procdescripcion);
                    if (!dr.IsDBNull(iProcdescripcion)) entity.Procdescripcion = dr.GetString(iProcdescripcion);

                    int iTabcddescripcion = dr.GetOrdinal(helper.Tabcddescripcion);
                    if (!dr.IsDBNull(iTabcddescripcion)) entity.Tabcddescripcion = dr.GetString(iTabcddescripcion);

                    int iTabcdcodi = dr.GetOrdinal(helper.Tabcdcodi);
                    if (!dr.IsDBNull(iTabcdcodi)) entity.Tabcdcodi = Convert.ToInt32(dr.GetValue(iTabcdcodi));
                    
                    int iProgahdescripcion = dr.GetOrdinal(helper.Progahdescripcion);
                    if (!dr.IsDBNull(iProgahdescripcion)) entity.Progahdescripcion = dr.GetString(iProgahdescripcion);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodigo)) entity.Elemcodigo = dr.GetString(iElemcodigo);

                    int iProgahaccionmejora = dr.GetOrdinal(helper.Progahaccionmejora);
                    if (!dr.IsDBNull(iProgahaccionmejora)) entity.Progahaccionmejora = dr.GetString(iProgahaccionmejora);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosBusquedaResultados(int audicodi)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroBusquedaResultados, audicodi);

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
