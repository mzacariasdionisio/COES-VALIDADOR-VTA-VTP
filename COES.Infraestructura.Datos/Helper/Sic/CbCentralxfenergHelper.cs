using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_CENTRALXFENERG
    /// </summary>
    public class CbCentralxfenergHelper : HelperBase
    {
        public CbCentralxfenergHelper() : base(Consultas.CbCentralxfenergSql)
        {
        }

        public CbCentralxfenergDTO Create(IDataReader dr)
        {
            CbCentralxfenergDTO entity = new CbCentralxfenergDTO();

            int iCbcxfenuevo = dr.GetOrdinal(this.Cbcxfenuevo);
            if (!dr.IsDBNull(iCbcxfenuevo)) entity.Cbcxfenuevo = Convert.ToInt32(dr.GetValue(iCbcxfenuevo));

            int iCbcxfeexistente = dr.GetOrdinal(this.Cbcxfeexistente);
            if (!dr.IsDBNull(iCbcxfeexistente)) entity.Cbcxfeexistente = Convert.ToInt32(dr.GetValue(iCbcxfeexistente));

            int iEstcomcodi = dr.GetOrdinal(this.Estcomcodi);
            if (!dr.IsDBNull(iEstcomcodi)) entity.Estcomcodi = Convert.ToInt32(dr.GetValue(iEstcomcodi));

            int iCbcxfecodi = dr.GetOrdinal(this.Cbcxfecodi);
            if (!dr.IsDBNull(iCbcxfecodi)) entity.Cbcxfecodi = Convert.ToInt32(dr.GetValue(iCbcxfecodi));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iCbcxfeactivo = dr.GetOrdinal(this.Cbcxfeactivo);
            if (!dr.IsDBNull(iCbcxfeactivo)) entity.Cbcxfeactivo = Convert.ToInt32(dr.GetValue(iCbcxfeactivo));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iCbcxfevisibleapp = dr.GetOrdinal(this.Cbcxfevisibleapp);
            if (!dr.IsDBNull(iCbcxfevisibleapp)) entity.Cbcxfevisibleapp = Convert.ToInt32(dr.GetValue(iCbcxfevisibleapp));

            int iCbcxfeorden = dr.GetOrdinal(this.Cbcxfeorden);
            if (!dr.IsDBNull(iCbcxfeorden)) entity.Cbcxfeorden = Convert.ToInt32(dr.GetValue(iCbcxfeorden));

            return entity;
        }

        #region Mapeo de Campos

        public string Cbcxfenuevo = "CBCXFENUEVO";
        public string Cbcxfeexistente = "CBCXFEEXISTENTE";
        public string Estcomcodi = "ESTCOMCODI";
        public string Cbcxfecodi = "CBCXFECODI";
        public string Fenergcodi = "FENERGCODI";
        public string Equicodi = "EQUICODI";
        public string Cbcxfeactivo = "CBCXFEACTIVO";
        public string Grupocodi = "GRUPOCODI";
        public string Cbcxfevisibleapp = "CBCXFEVISIBLEAPP";
        public string Cbcxfeorden = "CBCXFEORDEN";

        public string Emprcodi = "EMPRCODI";
        public string Equinomb = "EQUINOMB";
        public string Fenergnomb = "FENERGNOMB";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlGetByFenergYGrupocodi
        {
            get { return GetSqlXml("GetByFenergYGrupocodi"); }
        }

    }
}
