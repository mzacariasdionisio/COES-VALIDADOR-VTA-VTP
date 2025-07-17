using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_MEDICIONXINTERVALO
    /// </summary>
    public class MeMedicionxintervaloHelper : HelperBase
    {
        public MeMedicionxintervaloHelper()
            : base(Consultas.MeMedicionxintervaloSql)
        {
        }

        public MeMedicionxintervaloDTO Create(IDataReader dr)
        {
            MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();

            int iMedintfechaini = dr.GetOrdinal(this.Medintfechaini);
            if (!dr.IsDBNull(iMedintfechaini)) entity.Medintfechaini = dr.GetDateTime(iMedintfechaini);

            int iMedintfechafin = dr.GetOrdinal(this.Medintfechafin);
            if (!dr.IsDBNull(iMedintfechafin)) entity.Medintfechafin = dr.GetDateTime(iMedintfechafin);

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iMedinth1 = dr.GetOrdinal(this.Medinth1);
            if (!dr.IsDBNull(iMedinth1)) entity.Medinth1 = dr.GetDecimal(iMedinth1);

            int iMedintusumodificacion = dr.GetOrdinal(this.Medintusumodificacion);
            if (!dr.IsDBNull(iMedintusumodificacion)) entity.Medintusumodificacion = dr.GetString(iMedintusumodificacion);

            int iMedintfecmodificacion = dr.GetOrdinal(this.Medintfecmodificacion);
            if (!dr.IsDBNull(iMedintfecmodificacion)) entity.Medintfecmodificacion = dr.GetDateTime(iMedintfecmodificacion);

            int iMedintdescrip = dr.GetOrdinal(this.Medintdescrip);
            if (!dr.IsDBNull(iMedintdescrip)) entity.Medintdescrip = dr.GetString(iMedintdescrip);

            int iMedestcodi = dr.GetOrdinal(this.Medestcodi);
            if (!dr.IsDBNull(iMedestcodi)) entity.Medestcodi = Convert.ToInt32(dr.GetValue(iMedestcodi));

            int iMedintcodi = dr.GetOrdinal(this.Medintcodi);
            if (!dr.IsDBNull(iMedintcodi)) entity.Medintcodi = Convert.ToInt32(dr.GetValue(iMedintcodi));

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iMedsemana = dr.GetOrdinal(this.Medintsemana);
            if (!dr.IsDBNull(iMedsemana)) entity.Medintsemana = Convert.ToInt32(dr.GetValue(iMedsemana));

            int iMedintanio = dr.GetOrdinal(this.Medintanio);
            if (!dr.IsDBNull(iMedintanio)) entity.Medintanio = dr.GetDateTime(iMedintanio);

            int iMedintblqhoras = dr.GetOrdinal(this.Medintblqhoras);
            if (!dr.IsDBNull(iMedintblqhoras)) entity.Medintblqhoras = Convert.ToDecimal(dr.GetValue(iMedintblqhoras));

            int iMedintblqnumero = dr.GetOrdinal(this.Medintblqnumero);
            if (!dr.IsDBNull(iMedintblqnumero)) entity.Medintblqnumero = Convert.ToInt32(dr.GetValue(iMedintblqnumero));

            int iTptomedicodi = dr.GetOrdinal(this.Tptomedicodi);
            if (!dr.IsDBNull(iTptomedicodi)) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(iTptomedicodi));
            entity.Tptomedicodi = entity.Tptomedicodi > 0 ? entity.Tptomedicodi : -1;

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }


        #region Mapeo de Campos

        public string Medintfechaini = "MEDINTFECHAINI";
        public string Medintfechafin = "MEDINTFECHAFIN";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Lectcodi = "LECTCODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Medinth1 = "MEDINTH1";
        public string Medinth_1 = "MEDINTH_1";
        public string Medintusumodificacion = "MEDINTUSUMODIFICACION";
        public string Medintfecmodificacion = "MEDINTFECMODIFICACION";
        public string Medintdescrip = "MEDINTDESCRIP";
        public string Medestcodi = "MEDESTCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Equicodi = "Equicodi";
        public string Equinomb = "Equinomb";
        public string Grupocodi = "Grupocodi";
        public string Gruponomb = "Gruponomb";
        public string Cuenca = "Cuenca";
        public string Tipoptomedinomb = "Tptomedinomb";
        public string Tipoinfoabrev = "Tipoinfoabrev";
        public string Ptomedibarranomb = "Ptomedibarranomb";
        public string Famcodi = "Famcodi";
        public string Famabrev = "Famabrev";
        public string Emprcoes = "Emprcoes";
        public string Ptomedielenomb = "Ptomedielenomb";
        public string Fenergnomb = "Fenergnomb";
        public string Fenercolor = "Fenercolor";
        public string Equipadre = "Equipadre";
        public string Equipopadre = "Equipopadre";
        public string Medintcodi = "MEDINTCODI";
        public string Enviocodi = "ENVIOCODI";
        public string Medintsemana = "MEDINTSEMANA";
        public string Medintanio = "MEDINTANIO";
        public string Medintblqhoras = "MEDINTBLQHORAS";
        public string Medintblqnumero = "MEDINTBLQNUMERO";
        public string Hojacodi = "HOJACODI";
        public string Hojanomb = "HOJANOMBRE";
        public string ObraFechaPlanificada = "ObraFechaPlanificada";

        #region SIOSEIN
        public string Catecodi = "CATECODI";
        public string Emprcodi = "EMPRCODI";
        public string Tgenercodi = "TGENERCODI";
        public string Tgenernomb = "TGENERNOMB";
        public string Codcentral = "CODCENTRAL";
        public string Central = "CENTRAL";
        public string Tipopagente = "TIPOAGENTE";
        public string Fenergcodi = "FENERGCODI";
        public string Tptomedinomb = "TPTOMEDINOMB";
        public string Tptomedicodi = "TPTOMEDICODI";
        public string Barrcodi = "BARRCODI";
        public string Barrnombre = "BARRNOMBRE";
        #endregion

        #region Numerales Datos Base
        public string Valor = "VALOR";
        #endregion

        public string Tptomedicodi2 = "TPTOMEDICODI2";
        public string Semana = "SEMANA";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Pmbloqnombre = "PMBLOQNOMBRE";

        public string Sconnomb = "SCONNOMB";
        #endregion

        public string SqlDeleteEnvioArchivo
        {
            get { return base.GetSqlXml("DeleteEnvioArchivo"); }
        }

        public string SqlGetEnvioArchivo
        {
            get { return base.GetSqlXml("GetEnvioArchivo"); }
        }
        
        public string SqlGetHidrologiaDescargaVert
        {
            get { return base.GetSqlXml("GetHidrologiaDescargaVert"); }
        }

        public string SqlGetHidrologiaDescargaVertPag
        {
            get { return base.GetSqlXml("GetHidrologiaDescargaVertPag"); }
        }

        public string SqlGetListaMedxintervStock
        {
            get { return base.GetSqlXml("GetListaMedxintervStock"); }
        }

        public string SqlGetListaMedxintervConsumo
        {
            get { return base.GetSqlXml("GetListaMedxintervConsumo"); }
        }

        public string SqlGetConsumoCentral
        {
            get { return base.GetSqlXml("GetConsumoCentral"); }
        }

        public string SqlDeleteEnvioFormato
        {
            get { return base.GetSqlXml("DeleteEnvioFormato"); }
        }

        public string SqlDeleteEnvioFormatoColumna
        {
            get { return base.GetSqlXml("DeleteEnvioFormatoColumna"); }
        }

        public string SqlGetListaMedxintervDisponibilidad
        {
            get { return base.GetSqlXml("GetListaMedxintervDisponibilidad"); }
        }

        public string SqlGetListaMedxintervQuema
        {
            get { return base.GetSqlXml("GetListaMedxintervQuema"); }
        }

        public string SqlGetListaMedxintervStockPag
        {
            get { return base.GetSqlXml("GetListaMedxintervStockPag"); }
        }

        public string SqlListaFiltrada
        {
            get { return base.GetSqlXml("ListaFiltrada"); }
        }

        public string SqlDeleteEnvioMedicionxIntervalo
        {
            get { return base.GetSqlXml("DeleteEnvioMedicionxIntervalo"); }
        }

        public string SqlBuscarRegistroPeriodo
        {
            get { return base.GetSqlXml("BuscarRegistroPeriodo"); }
        }

        #region Pr31

        public string SqlGetCombustibleXCentral
        {
            get { return base.GetSqlXml("GetCombustibleXCentral"); }
        }

        #endregion

        #region siosein2
        public string Tipogenerrer = "TIPOGENERRER";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Grupointegrante = "GRUPOINTEGRANTE";
        public string Osinergcodi = "OSINERGCODI";


        public string SqlGetListaMedicionXIntervaloByLecturaYTipomedicion
        {
            get { return GetSqlXml("GetListaMedicionXIntervaloByLecturaYTipomedicion"); }
        }

        public string SqlGetListaMedicionXIntervaloByLecturaYTipomedicionYCentral
        {
            get { return GetSqlXml("GetListaMedicionXIntervaloByLecturaYTipomedicionYCentral"); }
        }
        #endregion

        #region PMPO

        public string SqlListarReporteGeneracionSDDP
        {
            get { return base.GetSqlXml("ListarReporteGeneracionSDDP"); }
        }
        public string SqlListarReporteSDDP
        {
            get { return base.GetSqlXml("ListarReporteSDDP"); }
        }
        #endregion

        #region FIT - VALORIZACION DIARIA

        public string SqlGetDemandaMedianoPlazoCOES
        {
            get { return GetSqlXml("GetDemandaMedianoPlazoCOES"); }
        }

        #endregion

        #region Mejoras RDO
        public string SqlGetListaDisponibilidadCombustible
        {
            get { return base.GetSqlXml("GetListaDisponibilidadCombustible"); }
        }
        public string SqlGetEnvioArchivoRDO
        {
            get { return base.GetSqlXml("GetEnvioArchivoRDO"); }
        }
        #endregion

        #region PrimasRER.2023
        public string SqlListarBarrasPMPO
        {
            get { return base.GetSqlXml("ListarBarrasPMPO"); }
        }
        public string SqlListarCentralesPMPO
        {
            get { return base.GetSqlXml("ListarCentralesPMPO"); }
        }
        #endregion
    }
}
