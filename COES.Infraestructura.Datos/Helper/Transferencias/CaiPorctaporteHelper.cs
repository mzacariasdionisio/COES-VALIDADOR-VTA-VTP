using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_PORCTAPORTE
    /// </summary>
    public class CaiPorctaporteHelper : HelperBase
    {
        public CaiPorctaporteHelper(): base(Consultas.CaiPorctaporteSql)
        {
        }

        public CaiPorctaporteDTO Create(IDataReader dr)
        {
            CaiPorctaporteDTO entity = new CaiPorctaporteDTO();

            int iCaipacodi = dr.GetOrdinal(this.Caipacodi);
            if (!dr.IsDBNull(iCaipacodi)) entity.Caipacodi = Convert.ToInt32(dr.GetValue(iCaipacodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCaipaimpaporte = dr.GetOrdinal(this.Caipaimpaporte);
            if (!dr.IsDBNull(iCaipaimpaporte)) entity.Caipaimpaporte = dr.GetDecimal(iCaipaimpaporte);

            int iCaipapctaporte = dr.GetOrdinal(this.Caipapctaporte);
            if (!dr.IsDBNull(iCaipapctaporte)) entity.Caipapctaporte = dr.GetDecimal(iCaipapctaporte);

            int iCaipausucreacion = dr.GetOrdinal(this.Caipausucreacion);
            if (!dr.IsDBNull(iCaipausucreacion)) entity.Caipausucreacion = dr.GetString(iCaipausucreacion);

            int iCaipafeccreacion = dr.GetOrdinal(this.Caipafeccreacion);
            if (!dr.IsDBNull(iCaipafeccreacion)) entity.Caipafeccreacion = dr.GetDateTime(iCaipafeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caipacodi = "CAIPACODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Emprcodi = "EMPRCODI";
        public string Caipaimpaporte = "CAIPAIMPAPORTE";
        public string Caipapctaporte = "CAIPAPCTAPORTE";
        public string Caipausucreacion = "CAIPAUSUCREACION";
        public string Caipafeccreacion = "CAIPAFECCREACION";

        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        #endregion

        public string SqlByEmpresaImporte
        {
            get { return GetSqlXml("ByEmpresaImporte"); }
        }
        
    }
}
