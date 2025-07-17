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
    public class VcrVerincumplimHelper : HelperBase
    {
        public VcrVerincumplimHelper(): base(Consultas.VcrVerincumplimSql)
        {
        }

        public VcrVerincumplimDTO Create(IDataReader dr)
        {
            VcrVerincumplimDTO entity = new VcrVerincumplimDTO();

            int iVcrinccodi = dr.GetOrdinal(this.Vcrinccodi);
            if (!dr.IsDBNull(iVcrinccodi)) entity.Vcrinccodi = Convert.ToInt32(dr.GetValue(iVcrinccodi));

            int iEquicodicen = dr.GetOrdinal(this.Equicodicen);
            if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

            int iEquicodiuni = dr.GetOrdinal(this.Equicodiuni);
            if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

            int iVcrvincodrpf = dr.GetOrdinal(this.Vcrvincodrpf);
            if (!dr.IsDBNull(iVcrvincodrpf)) entity.Vcrvincodrpf = Convert.ToInt32(dr.GetValue(iVcrvincodrpf));

            int iVcrvinfecha = dr.GetOrdinal(this.Vcrvinfecha);
            if (!dr.IsDBNull(iVcrvinfecha)) entity.Vcrvinfecha = dr.GetDateTime(iVcrvinfecha);

            int iVcrvincumpli = dr.GetOrdinal(this.Vcrvincumpli);
            if (!dr.IsDBNull(iVcrvincumpli)) entity.Vcrvincumpli = dr.GetDecimal(iVcrvincumpli);

            int iVcrvinobserv = dr.GetOrdinal(this.Vcrvinobserv);
            if (!dr.IsDBNull(iVcrvinobserv)) entity.Vcrvinobserv = dr.GetString(iVcrvinobserv);

            int iVcrvinusucreacion = dr.GetOrdinal(this.Vcrvinusucreacion);
            if (!dr.IsDBNull(iVcrvinusucreacion)) entity.Vcrvinusucreacion = dr.GetString(iVcrvinusucreacion);

            int iVcrvinfeccreacion = dr.GetOrdinal(this.Vcrvinfeccreacion);
            if (!dr.IsDBNull(iVcrvinfeccreacion)) entity.Vcrvinfeccreacion = dr.GetDateTime(iVcrvinfeccreacion);

            int iVcrvincodi = dr.GetOrdinal(this.Vcrvincodi);
            if (!dr.IsDBNull(iVcrvincodi)) entity.Vcrvincodi = Convert.ToInt32(dr.GetValue(iVcrvincodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrinccodi = "VCRINCCODI";
        public string Equicodicen = "EQUICODICEN";
        public string Equicodiuni = "EQUICODIUNI";
        public string Vcrvincodrpf = "VCRVINCODRPF";
        public string Vcrvinfecha = "VCRVINFECHA";
        public string Vcrvincumpli = "VCRVINCUMPLI";
        public string Vcrvinobserv = "VCRVINOBSERV";
        public string Vcrvinusucreacion = "VCRVINUSUCREACION";
        public string Vcrvinfeccreacion = "VCRVINFECCREACION";
        public string Vcrvincodi = "VCRVINCODI";

        //atributos adicionales
        public string Centralnombre = "CENTRALNOMBRE";
        public string Uninombre = "UNINOMBRE";
        #endregion

        public string SqlGetByIdPorUnidad
        {
            get { return base.GetSqlXml("GetByIdPorUnidad"); }
        }
    }
}
