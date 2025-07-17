using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_FAC_PER_MED
    /// </summary>
    public class RerFacPerMedHelper : HelperBase
    {
        public RerFacPerMedHelper() : base(Consultas.RerFacPerMedSql)
        {
        }

        public RerFacPerMedDTO Create(IDataReader dr)
        {
            RerFacPerMedDTO entity = new RerFacPerMedDTO();

            int iRerfpmcodi = dr.GetOrdinal(this.Rerfpmcodi);
            if (!dr.IsDBNull(iRerfpmcodi)) entity.Rerfpmcodi = Convert.ToInt32(dr.GetValue(iRerfpmcodi));

            int iRerfpmnombrearchivo = dr.GetOrdinal(this.Rerfpmnombrearchivo);
            if (!dr.IsDBNull(iRerfpmnombrearchivo)) entity.Rerfpmnombrearchivo = dr.GetString(iRerfpmnombrearchivo);

            int iRerfpmdesde = dr.GetOrdinal(this.Rerfpmdesde);
            if (!dr.IsDBNull(iRerfpmdesde)) entity.Rerfpmdesde = dr.GetDateTime(iRerfpmdesde);

            int iRerfpmhasta = dr.GetOrdinal(this.Rerfpmhasta);
            if (!dr.IsDBNull(iRerfpmhasta)) entity.Rerfpmhasta = dr.GetDateTime(iRerfpmhasta);

            int iRerfpmusucreacion = dr.GetOrdinal(this.Rerfpmusucreacion);
            if (!dr.IsDBNull(iRerfpmusucreacion)) entity.Rerfpmusucreacion = dr.GetString(iRerfpmusucreacion);

            int iRerfpmfeccreacion = dr.GetOrdinal(this.Rerfpmfeccreacion);
            if (!dr.IsDBNull(iRerfpmfeccreacion)) entity.Rerfpmfeccreacion = dr.GetDateTime(iRerfpmfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Rerfpmcodi = "RERFPMCODI";
        public string Rerfpmnombrearchivo = "RERFPMNOMBREARCHIVO";
        public string Rerfpmdesde = "RERFPMDESDE";
        public string Rerfpmhasta = "RERFPMHASTA";
        public string Rerfpmusucreacion = "RERFPMUSUCREACION";
        public string Rerfpmfeccreacion = "RERFPMFECCREACION";

        #endregion
    }
}