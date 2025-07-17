using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;


namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnAsociacionHelper : HelperBase
    {
        public PrnAsociacionHelper() : base(Consultas.PrnAsociacionSql)
        {
        }

        public PrnAsociacionDTO Create(IDataReader dr)
        {
            PrnAsociacionDTO entity = new PrnAsociacionDTO();

            int iAsociacodi = dr.GetOrdinal(this.Asociacodi);
            if (!dr.IsDBNull(iAsociacodi)) entity.Asociacodi = Convert.ToInt32(dr.GetValue(iAsociacodi));

            int iAsocianom = dr.GetOrdinal(this.Asocianom);
            if (!dr.IsDBNull(iAsocianom)) entity.Asocianom = dr.GetString(iAsocianom);

            int iAsociatipomedi = dr.GetOrdinal(this.Asociatipomedi);
            if (!dr.IsDBNull(iAsociatipomedi)) entity.Asociatipomedi = dr.GetString(iAsociatipomedi);

            int iAsodetcodi = dr.GetOrdinal(this.Asodetcodi);
            if (!dr.IsDBNull(iAsodetcodi)) entity.Asodetcodi = Convert.ToInt32(dr.GetValue(iAsodetcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iAsodettipomedi = dr.GetOrdinal(this.Asodettipomedi);
            if (!dr.IsDBNull(iAsodettipomedi)) entity.Asodettipomedi = dr.GetString(iAsodettipomedi);

            return entity;
        }

        public string Asociacodi = "ASOCIACODI";
        public string Asocianom = "ASOCIANOM";
        public string Asociatipomedi = "ASOCIATIPOMEDI";
        //adicionales
        public string Asodetcodi = "ASODETCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Asodettipomedi = "ASODETTIPOMEDI";

        #region Consultas a BD
        public string SqlListUnidadAgrupadaByTipo
        {
            get { return base.GetSqlXml("ListUnidadAgrupadaByTipo"); }
        }
        public string SqlListUnidadByAgrupacion
        {
            get { return base.GetSqlXml("ListUnidadByAgrupacion"); }
        }
        public string SqlDeleteAsociacionDetalleByTipo
        {
            get { return base.GetSqlXml("DeleteAsociacionDetalleByTipo"); }
        }
        public string SqlDeleteAsociacionByTipo
        {
            get { return base.GetSqlXml("DeleteAsociacionByTipo"); }
        }
        public string SqlListAsociacionDetalleByTipo
        {
            get { return base.GetSqlXml("ListAsociacionDetalleByTipo"); }
        }
        public string SqlSaveDetalle
        {
            get { return base.GetSqlXml("SaveDetalle"); }
        }
        public string SqlGetMaxIdDetalle
        {
            get { return base.GetSqlXml("GetMaxIdDetalle"); }
        }
        #endregion

    }
}
