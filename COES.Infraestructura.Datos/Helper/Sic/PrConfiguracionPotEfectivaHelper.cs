using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_CONFIGURACION_POT_EFECTIVA
    /// </summary>
    public class PrConfiguracionPotEfectivaHelper : HelperBase
    {
        public PrConfiguracionPotEfectivaHelper(): base(Consultas.PrConfiguracionPotEfectivaSql)
        {
        }

        public PrConfiguracionPotEfectivaDTO Create(IDataReader dr)
        {
            PrConfiguracionPotEfectivaDTO entity = new PrConfiguracionPotEfectivaDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iConfpeusuariocreacion = dr.GetOrdinal(this.Confpeusuariocreacion);
            if (!dr.IsDBNull(iConfpeusuariocreacion)) entity.Confpeusuariocreacion = dr.GetString(iConfpeusuariocreacion);

            int iConfpefechacreacion = dr.GetOrdinal(this.Confpefechacreacion);
            if (!dr.IsDBNull(iConfpefechacreacion)) entity.Confpefechacreacion = dr.GetDateTime(iConfpefechacreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Grupocodi = "GRUPOCODI";
        public string Confpeusuariocreacion = "CONFPEUSUARIOCREACION";
        public string Confpefechacreacion = "CONFPEFECHACREACION";

        #endregion

        public string SqlDeleteAll
        {
            get { return base.GetSqlXml("DeleteAll"); }
        }
    }
}
