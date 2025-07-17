using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class TrnLogCnaHelper : HelperBase
    {
        public TrnLogCnaHelper() : base(Consultas.TrnLogCnaSql)
        {

        }

        public TrnLogCnaDTO Create(IDataReader dr)
        {
            TrnLogCnaDTO entity = new TrnLogCnaDTO();

            #region Campos Originales
            // LOGCNACODI
            int iLogcnacodi = dr.GetOrdinal(this.Logcnacodi);
            if (!dr.IsDBNull(iLogcnacodi)) entity.Logcnacodi = dr.GetInt32(iLogcnacodi);

            // EMPRCODI
            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = dr.GetInt32(iEmprCodi);

            // FECHAPROCESO
            int iFechaProceso = dr.GetOrdinal(this.FechaProceso);
            if (!dr.IsDBNull(iFechaProceso)) entity.FechaProceso = dr.GetDateTime(iFechaProceso);

            #endregion

            return entity;
        }

        #region Campos Originales
        public string Logcnacodi = "LOGCNACODI";
        public string EmprCodi = "EMPRCODI";
        public string FechaProceso = "FECHAPROCESO";
        #endregion
        public string CantCna = "CANTCNA";
        public string Emprrazsocial = "EMPRRAZSOCIAL";
    }
}
