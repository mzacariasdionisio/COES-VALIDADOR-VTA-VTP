using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Dominio.DTO.Sp7;

namespace COES.Infraestructura.Datos.Helper.Sp7
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_ESTADCANALR_SP7
    /// </summary>
    public class TrEstadcanalrSp7Helper : HelperBase
    {
        public TrEstadcanalrSp7Helper(): base(Consultas.TrEstadcanalrSp7Sql)
        {
        }

        public TrEstadcanalrSp7DTO Create(IDataReader dr)
        {
            TrEstadcanalrSp7DTO entity = new TrEstadcanalrSp7DTO();

            int iEstcnlcodi = dr.GetOrdinal(this.Estcnlcodi);
            if (!dr.IsDBNull(iEstcnlcodi)) entity.Estcnlcodi = Convert.ToInt32(dr.GetValue(iEstcnlcodi));

            int iVercodi = dr.GetOrdinal(this.Vercodi);
            if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iZonacodi = dr.GetOrdinal(this.Zonacodi);
            if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

            int iEstcnlfecha = dr.GetOrdinal(this.Estcnlfecha);
            if (!dr.IsDBNull(iEstcnlfecha)) entity.Estcnlfecha = dr.GetDateTime(iEstcnlfecha);

            int iEstcnltvalido = dr.GetOrdinal(this.Estcnltvalido);
            if (!dr.IsDBNull(iEstcnltvalido)) entity.Estcnltvalido = dr.GetDecimal(iEstcnltvalido);

            int iEstcnltcong = dr.GetOrdinal(this.Estcnltcong);
            if (!dr.IsDBNull(iEstcnltcong)) entity.Estcnltcong = dr.GetDecimal(iEstcnltcong);

            int iEstcnltindet = dr.GetOrdinal(this.Estcnltindet);
            if (!dr.IsDBNull(iEstcnltindet)) entity.Estcnltindet = dr.GetDecimal(iEstcnltindet);

            int iEstcnltnnval = dr.GetOrdinal(this.Estcnltnnval);
            if (!dr.IsDBNull(iEstcnltnnval)) entity.Estcnltnnval = dr.GetDecimal(iEstcnltnnval);

            int iEstcnlultcalidad = dr.GetOrdinal(this.Estcnlultcalidad);
            if (!dr.IsDBNull(iEstcnlultcalidad)) entity.Estcnlultcalidad = Convert.ToInt32(dr.GetValue(iEstcnlultcalidad));

            int iEstcnlultcambio = dr.GetOrdinal(this.Estcnlultcambio);
            if (!dr.IsDBNull(iEstcnlultcambio)) entity.Estcnlultcambio = dr.GetDateTime(iEstcnlultcambio);

            int iEstcnlultcambioe = dr.GetOrdinal(this.Estcnlultcambioe);
            if (!dr.IsDBNull(iEstcnlultcambioe)) entity.Estcnlultcambioe = dr.GetDateTime(iEstcnlultcambioe);

            int iEstcnlultvalor = dr.GetOrdinal(this.Estcnlultvalor);
            if (!dr.IsDBNull(iEstcnlultvalor)) entity.Estcnlultvalor = dr.GetDecimal(iEstcnlultvalor);

            int iEstcnltretraso = dr.GetOrdinal(this.Estcnltretraso);
            if (!dr.IsDBNull(iEstcnltretraso)) entity.Estcnltretraso = dr.GetDecimal(iEstcnltretraso);

            int iEstcnlnumregistros = dr.GetOrdinal(this.Estcnlnumregistros);
            if (!dr.IsDBNull(iEstcnlnumregistros)) entity.Estcnlnumregistros = Convert.ToInt32(dr.GetValue(iEstcnlnumregistros));

            int iEstcnlverantcodi = dr.GetOrdinal(this.Estcnlverantcodi);
            if (!dr.IsDBNull(iEstcnlverantcodi)) entity.Estcnlverantcodi = Convert.ToInt32(dr.GetValue(iEstcnlverantcodi));

            int iEstcnlverdiaantcodi = dr.GetOrdinal(this.Estcnlverdiaantcodi);
            if (!dr.IsDBNull(iEstcnlverdiaantcodi)) entity.Estcnlverdiaantcodi = Convert.ToInt32(dr.GetValue(iEstcnlverdiaantcodi));

            int iEstcnlingreso = dr.GetOrdinal(this.Estcnlingreso);
            if (!dr.IsDBNull(iEstcnlingreso)) entity.Estcnlingreso = dr.GetString(iEstcnlingreso);

            int iEstcnlusucreacion = dr.GetOrdinal(this.Estcnlusucreacion);
            if (!dr.IsDBNull(iEstcnlusucreacion)) entity.Estcnlusucreacion = dr.GetString(iEstcnlusucreacion);

            int iEstcnlfeccreacion = dr.GetOrdinal(this.Estcnlfeccreacion);
            if (!dr.IsDBNull(iEstcnlfeccreacion)) entity.Estcnlfeccreacion = dr.GetDateTime(iEstcnlfeccreacion);

            int iEstcnlusumodificacion = dr.GetOrdinal(this.Estcnlusumodificacion);
            if (!dr.IsDBNull(iEstcnlusumodificacion)) entity.Estcnlusumodificacion = dr.GetString(iEstcnlusumodificacion);

            int iEstcnlfecmodificacion = dr.GetOrdinal(this.Estcnlfecmodificacion);
            if (!dr.IsDBNull(iEstcnlfecmodificacion)) entity.Estcnlfecmodificacion = dr.GetDateTime(iEstcnlfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Estcnlcodi = "ESTCNLCODI";
        public string Vercodi = "VERCODI";
        public string Canalcodi = "CANALCODI";
        public string Emprcodi = "EMPRCODI";
        public string Zonacodi = "ZONACODI";
        public string Estcnlfecha = "ESTCNLFECHA";
        public string Estcnltvalido = "ESTCNLTVALIDO";
        public string Estcnltcong = "ESTCNLTCONG";
        public string Estcnltindet = "ESTCNLTINDET";
        public string Estcnltnnval = "ESTCNLTNNVAL";
        public string Estcnlultcalidad = "ESTCNLULTCALIDAD";
        public string Estcnlultcambio = "ESTCNLULTCAMBIO";
        public string Estcnlultcambioe = "ESTCNLULTCAMBIOE";
        public string Estcnlultvalor = "ESTCNLULTVALOR";
        public string Estcnltretraso = "ESTCNLTRETRASO";
        public string Estcnlnumregistros = "ESTCNLNUMREGISTROS";
        public string Estcnlverantcodi = "ESTCNLVERANTCODI";
        public string Estcnlverdiaantcodi = "ESTCNLVERDIAANTCODI";
        public string Estcnlingreso = "ESTCNLINGRESO";        
        public string Estcnlusucreacion = "ESTCNLUSUCREACION";
        public string Estcnlfeccreacion = "ESTCNLFECCREACION";
        public string Estcnlusumodificacion = "ESTCNLUSUMODIFICACION";
        public string Estcnlfecmodificacion = "ESTCNLFECMODIFICACION";
        public string Verfechaini = "VERFECHAINI";

        public string Canalnomb = "CANALNOMB";
        public string Canaliccp = "CANALICCP";
        public string Canalunidad = "CANALUNIDAD";
        public string Emprenomb = "EMPRENOMB";
        public string Zonanomb = "ZONANOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string ObtenerListadoClasif
        {
            get { return base.GetSqlXml("ObtenerListadoClasif"); }
        }


        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string TotalRegistrosClasif
        {
            get { return base.GetSqlXml("TotalRegistrosClasif"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }

        public string SqlListVercodiFecha
        {
            get { return base.GetSqlXml("ListVercodiFecha"); }
        }

        public string SqlGetMinId
        {
            get { return base.GetSqlXml("GetMinId"); }
        }

        public string SqlGetDispDiaSignal
        {
            get { return base.GetSqlXml("GetDispDiaSignal"); }
        }

        


        #endregion
    }
}
