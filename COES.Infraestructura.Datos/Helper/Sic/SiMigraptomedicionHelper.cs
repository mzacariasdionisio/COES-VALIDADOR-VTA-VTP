using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MIGRAPTOMEDICION
    /// </summary>
    public class SiMigraptomedicionHelper : HelperBase
    {
        public SiMigraptomedicionHelper(): base(Consultas.SiMigraptomedicionSql)
        {
        }

        public SiMigraPtomedicionDTO Create(IDataReader dr)
        {
            SiMigraPtomedicionDTO entity = new SiMigraPtomedicionDTO();

            int iMgpmedcodi = dr.GetOrdinal(this.Mgpmedcodi);
            if (!dr.IsDBNull(iMgpmedcodi)) entity.Mgpmedcodi = Convert.ToInt32(dr.GetValue(iMgpmedcodi));

            int iMigempcodi = dr.GetOrdinal(this.Migempcodi);
            if (!dr.IsDBNull(iMigempcodi)) entity.Migempcodi = Convert.ToInt32(dr.GetValue(iMigempcodi));

            int iPtomedcodimigra = dr.GetOrdinal(this.Ptomedcodimigra);
            if (!dr.IsDBNull(iPtomedcodimigra)) entity.Ptomedcodimigra = Convert.ToInt32(dr.GetValue(iPtomedcodimigra));

            int iPtomedbajanuevo = dr.GetOrdinal(this.Ptomedbajanuevo);
            if (!dr.IsDBNull(iPtomedbajanuevo)) entity.Ptomedbajanuevo = Convert.ToInt32(dr.GetValue(iPtomedbajanuevo));

            int iMgpmedusucreacion = dr.GetOrdinal(this.Mgpmedusucreacion);
            if (!dr.IsDBNull(iMgpmedusucreacion)) entity.Mgpmedusucreacion = dr.GetString(iMgpmedusucreacion);

            int iMgpmedfeccreacion = dr.GetOrdinal(this.Mgpmedfeccreacion);
            if (!dr.IsDBNull(iMgpmedfeccreacion)) entity.Mgpmedfeccreacion = dr.GetDateTime(iMgpmedfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Mgpmedcodi = "MGPMEDCODI";
        public string Migempcodi = "MIGEMPCODI";
        public string Ptomedcodimigra = "PTOMEDCODIMIGRA";
        public string Ptomedbajanuevo = "PTOMEDBAJANUEVO";
        public string Mgpmedusucreacion = "MGPMEDUSUCREACION";
        public string Mgpmedfeccreacion = "MGPMEDFECCREACION";

        #endregion
    }
}
