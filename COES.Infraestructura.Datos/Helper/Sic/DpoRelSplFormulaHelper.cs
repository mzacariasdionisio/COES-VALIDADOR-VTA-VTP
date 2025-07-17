using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoRelSplFormulaHelper : HelperBase
    {
        public DpoRelSplFormulaHelper() : base(Consultas.DpoRelSplFormulaSql)
        {
        }

        public DpoRelSplFormulaDTO Create(IDataReader dr)
        {
            DpoRelSplFormulaDTO entity = new DpoRelSplFormulaDTO();

            int iSplfrmcodi = dr.GetOrdinal(this.Splfrmcodi);
            if (!dr.IsDBNull(iSplfrmcodi)) entity.Splfrmcodi = Convert.ToInt32(dr.GetValue(iSplfrmcodi));

            int iDposplcodi = dr.GetOrdinal(this.Dposplcodi);
            if (!dr.IsDBNull(iDposplcodi)) entity.Dposplcodi = Convert.ToInt32(dr.GetValue(iDposplcodi));

            int iBarsplcodi= dr.GetOrdinal(this.Barsplcodi);
            if (!dr.IsDBNull(iBarsplcodi)) entity.Barsplcodi = Convert.ToInt32(dr.GetValue(iBarsplcodi));

            int iPtomedicodifveg = dr.GetOrdinal(this.Ptomedicodifveg);
            if (!dr.IsDBNull(iPtomedicodifveg)) entity.Ptomedicodifveg = Convert.ToInt32(dr.GetValue(iPtomedicodifveg));

            int iPtomedicodiful = dr.GetOrdinal(this.Ptomedicodiful);
            if (!dr.IsDBNull(iPtomedicodiful)) entity.Ptomedicodiful = Convert.ToInt32(dr.GetValue(iPtomedicodiful));

            return entity;
        }


        #region Mapeo de Campos
        public string Splfrmcodi = "SPLFRMCODI";
        public string Dposplcodi = "DPOSPLCODI";
        public string Barsplcodi = "BARSPLCODI";
        public string Ptomedicodifveg = "PTOMEDICODIFVEG";
        public string Ptomedicodiful = "PTOMEDICODIFUL";
        public string Splfrmarea = "SPLFRMAREA";
        //Adicionales
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Grupoabrev = "GRUPOABREV";
        public string Dposplnombre = "DPOSPLNOMBRE";
        public string Nombvegetativa = "NOMBVEGETATIVA";
        public string Nombindustrial = "NOMBINDUSTRIAL";
        public string Splareanombre = "SPLAREANOMBRE";
        #endregion

        public string SqlListBarrasxVersion
        {
            get { return GetSqlXml("ListBarrasxVersion"); }
        }

        public string SqlUpdateFormulas
        {
            get { return GetSqlXml("UpdateFormulas"); }
        }

        public string SqlListFormulasVegetativa
        {
            get { return GetSqlXml("ListFormulasVegetativa"); }
        }

        public string SqlListFormulasIndustrial
        {
            get { return GetSqlXml("ListFormulasIndustrial"); }
        }

        public string SqlDeleteByVersion
        {
            get { return GetSqlXml("DeleteByVersion"); }
        }

        public string SqlDeleteByVersionxBarra
        {
            get { return GetSqlXml("DeleteByVersionxBarra"); }
        }
    }
}
