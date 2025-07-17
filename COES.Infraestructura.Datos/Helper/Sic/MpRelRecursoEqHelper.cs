using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_REL_RECURSO_EQ
    /// </summary>
    public class MpRelRecursoEqHelper : HelperBase
    {
        public MpRelRecursoEqHelper(): base(Consultas.MpRelRecursoEqSql)
        {
        }

        public MpRelRecursoEqDTO Create(IDataReader dr)
        {
            MpRelRecursoEqDTO entity = new MpRelRecursoEqDTO();

            int iMtopcodi = dr.GetOrdinal(this.Mtopcodi);
            if (!dr.IsDBNull(iMtopcodi)) entity.Mtopcodi = Convert.ToInt32(dr.GetValue(iMtopcodi));

            int iMrecurcodi = dr.GetOrdinal(this.Mrecurcodi);
            if (!dr.IsDBNull(iMrecurcodi)) entity.Mrecurcodi = Convert.ToInt32(dr.GetValue(iMrecurcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iMreqfactor = dr.GetOrdinal(this.Mreqfactor);
            if (!dr.IsDBNull(iMreqfactor)) entity.Mreqfactor = dr.GetDecimal(iMreqfactor);

            return entity;
        }


        #region Mapeo de Campos

        public string Mtopcodi = "MTOPCODI";
        public string Mrecurcodi = "MRECURCODI";
        public string Equicodi = "EQUICODI";
        public string Mreqfactor = "MREQFACTOR";

        #endregion
        public string SqlListarByTopologia
        {
            get { return base.GetSqlXml("ListarByTopologia"); }
        }

        public string SqlListarByTopologiaYRecurso
        {
            get { return base.GetSqlXml("ListarByTopologiaYRecurso"); }
        }
        
    }
}
