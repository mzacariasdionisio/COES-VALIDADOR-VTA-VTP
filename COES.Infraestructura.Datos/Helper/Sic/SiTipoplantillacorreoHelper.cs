using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_TIPOPLANTILLACORREO
    /// </summary>
    public class SiTipoplantillacorreoHelper : HelperBase
    {
        public SiTipoplantillacorreoHelper(): base(Consultas.SiTipoplantillacorreoSql)
        {
        }

        public SiTipoplantillacorreoDTO Create(IDataReader dr)
        {
            SiTipoplantillacorreoDTO entity = new SiTipoplantillacorreoDTO();

            int iTpcorrcodi = dr.GetOrdinal(this.Tpcorrcodi);
            if (!dr.IsDBNull(iTpcorrcodi)) entity.Tpcorrcodi = Convert.ToInt32(dr.GetValue(iTpcorrcodi));

            int iTpcorrdescrip = dr.GetOrdinal(this.Tpcorrdescrip);
            if (!dr.IsDBNull(iTpcorrdescrip)) entity.Tpcorrdescrip = dr.GetString(iTpcorrdescrip);

            return entity;
        }


        #region Mapeo de Campos

        public string Tpcorrcodi = "TPCORRCODI";
        public string Tpcorrdescrip = "TPCORRDESCRIP";

        #endregion
    }
}
