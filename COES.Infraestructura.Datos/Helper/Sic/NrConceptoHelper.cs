using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla NR_CONCEPTO
    /// </summary>
    public class NrConceptoHelper : HelperBase
    {
        public NrConceptoHelper(): base(Consultas.NrConceptoSql)
        {
        }

        public NrConceptoDTO Create(IDataReader dr)
        {
            NrConceptoDTO entity = new NrConceptoDTO();

            int iNrcptcodi = dr.GetOrdinal(this.Nrcptcodi);
            if (!dr.IsDBNull(iNrcptcodi)) entity.Nrcptcodi = Convert.ToInt32(dr.GetValue(iNrcptcodi));

            int iNrsmodcodi = dr.GetOrdinal(this.Nrsmodcodi);
            if (!dr.IsDBNull(iNrsmodcodi)) entity.Nrsmodcodi = Convert.ToInt32(dr.GetValue(iNrsmodcodi));

            int iNrcptabrev = dr.GetOrdinal(this.Nrcptabrev);
            if (!dr.IsDBNull(iNrcptabrev)) entity.Nrcptabrev = dr.GetString(iNrcptabrev);

            int iNrcptdescripcion = dr.GetOrdinal(this.Nrcptdescripcion);
            if (!dr.IsDBNull(iNrcptdescripcion)) entity.Nrcptdescripcion = dr.GetString(iNrcptdescripcion);

            int iNrcptorden = dr.GetOrdinal(this.Nrcptorden);
            if (!dr.IsDBNull(iNrcptorden)) entity.Nrcptorden = Convert.ToInt32(dr.GetValue(iNrcptorden));

            int iNrcpteliminado = dr.GetOrdinal(this.Nrcpteliminado);
            if (!dr.IsDBNull(iNrcpteliminado)) entity.Nrcpteliminado = dr.GetString(iNrcpteliminado);

            int iNrcptpadre = dr.GetOrdinal(this.Nrcptpadre);
            if (!dr.IsDBNull(iNrcptpadre)) entity.Nrcptpadre = Convert.ToInt32(dr.GetValue(iNrcptpadre));

            return entity;
        }


        #region Mapeo de Campos

        public string Nrcptcodi = "NRCPTCODI";
        public string Nrsmodcodi = "NRSMODCODI";
        public string Nrcptabrev = "NRCPTABREV";
        public string Nrcptdescripcion = "NRCPTDESCRIPCION";
        public string Nrcptorden = "NRCPTORDEN";
        public string Nrcpteliminado = "NRCPTELIMINADO";
        public string Nrcptpadre = "NRCPTPADRE";
        public string Nrsmodnombre = "NRSMODNOMBRE";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlListaSubModuloConcepto
        {
            get { return base.GetSqlXml("ListaSubModuloConcepto"); }
        }

        #endregion
    }
}
