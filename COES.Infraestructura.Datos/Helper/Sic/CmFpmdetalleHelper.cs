using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_FPMDETALLE
    /// </summary>
    public class CmFpmdetalleHelper : HelperBase
    {
        public CmFpmdetalleHelper(): base(Consultas.CmFpmdetalleSql)
        {
        }

        public CmFpmdetalleDTO Create(IDataReader dr)
        {
            CmFpmdetalleDTO entity = new CmFpmdetalleDTO();

            int iCmfpmdcodi = dr.GetOrdinal(this.Cmfpmdcodi);
            if (!dr.IsDBNull(iCmfpmdcodi)) entity.Cmfpmdcodi = Convert.ToInt32(dr.GetValue(iCmfpmdcodi));

            int iCmfpmcodi = dr.GetOrdinal(this.Cmfpmcodi);
            if (!dr.IsDBNull(iCmfpmcodi)) entity.Cmfpmcodi = Convert.ToInt32(dr.GetValue(iCmfpmcodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCmfpmdbase = dr.GetOrdinal(this.Cmfpmdbase);
            if (!dr.IsDBNull(iCmfpmdbase)) entity.Cmfpmdbase = dr.GetDecimal(iCmfpmdbase);

            int iCmfpmdmedia = dr.GetOrdinal(this.Cmfpmdmedia);
            if (!dr.IsDBNull(iCmfpmdmedia)) entity.Cmfpmdmedia = dr.GetDecimal(iCmfpmdmedia);

            int iCmfpmdpunta = dr.GetOrdinal(this.Cmfpmdpunta);
            if (!dr.IsDBNull(iCmfpmdpunta)) entity.Cmfpmdpunta = dr.GetDecimal(iCmfpmdpunta);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmfpmdcodi = "CMFPMDCODI";
        public string Cmfpmcodi = "CMFPMCODI";
        public string Barrcodi = "BARRCODI";
        public string Cmfpmdbase = "CMFPMDBASE";
        public string Cmfpmdmedia = "CMFPMDMEDIA";
        public string Cmfpmdpunta = "CMFPMDPUNTA";
        public string Fechamax = "FECHAMAX";

        #endregion

        public string SqlObtenerPorFecha
        {
            get { return base.GetSqlXml("ObtenerPorFecha"); }
        }
    }
}
