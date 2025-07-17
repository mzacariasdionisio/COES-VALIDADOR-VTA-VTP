using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_CENTRAL_CODRETIRO
    /// </summary>
    public class RerCentralCodRetiroHelper : HelperBase
    {
        public RerCentralCodRetiroHelper() : base(Consultas.RerCentralCodRetiroSql)
        {
        }

        public RerCentralCodRetiroDTO Create(IDataReader dr)
        {
            RerCentralCodRetiroDTO entity = new RerCentralCodRetiroDTO();

            int iRerCcrCodi = dr.GetOrdinal(this.Rerccrcodi);
            if (!dr.IsDBNull(iRerCcrCodi)) entity.Rerccrcodi = Convert.ToInt32(dr.GetValue(iRerCcrCodi));

            int iRerPprCodi = dr.GetOrdinal(this.Rerpprcodi);
            if (!dr.IsDBNull(iRerPprCodi)) entity.Rerpprcodi = Convert.ToInt32(dr.GetValue(iRerPprCodi));

            int iRercencodi = dr.GetOrdinal(this.Rercencodi);
            if (!dr.IsDBNull(iRercencodi)) entity.Rercencodi = Convert.ToInt32(dr.GetValue(iRercencodi));

            int iCoresocodi = dr.GetOrdinal(this.Coresocodi);
            if (!dr.IsDBNull(iCoresocodi)) entity.Coresocodi = Convert.ToInt32(dr.GetValue(iCoresocodi));

            int iRerccrusucreacion = dr.GetOrdinal(this.Rerccrusucreacion);
            if (!dr.IsDBNull(iRerccrusucreacion)) entity.Rerccrusucreacion = dr.GetString(iRerccrusucreacion);

            int iRerccrfeccreacion = dr.GetOrdinal(this.Rerccrfeccreacion);
            if (!dr.IsDBNull(iRerccrfeccreacion)) entity.Rerccrfeccreacion = dr.GetDateTime(iRerccrfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rerccrcodi = "RERCCRCODI";
        public string Rerpprcodi = "RERPPRCODI";
        public string Rercencodi = "RERCENCODI";
        public string Coresocodi = "CORESOCODI";
        public string Rerccrusucreacion = "RERCCRUSUCREACION";
        public string Rerccrfeccreacion = "RERCCRFECCREACION";

        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Cantidad = "CANTIDAD";
        public string Equinomb = "EQUINOMB";
        #endregion

        public string SqlListCantidadByRerpprcodi
        {
            get { return base.GetSqlXml("ListCantidadByRerpprcodi"); }
        }
        public string SqlListNombreCodRetiroBarrTransferenciaByRerpprcodiRercencodi
        {
            get { return base.GetSqlXml("ListNombreCodRetiroBarrTransferenciaByRerpprcodiRercencodi"); }
        }
        public string SqlDeleteAllByRerpprcodiRercencodi
        {
            get { return base.GetSqlXml("DeleteAllByRerpprcodiRercencodi"); }
        }
        //CU21
        public string SqlListaCodigoRetiroByEquipo
        {
            get { return base.GetSqlXml("ListaCodigoRetiroByEquipo"); }
        }
    }
}