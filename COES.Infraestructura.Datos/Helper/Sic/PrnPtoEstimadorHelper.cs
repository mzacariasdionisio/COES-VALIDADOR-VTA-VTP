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
    public class PrnPtoEstimadorHelper : HelperBase
    {
        public PrnPtoEstimadorHelper() : base(Consultas.PrnPtoEstimadorSql)
        {
        }

        public PrnPtoEstimadorDTO Create(IDataReader dr)
        {
            PrnPtoEstimadorDTO entity = new PrnPtoEstimadorDTO();

            int iPtoetmcodi = dr.GetOrdinal(this.Ptoetmcodi);
            if (!dr.IsDBNull(iPtoetmcodi)) entity.Ptoetmcodi = Convert.ToInt32(dr.GetValue(iPtoetmcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoetmcodi));

            int iPtoetmtipomedi = dr.GetOrdinal(this.Ptoetmtipomedi);
            if (!dr.IsDBNull(iPtoetmtipomedi)) entity.Ptoetmtipomedi = dr.GetString(iPtoetmtipomedi);

            int iPtomedidesc = dr.GetOrdinal(this.Ptomedidesc);
            if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

            return entity;
        }

        public string Ptoetmcodi = "PTOETMCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptoetmtipomedi = "PTOETMTIPOMEDI";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Origlectcodi = "ORIGLECTCODI";
    }
}
