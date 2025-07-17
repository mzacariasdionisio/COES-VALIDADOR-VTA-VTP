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
    public class PrnRelacionTnaHelper : HelperBase
    {
        public PrnRelacionTnaHelper() : base(Consultas.PrnRelacionTnaSql)
        {
        }

        public PrnRelacionTnaDTO Create(IDataReader dr)
        {
            PrnRelacionTnaDTO entity = new PrnRelacionTnaDTO();

            int iReltnacodi = dr.GetOrdinal(this.Reltnacodi);
            if (!dr.IsDBNull(iReltnacodi)) entity.Reltnacodi = Convert.ToInt32(dr.GetValue(iReltnacodi));

            int iPtomedicodi = dr.GetOrdinal(this.Reltnaformula);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Reltnaformula = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iReltnanom = dr.GetOrdinal(this.Reltnanom);
            if (!dr.IsDBNull(iReltnanom)) entity.Reltnanom = dr.GetString(iReltnanom);

            int iReltnadetcodi = dr.GetOrdinal(this.Reltnadetcodi);
            if (!dr.IsDBNull(iReltnadetcodi)) entity.Reltnadetcodi = Convert.ToInt32(dr.GetValue(iReltnadetcodi));

            int iBarracodi = dr.GetOrdinal(this.Barracodi);
            if (!dr.IsDBNull(iBarracodi)) entity.Barracodi = Convert.ToInt32(dr.GetValue(iBarracodi));

            int iPtomedicodi2 = dr.GetOrdinal(this.Reltnadetformula);
            if (!dr.IsDBNull(iPtomedicodi2)) entity.Reltnadetformula = Convert.ToInt32(dr.GetValue(iPtomedicodi2));

            return entity;
        }

        public string Reltnacodi = "RELTNACODI";
        public string Reltnaformula = "RELTNAFORMULA";
        public string Reltnanom = "RELTNANOM";
        //adicionales
        public string Reltnadetcodi = "RELTNADETCODI";
        public string Barracodi = "BARRACODI";
        public string Barranom = "BARRANOM";
        public string Reltnadetformula = "RELTNADETFORMULA";
        public string Formulanomb = "FORMULANOMB";

        #region Consultas a BD
        public string SqlListRelacionTnaDetalleById
        {
            get { return base.GetSqlXml("ListRelacionTnaDetalleById"); }
        }
        public string SqlListRelacionTnaDetalle
        {
            get { return base.GetSqlXml("ListRelacionTnaDetalle"); }
        }
        public string SqlGetMaxIdRelacionTnaDetalle
        {
            get { return base.GetSqlXml("GetMaxIdRelacionTnaDetalle"); }
        }
        public string SqlSaveRelacionTnaDetalle
        {
            get { return base.GetSqlXml("SaveRelacionTnaDetalle"); }
        }
        public string SqlDeleteRelacionTnaDetalle
        {
            get { return base.GetSqlXml("DeleteRelacionTnaDetalle"); }
        }
        #endregion
    }
}
