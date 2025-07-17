using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_CARGOINCUMPL
    /// </summary>
    public class VcrCargoincumplHelper : HelperBase
    {
        public VcrCargoincumplHelper(): base(Consultas.VcrCargoincumplSql)
        {
        }

        public VcrCargoincumplDTO Create(IDataReader dr)
        {
            VcrCargoincumplDTO entity = new VcrCargoincumplDTO();

            int iVcrcicodi = dr.GetOrdinal(this.Vcrcicodi);
            if (!dr.IsDBNull(iVcrcicodi)) entity.Vcrcicodi = Convert.ToInt32(dr.GetValue(iVcrcicodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iVcrcicargoincumplmes = dr.GetOrdinal(this.Vcrcicargoincumplmes);
            if (!dr.IsDBNull(iVcrcicargoincumplmes)) entity.Vcrcicargoincumplmes = dr.GetDecimal(iVcrcicargoincumplmes);

            int iVcrcisaldoanterior = dr.GetOrdinal(this.Vcrcisaldoanterior);
            if (!dr.IsDBNull(iVcrcisaldoanterior)) entity.Vcrcisaldoanterior = dr.GetDecimal(iVcrcisaldoanterior);

            int iVcrcicargoincumpl = dr.GetOrdinal(this.Vcrcicargoincumpl);
            if (!dr.IsDBNull(iVcrcicargoincumpl)) entity.Vcrcicargoincumpl = dr.GetDecimal(iVcrcicargoincumpl);

            int iVcrcicarginctransf = dr.GetOrdinal(this.Vcrcicarginctransf);
            if (!dr.IsDBNull(iVcrcicarginctransf)) entity.Vcrcicarginctransf = dr.GetDecimal(iVcrcicarginctransf);

            int iVcrcisaldomes = dr.GetOrdinal(this.Vcrcisaldomes);
            if (!dr.IsDBNull(iVcrcisaldomes)) entity.Vcrcisaldomes = dr.GetDecimal(iVcrcisaldomes);

            int iPericodidest = dr.GetOrdinal(this.Pericodidest);
            if (!dr.IsDBNull(iPericodidest)) entity.Pericodidest = Convert.ToInt32(dr.GetValue(iPericodidest));

            int iVcrciusucreacion = dr.GetOrdinal(this.Vcrciusucreacion);
            if (!dr.IsDBNull(iVcrciusucreacion)) entity.Vcrciusucreacion = dr.GetString(iVcrciusucreacion);

            int iVcrcifeccreacion = dr.GetOrdinal(this.Vcrcifeccreacion);
            if (!dr.IsDBNull(iVcrcifeccreacion)) entity.Vcrcifeccreacion = dr.GetDateTime(iVcrcifeccreacion);

            int iVcrcisaldomesanterior = dr.GetOrdinal(this.Vcrcisaldomesanterior);
            if (!dr.IsDBNull(iVcrcisaldomesanterior)) entity.VcrcisaldomesAnterior = dr.GetDecimal(iVcrcisaldomesanterior);

            int iVcrciincumplsrvrsf = dr.GetOrdinal(this.Vcrciincumplsrvrsf);
            if (!dr.IsDBNull(iVcrciincumplsrvrsf)) entity.Vcrciincumplsrvrsf = dr.GetDecimal(iVcrciincumplsrvrsf);

            int iVcrciincent = dr.GetOrdinal(this.Vcrciincent);
            if (!dr.IsDBNull(iVcrciincent)) entity.Vcrciincent = dr.GetDecimal(iVcrciincent);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrcicodi = "VCRCICODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Equicodi = "EQUICODI";
        public string Vcrcicargoincumplmes = "VCRCICARGOINCUMPLMES";
        public string Vcrcisaldoanterior = "VCRCISALDOANTERIOR";
        public string Vcrcicargoincumpl = "VCRCICARGOINCUMPL";
        public string Vcrcicarginctransf = "VCRCICARGINCTRANSF";
        public string Vcrcisaldomes = "VCRCISALDOMES";
        public string Vcrcisaldomesanterior = "VCRCISALDOMESANTERIOR";
        public string Pericodidest = "PERICODIDEST";
        public string Vcrciusucreacion = "VCRCIUSUCREACION";
        public string Vcrcifeccreacion = "VCRCIFECCREACION";

        //ASSETEC 202012
        public string Vcrciincumplsrvrsf = "VCRCIINCUMPLSRVRSF";
        public string Vcrciincent = "VCRCIINCENT";
        #endregion

        //METODO PARA LA TABLA VCR_CARGOINCUMPL
        public string SqlListCargoIncumplGrupoCalculado
        {
            get { return base.GetSqlXml("ListCargoIncumplGrupoCalculado"); }
        }

        public string SqlTotalMesServicioRSFConsiderados
        {
            get { return base.GetSqlXml("TotalMesServicioRSFConsiderados"); }
        }
        
    }
}
