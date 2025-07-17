using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_RELACION_RPT
    /// </summary>
    public class IndRelacionRptHelper : HelperBase
    {
        public IndRelacionRptHelper() : base(Consultas.IndRelacionRptSql)
        {
        }

        public IndRelacionRptDTO Create(IDataReader dr)
        {
            IndRelacionRptDTO entity = new IndRelacionRptDTO();

            int iIrelrpcodi = dr.GetOrdinal(this.Irelrpcodi);
            if (!dr.IsDBNull(iIrelrpcodi)) entity.Irelrpcodi = Convert.ToInt32(dr.GetValue(iIrelrpcodi));

            int iIrelrpidprinc = dr.GetOrdinal(this.Irelrpidprinc);
            if (!dr.IsDBNull(iIrelrpidprinc)) entity.Irelrpidprinc = Convert.ToInt32(dr.GetValue(iIrelrpidprinc));

            int iIrelpridsec = dr.GetOrdinal(this.Irelpridsec);
            if (!dr.IsDBNull(iIrelpridsec)) entity.Irelpridsec = Convert.ToInt32(dr.GetValue(iIrelpridsec));

            return entity;
        }


        #region Mapeo de Campos

        public string Irelrpcodi = "IRELRPCODI";
        public string Irelrpidprinc = "IRELRPIDPRINC";
        public string Irelpridsec = "IRELPRIDSEC";

        #endregion
    }
}
