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
    public class PrnVariableHelper : HelperBase
    {
        public PrnVariableHelper() : base(Consultas.PrnVariableSql)
        {
        }

        public PrnVariableDTO Create(IDataReader dr)
        {
            PrnVariableDTO entity = new PrnVariableDTO();

            int iPrnvarcodi = dr.GetOrdinal(this.Prnvarcodi);
            if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

            int iPrnvarnom = dr.GetOrdinal(this.Prnvarnom);
            if (!dr.IsDBNull(iPrnvarnom)) entity.Prnvarnom = dr.GetString(iPrnvarnom);

            int iPrnvarabrev = dr.GetOrdinal(this.Prnvarabrev);
            if (!dr.IsDBNull(iPrnvarabrev)) entity.Prnvarabrev = dr.GetString(iPrnvarabrev);

            int iPrnvartipomedi = dr.GetOrdinal(this.Prnvarabrev);
            if (!dr.IsDBNull(iPrnvartipomedi)) entity.Prnvartipomedi = dr.GetString(iPrnvartipomedi);

            return entity;
        }

        public string Prnvarcodi = "PRNVARCODI";
        public string Prnvarnom = "PRNVARNOM";
        public string Prnvarabrev = "PRNVARABREV";
        public string Prnvartipomedi = "PRNVARTIPOMEDI";
        
        #region Consultas a BD
        public string SqlListVariableByTipo
        {
            get { return base.GetSqlXml("ListVariableByTipo"); }
        }
        #endregion
    }
}
