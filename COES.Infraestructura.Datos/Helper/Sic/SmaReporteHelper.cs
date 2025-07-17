using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_OFERTA
    /// </summary>
    public class SmaReporteHelper : HelperBase
    {
        public SmaReporteHelper(): base(Consultas.SmaReporteSql)
        {
        }

        public SmaReporteDTO Create(IDataReader dr)
        {
            SmaReporteDTO entity = new SmaReporteDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            return entity;
        }


        #region Mapeo de Campos

        public string   Gruponomb = "GRUPONOMB";
        public string   Grupocodi = "GRUPOCODI";
        public string   Repofecha = "REPOFECHA";
        public string   Repointvini = "REPOINTVINI";
        public string   Repointvfin = "REPOINTVFIN";
        public string Repohoraini = "REPOHORAINI";
        public string Repohorafin = "REPOHORAFIN";
        public string Urscodi = "URSCODI";
        public string Repopotmaxofer = "REPOPOTMAXOFER";
        public string Repoprecio = "REPOPRECIO";
        public string Reponrounit = "REPONROUNIT";

        #endregion


    }
}
