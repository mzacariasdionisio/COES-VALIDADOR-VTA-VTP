using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_DOCUMENTOS
    /// </summary>
    public class CpaDocumentosDetalleHelper : HelperBase
    {
        public CpaDocumentosDetalleHelper() : base(Consultas.CpaDocumentosDetalleSql)
        {
        }

        public CpaDocumentosDetalleDTO Create(IDataReader dr)
        {
            CpaDocumentosDetalleDTO entity = new CpaDocumentosDetalleDTO();

            int iCpaddtcodi = dr.GetOrdinal(Cpaddtcodi);
            if (!dr.IsDBNull(iCpaddtcodi)) entity.Cpaddtcodi = dr.GetInt32(iCpaddtcodi);

            int iCpadoccodi = dr.GetOrdinal(Cpadoccodi);
            if (!dr.IsDBNull(iCpadoccodi)) entity.Cpadoccodi = dr.GetInt32(iCpadoccodi);

            int iCpaddtruta = dr.GetOrdinal(Cpaddtruta);
            if (!dr.IsDBNull(iCpaddtruta)) entity.Cpaddtruta = dr.GetString(iCpaddtruta);

            int iCpaddtnombre = dr.GetOrdinal(Cpaddtnombre);
            if (!dr.IsDBNull(iCpaddtnombre)) entity.Cpaddtnombre = dr.GetString(iCpaddtnombre);

            int iCpaddttamano = dr.GetOrdinal(Cpaddttamano);
            if (!dr.IsDBNull(iCpaddttamano)) entity.Cpaddttamano = dr.GetString(iCpaddttamano);

            int iCpaddtusucreacion = dr.GetOrdinal(Cpaddtusucreacion);
            if (!dr.IsDBNull(iCpaddtusucreacion)) entity.Cpaddtusucreacion = dr.GetString(iCpaddtusucreacion);

            int iCpaddtfeccreacion = dr.GetOrdinal(Cpaddtfeccreacion);
            if (!dr.IsDBNull(iCpaddtfeccreacion)) entity.Cpaddtfeccreacion = dr.GetDateTime(iCpaddtfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpaddtcodi = "CPADDTCODI";
        public string Cpadoccodi = "CPADOCCODI";
        public string Cpaddtruta = "CPADDTRUTA";
        public string Cpaddtnombre = "CPADDTNOMBRE";
        public string Cpaddttamano = "CPADDTTAMANO";
        public string Cpaddtusucreacion = "CPADDTUSUCREACION";
        public string Cpaddtfeccreacion = "CPADDTFECCREACION";
        #endregion

        public string SqlGetDetalleByDocumento
        {
            get { return base.GetSqlXml("GetDetalleByDocumento"); }
        }
    }
}
