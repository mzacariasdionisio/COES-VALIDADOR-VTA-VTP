using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_MEDICION48
    /// </summary>
    //[DataContract]
    //[Serializable]
    public class MeMedicion48DTO : EntityBase
    {
        [DataMember]
        public string Lastuser { get; set; }
        [DataMember]
        public DateTime? Lastdate { get; set; }
        [DataMember]
        public int Lectcodi { get; set; }
        [DataMember]
        public DateTime Medifecha { get; set; }
        [DataMember]
        public int Tipoinfocodi { get; set; }
        [DataMember]
        public int Ptomedicodi { get; set; }
        [DataMember]
        public decimal? H1 { get; set; }
        [DataMember]
        public decimal? Meditotal { get; set; }
        [DataMember]
        public string Mediestado { get; set; }
        [DataMember]
        public int Equipadre { get; set; }
        [DataMember]
        public string Central { get; set; }
        [DataMember]
        public int Equicodi { get; set; }
        [DataMember]
        public string Equinomb { get; set; }
        [DataMember]
        public string Equiabrev { get; set; }
        [DataMember]
        public int Emprcodi { get; set; }
        [DataMember]
        public string Emprnomb { get; set; }
        [DataMember]
        public string Emprabrev { get; set; }
        [DataMember]
        public int Fenergcodi { get; set; }
        [DataMember]
        public int Grupocodi { get; set; }
        [DataMember]
        public string Fenergnomb { get; set; }
        [DataMember]
        public string Gruponomb { get; set; }
        [DataMember]
        public string Fenergabrev { get; set; }
        [DataMember]
        public int Tgenercodi { get; set; }
        [DataMember]
        public string Tgenernomb { get; set; }
        [DataMember]
        public string Anio { get; set; }
        [DataMember]
        public string Mes { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Ptomedinomb { get; set; }
        [DataMember]
        public string Ptomedidesc { get; set; }
        [DataMember]
        public string Tipoinfodesc { get; set; }
        [DataMember]
        public decimal? Equitension { get; set; }
        [DataMember]
        public int Areacodi { get; set; }
        [DataMember]
        public string Areanomb { get; set; }
        [DataMember]
        public decimal? H2 { get; set; }
        [DataMember]
        public decimal? H3 { get; set; }
        [DataMember]
        public decimal? H4 { get; set; }
        [DataMember]
        public decimal? H5 { get; set; }
        [DataMember]
        public decimal? H6 { get; set; }
        [DataMember]
        public decimal? H7 { get; set; }
        [DataMember]
        public decimal? H8 { get; set; }
        [DataMember]
        public decimal? H9 { get; set; }
        [DataMember]
        public decimal? H10 { get; set; }
        [DataMember]
        public decimal? H11 { get; set; }
        [DataMember]
        public decimal? H12 { get; set; }
        [DataMember]
        public decimal? H13 { get; set; }
        [DataMember]
        public decimal? H14 { get; set; }
        [DataMember]
        public decimal? H15 { get; set; }
        [DataMember]
        public decimal? H16 { get; set; }
        [DataMember]
        public decimal? H17 { get; set; }
        [DataMember]
        public decimal? H18 { get; set; }
        [DataMember]
        public decimal? H19 { get; set; }
        [DataMember]
        public decimal? H20 { get; set; }
        [DataMember]
        public decimal? H21 { get; set; }
        [DataMember]
        public decimal? H22 { get; set; }
        [DataMember]
        public decimal? H23 { get; set; }
        [DataMember]
        public decimal? H24 { get; set; }
        [DataMember]
        public decimal? H25 { get; set; }
        [DataMember]
        public decimal? H26 { get; set; }
        [DataMember]
        public decimal? H27 { get; set; }
        [DataMember]
        public decimal? H28 { get; set; }
        [DataMember]
        public decimal? H29 { get; set; }
        [DataMember]
        public decimal? H30 { get; set; }
        [DataMember]
        public decimal? H31 { get; set; }
        [DataMember]
        public decimal? H32 { get; set; }
        [DataMember]
        public decimal? H33 { get; set; }
        [DataMember]
        public decimal? H34 { get; set; }
        [DataMember]
        public decimal? H35 { get; set; }
        [DataMember]
        public decimal? H36 { get; set; }
        [DataMember]
        public decimal? H37 { get; set; }
        [DataMember]
        public decimal? H38 { get; set; }
        [DataMember]
        public decimal? H39 { get; set; }
        [DataMember]
        public decimal? H40 { get; set; }
        [DataMember]
        public decimal? H41 { get; set; }
        [DataMember]
        public decimal? H42 { get; set; }
        [DataMember]
        public decimal? H43 { get; set; }
        [DataMember]
        public decimal? H44 { get; set; }
        [DataMember]
        public decimal? H45 { get; set; }
        [DataMember]
        public decimal? H46 { get; set; }
        [DataMember]
        public decimal? H47 { get; set; }
        [DataMember]
        public decimal? H48 { get; set; }
        [DataMember]
        public string Tipoinfoabrev { get; set; }
        [DataMember]
        public int Tipoptomedicodi { get; set; }
        [DataMember]
        public string Tipoptomedinomb { get; set; }
        [DataMember]
        public int Hojacodi { get; set; }

        /// <summary>
        /// Campos aplicativo PR16
        /// </summary>
        [DataMember]
        public int Item { get; set; }
        [DataMember]
        public string Periodo { get; set; }
        [DataMember]
        public string Fuente { get; set; }
        [DataMember]
        public DateTime FechaFila { get; set; }
        [DataMember]
        public string Cumplimiento { get; set; }
        [DataMember]
        public string CodigoCliente { get; set; }
        [DataMember]
        public string Suministrador { get; set; }
        [DataMember]
        public string RucEmpresa { get; set; }
        [DataMember]
        public string NombreEmpresa { get; set; }
        [DataMember]
        public string Subestacion { get; set; }
        [DataMember]
        public string Tension { get; set; }
        [DataMember]
        public int NroEnvios { get; set; }
        [DataMember]
        public DateTime? FechaPrimerEnvio { get; set; }
        [DataMember]
        public DateTime? FechaUltimoEnvio { get; set; }
        [DataMember]
        public DateTime IniRemision { get; set; }
        [DataMember]
        public DateTime FinRemision { get; set; }
        [DataMember]
        public DateTime IniPeriodo { get; set; }
        [DataMember]
        public string Grupotipo { get; set; }
        [DataMember]
        public int Grupourspadre { get; set; }

        /// <summary>
        /// Campos agregados para RFNE
        /// </summary>      
        [DataMember]
        public string Lectnomb { get; set; }
        [DataMember]
        public string Grupoabrev { get; set; }
        [DataMember]
        public int Canalcodi { get; set; }


        #region PR5
        public string Famnomb { get; set; }
        public int Famcodi { get; set; }
        public List<MeMedicion48DTO> ListaCentral = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRecursos = new List<MeMedicion48DTO>();
        public string Tipogenerrer { get; set; }
        public string Grupotipocogen { get; set; }
        public string Grupointegrante { get; set; }
        public int Tipogrupocodi { get; set; }
        public string Fenercolor { get; set; }
        public string AreaOperativa { get; set; }
        public int Subestacioncodi { get; set; }
        public decimal? Minimo { get; set; }
        public decimal? Maximo { get; set; }
        public decimal? Promedio { get; set; }
        public string SubareaOperativa { get; set; }
        public int Orden { get; set; }
        public decimal Factor { get; set; }
        public int TipoReservaFria { get; set; }
        public decimal? SincronizacionMin { get; set; }
        public string SincronizacionTiempo { get; set; }
        public DateTime Hophorini { get; set; }
        public DateTime Hophorfin { get; set; }
        public int? Repptoorden { get; set; }
        public int Areaoperativacodi { get; set; }
        public int TipoResultadoFecha { get; set; }
        public int? Reporcodi { get; set; }
        public int Ptomedicodi1 { get; set; }
        public string Tgenercolor { get; set; }
        public int Grupopadre { get; set; }
        public string Grupocentral { get; set; }
        public int TipoMediTotal { get; set; }
        public string Grupocomb { get; set; }
        public int OrdenArea { get; set; }

        public DateTime FechaMD { get; set; }
        public decimal Exp { get; set; }
        public decimal Imp { get; set; }

        public string NombreSerie { get; set; }
        public string ColorSerie { get; set; }

        #endregion

        #region SIOSEIN
        //Inicio - SIOSEIN
        public decimal H1Hidro { get; set; }
        public decimal H1Termo { get; set; }
        public decimal H1Solar { get; set; }
        public decimal H1Eolica { get; set; }
        public int Ctgdetcodi { get; set; }
        public string Ctgdetnomb { get; set; }
        public int MesdelAnio { get; set; }
        public string PropiedadGas { get; set; }
        public decimal? MaxDemanda { get; set; }
        public decimal? PorcentParticipacion { get; set; }
        public string TiempoMaximaDemanda { get; set; }
        public string Osinergcodi { get; set; }
        public string Grupomiembro { get; set; }
        public int CodCentral { get; set; }
        public decimal ValorTermoelectrico { get; set; }
        public decimal Total { get; set; }
        public decimal Participacion { get; set; }
        public decimal ValorSolar { get; set; }
        public decimal ValorEolico { get; set; }
        public decimal ValorHidroelectrico { get; set; }
        public decimal ValorRenovable { get; set; }
        public decimal TotalPeriodoAnterior { get; set; }
        public decimal TotalPeriodo { get; set; }
        public double TotalPorcentaje { get; set; }
        public decimal ValorEnerActivTxt { get; set; }
        public decimal ValorEnerActivCoes { get; set; }
        public string Fenergosinergcodi { get; set; }
        public decimal Agua { get; set; }
        public decimal Bagazo { get; set; }
        public decimal Biogas { get; set; }
        public decimal Carbon { get; set; }
        public decimal DieselB5 { get; set; }
        public decimal Eolica { get; set; }
        public decimal Gas { get; set; }
        public decimal Residual { get; set; }
        public decimal ResidualR500 { get; set; }
        public decimal ResidualR6 { get; set; }
        public decimal Solar { get; set; }
        public decimal ReservaFria { get; set; }
        public decimal Emergencia { get; set; }
        public decimal NodoEnergetico { get; set; }

        public List<MeMedicion48DTO> ListaCostOper = new List<MeMedicion48DTO>();
        public List<CostoComponentes> ListaCostoComp = new List<CostoComponentes>();
        public int Barrcodi { get; set; }
        public string Barrnombre { get; set; }
        public string Barrtension { get; set; }

        public int? Emprorden { get; set; }
        public int? Grupoorden { get; set; }

        public decimal CostoNoCombustible { get; set; }
        public decimal CostoConsumoCombustible { get; set; }
        public decimal CostoCombustibleBajaEficiencia { get; set; }
        public decimal CostoArranque { get; set; }
        public decimal CostoCalculado { get; set; }
        public decimal CostoOperacion { get; set; }
        public int NroArranque { get; set; }
        public decimal ConsumoCombustible { get; set; }
        public int NroSemana { get; set; }
        //Movisoft 01-04-2022
        public decimal Tipocambio { get; set; }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Semana { get; set; }
        public List<MeMedicion48DTO> ListaMedicion48 { get; set; }
        public decimal Cvnc { get; set; }
        public decimal Cvc { get; set; }
        public decimal Ccbef { get; set; }
        public decimal CarrUS { get; set; }
        public int Narranques { get; set; }
        #endregion

        #region MonitoreoMME
        public int TipoFormulaMonitoreo { get; set; }
        public int Catecodi { get; set; }
        public bool TieneIndicador { get; set; }
        public int Cnfbarcodi { get; set; }
        public string Cnfbarnombre { get; set; }
        #endregion

        #region Numerales Datos Base
        public string Osicodi { get; set; }
        public decimal Valor { get; set; }
        public string Dia { get; set; }
        #endregion

        #region YupanaContinuo
        public int Recurcodi { get; set; }
        public int Recptok { get; set; }
        #endregion

        public string MensajeValidacion { get; set; }
        public string MedifechaPto { get; set; }

        #region Mejoras RDO
        public string E1 { get; set; }
        public string E2 { get; set; }
        public string E3 { get; set; }
        public string E4 { get; set; }
        public string E5 { get; set; }
        public string E6 { get; set; }
        public string E7 { get; set; }
        public string E8 { get; set; }
        public string E9 { get; set; }
        public string E10 { get; set; }
        public string E11 { get; set; }
        public string E12 { get; set; }
        public string E13 { get; set; }
        public string E14 { get; set; }
        public string E15 { get; set; }
        public string E16 { get; set; }
        public string E17 { get; set; }
        public string E18 { get; set; }
        public string E19 { get; set; }
        public string E20 { get; set; }
        public string E21 { get; set; }
        public string E22 { get; set; }
        public string E23 { get; set; }
        public string E24 { get; set; }
        public string E25 { get; set; }
        public string E26 { get; set; }
        public string E27 { get; set; }
        public string E28 { get; set; }
        public string E29 { get; set; }
        public string E30 { get; set; }
        public string E31 { get; set; }
        public string E32 { get; set; }
        public string E33 { get; set; }
        public string E34 { get; set; }
        public string E35 { get; set; }
        public string E36 { get; set; }
        public string E37 { get; set; }
        public string E38 { get; set; }
        public string E39 { get; set; }
        public string E40 { get; set; }
        public string E41 { get; set; }
        public string E42 { get; set; }
        public string E43 { get; set; }
        public string E44 { get; set; }
        public string E45 { get; set; }
        public string E46 { get; set; }
        public string E47 { get; set; }
        public string E48 { get; set; }
        public int Enviocodi { get; set; }
        #endregion

        #region Informes-SGI

        [DataMember]
        public int? T1 { get; set; }

        [DataMember]
        public int? T2 { get; set; }
        [DataMember]
        public int? T3 { get; set; }
        [DataMember]
        public int? T4 { get; set; }
        [DataMember]
        public int? T5 { get; set; }
        [DataMember]
        public int? T6 { get; set; }
        [DataMember]
        public int? T7 { get; set; }
        [DataMember]
        public int? T8 { get; set; }
        [DataMember]
        public int? T9 { get; set; }
        [DataMember]
        public int? T10 { get; set; }
        [DataMember]
        public int? T11 { get; set; }
        [DataMember]
        public int? T12 { get; set; }
        [DataMember]
        public int? T13 { get; set; }
        [DataMember]
        public int? T14 { get; set; }
        [DataMember]
        public int? T15 { get; set; }
        [DataMember]
        public int? T16 { get; set; }
        [DataMember]
        public int? T17 { get; set; }
        [DataMember]
        public int? T18 { get; set; }
        [DataMember]
        public int? T19 { get; set; }
        [DataMember]
        public int? T20 { get; set; }
        [DataMember]
        public int? T21 { get; set; }
        [DataMember]
        public int? T22 { get; set; }
        [DataMember]
        public int? T23 { get; set; }
        [DataMember]
        public int? T24 { get; set; }
        [DataMember]
        public int? T25 { get; set; }
        [DataMember]
        public int? T26 { get; set; }
        [DataMember]
        public int? T27 { get; set; }
        [DataMember]
        public int? T28 { get; set; }
        [DataMember]
        public int? T29 { get; set; }
        [DataMember]
        public int? T30 { get; set; }
        [DataMember]
        public int? T31 { get; set; }
        [DataMember]
        public int? T32 { get; set; }
        [DataMember]
        public int? T33 { get; set; }
        [DataMember]
        public int? T34 { get; set; }
        [DataMember]
        public int? T35 { get; set; }
        [DataMember]
        public int? T36 { get; set; }
        [DataMember]
        public int? T37 { get; set; }
        [DataMember]
        public int? T38 { get; set; }
        [DataMember]
        public int? T39 { get; set; }
        [DataMember]
        public int? T40 { get; set; }
        [DataMember]
        public int? T41 { get; set; }
        [DataMember]
        public int? T42 { get; set; }
        [DataMember]
        public int? T43 { get; set; }
        [DataMember]
        public int? T44 { get; set; }
        [DataMember]
        public int? T45 { get; set; }
        [DataMember]
        public int? T46 { get; set; }
        [DataMember]
        public int? T47 { get; set; }
        [DataMember]
        public int? T48 { get; set; }

        #endregion
    }

    /// <summary>
    /// Clase para carga de datos servicio web
    /// </summary>
    public class Medicion48
    {
        public DateTime Medifecha { get; set; }
        public int Lectcodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public int PtoMedicion { get; set; }
        public string MedifechaStr { get; set; }
        public int Ptomedicodi { get; set; }
        public decimal H1 { get; set; }
        public decimal H2 { get; set; }
        public decimal H3 { get; set; }
        public decimal H4 { get; set; }
        public decimal H5 { get; set; }
        public decimal H6 { get; set; }
        public decimal H7 { get; set; }
        public decimal H8 { get; set; }
        public decimal H9 { get; set; }
        public decimal H10 { get; set; }
        public decimal H11 { get; set; }
        public decimal H12 { get; set; }
        public decimal H13 { get; set; }
        public decimal H14 { get; set; }
        public decimal H15 { get; set; }
        public decimal H16 { get; set; }
        public decimal H17 { get; set; }
        public decimal H18 { get; set; }
        public decimal H19 { get; set; }
        public decimal H20 { get; set; }
        public decimal H21 { get; set; }
        public decimal H22 { get; set; }
        public decimal H23 { get; set; }
        public decimal H24 { get; set; }
        public decimal H25 { get; set; }
        public decimal H26 { get; set; }
        public decimal H27 { get; set; }
        public decimal H28 { get; set; }
        public decimal H29 { get; set; }
        public decimal H30 { get; set; }
        public decimal H31 { get; set; }
        public decimal H32 { get; set; }
        public decimal H33 { get; set; }
        public decimal H34 { get; set; }
        public decimal H35 { get; set; }
        public decimal H36 { get; set; }
        public decimal H37 { get; set; }
        public decimal H38 { get; set; }
        public decimal H39 { get; set; }
        public decimal H40 { get; set; }
        public decimal H41 { get; set; }
        public decimal H42 { get; set; }
        public decimal H43 { get; set; }
        public decimal H44 { get; set; }
        public decimal H45 { get; set; }
        public decimal H46 { get; set; }
        public decimal H47 { get; set; }
        public decimal H48 { get; set; }


    }

    public class ReporteCMGRealDTO
    {
        public DateTime Fecha { get; set; }
        public decimal? Sur { get; set; }
        public decimal? Centro { get; set; }
        public decimal? Norte { get; set; }         
    }

    #region SIOSEIN
    [Serializable]
    public class CostoComponentes
    {
        public int? GrupoCodi { get; set; }
        public DateTime Fecha { get; set; }
        public decimal CostoNoCombustible { get; set; }
        public decimal ConsumoComb { get; set; }
        public decimal CostoBajaEficiencia { get; set; }
        public decimal CostoArranque { get; set; }
    }

    #endregion
}
