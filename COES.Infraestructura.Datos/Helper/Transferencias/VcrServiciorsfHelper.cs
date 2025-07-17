using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_SERVICIORSF
    /// </summary>
    public class VcrServiciorsfHelper : HelperBase
    {
        public VcrServiciorsfHelper(): base(Consultas.VcrServiciorsfSql)
        {
        }

        public VcrServiciorsfDTO Create(IDataReader dr)
        {
            VcrServiciorsfDTO entity = new VcrServiciorsfDTO();

            int iVcsrsfcodi = dr.GetOrdinal(this.Vcsrsfcodi);
            if (!dr.IsDBNull(iVcsrsfcodi)) entity.Vcsrsfcodi = Convert.ToInt32(dr.GetValue(iVcsrsfcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iVcsrsffecha = dr.GetOrdinal(this.Vcsrsffecha);
            if (!dr.IsDBNull(iVcsrsffecha)) entity.Vcsrsffecha = dr.GetDateTime(iVcsrsffecha);

            int iVcsrsfasignreserva = dr.GetOrdinal(this.Vcsrsfasignreserva);
            if (!dr.IsDBNull(iVcsrsfasignreserva)) entity.Vcsrsfasignreserva = dr.GetDecimal(iVcsrsfasignreserva);

            int iVcsrsfcostportun = dr.GetOrdinal(this.Vcsrsfcostportun);
            if (!dr.IsDBNull(iVcsrsfcostportun)) entity.Vcsrsfcostportun = dr.GetDecimal(iVcsrsfcostportun);

            int iVcsrsfcostotcomps = dr.GetOrdinal(this.Vcsrsfcostotcomps);
            if (!dr.IsDBNull(iVcsrsfcostotcomps)) entity.Vcsrsfcostotcomps = dr.GetDecimal(iVcsrsfcostotcomps);

            int iVcsrsfresvnosumn = dr.GetOrdinal(this.Vcsrsfresvnosumn);
            if (!dr.IsDBNull(iVcsrsfresvnosumn)) entity.Vcsrsfresvnosumn = dr.GetDecimal(iVcsrsfresvnosumn);

            int iVcsrscostotservrsf = dr.GetOrdinal(this.Vcsrscostotservrsf);
            if (!dr.IsDBNull(iVcsrscostotservrsf)) entity.Vcsrscostotservrsf = dr.GetDecimal(iVcsrscostotservrsf);

            int iVcsrsfusucreacion = dr.GetOrdinal(this.Vcsrsfusucreacion);
            if (!dr.IsDBNull(iVcsrsfusucreacion)) entity.Vcsrsfusucreacion = dr.GetString(iVcsrsfusucreacion);

            int iVcsrsffeccreacion = dr.GetOrdinal(this.Vcsrsffeccreacion);
            if (!dr.IsDBNull(iVcsrsffeccreacion)) entity.Vcsrsffeccreacion = dr.GetDateTime(iVcsrsffeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcsrsfcodi = "VCSRSFCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Vcsrsffecha = "VCSRSFFECHA";
        public string Vcsrsfasignreserva = "VCSRSFASIGNRESERVA";
        public string Vcsrsfcostportun = "VCSRSFCOSTPORTUN";
        public string Vcsrsfcostotcomps = "VCSRSFCOSTOTCOMPS";
        public string Vcsrsfresvnosumn = "VCSRSFRESVNOSUMN";
        public string Vcsrscostotservrsf = "VCSRSCOSTOTSERVRSF";
        public string Vcsrsfusucreacion = "VCSRSFUSUCREACION";
        public string Vcsrsffeccreacion = "VCSRSFFECCREACION";

        #endregion

        public string SqlGetByIdValoresDia
        {
            get { return base.GetSqlXml("GetByIdValoresDia"); }
        }
    }
}
