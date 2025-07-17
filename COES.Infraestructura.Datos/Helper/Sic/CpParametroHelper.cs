using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_PARAMETRO
    /// </summary>
    public class CpParametroHelper : HelperBase
    {
        public CpParametroHelper(): base(Consultas.CpParametroSql)
        {
        }

        public CpParametroDTO Create(IDataReader dr)
        {
            CpParametroDTO entity = new CpParametroDTO();

            int iParamcodi = dr.GetOrdinal(this.Paramcodi);
            if (!dr.IsDBNull(iParamcodi)) entity.Paramcodi = Convert.ToInt32(dr.GetValue(iParamcodi));

            int iParamnombre = dr.GetOrdinal(this.Paramnombre);
            if (!dr.IsDBNull(iParamnombre)) entity.Paramnombre = dr.GetString(iParamnombre);

            int iParamunidad = dr.GetOrdinal(this.Paramunidad);
            if (!dr.IsDBNull(iParamunidad)) entity.Paramunidad = dr.GetString(iParamunidad);

            int iParamvalor = dr.GetOrdinal(this.Paramvalor);
            if (!dr.IsDBNull(iParamvalor)) entity.Paramvalor = dr.GetString(iParamvalor);

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iParamactivo = dr.GetOrdinal(this.Paramactivo);
            if (!dr.IsDBNull(iParamactivo)) entity.Paramactivo = Convert.ToInt32(dr.GetValue(iParamactivo));

            return entity;
        }


        #region Mapeo de Campos

        public string Paramcodi = "PARAMCODI";
        public string Paramnombre = "PARAMNOMBRE";
        public string Paramunidad = "PARAMUNIDAD";
        public string Paramvalor = "PARAMVALOR";
        public string Topcodi = "TOPCODI";
        public string Paramactivo = "PARAMACTIVO";

        #endregion

        public string SqlCopiarParametroAEscenario
        {
            get { return GetSqlXml("CopiarParametroAEscenario"); }
        }
    }
}
