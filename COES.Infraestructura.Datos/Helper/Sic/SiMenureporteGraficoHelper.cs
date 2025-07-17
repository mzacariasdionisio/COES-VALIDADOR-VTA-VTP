using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MENUREPORTE_GRAFICO
    /// </summary>
    public class SiMenureporteGraficoHelper : HelperBase
    {
        public SiMenureporteGraficoHelper(): base(Consultas.SiMenureporteGraficoSql)
        {
        }

        public SiMenureporteGraficoDTO Create(IDataReader dr)
        {
            SiMenureporteGraficoDTO entity = new SiMenureporteGraficoDTO();

            int iMrepcodi = dr.GetOrdinal(this.Mrepcodi);
            if (!dr.IsDBNull(iMrepcodi)) entity.Mrepcodi = Convert.ToInt32(dr.GetValue(iMrepcodi));

            int iMrgrcodi = dr.GetOrdinal(this.Mrgrcodi);
            if (!dr.IsDBNull(iMrgrcodi)) entity.Mrgrcodi = Convert.ToInt32(dr.GetValue(iMrgrcodi));

            int iMrgrestado = dr.GetOrdinal(this.Mrgrestado);
            if (!dr.IsDBNull(iMrgrestado)) entity.Mrgrestado = Convert.ToInt32(dr.GetValue(iMrgrestado));

            int iReporcodi = dr.GetOrdinal(this.Reporcodi);
            if (!dr.IsDBNull(iReporcodi)) entity.Reporcodi = Convert.ToInt32(dr.GetValue(iReporcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Mrepcodi = "MREPCODI";
        public string Mrgrcodi = "MRGRCODI";
        public string Mrgrestado = "MRGRESTADO";
        public string Reporcodi = "REPORCODI";

        #endregion
    }
}
