using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class MeEnvioEveEventoHelper : HelperBase
    {
        public MeEnvioEveEventoHelper()
            : base(Consultas.MeEnvioEveEventoSql)
        {
        }

        public MeEnvioEveEventoDTO Create(IDataReader dr)
        {
            MeEnvioEveEventoDTO entity = new MeEnvioEveEventoDTO();

            int iEnv_evencodi = dr.GetOrdinal(this.Env_evencodi);
            if (!dr.IsDBNull(iEnv_evencodi)) entity.Env_Evencodi = Convert.ToInt32(dr.GetValue(iEnv_evencodi));

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iEnvetapainforme = dr.GetOrdinal(this.Envetapainforme);
            if (!dr.IsDBNull(iEnvetapainforme)) entity.Envetapainforme = Convert.ToInt32(dr.GetValue(iEnvetapainforme));

            return entity;
        }

        #region Mapeo de Campos
        public string Env_evencodi = "ENV_EVENCODI";
        public string Enviocodi = "ENVIOCODI";
        public string Evencodi = "EVENCODI";
        public string Envetapainforme = "ENVETAPAINFORME";
        #endregion

    }
}
