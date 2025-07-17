using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_DETALLEETAPA
    /// </summary>
    public class CpDetalleetapaHelper : HelperBase
    {
        public CpDetalleetapaHelper(): base(Consultas.CpDetalleetapaSql)
        {
        }

        public CpDetalleetapaDTO Create(IDataReader dr)
        {
            CpDetalleetapaDTO entity = new CpDetalleetapaDTO();

            int iEtpini = dr.GetOrdinal(this.Etpini);
            if (!dr.IsDBNull(iEtpini)) entity.Etpini = dr.GetInt32(iEtpini);

            int iEtpdelta = dr.GetOrdinal(this.Etpdelta);
            if (!dr.IsDBNull(iEtpdelta)) entity.Etpdelta = dr.GetDecimal(iEtpdelta);

            int iEtpfin = dr.GetOrdinal(this.Etpfin);
            if (!dr.IsDBNull(iEtpfin)) entity.Etpfin = dr.GetInt32(iEtpfin);

            int iEtpbloque = dr.GetOrdinal(this.Etpbloque);
            if (!dr.IsDBNull(iEtpbloque)) entity.Etpbloque = Convert.ToInt32(dr.GetValue(iEtpbloque));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Etpini = "ETPINI";
        public string Etpdelta = "ETPDELTA";
        public string Etpfin = "ETPFIN";
        public string Etpbloque = "ETPBLOQUE";
        public string Topcodi = "TOPCODI";

        #endregion

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }

        public string SqlListarPorTopologia
        {
            get { return base.GetSqlXml("ListarPorTopologias"); }
        }
        
    }
}
