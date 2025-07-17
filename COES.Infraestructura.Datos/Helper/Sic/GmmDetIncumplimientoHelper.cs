using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class GmmDetIncumplimientoHelper : HelperBase
    {
        public GmmDetIncumplimientoHelper()
            : base(Consultas.GmmDetIncumplimientoSql)
        {
        }

        #region Mapeo de Campos
        public string Incucodi = "INCUCODI";
        public string Tinfcodi = "TINFCODI";
        public string Dincfecrecepcion = "DINCFECRECEPCION";
        public string Dincarchivo = "DINCARCHIVO";
        public string Dinccodi = "DINCCODI";
        public string Dincestado = "DINCESTADO";

        public string Detdinccodi = "Detdinccodi";
        public string DetTipInforme = "DetTipInforme";
        public string DetIncFecEntregaDet = "DetIncFecEntregaDet";
        public string DetIncArchivoDet = "DetIncArchivoDet";

        //public string Tinfcodi = "TINFCODI";
        public string Tinfinforme = "TINFINFORME";
        #endregion

        public string SqlListarArchivos
        {
            get { return base.GetSqlXml("ListarArchivos"); }
        }
        public string SqlListarTipoInforme
        {
            get { return base.GetSqlXml("ListarTipoInforme"); }
        }
        public GmmDetIncumplimientoDTO CreateListarArchivos(IDataReader dr)
        {
            GmmDetIncumplimientoDTO entity = new GmmDetIncumplimientoDTO();

            #region Incumplimiento
            int iDetdinccodi = dr.GetOrdinal(this.Detdinccodi);
            if (!dr.IsDBNull(iDetdinccodi)) entity.Detdinccodi = dr.GetInt32(iDetdinccodi);

            int iDetTipInforme = dr.GetOrdinal(this.DetTipInforme);
            if (!dr.IsDBNull(iDetTipInforme)) entity.DetTipInforme = dr.GetString(iDetTipInforme);

            int iDetIncFecEntregaDet = dr.GetOrdinal(this.DetIncFecEntregaDet);
            if (!dr.IsDBNull(iDetIncFecEntregaDet)) entity.DetIncFecEntregaDet = dr.GetDateTime(iDetIncFecEntregaDet);

            int iDetIncArchivoDet = dr.GetOrdinal(this.DetIncArchivoDet);
            if (!dr.IsDBNull(iDetIncArchivoDet)) entity.DetIncArchivoDet = dr.GetString(iDetIncArchivoDet);
            #endregion

            return entity;
        }

        public GmmTipInformeDTO CreateListaTipoInforme(IDataReader dr)
        {
            GmmTipInformeDTO entity = new GmmTipInformeDTO();

            int iTinfcodi = dr.GetOrdinal(this.Tinfcodi);
            if (!dr.IsDBNull(iTinfcodi)) entity.TINFCODI = dr.GetString(iTinfcodi);

            int iTinfinforme = dr.GetOrdinal(this.Tinfinforme);
            if (!dr.IsDBNull(iTinfinforme)) entity.TINFINFORME = dr.GetString(iTinfinforme);

            return entity;
        }
    }
}
