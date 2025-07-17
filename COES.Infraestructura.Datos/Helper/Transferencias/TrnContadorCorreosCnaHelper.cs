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
    public class TrnContadorCorreosCnaHelper : HelperBase
    {
        public TrnContadorCorreosCnaHelper() : base(Consultas.TrnContadorCorreosCnaSql)
        {

        }

        public TrnContadorCorreosCnaDTO Create(IDataReader dr)
        {
            TrnContadorCorreosCnaDTO entity = new TrnContadorCorreosCnaDTO();

            #region Campos Originales
            // CONTCNACODI
            int iContcnacodi = dr.GetOrdinal(this.Contcnacodi);
            if (!dr.IsDBNull(iContcnacodi)) entity.Contcnacodi = dr.GetInt32(iContcnacodi);

            // EMPRCODI
            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = dr.GetInt32(iEmprCodi);

            // FECHAVIGENCIA
            int iCantcorreos = dr.GetOrdinal(this.Cantcorreos);
            if (!dr.IsDBNull(iCantcorreos)) entity.Cantcorreos = dr.GetInt32(iCantcorreos);

            // LASTUSER
            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            // LASTDATE
            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            #endregion

            return entity;
        }

        #region Campos Originales
        public string Contcnacodi = "CONTCNACODI";
        public string EmprCodi = "EMPRCODI";
        public string Cantcorreos = "CANTCORREOS";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        #endregion

        public string SqlObtenerContadorParticipante
        {
            get { return base.GetSqlXml("ObtenerContadorParticipante"); }
        }
    }
}
