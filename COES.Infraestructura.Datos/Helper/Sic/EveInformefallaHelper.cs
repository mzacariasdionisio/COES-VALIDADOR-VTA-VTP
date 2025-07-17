using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_INFORMEFALLA
    /// </summary>
    public class EveInformefallaHelper : HelperBase
    {
        public EveInformefallaHelper() : base(Consultas.EveInformefallaSql)
        {
        }

        public EveInformefallaDTO Create(IDataReader dr)
        {
            EveInformefallaDTO entity = new EveInformefallaDTO();

            int iEveninfcodi = dr.GetOrdinal(this.Eveninfcodi);
            if (!dr.IsDBNull(iEveninfcodi)) entity.Eveninfcodi = Convert.ToInt32(dr.GetValue(iEveninfcodi));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEvenanio = dr.GetOrdinal(this.Evenanio);
            if (!dr.IsDBNull(iEvenanio)) entity.Evenanio = Convert.ToInt32(dr.GetValue(iEvenanio));

            int iEvencorr = dr.GetOrdinal(this.Evencorr);
            if (!dr.IsDBNull(iEvencorr)) entity.Evencorr = Convert.ToInt32(dr.GetValue(iEvencorr));

            int iEveninffechemis = dr.GetOrdinal(this.Eveninffechemis);
            if (!dr.IsDBNull(iEveninffechemis)) entity.Eveninffechemis = dr.GetDateTime(iEveninffechemis);

            int iEveninfelab = dr.GetOrdinal(this.Eveninfelab);
            if (!dr.IsDBNull(iEveninfelab)) entity.Eveninfelab = dr.GetString(iEveninfelab);

            int iEveninfrevs = dr.GetOrdinal(this.Eveninfrevs);
            if (!dr.IsDBNull(iEveninfrevs)) entity.Eveninfrevs = dr.GetString(iEveninfrevs);

            int iEveninflastuser = dr.GetOrdinal(this.Eveninflastuser);
            if (!dr.IsDBNull(iEveninflastuser)) entity.Eveninflastuser = dr.GetString(iEveninflastuser);

            int iEveninflastdate = dr.GetOrdinal(this.Eveninflastdate);
            if (!dr.IsDBNull(iEveninflastdate)) entity.Eveninflastdate = dr.GetDateTime(iEveninflastdate);

            int iEveninfemitido = dr.GetOrdinal(this.Eveninfemitido);
            if (!dr.IsDBNull(iEveninfemitido)) entity.Eveninfemitido = dr.GetString(iEveninfemitido);

            int iEveninfpfechemis = dr.GetOrdinal(this.Eveninfpfechemis);
            if (!dr.IsDBNull(iEveninfpfechemis)) entity.Eveninfpfechemis = dr.GetDateTime(iEveninfpfechemis);

            int iEveninfpelab = dr.GetOrdinal(this.Eveninfpelab);
            if (!dr.IsDBNull(iEveninfpelab)) entity.Eveninfpelab = dr.GetString(iEveninfpelab);

            int iEveninfprevs = dr.GetOrdinal(this.Eveninfprevs);
            if (!dr.IsDBNull(iEveninfprevs)) entity.Eveninfprevs = dr.GetString(iEveninfprevs);

            int iEveninfpifechemis = dr.GetOrdinal(this.Eveninfpifechemis);
            if (!dr.IsDBNull(iEveninfpifechemis)) entity.Eveninfpifechemis = dr.GetDateTime(iEveninfpifechemis);

            int iEveninfpielab = dr.GetOrdinal(this.Eveninfpielab);
            if (!dr.IsDBNull(iEveninfpielab)) entity.Eveninfpielab = dr.GetString(iEveninfpielab);

            int iEveninfpirevs = dr.GetOrdinal(this.Eveninfpirevs);
            if (!dr.IsDBNull(iEveninfpirevs)) entity.Eveninfpirevs = dr.GetString(iEveninfpirevs);

            int iEveninfpemitido = dr.GetOrdinal(this.Eveninfpemitido);
            if (!dr.IsDBNull(iEveninfpemitido)) entity.Eveninfpemitido = dr.GetString(iEveninfpemitido);

            int iEveninfpiemitido = dr.GetOrdinal(this.Eveninfpiemitido);
            if (!dr.IsDBNull(iEveninfpiemitido)) entity.Eveninfpiemitido = dr.GetString(iEveninfpiemitido);

            int iEveninfmem = dr.GetOrdinal(this.Eveninfmem);
            if (!dr.IsDBNull(iEveninfmem)) entity.Eveninfmem = dr.GetString(iEveninfmem);

            int iEveninfpiemit = dr.GetOrdinal(this.Eveninfpiemit);
            if (!dr.IsDBNull(iEveninfpiemit)) entity.Eveninfpiemit = dr.GetString(iEveninfpiemit);

            int iEveninfpemit = dr.GetOrdinal(this.Eveninfpemit);
            if (!dr.IsDBNull(iEveninfpemit)) entity.Eveninfpemit = dr.GetString(iEveninfpemit);

            int iEveninfemit = dr.GetOrdinal(this.Eveninfemit);
            if (!dr.IsDBNull(iEveninfemit)) entity.Eveninfemit = dr.GetString(iEveninfemit);

            int iEvencorrmem = dr.GetOrdinal(this.Evencorrmem);
            if (!dr.IsDBNull(iEvencorrmem)) entity.Evencorrmem = Convert.ToInt32(dr.GetValue(iEvencorrmem));

            int iEveninfmemfechemis = dr.GetOrdinal(this.Eveninfmemfechemis);
            if (!dr.IsDBNull(iEveninfmemfechemis)) entity.Eveninfmemfechemis = dr.GetDateTime(iEveninfmemfechemis);

            int iEveninfmemelab = dr.GetOrdinal(this.Eveninfmemelab);
            if (!dr.IsDBNull(iEveninfmemelab)) entity.Eveninfmemelab = dr.GetString(iEveninfmemelab);

            int iEveninfmemrevs = dr.GetOrdinal(this.Eveninfmemrevs);
            if (!dr.IsDBNull(iEveninfmemrevs)) entity.Eveninfmemrevs = dr.GetString(iEveninfmemrevs);

            int iEveninfmememit = dr.GetOrdinal(this.Eveninfmememit);
            if (!dr.IsDBNull(iEveninfmememit)) entity.Eveninfmememit = dr.GetString(iEveninfmememit);

            int iEveninfmememitido = dr.GetOrdinal(this.Eveninfmememitido);
            if (!dr.IsDBNull(iEveninfmememitido)) entity.Eveninfmememitido = dr.GetString(iEveninfmememitido);

            int iEvencorrSco = dr.GetOrdinal(this.EvencorrSco);
            if (!dr.IsDBNull(iEvencorrSco)) entity.EvencorrSco = Convert.ToInt32(dr.GetValue(iEvencorrSco));

            int iEveninfactuacion = dr.GetOrdinal(this.Eveninfactuacion);
            if (!dr.IsDBNull(iEveninfactuacion)) entity.Eveninfactuacion = dr.GetString(iEveninfactuacion);

            int iEveninfactllamado = dr.GetOrdinal(this.Eveninfactllamado);
            if (!dr.IsDBNull(iEveninfactllamado)) entity.Eveninfactllamado = dr.GetString(iEveninfactllamado);

            int iEveninfactelab = dr.GetOrdinal(this.Eveninfactelab);
            if (!dr.IsDBNull(iEveninfactelab)) entity.Eveninfactelab = dr.GetString(iEveninfactelab);

            int iEveninfactfecha = dr.GetOrdinal(this.Eveninfactfecha);
            if (!dr.IsDBNull(iEveninfactfecha)) entity.Eveninfactfecha = dr.GetDateTime(iEveninfactfecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Eveninfcodi = "EVENINFCODI";
        public string Evencodi = "EVENCODI";
        public string Evenanio = "EVENANIO";
        public string Evencorr = "EVENCORR";
        public string Eveninffechemis = "EVENINFFECHEMIS";
        public string Eveninfelab = "EVENINFELAB";
        public string Eveninfrevs = "EVENINFREVS";
        public string Eveninflastuser = "EVENINFLASTUSER";
        public string Eveninflastdate = "EVENINFLASTDATE";
        public string Eveninfemitido = "EVENINFEMITIDO";
        public string Eveninfpfechemis = "EVENINFPFECHEMIS";
        public string Eveninfpelab = "EVENINFPELAB";
        public string Eveninfprevs = "EVENINFPREVS";
        public string Eveninfpifechemis = "EVENINFPIFECHEMIS";
        public string Eveninfpielab = "EVENINFPIELAB";
        public string Eveninfpirevs = "EVENINFPIREVS";
        public string Eveninfpemitido = "EVENINFPEMITIDO";
        public string Eveninfpiemitido = "EVENINFPIEMITIDO";
        public string Eveninfmem = "EVENINFMEM";
        public string Eveninfpiemit = "EVENINFPIEMIT";
        public string Eveninfpemit = "EVENINFPEMIT";
        public string Eveninfemit = "EVENINFEMIT";
        public string Evencorrmem = "EVENCORRMEM";
        public string Eveninfmemfechemis = "EVENINFMEMFECHEMIS";
        public string Eveninfmemelab = "EVENINFMEMELAB";
        public string Eveninfmemrevs = "EVENINFMEMREVS";
        public string Eveninfmememit = "EVENINFMEMEMIT";
        public string Eveninfmememitido = "EVENINFMEMEMITIDO";
        public string EvencorrSco = "EVENCORR_SCO";
        public string Eveninfactuacion = "EVENINFACTUACION";
        public string Eveninfactllamado = "EVENINFACTLLAMADO";
        public string Eveninfactelab = "EVENINFACTELAB";
        public string Eveninfactfecha = "EVENINFACTFECHA";
        public string Evenasunto = "EVENASUNTO";
        public string Corrmem = "CORRMEM";
        public string Emprnomb = "EMPRNOMB";
        public string Tareaabrev = "TAREAABREV";
        public string Areanomb = "AREANOMB";
        public string Famabrev = "FAMABREV";
        public string Equiabrev = "EQUIABREV";
        public string Evenmwindisp = "EVENMWINDISP";
        public string Evenini = "EVENINI";
        public string ExtOsinerg = "EXTOSINERG";
        public string ObsPrelimIni = "OBSPRELIMINI";
        public string ObsPrelim = "OBSPRELIM";
        public string ObsFinal = "OBSFINAL";
        public string ObsMem = "OBSMEM";
        public string Correlativo = "CORRELATIVO";
        public string Plazo = "PLAZO";
        public string Eveninfplazodiasipi = "EVENINFPLAZODIASIPI";
        public string Eveninfplazodiasif = "EVENINFPLAZODIASIF";
        public string Eveninfplazohoraipi = "EVENINFPLAZOHORAIPI";
        public string Eveninfplazohoraif = "EVENINFPLAZOHORAIF";
        public string Eveninfplazominipi = "EVENINFPLAZOMINIPI";
        public string Eveninfplazominif = "EVENINFPLAZOMINIF";



        public string SqlValidarInformeFallaN1
        {
            get { return base.GetSqlXml("ValidarInformeFallaN1"); }
        }

        public string SqlEliminarInformeFallaN1
        {
            get { return base.GetSqlXml("EliminarInformeFallaN1"); }
        }

        public string SqlObtenerCorrelativoInformeFalla
        {
            get { return base.GetSqlXml("ObtenerCorrelativoInformeFalla"); }
        }

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlSaveEvento
        {
            get { return base.GetSqlXml("SaveEvento"); }
        }

        public string SqlObtenerAlertaInformeFalla
        {
            get { return base.GetSqlXml("ObtenerAlertaInformeFalla"); }
        }

        public string SqlMostrarEventoInformeFalla
        {
            get { return base.GetSqlXml("MostrarEventoInformeFalla"); }
        }

        public string SqlActualizarAmpliacion
        {
            get { return base.GetSqlXml("ActualizarAmpliacion"); }
        }
        #endregion
    }
}

