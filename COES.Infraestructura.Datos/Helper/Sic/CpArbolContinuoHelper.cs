using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_ARBOL_CONTINUO
    /// </summary>
    public class CpArbolContinuoHelper : HelperBase
    {
        public CpArbolContinuoHelper() : base(Consultas.CpArbolContinuoSql)
        {
        }

        public CpArbolContinuoDTO Create(IDataReader dr)
        {
            CpArbolContinuoDTO entity = new CpArbolContinuoDTO();

            int iCparbcodi = dr.GetOrdinal(this.Cparbcodi);
            if (!dr.IsDBNull(iCparbcodi)) entity.Cparbcodi = Convert.ToInt32(dr.GetValue(iCparbcodi));

            int iCparbtag = dr.GetOrdinal(this.Cparbtag);
            if (!dr.IsDBNull(iCparbtag)) entity.Cparbtag = dr.GetString(iCparbtag);

            int iCparbfecregistro = dr.GetOrdinal(this.Cparbfecregistro);
            if (!dr.IsDBNull(iCparbfecregistro)) entity.Cparbfecregistro = dr.GetDateTime(iCparbfecregistro);

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iCparbusuregistro = dr.GetOrdinal(this.Cparbusuregistro);
            if (!dr.IsDBNull(iCparbusuregistro)) entity.Cparbusuregistro = dr.GetString(iCparbusuregistro);

            int iCparbestado = dr.GetOrdinal(this.Cparbestado);
            if (!dr.IsDBNull(iCparbestado)) entity.Cparbestado = dr.GetString(iCparbestado);

            int iCparbfecha = dr.GetOrdinal(this.Cparbfecha);
            if (!dr.IsDBNull(iCparbfecha)) entity.Cparbfecha = dr.GetDateTime(iCparbfecha);

            int iCparbbloquehorario = dr.GetOrdinal(this.Cparbbloquehorario);
            if (!dr.IsDBNull(iCparbbloquehorario)) entity.Cparbbloquehorario = Convert.ToInt32(dr.GetValue(iCparbbloquehorario));

            int iCparbdetalleejec = dr.GetOrdinal(this.Cparbdetalleejec);
            if (!dr.IsDBNull(iCparbdetalleejec)) entity.Cparbdetalleejec = dr.GetString(iCparbdetalleejec);

            int iCparbidentificador = dr.GetOrdinal(this.Cparbidentificador);
            if (!dr.IsDBNull(iCparbidentificador)) entity.Cparbidentificador = dr.GetString(iCparbidentificador);

            int iCparbfeciniproceso = dr.GetOrdinal(this.Cparbfeciniproceso);
            if (!dr.IsDBNull(iCparbfeciniproceso)) entity.Cparbfeciniproceso = dr.GetDateTime(iCparbfeciniproceso);

            int iCparbfecfinproceso = dr.GetOrdinal(this.Cparbfecfinproceso);
            if (!dr.IsDBNull(iCparbfecfinproceso)) entity.Cparbfecfinproceso = dr.GetDateTime(iCparbfecfinproceso);

            int iCparbmsjproceso = dr.GetOrdinal(this.Cparbmsjproceso);
            if (!dr.IsDBNull(iCparbmsjproceso)) entity.Cparbmsjproceso = dr.GetString(iCparbmsjproceso);

            int iCparbporcentaje = dr.GetOrdinal(this.Cparbporcentaje);
            if (!dr.IsDBNull(iCparbporcentaje)) entity.Cparbporcentaje = dr.GetDecimal(iCparbporcentaje);
            
            return entity;
        }


        #region Mapeo de Campos

        public string Cparbcodi = "CPARBCODI";
        public string Cparbtag = "CPARBTAG";
        public string Cparbfecregistro = "CPARBFECREGISTRO";
        public string Topcodi = "TOPCODI";
        public string Cparbusuregistro = "CPARBUSUREGISTRO";
        public string Cparbestado = "CPARBESTADO";
        public string Cparbfecha = "CPARBFECHA";
        public string Cparbbloquehorario = "CPARBBLOQUEHORARIO";
        public string Cparbdetalleejec = "CPARBDETALLEEJEC";
        public string Cparbidentificador = "CPARBIDENTIFICADOR";
        public string Cparbfeciniproceso = "CPARBFECINIPROCESO";
        public string Cparbfecfinproceso = "CPARBFECFINPROCESO";
        public string Cparbmsjproceso = "CPARBMSJPROCESO";
        public string Cparbporcentaje = "CPARBPORCENTAJE";

        #endregion

        public string SqlGetUltimoArbol
        {
            get { return base.GetSqlXml("GetUltimoArbol"); }
        }
    }
}
