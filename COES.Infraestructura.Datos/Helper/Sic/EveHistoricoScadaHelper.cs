using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Globalization;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class EveHistoricoScadaHelper : HelperBase
    {
        public EveHistoricoScadaHelper() : base(Consultas.EveHistoricoScadaSql)
        {
        }

        public EveHistoricoScadaDTO Create(IDataReader dr)
        {
            EveHistoricoScadaDTO entity = new EveHistoricoScadaDTO();

            int iEvehistscdacodi = dr.GetOrdinal(this.Evehistscdacodi);
            if (!dr.IsDBNull(iEvehistscdacodi)) entity.EVEHISTSCDACODI = dr.GetInt32(iEvehistscdacodi);

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);

            int iEvehistscdazonacodi = dr.GetOrdinal(this.Evehistscdazonacodi);
            if (!dr.IsDBNull(iEvehistscdazonacodi)) entity.EVEHISTSCDAZONACODI = dr.GetInt32(iEvehistscdazonacodi);

            int iEvehistscdacanalcodi = dr.GetOrdinal(this.Evehistscdacanalcodi);
            if (!dr.IsDBNull(iEvehistscdacanalcodi)) entity.EVEHISTSCDACANALCODI = dr.GetInt32(iEvehistscdacanalcodi);

            int iEvehistscdacodiequipo = dr.GetOrdinal(this.Evehistscdacodiequipo);
            if (!dr.IsDBNull(iEvehistscdacodiequipo)) entity.EVEHISTSCDACODIEQUIPO = dr.GetString(iEvehistscdacodiequipo);

            int iEvehistscdafechdesconexion = dr.GetOrdinal(this.Evehistscdafechdesconexion);
            if (!dr.IsDBNull(iEvehistscdafechdesconexion))
            {
                var fecha = dr.GetValue(iEvehistscdafechdesconexion);
                DateTime Evehistscdafechdesconexion = DateTime.ParseExact(fecha.ToString(), "dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
                entity.EVEHISTSCDAFECHDESCONEXION = Evehistscdafechdesconexion;
                entity.strEVEHISTSCDAFECHDESCONEXION = fecha.ToString();
            }
                

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Evehistscdacodi = "EVEHISTSCDACODI";
        public string Evencodi = "EVENCODI";
        public string Evehistscdazonacodi = "EVEHISTSCDAZONACODI";
        public string Evehistscdacanalcodi = "EVEHISTSCDACANALCODI";
        public string Evehistscdacodiequipo = "EVEHISTSCDACODIEQUIPO";
        public string Evehistscdafechdesconexion = "EVEHISTSCDAFECHDESCONEXION";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string EVEHISTSCDAZONADESC = "EVEHISTSCDAZONADESC";
        public string EVEHISTSCDACANALDESC = "EVEHISTSCDACANALDESC";

        #endregion

        #region Campos adicionales
        public string Zonaabrev = "ZONAABREV";
        public string Canalnomb = "CANALNOMB";
        #endregion

        public string SqlDeleteAll
        {
            get { return base.GetSqlXml("DeleteAll"); }
        }
    }
}
