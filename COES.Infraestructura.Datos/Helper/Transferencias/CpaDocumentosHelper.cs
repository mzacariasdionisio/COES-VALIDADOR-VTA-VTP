using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_DOCUMENTOS
    /// </summary>
    public class CpaDocumentosHelper : HelperBase
    {
        public CpaDocumentosHelper() : base(Consultas.CpaDocumentosSql)
        {
        }

        public CpaDocumentosDTO Create(IDataReader dr)
        {
            CpaDocumentosDTO entity = new CpaDocumentosDTO();

            int iCpadoccodi = dr.GetOrdinal(Cpadoccodi);
            if (!dr.IsDBNull(iCpadoccodi)) entity.Cpadoccodi = dr.GetInt32(iCpadoccodi);

            int iEmprcodi= dr.GetOrdinal(Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpadoccodenvio = dr.GetOrdinal(Cpadoccodenvio);
            if (!dr.IsDBNull(iCpadoccodenvio)) entity.Cpadoccodenvio = dr.GetInt32(iCpadoccodenvio);

            int iCpadocusucreacion = dr.GetOrdinal(Cpadocusucreacion);
            if (!dr.IsDBNull(iCpadocusucreacion)) entity.Cpadocusucreacion = dr.GetString(iCpadocusucreacion);

            int iCpadocfeccreacion = dr.GetOrdinal(Cpadocfeccreacion);
            if (!dr.IsDBNull(iCpadocfeccreacion)) entity.Cpadocfeccreacion = dr.GetDateTime(iCpadocfeccreacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Cpadoccodi = "CPADOCCODI";
        public string Emprcodi = "EMPRCODI";
        public string Cparcodi = "CPARCODI";
        public string Cpadoccodenvio = "CPADOCCODENVIO";
        public string Cpadocusucreacion = "CPADOCUSUCREACION";
        public string Cpadocfeccreacion = "CPADOCFECCREACION";
        //Adicionales
        public string Cpaapanio = "CPAAPANIO";
        public string Cpaapajuste = "CPAAPAJUSTE";
        public string Cparrevision = "CPARREVISION";
        public string Emprnomb = "EMPRNOMB";
        #endregion

        public string SqlGetDocumentosByFilters
        {
            get { return base.GetSqlXml("GetDocumentosByFilters"); }
        }
    }
}
