using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_OBSERVACION
    /// </summary>
    public class TrObservacionRepository: RepositoryBase, ITrObservacionRepository
    {
        public TrObservacionRepository(string strConn): base(strConn)
        {
        }

        TrObservacionHelper helper = new TrObservacionHelper();

        public int Save(TrObservacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Obscanusucreacion, DbType.String, entity.Obscanusucreacion);
            dbProvider.AddInParameter(command, helper.Obscanusumodificacion, DbType.String, entity.Obscanusumodificacion);
            dbProvider.AddInParameter(command, helper.Obscanfeccreacion, DbType.DateTime, entity.Obscanfeccreacion);
            dbProvider.AddInParameter(command, helper.Obscanfecmodificacion, DbType.DateTime, entity.Obscanfecmodificacion);
            dbProvider.AddInParameter(command, helper.Obscanestado, DbType.String, entity.Obscanestado);
            dbProvider.AddInParameter(command, helper.Obscancomentario, DbType.String, entity.Obscancomentario);
            dbProvider.AddInParameter(command, helper.Obscantipo, DbType.String, entity.Obscantipo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);            

            #region "FIT - Señales no Disponibles Save"

            dbProvider.AddInParameter(command, helper.Obscanproceso, DbType.String, entity.Obscanproceso);
            dbProvider.AddInParameter(command, helper.Obscancomentarioagente, DbType.String, entity.Obscancomentarioagente);

            #endregion

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrObservacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Obscanusucreacion, DbType.String, entity.Obscanusucreacion);
            dbProvider.AddInParameter(command, helper.Obscanusumodificacion, DbType.String, entity.Obscanusumodificacion);
            //dbProvider.AddInParameter(command, helper.Obscanfeccreacion, DbType.DateTime, entity.Obscanfeccreacion);
            dbProvider.AddInParameter(command, helper.Obscanfecmodificacion, DbType.DateTime, entity.Obscanfecmodificacion);
            //dbProvider.AddInParameter(command, helper.Obscanestado, DbType.String, entity.Obscanestado);
            dbProvider.AddInParameter(command, helper.Obscancomentario, DbType.String, entity.Obscancomentario);
            dbProvider.AddInParameter(command, helper.Obscantipo, DbType.String, entity.Obscantipo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Obscancomentarioagente, DbType.String, entity.Obscancomentarioagente);
            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, entity.Obscancodi);            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int obscancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, obscancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrObservacionDTO GetById(int obscancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, obscancodi);
            TrObservacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrObservacionDTO> List()
        {
            List<TrObservacionDTO> entitys = new List<TrObservacionDTO>();
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

        public List<TrObservacionDTO> GetByCriteria(int empresa, DateTime fechaInicio, DateTime fechaFin)
        {
            List<TrObservacionDTO> entitys = new List<TrObservacionDTO>();
            string query = string.Format(helper.SqlGetByCriteria, empresa, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrObservacionDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ScEmpresaDTO> ObtenerEmpresasScada()
        {
            List<ScEmpresaDTO> entitys = new List<ScEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasScada);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ScEmpresaDTO entity = new ScEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprenomb = dr.GetString(iEmprnomb);

                    int iEmprcodisic = dr.GetOrdinal(helper.Emprcodisic);
                    if (!dr.IsDBNull(iEmprcodisic)) entity.Emprcodisic = Convert.ToInt32(dr.GetValue(iEmprcodisic));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrZonaSp7DTO> ObtenerZonasPorEmpresa(int emprcodi)
        {
            List<TrZonaSp7DTO> entitys = new List<TrZonaSp7DTO>();
            string query = string.Format(helper.SqlObtenerZonasPorEmpresa, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrZonaSp7DTO entity = new TrZonaSp7DTO();

                    int iZonacodi = dr.GetOrdinal(helper.Zonacodi);
                    if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

                    int iZonanomb = dr.GetOrdinal(helper.Zonanomb);
                    if(!dr.IsDBNull(iZonanomb))entity.Zonanomb = dr.GetString(iZonanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCanalSp7DTO> ObtenerCanalesObservacion(int empresa, int zona, string tipo, string descripcion, int nroPagina, int nroFilas)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            string query = string.Format(helper.SqlObtenerCanales, empresa, zona, tipo, descripcion, nroPagina, nroFilas);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iCanalabrev = dr.GetOrdinal(helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iCanalpointtype = dr.GetOrdinal(helper.Canalpointtype);
                    if (!dr.IsDBNull(iCanalpointtype)) entity.Canaltipo = dr.GetString(iCanalpointtype);

                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);

                    int iZonanomb = dr.GetOrdinal(helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;        
        }

        public int ObtenerNroFilasBusquedaCanal(int empresa, int zona, string tipo, string descripcion)
        {
            String query = String.Format(helper.SqlObtenerNroFilaBusquedaCanal, empresa, zona, tipo, descripcion);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<TrCanalSp7DTO> ObtenerCanalesPorCodigos(string canales)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();

            String query = String.Format(helper.SqlObtenerCanalesPorCodigo, canales);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iCanalabrev = dr.GetOrdinal(helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iCanalpointtype = dr.GetOrdinal(helper.Canalpointtype);
                    if (!dr.IsDBNull(iCanalpointtype)) entity.Canaltipo = dr.GetString(iCanalpointtype);

                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);

                    int iZonanomb = dr.GetOrdinal(helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;        

        }

        public void ActualizarEstado(int idObservacion, string estado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEstado);

            dbProvider.AddInParameter(command, helper.Obscanestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, idObservacion);

            dbProvider.ExecuteNonQuery(command);        
        }

        public TrObservacionDTO ObtenerEmpresa(int idEmpresa)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            TrObservacionDTO entity = null;

            using(IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new TrObservacionDTO();
                    
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                }
            }

            return entity;
           
        }

        #region "FIT - Señales no Disponibles" 

        public int ObtenerNroFilasBusquedaCanalSenalesObservadasBusqueda(int empresa)
        {
            DateTime fecha = new DateTime();
            fecha = DateTime.Now.AddDays(-1);

            String query = String.Format(helper.SqlObtenerNroFilaBusquedaCanalSenalesObservadasBusqueda, empresa, fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }


        public List<TrCanalSp7DTO> ObtenerCanalesObservacionSenalesObservadasBusqueda(int empresa, int nroPagina, int nroFilas)
        {
            DateTime fecha = new DateTime();
            fecha = DateTime.Now.AddDays(-1);

            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            string query = string.Format(helper.SqlObtenerCanalesSenalesObservadasBusqueda, empresa, fecha.ToString(ConstantesBase.FormatoFecha), nroPagina, nroFilas);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iCanalabrev = dr.GetOrdinal(helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iCanalpointtype = dr.GetOrdinal(helper.Canalpointtype);
                    if (!dr.IsDBNull(iCanalpointtype)) entity.Canaltipo = dr.GetString(iCanalpointtype);

                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);

                    int iZonanomb = dr.GetOrdinal(helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    //-- Agregados MOTIVO, TIEMPO, CALIDAD, CAIDA

                    int iMotivo = dr.GetOrdinal(helper.Motivo);
                    if (!dr.IsDBNull(iMotivo)) entity.Motivo = dr.GetString(iMotivo);

                    //int iTiempo = dr.GetOrdinal(helper.Tiempo);
                    //if (!dr.IsDBNull(iTiempo)) entity.Tiempo = dr.GetString(iTiempo);

                    //int iCalidad = dr.GetOrdinal(helper.Calidad);
                    //if (!dr.IsDBNull(iCalidad)) entity.Calidad = dr.GetString(iCalidad);

                    //int iCaida = dr.GetOrdinal(helper.Caida);
                    //if (!dr.IsDBNull(iCaida)) entity.Caida = dr.GetString(iCaida);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCanalSp7DTO> ObtenerSenalesObservadas(int empresa)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();            

            string query = string.Format(helper.SqlObtenerSenalesObservadas, empresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iCanalabrev = dr.GetOrdinal(helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iCanalpointtype = dr.GetOrdinal(helper.Canalpointtype);
                    if (!dr.IsDBNull(iCanalpointtype)) entity.Canaltipo = dr.GetString(iCanalpointtype);

                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);

                    int iZonanomb = dr.GetOrdinal(helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);
                    
                    int iMotivo = dr.GetOrdinal(helper.Motivo);
                    if (!dr.IsDBNull(iMotivo)) entity.Motivo = dr.GetString(iMotivo);

                    int iCanalhora = dr.GetOrdinal(helper.Canalfhora);
                    if (!dr.IsDBNull(iCanalhora)) entity.Canalfhora = dr.GetDateTime(iCanalhora);

                    int iCanalfhora2 = dr.GetOrdinal(helper.Canalfhora2);
                    if (!dr.IsDBNull(iCanalfhora2)) entity.Canalfhora2 = dr.GetDateTime(iCanalfhora2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }



        public List<TrCanalSp7DTO> ObtenerSenalesObservadasReportadas(int empresa)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();

            string query = string.Format(helper.SqlObtenerSenalesObservadasReportadas, empresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

    }
}
