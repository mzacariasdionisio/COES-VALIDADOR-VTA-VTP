using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO
    /// </summary>
    public class FtExtEnvioHelper : HelperBase
    {
        public FtExtEnvioHelper() : base(Consultas.FtExtEnvioSql)
        {
        }

        public FtExtEnvioDTO Create(IDataReader dr)
        {
            FtExtEnvioDTO entity = new FtExtEnvioDTO();

            int iFtetcodi = dr.GetOrdinal(this.Ftetcodi);
            if (!dr.IsDBNull(iFtetcodi)) entity.Ftetcodi = Convert.ToInt32(dr.GetValue(iFtetcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFtprycodi = dr.GetOrdinal(this.Ftprycodi);
            if (!dr.IsDBNull(iFtprycodi)) entity.Ftprycodi = Convert.ToInt32(dr.GetValue(iFtprycodi));

            int iFtenvcodi = dr.GetOrdinal(this.Ftenvcodi);
            if (!dr.IsDBNull(iFtenvcodi)) entity.Ftenvcodi = Convert.ToInt32(dr.GetValue(iFtenvcodi));

            int iFtenvfecsolicitud = dr.GetOrdinal(this.Ftenvfecsolicitud);
            if (!dr.IsDBNull(iFtenvfecsolicitud)) entity.Ftenvfecsolicitud = dr.GetDateTime(iFtenvfecsolicitud);

            int iFtenvususolicitud = dr.GetOrdinal(this.Ftenvususolicitud);
            if (!dr.IsDBNull(iFtenvususolicitud)) entity.Ftenvususolicitud = dr.GetString(iFtenvususolicitud);

            int iFtenvfecaprobacion = dr.GetOrdinal(this.Ftenvfecaprobacion);
            if (!dr.IsDBNull(iFtenvfecaprobacion)) entity.Ftenvfecaprobacion = dr.GetDateTime(iFtenvfecaprobacion);

            int iFtenvusuaprobacion = dr.GetOrdinal(this.Ftenvusuaprobacion);
            if (!dr.IsDBNull(iFtenvusuaprobacion)) entity.Ftenvusuaprobacion = dr.GetString(iFtenvusuaprobacion);

            int iFtenvfecfinrptasolicitud = dr.GetOrdinal(this.Ftenvfecfinrptasolicitud);
            if (!dr.IsDBNull(iFtenvfecfinrptasolicitud)) entity.Ftenvfecfinrptasolicitud = dr.GetDateTime(iFtenvfecfinrptasolicitud);

            int iFtenvfecfinsubsanarobs = dr.GetOrdinal(this.Ftenvfecfinsubsanarobs);
            if (!dr.IsDBNull(iFtenvfecfinsubsanarobs)) entity.Ftenvfecfinsubsanarobs = dr.GetDateTime(iFtenvfecfinsubsanarobs);

            int iFtenvtipoenvio = dr.GetOrdinal(this.Ftenvtipoenvio);
            if (!dr.IsDBNull(iFtenvtipoenvio)) entity.Ftenvtipoenvio = Convert.ToInt32(dr.GetValue(iFtenvtipoenvio));

            int iFtevcodi = dr.GetOrdinal(this.Ftevcodi);
            if (!dr.IsDBNull(iFtevcodi)) entity.Ftevcodi = Convert.ToInt32(dr.GetValue(iFtevcodi));

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iFtenvusumodificacion = dr.GetOrdinal(this.Ftenvusumodificacion);
            if (!dr.IsDBNull(iFtenvusumodificacion)) entity.Ftenvusumodificacion = dr.GetString(iFtenvusumodificacion);

            int iFtenvfecmodificacion = dr.GetOrdinal(this.Ftenvfecmodificacion);
            if (!dr.IsDBNull(iFtenvfecmodificacion)) entity.Ftenvfecmodificacion = dr.GetDateTime(iFtenvfecmodificacion);

            int iFtenvtipoformato = dr.GetOrdinal(this.Ftenvtipoformato);
            if (!dr.IsDBNull(iFtenvtipoformato)) entity.Ftenvtipoformato = Convert.ToInt32(dr.GetValue(iFtenvtipoformato));

            int iFtenvobs = dr.GetOrdinal(this.Ftenvobs);
            if (!dr.IsDBNull(iFtenvobs)) entity.Ftenvobs = dr.GetString(iFtenvobs);

            int iFtenvfecvigencia = dr.GetOrdinal(this.Ftenvfecvigencia);
            if (!dr.IsDBNull(iFtenvfecvigencia)) entity.Ftenvfecvigencia = dr.GetDateTime(iFtenvfecvigencia);

            int iFtenvfecsistema = dr.GetOrdinal(this.Ftenvfecsistema);
            if (!dr.IsDBNull(iFtenvfecsistema)) entity.Ftenvfecsistema = dr.GetDateTime(iFtenvfecsistema);

            int iFtenvfecampliacion = dr.GetOrdinal(this.Ftenvfecampliacion);
            if (!dr.IsDBNull(iFtenvfecampliacion)) entity.Ftenvfecampliacion = dr.GetDateTime(iFtenvfecampliacion);

            int iFtenvfecobservacion = dr.GetOrdinal(this.Ftenvfecobservacion);
            if (!dr.IsDBNull(iFtenvfecobservacion)) entity.Ftenvfecobservacion = dr.GetDateTime(iFtenvfecobservacion);

            int iFtenvenlacesint = dr.GetOrdinal(this.Ftenvenlacesint);
            if (!dr.IsDBNull(iFtenvenlacesint)) entity.Ftenvenlacesint = dr.GetString(iFtenvenlacesint);

            int iFtenvenlacecarta = dr.GetOrdinal(this.Ftenvenlacecarta);
            if (!dr.IsDBNull(iFtenvenlacecarta)) entity.Ftenvenlacecarta = dr.GetString(iFtenvenlacecarta);

            int iFtenvenlaceotro = dr.GetOrdinal(this.Ftenvenlaceotro);
            if (!dr.IsDBNull(iFtenvenlaceotro)) entity.Ftenvenlaceotro = dr.GetString(iFtenvenlaceotro);

            int iFtenvfecinirev1 = dr.GetOrdinal(this.Ftenvfecinirev1);
            if (!dr.IsDBNull(iFtenvfecinirev1)) entity.Ftenvfecinirev1 = dr.GetDateTime(iFtenvfecinirev1);

            int iFtenvfecinirev2 = dr.GetOrdinal(this.Ftenvfecinirev2);
            if (!dr.IsDBNull(iFtenvfecinirev2)) entity.Ftenvfecinirev2 = dr.GetDateTime(iFtenvfecinirev2);

            int iFtenvtipocasoesp = dr.GetOrdinal(this.Ftenvtipocasoesp);
            if (!dr.IsDBNull(iFtenvtipocasoesp)) entity.Ftenvtipocasoesp = Convert.ToInt32(dr.GetValue(iFtenvtipocasoesp));

            int iFtenvflaghabeq = dr.GetOrdinal(this.Ftenvflaghabeq);
            if (!dr.IsDBNull(iFtenvflaghabeq)) entity.Ftenvflaghabeq = dr.GetString(iFtenvflaghabeq);

            return entity;
        }

        #region Mapeo de Campos

        public string Ftetcodi = "FTETCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ftprycodi = "FTPRYCODI";
        public string Ftenvcodi = "FTENVCODI";
        public string Ftenvfecsolicitud = "FTENVFECSOLICITUD";
        public string Ftenvususolicitud = "FTENVUSUSOLICITUD";
        public string Ftenvfecaprobacion = "FTENVFECAPROBACION";
        public string Ftenvusuaprobacion = "FTENVUSUAPROBACION";
        public string Ftenvfecfinrptasolicitud = "FTENVFECFINRPTASOLICITUD";
        public string Ftenvfecfinsubsanarobs = "FTENVFECFINSUBSANAROBS";
        public string Ftenvtipoenvio = "FTENVTIPOENVIO";
        public string Ftevcodi = "FTEVCODI";
        public string Estenvcodi = "ESTENVCODI";
        public string Ftenvfecmodificacion = "FTENVFECMODIFICACION";
        public string Ftenvusumodificacion = "FTENVUSUMODIFICACION";
        public string Ftenvfecvigencia = "FTENVFECVIGENCIA";
        public string Ftenvfecsistema = "FTENVFECSISTEMA";
        public string Ftenvfecampliacion = "FTENVFECAMPLIACION";
        public string Ftenvfecobservacion = "FTENVFECOBSERVACION";
        public string Ftenvenlacesint = "FTENVENLACESINT";
        public string Ftenvenlacecarta = "FTENVENLACECARTA";
        public string Ftenvenlaceotro = "FTENVENLACEOTRO";
        public string Ftenvfecinirev1 = "FTENVFECINIREV1";
        public string Ftenvfecinirev2 = "FTENVFECINIREV2";
        public string Ftenvtipocasoesp = "FTENVTIPOCASOESP";
        public string Ftenvflaghabeq = "FTENVFLAGHABEQ";

        public string Emprnomb = "EMPRNOMB";
        public string Ftetnombre = "FTETNOMBRE";
        public string Ftprynombre = "FTPRYNOMBRE";
        public string Ftenvtipoformato = "FTENVTIPOFORMATO"; 
        public string Ftenvobs = "FTENVOBS";

        public string Fteeqcodi = "FTEEQCODI";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Tipoelemento = "TIPOELEMENTO";
        public string Famcodi = "FAMCODI";
        public string Catecodi = "CATECODI";
        public string Nombreelemento = "NOMBREELEMENTO";
        public string Abrevelemento = "ABREVELEMENTO";
        public string Estadoelemento = "ESTADOELEMENTO";
        public string Idelemento = "IDELEMENTO";
        public string Areaelemento = "AREAELEMENTO";

        public string Envarestado = "ENVARESTADO";
        public string Faremnombre = "FAREMNOMBRE";
        public string Faremcodi = "FAREMCODI";
        public string Envarfecmaxrpta = "ENVARFECMAXRPTA";
        public string Ftevercodi = "FTEVERCODI";
        public string Estenvcodiversion = "ESTENVCODIVERSION";
        

        #endregion

        public string SqlObtenerTotalXFiltro
        {
            get { return GetSqlXml("ObtenerTotalXFiltro"); }
        }

        public string SqlListaEnvios
        {
            get { return GetSqlXml("ListaEnvios"); }
        }

        public string SqlGetMaxIdAutoguardado
        {
            get { return GetSqlXml("GetMaxIdAutoguardado"); }
        }

        public string SqlListarEnvioAutoguardado
        {
            get { return GetSqlXml("ListarEnvioAutoguardado"); }
        }

        public string SqlListarEnviosYEqNoSeleccionable
        {
            get { return GetSqlXml("ListarEnviosYEqNoSeleccionable"); }
        }

        public string SqlListarEnviosYEqAprobado
        {
            get { return GetSqlXml("ListarEnviosYEqAprobado"); }
        }

        public string SqlListaEnviosPorEstado
        {
            get { return GetSqlXml("ListaEnviosPorEstado"); }
        }

        public string SqlListaEnviosAreas
        {
            get { return GetSqlXml("ListaEnviosAreas"); }
        }

        public string SqlListarEnviosDerivadosPorCarpetaYEstado
        {
            get { return GetSqlXml("ListarEnviosDerivadosPorCarpetaYEstado"); }
        }

        public string SqlListarRelacionEnvioVersionArea
        {
            get { return GetSqlXml("ListarRelacionEnvioVersionArea"); }
        }
        
    }
}
