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
    public class DpoTrafoBarraHelper : HelperBase
    {
        public DpoTrafoBarraHelper() : base(Consultas.DpoTrafoBarraSql)
        {
        }

        public DpoTrafoBarraDTO Create(IDataReader dr)
        {
            DpoTrafoBarraDTO entity = new DpoTrafoBarraDTO();

            int iTnfbarcodi = dr.GetOrdinal(this.Tnfbarcodi);
            if (!dr.IsDBNull(iTnfbarcodi)) entity.Tnfbarcodi = Convert.ToInt32(dr.GetValue(iTnfbarcodi));

            int iTnfbartnfcodi = dr.GetOrdinal(this.Tnfbartnfcodi);
            if (!dr.IsDBNull(iTnfbartnfcodi)) entity.Tnfbartnfcodi = dr.GetString(iTnfbartnfcodi);

            int iTnfbarbarcodi = dr.GetOrdinal(this.Tnfbarbarcodi);
            if (!dr.IsDBNull(iTnfbarbarcodi)) entity.Tnfbarbarcodi = dr.GetString(iTnfbarbarcodi);

            int iTnfbarbarnombre = dr.GetOrdinal(this.Tnfbarbarnombre);
            if (!dr.IsDBNull(iTnfbarbarnombre)) entity.Tnfbarbarnombre = dr.GetString(iTnfbarbarnombre);

            int iTnfbarbartension = dr.GetOrdinal(this.Tnfbarbartension);
            if (!dr.IsDBNull(iTnfbarbartension)) entity.Tnfbarbartension = dr.GetDecimal(iTnfbarbartension);

            return entity;
        }


        #region Mapeo de Campos
        public string Tnfbarcodi = "TNFBARCODI";
        public string Tnfbartnfcodi = "TNFBARTNFCODI";
        public string Tnfbarbarcodi = "TNFBARBARCODI";
        public string Tnfbarbarnombre = "TNFBARBARNOMBRE";
        public string Tnfbarbartension = "TNFBARBARTENSION";
        #endregion

        public string SqlListTrafoBarraById
        {
            get { return GetSqlXml("ListTrafoBarraById"); }
        }
    }
}
