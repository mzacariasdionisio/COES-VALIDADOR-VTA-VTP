using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_GRUPO_LINEA
    /// </summary>
    public class EqGrupoLineaHelper : HelperBase
    {
        public EqGrupoLineaHelper(): base(Consultas.EqGrupoLineaSql)
        {
        }

        public EqGrupoLineaDTO Create(IDataReader dr)
        {
            EqGrupoLineaDTO entity = new EqGrupoLineaDTO();

            int iGrulincodi = dr.GetOrdinal(this.Grulincodi);
            if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

            int iGrulinnombre = dr.GetOrdinal(this.Grulinnombre);
            if (!dr.IsDBNull(iGrulinnombre)) entity.Grulinnombre = dr.GetString(iGrulinnombre);

            int iGrulinvallintrans = dr.GetOrdinal(this.Grulinvallintrans);
            if (!dr.IsDBNull(iGrulinvallintrans)) entity.Grulinvallintrans = dr.GetDecimal(iGrulinvallintrans);

            int iGrulinporlimtrans = dr.GetOrdinal(this.Grulinporlimtrans);
            if (!dr.IsDBNull(iGrulinporlimtrans)) entity.Grulinporlimtrans = dr.GetDecimal(iGrulinporlimtrans);

            int iGrulinestado = dr.GetOrdinal(this.Grulinestado);
            if (!dr.IsDBNull(iGrulinestado)) entity.Grulinestado = dr.GetString(iGrulinestado);

            int iNombrencp = dr.GetOrdinal(this.Nombrencp);
            if (!dr.IsDBNull(iNombrencp)) entity.Nombrencp = dr.GetString(iNombrencp);

            int iCodincp = dr.GetOrdinal(this.Codincp);
            if (!dr.IsDBNull(iCodincp)) entity.Codincp = Convert.ToInt32(dr.GetValue(iCodincp));

            int iGrulintipo = dr.GetOrdinal(this.Grulintipo);
            if (!dr.IsDBNull(iGrulintipo)) entity.Grulintipo = dr.GetString(iGrulintipo);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Grulincodi = "GRULINCODI";
        public string Grulinnombre = "GRULINNOMBRE";
        public string Grulinvallintrans = "GRULINVALLINTRANS";
        public string Grulinporlimtrans = "GRULINPORLIMTRANS";
        public string Grulinestado = "GRULINESTADO";
        public string Nombrencp = "NOMBRENCP";
        public string Codincp = "CODINCP";
        public string Grulintipo = "GRULINTIPO";
        public string Equicodi = "EQUICODI";

        #endregion
    }
}
