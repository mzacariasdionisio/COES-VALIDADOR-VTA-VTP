using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_CIRCULAR_SP7
    /// </summary>
    public class TrCircularSp7Helper : HelperBase
    {
        public TrCircularSp7Helper(): base(Consultas.TrCircularSp7Sql)
        {
        }

        public TrCircularSp7DTO Create(IDataReader dr)
        {
            TrCircularSp7DTO entity = new TrCircularSp7DTO();

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iCanalfhsist = dr.GetOrdinal(this.Canalfhsist);
            if (!dr.IsDBNull(iCanalfhsist)) entity.Canalfhsist = dr.GetDateTime(iCanalfhsist);

            int iCanalvalor = dr.GetOrdinal(this.Canalvalor);
            if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

            int iCanalcalidad = dr.GetOrdinal(this.Canalcalidad);
            if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

            int iCanalfechahora = dr.GetOrdinal(this.Canalfechahora);
            if (!dr.IsDBNull(iCanalfechahora)) entity.Canalfechahora = dr.GetDateTime(iCanalfechahora);

            return entity;
        }


        #region Mapeo de Campos

        public string Canalcodi = "CANALCODI";
        public string Canalfhsist = "CANALFHSIST";
        public string Canalvalor = "CANALVALOR";
        public string Canalcalidad = "CANALCALIDAD";
        public string Canalfechahora = "CANALFECHAHORA";

        public string Canalnomb = "CANALNOMB";
        public string Calnomb = "CALNOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string ObtenerListadoRango
        {
            get { return base.GetSqlXml("ObtenerListadoRango"); }
        }

        public string ObtenerListadoClasif
        {
            get { return base.GetSqlXml("ObtenerListadoClasif"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string CrearTablaCampo0
        {
            get { return base.GetSqlXml("CrearTablaCampo0"); }
        }

        public string CrearTablaCampo1
        {
            get { return base.GetSqlXml("CrearTablaCampo1"); }
        }

        public string CrearTablaCampo2
        {
            get { return base.GetSqlXml("CrearTablaCampo2"); }
        }
        public string CrearTablaCampo3
        {
            get { return base.GetSqlXml("CrearTablaCampo3"); }
        }
        public string CrearTablaCampo4
        {
            get { return base.GetSqlXml("CrearTablaCampo4"); }
        }
        public string CrearTablaCampo5
        {
            get { return base.GetSqlXml("CrearTablaCampo5"); }
        }
        public string CrearTablaCampo6
        {
            get { return base.GetSqlXml("CrearTablaCampo6"); }
        }
        public string CrearTablaCampo7
        {
            get { return base.GetSqlXml("CrearTablaCampo7"); }
        }

        #endregion

        public string SqlObtenerConsultaFlujos
        {
            get { return base.GetSqlXml("ObtenerConsultaFlujos"); }
        }

        public string SqlObtenerCircularesPorFecha
        {
            get { return base.GetSqlXml("ObtenerCircularesPorFecha"); }
        }

        public string SqlObtenerCanalesParaMuestraInstantanea
        {
            get { return base.GetSqlXml("ObtenerCanalesParaMuestraInstantanea"); }
        }

        public string SqlObtenerCircularesPorIntervalosDeFechaMuestraInstantanea
        {
            get { return base.GetSqlXml("ObtenerCircularesPorIntervalosDeFechaMuestraInstantanea"); }
        }

        public string SqlGetCanalById
        {
            get { return base.GetSqlXml("GetCanalById"); }
        }

        public string SqlEliminarRegistroMuestraInstantanea
        {
            get { return base.GetSqlXml("EliminarRegistroMuestraInstantanea"); }
        }

        public string SqlGenerarRegistroMuestraInstantanea
        {
            get { return base.GetSqlXml("GenerarRegistroMuestraInstantanea"); }
        }

        public string GetCalidadesSp7
        {
            get { return base.GetSqlXml("GetCalidadesSp7"); }
        }

    }
}
