using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_CATEGORIA
    /// </summary>
    public class MpCategoriaHelper : HelperBase
    {
        public MpCategoriaHelper(): base(Consultas.MpCategoriaSql)
        {
        }

        public MpCategoriaDTO Create(IDataReader dr)
        {
            MpCategoriaDTO entity = new MpCategoriaDTO();

            int iMcatcodi = dr.GetOrdinal(this.Mcatcodi);
            if (!dr.IsDBNull(iMcatcodi)) entity.Mcatcodi = Convert.ToInt32(dr.GetValue(iMcatcodi));

            int iMcatnomb = dr.GetOrdinal(this.Mcatnomb);
            if (!dr.IsDBNull(iMcatnomb)) entity.Mcatnomb = dr.GetString(iMcatnomb);

            int iMcatabrev = dr.GetOrdinal(this.Mcatabrev);
            if (!dr.IsDBNull(iMcatabrev)) entity.Mcatabrev = dr.GetString(iMcatabrev);

            int iMcattipo = dr.GetOrdinal(this.Mcattipo);
            if (!dr.IsDBNull(iMcattipo)) entity.Mcattipo = Convert.ToInt32(dr.GetValue(iMcattipo));

            int iMcatdesc = dr.GetOrdinal(this.Mcatdesc);
            if (!dr.IsDBNull(iMcatdesc)) entity.Mcatdesc = dr.GetString(iMcatdesc);

            return entity;
        }


        #region Mapeo de Campos

        public string Mcatcodi = "MCATCODI";
        public string Mcatnomb = "MCATNOMB";
        public string Mcatabrev = "MCATABREV";
        public string Mcattipo = "MCATTIPO";
        public string Mcatdesc = "MCATDESC";

        #endregion
    }
}
