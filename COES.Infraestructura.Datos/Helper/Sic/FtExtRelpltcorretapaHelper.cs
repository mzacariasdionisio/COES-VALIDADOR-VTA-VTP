using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_RELPLTCORRETAPA
    /// </summary>
    public class FtExtRelpltcorretapaHelper : HelperBase
    {
        public FtExtRelpltcorretapaHelper() : base(Consultas.FtExtRelpltcorretapaSql)
        {
        }

        public FtExtRelpltcorretapaDTO Create(IDataReader dr)
        {
            FtExtRelpltcorretapaDTO entity = new FtExtRelpltcorretapaDTO();

            int iPlantcodi = dr.GetOrdinal(this.Plantcodi);
            if (!dr.IsDBNull(iPlantcodi)) entity.Plantcodi = Convert.ToInt32(dr.GetValue(iPlantcodi));

            int iFtetcodi = dr.GetOrdinal(this.Ftetcodi);
            if (!dr.IsDBNull(iFtetcodi)) entity.Ftetcodi = Convert.ToInt32(dr.GetValue(iFtetcodi));

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iTpcorrcodi = dr.GetOrdinal(this.Tpcorrcodi);
            if (!dr.IsDBNull(iTpcorrcodi)) entity.Tpcorrcodi = Convert.ToInt32(dr.GetValue(iTpcorrcodi));

            int iFtrpcetipoespecial = dr.GetOrdinal(this.Ftrpcetipoespecial);
            if (!dr.IsDBNull(iFtrpcetipoespecial)) entity.Ftrpcetipoespecial = Convert.ToInt32(dr.GetValue(iFtrpcetipoespecial)); 

                int iFtrpcetipoampliacion = dr.GetOrdinal(this.Ftrpcetipoampliacion);
            if (!dr.IsDBNull(iFtrpcetipoampliacion)) entity.Ftrpcetipoampliacion = Convert.ToInt32(dr.GetValue(iFtrpcetipoampliacion));
            int iFcoretcodi = dr.GetOrdinal(this.Fcoretcodi);
            if (!dr.IsDBNull(iFcoretcodi)) entity.Fcoretcodi = Convert.ToInt32(dr.GetValue(iFcoretcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Plantcodi = "PLANTCODI";
        public string Ftetcodi = "FTETCODI";
        public string Fcoretcodi = "FCORETCODI";
        public string Estenvcodi = "ESTENVCODI";
        public string Tpcorrcodi = "TPCORRCODI";
        public string Ftrpcetipoespecial = "FTRPCETIPOESPECIAL";
        public string Ftrpcetipoampliacion = "FTRPCETIPOAMPLIACION";

        public string Ftetnombre = "FTETNOMBRE";
        
        public string Tpcorrdescrip = "TPCORRDESCRIP";

        #endregion

        public string SqlGetRelacionSimple
        {
            get { return GetSqlXml("GetRelacionSimple"); }
        }
        public string SqlGetRelacionEspecial
        {
            get { return GetSqlXml("GetRelacionEspecial"); }
        }
        public string SqlGetRelacionAmpliacion
        {
            get { return GetSqlXml("GetRelacionAmpliacion"); }
        }
        
        public string SqlGetRelacionEspecialYAmpliacion
        {
            get { return GetSqlXml("GetRelacionEspecialYAmpliacion"); }
        }
        

    }
}
