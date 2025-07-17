using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Globalization;



namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EPO_ESTUDIO_EPO
    /// </summary>
    public class EpoEstudioEpoRepository: RepositoryBase, IEpoEstudioEpoRepository
    {
        public EpoEstudioEpoRepository(string strConn): base(strConn)
        {
        }

        EpoEstudioEpoHelper helper = new EpoEstudioEpoHelper();

        public int Save(EpoEstudioEpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Estacodi, DbType.Int32, entity.Estacodi);
            dbProvider.AddInParameter(command, helper.Estepocodiusu, DbType.String, entity.Estepocodiusu);
            dbProvider.AddInParameter(command, helper.Esteponomb, DbType.String, entity.Esteponomb);
            dbProvider.AddInParameter(command, helper.Emprcoditp, DbType.Int32, entity.Emprcoditp);
            dbProvider.AddInParameter(command, helper.Emprcoditi, DbType.Int32, entity.Emprcoditi);
            dbProvider.AddInParameter(command, helper.Estepopotencia, DbType.String, entity.Estepopotencia);
            dbProvider.AddInParameter(command, helper.Estepocapacidad, DbType.String, entity.Estepocapacidad);
            dbProvider.AddInParameter(command, helper.Estepocarga, DbType.String, entity.Estepocarga);
            dbProvider.AddInParameter(command, helper.Estepopuntoconexion, DbType.String, entity.Estepopuntoconexion);
            dbProvider.AddInParameter(command, helper.Estepoanospuestaservicio, DbType.String, entity.Estepoanospuestaservicio);
            dbProvider.AddInParameter(command, helper.Estepootros, DbType.String, entity.Estepootros);
            dbProvider.AddInParameter(command, helper.Estepoobs, DbType.String, entity.Estepoobs);
            dbProvider.AddInParameter(command, helper.Estepofechaini, DbType.DateTime, entity.Estepofechaini);
            dbProvider.AddInParameter(command, helper.Esteporesumenejecutivotit, DbType.String, entity.Esteporesumenejecutivotit);
            dbProvider.AddInParameter(command, helper.Esteporesumenejecutivoenl, DbType.String, entity.Esteporesumenejecutivoenl);
            dbProvider.AddInParameter(command, helper.Estepofechafin, DbType.DateTime, entity.Estepofechafin);
            dbProvider.AddInParameter(command, helper.Estepocertconformidadtit, DbType.String, entity.Estepocertconformidadtit);
            dbProvider.AddInParameter(command, helper.Estepocertconformidadenl, DbType.String, entity.Estepocertconformidadenl);
            dbProvider.AddInParameter(command, helper.Estepoplazorevcoesporv, DbType.Int32, entity.Estepoplazorevcoesporv);
            dbProvider.AddInParameter(command, helper.Estepoplazorevcoesvenc, DbType.Int32, entity.Estepoplazorevcoesvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazolevobsporv, DbType.Int32, entity.Estepoplazolevobsporv);
            dbProvider.AddInParameter(command, helper.Estepoplazolevobsvenc, DbType.Int32, entity.Estepoplazolevobsvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazoalcancesvenc, DbType.Int32, entity.Estepoplazoalcancesvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazoverificacionvenc, DbType.Int32, entity.Estepoplazoverificacionvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazorevterinvporv, DbType.Int32, entity.Estepoplazorevterinvporv);
            dbProvider.AddInParameter(command, helper.Estepoplazorevterinvvenc, DbType.Int32, entity.Estepoplazorevterinvvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazoenvestterinvporv, DbType.Int32, entity.Estepoplazoenvestterinvporv);
            dbProvider.AddInParameter(command, helper.Estepoplazoenvestterinvvenc, DbType.Int32, entity.Estepoplazoenvestterinvvenc);
            dbProvider.AddInParameter(command, helper.Estepoalcancefechaini, DbType.DateTime, entity.Estepoalcancefechaini);
            dbProvider.AddInParameter(command, helper.Estepoalcancesolesttit, DbType.String, entity.Estepoalcancesolesttit);
            dbProvider.AddInParameter(command, helper.Estepoalcancesolestenl, DbType.String, entity.Estepoalcancesolestenl);
            dbProvider.AddInParameter(command, helper.Estepoalcancesolestobs, DbType.String, entity.Estepoalcancesolestobs);
            dbProvider.AddInParameter(command, helper.Estepoalcancefechafin, DbType.DateTime, entity.Estepoalcancefechafin);
            dbProvider.AddInParameter(command, helper.Estepoalcanceenviotit, DbType.String, entity.Estepoalcanceenviotit);
            dbProvider.AddInParameter(command, helper.Estepoalcanceenvioenl, DbType.String, entity.Estepoalcanceenvioenl);
            dbProvider.AddInParameter(command, helper.Estepoalcanceenvioobs, DbType.String, entity.Estepoalcanceenvioobs);
            dbProvider.AddInParameter(command, helper.Estepoverifechaini, DbType.DateTime, entity.Estepoverifechaini);
            dbProvider.AddInParameter(command, helper.Estepoverientregaesttit, DbType.String, entity.Estepoverientregaesttit);
            dbProvider.AddInParameter(command, helper.Estepoverientregaestenl, DbType.String, entity.Estepoverientregaestenl);
            dbProvider.AddInParameter(command, helper.Estepoverientregaestobs, DbType.String, entity.Estepoverientregaestobs);
            dbProvider.AddInParameter(command, helper.Estepoverifechafin, DbType.DateTime, entity.Estepoverifechafin);
            dbProvider.AddInParameter(command, helper.Estepovericartatit, DbType.String, entity.Estepovericartatit);
            dbProvider.AddInParameter(command, helper.Estepovericartaenl, DbType.String, entity.Estepovericartaenl);
            dbProvider.AddInParameter(command, helper.Estepovericartaobs, DbType.String, entity.Estepovericartaobs);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Estepojustificacion, DbType.String, entity.Estepojustificacion);
            dbProvider.AddInParameter(command, helper.Estepoacumdiascoes, DbType.Int32, entity.Estepoacumdiascoes);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Estepocodiproy, DbType.String, entity.Estepocodiproy);
            dbProvider.AddInParameter(command, helper.Esteporesponsable, DbType.String, entity.Esteporesponsable);
            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.Int32, entity.PuntCodi);
            dbProvider.AddInParameter(command, helper.EstepoTipoProyecto, DbType.String, entity.EstepoTipoProyecto);
            dbProvider.AddInParameter(command, helper.Tipoconfig, DbType.Int32, entity.TipoConfig);

            dbProvider.AddInParameter(command, helper.EstepoAbsTit, DbType.String, entity.EstepoAbsTit);
            dbProvider.AddInParameter(command, helper.EstepoAbsEnl, DbType.String, entity.EstepoAbsEnl);
            dbProvider.AddInParameter(command, helper.EstepoAbsFFin, DbType.DateTime, entity.EstepoAbsFFin);
            dbProvider.AddInParameter(command, helper.EstepoAbsObs, DbType.String, entity.EstepoAbsObs);
            dbProvider.AddInParameter(command, helper.Estepoplazoverificacionvencabs, DbType.Int32, entity.Estepoplazoverificacionvencabs);

            #region Mejoras EO-EPO
            dbProvider.AddInParameter(command, helper.Estacodivigencia,DbType.Int32, entity.Estacodivigencia);
            #endregion

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoEstudioEpoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Estacodi, DbType.Int32, entity.Estacodi);
            dbProvider.AddInParameter(command, helper.Estepocodiusu, DbType.String, entity.Estepocodiusu);
            dbProvider.AddInParameter(command, helper.Esteponomb, DbType.String, entity.Esteponomb);
            dbProvider.AddInParameter(command, helper.Emprcoditp, DbType.Int32, entity.Emprcoditp);
            dbProvider.AddInParameter(command, helper.Emprcoditi, DbType.Int32, entity.Emprcoditi);
            dbProvider.AddInParameter(command, helper.Estepopotencia, DbType.String, entity.Estepopotencia);
            dbProvider.AddInParameter(command, helper.Estepocapacidad, DbType.String, entity.Estepocapacidad);
            dbProvider.AddInParameter(command, helper.Estepocarga, DbType.String, entity.Estepocarga);
            dbProvider.AddInParameter(command, helper.Estepopuntoconexion, DbType.String, entity.Estepopuntoconexion);
            dbProvider.AddInParameter(command, helper.Estepoanospuestaservicio, DbType.String, entity.Estepoanospuestaservicio);
            dbProvider.AddInParameter(command, helper.Estepootros, DbType.String, entity.Estepootros);
            dbProvider.AddInParameter(command, helper.Estepoobs, DbType.String, entity.Estepoobs);
            dbProvider.AddInParameter(command, helper.Estepofechaini, DbType.DateTime, entity.Estepofechaini);
            dbProvider.AddInParameter(command, helper.Esteporesumenejecutivotit, DbType.String, entity.Esteporesumenejecutivotit);
            dbProvider.AddInParameter(command, helper.Esteporesumenejecutivoenl, DbType.String, entity.Esteporesumenejecutivoenl);
            dbProvider.AddInParameter(command, helper.Estepofechafin, DbType.DateTime, entity.Estepofechafin);
            dbProvider.AddInParameter(command, helper.Estepocertconformidadtit, DbType.String, entity.Estepocertconformidadtit);
            dbProvider.AddInParameter(command, helper.Estepocertconformidadenl, DbType.String, entity.Estepocertconformidadenl);
            dbProvider.AddInParameter(command, helper.Estepoplazorevcoesporv, DbType.Int32, entity.Estepoplazorevcoesporv);
            dbProvider.AddInParameter(command, helper.Estepoplazorevcoesvenc, DbType.Int32, entity.Estepoplazorevcoesvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazolevobsporv, DbType.Int32, entity.Estepoplazolevobsporv);
            dbProvider.AddInParameter(command, helper.Estepoplazolevobsvenc, DbType.Int32, entity.Estepoplazolevobsvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazoalcancesvenc, DbType.Int32, entity.Estepoplazoalcancesvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazoverificacionvenc, DbType.Int32, entity.Estepoplazoverificacionvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazorevterinvporv, DbType.Int32, entity.Estepoplazorevterinvporv);
            dbProvider.AddInParameter(command, helper.Estepoplazorevterinvvenc, DbType.Int32, entity.Estepoplazorevterinvvenc);
            dbProvider.AddInParameter(command, helper.Estepoplazoenvestterinvporv, DbType.Int32, entity.Estepoplazoenvestterinvporv);
            dbProvider.AddInParameter(command, helper.Estepoplazoenvestterinvvenc, DbType.Int32, entity.Estepoplazoenvestterinvvenc);
            dbProvider.AddInParameter(command, helper.Estepoalcancefechaini, DbType.DateTime, entity.Estepoalcancefechaini);
            dbProvider.AddInParameter(command, helper.Estepoalcancesolesttit, DbType.String, entity.Estepoalcancesolesttit);
            dbProvider.AddInParameter(command, helper.Estepoalcancesolestenl, DbType.String, entity.Estepoalcancesolestenl);
            dbProvider.AddInParameter(command, helper.Estepoalcancesolestobs, DbType.String, entity.Estepoalcancesolestobs);
            dbProvider.AddInParameter(command, helper.Estepoalcancefechafin, DbType.DateTime, entity.Estepoalcancefechafin);
            dbProvider.AddInParameter(command, helper.Estepoalcanceenviotit, DbType.String, entity.Estepoalcanceenviotit);
            dbProvider.AddInParameter(command, helper.Estepoalcanceenvioenl, DbType.String, entity.Estepoalcanceenvioenl);
            dbProvider.AddInParameter(command, helper.Estepoalcanceenvioobs, DbType.String, entity.Estepoalcanceenvioobs);
            dbProvider.AddInParameter(command, helper.Estepoverifechaini, DbType.DateTime, entity.Estepoverifechaini);
            dbProvider.AddInParameter(command, helper.Estepoverientregaesttit, DbType.String, entity.Estepoverientregaesttit);
            dbProvider.AddInParameter(command, helper.Estepoverientregaestenl, DbType.String, entity.Estepoverientregaestenl);
            dbProvider.AddInParameter(command, helper.Estepoverientregaestobs, DbType.String, entity.Estepoverientregaestobs);
            dbProvider.AddInParameter(command, helper.Estepoverifechafin, DbType.DateTime, entity.Estepoverifechafin);
            dbProvider.AddInParameter(command, helper.Estepovericartatit, DbType.String, entity.Estepovericartatit);
            dbProvider.AddInParameter(command, helper.Estepovericartaenl, DbType.String, entity.Estepovericartaenl);
            dbProvider.AddInParameter(command, helper.Estepovericartaobs, DbType.String, entity.Estepovericartaobs);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Estepojustificacion, DbType.String, entity.Estepojustificacion);
            dbProvider.AddInParameter(command, helper.Estepoacumdiascoes, DbType.Int32, entity.Estepoacumdiascoes);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Estepocodiproy, DbType.String, entity.Estepocodiproy);
            dbProvider.AddInParameter(command, helper.Esteporesponsable, DbType.String, entity.Esteporesponsable);             
            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.String, entity.PuntCodi);
            dbProvider.AddInParameter(command, helper.EstepoTipoProyecto, DbType.String, entity.EstepoTipoProyecto);
            dbProvider.AddInParameter(command, helper.Tipoconfig, DbType.Int32, entity.TipoConfig);

            dbProvider.AddInParameter(command, helper.EstepoAbsTit, DbType.String, entity.EstepoAbsTit);
            dbProvider.AddInParameter(command, helper.EstepoAbsEnl, DbType.String, entity.EstepoAbsEnl);
            dbProvider.AddInParameter(command, helper.EstepoAbsFFin, DbType.DateTime, entity.EstepoAbsFFin);
            dbProvider.AddInParameter(command, helper.EstepoAbsObs, DbType.String, entity.EstepoAbsObs);
            dbProvider.AddInParameter(command, helper.Estepoplazoverificacionvencabs, DbType.Int32, entity.Estepoplazoverificacionvencabs);

            #region Mejoras EO-EPO
            dbProvider.AddInParameter(command, helper.Estacodivigencia, DbType.Int32, entity.Estacodivigencia);
            #endregion

            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, entity.Estepocodi);
           

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int estepocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, estepocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoEstudioEpoDTO GetById(int estepocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Estepocodi, DbType.Int32, estepocodi);
            EpoEstudioEpoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEsteporesponsable = dr.GetOrdinal(helper.Esteporesponsable);
                    if (!dr.IsDBNull(iEsteporesponsable)) entity.Esteporesponsable = dr.GetString(iEsteporesponsable);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTerceroinvolucrado = dr.GetOrdinal(helper.Terceroinvolucrado);
                    if (!dr.IsDBNull(iTerceroinvolucrado)) entity.Terceroinvolucrado = dr.GetString(iTerceroinvolucrado);

                    int iEstadescripcion = dr.GetOrdinal(helper.Estadescripcion);
                    if (!dr.IsDBNull(iEstadescripcion)) entity.Estadescripcion = dr.GetString(iEstadescripcion);

                    int iTipoConfig = dr.GetOrdinal(helper.Tipoconfig);
                    if (!dr.IsDBNull(iTipoConfig )) entity.TipoConfig  = Convert.ToInt32(dr.GetValue(iTipoConfig));

                    #region Mejoras EO-EPO
                    int iEstacodivigencia = dr.GetOrdinal(helper.Estacodivigencia);
                    if (!dr.IsDBNull(iEstacodivigencia))
                    {
                        entity.Estacodivigencia = Convert.ToInt32(dr[iEstacodivigencia]);
                    }
                    else
                        entity.Estacodivigencia = 0;
                    #endregion

                    #region Mejoras EO-EPO-II
                    int iZonDescripcion = dr.GetOrdinal(helper.ZonDescripcion);
                    if (!dr.IsDBNull(iZonDescripcion)) entity.ZonDescripcion = dr.GetString(iZonDescripcion);
                    #endregion
                }
            }

            return entity;
        }

        public List<EpoEstudioEpoDTO> List()
        {
            List<EpoEstudioEpoDTO> entitys = new List<EpoEstudioEpoDTO>();
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
        
        public List<EpoEstudioEpoDTO> GetByCriteria(EpoEstudioEpoDTO estudioepo)
        {
      
            string sRangoPresentacion = (estudioepo.FIniPresentacion != null & estudioepo.FFinPresentacion != null) ? " epo.estepofechaini BETWEEN TO_DATE('" + estudioepo.FIniPresentacion + "','dd/mm/yyyy')  AND  TO_DATE('" + estudioepo.FFinPresentacion + "','dd/mm/yyyy') " : " 1=1 ";
            string sRangoConformidad = (estudioepo.FIniConformidad != null & estudioepo.FFinConformidad != null) ? " epo.estepofechafin  BETWEEN  TO_DATE('" + estudioepo.FIniConformidad + "','dd/mm/yyyy')  AND  TO_DATE('" + estudioepo.FFinConformidad + "','dd/mm/yyyy') " : " 1=1 ";

            if (estudioepo.PuntCodi == null) estudioepo.PuntCodi = 0; // para el case del multiselect viene con null;

            string sql = string.Format(helper.SqlGetByCriteria, sRangoPresentacion, sRangoConformidad
                                                              , estudioepo.Estacodi, estudioepo.Emprcoditp, estudioepo.Esteponomb
                                                              , estudioepo.PuntCodi, estudioepo.Estepocodiproy
                                                              , estudioepo.nroPagina, estudioepo.nroFilas, estudioepo.Estepoanospuestaservicio, estudioepo.EstepoTipoProyecto, estudioepo.Zoncodi);

            List<EpoEstudioEpoDTO> entitys = new List<EpoEstudioEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioEpoDTO entity = new EpoEstudioEpoDTO();

                    int iEstepocodi = dr.GetOrdinal(helper.Estepocodi);
                    if (!dr.IsDBNull(iEstepocodi)) entity.Estepocodi = Convert.ToInt32(dr.GetValue(iEstepocodi));

                    int iEstepocodiusu = dr.GetOrdinal(helper.Estepocodiusu);
                    if (!dr.IsDBNull(iEstepocodiusu)) entity.Estepocodiusu = dr.GetString(iEstepocodiusu);

                    int iEsteponomb = dr.GetOrdinal(helper.Esteponomb);
                    if (!dr.IsDBNull(iEsteponomb)) entity.Esteponomb = dr.GetString(iEsteponomb);

                    int iEstepofechaini = dr.GetOrdinal(helper.Estepofechaini);
                    if (!dr.IsDBNull(iEstepofechaini)) entity.Estepofechaini = dr.GetDateTime(iEstepofechaini);

                    int iEstepofechafin = dr.GetOrdinal(helper.Estepofechafin);
                    if (!dr.IsDBNull(iEstepofechafin)) entity.Estepofechafin = dr.GetDateTime(iEstepofechafin);

                    int iEstadescripcion = dr.GetOrdinal(helper.Estadescripcion);
                    if (!dr.IsDBNull(iEstadescripcion)) entity.Estadescripcion = dr.GetString(iEstadescripcion);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iTerceroinvolucrado = dr.GetOrdinal(helper.Terceroinvolucrado);
                    if (!dr.IsDBNull(iTerceroinvolucrado)) entity.Terceroinvolucrado = dr.GetString(iTerceroinvolucrado);

                    int iEstepoalcancesolestenl = dr.GetOrdinal(helper.Estepoalcancesolestenl);
                    if (!dr.IsDBNull(iEstepoalcancesolestenl)) entity.Estepoalcancesolestenl = dr.GetString(iEstepoalcancesolestenl);

                    int iEstepopotencia = dr.GetOrdinal(helper.Estepopotencia);
                    if (!dr.IsDBNull(iEstepopotencia)) entity.Estepopotencia = dr.GetString(iEstepopotencia);

                    int iEstepopuntoconexion = dr.GetOrdinal(helper.Estepopuntoconexion);
                    if (!dr.IsDBNull(iEstepopuntoconexion)) entity.Estepopuntoconexion = dr.GetString(iEstepopuntoconexion);

                    int iEstepoanospuestaservicio = dr.GetOrdinal(helper.Estepoanospuestaservicio);
                    if (!dr.IsDBNull(iEstepoanospuestaservicio)) entity.Estepoanospuestaservicio = dr.GetString(iEstepoanospuestaservicio);

                    int iEstepoobs = dr.GetOrdinal(helper.Estepoobs);
                    if (!dr.IsDBNull(iEstepoobs)) entity.Estepoobs = dr.GetString(iEstepoobs);

                    int iEsteporesumenejecutivoenl = dr.GetOrdinal(helper.Esteporesumenejecutivoenl);
                    if (!dr.IsDBNull(iEsteporesumenejecutivoenl)) entity.Esteporesumenejecutivoenl = dr.GetString(iEsteporesumenejecutivoenl);

                    int iEstepocodiproy = dr.GetOrdinal(helper.Estepocodiproy);
                    if (!dr.IsDBNull(iEstepocodiproy)) entity.Estepocodiproy = dr.GetString(iEstepocodiproy);

                    #region Mejoras EO-EPO
                    int iEstepoVigencia = dr.GetOrdinal(helper.EstepoVigencia);
                    if (!dr.IsDBNull(iEstepoVigencia)) entity.EstepoVigencia = dr.GetString(iEstepoVigencia);
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

        public int ObtenerNroRegistroBusqueda(EpoEstudioEpoDTO estudioepo)
        {
            //string sRangoPresentacion = (estudioepo.FechaIniPresentacion != null & estudioepo.FechaFinPresentacion != null) ? " epo.estepofechaini BETWEEN TO_DATE('" + estudioepo.FechaIniPresentacion.Value.ToString("yyyy/MM/dd") + "','YYYY-MM-DD')  AND  TO_DATE('" + estudioepo.FechaFinPresentacion.Value.ToString("yyyy/MM/dd") + "','YYYY-MM-DD') " : " 1=1 ";
            //string sRangoConformidad = (estudioepo.FechaIniConformidad != null & estudioepo.FechaFinConformidad != null) ? " epo.estepofechafin  BETWEEN  TO_DATE('" + estudioepo.FechaIniConformidad.Value.ToString("yyyy/MM/dd") + "','YYYY-MM-DD')  AND  TO_DATE('" + estudioepo.FechaFinConformidad.Value.ToString("yyyy/MM/dd") + "','YYYY-MM-DD') " : " 1=1 ";

            string sRangoPresentacion = (estudioepo.FIniPresentacion != null & estudioepo.FFinPresentacion != null) ? " epo.estepofechaini BETWEEN TO_DATE('" + estudioepo.FIniPresentacion + "','dd/mm/yyyy')  AND  TO_DATE('" + estudioepo.FFinPresentacion + "','dd/mm/yyyy') " : " 1=1 ";
            string sRangoConformidad = (estudioepo.FIniConformidad != null & estudioepo.FFinConformidad != null) ? " epo.estepofechafin  BETWEEN  TO_DATE('" + estudioepo.FIniConformidad + "','dd/mm/yyyy')  AND  TO_DATE('" + estudioepo.FFinConformidad + "','dd/mm/yyyy') " : " 1=1 ";


            if (estudioepo.PuntCodi == null) estudioepo.PuntCodi = 0; // para el case del multiselect viene con null;
            string sql = string.Format(helper.SqlNroRegistros, sRangoPresentacion, sRangoConformidad
                                                              , estudioepo.Estacodi, estudioepo.Emprcoditp, estudioepo.Esteponomb
                                                              , estudioepo.PuntCodi, estudioepo.Estepocodiproy
                                                              , estudioepo.nroPagina, estudioepo.nroFilas, estudioepo.Estepoanospuestaservicio, estudioepo.EstepoTipoProyecto, estudioepo.Zoncodi);

            List<EpoEstudioEpoDTO> entitys = new List<EpoEstudioEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        #region Mejoras EO-EPO
        public List<EpoEstudioEpoDTO> ListVigenciaAnioIngreso(string FechaAnioIngreso)
        {
            string sql = string.Format(helper.SqlListVigenciaAnioIngreso, FechaAnioIngreso);

            List<EpoEstudioEpoDTO> entitys = new List<EpoEstudioEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioEpoDTO entity = new EpoEstudioEpoDTO();

                    int iEsteponomb = dr.GetOrdinal(helper.Esteponomb);
                    if (!dr.IsDBNull(iEsteponomb)) entity.Esteponomb = dr.GetString(iEsteponomb);

                    int iEstepoanospuestaservicio = dr.GetOrdinal(helper.Estepoanospuestaservicio);
                    if (!dr.IsDBNull(iEstepoanospuestaservicio)) entity.Estepoanospuestaservicio = dr.GetString(iEstepoanospuestaservicio);

                    int iEstepopuntoconexion = dr.GetOrdinal(helper.Estepopuntoconexion);
                    if (!dr.IsDBNull(iEstepopuntoconexion)) entity.Estepopuntoconexion = dr.GetString(iEstepopuntoconexion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<EpoEstudioEpoDTO> ListVigencia36Meses()
        {
            string sql = string.Format(helper.SqlListVigencia36Meses);

            List<EpoEstudioEpoDTO> entitys = new List<EpoEstudioEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioEpoDTO entity = new EpoEstudioEpoDTO();

                    int iEsteponomb = dr.GetOrdinal(helper.Esteponomb);
                    if (!dr.IsDBNull(iEsteponomb)) entity.Esteponomb = dr.GetString(iEsteponomb);

                    int iEstepoanospuestaservicio = dr.GetOrdinal(helper.Estepoanospuestaservicio);
                    if (!dr.IsDBNull(iEstepoanospuestaservicio)) entity.Estepoanospuestaservicio = dr.GetString(iEstepoanospuestaservicio);

                    int iEstepopuntoconexion = dr.GetOrdinal(helper.Estepopuntoconexion);
                    if (!dr.IsDBNull(iEstepopuntoconexion)) entity.Estepopuntoconexion = dr.GetString(iEstepopuntoconexion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<EpoEstudioEpoDTO> ListVigencia48Meses()
        {
            string sql = string.Format(helper.SqlListVigencia48Meses);

            List<EpoEstudioEpoDTO> entitys = new List<EpoEstudioEpoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoEstudioEpoDTO entity = new EpoEstudioEpoDTO();

                    int iEsteponomb = dr.GetOrdinal(helper.Esteponomb);
                    if (!dr.IsDBNull(iEsteponomb)) entity.Esteponomb = dr.GetString(iEsteponomb);

                    int iEstepoanospuestaservicio = dr.GetOrdinal(helper.Estepoanospuestaservicio);
                    if (!dr.IsDBNull(iEstepoanospuestaservicio)) entity.Estepoanospuestaservicio = dr.GetString(iEstepoanospuestaservicio);

                    int iEstepopuntoconexion = dr.GetOrdinal(helper.Estepopuntoconexion);
                    if (!dr.IsDBNull(iEstepopuntoconexion)) entity.Estepopuntoconexion = dr.GetString(iEstepopuntoconexion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
