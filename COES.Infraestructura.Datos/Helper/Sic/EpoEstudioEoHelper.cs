using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_ESTUDIO_EO
    /// </summary>
    public class EpoEstudioEoHelper : HelperBase
    {
        public EpoEstudioEoHelper(): base(Consultas.EpoEstudioEoSql)
        {
        }

        public string SqlObtenerNroRegistroBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusqueda"); }
        }

        public string SqlGetByFwUser
        {
            get { return base.GetSqlXml("GetByFwUser"); }
        }
        
        public EpoEstudioEoDTO Create(IDataReader dr)
        {
            EpoEstudioEoDTO entity = new EpoEstudioEoDTO();

            int iEsteofechaopecomercial = dr.GetOrdinal(this.Esteofechaopecomercial);
            if (!dr.IsDBNull(iEsteofechaopecomercial)) entity.Esteofechaopecomercial = dr.GetDateTime(iEsteofechaopecomercial);

            int iEsteofechaintegracion = dr.GetOrdinal(this.Esteofechaintegracion);
            if (!dr.IsDBNull(iEsteofechaintegracion)) entity.Esteofechaintegracion = dr.GetDateTime(iEsteofechaintegracion);

            int iEsteofechaconexion = dr.GetOrdinal(this.Esteofechaconexion);
            if (!dr.IsDBNull(iEsteofechaconexion)) entity.Esteofechaconexion = dr.GetDateTime(iEsteofechaconexion);

            int iEsteocodi = dr.GetOrdinal(this.Esteocodi);
            if (!dr.IsDBNull(iEsteocodi)) entity.Esteocodi = Convert.ToInt32(dr.GetValue(iEsteocodi));

            int iEstacodi = dr.GetOrdinal(this.Estacodi);
            if (!dr.IsDBNull(iEstacodi)) entity.Estacodi = Convert.ToInt32(dr.GetValue(iEstacodi));

            int iEsteocodiusu = dr.GetOrdinal(this.Esteocodiusu);
            if (!dr.IsDBNull(iEsteocodiusu)) entity.Esteocodiusu = dr.GetString(iEsteocodiusu);

            int iEsteonomb = dr.GetOrdinal(this.Esteonomb);
            if (!dr.IsDBNull(iEsteonomb)) entity.Esteonomb = dr.GetString(iEsteonomb);

            int iEmprcoditp = dr.GetOrdinal(this.Emprcoditp);
            if (!dr.IsDBNull(iEmprcoditp)) entity.Emprcoditp = Convert.ToInt32(dr.GetValue(iEmprcoditp));

            int iEmprcoditi = dr.GetOrdinal(this.Emprcoditi);
            if (!dr.IsDBNull(iEmprcoditi)) entity.Emprcoditi = Convert.ToInt32(dr.GetValue(iEmprcoditi));

            int iEsteopotencia = dr.GetOrdinal(this.Esteopotencia);
            if (!dr.IsDBNull(iEsteopotencia)) entity.Esteopotencia = dr.GetString(iEsteopotencia);

            int iEsteocapacidad = dr.GetOrdinal(this.Esteocapacidad);
            if (!dr.IsDBNull(iEsteocapacidad)) entity.Esteocapacidad = dr.GetString(iEsteocapacidad);

            int iEsteocarga = dr.GetOrdinal(this.Esteocarga);
            if (!dr.IsDBNull(iEsteocarga)) entity.Esteocarga = dr.GetString(iEsteocarga);

            int iEsteopotenciarer = dr.GetOrdinal(this.Esteopotenciarer);
            if (!dr.IsDBNull(iEsteopotenciarer)) entity.Esteopotenciarer = dr.GetString(iEsteopotenciarer);

            int iEsteopuntoconexion = dr.GetOrdinal(this.Esteopuntoconexion);
            if (!dr.IsDBNull(iEsteopuntoconexion)) entity.Esteopuntoconexion = dr.GetString(iEsteopuntoconexion);

            int iEsteoanospuestaservicio = dr.GetOrdinal(this.Esteoanospuestaservicio);
            if (!dr.IsDBNull(iEsteoanospuestaservicio)) entity.Esteoanospuestaservicio = dr.GetString(iEsteoanospuestaservicio);

            int iEsteootros = dr.GetOrdinal(this.Esteootros);
            if (!dr.IsDBNull(iEsteootros)) entity.Esteootros = dr.GetString(iEsteootros);

            int iEsteoobs = dr.GetOrdinal(this.Esteoobs);
            if (!dr.IsDBNull(iEsteoobs)) entity.Esteoobs = dr.GetString(iEsteoobs);

            int iEsteofechaini = dr.GetOrdinal(this.Esteofechaini);
            if (!dr.IsDBNull(iEsteofechaini)) entity.Esteofechaini = dr.GetDateTime(iEsteofechaini);

            int iEsteoresumenejecutivotit = dr.GetOrdinal(this.Esteoresumenejecutivotit);
            if (!dr.IsDBNull(iEsteoresumenejecutivotit)) entity.Esteoresumenejecutivotit = dr.GetString(iEsteoresumenejecutivotit);

            int iEsteoresumenejecutivoenl = dr.GetOrdinal(this.Esteoresumenejecutivoenl);
            if (!dr.IsDBNull(iEsteoresumenejecutivoenl)) entity.Esteoresumenejecutivoenl = dr.GetString(iEsteoresumenejecutivoenl);

            int iEsteofechafin = dr.GetOrdinal(this.Esteofechafin);
            if (!dr.IsDBNull(iEsteofechafin)) entity.Esteofechafin = dr.GetDateTime(iEsteofechafin);

            int iEsteocertconformidadtit = dr.GetOrdinal(this.Esteocertconformidadtit);
            if (!dr.IsDBNull(iEsteocertconformidadtit)) entity.Esteocertconformidadtit = dr.GetString(iEsteocertconformidadtit);

            int iEsteocertconformidadenl = dr.GetOrdinal(this.Esteocertconformidadenl);
            if (!dr.IsDBNull(iEsteocertconformidadenl)) entity.Esteocertconformidadenl = dr.GetString(iEsteocertconformidadenl);

            int iEsteoplazorevcoesporv = dr.GetOrdinal(this.Esteoplazorevcoesporv);
            if (!dr.IsDBNull(iEsteoplazorevcoesporv)) entity.Esteoplazorevcoesporv = Convert.ToInt32(dr.GetValue(iEsteoplazorevcoesporv));

            int iEsteoplazorevcoesvenc = dr.GetOrdinal(this.Esteoplazorevcoesvenc);
            if (!dr.IsDBNull(iEsteoplazorevcoesvenc)) entity.Esteoplazorevcoesvenc = Convert.ToInt32(dr.GetValue(iEsteoplazorevcoesvenc));

            int iEsteoplazolevobsporv = dr.GetOrdinal(this.Esteoplazolevobsporv);
            if (!dr.IsDBNull(iEsteoplazolevobsporv)) entity.Esteoplazolevobsporv = Convert.ToInt32(dr.GetValue(iEsteoplazolevobsporv));

            int iEsteoplazolevobsvenc = dr.GetOrdinal(this.Esteoplazolevobsvenc);
            if (!dr.IsDBNull(iEsteoplazolevobsvenc)) entity.Esteoplazolevobsvenc = Convert.ToInt32(dr.GetValue(iEsteoplazolevobsvenc));

            int iEsteoplazoalcancesvenc = dr.GetOrdinal(this.Esteoplazoalcancesvenc);
            if (!dr.IsDBNull(iEsteoplazoalcancesvenc)) entity.Esteoplazoalcancesvenc = Convert.ToInt32(dr.GetValue(iEsteoplazoalcancesvenc));

            int iEsteoplazoverificacionvenc = dr.GetOrdinal(this.Esteoplazoverificacionvenc);
            if (!dr.IsDBNull(iEsteoplazoverificacionvenc)) entity.Esteoplazoverificacionvenc = Convert.ToInt32(dr.GetValue(iEsteoplazoverificacionvenc));

            int iEsteoplazorevterinvporv = dr.GetOrdinal(this.Esteoplazorevterinvporv);
            if (!dr.IsDBNull(iEsteoplazorevterinvporv)) entity.Esteoplazorevterinvporv = Convert.ToInt32(dr.GetValue(iEsteoplazorevterinvporv));

            int iEsteoplazorevterinvvenc = dr.GetOrdinal(this.Esteoplazorevterinvvenc);
            if (!dr.IsDBNull(iEsteoplazorevterinvvenc)) entity.Esteoplazorevterinvvenc = Convert.ToInt32(dr.GetValue(iEsteoplazorevterinvvenc));

            int iEsteoplazoenvestterinvporv = dr.GetOrdinal(this.Esteoplazoenvestterinvporv);
            if (!dr.IsDBNull(iEsteoplazoenvestterinvporv)) entity.Esteoplazoenvestterinvporv = Convert.ToInt32(dr.GetValue(iEsteoplazoenvestterinvporv));

            int iEsteoplazoenvestterinvvenc = dr.GetOrdinal(this.Esteoplazoenvestterinvvenc);
            if (!dr.IsDBNull(iEsteoplazoenvestterinvvenc)) entity.Esteoplazoenvestterinvvenc = Convert.ToInt32(dr.GetValue(iEsteoplazoenvestterinvvenc));

            int iEsteoalcancefechaini = dr.GetOrdinal(this.Esteoalcancefechaini);
            if (!dr.IsDBNull(iEsteoalcancefechaini)) entity.Esteoalcancefechaini = dr.GetDateTime(iEsteoalcancefechaini);

            int iEsteoalcancesolesttit = dr.GetOrdinal(this.Esteoalcancesolesttit);
            if (!dr.IsDBNull(iEsteoalcancesolesttit)) entity.Esteoalcancesolesttit = dr.GetString(iEsteoalcancesolesttit);

            int iEsteoalcancesolestenl = dr.GetOrdinal(this.Esteoalcancesolestenl);
            if (!dr.IsDBNull(iEsteoalcancesolestenl)) entity.Esteoalcancesolestenl = dr.GetString(iEsteoalcancesolestenl);

            int iEsteoalcancesolestobs = dr.GetOrdinal(this.Esteoalcancesolestobs);
            if (!dr.IsDBNull(iEsteoalcancesolestobs)) entity.Esteoalcancesolestobs = dr.GetString(iEsteoalcancesolestobs);

            int iEsteoalcancefechafin = dr.GetOrdinal(this.Esteoalcancefechafin);
            if (!dr.IsDBNull(iEsteoalcancefechafin)) entity.Esteoalcancefechafin = dr.GetDateTime(iEsteoalcancefechafin);

            int iEsteoalcanceenviotit = dr.GetOrdinal(this.Esteoalcanceenviotit);
            if (!dr.IsDBNull(iEsteoalcanceenviotit)) entity.Esteoalcanceenviotit = dr.GetString(iEsteoalcanceenviotit);

            int iEsteoalcanceenvioenl = dr.GetOrdinal(this.Esteoalcanceenvioenl);
            if (!dr.IsDBNull(iEsteoalcanceenvioenl)) entity.Esteoalcanceenvioenl = dr.GetString(iEsteoalcanceenvioenl);

            int iEsteoalcanceenvioobs = dr.GetOrdinal(this.Esteoalcanceenvioobs);
            if (!dr.IsDBNull(iEsteoalcanceenvioobs)) entity.Esteoalcanceenvioobs = dr.GetString(iEsteoalcanceenvioobs);

            int iEsteoverifechaini = dr.GetOrdinal(this.Esteoverifechaini);
            if (!dr.IsDBNull(iEsteoverifechaini)) entity.Esteoverifechaini = dr.GetDateTime(iEsteoverifechaini);

            int iEsteoverientregaesttit = dr.GetOrdinal(this.Esteoverientregaesttit);
            if (!dr.IsDBNull(iEsteoverientregaesttit)) entity.Esteoverientregaesttit = dr.GetString(iEsteoverientregaesttit);

            int iEsteoverientregaestenl = dr.GetOrdinal(this.Esteoverientregaestenl);
            if (!dr.IsDBNull(iEsteoverientregaestenl)) entity.Esteoverientregaestenl = dr.GetString(iEsteoverientregaestenl);

            int iEsteoverientregaestobs = dr.GetOrdinal(this.Esteoverientregaestobs);
            if (!dr.IsDBNull(iEsteoverientregaestobs)) entity.Esteoverientregaestobs = dr.GetString(iEsteoverientregaestobs);

            int iEsteoverifechafin = dr.GetOrdinal(this.Esteoverifechafin);
            if (!dr.IsDBNull(iEsteoverifechafin)) entity.Esteoverifechafin = dr.GetDateTime(iEsteoverifechafin);

            int iEsteovericartatit = dr.GetOrdinal(this.Esteovericartatit);
            if (!dr.IsDBNull(iEsteovericartatit)) entity.Esteovericartatit = dr.GetString(iEsteovericartatit);

            int iEsteovericartaenl = dr.GetOrdinal(this.Esteovericartaenl);
            if (!dr.IsDBNull(iEsteovericartaenl)) entity.Esteovericartaenl = dr.GetString(iEsteovericartaenl);

            int iEsteovericartaobs = dr.GetOrdinal(this.Esteovericartaobs);
            if (!dr.IsDBNull(iEsteovericartaobs)) entity.Esteovericartaobs = dr.GetString(iEsteovericartaobs);

            int iEsteopuestaenservfecha = dr.GetOrdinal(this.Esteopuestaenservfecha);
            if (!dr.IsDBNull(iEsteopuestaenservfecha)) entity.Esteopuestaenservfecha = dr.GetDateTime(iEsteopuestaenservfecha);

            int iEsteopuestaenservcomentario = dr.GetOrdinal(this.Esteopuestaenservcomentario);
            if (!dr.IsDBNull(iEsteopuestaenservcomentario)) entity.Esteopuestaenservcomentario = dr.GetString(iEsteopuestaenservcomentario);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iEsteojustificacion = dr.GetOrdinal(this.Esteojustificacion);
            if (!dr.IsDBNull(iEsteojustificacion)) entity.Esteojustificacion = dr.GetString(iEsteojustificacion);

            int iEsteoacumdiascoes = dr.GetOrdinal(this.Esteoacumdiascoes);
            if (!dr.IsDBNull(iEsteoacumdiascoes)) entity.Esteoacumdiascoes = Convert.ToInt32(dr.GetValue(iEsteoacumdiascoes));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iEsteocodiproy = dr.GetOrdinal(this.Esteocodiproy);
            if (!dr.IsDBNull(iEsteocodiproy)) entity.Esteocodiproy = dr.GetString(iEsteocodiproy);


            int iEsteoTipoProyecto = dr.GetOrdinal(this.EsteoTipoProyecto);
            if (!dr.IsDBNull(iEsteoTipoProyecto)) entity.EsteoTipoProyecto = dr.GetString(iEsteoTipoProyecto);

            int iPuntCodi = dr.GetOrdinal(this.PuntCodi);
            if (!dr.IsDBNull(iPuntCodi)) entity.PuntCodi = Convert.ToInt32(dr.GetValue(iPuntCodi));


            int iTipoConfig = dr.GetOrdinal(this.Tipoconfig);
            if(!dr.IsDBNull(iTipoConfig) ) entity.TipoConfig = Convert.ToInt32(dr.GetValue(iTipoConfig ));


            int iEsteoAbsTit = dr.GetOrdinal(this.EsteoAbsTit);
            if (!dr.IsDBNull(iEsteoAbsTit)) entity.EsteoAbsTit = dr.GetString(iEsteoAbsTit);

            int iEsteoAbsEnl = dr.GetOrdinal(this.EsteoAbsEnl);
            if (!dr.IsDBNull(iEsteoAbsEnl)) entity.EsteoAbsEnl = dr.GetString(iEsteoAbsEnl);

            int iEsteoAbsFFin = dr.GetOrdinal(this.EsteoAbsFFin);
            if (!dr.IsDBNull(iEsteoAbsFFin)) entity.EsteoAbsFFin = dr.GetDateTime(iEsteoAbsFFin);

            int iEsteoAbsObs = dr.GetOrdinal(this.EsteoAbsObs);
            if (!dr.IsDBNull(iEsteoAbsObs)) entity.EsteoAbsObs = dr.GetString(iEsteoAbsObs);

            int iEsteoplazoverificacionvencabs = dr.GetOrdinal(this.Esteoplazoverificacionvencabs);
            if (!dr.IsDBNull(iEsteoplazoverificacionvencabs)) entity.Esteoplazoverificacionvencabs = Convert.ToInt32(dr.GetValue(iEsteoplazoverificacionvencabs));


            return entity;
        }


        #region Mapeo de Campos

        public string Esteofechaopecomercial = "ESTEOFECHAOPECOMERCIAL";
        public string Esteofechaintegracion = "ESTEOFECHAINTEGRACION";
        public string Esteofechaconexion = "ESTEOFECHACONEXION";
        public string Esteocodi = "ESTEOCODI";
        public string Estacodi = "ESTACODI";
        public string Esteocodiusu = "ESTEOCODIUSU";
        public string Esteonomb = "ESTEONOMB";
        public string Emprcoditp = "EMPRCODITP";
        public string Emprcoditi = "EMPRCODITI";
        public string Esteopotencia = "ESTEOPOTENCIA";
        public string Esteocapacidad = "ESTEOCAPACIDAD";
        public string Esteocarga = "ESTEOCARGA";
        public string Esteopotenciarer = "ESTEOPOTENCIARER";
        public string Esteopuntoconexion = "ESTEOPUNTOCONEXION";
        public string Esteoanospuestaservicio = "ESTEOANOSPUESTASERVICIO";
        public string Esteootros = "ESTEOOTROS";
        public string Esteoobs = "ESTEOOBS";
        public string Esteofechaini = "ESTEOFECHAINI";
        public string Esteoresumenejecutivotit = "ESTEORESUMENEJECUTIVOTIT";
        public string Esteoresumenejecutivoenl = "ESTEORESUMENEJECUTIVOENL";
        public string Esteofechafin = "ESTEOFECHAFIN";
        public string Esteocertconformidadtit = "ESTEOCERTCONFORMIDADTIT";
        public string Esteocertconformidadenl = "ESTEOCERTCONFORMIDADENL";
        public string Esteoplazorevcoesporv = "ESTEOPLAZOREVCOESPORV";
        public string Esteoplazorevcoesvenc = "ESTEOPLAZOREVCOESVENC";
        public string Esteoplazolevobsporv = "ESTEOPLAZOLEVOBSPORV";
        public string Esteoplazolevobsvenc = "ESTEOPLAZOLEVOBSVENC";
        public string Esteoplazoalcancesvenc = "ESTEOPLAZOALCANCESVENC";
        public string Esteoplazoverificacionvenc = "ESTEOPLAZOVERIFICACIONVENC";
        public string Esteoplazorevterinvporv = "ESTEOPLAZOREVTERINVPORV";
        public string Esteoplazorevterinvvenc = "ESTEOPLAZOREVTERINVVENC";
        public string Esteoplazoenvestterinvporv = "ESTEOPLAZOENVESTTERINVPORV";
        public string Esteoplazoenvestterinvvenc = "ESTEOPLAZOENVESTTERINVVENC";
        public string Esteoalcancefechaini = "ESTEOALCANCEFECHAINI";
        public string Esteoalcancesolesttit = "ESTEOALCANCESOLESTTIT";
        public string Esteoalcancesolestenl = "ESTEOALCANCESOLESTENL";
        public string Esteoalcancesolestobs = "ESTEOALCANCESOLESTOBS";
        public string Esteoalcancefechafin = "ESTEOALCANCEFECHAFIN";
        public string Esteoalcanceenviotit = "ESTEOALCANCEENVIOTIT";
        public string Esteoalcanceenvioenl = "ESTEOALCANCEENVIOENL";
        public string Esteoalcanceenvioobs = "ESTEOALCANCEENVIOOBS";
        public string Esteoverifechaini = "ESTEOVERIFECHAINI";
        public string Esteoverientregaesttit = "ESTEOVERIENTREGAESTTIT";
        public string Esteoverientregaestenl = "ESTEOVERIENTREGAESTENL";
        public string Esteoverientregaestobs = "ESTEOVERIENTREGAESTOBS";
        public string Esteoverifechafin = "ESTEOVERIFECHAFIN";
        public string Esteovericartatit = "ESTEOVERICARTATIT";
        public string Esteovericartaenl = "ESTEOVERICARTAENL";
        public string Esteovericartaobs = "ESTEOVERICARTAOBS";
        public string Esteopuestaenservfecha = "ESTEOPUESTAENSERVFECHA";
        public string Esteopuestaenservcomentario = "ESTEOPUESTAENSERVCOMENTARIO";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Esteojustificacion = "ESTEOJUSTIFICACION";
        public string Esteoacumdiascoes = "ESTEOACUMDIASCOES";
        public string Usercode = "USERCODE";
        public string Esteocodiproy = "Esteocodiproy";
        public string Esteoresponsable = "Esteoresponsable";
        public string Tipoconfig = "TIPOCONFIG";
        public string PuntCodi = "PUNTCODI";



        public string EsteoAbsTit = "ESTEOABSTIT";
        public string EsteoAbsEnl = "ESTEOABSENL";
        public string EsteoAbsFFin = "ESTEOABSFFIN";
        public string EsteoAbsObs = "ESTEOABSOBS";



        #endregion

        public string Estadescripcion = "estadescripcion";
        public string Emprnomb = "emprnomb";
        public string Username = "username";
        public string Terceroinvolucrado = "terceroinvolucrado";

        public string Esteorespes = "username";
        public string Esteorescodi = "usercode";
        public string EsteoTipoProyecto = "ESTEOTIPOPROYECTO";

        public string Esteoplazoverificacionvencabs = "ESTEOPLAZOVERIFICACIONVENCABS";
        #region Mejoras EO-EPO 
        public string EsteoVigencia = "ESTEOVIGENCIA";
        #endregion
        #region Mejoras EO-EPO-II
        public string ZonDescripcion = "ZONDESCRIPCION";
        #endregion

        #region Ficha Tecnica 2023
        public string SqlListarPorEmpresa
        {
            get { return base.GetSqlXml("ListarPorEmpresa"); }
        }
        
        #endregion
    }
}
