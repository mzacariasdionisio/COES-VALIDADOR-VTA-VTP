using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_UNIDADEXONERADA
    /// </summary>
    public class VcrUnidadexoneradaHelper : HelperBase
    {
        public VcrUnidadexoneradaHelper(): base(Consultas.VcrUnidadexoneradaSql)
        {
        }

        public VcrUnidadexoneradaDTO Create(IDataReader dr)
        {
            VcrUnidadexoneradaDTO entity = new VcrUnidadexoneradaDTO();

            int iVcruexcodi = dr.GetOrdinal(this.Vcruexcodi);
            if (!dr.IsDBNull(iVcruexcodi)) entity.Vcruexcodi = Convert.ToInt32(dr.GetValue(iVcruexcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicen = dr.GetOrdinal(this.Equicodicen);
            if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

            int iEquicodiuni = dr.GetOrdinal(this.Equicodiuni);
            if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

            int iVcruexonerar = dr.GetOrdinal(this.Vcruexonerar);
            if (!dr.IsDBNull(iVcruexonerar)) entity.Vcruexonerar = dr.GetString(iVcruexonerar);

            int iVcruexobservacion = dr.GetOrdinal(this.Vcruexobservacion);
            if (!dr.IsDBNull(iVcruexobservacion)) entity.Vcruexobservacion = dr.GetString(iVcruexobservacion);

            int iVcruexusucreacion = dr.GetOrdinal(this.Vcruexusucreacion);
            if (!dr.IsDBNull(iVcruexusucreacion)) entity.Vcruexusucreacion = dr.GetString(iVcruexusucreacion);

            int iVcruexfeccreacion = dr.GetOrdinal(this.Vcruexfeccreacion);
            if (!dr.IsDBNull(iVcruexfeccreacion)) entity.Vcruexfeccreacion = dr.GetDateTime(iVcruexfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcruexcodi = "VCRUEXCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicen = "EQUICODICEN";
        public string Equicodiuni = "EQUICODIUNI";
        public string Vcruexonerar = "VCRUEXONERAR";
        public string Vcruexobservacion = "VCRUEXOBSERVACION";
        public string Vcruexusucreacion = "VCRUEXUSUCREACION";
        public string Vcruexfeccreacion = "VCRUEXFECCREACION";

        //ATRIBUTOS EMPLEADOS PARA LAS CONSULTAS
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcen = "EQUINOMBCEN";
        public string Equinombuni = "EQUINOMBUNI";

        #endregion

        //METODO PARA LA TABLA VCR_MEDBORNECARGOINC
        public string SqlListParametro
        {
            get { return base.GetSqlXml("ListParametro"); }
        }

        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }

        public string SqlUpdateVersionSI
        {
            get { return base.GetSqlXml("UpdateVersionSI"); }
        }

        public string SqlUpdateVersionNO
        {
            get { return base.GetSqlXml("UpdateVersionNO"); }
        }
    }
}
