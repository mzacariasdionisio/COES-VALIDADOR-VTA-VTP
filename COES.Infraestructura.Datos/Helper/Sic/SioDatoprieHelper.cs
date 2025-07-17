using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SIO_DATOPRIE
    /// </summary>
    public class SioDatoprieHelper : HelperBase
    {
        public SioDatoprieHelper()
            : base(Consultas.SioDatoprieSql)
        {
        }

        public SioDatoprieDTO Create(IDataReader dr)
        {
            SioDatoprieDTO entity = new SioDatoprieDTO();

            int iDpriecodi = dr.GetOrdinal(this.Dpriecodi);
            if (!dr.IsDBNull(iDpriecodi)) entity.Dpriecodi = Convert.ToInt32(dr.GetValue(iDpriecodi));

            int iDprievalor = dr.GetOrdinal(this.Dprievalor);
            if (!dr.IsDBNull(iDprievalor)) entity.Dprievalor = dr.GetString(iDprievalor);

            int iDprieperiodo = dr.GetOrdinal(this.Dprieperiodo);
            if (!dr.IsDBNull(iDprieperiodo)) entity.Dprieperiodo = dr.GetDateTime(iDprieperiodo);

            int iDpriefechadia = dr.GetOrdinal(this.Dpriefechadia);
            if (!dr.IsDBNull(iDpriefechadia)) entity.Dpriefechadia = dr.GetDateTime(iDpriefechadia);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprcodi2 = dr.GetOrdinal(this.Emprcodi2);
            if (!dr.IsDBNull(iEmprcodi2)) entity.Emprcodi2 = Convert.ToInt32(dr.GetValue(iEmprcodi2));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iDprieusuario = dr.GetOrdinal(this.Dprieusuario);
            if (!dr.IsDBNull(iDprieusuario)) entity.Dprieusuario = dr.GetString(iDprieusuario);

            int iDpriefecha = dr.GetOrdinal(this.Dpriefecha);
            if (!dr.IsDBNull(iDpriefecha)) entity.Dpriefecha = dr.GetDateTime(iDpriefecha);

            int iCabpricodi = dr.GetOrdinal(this.Cabpricodi);
            if (!dr.IsDBNull(iCabpricodi)) entity.Cabpricodi = Convert.ToInt32(dr.GetValue(iCabpricodi));

            return entity;
        }

        public string SqlGetByCabpricodi
        {
            get { return base.GetSqlXml("GetByCabpricodi"); }
        }

        //Inicio Tabla 26
        public string SqlGetListaDifusionEnergPrie
        {
            get { return base.GetSqlXml("GetListaDifusionEnergPrie"); }
        }
        //Fin 
        /// <summary>
        /// Tabla 07
        /// </summary>
        public string SqlGetListaByCabpricodi
        {
            get { return base.GetSqlXml("GetListaByCabpricodi"); }
        }




        public string SqlGetDifusionTransfPotencia
        {
            get { return base.GetSqlXml("GetDifusionTransfPotencia"); }
        }


        public string SqlGetListaDifusionEnergPrieByFiltro
        {
            get { return base.GetSqlXml("GetListaDifusionEnergPrieByFiltro"); }
        }


        public string SqlGetByLectPtomedFechaOrdenPrie
        {
            get { return base.GetSqlXml("GetByLectPtomedFechaOrden"); }
        }

        public string SqlGetListaDifusionCostoMarginal
        {
            get { return base.GetSqlXml("GetListaDifusionCostoMarginal"); }
        }
        public string SqlGetCostoVariableByFiltro
        {
            get { return base.GetSqlXml("GetCostoVariableByFiltro"); }
        }


        public string SqlValidarDataPorCodigoCabecera
        {
            get { return base.GetSqlXml("ValidarDataPorCodigoCabecera"); }
        }

        public string SqlBorrarDataPorCodigoCabecera
        {
            get { return base.GetSqlXml("BorrarDataPorCodigoCabecera"); }
        }

        public string SqlDeleteByPeriodoAndPriecodi
        {
            get { return base.GetSqlXml("DeleteByPeriodoAndPriecodi"); }
        }

        #region SIOSEIN

        public string SqlGetSioDatosprieByCriteria
        {
            get { return base.GetSqlXml("GetSioDatosprieByCriteria"); }
        }

        #endregion

        #region "SIOSEIN-PRIE-2021"

        public string SqlGetByCabpricodi2
        {
            get { return base.GetSqlXml("GetByCabpricodi2"); }
        }

        public string SqlGetReporteRR05ByOsinergcodi
        {
            get { return base.GetSqlXml("GetReporteRR05ByOsinergcodi"); }
        }

        public string SqlGetReporteR05MDTByOsinergcodi
        {
            get { return base.GetSqlXml("GetReporteR05MDTByOsinergcodi"); }
        }

        public string SqlGetReporteR05IEyR05MDE
        {
            get { return base.GetSqlXml("GetReporteR05IEyR05MDE"); }
        }

        public string SqlObtenerMeMedicion48
        {
            get { return base.GetSqlXml("ObtenerMeMedicion48"); }
        }
        public string SqlObtenerMeMedicion24
        {
            get { return base.GetSqlXml("ObtenerMeMedicion24"); }
        }
        public string SqlObtenerMeMedicionxIntervalo
        {
            get { return base.GetSqlXml("ObtenerMeMedicionxIntervalo"); }
        }

        #endregion

        #region Mapeo de Campos

        public string Dpriecodi = "DPRIECODI";
        public string Dprievalor = "DPRIEVALOR";
        public string Dprieperiodo = "DPRIEPERIODO";
        public string Dpriefechadia = "DPRIEFECHADIA";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Barrcodi = "BARRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Emprcodi2 = "EMPRCODI2";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Dprieusuario = "DPRIEUSUCREACION";
        public string Dpriefecha = "DPRIEFECCREACION";
        public string Cabpricodi = "CABPRICODI";

        //campo para conteo
        public string Count = "COUNT";

        #region "SIOSEIN-PRIE-2021"
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Famcodi = "FAMCODI";
        public string Osinergcodi = "OSINERGCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Tipogrupocodi = "TIPOGRUPOCODI";
        public string Tipogenerrer = "TIPOGENERRER";
        public string Tgenercodi = "TGENERCODI";
        public string Tgenernomb = "TGENERNOMB";
        public string Fenergcodi = "FENERGCODI";
        public string Fenergnomb = "FENERGNOMB";
        public string Medifecha = "MEDIFECHA";
        public string Maxdemanda = "MAXDEMANDA";
        public string Codcentral = "CODCENTRAL";
        public string Central = "CENTRAL";
        public string Grupomiembro = "GRUPOMIEMBRO";
        public string Ctgdetnomb = "CTGDETNOMB";

        public string Lectcodi = "LECTCODI";
        public string Osicodi = "OSICODI";
        public string Tptomedicodi = "TPTOMEDICODI";
        public string Tptomedinomb = "TPTOMEDINOMB";
        public string Equiabrev = "EQUIABREV";
        public string Tptomedielenomb = "TPTOMEDIELENOMB";
        public string Tptomedidesc = "TPTOMEDIDESC";
        public string Medinth1 = "MEDINTH1";
        #endregion

        #endregion
    }
}
