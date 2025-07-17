using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_SDDP_PARAMDIA
    /// </summary>
    /// 

    public class CaiSddpParametroHelper : HelperBase
    {
        public CaiSddpParametroHelper()
            : base(Consultas.CaiSddpParametroSql)
        {
        }

        public CaiSddpParametroDTO Create(IDataReader dr)
        {
            CaiSddpParametroDTO entity = new CaiSddpParametroDTO();

            int iSddppdcodi = dr.GetOrdinal(this.Sddppmcodi);
            if (!dr.IsDBNull(iSddppdcodi)) entity.Sddppmcodi = Convert.ToInt32(dr.GetValue(iSddppdcodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iSddppmtc = dr.GetOrdinal(this.Sddppmtc);
            if (!dr.IsDBNull(iSddppmtc)) entity.Sddppmtc = Convert.ToDecimal(dr.GetValue(iSddppmtc));

            int iSddppmsemini = dr.GetOrdinal(this.Sddppmsemini);
            if (!dr.IsDBNull(iSddppmsemini)) entity.Sddppmsemini = Convert.ToInt32(dr.GetValue(iSddppmsemini));

            int iSddppmnumsem = dr.GetOrdinal(this.Sddppmnumsem);
            if (!dr.IsDBNull(iSddppmnumsem)) entity.Sddppmnumsem = Convert.ToInt32(dr.GetValue(iSddppmnumsem));

            int iSddppmcantbloque = dr.GetOrdinal(this.Sddppmcantbloque);
            if (!dr.IsDBNull(iSddppmcantbloque)) entity.Sddppmcantbloque = Convert.ToInt32(iSddppmcantbloque);

            int iSddppmnumserie = dr.GetOrdinal(this.Sddppmnumserie);
            if (!dr.IsDBNull(iSddppmnumserie)) entity.Sddppmnumserie = Convert.ToInt32(dr.GetValue(iSddppmnumserie));

            int iSddppmusucreacion = dr.GetOrdinal(this.Sddppmusucreacion);
            if (!dr.IsDBNull(iSddppmusucreacion)) entity.Sddppmusucreacion = dr.GetString(iSddppmusucreacion);

            int iSddppmfeccreacion = dr.GetOrdinal(this.Sddppmfeccreacion);
            if (!dr.IsDBNull(iSddppmfeccreacion)) entity.Sddppmfeccreacion = dr.GetDateTime(iSddppmfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Sddppmcodi = "SDDPPMCODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Sddppmtc = "SDDPPMTC";
        public string Sddppmsemini = "SDDPPMSEMINI";
        public string Sddppmnumsem = "SDDPPMNUMSEM";
        public string Sddppmcantbloque = "SDDPPMCANTBLOQUE";
        public string Sddppmnumserie = "SDDPPMNUMSERIE";
        public string Sddppmusucreacion = "SDDPPMUSUCREACION";
        public string Sddppmfeccreacion = "SDDPPMFECCREACION";


        #endregion
    }
}

