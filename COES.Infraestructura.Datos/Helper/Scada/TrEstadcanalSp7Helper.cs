using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_ESTADCANAL_SP7
    /// </summary>
    public class TrEstadcanalSp7Helper : HelperBase
    {
        public TrEstadcanalSp7Helper(): base(Consultas.TrEstadcanalSp7Sql)
        {
        }

        public TrEstadcanalSp7DTO Create(IDataReader dr)
        {
            TrEstadcanalSp7DTO entity = new TrEstadcanalSp7DTO();

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iFecha = dr.GetOrdinal(this.Fecha);
            if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);

            int iTvalido = dr.GetOrdinal(this.Tvalido);
            if (!dr.IsDBNull(iTvalido)) entity.Tvalido = dr.GetDecimal(iTvalido);

            int iTcong = dr.GetOrdinal(this.Tcong);
            if (!dr.IsDBNull(iTcong)) entity.Tcong = dr.GetDecimal(iTcong);

            int iTindet = dr.GetOrdinal(this.Tindet);
            if (!dr.IsDBNull(iTindet)) entity.Tindet = dr.GetDecimal(iTindet);

            int iTnnval = dr.GetOrdinal(this.Tnnval);
            if (!dr.IsDBNull(iTnnval)) entity.Tnnval = dr.GetDecimal(iTnnval);

            int iUltcalidad = dr.GetOrdinal(this.Ultcalidad);
            if (!dr.IsDBNull(iUltcalidad)) entity.Ultcalidad = Convert.ToInt32(dr.GetValue(iUltcalidad));

            int iUltcambio = dr.GetOrdinal(this.Ultcambio);
            if (!dr.IsDBNull(iUltcambio)) entity.Ultcambio = dr.GetDateTime(iUltcambio);

            int iUltcambioe = dr.GetOrdinal(this.Ultcambioe);
            if (!dr.IsDBNull(iUltcambioe)) entity.Ultcambioe = dr.GetDateTime(iUltcambioe);

            int iUltvalor = dr.GetOrdinal(this.Ultvalor);
            if (!dr.IsDBNull(iUltvalor)) entity.Ultvalor = dr.GetDecimal(iUltvalor);

            int iTretraso = dr.GetOrdinal(this.Tretraso);
            if (!dr.IsDBNull(iTretraso)) entity.Tretraso = dr.GetDecimal(iTretraso);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iTrstdlastdate = dr.GetOrdinal(this.Trstdlastdate);
            if (!dr.IsDBNull(iTrstdlastdate)) entity.Trstdlastdate = dr.GetDateTime(iTrstdlastdate);

            int iNumregistros = dr.GetOrdinal(this.Numregistros);
            if (!dr.IsDBNull(iNumregistros)) entity.Numregistros = Convert.ToInt32(dr.GetValue(iNumregistros));

            int iTrstdingreso = dr.GetOrdinal(this.Trstdingreso);
            if (!dr.IsDBNull(iTrstdingreso)) entity.Trstdingreso = dr.GetString(iTrstdingreso);

            return entity;
        }


        #region Mapeo de Campos

        public string Canalcodi = "CANALCODI";
        public string Fecha = "FECHA";
        public string Tvalido = "TVALIDO";
        public string Tcong = "TCONG";
        public string Tindet = "TINDET";
        public string Tnnval = "TNNVAL";
        public string Ultcalidad = "ULTCALIDAD";
        public string Ultcambio = "ULTCAMBIO";
        public string Ultcambioe = "ULTCAMBIOE";
        public string Ultvalor = "ULTVALOR";
        public string Tretraso = "TRETRASO";
        public string Emprcodi = "EMPRCODI";
        public string Trstdlastdate = "TRSTDLASTDATE";
        public string Numregistros = "NUMREGISTROS";
        public string Trstdingreso = "TRSTDINGRESO";

        #endregion


        public string SqlGetDispDiaSignal
        {
            get { return base.GetSqlXml("GetDispDiaSignal"); }
        }

        public string SqlGePaginadoDiaSignal
        {
            get { return base.GetSqlXml("PaginadoDiaSignal"); }
        }




    }
}
