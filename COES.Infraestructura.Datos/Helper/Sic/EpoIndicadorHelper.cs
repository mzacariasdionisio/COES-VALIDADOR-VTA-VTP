using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_INDICADOR
    /// </summary>
    public class EpoIndicadorHelper : HelperBase
    {
        public EpoIndicadorHelper(): base(Consultas.EpoIndicadorSql)
        {
        }

        public EpoIndicadorDTO Create(IDataReader dr)
        {
            EpoIndicadorDTO entity = new EpoIndicadorDTO();

            int iIndcodi = dr.GetOrdinal(this.Indcodi);
            if (!dr.IsDBNull(iIndcodi)) entity.Indcodi = Convert.ToInt32(dr.GetValue(iIndcodi));

            int iIndnomb = dr.GetOrdinal(this.Indnomb);
            if (!dr.IsDBNull(iIndnomb)) entity.Indnomb = dr.GetString(iIndnomb);

            int iIndmensajeleyenda = dr.GetOrdinal(this.Indmensajeleyenda);
            if (!dr.IsDBNull(iIndmensajeleyenda)) entity.Indmensajeleyenda = dr.GetString(iIndmensajeleyenda);

            int iIndformatoescalavalores = dr.GetOrdinal(this.Indformatoescalavalores);
            if (!dr.IsDBNull(iIndformatoescalavalores)) entity.Indformatoescalavalores = dr.GetString(iIndformatoescalavalores);

            int iIndintervalo = dr.GetOrdinal(this.Indintervalo);
            if (!dr.IsDBNull(iIndintervalo)) entity.Indintervalo = dr.GetDecimal(iIndintervalo);

            return entity;
        }


        #region Mapeo de Campos

        public string Indcodi = "INDCODI";
        public string Indnomb = "INDNOMB";
        public string Indmensajeleyenda = "INDMENSAJELEYENDA";
        public string Indformatoescalavalores = "INDFORMATOESCALAVALORES";
        public string Indintervalo = "INDINTERVALO";

        #endregion
    }
}
