using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{

    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_SDDP_PARAMINT
    /// </summary>
    /// 
    public class CaiSddpParamintHelper : HelperBase
    {
        public CaiSddpParamintHelper()
            : base(Consultas.CaiSddpParamintSql)
        {
        }

        public CaiSddpParamintDTO Create(IDataReader dr)
        {
            CaiSddpParamintDTO entity = new CaiSddpParamintDTO();

            int iSddppicodi = dr.GetOrdinal(this.Sddppicodi);
            if (!dr.IsDBNull(iSddppicodi)) entity.Sddppicodi = Convert.ToInt32(dr.GetValue(iSddppicodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iSddppiintervalo = dr.GetOrdinal(this.Sddppiintervalo);
            if (!dr.IsDBNull(iSddppiintervalo)) entity.Sddppiintervalo = dr.GetDateTime(iSddppiintervalo);

            int iSddppilaboral = dr.GetOrdinal(this.Sddppilaboral);
            if (!dr.IsDBNull(iSddppilaboral)) entity.Sddppilaboral = Convert.ToInt32(dr.GetValue(iSddppilaboral));

            int iSddppibloque = dr.GetOrdinal(this.Sddppibloque);
            if (!dr.IsDBNull(iSddppibloque)) entity.Sddppibloque = Convert.ToInt32(dr.GetValue(iSddppibloque));

            int iSddppiusucreacion = dr.GetOrdinal(this.Sddppiusucreacion);
            if (!dr.IsDBNull(iSddppiusucreacion)) entity.Sddppiusucreacion = dr.GetString(iSddppiusucreacion);

            int iSddppifeccreacion = dr.GetOrdinal(this.Sddppifeccreacion);
            if (!dr.IsDBNull(iSddppifeccreacion)) entity.Sddppifeccreacion = dr.GetDateTime(iSddppifeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Sddppicodi = "SDDPPICODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Sddppiintervalo = "SDDPPIINTERVALO";
        public string Sddppilaboral = "SDDPPILABORAL";
        public string Sddppibloque = "SDDPPIBLOQUE";
        public string Sddppiusucreacion = "SDDPPIUSUCREACION";
        public string Sddppifeccreacion = "SDDPPIFECCREACION";


        #endregion
    }
}

