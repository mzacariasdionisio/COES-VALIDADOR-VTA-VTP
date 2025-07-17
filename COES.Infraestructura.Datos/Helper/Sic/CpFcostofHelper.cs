using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_FCOSTOF
    /// </summary>
    public class CpFcostofHelper : HelperBase
    {
        public CpFcostofHelper(): base(Consultas.CpFcostofSql)
        {
        }

        public CpFcostofDTO Create(IDataReader dr)
        {
            CpFcostofDTO entity = new CpFcostofDTO();

            int iFcfembalses = dr.GetOrdinal(this.Fcfembalses);
            if (!dr.IsDBNull(iFcfembalses)) entity.Fcfembalses = dr.GetString(iFcfembalses);

            int iFcfnumcortes = dr.GetOrdinal(this.Fcfnumcortes);
            if (!dr.IsDBNull(iFcfnumcortes)) entity.Fcfnumcortes = Convert.ToInt32(dr.GetValue(iFcfnumcortes));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Fcfembalses = "FCFEMBALSES";
        public string Fcfnumcortes = "FCFNUMCORTES";
        public string Topcodi = "TOPCODI";

        #endregion

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }

    }
}
