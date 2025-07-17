using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla F_INDICADOR
    /// </summary>
    public class FIndicadorHelper : HelperBase
    {
        public FIndicadorHelper(): base(Consultas.FIndicadorSql)
        {
        }

        public FIndicadorDTO Create(IDataReader dr)
        {
            FIndicadorDTO entity = new FIndicadorDTO();

            int iFechahora = dr.GetOrdinal(this.Fechahora);
            if (!dr.IsDBNull(iFechahora)) entity.Fechahora = dr.GetDateTime(iFechahora);

            int iGps = dr.GetOrdinal(this.Gps);
            if (!dr.IsDBNull(iGps)) entity.Gps = Convert.ToInt32(dr.GetValue(iGps));

            int iIndiccodi = dr.GetOrdinal(this.Indiccodi);
            if (!dr.IsDBNull(iIndiccodi)) entity.Indiccodi = dr.GetString(iIndiccodi);

            int iIndicitem = dr.GetOrdinal(this.Indicitem);
            if (!dr.IsDBNull(iIndicitem)) entity.Indicitem = Convert.ToInt32(dr.GetValue(iIndicitem));

            int iIndicvalor = dr.GetOrdinal(this.Indicvalor);
            if (!dr.IsDBNull(iIndicvalor)) entity.Indicvalor = dr.GetDecimal(iIndicvalor);

            return entity;
        }


        #region Mapeo de Campos

        public string Fechahora = "FECHAHORA";
        public string Gps = "GPS";
        public string Indiccodi = "INDICCODI";
        public string Indicitem = "INDICITEM";
        public string Indicvalor = "INDICVALOR";

        #region PR5
        public string Emprcodi = "EMPRCODI";
        public string Gpsnomb = "GPSNOMB";
        #endregion

        #endregion

        public string SqlGetTransgresion
        {
            get { return base.GetSqlXml("GetTransgresion"); }
        }

        public string SqlGetFallaAcumulada
        {
            get { return base.GetSqlXml("GetFallaAcumulada"); }
        }

        public string SqlGetTransgresionFrec
        {
            get { return base.GetSqlXml("GetTransgresionFrec"); }
        }

        public string SqlGetFallaAcumuladaFrec
        {
            get { return base.GetSqlXml("GetFallaAcumuladaFrec"); }
        }

        #region PR5
        public string SqlListarReporteVariacionesFrecuenciaSEIN
        {
            get { return base.GetSqlXml("ListarReporteVariacionesFrecuenciaSEIN"); }
        }

        public string SqlListarTransgresionXRango
        {
            get { return base.GetSqlXml("ListarTransgresionXRango"); }
        }

        public string SqlGetFallaAcumuladaXRango
        {
            get { return base.GetSqlXml("GetFallaAcumuladaXRango"); }
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaIndicador
        {
            get { return base.GetSqlXml("ListaIndicador"); }
        }

        public string SqlListaIndicadorAcu
        {
            get { return base.GetSqlXml("ListaIndicadorAcu"); }
        }
        #endregion
    }
}
