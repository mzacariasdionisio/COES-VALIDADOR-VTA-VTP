using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_REL_RECURSO_SDDP
    /// </summary>
    public class MpRelRecursoSddpHelper : HelperBase
    {
        public MpRelRecursoSddpHelper(): base(Consultas.MpRelRecursoSddpSql)
        {
        }

        public MpRelRecursoSddpDTO Create(IDataReader dr)
        {
            MpRelRecursoSddpDTO entity = new MpRelRecursoSddpDTO();

            int iMtopcodi = dr.GetOrdinal(this.Mtopcodi);
            if (!dr.IsDBNull(iMtopcodi)) entity.Mtopcodi = Convert.ToInt32(dr.GetValue(iMtopcodi));

            int iMrecurcodi = dr.GetOrdinal(this.Mrecurcodi);
            if (!dr.IsDBNull(iMrecurcodi)) entity.Mrecurcodi = Convert.ToInt32(dr.GetValue(iMrecurcodi));

            int iSddpcodi = dr.GetOrdinal(this.Sddpcodi);
            if (!dr.IsDBNull(iSddpcodi)) entity.Sddpcodi = Convert.ToInt32(dr.GetValue(iSddpcodi));

            int iMrsddpfactor = dr.GetOrdinal(this.Mrsddpfactor);
            if (!dr.IsDBNull(iMrsddpfactor)) entity.Mrsddpfactor = dr.GetDecimal(iMrsddpfactor);

            return entity;
        }


        #region Mapeo de Campos

        public string Mtopcodi = "MTOPCODI";
        public string Mrecurcodi = "MRECURCODI";
        public string Sddpcodi = "SDDPCODI";
        public string Mrsddpfactor = "MRSDDPFACTOR";

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
