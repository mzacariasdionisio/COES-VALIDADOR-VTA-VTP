using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SPO_NUMERALDAT
    /// </summary>
    public class SpoNumeraldatHelper : HelperBase
    {
        public SpoNumeraldatHelper(): base(Consultas.SpoNumeraldatSql)
        {
        }

        public SpoNumeraldatDTO Create(IDataReader dr)
        {
            SpoNumeraldatDTO entity = new SpoNumeraldatDTO();

            int iNumdatcodi = dr.GetOrdinal(this.Numdatcodi);
            if (!dr.IsDBNull(iNumdatcodi)) entity.Numdatcodi = Convert.ToInt32(dr.GetValue(iNumdatcodi));

            int iVerncodi = dr.GetOrdinal(this.Verncodi);
            if (!dr.IsDBNull(iVerncodi)) entity.Verncodi = Convert.ToInt32(dr.GetValue(iVerncodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iSconcodi = dr.GetOrdinal(this.Sconcodi);
            if (!dr.IsDBNull(iSconcodi)) entity.Sconcodi = Convert.ToInt32(dr.GetValue(iSconcodi));

            int iClasicodi = dr.GetOrdinal(this.Clasicodi);
            if (!dr.IsDBNull(iClasicodi)) entity.Clasicodi = Convert.ToInt32(dr.GetValue(iClasicodi));

            int iNumdatvalor = dr.GetOrdinal(this.Numdatvalor);
            if (!dr.IsDBNull(iNumdatvalor)) entity.Numdatvalor = dr.GetDecimal(iNumdatvalor);

            int iNumdatfechainicio = dr.GetOrdinal(this.Numdatfechainicio);
            if (!dr.IsDBNull(iNumdatfechainicio)) entity.Numdatfechainicio = dr.GetDateTime(iNumdatfechainicio);

            int iNumdatfechafin = dr.GetOrdinal(this.Numdatfechafin);
            if (!dr.IsDBNull(iNumdatfechafin)) entity.Numdatfechafin = dr.GetDateTime(iNumdatfechafin);

            return entity;
        }


        #region Mapeo de Campos

        public string Numdatcodi = "NUMDATCODI"; 
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Sconcodi = "SCONCODI";
        public string Clasicodi = "CLASICODI";
        public string Numdatvalor = "NUMDATVALOR";
        public string Numdatfechainicio = "NUMDATFECHAINICIO";
        public string Numdatfechafin = "NUMDATFECHAFIN";
        public string Verncodi = "VERNCODI";

        public string Clasinombre = "CLASINOMBRE";
        public string Sconnomb = "SCONNOMB";
        public string Sconactivo = "SCONACTIVO";
        public string Numcdescrip = "NUMCDESCRIP";
        public string Numecodi = "NUMECODI";
        public string Numccodi = "NUMCCODI";
        public string Sconorden = "SCONORDEN";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlGetDataNumerales
        {
            get { return base.GetSqlXml("GetDataNumerales"); }
        }

        public string SqlGetDataVAlorAgua
        {
            get { return base.GetSqlXml("GetDataVAlorAgua"); }
        }
    }
}
