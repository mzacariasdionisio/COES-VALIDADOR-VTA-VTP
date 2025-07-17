using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_COSTOPORTDETALLE
    /// </summary>
    public class VcrCostoportdetalleHelper : HelperBase
    {
        public VcrCostoportdetalleHelper(): base(Consultas.VcrCostoportdetalleSql)
        {
        }

        public VcrCostoportdetalleDTO Create(IDataReader dr)
        {
            VcrCostoportdetalleDTO entity = new VcrCostoportdetalleDTO();

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrcodfecha = dr.GetOrdinal(this.Vcrcodfecha);
            if (!dr.IsDBNull(iVcrcodfecha)) entity.Vcrcodfecha = dr.GetDateTime(iVcrcodfecha);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iVcrcodinterv = dr.GetOrdinal(this.Vcrcodinterv);
            if (!dr.IsDBNull(iVcrcodinterv)) entity.Vcrcodinterv = Convert.ToInt32(dr.GetValue(iVcrcodinterv));

            int iVcrcodpdo = dr.GetOrdinal(this.Vcrcodpdo);
            if (!dr.IsDBNull(iVcrcodpdo)) entity.Vcrcodpdo = dr.GetDecimal(iVcrcodpdo);

            int iVcrcodcmgcp = dr.GetOrdinal(this.Vcrcodcmgcp);
            if (!dr.IsDBNull(iVcrcodcmgcp)) entity.Vcrcodcmgcp = dr.GetDecimal(iVcrcodcmgcp);

            int iVcrcodcv = dr.GetOrdinal(this.Vcrcodcv);
            if (!dr.IsDBNull(iVcrcodcv)) entity.Vcrcodcv = dr.GetDecimal(iVcrcodcv);

            int iVcrcodcostoportun = dr.GetOrdinal(this.Vcrcodcostoportun);
            if (!dr.IsDBNull(iVcrcodcostoportun)) entity.Vcrcodcostoportun = dr.GetDecimal(iVcrcodcostoportun);

            int iVcrcodusucreacion = dr.GetOrdinal(this.Vcrcodusucreacion);
            if (!dr.IsDBNull(iVcrcodusucreacion)) entity.Vcrcodusucreacion = dr.GetString(iVcrcodusucreacion);

            int iVcrcodfeccreacion = dr.GetOrdinal(this.Vcrcodfeccreacion);
            if (!dr.IsDBNull(iVcrcodfeccreacion)) entity.Vcrcodfeccreacion = dr.GetDateTime(iVcrcodfeccreacion);

            int iVcrcodcodi = dr.GetOrdinal(this.Vcrcodcodi);
            if (!dr.IsDBNull(iVcrcodcodi)) entity.Vcrcodcodi = Convert.ToInt32(dr.GetValue(iVcrcodcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrcodcodi = "VCRCODCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrcodfecha = "VCRCODFECHA";
        public string Equicodi = "EQUICODI";
        public string Vcrcodinterv = "VCRCODINTERV";
        public string Vcrcodpdo = "VCRCODPDO";
        public string Vcrcodcmgcp = "VCRCODCMGCP";
        public string Vcrcodcv = "VCRCODCV";
        public string Vcrcodcostoportun = "VCRCODCOSTOPORTUN";
        public string Vcrcodusucreacion = "VCRCODUSUCREACION";
        public string Vcrcodfeccreacion = "VCRCODFECCREACION";
        
        #endregion

        //Metodos de consulta
        public string SqlListPorMesURS
        {
            get { return base.GetSqlXml("ListPorMesURS"); }
        }
    }
}
