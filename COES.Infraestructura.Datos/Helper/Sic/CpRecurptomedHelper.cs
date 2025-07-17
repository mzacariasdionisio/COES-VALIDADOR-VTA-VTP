using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_RECURPTOMED
    /// </summary>
    public class CpRecurptomedHelper : HelperBase
    {
        public CpRecurptomedHelper(): base(Consultas.CpRecurptomedSql)
        {
        }

        public CpRecurptomedDTO Create(IDataReader dr)
        {
            CpRecurptomedDTO entity = new CpRecurptomedDTO();

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iRecptok = dr.GetOrdinal(this.Recptok);
            if (!dr.IsDBNull(iRecptok)) entity.Recptok = Convert.ToInt32(dr.GetValue(iRecptok));

            return entity;
        }


        #region Mapeo de Campos

        public string Recurcodi = "RECURCODI";
        public string Topcodi = "TOPCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Recptok = "RECPTOK";
        public string Ptomedinomb = "PTOMEDINOMB";

        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";
        public string Famabrev = "Famabrev";
        public string Famcodi = "FAMCODI";
        public string Equinomb = "Equinomb";
        public string Catnombre = "Catnombre";

        #endregion

        public string SqlGetByCategoria
        {
            get { return base.GetSqlXml("GetByCategoria"); }
        }

        public string SqlCrearCopiaRecurptomed
        {
            get { return base.GetSqlXml("CrearCopiaRecurptomed"); }
        }

        public string SqlDeleteAll
        {
            get { return base.GetSqlXml("DeleteAll"); }
        }

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }

        public string SqlListXTopologia
        {
            get { return base.GetSqlXml("ListXTopologia"); }
        }

        public string SqlListByTopcodi
        {
            get { return base.GetSqlXml("ListByTopcodi"); }
        }
    }
}
