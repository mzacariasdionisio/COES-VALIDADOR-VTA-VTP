using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class RerEvaluacionEnergiaUnidDetHelper : HelperBase
    {
        public RerEvaluacionEnergiaUnidDetHelper() : base(Consultas.RerEvaluacionEnergiaUnidDetSql)
        {
        }

        #region Mapeo de Campos
        public string Rereedcodi = "REREEDCODI";
        public string Rereeucodi = "REREEUCODI";
        public string Rereedenergiaunidad = "REREEDENERGIAUNIDAD";
        #endregion

        public RerEvaluacionEnergiaUnidDetDTO Create(IDataReader dr)
        {
            RerEvaluacionEnergiaUnidDetDTO entity = new RerEvaluacionEnergiaUnidDetDTO();

            int iRereedcodi = dr.GetOrdinal(this.Rereedcodi);
            if (!dr.IsDBNull(iRereedcodi)) entity.Rereedcodi = Convert.ToInt32(dr.GetValue(iRereedcodi));

            int iRereeucodi = dr.GetOrdinal(this.Rereeucodi);
            if (!dr.IsDBNull(iRereeucodi)) entity.Rereeucodi = Convert.ToInt32(dr.GetValue(iRereeucodi));

            int iRereedenergiaunidad = dr.GetOrdinal(this.Rereedenergiaunidad);
            if (!dr.IsDBNull(iRereedenergiaunidad)) entity.Rereedenergiaunidad = dr.GetString(iRereedenergiaunidad);

            return entity;
        }
    }
}

