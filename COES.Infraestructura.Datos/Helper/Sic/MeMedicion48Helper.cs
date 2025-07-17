using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Linq.Expressions;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_MEDICION48
    /// </summary>
    public class MeMedicion48Helper : HelperBase
    {
        public MeMedicion48Helper(): base(Consultas.MeMedicion48Sql)
        {
        }

        public MeMedicion48DTO Create(IDataReader dr)
        {
            MeMedicion48DTO entity = new MeMedicion48DTO();

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iMeditotal = dr.GetOrdinal(this.Meditotal);
            if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

            int iMediestado = dr.GetOrdinal(this.Mediestado);
            if (!dr.IsDBNull(iMediestado)) entity.Mediestado = dr.GetString(iMediestado);

            this.GetH1To48(dr, entity);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }

        #region PR5
        public void GetH1To48(IDataReader dr, MeMedicion48DTO entity)
        {
            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iH25 = dr.GetOrdinal(this.H25);
            if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

            int iH26 = dr.GetOrdinal(this.H26);
            if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

            int iH27 = dr.GetOrdinal(this.H27);
            if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

            int iH28 = dr.GetOrdinal(this.H28);
            if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

            int iH29 = dr.GetOrdinal(this.H29);
            if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

            int iH30 = dr.GetOrdinal(this.H30);
            if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

            int iH31 = dr.GetOrdinal(this.H31);
            if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

            int iH32 = dr.GetOrdinal(this.H32);
            if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

            int iH33 = dr.GetOrdinal(this.H33);
            if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

            int iH34 = dr.GetOrdinal(this.H34);
            if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

            int iH35 = dr.GetOrdinal(this.H35);
            if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

            int iH36 = dr.GetOrdinal(this.H36);
            if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

            int iH37 = dr.GetOrdinal(this.H37);
            if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

            int iH38 = dr.GetOrdinal(this.H38);
            if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

            int iH39 = dr.GetOrdinal(this.H39);
            if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

            int iH40 = dr.GetOrdinal(this.H40);
            if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

            int iH41 = dr.GetOrdinal(this.H41);
            if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

            int iH42 = dr.GetOrdinal(this.H42);
            if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

            int iH43 = dr.GetOrdinal(this.H43);
            if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

            int iH44 = dr.GetOrdinal(this.H44);
            if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

            int iH45 = dr.GetOrdinal(this.H45);
            if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

            int iH46 = dr.GetOrdinal(this.H46);
            if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

            int iH47 = dr.GetOrdinal(this.H47);
            if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

            int iH48 = dr.GetOrdinal(this.H48);
            if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);
        }

        #endregion


        #region Mapeo de Campos

        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Lectcodi = "LECTCODI";
        public string Medifecha = "MEDIFECHA";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Meditotal = "MEDITOTAL";
        public string Mediestado = "MEDIESTADO";
        public string Equipadre = "EQUIPADRE";
        public string Central = "CENTRAL";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Equiabrev = "EQUIABREV";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string Emprorden = "EMPRORDEN";
        public string Fenergcodi = "FENERGCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Fenergnomb = "FENERGNOMB";
        public string Gruponomb = "GRUPONOMB";
        public string Grupoorden = "GRUPOORDEN";
        public string Fenergabrev = "FENERGABREV";
        public string Tipogrupocodi = "TIPOGRUPOCODI";
        public string Tgenercodi = "TGENERCODI";
        public string Tgenernomb = "TGENERNOMB";
        public string Tgenercolor = "TGENERCOLOR";
        public string Descripcion = "DESCRIPCION";
        public string Ptomedinomb = "PTOMEDINOMB";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Tipoinfodesc = "TIPOINFODESC";
        public string Tipoinfoabrev = "TIPOINFOABREV";
        public string Tipoptomedinomb = "TPTOMEDINOMB";
        public string Equitension = "EQUITENSION";
        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Anio = "YEAR";
        public string Mes = "MONTH";
        public string H1 = "H1";
        public string H2 = "H2";
        public string H3 = "H3";
        public string H4 = "H4";
        public string H5 = "H5";
        public string H6 = "H6";
        public string H7 = "H7";
        public string H8 = "H8";
        public string H9 = "H9";
        public string H10 = "H10";
        public string H11 = "H11";
        public string H12 = "H12";
        public string H13 = "H13";
        public string H14 = "H14";
        public string H15 = "H15";
        public string H16 = "H16";
        public string H17 = "H17";
        public string H18 = "H18";
        public string H19 = "H19";
        public string H20 = "H20";
        public string H21 = "H21";
        public string H22 = "H22";
        public string H23 = "H23";
        public string H24 = "H24";
        public string H25 = "H25";
        public string H26 = "H26";
        public string H27 = "H27";
        public string H28 = "H28";
        public string H29 = "H29";
        public string H30 = "H30";
        public string H31 = "H31";
        public string H32 = "H32";
        public string H33 = "H33";
        public string H34 = "H34";
        public string H35 = "H35";
        public string H36 = "H36";
        public string H37 = "H37";
        public string H38 = "H38";
        public string H39 = "H39";
        public string H40 = "H40";
        public string H41 = "H41";
        public string H42 = "H42";
        public string H43 = "H43";
        public string H44 = "H44";
        public string H45 = "H45";
        public string H46 = "H46";
        public string H47 = "H47";
        public string H48 = "H48";

        /// <summary>
        /// Campos aplicativo PR16
        /// </summary>
        public string Item = "ITEM";
        public string Periodo = "PERIODO";
        public string Fuente = "FUENTE";
        public string FechaFila = "FECHA_FILA";
        public string Cumplimiento = "CUMPLIMIENTO";
        public string CodigoCliente = "CODIGO_CLIENTE";
        public string Suministrador = "SUMINISTRADOR";
        public string RucEmpresa = "RUC_EMPRESA";
        public string NombreEmpresa = "NOMBRE_EMPRESA";
        public string Subestacion = "SUBESTACION";
        public string Tension = "TENSION";
        public string NroEnvios = "NRO_ENVIOS";
        public string FechaPrimerEnvio = "FECHA_PRIMER_ENVIO";
        public string FechaUltimoEnvio = "FECHA_ULTIMO_ENVIO";
        public string IniRemision = "INI_REMISION";
        public string FinRemision = "FIN_REMISION";
        public string IniPeriodo = "INI_PERIODO";
        public string Qregistros = "Q_REGISTROS";
        public string Grupotipo = "GRUPOTIPO";
        public string Grupourspadre = "GRUPOURSPADRE";
        public string Lectnomb = "LECTNOMB";

        /// Campos Yupana Continuo
        public string Recurcodi = "RECURCODI";
        public string Recptok = "RECPTOK";

        #region PR5
        /// <summary>
        /// Campos aplicativo PR5 2DA ETAPA
        /// </summary>
        public string Famnomb = "FAMNOMB";
        public string Famcodi = "FAMCODI";
        public string Fenercolor = "FENERCOLOR";
        public string AreaOperativa = "AREAOPERATIVA";
        public string Subestacioncodi = "SUBESTACIONCODI";
        public string Emprcoes = "EMPRCOES";
        public string OrdenCalculado = "ORDEN_CALCULADO";
        public string PtomediCalculado = "PTOMEDICODI_CALCULADO";
        public string PtomedidescCalculado = "DESC_CALCULADO";
        public string Factor = "FACTOR";
        public string Areaoperativacodi = "AREAOPERATIVACODI";
        public string Areaoperativa = "AREAOPERATIVA";
        public string Reporcodi = "REPORCODI";
        public string Hojacodi = "HOJACODI";
        public string Tipoptomedicodi = "TPTOMEDICODI";
        #endregion

        #region SIOSEIN
        /// <summary>
        /// SIOSEIN
        /// </summary>
        public string Osinergcodi = "OSINERGCODI";
        public string Ctgdetcodi = "CTGDETCODI";
        public string Ctgdetnomb = "CTGDETNOMB";
        public string MesdelAnio = "MES";
        public string Grupomiembro = "GRUPOMIEMBRO";
        public string CodCentral = "CODCENTRAL";
        public string Tipogenerrer = "TIPOGENERRER";
        public string Grupointegrante = "Grupointegrante";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrtension = "BARRTENSION";
        public string Barrcodi = "BARRCODI";
        //Movisoft 01-04-2022
        public string Tipocambio = "TCAMBIO";

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string TableName = "ME_MEDICION48";
        public string MaxDemanda = "MaxDemanda";
        #endregion

        #region MonitoreoMME
        public string Grupopadre = "Grupopadre";
        public string Grupoabrev = "Grupoabrev";
        #endregion

        #region Mejoras RDO
        public string Enviocodi = "ENVIOCODI";
        public string E1 = "E1";
        public string E2 = "E2";
        public string E3 = "E3";
        public string E4 = "E4";
        public string E5 = "E5";
        public string E6 = "E6";
        public string E7 = "E7";
        public string E8 = "E8";
        public string E9 = "E9";
        public string E10 = "E10";
        public string E11 = "E11";
        public string E12 = "E12";
        public string E13 = "E13";
        public string E14 = "E14";
        public string E15 = "E15";
        public string E16 = "E16";
        public string E17 = "E17";
        public string E18 = "E18";
        public string E19 = "E19";
        public string E20 = "E20";
        public string E21 = "E21";
        public string E22 = "E22";
        public string E23 = "E23";
        public string E24 = "E24";
        public string E25 = "E25";
        public string E26 = "E26";
        public string E27 = "E27";
        public string E28 = "E28";
        public string E29 = "E29";
        public string E30 = "E30";
        public string E31 = "E31";
        public string E32 = "E32";
        public string E33 = "E33";
        public string E34 = "E34";
        public string E35 = "E35";
        public string E36 = "E36";
        public string E37 = "E37";
        public string E38 = "E38";
        public string E39 = "E39";
        public string E40 = "E40";
        public string E41 = "E41";
        public string E42 = "E42";
        public string E43 = "E43";
        public string E44 = "E44";
        public string E45 = "E45";
        public string E46 = "E46";
        public string E47 = "E47";
        public string E48 = "E48";
        #endregion

        #region Informes SGI


        #endregion

        public string SqlGetGeneracionRER
        {
            get { return base.GetSqlXml("GetGeneracionRER"); }
        }

        public string SqlDeleteGeneracionRERPorPunto
        {
            get { return base.GetSqlXml("DeleteGeneracionRERPorPunto"); }
        }

        public string SqlObtenerGeneracionRERPorPunto
        {
            get { return base.GetSqlXml("ObtenerGeneracionRERPorPunto"); }
        }

        public string SqlObtenerEmpresaPtoMedicion
        {
            get { return base.GetSqlXml("ObtenerEmpresaPtoMedicion"); }
        }

        public string SqlObtenerNroRegistrosEjecutado
        {
            get { return base.GetSqlXml("ObtenerNroRegistrosEjecutado"); }
        }

        public string SqlObtenerConsultaEjecutado
        {
            get { return base.GetSqlXml("ObtenerConsultaEjecutado"); }
        }

        public string SqlObtenerTotalConsultaEjecutado
        {
            get { return base.GetSqlXml("ObtenerTotalConsultaEjecutado"); }
        }

        public string SqlObtenerExportacionConsultaEjecutado
        {
            get { return base.GetSqlXml("ObtenerExportacionConsultaEjecutado"); }
        }

        public string SqlObtenerEjecutadoConsolidado
        {
            get { return base.GetSqlXml("ObtenerEjecutadoConsolidado"); }
        }

        public string SqlObtenerCmgRealPorArea
        {
            get { return base.GetSqlXml("ObtenerCmgRealPorArea"); }
        }

        public string SqlDeleteEnvioArchivo
        {
            get { return base.GetSqlXml("DeleteEnvioArchivo"); }
        }

        public string SqlGetEnvioArchivo
        {
            get { return base.GetSqlXml("GetEnvioArchivo"); }
        }

        public string SqlGetEnvioArchivo2
        {
            get { return base.GetSqlXml("GetEnvioArchivo2"); }
        }

        public string SqlGetByPtoMedicion
        {
            get { return base.GetSqlXml("GetByPtoMedicion"); }
        }

        public string SqlGetInterconexiones
        {
            get { return base.GetSqlXml("GetInterconexiones"); }
        }

        public string SqlObtenerGeneracionPorEmpresa
        {
            get { return base.GetSqlXml("ObtenerGeneracionPorEmpresa"); }
        }

        public string SqlObtenerGeneracionPorEmpresaTipoGeneracion
        {
            get { return base.GetSqlXml("ObtenerGeneracionPorEmpresaTipoGeneracion"); }
        }

        public string SqlObtenerGeneracionPorEmpresaTipoGeneracionMovil
        {
            get { return base.GetSqlXml("ObtenerGeneracionPorEmpresaTipoGeneracionMovil"); }
        }

        public string SqlObtenerGeneracionAcumuladaAnual
        {
            get { return base.GetSqlXml("ObtenerGeneracionAcumuladaAnual"); }
        }

        public string SqlDemandaDespachoOfertaNCP
        {
            get { return base.GetSqlXml("DemandaDespachoOfertaNCP"); }
        }

        public string SqlObtenerDemandaPortalWebTipo
        {
            get { return base.GetSqlXml("ObtenerDemandaPortalWebTipo"); }
        }

        public string SqlObtenerDemandaPicoDiaAnterior
        {
            get { return base.GetSqlXml("ObtenerDemandaPicoDiaAnterior"); }
        }

        public string SqlObtenerConsultaDemandaBarras
        {
            get { return base.GetSqlXml("ObtenerConsultaDemandaBarras"); }
        }

        public string SqlObtenerDatosHidrologiaPortal
        {
            get { return base.GetSqlXml("ObtenerDatosHidrologiaPortal"); }
        }

        public string SqlObtenerConsultaWebReporte
        {
            get { return base.GetSqlXml("ObtenerConsultaWebReporte"); }
        }

        public string SqlObtenerDatosEjecutado
        {
            get { return base.GetSqlXml("ObtenerDatosEjecutado"); }
        }

        public string SqlListReporteInformacion
        {
            get { return base.GetSqlXml("ListReporteInformacion30min"); }
        }

        public string SqlListReporteInformacionCount
        {
            get { return base.GetSqlXml("ListReporteInformacion30minCount"); }
        }

        public string SqlObtenerDatosEquipoLectura
        {
            get { return base.GetSqlXml("ObtenerDatosEquipoLectura"); }
        }

        public string SqlObtenerDatosPtoMedicionLectura
        {
            get { return base.GetSqlXml("ObtenerDatosPtoMedicionLectura"); }
        }

        public string SqlObtenerDatosPtoMedicionLecturaInfo
        {
            get { return base.GetSqlXml("ObtenerDatosPtoMedicionLecturaInfo"); }
        }

        //inicio modificado
        public string SqlObtenerMedicion48
        {
            get { return base.GetSqlXml("ObtenerMedicion48"); }
        }

        //fin modificado
        public string SqlGetDespachoProgramado
        {
            get { return base.GetSqlXml("GetDespachoProgramado"); }
        }

        #region "COSTO OPORTUNIDAD"
        public string SqlGetReservaProgramado
        {
            get { return base.GetSqlXml("GetReservaProgramado"); }
        }

        public string SqlGetListadoReserva
        {
            get { return base.GetSqlXml("GetListadoReserva"); }
        }
        #endregion

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        #region PR5

        public string SqlObtenerTotalMedicion48ByPtomedicion
        {
            get { return base.GetSqlXml("ObtenerTotalMedicion48ByPtomedicion"); }
        }

        public string SqlListarMeMedicion48ByFiltro
        {
            get { return base.GetSqlXml("ListarMeMedicion48ByFiltro"); }
        }

        public string SqlListarDetalleGeneracion48
        {
            get { return base.GetSqlXml("ListarDetalleGeneracion48"); }
        }

        public string SqlGetConsolidadoMaximaDemanda
        {
            get { return base.GetSqlXml("GetConsolidadoMaximaDemanda"); }
        }

        public string SqlGetDemandaCOESPtoMedicion48
        {
            get { return base.GetSqlXml("GetDemandaCOESPtoMedicion48"); }
        }

        public string SqlGetConsolidadoMaximaDemandaSinGrupoIntegrante
        {
            get { return base.GetSqlXml("GetConsolidadoMaximaDemandaSinGrupoIntegrante"); }
        }
        public string SqlGetDemandaEjecutadaConEcuador
        {
            get { return base.GetSqlXml("GetDemandaEjecutadaConEcuador"); }
        }

        #endregion

        #region SIOSEIN

        public string SqlGetCostosMarginalesProgPorRangoFechaStaRosa
        {
            get { return base.GetSqlXml("GetCostosMarginalesProgPorRangoFechaStaRosa"); }
        }
       
        #endregion

        #region Indisponibilidades
        public string SqlIndisponibilidadesMedicion48Cuadro5
        {
            get { return base.GetSqlXml("GetIndisponibilidadesMedicion48Cuadro5"); }
        }
        public string SqlIndisponibilidadesMedicion48Cuadro2
        {
            get { return base.GetSqlXml("GetIndisponibilidadesMedicion48Cuadro2"); }
        }
        #endregion

        #region MigracionSGOCOES-GrupoB

        public string SqlGetListaMedicion48xlectcodi
        {
            get { return base.GetSqlXml("GetListaMedicion48xlectcodi"); }
        }

        public string SqlRptCmgCortoPlazo
        {
            get { return base.GetSqlXml("RptCmgCortoPlazo"); }
        }

        public string SqlDeleteMasivo
        {
            get { return base.GetSqlXml("DeleteMasivo"); }
        }

        public string SqlGetListaDemandaBarras
        {
            get { return base.GetSqlXml("GetListaDemandaBarras"); }
        }
        #endregion



        #region FIT - SGOCOES-GrupoA - Soporte

        public string SqlDeleteSCOActualizacion
        {
            get { return base.GetSqlXml("DeleteSCOActualizacion"); }
        }

        #endregion

        public string SqlObtenerProgramaReProgramaDia
        {
            get { return base.GetSqlXml("ObtenerProgramaReProgramaDia"); }
        }

        #region Fit - VALORIZACION DIARIA

        public string SqlGetEmpresaEnergiaEntregada
        {
            get { return base.GetSqlXml("GetEmpresaEnergiaEntregada"); }
        }

        public string SqlGetEnergiaEntregadabyEmpresas
        {
            get { return base.GetSqlXml("GetEnergiaEntregadabyEmpresas"); }
        }

        public string SqlGetIntervaloPuntaMes
        {
            get { return base.GetSqlXml("GetIntervaloPuntaMes"); }
        }

        #endregion

        public string SqlObtenerListaMedicion48Ptomedicion
        {
            get { return base.GetSqlXml("ObtenerListaMedicion48Ptomedicion"); }
        }

        #region Numerales Datos Base 


        public string Osicodi = "OSICODI";
        public string Dia = "DIA";


        public string SqlDatosBase_5_8_4
        {
            get { return base.GetSqlXml("ListaDatosBase_5_8_4"); }
        }
        public string SqlDatosBase_5_8_5
        {
            get { return base.GetSqlXml("ListaDatosBase_5_8_5"); }
        }
        public string SqlDatosBase_5_9_1
        {
            get { return base.GetSqlXml("ListaDatosBase_5_9_1"); }
        }
        public string SqlDatosBase_5_9_2
        {
            get { return base.GetSqlXml("ListaDatosBase_5_9_2"); }
        }
        public string SqlDatosBase_5_9_3
        {
            get { return base.GetSqlXml("ListaDatosBase_5_9_3"); }
        }
        public string SqlListaMedUsuariosLibres
        {
            get { return base.GetSqlXml("ListaMedUsuariosLibres"); }
        }
        public string SqlObtenerConsultaTipoGeneracion
        {
            get { return base.GetSqlXml("ObtenerConsultaPorTipoGeneracion"); }
        }
        public string SqlNroRegistrosConsultasTipoGeneracion
        {
            get { return base.GetSqlXml("NroRegistrosConsultasTipoGeneracion"); }
        }
        #endregion

        public string SqlObtenerDatosDespachoComparativo
        {
            get { return base.GetSqlXml("ObtenerDatosDespachoComparativo"); }
        }

        #region Yupana
        public string SqlObtenerAportesHidro
        {
            get { return base.GetSqlXml("ObtenerAportesHidro"); }
        }

        #endregion

        #region Mejoras RDO
        public string SqlGetEnvioArchivoEjecutados
        {
            get { return base.GetSqlXml("GetEnvioArchivoEjecutados"); }
        }
        public string SqlGetGeneracionRERNC
        {
            get { return base.GetSqlXml("GetGeneracionRERNC"); }
        }
        public MeMedicion48DTO CreateEjecutados(IDataReader dr)
        {
            MeMedicion48DTO entity = new MeMedicion48DTO();

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iMeditotal = dr.GetOrdinal(this.Meditotal);
            if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

            int iMediestado = dr.GetOrdinal(this.Mediestado);
            if (!dr.IsDBNull(iMediestado)) entity.Mediestado = dr.GetString(iMediestado);

            this.GetH1To48(dr, entity);

            int iE1 = dr.GetOrdinal(this.E1);
            if (!dr.IsDBNull(iE1)) entity.E1 = dr.GetString(iE1);

            int iE2 = dr.GetOrdinal(this.E2);
            if (!dr.IsDBNull(iE2)) entity.E2 = dr.GetString(iE2);

            int iE3 = dr.GetOrdinal(this.E3);
            if (!dr.IsDBNull(iE3)) entity.E3 = dr.GetString(iE3);

            int iE4 = dr.GetOrdinal(this.E4);
            if (!dr.IsDBNull(iE4)) entity.E4 = dr.GetString(iE4);

            int iE5 = dr.GetOrdinal(this.E5);
            if (!dr.IsDBNull(iE5)) entity.E5 = dr.GetString(iE5);

            int iE6 = dr.GetOrdinal(this.E6);
            if (!dr.IsDBNull(iE6)) entity.E6 = dr.GetString(iE6);

            int iE7 = dr.GetOrdinal(this.E7);
            if (!dr.IsDBNull(iE7)) entity.E7 = dr.GetString(iE7);

            int iE8 = dr.GetOrdinal(this.E8);
            if (!dr.IsDBNull(iE8)) entity.E8 = dr.GetString(iE8);

            int iE9 = dr.GetOrdinal(this.E9);
            if (!dr.IsDBNull(iE9)) entity.E9 = dr.GetString(iE9);

            int iE10 = dr.GetOrdinal(this.E10);
            if (!dr.IsDBNull(iE10)) entity.E10 = dr.GetString(iE10);

            int iE11 = dr.GetOrdinal(this.E11);
            if (!dr.IsDBNull(iE11)) entity.E11 = dr.GetString(iE11);

            int iE12 = dr.GetOrdinal(this.E12);
            if (!dr.IsDBNull(iE12)) entity.E12 = dr.GetString(iE12);

            int iE13 = dr.GetOrdinal(this.E13);
            if (!dr.IsDBNull(iE13)) entity.E13 = dr.GetString(iE13);

            int iE14 = dr.GetOrdinal(this.E14);
            if (!dr.IsDBNull(iE14)) entity.E14 = dr.GetString(iE14);

            int iE15 = dr.GetOrdinal(this.E15);
            if (!dr.IsDBNull(iE15)) entity.E15 = dr.GetString(iE15);

            int iE16 = dr.GetOrdinal(this.E16);
            if (!dr.IsDBNull(iE16)) entity.E16 = dr.GetString(iE16);

            int iE17 = dr.GetOrdinal(this.E17);
            if (!dr.IsDBNull(iE17)) entity.E17 = dr.GetString(iE17);

            int iE18 = dr.GetOrdinal(this.E18);
            if (!dr.IsDBNull(iE18)) entity.E18 = dr.GetString(iE18);

            int iE19 = dr.GetOrdinal(this.E19);
            if (!dr.IsDBNull(iE19)) entity.E19 = dr.GetString(iE19);

            int iE20 = dr.GetOrdinal(this.E20);
            if (!dr.IsDBNull(iE20)) entity.E20 = dr.GetString(iE20);

            int iE21 = dr.GetOrdinal(this.E21);
            if (!dr.IsDBNull(iE21)) entity.E21 = dr.GetString(iE21);

            int iE22 = dr.GetOrdinal(this.E22);
            if (!dr.IsDBNull(iE22)) entity.E22 = dr.GetString(iE22);

            int iE23 = dr.GetOrdinal(this.E23);
            if (!dr.IsDBNull(iE23)) entity.E23 = dr.GetString(iE23);

            int iE24 = dr.GetOrdinal(this.E24);
            if (!dr.IsDBNull(iE24)) entity.E24 = dr.GetString(iE24);

            int iE25 = dr.GetOrdinal(this.E25);
            if (!dr.IsDBNull(iE25)) entity.E25 = dr.GetString(iE25);

            int iE26 = dr.GetOrdinal(this.E26);
            if (!dr.IsDBNull(iE26)) entity.E26 = dr.GetString(iE26);

            int iE27 = dr.GetOrdinal(this.E27);
            if (!dr.IsDBNull(iE27)) entity.E27 = dr.GetString(iE27);

            int iE28 = dr.GetOrdinal(this.E28);
            if (!dr.IsDBNull(iE28)) entity.E28 = dr.GetString(iE28);

            int iE29 = dr.GetOrdinal(this.E29);
            if (!dr.IsDBNull(iE29)) entity.E29 = dr.GetString(iE29);

            int iE30 = dr.GetOrdinal(this.E30);
            if (!dr.IsDBNull(iE30)) entity.E30 = dr.GetString(iE30);

            int iE31 = dr.GetOrdinal(this.E31);
            if (!dr.IsDBNull(iE31)) entity.E31 = dr.GetString(iE31);

            int iE32 = dr.GetOrdinal(this.E32);
            if (!dr.IsDBNull(iE32)) entity.E32 = dr.GetString(iE32);

            int iE33 = dr.GetOrdinal(this.E33);
            if (!dr.IsDBNull(iE33)) entity.E33 = dr.GetString(iE33);

            int iE34 = dr.GetOrdinal(this.E34);
            if (!dr.IsDBNull(iE34)) entity.E34 = dr.GetString(iE34);

            int iE35 = dr.GetOrdinal(this.E35);
            if (!dr.IsDBNull(iE35)) entity.E35 = dr.GetString(iE35);

            int iE36 = dr.GetOrdinal(this.E36);
            if (!dr.IsDBNull(iE36)) entity.E36 = dr.GetString(iE36);

            int iE37 = dr.GetOrdinal(this.E37);
            if (!dr.IsDBNull(iE37)) entity.E37 = dr.GetString(iE37);

            int iE38 = dr.GetOrdinal(this.E38);
            if (!dr.IsDBNull(iE38)) entity.E38 = dr.GetString(iE38);

            int iE39 = dr.GetOrdinal(this.E39);
            if (!dr.IsDBNull(iE39)) entity.E39 = dr.GetString(iE39);

            int iE40 = dr.GetOrdinal(this.E40);
            if (!dr.IsDBNull(iE40)) entity.E40 = dr.GetString(iE40);

            int iE41 = dr.GetOrdinal(this.E41);
            if (!dr.IsDBNull(iE41)) entity.E41 = dr.GetString(iE41);

            int iE42 = dr.GetOrdinal(this.E42);
            if (!dr.IsDBNull(iE42)) entity.E42 = dr.GetString(iE42);

            int iE43 = dr.GetOrdinal(this.E43);
            if (!dr.IsDBNull(iE43)) entity.E43 = dr.GetString(iE43);

            int iE44 = dr.GetOrdinal(this.E44);
            if (!dr.IsDBNull(iE44)) entity.E44 = dr.GetString(iE44);

            int iE45 = dr.GetOrdinal(this.E45);
            if (!dr.IsDBNull(iE45)) entity.E45 = dr.GetString(iE45);

            int iE46 = dr.GetOrdinal(this.E46);
            if (!dr.IsDBNull(iE46)) entity.E46 = dr.GetString(iE46);

            int iE47 = dr.GetOrdinal(this.E47);
            if (!dr.IsDBNull(iE47)) entity.E47 = dr.GetString(iE47);

            int iE48 = dr.GetOrdinal(this.E48);
            if (!dr.IsDBNull(iE48)) entity.E48 = dr.GetString(iE48);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }
        public string SqlGetEnvioMeMedicion48Intranet
        {
            get { return base.GetSqlXml("GetEnvioMeMedicion48Intranet"); }
        }
        #endregion

        #region Mejoras RDO-II
        public string SqlGetEnvioArchivoUltimosEjecutados
        {
            get { return base.GetSqlXml("GetEnvioArchivoUltimosEjecutados"); }
        }
        public string SqlGetEnvioArchivoEjecutadosIntranet
        {
            get { return base.GetSqlXml("GetEnvioArchivoEjecutadosIntranet"); }
        }
        public string SqlGetEnvioArchivoUltimoEjecutado
        {
            get { return base.GetSqlXml("GetEnvioArchivoEjecutadosxEnviocodi"); }
        }
        #endregion

        #region Informes-SGI

        public string SqlObtenerDatosPorReporte
        {
            get { return base.GetSqlXml("ObtenerDatosPorReporte"); }
        }

        public string SqlSaveInfoAdicional
        {
            get { return base.GetSqlXml("SaveInfoAdicional"); }
        }

        #endregion

        #endregion

        #region Demanda PO
        public string SqlObtenerDemandaGeneracionPO
        {
            get { return base.GetSqlXml("ObtenerDemandaGeneracionPO"); }
        }

        public string SqlLeerMedidoresHidrologia
        {
            get { return base.GetSqlXml("LeerMedidoresHidrologia"); }
        }

        public string SqlDemandaProgramadaxAreas
        {
            get { return base.GetSqlXml("DemandaProgramadaxAreas"); }
        }
        public string SqlDemandaProgramadaDiariaCOES
        {
            get { return base.GetSqlXml("DemandaProgramadaDiariaCOES"); }
        }

        public string SqlDemandaDiariaxAreas
        {
            get { return base.GetSqlXml("DemandaDiariaxAreas"); }
        }

        #endregion
    }
}
