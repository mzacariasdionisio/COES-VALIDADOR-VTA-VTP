using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla AUD_ELEMENTO
    /// </summary>
    public class AudElementoRepository: RepositoryBase, IAudElementoRepository
    {
        public AudElementoRepository(string strConn): base(strConn)
        {
        }

        AudElementoHelper helper = new AudElementoHelper();
        AreaHelper areaHelper = new AreaHelper();

        public int Save(AudElementoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Elemcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipoelemento, DbType.Int32, entity.Tabcdcoditipoelemento);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);
            dbProvider.AddInParameter(command, helper.Elemcodigo, DbType.String, entity.Elemcodigo);
            dbProvider.AddInParameter(command, helper.Elemdescripcion, DbType.String, entity.Elemdescripcion);
            dbProvider.AddInParameter(command, helper.Elemactivo, DbType.String, entity.Elemactivo);
            dbProvider.AddInParameter(command, helper.Elemhistorico, DbType.String, entity.Elemhistorico);
            dbProvider.AddInParameter(command, helper.Elemusucreacion, DbType.String, entity.Elemusucreacion);
            dbProvider.AddInParameter(command, helper.Elemfeccreacion, DbType.DateTime, entity.Elemfeccreacion);
            dbProvider.AddInParameter(command, helper.Elemusumodificacion, DbType.String, entity.Elemusumodificacion);
            dbProvider.AddInParameter(command, helper.Elemfecmodificacion, DbType.DateTime, entity.Elemfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudElementoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tabcdcoditipoelemento, DbType.Int32, entity.Tabcdcoditipoelemento);
            dbProvider.AddInParameter(command, helper.Proccodi, DbType.Int32, entity.Proccodi);
            dbProvider.AddInParameter(command, helper.Elemcodigo, DbType.String, entity.Elemcodigo);
            dbProvider.AddInParameter(command, helper.Elemdescripcion, DbType.String, entity.Elemdescripcion);
            dbProvider.AddInParameter(command, helper.Elemactivo, DbType.String, entity.Elemactivo);
            dbProvider.AddInParameter(command, helper.Elemhistorico, DbType.String, entity.Elemhistorico);
            dbProvider.AddInParameter(command, helper.Elemusumodificacion, DbType.String, entity.Elemusumodificacion);
            dbProvider.AddInParameter(command, helper.Elemfecmodificacion, DbType.DateTime, entity.Elemfecmodificacion);
            dbProvider.AddInParameter(command, helper.Elemcodi, DbType.Int32, entity.Elemcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudElementoDTO elemento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Elemusumodificacion, DbType.String, elemento.Elemusumodificacion);
            dbProvider.AddInParameter(command, helper.Elemfecmodificacion, DbType.DateTime, elemento.Elemfecmodificacion);
            dbProvider.AddInParameter(command, helper.Elemcodi, DbType.Int32, elemento.Elemcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudElementoDTO GetById(int elemcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Elemcodi, DbType.Int32, elemcodi);
            AudElementoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iAreacodi = dr.GetOrdinal(helper.AreaCodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iExisteprogaudielemento = dr.GetOrdinal(helper.Existeprogaudielemento);
                    if (!dr.IsDBNull(iExisteprogaudielemento)) entity.Existeprogaudielemento = Convert.ToInt32(dr.GetValue(iExisteprogaudielemento));
                }
            }

            return entity;
        }

        public List<AudElementoDTO> List()
        {
            List<AudElementoDTO> entitys = new List<AudElementoDTO>();
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

        public List<AudElementoDTO> GetByCriteria(AudElementoDTO audElementoDTO)
        {
            List<AudElementoDTO> entitys = new List<AudElementoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, audElementoDTO.Tabcdcoditipoelemento, audElementoDTO.Elemactivo,  audElementoDTO.Proccodi.HasValue? audElementoDTO.Proccodi : 0, audElementoDTO.Areacodi, audElementoDTO.nroPagina, audElementoDTO.nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudElementoDTO entity = helper.Create(dr);
                    int iProcDescripcion = dr.GetOrdinal("ProcDescripcion");
                    if (!dr.IsDBNull(iProcDescripcion)) entity.Procdescripcion = dr.GetString(iProcDescripcion);

                    int iTipoElemento = dr.GetOrdinal("TipoElemento");
                    if (!dr.IsDBNull(iTipoElemento)) entity.TipoElemento = dr.GetString(iTipoElemento);
                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosBusqueda(AudElementoDTO audElementoDTO)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroBusqueda, audElementoDTO.Tabcdcoditipoelemento, audElementoDTO.Elemactivo, audElementoDTO.Proccodi.HasValue? audElementoDTO.Proccodi:0, audElementoDTO.Areacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<SiAreaDTO>GetByAreaElemento()
        {
            List<SiAreaDTO> entitys = new List<SiAreaDTO>();
            string sql = string.Format(helper.SqlGetByElementoPorArea);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiAreaDTO entity = new SiAreaDTO();
                    int iAreacodi = dr.GetOrdinal("Areacodi");
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));
                    int iAreanom = dr.GetOrdinal("Areanomb");
                    if (!dr.IsDBNull(iAreanom)) entity.Areanomb = dr.GetString(iAreanom);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AudProcesoDTO> GetByProcesoElemento()
        {
            List<AudProcesoDTO> entitys = new List<AudProcesoDTO>();
            string sql = string.Format(helper.SqlGetByProcesoElemento);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProcesoDTO entity = new AudProcesoDTO();
                    int iProccodigo = dr.GetOrdinal("Proccodigo");
                    if (!dr.IsDBNull(iProccodigo)) entity.Proccodigo = dr.GetString(iProccodigo);
                    int iProcdescripcion = dr.GetOrdinal("Procdescripcion");
                    if (!dr.IsDBNull(iProcdescripcion)) entity.Procdescripcion = dr.GetString(iProcdescripcion);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AudElementoDTO> GetByProcesoPorElemento(AudElementoDTO audElementoDTO)
        {
            List<AudElementoDTO> entitys = new List<AudElementoDTO>();
            string sql = string.Format(helper.SqlGetByProcesoPorElemento,audElementoDTO.Tabcdcoditipoelemento);
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

        public List<AudElementoDTO> GetByElementosPorProceso(int proccodi)
        {
            List<AudElementoDTO> entitys = new List<AudElementoDTO>();

            string sql = string.Format(helper.SqlGetByElementosPorProceso, proccodi);

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

        public List<AudElementoDTO> GetByElementosPorProcesoAP(int plancodi, string procesos)
        {
            List<AudElementoDTO> entitys = new List<AudElementoDTO>();

            string sql = string.Format(helper.SqlGetByElementosPorProcesoAP, plancodi, procesos);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudElementoDTO elemento = helper.Create(dr);

                    int iAudppcodi = dr.GetOrdinal(helper.Audppcodi);
                    if (!dr.IsDBNull(iAudppcodi)) elemento.Audppcodi = Convert.ToInt32(dr.GetValue(iAudppcodi));
                    
                    entitys.Add(elemento);
                }
            }

            return entitys;
        }
        
        public List<AudElementoDTO> GetByElementosPorTipo(int tipoelemento)
        {
            List<AudElementoDTO> entitys = new List<AudElementoDTO>();

            string sql = string.Format(helper.SqlGetByElementosPorTipo, tipoelemento);

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

        public List<AudElementoDTO> GetByElementosPorProcesoTipo(string procesos, int tipoelemento)
        {
            List<AudElementoDTO> entitys = new List<AudElementoDTO>();

            string sql = string.Format(helper.SqlGetByElementosPorProcesoTipo, tipoelemento, procesos);

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

        public string GetByElementoValidacion(int elemcodi)
        {
            string validacionMensaje = string.Empty;

            string query = string.Format(helper.SqlGetByElementoValidacion, elemcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iValidacionmensaje = dr.GetOrdinal(helper.Validacionmensaje);
                    if (!dr.IsDBNull(iValidacionmensaje)) validacionMensaje = dr.GetString(iValidacionmensaje);
                }
            }

            return validacionMensaje;
        }
    }
}
