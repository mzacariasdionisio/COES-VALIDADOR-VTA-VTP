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
    /// Clase de acceso a datos de la tabla EPO_ESTUDIO_EO
    /// </summary>
    public class EpoEstudioEoRepository: RepositoryBase, IEpoEstudioEoRepository
    {
        string sConn = string.Empty;
        public EpoEstudioEoRepository(string strConn): base(strConn)
        {
            sConn = strConn;
        }

        EpoEstudioEoHelper helper = new EpoEstudioEoHelper();

        public int Save(EpoEstudioEoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Esteofechaopecomercial, DbType.DateTime, entity.Esteofechaopecomercial);
            dbProvider.AddInParameter(command, helper.Esteofechaintegracion, DbType.DateTime, entity.Esteofechaintegracion);
            dbProvider.AddInParameter(command, helper.Esteofechaconexion, DbType.DateTime, entity.Esteofechaconexion);
            dbProvider.AddInParameter(command, helper.Estacodi, DbType.Int32, entity.Estacodi);
            dbProvider.AddInParameter(command, helper.Esteocodiusu, DbType.String, entity.Esteocodiusu);
            dbProvider.AddInParameter(command, helper.Esteonomb, DbType.String, entity.Esteonomb);
            dbProvider.AddInParameter(command, helper.Emprcoditp, DbType.Int32, entity.Emprcoditp);
            dbProvider.AddInParameter(command, helper.Emprcoditi, DbType.Int32, entity.Emprcoditi);
            dbProvider.AddInParameter(command, helper.Esteopotencia, DbType.String, entity.Esteopotencia);
            dbProvider.AddInParameter(command, helper.Esteocapacidad, DbType.String, entity.Esteocapacidad);
            dbProvider.AddInParameter(command, helper.Esteocarga, DbType.String, entity.Esteocarga);
            dbProvider.AddInParameter(command, helper.Esteopotenciarer, DbType.String, entity.Esteopotenciarer);
            dbProvider.AddInParameter(command, helper.Esteopuntoconexion, DbType.String, entity.Esteopuntoconexion);
            dbProvider.AddInParameter(command, helper.Esteoanospuestaservicio, DbType.String, entity.Esteoanospuestaservicio);
            dbProvider.AddInParameter(command, helper.Esteootros, DbType.String, entity.Esteootros);
            dbProvider.AddInParameter(command, helper.Esteoobs, DbType.String, entity.Esteoobs);
            dbProvider.AddInParameter(command, helper.Esteofechaini, DbType.DateTime, entity.Esteofechaini);
            dbProvider.AddInParameter(command, helper.Esteoresumenejecutivotit, DbType.String, entity.Esteoresumenejecutivotit);
            dbProvider.AddInParameter(command, helper.Esteoresumenejecutivoenl, DbType.String, entity.Esteoresumenejecutivoenl);
            dbProvider.AddInParameter(command, helper.Esteofechafin, DbType.DateTime, entity.Esteofechafin);
            dbProvider.AddInParameter(command, helper.Esteocertconformidadtit, DbType.String, entity.Esteocertconformidadtit);
            dbProvider.AddInParameter(command, helper.Esteocertconformidadenl, DbType.String, entity.Esteocertconformidadenl);
            dbProvider.AddInParameter(command, helper.Esteoplazorevcoesporv, DbType.Int32, entity.Esteoplazorevcoesporv);
            dbProvider.AddInParameter(command, helper.Esteoplazorevcoesvenc, DbType.Int32, entity.Esteoplazorevcoesvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazolevobsporv, DbType.Int32, entity.Esteoplazolevobsporv);
            dbProvider.AddInParameter(command, helper.Esteoplazolevobsvenc, DbType.Int32, entity.Esteoplazolevobsvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazoalcancesvenc, DbType.Int32, entity.Esteoplazoalcancesvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazoverificacionvenc, DbType.Int32, entity.Esteoplazoverificacionvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazorevterinvporv, DbType.Int32, entity.Esteoplazorevterinvporv);
            dbProvider.AddInParameter(command, helper.Esteoplazorevterinvvenc, DbType.Int32, entity.Esteoplazorevterinvvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazoenvestterinvporv, DbType.Int32, entity.Esteoplazoenvestterinvporv);
            dbProvider.AddInParameter(command, helper.Esteoplazoenvestterinvvenc, DbType.Int32, entity.Esteoplazoenvestterinvvenc);
            dbProvider.AddInParameter(command, helper.Esteoalcancefechaini, DbType.DateTime, entity.Esteoalcancefechaini);
            dbProvider.AddInParameter(command, helper.Esteoalcancesolesttit, DbType.String, entity.Esteoalcancesolesttit);
            dbProvider.AddInParameter(command, helper.Esteoalcancesolestenl, DbType.String, entity.Esteoalcancesolestenl);
            dbProvider.AddInParameter(command, helper.Esteoalcancesolestobs, DbType.String, entity.Esteoalcancesolestobs);
            dbProvider.AddInParameter(command, helper.Esteoalcancefechafin, DbType.DateTime, entity.Esteoalcancefechafin);
            dbProvider.AddInParameter(command, helper.Esteoalcanceenviotit, DbType.String, entity.Esteoalcanceenviotit);
            dbProvider.AddInParameter(command, helper.Esteoalcanceenvioenl, DbType.String, entity.Esteoalcanceenvioenl);
            dbProvider.AddInParameter(command, helper.Esteoalcanceenvioobs, DbType.String, entity.Esteoalcanceenvioobs);
            dbProvider.AddInParameter(command, helper.Esteoverifechaini, DbType.DateTime, entity.Esteoverifechaini);
            dbProvider.AddInParameter(command, helper.Esteoverientregaesttit, DbType.String, entity.Esteoverientregaesttit);
            dbProvider.AddInParameter(command, helper.Esteoverientregaestenl, DbType.String, entity.Esteoverientregaestenl);
            dbProvider.AddInParameter(command, helper.Esteoverientregaestobs, DbType.String, entity.Esteoverientregaestobs);
            dbProvider.AddInParameter(command, helper.Esteoverifechafin, DbType.DateTime, entity.Esteoverifechafin);
            dbProvider.AddInParameter(command, helper.Esteovericartatit, DbType.String, entity.Esteovericartatit);
            dbProvider.AddInParameter(command, helper.Esteovericartaenl, DbType.String, entity.Esteovericartaenl);
            dbProvider.AddInParameter(command, helper.Esteovericartaobs, DbType.String, entity.Esteovericartaobs);
            dbProvider.AddInParameter(command, helper.Esteopuestaenservfecha, DbType.DateTime, entity.Esteopuestaenservfecha);
            dbProvider.AddInParameter(command, helper.Esteopuestaenservcomentario, DbType.String, entity.Esteopuestaenservcomentario);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Esteojustificacion, DbType.String, entity.Esteojustificacion);
            dbProvider.AddInParameter(command, helper.Esteoacumdiascoes, DbType.Int32, entity.Esteoacumdiascoes);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Esteocodiproy, DbType.String, entity.Esteocodiproy);
            dbProvider.AddInParameter(command, helper.Esteoresponsable, DbType.String, entity.Esteoresponsable);
            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.String, entity.PuntCodi);
            dbProvider.AddInParameter(command, helper.EsteoTipoProyecto, DbType.String, entity.EsteoTipoProyecto);
            dbProvider.AddInParameter(command, helper.Tipoconfig, DbType.Int32, entity.TipoConfig);

            dbProvider.AddInParameter(command, helper.EsteoAbsTit, DbType.String, entity.EsteoAbsTit);
            dbProvider.AddInParameter(command, helper.EsteoAbsEnl, DbType.String, entity.EsteoAbsEnl);
            dbProvider.AddInParameter(command, helper.EsteoAbsFFin, DbType.DateTime, entity.EsteoAbsFFin);
            dbProvider.AddInParameter(command, helper.EsteoAbsObs, DbType.String, entity.EsteoAbsObs);
            dbProvider.AddInParameter(command, helper.Esteoplazoverificacionvencabs, DbType.Int32, entity.Esteoplazoverificacionvencabs);




            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoEstudioEoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Esteofechaopecomercial, DbType.DateTime, entity.Esteofechaopecomercial);
            dbProvider.AddInParameter(command, helper.Esteofechaintegracion, DbType.DateTime, entity.Esteofechaintegracion);
            dbProvider.AddInParameter(command, helper.Esteofechaconexion, DbType.DateTime, entity.Esteofechaconexion);
            
            dbProvider.AddInParameter(command, helper.Estacodi, DbType.Int32, entity.Estacodi);
            dbProvider.AddInParameter(command, helper.Esteocodiusu, DbType.String, entity.Esteocodiusu);
            dbProvider.AddInParameter(command, helper.Esteonomb, DbType.String, entity.Esteonomb);
            dbProvider.AddInParameter(command, helper.Emprcoditp, DbType.Int32, entity.Emprcoditp);
            dbProvider.AddInParameter(command, helper.Emprcoditi, DbType.Int32, entity.Emprcoditi);
            dbProvider.AddInParameter(command, helper.Esteopotencia, DbType.String, entity.Esteopotencia);
            dbProvider.AddInParameter(command, helper.Esteocapacidad, DbType.String, entity.Esteocapacidad);
            dbProvider.AddInParameter(command, helper.Esteocarga, DbType.String, entity.Esteocarga);
            dbProvider.AddInParameter(command, helper.Esteopotenciarer, DbType.String, entity.Esteopotenciarer);
            dbProvider.AddInParameter(command, helper.Esteopuntoconexion, DbType.String, entity.Esteopuntoconexion);
            dbProvider.AddInParameter(command, helper.Esteoanospuestaservicio, DbType.String, entity.Esteoanospuestaservicio);
            dbProvider.AddInParameter(command, helper.Esteootros, DbType.String, entity.Esteootros);
            dbProvider.AddInParameter(command, helper.Esteoobs, DbType.String, entity.Esteoobs);
            dbProvider.AddInParameter(command, helper.Esteofechaini, DbType.DateTime, entity.Esteofechaini);
            dbProvider.AddInParameter(command, helper.Esteoresumenejecutivotit, DbType.String, entity.Esteoresumenejecutivotit);
            dbProvider.AddInParameter(command, helper.Esteoresumenejecutivoenl, DbType.String, entity.Esteoresumenejecutivoenl);
            dbProvider.AddInParameter(command, helper.Esteofechafin, DbType.DateTime, entity.Esteofechafin);
            dbProvider.AddInParameter(command, helper.Esteocertconformidadtit, DbType.String, entity.Esteocertconformidadtit);
            dbProvider.AddInParameter(command, helper.Esteocertconformidadenl, DbType.String, entity.Esteocertconformidadenl);
            dbProvider.AddInParameter(command, helper.Esteoplazorevcoesporv, DbType.Int32, entity.Esteoplazorevcoesporv);
            dbProvider.AddInParameter(command, helper.Esteoplazorevcoesvenc, DbType.Int32, entity.Esteoplazorevcoesvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazolevobsporv, DbType.Int32, entity.Esteoplazolevobsporv);
            dbProvider.AddInParameter(command, helper.Esteoplazolevobsvenc, DbType.Int32, entity.Esteoplazolevobsvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazoalcancesvenc, DbType.Int32, entity.Esteoplazoalcancesvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazoverificacionvenc, DbType.Int32, entity.Esteoplazoverificacionvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazorevterinvporv, DbType.Int32, entity.Esteoplazorevterinvporv);
            dbProvider.AddInParameter(command, helper.Esteoplazorevterinvvenc, DbType.Int32, entity.Esteoplazorevterinvvenc);
            dbProvider.AddInParameter(command, helper.Esteoplazoenvestterinvporv, DbType.Int32, entity.Esteoplazoenvestterinvporv);
            dbProvider.AddInParameter(command, helper.Esteoplazoenvestterinvvenc, DbType.Int32, entity.Esteoplazoenvestterinvvenc);
            dbProvider.AddInParameter(command, helper.Esteoalcancefechaini, DbType.DateTime, entity.Esteoalcancefechaini);
            dbProvider.AddInParameter(command, helper.Esteoalcancesolesttit, DbType.String, entity.Esteoalcancesolesttit);
            dbProvider.AddInParameter(command, helper.Esteoalcancesolestenl, DbType.String, entity.Esteoalcancesolestenl);
            dbProvider.AddInParameter(command, helper.Esteoalcancesolestobs, DbType.String, entity.Esteoalcancesolestobs);
            dbProvider.AddInParameter(command, helper.Esteoalcancefechafin, DbType.DateTime, entity.Esteoalcancefechafin);
            dbProvider.AddInParameter(command, helper.Esteoalcanceenviotit, DbType.String, entity.Esteoalcanceenviotit);
            dbProvider.AddInParameter(command, helper.Esteoalcanceenvioenl, DbType.String, entity.Esteoalcanceenvioenl);
            dbProvider.AddInParameter(command, helper.Esteoalcanceenvioobs, DbType.String, entity.Esteoalcanceenvioobs);
            dbProvider.AddInParameter(command, helper.Esteoverifechaini, DbType.DateTime, entity.Esteoverifechaini);
            dbProvider.AddInParameter(command, helper.Esteoverientregaesttit, DbType.String, entity.Esteoverientregaesttit);
            dbProvider.AddInParameter(command, helper.Esteoverientregaestenl, DbType.String, entity.Esteoverientregaestenl);
            dbProvider.AddInParameter(command, helper.Esteoverientregaestobs, DbType.String, entity.Esteoverientregaestobs);
            dbProvider.AddInParameter(command, helper.Esteoverifechafin, DbType.DateTime, entity.Esteoverifechafin);
            dbProvider.AddInParameter(command, helper.Esteovericartatit, DbType.String, entity.Esteovericartatit);
            dbProvider.AddInParameter(command, helper.Esteovericartaenl, DbType.String, entity.Esteovericartaenl);
            dbProvider.AddInParameter(command, helper.Esteovericartaobs, DbType.String, entity.Esteovericartaobs);
            dbProvider.AddInParameter(command, helper.Esteopuestaenservfecha, DbType.DateTime, entity.Esteopuestaenservfecha);
            dbProvider.AddInParameter(command, helper.Esteopuestaenservcomentario, DbType.String, entity.Esteopuestaenservcomentario);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Esteojustificacion, DbType.String, entity.Esteojustificacion);
            dbProvider.AddInParameter(command, helper.Esteoacumdiascoes, DbType.Int32, entity.Esteoacumdiascoes);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Esteocodiproy, DbType.String, entity.Esteocodiproy);
            dbProvider.AddInParameter(command, helper.Esteoresponsable, DbType.String, entity.Esteoresponsable);
            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.String, entity.PuntCodi);
            dbProvider.AddInParameter(command, helper.EsteoTipoProyecto, DbType.String, entity.EsteoTipoProyecto);
            dbProvider.AddInParameter(command, helper.Tipoconfig, DbType.Int32, entity.TipoConfig);

            dbProvider.AddInParameter(command, helper.EsteoAbsTit, DbType.String, entity.EsteoAbsTit);
            dbProvider.AddInParameter(command, helper.EsteoAbsEnl, DbType.String, entity.EsteoAbsEnl);
            dbProvider.AddInParameter(command, helper.EsteoAbsFFin, DbType.DateTime, entity.EsteoAbsFFin);
            dbProvider.AddInParameter(command, helper.EsteoAbsObs, DbType.String, entity.EsteoAbsObs);
            dbProvider.AddInParameter(command, helper.Esteoplazoverificacionvencabs, DbType.Int32, entity.Esteoplazoverificacionvencabs);


            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, entity.Esteocodi);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int esteocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, esteocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoEstudioEoDTO GetById(int esteocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, esteocodi);
            EpoEstudioEoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEsteoresponsable = dr.GetOrdinal(helper.Esteoresponsable);
                    if (!dr.IsDBNull(iEsteoresponsable)) entity.Esteoresponsable = dr.GetString(iEsteoresponsable);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTerceroinvolucrado = dr.GetOrdinal(helper.Terceroinvolucrado);
                    if (!dr.IsDBNull(iTerceroinvolucrado)) entity.Terceroinvolucrado = dr.GetString(iTerceroinvolucrado);

                    int iEstadescripcion = dr.GetOrdinal(helper.Estadescripcion);
                    if (!dr.IsDBNull(iEstadescripcion)) entity.Estadescripcion = dr.GetString(iEstadescripcion);

                    int iTipoConfig = dr.GetOrdinal(helper.Tipoconfig);
                    if (!dr.IsDBNull(iTipoConfig)) entity.TipoConfig = Convert.ToInt32(dr.GetValue(iTipoConfig));

                    #region Mejoras EO-EPO-II
                    int iZonDescripcion = dr.GetOrdinal(helper.ZonDescripcion);
                    if (!dr.IsDBNull(iZonDescripcion)) entity.ZonDescripcion = dr.GetString(iZonDescripcion);
                    #endregion
                }
            }

            return entity;
        }

        public List<EpoEstudioEoDTO> List()
        {
            List<EpoEstudioEoDTO> entitys = new List<EpoEstudioEoDTO>();
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

        public List<EpoEstudioEoDTO> GetByCriteria(EpoEstudioEoDTO estudioeo)
        {
            string sRangoPresentacion = (estudioeo.FIniPresentacion != null & estudioeo.FFinPresentacion != null) ? " eo.esteofechaini BETWEEN TO_DATE('" + estudioeo.FIniPresentacion + "','dd/mm/yyyy')  AND  TO_DATE('" + estudioeo.FFinPresentacion + "','dd/mm/yyyy') " : " 1=1 ";
            string sRangoConformidad = (estudioeo.FIniConformidad != null & estudioeo.FFinConformidad != null) ? " eo.esteofechafin  BETWEEN  TO_DATE('" + estudioeo.FIniConformidad + "','dd/mm/yyyy')  AND  TO_DATE('" + estudioeo.FFinConformidad + "','dd/mm/yyyy') " : " 1=1 ";


            if (estudioeo.PuntCodi == null) estudioeo.PuntCodi = 0; // para el case del multiselect viene con null;
            string sql = string.Format(helper.SqlGetByCriteria, sRangoPresentacion, sRangoConformidad
                                                              , estudioeo.Estacodi, estudioeo.Emprcoditp, estudioeo.Esteonomb
                                                              , estudioeo.PuntCodi, estudioeo.Esteocodiproy
                                                              , estudioeo.nroPagina, estudioeo.nroFilas, estudioeo.Esteoanospuestaservicio, estudioeo.EsteoTipoProyecto, estudioeo.Zoncodi);

            List<EpoEstudioEoDTO> entitys = new List<EpoEstudioEoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioEoDTO entity = new EpoEstudioEoDTO();

                    int iEstepocodi = dr.GetOrdinal(helper.Esteocodi);
                    if (!dr.IsDBNull(iEstepocodi)) entity.Esteocodi = Convert.ToInt32(dr.GetValue(iEstepocodi));

                    int iEstepocodiusu = dr.GetOrdinal(helper.Esteocodiusu);
                    if (!dr.IsDBNull(iEstepocodiusu)) entity.Esteocodiusu = dr.GetString(iEstepocodiusu);

                    int iEsteponomb = dr.GetOrdinal(helper.Esteonomb);
                    if (!dr.IsDBNull(iEsteponomb)) entity.Esteonomb = dr.GetString(iEsteponomb);

                    int iEstepofechaini = dr.GetOrdinal(helper.Esteofechaini);
                    if (!dr.IsDBNull(iEstepofechaini)) entity.Esteofechaini = dr.GetDateTime(iEstepofechaini);

                    int iEstepofechafin = dr.GetOrdinal(helper.Esteofechafin);
                    if (!dr.IsDBNull(iEstepofechafin)) entity.Esteofechafin = dr.GetDateTime(iEstepofechafin);

                    int iEstadescripcion = dr.GetOrdinal(helper.Estadescripcion);
                    if (!dr.IsDBNull(iEstadescripcion)) entity.Estadescripcion = dr.GetString(iEstadescripcion);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iTerceroinvolucrado = dr.GetOrdinal(helper.Terceroinvolucrado);
                    if (!dr.IsDBNull(iTerceroinvolucrado)) entity.Terceroinvolucrado = dr.GetString(iTerceroinvolucrado);

                    int iEsteoalcancesolestenl = dr.GetOrdinal(helper.Esteoalcancesolestenl);
                    if (!dr.IsDBNull(iEsteoalcancesolestenl)) entity.Esteoalcancesolestenl = dr.GetString(iEsteoalcancesolestenl);

                    int iEsteopotencia = dr.GetOrdinal(helper.Esteopotencia);
                    if (!dr.IsDBNull(iEsteopotencia)) entity.Esteopotencia = dr.GetString(iEsteopotencia);

                    int iEsteopuntoconexion = dr.GetOrdinal(helper.Esteopuntoconexion);
                    if (!dr.IsDBNull(iEsteopuntoconexion)) entity.Esteopuntoconexion = dr.GetString(iEsteopuntoconexion);

                    int iEsteoanospuestaservicio = dr.GetOrdinal(helper.Esteoanospuestaservicio);
                    if (!dr.IsDBNull(iEsteoanospuestaservicio)) entity.Esteoanospuestaservicio = dr.GetString(iEsteoanospuestaservicio);

                    int iEsteoobs = dr.GetOrdinal(helper.Esteoobs);
                    if (!dr.IsDBNull(iEsteoobs)) entity.Esteoobs = dr.GetString(iEsteoobs);

                    int iEsteoresumenejecutivoenl = dr.GetOrdinal(helper.Esteoresumenejecutivoenl);
                    if (!dr.IsDBNull(iEsteoresumenejecutivoenl)) entity.Esteoresumenejecutivoenl = dr.GetString(iEsteoresumenejecutivoenl);

                    int iEsteocodiproy = dr.GetOrdinal(helper.Esteocodiproy);
                    if (!dr.IsDBNull(iEsteocodiproy)) entity.Esteocodiproy = dr.GetString(iEsteocodiproy);

                    int iEsteofechaopecomercial = dr.GetOrdinal(helper.Esteofechaopecomercial);
                    if (!dr.IsDBNull(iEsteofechaopecomercial)) entity.Esteofechaopecomercial = dr.GetDateTime(iEsteofechaopecomercial);

                    int iEsteofechaconexion = dr.GetOrdinal(helper.Esteofechaconexion);
                    if (!dr.IsDBNull(iEsteofechaconexion)) entity.Esteofechaconexion = dr.GetDateTime(iEsteofechaconexion);

                    int iEsteofechaintegracion = dr.GetOrdinal(helper.Esteofechaintegracion);
                    if (!dr.IsDBNull(iEsteofechaintegracion)) entity.Esteofechaintegracion = dr.GetDateTime(iEsteofechaintegracion);

                    #region Mejoras EO-EPO
                    int iEsteoVigencia = dr.GetOrdinal(helper.EsteoVigencia);
                    if (!dr.IsDBNull(iEsteoVigencia)) entity.EsteoVigencia = dr.GetString(iEsteoVigencia);
                    #endregion
                    #region Mejoras EO-EPO-II
                    int iZonDescripcion = dr.GetOrdinal(helper.ZonDescripcion);
                    if (!dr.IsDBNull(iZonDescripcion)) entity.ZonDescripcion = dr.GetString(iZonDescripcion);
                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistroBusqueda(EpoEstudioEoDTO estudioeo)
        {

            //string sRangoPresentacion = (estudioeo.FechaIniPresentacion != null & estudioeo.FechaFinPresentacion != null) ? " epo.esteofechaini BETWEEN TO_DATE('" + estudioeo.FechaIniPresentacion.Value.ToString("yyyy/MM/dd") + "','YYYY-MM-DD')  AND  TO_DATE('" + estudioeo.FechaFinPresentacion.Value.ToString("yyyy/MM/dd") + "','YYYY-MM-DD') " : " 1=1 ";
            //string sRangoConformidad = (estudioeo.FechaIniConformidad != null & estudioeo.FechaFinConformidad != null) ? " epo.esteofechafin  BETWEEN  TO_DATE('" + estudioeo.FechaIniConformidad.Value.ToString("yyyy/MM/dd") + "','YYYY-MM-DD')  AND  TO_DATE('" + estudioeo.FechaFinConformidad.Value.ToString("yyyy/MM/dd") + "','YYYY-MM-DD') " : " 1=1 ";

            string sRangoPresentacion = (estudioeo.FIniPresentacion != null & estudioeo.FFinPresentacion != null) ? " eo.esteofechaini BETWEEN TO_DATE('" + estudioeo.FIniPresentacion + "','dd/mm/yyyy')  AND  TO_DATE('" + estudioeo.FFinPresentacion + "','dd/mm/yyyy') " : " 1=1 ";
            string sRangoConformidad = (estudioeo.FIniConformidad != null & estudioeo.FFinConformidad != null) ? " eo.esteofechafin  BETWEEN  TO_DATE('" + estudioeo.FIniConformidad + "','dd/mm/yyyy')  AND  TO_DATE('" + estudioeo.FFinConformidad + "','dd/mm/yyyy') " : " 1=1 ";

            if (estudioeo.PuntCodi == null) estudioeo.PuntCodi = 0; // para el case del multiselect viene con null;
            string sql = string.Format(helper.SqlNroRegistros, sRangoPresentacion, sRangoConformidad
                                                                         , estudioeo.Estacodi, estudioeo.Emprcoditp, estudioeo.Esteonomb
                                                                         , estudioeo.PuntCodi, estudioeo.Esteocodiproy
                                                                         , estudioeo.nroPagina, estudioeo.nroFilas, estudioeo.Esteoanospuestaservicio, estudioeo.EsteoTipoProyecto, estudioeo.Zoncodi);

            List<EpoEstudioEpoDTO> entitys = new List<EpoEstudioEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<EpoEstudioEoDTO> ListFwUser()
        {
            List<EpoEstudioEoDTO> entitys = new List<EpoEstudioEoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByFwUser);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioEoDTO entity = new EpoEstudioEoDTO();

                    int iEsteorespes = dr.GetOrdinal(helper.Esteorespes);
                    if (!dr.IsDBNull(iEsteorespes)) entity.Esteorespes = dr.GetString(iEsteorespes);

                    int iEsteorescodi = dr.GetOrdinal(helper.Esteorescodi);
                    if (!dr.IsDBNull(iEsteorescodi)) entity.Esteorescodi = Convert.ToInt32(dr.GetValue(iEsteorescodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #region Mejoras EO-EPO
        public List<EpoEstudioEoDTO> ListVigencia12Meses()
        {
            string sql = string.Format(helper.SqlListVigencia12Meses);

            List<EpoEstudioEoDTO> entitys = new List<EpoEstudioEoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioEoDTO entity = new EpoEstudioEoDTO();

                    int iEsteonomb = dr.GetOrdinal(helper.Esteonomb);
                    if (!dr.IsDBNull(iEsteonomb)) entity.Esteonomb = dr.GetString(iEsteonomb);

                    int iEsteoanospuestaservicio = dr.GetOrdinal(helper.Esteoanospuestaservicio);
                    if (!dr.IsDBNull(iEsteoanospuestaservicio)) entity.Esteoanospuestaservicio = dr.GetString(iEsteoanospuestaservicio);

                    int iEsteopuntoconexion = dr.GetOrdinal(helper.Esteopuntoconexion);
                    if (!dr.IsDBNull(iEsteopuntoconexion)) entity.Esteopuntoconexion = dr.GetString(iEsteopuntoconexion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Ficha Tecnica 2023
        public List<EpoEstudioEoDTO> ListarPorEmpresa(int idEmpresa)
        {
            List<EpoEstudioEoDTO> entitys = new List<EpoEstudioEoDTO>();

            String sql = String.Format(helper.SqlListarPorEmpresa, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioEoDTO entity = new EpoEstudioEoDTO();

                    int iEstepocodi = dr.GetOrdinal(helper.Esteocodi);
                    if (!dr.IsDBNull(iEstepocodi)) entity.Esteocodi = Convert.ToInt32(dr.GetValue(iEstepocodi));

                    int iEstepocodiusu = dr.GetOrdinal(helper.Esteocodiusu);
                    if (!dr.IsDBNull(iEstepocodiusu)) entity.Esteocodiusu = dr.GetString(iEstepocodiusu);

                    int iEsteponomb = dr.GetOrdinal(helper.Esteonomb);
                    if (!dr.IsDBNull(iEsteponomb)) entity.Esteonomb = dr.GetString(iEsteponomb);

                    int iEstepofechaini = dr.GetOrdinal(helper.Esteofechaini);
                    if (!dr.IsDBNull(iEstepofechaini)) entity.Esteofechaini = dr.GetDateTime(iEstepofechaini);

                    int iEstepofechafin = dr.GetOrdinal(helper.Esteofechafin);
                    if (!dr.IsDBNull(iEstepofechafin)) entity.Esteofechafin = dr.GetDateTime(iEstepofechafin);

                    int iEstadescripcion = dr.GetOrdinal(helper.Estadescripcion);
                    if (!dr.IsDBNull(iEstadescripcion)) entity.Estadescripcion = dr.GetString(iEstadescripcion);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iTerceroinvolucrado = dr.GetOrdinal(helper.Terceroinvolucrado);
                    if (!dr.IsDBNull(iTerceroinvolucrado)) entity.Terceroinvolucrado = dr.GetString(iTerceroinvolucrado);

                    int iEsteoalcancesolestenl = dr.GetOrdinal(helper.Esteoalcancesolestenl);
                    if (!dr.IsDBNull(iEsteoalcancesolestenl)) entity.Esteoalcancesolestenl = dr.GetString(iEsteoalcancesolestenl);

                    int iEsteopotencia = dr.GetOrdinal(helper.Esteopotencia);
                    if (!dr.IsDBNull(iEsteopotencia)) entity.Esteopotencia = dr.GetString(iEsteopotencia);

                    int iEsteopuntoconexion = dr.GetOrdinal(helper.Esteopuntoconexion);
                    if (!dr.IsDBNull(iEsteopuntoconexion)) entity.Esteopuntoconexion = dr.GetString(iEsteopuntoconexion);

                    int iEsteoanospuestaservicio = dr.GetOrdinal(helper.Esteoanospuestaservicio);
                    if (!dr.IsDBNull(iEsteoanospuestaservicio)) entity.Esteoanospuestaservicio = dr.GetString(iEsteoanospuestaservicio);

                    int iEsteoobs = dr.GetOrdinal(helper.Esteoobs);
                    if (!dr.IsDBNull(iEsteoobs)) entity.Esteoobs = dr.GetString(iEsteoobs);

                    int iEsteoresumenejecutivoenl = dr.GetOrdinal(helper.Esteoresumenejecutivoenl);
                    if (!dr.IsDBNull(iEsteoresumenejecutivoenl)) entity.Esteoresumenejecutivoenl = dr.GetString(iEsteoresumenejecutivoenl);

                    int iEsteocodiproy = dr.GetOrdinal(helper.Esteocodiproy);
                    if (!dr.IsDBNull(iEsteocodiproy)) entity.Esteocodiproy = dr.GetString(iEsteocodiproy);

                    int iEsteofechaopecomercial = dr.GetOrdinal(helper.Esteofechaopecomercial);
                    if (!dr.IsDBNull(iEsteofechaopecomercial)) entity.Esteofechaopecomercial = dr.GetDateTime(iEsteofechaopecomercial);

                    int iEsteofechaconexion = dr.GetOrdinal(helper.Esteofechaconexion);
                    if (!dr.IsDBNull(iEsteofechaconexion)) entity.Esteofechaconexion = dr.GetDateTime(iEsteofechaconexion);

                    int iEsteofechaintegracion = dr.GetOrdinal(helper.Esteofechaintegracion);
                    if (!dr.IsDBNull(iEsteofechaintegracion)) entity.Esteofechaintegracion = dr.GetDateTime(iEsteofechaintegracion);

                    #region Mejoras EO-EPO
                    int iEsteoVigencia = dr.GetOrdinal(helper.EsteoVigencia);
                    if (!dr.IsDBNull(iEsteoVigencia)) entity.EsteoVigencia = dr.GetString(iEsteoVigencia);
                    #endregion
                    #region Mejoras EO-EPO-II
                    int iZonDescripcion = dr.GetOrdinal(helper.ZonDescripcion);
                    if (!dr.IsDBNull(iZonDescripcion)) entity.ZonDescripcion = dr.GetString(iZonDescripcion);
                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
        #endregion
    }
}
