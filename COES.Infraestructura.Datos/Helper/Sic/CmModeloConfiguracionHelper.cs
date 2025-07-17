using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_MODELO_CONFIGURACION
    /// </summary>
    public class CmModeloConfiguracionHelper : HelperBase
    {
        public CmModeloConfiguracionHelper() : base(Consultas.CmModeloConfiguracionSql)
        {
        }

        public CmModeloConfiguracionDTO Create(IDataReader dr)
        {
            CmModeloConfiguracionDTO entity = new CmModeloConfiguracionDTO();

            int iModconcodi = dr.GetOrdinal(this.Modconcodi);
            if (!dr.IsDBNull(iModconcodi)) entity.Modconcodi = Convert.ToInt32(dr.GetValue(iModconcodi));

            int iModagrcodi = dr.GetOrdinal(this.Modagrcodi);
            if (!dr.IsDBNull(iModagrcodi)) entity.Modagrcodi = Convert.ToInt32(dr.GetValue(iModagrcodi));

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iModcontipo = dr.GetOrdinal(this.Modcontipo);
            if (!dr.IsDBNull(iModcontipo)) entity.Modcontipo = dr.GetString(iModcontipo);

            int iModconsigno = dr.GetOrdinal(this.Modconsigno);
            if (!dr.IsDBNull(iModconsigno)) entity.Modconsigno = dr.GetString(iModconsigno);

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Modconcodi = "MODCONCODI";
        public string Modagrcodi = "MODAGRCODI";
        public string Recurcodi = "RECURCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Equicodi = "EQUICODI";
        public string Modcontipo = "MODCONTIPO";
        public string Modconsigno = "MODCONSIGNO";
        public string Topcodi = "TOPCODI";

        #endregion
    }
}
