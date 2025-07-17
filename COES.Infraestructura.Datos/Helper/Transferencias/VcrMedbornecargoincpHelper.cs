using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_MEDBORNECARGOINCP
    /// </summary>
    public class VcrMedbornecargoincpHelper : HelperBase
    {
        public VcrMedbornecargoincpHelper(): base(Consultas.VcrMedbornecargoincpSql)
        {
        }

        public VcrMedbornecargoincpDTO Create(IDataReader dr)
        {
            VcrMedbornecargoincpDTO entity = new VcrMedbornecargoincpDTO();

            int iVcmbcicodi = dr.GetOrdinal(this.Vcmbcicodi);
            if (!dr.IsDBNull(iVcmbcicodi)) entity.Vcmbcicodi = Convert.ToInt32(dr.GetValue(iVcmbcicodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicen = dr.GetOrdinal(this.Equicodicen);
            if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

            int iEquicodiuni = dr.GetOrdinal(this.Equicodiuni);
            if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

            int iVcmbciconsiderar = dr.GetOrdinal(this.Vcmbciconsiderar);
            if (!dr.IsDBNull(iVcmbciconsiderar)) entity.Vcmbciconsiderar = dr.GetString(iVcmbciconsiderar);

            int iVcmbciusucreacion = dr.GetOrdinal(this.Vcmbciusucreacion);
            if (!dr.IsDBNull(iVcmbciusucreacion)) entity.Vcmbciusucreacion = dr.GetString(iVcmbciusucreacion);

            int iVcmbcifeccreacion = dr.GetOrdinal(this.Vcmbcifeccreacion);
            if (!dr.IsDBNull(iVcmbcifeccreacion)) entity.Vcmbcifeccreacion = dr.GetDateTime(iVcmbcifeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcmbcicodi = "VCMBCICODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicen = "EQUICODICEN";
        public string Equicodiuni = "EQUICODIUNI";
        public string Vcmbciconsiderar = "VCMBCICONSIDERAR";
        public string Vcmbciusucreacion = "VCMBCIUSUCREACION";
        public string Vcmbcifeccreacion = "VCMBCIFECCREACION";

        //ATRIBUTOS EMPLEADOS PARA LAS CONSULTAS
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcen = "EQUINOMBCEN";
        public string Equinombuni = "EQUINOMBUNI";

        #endregion

        //METODO PARA LA TABLA VCR_MEDBORNECARGOINC
        public string SqlUpdateVersionNO
        {
            get { return base.GetSqlXml("UpdateVersionNO"); }
        }

        public string SqlUpdateVersionSI
        {
            get { return base.GetSqlXml("UpdateVersionSI"); }
        }
    }
}
