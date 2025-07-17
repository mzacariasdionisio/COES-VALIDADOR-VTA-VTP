using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_VERINCUMPLIM
    /// </summary>
    public class VcrVerporctreservHelper : HelperBase
    {
        public VcrVerporctreservHelper(): base(Consultas.VcrVerporctreservSql)
        {
        }

        public VcrVerporctreservDTO Create(IDataReader dr)
        {
            VcrVerporctreservDTO entity = new VcrVerporctreservDTO();

            int iVcrvprcodi = dr.GetOrdinal(this.Vcrvprcodi);
            if (!dr.IsDBNull(iVcrvprcodi)) entity.Vcrvprcodi = Convert.ToInt32(dr.GetValue(iVcrvprcodi));

            int iEquicodicen = dr.GetOrdinal(this.Equicodicen);
            if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

            int iEquicodiuni = dr.GetOrdinal(this.Equicodiuni);
            if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

            int iVcrvprfecha = dr.GetOrdinal(this.Vcrvprfecha);
            if (!dr.IsDBNull(iVcrvprfecha)) entity.Vcrvprfecha = dr.GetDateTime(iVcrvprfecha);

            int iVcrvprrpns = dr.GetOrdinal(this.Vcrvprrpns);
            if (!dr.IsDBNull(iVcrvprrpns)) entity.Vcrvprrpns = dr.GetDecimal(iVcrvprrpns);

            int iVcrvprusucreacion = dr.GetOrdinal(this.Vcrvprusucreacion);
            if (!dr.IsDBNull(iVcrvprusucreacion)) entity.Vcrvprusucreacion = dr.GetString(iVcrvprusucreacion);

            int iVcrvprfeccreacion = dr.GetOrdinal(this.Vcrvprfeccreacion);
            if (!dr.IsDBNull(iVcrvprfeccreacion)) entity.Vcrvprfeccreacion = dr.GetDateTime(iVcrvprfeccreacion);

            int iVcrinccodi = dr.GetOrdinal(this.Vcrinccodi);
            if (!dr.IsDBNull(iVcrinccodi)) entity.Vcrinccodi = Convert.ToInt32(dr.GetValue(iVcrinccodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrvprcodi = "VCRVPRCODI";
        public string Equicodicen = "EQUICODICEN";
        public string Equicodiuni = "EQUICODIUNI";
        public string Vcrvprfecha = "VCRVPRFECHA";
        public string Vcrvprrpns = "VCRVPRRPNS";
        public string Vcrvprusucreacion = "VCRVPRUSUCREACION";
        public string Vcrvprfeccreacion = "VCRVPRFECCREACION";
        public string Vcrinccodi = "VCRINCCODI";

        //atributos adicionales
        public string Centralnombre = "CENTRALNOMBRE";
        public string Unidadnombre = "UNIDADNOMBRE";
        #endregion

        public string SqlGetByIdPorUnidad
        {
            get { return base.GetSqlXml("GetByIdPorUnidad"); }
        }
    }
}
