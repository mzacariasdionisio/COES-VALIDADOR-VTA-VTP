using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_MEDBORNE
    /// </summary>
    public class VcrMedborneHelper : HelperBase
    {
        public VcrMedborneHelper(): base(Consultas.VcrMedborneSql)
        {
        }

        public VcrMedborneDTO Create(IDataReader dr)
        {
            VcrMedborneDTO entity = new VcrMedborneDTO();

            int iVcrmebcodi = dr.GetOrdinal(this.Vcrmebcodi);
            if (!dr.IsDBNull(iVcrmebcodi)) entity.Vcrmebcodi = Convert.ToInt32(dr.GetValue(iVcrmebcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicen = dr.GetOrdinal(this.Equicodicen);
            if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

            int iEquicodiuni = dr.GetOrdinal(this.Equicodiuni);
            if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

            int iVcrmebfecha = dr.GetOrdinal(this.Vcrmebfecha);
            if (!dr.IsDBNull(iVcrmebfecha)) entity.Vcrmebfecha = dr.GetDateTime(iVcrmebfecha);

            int iVcrmebptomed = dr.GetOrdinal(this.Vcrmebptomed);
            if (!dr.IsDBNull(iVcrmebptomed)) entity.Vcrmebptomed = dr.GetString(iVcrmebptomed);

            int iVcrmebpotenciamed = dr.GetOrdinal(this.Vcrmebpotenciamed);
            if (!dr.IsDBNull(iVcrmebpotenciamed)) entity.Vcrmebpotenciamed = dr.GetDecimal(iVcrmebpotenciamed);

            int iVcrmebusucreacion = dr.GetOrdinal(this.Vcrmebusucreacion);
            if (!dr.IsDBNull(iVcrmebusucreacion)) entity.Vcrmebusucreacion = dr.GetString(iVcrmebusucreacion);

            int iVcrmebfeccreacion = dr.GetOrdinal(this.Vcrmebfeccreacion);
            if (!dr.IsDBNull(iVcrmebfeccreacion)) entity.Vcrmebfeccreacion = dr.GetDateTime(iVcrmebfeccreacion);

            //ASSETEC 202012
            int iVcrmebpotenciamedgrp = dr.GetOrdinal(this.Vcrmebpotenciamedgrp);
            if (!dr.IsDBNull(iVcrmebpotenciamedgrp)) entity.Vcrmebpotenciamedgrp = dr.GetDecimal(iVcrmebpotenciamedgrp);

            int iVcrmebpresencia = dr.GetOrdinal(this.Vcrmebpresencia);
            if (!dr.IsDBNull(iVcrmebpresencia)) entity.Vcrmebpresencia = dr.GetDecimal(iVcrmebpresencia);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrmebcodi = "VCRMEBCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicen = "EQUICODICEN";
        public string Equicodiuni = "EQUICODIUNI";
        public string Vcrmebfecha = "VCRMEBFECHA";
        public string Vcrmebptomed = "VCRMEBPTOMED";
        public string Vcrmebpotenciamed = "VCRMEBPOTENCIAMED";
        public string Vcrmebusucreacion = "VCRMEBUSUCREACION";
        public string Vcrmebfeccreacion = "VCRMEBFECCREACION";
        //ASSETEC 202012
        public string Vcrmebpotenciamedgrp = "VCRMEBPOTENCIAMEDGRP";
        public string Vcrmebpresencia = "VCRMEBPRESENCIA";

        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcen = "EQUINOMBCEN";
        public string Equinombuni = "EQUINOMBUNI";
        public string Vcmbciconsiderar = "VCMBCICONSIDERAR";

        #endregion
        public string TableName = "VCR_MEDBORNE";
        //METODO PARA LA TABLA VCR_MEDBORNE
        public string SqlListDistintos
        {
            get { return base.GetSqlXml("ListDistintos"); }
        }

        public string SqlListDiaSinUnidExonRSF
        {
            get { return base.GetSqlXml("ListDiaSinUnidExonRSF"); }
        }

        public string SqlListMes
        {
            get { return base.GetSqlXml("ListMes"); }
        }

        public string SqlListMesConsiderados
        {
            get { return base.GetSqlXml("ListMesConsiderados"); }
        }

        public string SqlListMesConsideradosTotales
        {
            get { return base.GetSqlXml("ListMesConsideradosTotales"); }
        }

        public string SqlTotalUnidNoExonRSF
        {
            get { return base.GetSqlXml("TotalUnidNoExonRSF"); }
        }
        
    }
}
