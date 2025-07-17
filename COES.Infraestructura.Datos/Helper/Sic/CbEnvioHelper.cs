using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_ENVIO
    /// </summary>
    public class CbEnvioHelper : HelperBase
    {
        public CbEnvioHelper() : base(Consultas.CbEnvioSql)
        {
        }

        public CbEnvioDTO Create(IDataReader dr)
        {
            CbEnvioDTO entity = new CbEnvioDTO();

            int iCbenvcodi = dr.GetOrdinal(this.Cbenvcodi);
            if (!dr.IsDBNull(iCbenvcodi)) entity.Cbenvcodi = Convert.ToInt32(dr.GetValue(iCbenvcodi));

            int iCbenvfecsolicitud = dr.GetOrdinal(this.Cbenvfecsolicitud);
            if (!dr.IsDBNull(iCbenvfecsolicitud)) entity.Cbenvfecsolicitud = dr.GetDateTime(iCbenvfecsolicitud);

            int iCbenvususolicitud = dr.GetOrdinal(this.Cbenvususolicitud);
            if (!dr.IsDBNull(iCbenvususolicitud)) entity.Cbenvususolicitud = dr.GetString(iCbenvususolicitud);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCbenvfecaprobacion = dr.GetOrdinal(this.Cbenvfecaprobacion);
            if (!dr.IsDBNull(iCbenvfecaprobacion)) entity.Cbenvfecaprobacion = dr.GetDateTime(iCbenvfecaprobacion);

            int iCbenvusuaprobacion = dr.GetOrdinal(this.Cbenvusuaprobacion);
            if (!dr.IsDBNull(iCbenvusuaprobacion)) entity.Cbenvusuaprobacion = dr.GetString(iCbenvusuaprobacion);

            int iCbenvestado = dr.GetOrdinal(this.Cbenvestado);
            if (!dr.IsDBNull(iCbenvestado)) entity.Cbenvestado = dr.GetString(iCbenvestado);

            int iCbenvplazo = dr.GetOrdinal(this.Cbenvplazo);
            if (!dr.IsDBNull(iCbenvplazo)) entity.Cbenvplazo = dr.GetString(iCbenvplazo);

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iCbenvfecpreciovigente = dr.GetOrdinal(this.Cbenvfecpreciovigente);
            if (!dr.IsDBNull(iCbenvfecpreciovigente)) entity.Cbenvfecpreciovigente = dr.GetDateTime(iCbenvfecpreciovigente);

            int iEstcomcodi = dr.GetOrdinal(this.Estcomcodi);
            if (!dr.IsDBNull(iEstcomcodi)) entity.Estcomcodi = Convert.ToInt32(dr.GetValue(iEstcomcodi));

            //int iCbenvmoneda = dr.GetOrdinal(this.Cbenvmoneda);
            //if (!dr.IsDBNull(iCbenvmoneda)) entity.Cbenvmoneda = dr.GetString(iCbenvmoneda);

            int iCbenvunidad = dr.GetOrdinal(this.Cbenvunidad);
            if (!dr.IsDBNull(iCbenvunidad)) entity.Cbenvunidad = dr.GetString(iCbenvunidad);

            int iCbenvfecfinrptasolicitud = dr.GetOrdinal(this.Cbenvfecfinrptasolicitud);
            if (!dr.IsDBNull(iCbenvfecfinrptasolicitud)) entity.Cbenvfecfinrptasolicitud = dr.GetDateTime(iCbenvfecfinrptasolicitud);

            int iCbenvfecfinsubsanarobs = dr.GetOrdinal(this.Cbenvfecfinsubsanarobs);
            if (!dr.IsDBNull(iCbenvfecfinsubsanarobs)) entity.Cbenvfecfinsubsanarobs = dr.GetDateTime(iCbenvfecfinsubsanarobs);

            int iCbenvfecmodificacion = dr.GetOrdinal(this.Cbenvfecmodificacion);
            if (!dr.IsDBNull(iCbenvfecmodificacion)) entity.Cbenvfecmodificacion = dr.GetDateTime(iCbenvfecmodificacion);

            int iCbenvobs = dr.GetOrdinal(this.Cbenvobs);
            if (!dr.IsDBNull(iCbenvobs)) entity.Cbenvobs = dr.GetString(iCbenvobs);

            int iCbenvfecampl = dr.GetOrdinal(this.Cbenvfecampl);
            if (!dr.IsDBNull(iCbenvfecampl)) entity.Cbenvfecampl = dr.GetDateTime(iCbenvfecampl);

            int iCbenvitem106 = dr.GetOrdinal(this.Cbenvitem106);
            if (!dr.IsDBNull(iCbenvitem106)) entity.Cbenvitem106 = dr.GetString(iCbenvitem106);

            int iCbenvTipoCentral = dr.GetOrdinal(this.CbenvTipoCentral);
            if (!dr.IsDBNull(iCbenvTipoCentral)) entity.Cbenvtipocentral = dr.GetString(iCbenvTipoCentral);

            int iCbenvfechaPeriodo = dr.GetOrdinal(this.CbenvfechaPeriodo);
            if (!dr.IsDBNull(iCbenvfechaPeriodo)) entity.Cbenvfechaperiodo = dr.GetDateTime(iCbenvfechaPeriodo);

            int iCbenvfecsistema = dr.GetOrdinal(this.Cbenvfecsistema);
            if (!dr.IsDBNull(iCbenvfecsistema)) entity.Cbenvfecsistema = dr.GetDateTime(iCbenvfecsistema);

            int iCbftcodi = dr.GetOrdinal(this.Cbftcodi);
            if (!dr.IsDBNull(iCbftcodi)) entity.Cbftcodi = Convert.ToInt32(dr.GetValue(iCbftcodi));

            int iCbenvtipocarga = dr.GetOrdinal(this.Cbenvtipocarga);
            if (!dr.IsDBNull(iCbenvtipocarga)) entity.Cbenvtipocarga = dr.GetString(iCbenvtipocarga);

            int iCbenvusucarga = dr.GetOrdinal(this.Cbenvusucarga);
            if (!dr.IsDBNull(iCbenvusucarga)) entity.Cbenvusucarga = dr.GetString(iCbenvusucarga);

            int iCbenvtipoenvio = dr.GetOrdinal(this.Cbenvtipoenvio);
            if (!dr.IsDBNull(iCbenvtipoenvio)) entity.Cbenvtipoenvio = Convert.ToInt32(dr.GetValue(iCbenvtipoenvio));

            return entity;
        }

        #region Mapeo de Campos

        public string Cbenvcodi = "CBENVCODI";
        public string Cbenvfecsolicitud = "CBENVFECSOLICITUD";
        public string Cbenvususolicitud = "CBENVUSUSOLICITUD";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Cbenvfecaprobacion = "CBENVFECAPROBACION";
        public string Cbenvusuaprobacion = "CBENVUSUAPROBACION";
        public string Cbenvestado = "CBENVESTADO";
        public string Cbenvplazo = "CBENVPLAZO";
        public string Fenergcodi = "FENERGCODI";
        public string Estenvcodi = "ESTENVCODI";
        public string Cbenvfecpreciovigente = "CBENVFECPRECIOVIGENTE";
        public string Estcomcodi = "ESTCOMCODI";
        public string Cbenvmoneda = "CBENVMONEDA";
        public string Cbenvunidad = "CBENVUNIDAD";
        public string Cbenvfecfinrptasolicitud = "CBENVFECFINRPTASOLICITUD";
        public string Cbenvfecfinsubsanarobs = "CBENVFECFINSUBSANAROBS";
        public string Cbenvfecmodificacion = "CBENVFECMODIFICACION";
        public string Cbenvobs = "Cbenvobs";
        public string Cbenvfecampl = "Cbenvfecampl";
        public string Cbenvitem106 = "Cbenvitem106";
        public string CbenvTipoCentral = "CBENVTIPOCENTRAL";
        public string CbenvfechaPeriodo = "CBENVFECHAPERIODO";
        public string Cbenvfecsistema = "CBENVFECSISTEMA";
        public string Cbenvtipocarga = "Cbenvtipocarga";
        public string Cbenvusucarga = "Cbenvusucarga";
        public string Cbenvtipoenvio = "CBENVTIPOENVIO";

        public string Cbftcodi = "CBFTCODI";
        public string Emprnomb = "Emprnomb";
        public string Equinomb = "Equinomb";
        public string Gruponomb = "Gruponomb";
        public string Estenvnomb = "Estenvnomb";
        public string Estcomnomb = "Estcomnomb";
        public string Fenergnomb = "Fenergnomb";

        #endregion

        public string SqlObtenerTotalXFiltro
        {
            get { return GetSqlXml("ObtenerTotalXFiltro"); }
        }

        public string SqlListXEstado
        {
            get { return GetSqlXml("ListXEstado"); }
        }
        public string SqlListaEnvios
        {
            get { return GetSqlXml("ListaEnvios"); }
        }
        public string SqlGetByTipoCombustibleYVigenciaYTipocentral
        {
            get { return GetSqlXml("GetByTipoCombustibleYVigenciaYTipocentral"); }
        }

        public string SqlListaAutoguardados
        {
            get { return GetSqlXml("ListaAutoguardados"); }
        }

        public string SqlCambiarEstado
        {
            get { return GetSqlXml("CambiarEstado"); }
        }

        public string SqlGetDatosReporteCumplimiento
        {
            get { return GetSqlXml("GetDatosReporteCumplimiento"); }
        }

        public string SqlListaEnviosXPeriodo
        {
            get { return GetSqlXml("ListaEnviosXPeriodo"); }
        }

        public string SqlGetMaxIdAutoguardado
        {
            get { return GetSqlXml("GetMaxIdAutoguardado"); }
        }
    }
}
