using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_CONCEPTO
    /// </summary>
    public class SiConceptoHelper : HelperBase
    {
        public SiConceptoHelper()
            : base(Consultas.SiConceptoSql)
        {
        }

        public SiConceptoDTO Create(IDataReader dr)
        {
            SiConceptoDTO entity = new SiConceptoDTO();

            int iConsiscodi = dr.GetOrdinal(this.Consiscodi);
            if (!dr.IsDBNull(iConsiscodi)) entity.Consiscodi = Convert.ToInt32(dr.GetValue(iConsiscodi));

            int iConsisabrev = dr.GetOrdinal(this.Consisabrev);
            if (!dr.IsDBNull(iConsisabrev)) entity.Consisabrev = dr.GetString(iConsisabrev);

            int iConsisdesc = dr.GetOrdinal(this.Consisdesc);
            if (!dr.IsDBNull(iConsisdesc)) entity.Consisdesc = dr.GetString(iConsisdesc);

            int iConsisactivo = dr.GetOrdinal(this.Consisactivo);
            if (!dr.IsDBNull(iConsisactivo)) entity.Consisactivo = dr.GetString(iConsisactivo);

            int iConsisorden = dr.GetOrdinal(this.Consisorden);
            if (!dr.IsDBNull(iConsisorden)) entity.Consisorden = Convert.ToInt32(dr.GetValue(iConsisorden));

            return entity;
        }


        #region Mapeo de Campos

        public string Consiscodi = "CONSISCODI";
        public string Consisabrev = "CONSISABREV";
        public string Consisdesc = "CONSISDESC";
        public string Consisactivo = "CONSISACTIVO";
        public string Consisorden = "CONSISORDEN";

        #endregion
    }
}
