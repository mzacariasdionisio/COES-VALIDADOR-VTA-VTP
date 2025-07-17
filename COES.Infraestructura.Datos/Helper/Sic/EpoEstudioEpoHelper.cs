using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_ESTUDIO_EPO
    /// </summary>
    public class EpoEstudioEpoHelper : HelperBase
    {
        public EpoEstudioEpoHelper() : base(Consultas.EpoEstudioEpoSql)
        {
        }
        
        public EpoEstudioEpoDTO Create(IDataReader dr)
        {
            EpoEstudioEpoDTO entity = new EpoEstudioEpoDTO();

            int iEstepocodi = dr.GetOrdinal(this.Estepocodi);
            if (!dr.IsDBNull(iEstepocodi)) entity.Estepocodi = Convert.ToInt32(dr.GetValue(iEstepocodi));

            int iEstacodi = dr.GetOrdinal(this.Estacodi);
            if (!dr.IsDBNull(iEstacodi)) entity.Estacodi = Convert.ToInt32(dr.GetValue(iEstacodi));

            int iEstepocodiusu = dr.GetOrdinal(this.Estepocodiusu);
            if (!dr.IsDBNull(iEstepocodiusu)) entity.Estepocodiusu = dr.GetString(iEstepocodiusu);

            int iEsteponomb = dr.GetOrdinal(this.Esteponomb);
            if (!dr.IsDBNull(iEsteponomb)) entity.Esteponomb = dr.GetString(iEsteponomb);

            int iEmprcoditp = dr.GetOrdinal(this.Emprcoditp);
            if (!dr.IsDBNull(iEmprcoditp)) entity.Emprcoditp = Convert.ToInt32(dr.GetValue(iEmprcoditp));

            int iEmprcoditi = dr.GetOrdinal(this.Emprcoditi);
            if (!dr.IsDBNull(iEmprcoditi)) entity.Emprcoditi = Convert.ToInt32(dr.GetValue(iEmprcoditi));

            int iEstepopotencia = dr.GetOrdinal(this.Estepopotencia);
            if (!dr.IsDBNull(iEstepopotencia)) entity.Estepopotencia = dr.GetString(iEstepopotencia);

            int iEstepocapacidad = dr.GetOrdinal(this.Estepocapacidad);
            if (!dr.IsDBNull(iEstepocapacidad)) entity.Estepocapacidad = dr.GetString(iEstepocapacidad);

            int iEstepocarga = dr.GetOrdinal(this.Estepocarga);
            if (!dr.IsDBNull(iEstepocarga)) entity.Estepocarga = dr.GetString(iEstepocarga);

            int iEstepopuntoconexion = dr.GetOrdinal(this.Estepopuntoconexion);
            if (!dr.IsDBNull(iEstepopuntoconexion)) entity.Estepopuntoconexion = dr.GetString(iEstepopuntoconexion);

            int iEstepoanospuestaservicio = dr.GetOrdinal(this.Estepoanospuestaservicio);
            if (!dr.IsDBNull(iEstepoanospuestaservicio)) entity.Estepoanospuestaservicio = dr.GetString(iEstepoanospuestaservicio);

            int iEstepootros = dr.GetOrdinal(this.Estepootros);
            if (!dr.IsDBNull(iEstepootros)) entity.Estepootros = dr.GetString(iEstepootros);

            int iEstepoobs = dr.GetOrdinal(this.Estepoobs);
            if (!dr.IsDBNull(iEstepoobs)) entity.Estepoobs = dr.GetString(iEstepoobs);

            int iEstepofechaini = dr.GetOrdinal(this.Estepofechaini);
            if (!dr.IsDBNull(iEstepofechaini)) entity.Estepofechaini = dr.GetDateTime(iEstepofechaini);

            int iEsteporesumenejecutivotit = dr.GetOrdinal(this.Esteporesumenejecutivotit);
            if (!dr.IsDBNull(iEsteporesumenejecutivotit)) entity.Esteporesumenejecutivotit = dr.GetString(iEsteporesumenejecutivotit);

            int iEsteporesumenejecutivoenl = dr.GetOrdinal(this.Esteporesumenejecutivoenl);
            if (!dr.IsDBNull(iEsteporesumenejecutivoenl)) entity.Esteporesumenejecutivoenl = dr.GetString(iEsteporesumenejecutivoenl);

            int iEstepofechafin = dr.GetOrdinal(this.Estepofechafin);
            if (!dr.IsDBNull(iEstepofechafin)) entity.Estepofechafin = dr.GetDateTime(iEstepofechafin);

            int iEstepocertconformidadtit = dr.GetOrdinal(this.Estepocertconformidadtit);
            if (!dr.IsDBNull(iEstepocertconformidadtit)) entity.Estepocertconformidadtit = dr.GetString(iEstepocertconformidadtit);

            int iEstepocertconformidadenl = dr.GetOrdinal(this.Estepocertconformidadenl);
            if (!dr.IsDBNull(iEstepocertconformidadenl)) entity.Estepocertconformidadenl = dr.GetString(iEstepocertconformidadenl);

            int iEstepoplazorevcoesporv = dr.GetOrdinal(this.Estepoplazorevcoesporv);
            if (!dr.IsDBNull(iEstepoplazorevcoesporv)) entity.Estepoplazorevcoesporv = Convert.ToInt32(dr.GetValue(iEstepoplazorevcoesporv));

            int iEstepoplazorevcoesvenc = dr.GetOrdinal(this.Estepoplazorevcoesvenc);
            if (!dr.IsDBNull(iEstepoplazorevcoesvenc)) entity.Estepoplazorevcoesvenc = Convert.ToInt32(dr.GetValue(iEstepoplazorevcoesvenc));

            int iEstepoplazolevobsporv = dr.GetOrdinal(this.Estepoplazolevobsporv);
            if (!dr.IsDBNull(iEstepoplazolevobsporv)) entity.Estepoplazolevobsporv = Convert.ToInt32(dr.GetValue(iEstepoplazolevobsporv));

            int iEstepoplazolevobsvenc = dr.GetOrdinal(this.Estepoplazolevobsvenc);
            if (!dr.IsDBNull(iEstepoplazolevobsvenc)) entity.Estepoplazolevobsvenc = Convert.ToInt32(dr.GetValue(iEstepoplazolevobsvenc));

            int iEstepoplazoalcancesvenc = dr.GetOrdinal(this.Estepoplazoalcancesvenc);
            if (!dr.IsDBNull(iEstepoplazoalcancesvenc)) entity.Estepoplazoalcancesvenc = Convert.ToInt32(dr.GetValue(iEstepoplazoalcancesvenc));

            int iEstepoplazoverificacionvenc = dr.GetOrdinal(this.Estepoplazoverificacionvenc);
            if (!dr.IsDBNull(iEstepoplazoverificacionvenc)) entity.Estepoplazoverificacionvenc = Convert.ToInt32(dr.GetValue(iEstepoplazoverificacionvenc));

            int iEstepoplazorevterinvporv = dr.GetOrdinal(this.Estepoplazorevterinvporv);
            if (!dr.IsDBNull(iEstepoplazorevterinvporv)) entity.Estepoplazorevterinvporv = Convert.ToInt32(dr.GetValue(iEstepoplazorevterinvporv));

            int iEstepoplazorevterinvvenc = dr.GetOrdinal(this.Estepoplazorevterinvvenc);
            if (!dr.IsDBNull(iEstepoplazorevterinvvenc)) entity.Estepoplazorevterinvvenc = Convert.ToInt32(dr.GetValue(iEstepoplazorevterinvvenc));

            int iEstepoplazoenvestterinvporv = dr.GetOrdinal(this.Estepoplazoenvestterinvporv);
            if (!dr.IsDBNull(iEstepoplazoenvestterinvporv)) entity.Estepoplazoenvestterinvporv = Convert.ToInt32(dr.GetValue(iEstepoplazoenvestterinvporv));

            int iEstepoplazoenvestterinvvenc = dr.GetOrdinal(this.Estepoplazoenvestterinvvenc);
            if (!dr.IsDBNull(iEstepoplazoenvestterinvvenc)) entity.Estepoplazoenvestterinvvenc = Convert.ToInt32(dr.GetValue(iEstepoplazoenvestterinvvenc));

            int iEstepoalcancefechaini = dr.GetOrdinal(this.Estepoalcancefechaini);
            if (!dr.IsDBNull(iEstepoalcancefechaini)) entity.Estepoalcancefechaini = dr.GetDateTime(iEstepoalcancefechaini);

            int iEstepoalcancesolesttit = dr.GetOrdinal(this.Estepoalcancesolesttit);
            if (!dr.IsDBNull(iEstepoalcancesolesttit)) entity.Estepoalcancesolesttit = dr.GetString(iEstepoalcancesolesttit);

            int iEstepoalcancesolestenl = dr.GetOrdinal(this.Estepoalcancesolestenl);
            if (!dr.IsDBNull(iEstepoalcancesolestenl)) entity.Estepoalcancesolestenl = dr.GetString(iEstepoalcancesolestenl);

            int iEstepoalcancesolestobs = dr.GetOrdinal(this.Estepoalcancesolestobs);
            if (!dr.IsDBNull(iEstepoalcancesolestobs)) entity.Estepoalcancesolestobs = dr.GetString(iEstepoalcancesolestobs);

            int iEstepoalcancefechafin = dr.GetOrdinal(this.Estepoalcancefechafin);
            if (!dr.IsDBNull(iEstepoalcancefechafin)) entity.Estepoalcancefechafin = dr.GetDateTime(iEstepoalcancefechafin);

            int iEstepoalcanceenviotit = dr.GetOrdinal(this.Estepoalcanceenviotit);
            if (!dr.IsDBNull(iEstepoalcanceenviotit)) entity.Estepoalcanceenviotit = dr.GetString(iEstepoalcanceenviotit);

            int iEstepoalcanceenvioenl = dr.GetOrdinal(this.Estepoalcanceenvioenl);
            if (!dr.IsDBNull(iEstepoalcanceenvioenl)) entity.Estepoalcanceenvioenl = dr.GetString(iEstepoalcanceenvioenl);

            int iEstepoalcanceenvioobs = dr.GetOrdinal(this.Estepoalcanceenvioobs);
            if (!dr.IsDBNull(iEstepoalcanceenvioobs)) entity.Estepoalcanceenvioobs = dr.GetString(iEstepoalcanceenvioobs);

            int iEstepoverifechaini = dr.GetOrdinal(this.Estepoverifechaini);
            if (!dr.IsDBNull(iEstepoverifechaini)) entity.Estepoverifechaini = dr.GetDateTime(iEstepoverifechaini);

            int iEstepoverientregaesttit = dr.GetOrdinal(this.Estepoverientregaesttit);
            if (!dr.IsDBNull(iEstepoverientregaesttit)) entity.Estepoverientregaesttit = dr.GetString(iEstepoverientregaesttit);

            int iEstepoverientregaestenl = dr.GetOrdinal(this.Estepoverientregaestenl);
            if (!dr.IsDBNull(iEstepoverientregaestenl)) entity.Estepoverientregaestenl = dr.GetString(iEstepoverientregaestenl);

            int iEstepoverientregaestobs = dr.GetOrdinal(this.Estepoverientregaestobs);
            if (!dr.IsDBNull(iEstepoverientregaestobs)) entity.Estepoverientregaestobs = dr.GetString(iEstepoverientregaestobs);

            int iEstepoverifechafin = dr.GetOrdinal(this.Estepoverifechafin);
            if (!dr.IsDBNull(iEstepoverifechafin)) entity.Estepoverifechafin = dr.GetDateTime(iEstepoverifechafin);

            int iEstepovericartatit = dr.GetOrdinal(this.Estepovericartatit);
            if (!dr.IsDBNull(iEstepovericartatit)) entity.Estepovericartatit = dr.GetString(iEstepovericartatit);

            int iEstepovericartaenl = dr.GetOrdinal(this.Estepovericartaenl);
            if (!dr.IsDBNull(iEstepovericartaenl)) entity.Estepovericartaenl = dr.GetString(iEstepovericartaenl);

            int iEstepovericartaobs = dr.GetOrdinal(this.Estepovericartaobs);
            if (!dr.IsDBNull(iEstepovericartaobs)) entity.Estepovericartaobs = dr.GetString(iEstepovericartaobs);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iEstepojustificacion = dr.GetOrdinal(this.Estepojustificacion);
            if (!dr.IsDBNull(iEstepojustificacion)) entity.Estepojustificacion = dr.GetString(iEstepojustificacion);

            int iEstepoacumdiascoes = dr.GetOrdinal(this.Estepoacumdiascoes);
            if (!dr.IsDBNull(iEstepoacumdiascoes)) entity.Estepoacumdiascoes = Convert.ToInt32(dr.GetValue(iEstepoacumdiascoes));

            
            int iEstepocodiproy = dr.GetOrdinal(this.Estepocodiproy);
            if (!dr.IsDBNull(iEstepocodiproy)) entity.Estepocodiproy = dr.GetString(iEstepocodiproy);

            int iEstepoTipoProyecto = dr.GetOrdinal(this.EstepoTipoProyecto);
            if (!dr.IsDBNull(iEstepoTipoProyecto)) entity.EstepoTipoProyecto = dr.GetString(iEstepoTipoProyecto);

            int iPuntCodi = dr.GetOrdinal(this.PuntCodi);
            if (!dr.IsDBNull(iPuntCodi)) entity.PuntCodi = Convert.ToInt32(dr.GetValue(iPuntCodi));


            int iEstepoAbsTit = dr.GetOrdinal(this.EstepoAbsTit);
            if (!dr.IsDBNull(iEstepoAbsTit)) entity.EstepoAbsTit = dr.GetString(iEstepoAbsTit);

            int iEstepoAbsEnl = dr.GetOrdinal(this.EstepoAbsEnl);
            if (!dr.IsDBNull(iEstepoAbsEnl)) entity.EstepoAbsEnl = dr.GetString(iEstepoAbsEnl);

            int iEstepoAbsFFin = dr.GetOrdinal(this.EstepoAbsFFin);
            if (!dr.IsDBNull(iEstepoAbsFFin)) entity.EstepoAbsFFin = dr.GetDateTime(iEstepoAbsFFin);

            int iEstepoAbsObs = dr.GetOrdinal(this.EstepoAbsObs);
            if (!dr.IsDBNull(iEstepoAbsObs)) entity.EstepoAbsObs = dr.GetString(iEstepoAbsObs);

            int iEstepoplazoverificacionvencabs = dr.GetOrdinal(this.Estepoplazoverificacionvencabs);
            if (!dr.IsDBNull(iEstepoplazoverificacionvencabs)) entity.Estepoplazoverificacionvencabs = Convert.ToInt32(dr.GetValue(iEstepoplazoverificacionvencabs));


            return entity;
        }

        public string SqlObtenerNroRegistroBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusqueda"); }
        }

        #region Mapeo de Campos

        public string Estepocodi = "ESTEPOCODI";
        public string Estacodi = "ESTACODI";
        public string Estepocodiusu = "ESTEPOCODIUSU";
        public string Esteponomb = "ESTEPONOMB";
        public string Emprcoditp = "EMPRCODITP";
        public string Emprcoditi = "EMPRCODITI";
        public string Estepopotencia = "ESTEPOPOTENCIA";
        public string Estepocapacidad = "ESTEPOCAPACIDAD";
        public string Estepocarga = "ESTEPOCARGA";
        public string Estepopuntoconexion = "ESTEPOPUNTOCONEXION";
        public string Estepoanospuestaservicio = "ESTEPOANOSPUESTASERVICIO";
        public string Estepootros = "ESTEPOOTROS";
        public string Estepoobs = "ESTEPOOBS";
        public string Estepofechaini = "ESTEPOFECHAINI";
        public string Esteporesumenejecutivotit = "ESTEPORESUMENEJECUTIVOTIT";
        public string Esteporesumenejecutivoenl = "ESTEPORESUMENEJECUTIVOENL";
        public string Estepofechafin = "ESTEPOFECHAFIN";
        public string Estepocertconformidadtit = "ESTEPOCERTCONFORMIDADTIT";
        public string Estepocertconformidadenl = "ESTEPOCERTCONFORMIDADENL";
        public string Estepoplazorevcoesporv = "ESTEPOPLAZOREVCOESPORV";
        public string Estepoplazorevcoesvenc = "ESTEPOPLAZOREVCOESVENC";
        public string Estepoplazolevobsporv = "ESTEPOPLAZOLEVOBSPORV";
        public string Estepoplazolevobsvenc = "ESTEPOPLAZOLEVOBSVENC";
        public string Estepoplazoalcancesvenc = "ESTEPOPLAZOALCANCESVENC";
        public string Estepoplazoverificacionvenc = "ESTEPOPLAZOVERIFICACIONVENC";
        public string Estepoplazorevterinvporv = "ESTEPOPLAZOREVTERINVPORV";
        public string Estepoplazorevterinvvenc = "ESTEPOPLAZOREVTERINVVENC";
        public string Estepoplazoenvestterinvporv = "ESTEPOPLAZOENVESTTERINVPORV";
        public string Estepoplazoenvestterinvvenc = "ESTEPOPLAZOENVESTTERINVVENC";
        public string Estepoalcancefechaini = "ESTEPOALCANCEFECHAINI";
        public string Estepoalcancesolesttit = "ESTEPOALCANCESOLESTTIT";
        public string Estepoalcancesolestenl = "ESTEPOALCANCESOLESTENL";
        public string Estepoalcancesolestobs = "ESTEPOALCANCESOLESTOBS";
        public string Estepoalcancefechafin = "ESTEPOALCANCEFECHAFIN";
        public string Estepoalcanceenviotit = "ESTEPOALCANCEENVIOTIT";
        public string Estepoalcanceenvioenl = "ESTEPOALCANCEENVIOENL";
        public string Estepoalcanceenvioobs = "ESTEPOALCANCEENVIOOBS";
        public string Estepoverifechaini = "ESTEPOVERIFECHAINI";
        public string Estepoverientregaesttit = "ESTEPOVERIENTREGAESTTIT";
        public string Estepoverientregaestenl = "ESTEPOVERIENTREGAESTENL";
        public string Estepoverientregaestobs = "ESTEPOVERIENTREGAESTOBS";
        public string Estepoverifechafin = "ESTEPOVERIFECHAFIN";
        public string Estepovericartatit = "ESTEPOVERICARTATIT";
        public string Estepovericartaenl = "ESTEPOVERICARTAENL";
        public string Estepovericartaobs = "ESTEPOVERICARTAOBS";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Estepojustificacion = "ESTEPOJUSTIFICACION";
        public string Estepoacumdiascoes = "ESTEPOACUMDIASCOES";
        public string Usercode = "USERCODE";
        public string Estepocodiproy = "ESTEPOCODIPROY";
        public string Esteporesponsable = "ESTEPORESPONSABLE";
        public string Tipoconfig = "TIPOCONFIG";
        public string PuntCodi = "PUNTCODI";

        public string Estadescripcion = "estadescripcion";
        public string Emprnomb = "emprnomb";
        public string Username = "username";
        public string Terceroinvolucrado = "terceroinvolucrado";

        public string EstepoTipoProyecto = "ESTEPOTIPOPROYECTO";


        public string EstepoAbsTit = "ESTEPOABSTIT";
        public string EstepoAbsEnl = "ESTEPOABSENL";
        public string EstepoAbsFFin = "ESTEPOABSFFIN";
        public string EstepoAbsObs = "ESTEPOABSOBS";

        public string Estepoplazoverificacionvencabs = "ESTEPOPLAZOVERIFICACIONVENCABS";

        #region Mejoras EO-EPO
        public string EstepoVigencia = "ESTEPOVIGENCIA";
        public string Estacodivigencia = "ESTACODIVIGENCIA";
        #endregion
        #region Mejoras EO-EPO-II
        public string ZonDescripcion = "ZONDESCRIPCION";
        #endregion
        #endregion
    }
}
