using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_INSUMO_LOG
    /// </summary>
    public class IndInsumoLogHelper : HelperBase
    {
        public IndInsumoLogHelper() : base(Consultas.IndInsumoLogSql)
        {
        }

        public IndInsumoLogDTO Create(IDataReader dr)
        {
            IndInsumoLogDTO entity = new IndInsumoLogDTO();

            int iIlogcodi = dr.GetOrdinal(this.Ilogcodi);
            if (!dr.IsDBNull(iIlogcodi)) entity.Ilogcodi = Convert.ToInt32(dr.GetValue(iIlogcodi));

            int iIrptcodi = dr.GetOrdinal(this.Irptcodi);
            if (!dr.IsDBNull(iIrptcodi)) entity.Irptcodi = Convert.ToInt32(dr.GetValue(iIrptcodi));

            int iIloginsumo = dr.GetOrdinal(this.Iloginsumo);
            if (!dr.IsDBNull(iIloginsumo)) entity.Iloginsumo = Convert.ToInt32(dr.GetValue(iIloginsumo));

            int iIlogcodigo = dr.GetOrdinal(this.Ilogcodigo);
            if (!dr.IsDBNull(iIlogcodigo)) entity.Ilogcodigo = Convert.ToInt32(dr.GetValue(iIlogcodigo));

            return entity;
        }

        #region Mapeo de Campos

        public string Ilogcodi = "ILOGCODI";
        public string Irptcodi = "IRPTCODI";
        public string Iloginsumo = "ILOGINSUMO";
        public string Ilogcodigo = "ILOGCODIGO";

        #endregion
    }
}
