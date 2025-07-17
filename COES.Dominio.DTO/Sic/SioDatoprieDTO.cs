using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Linq.Expressions;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SIO_DATOPRIE
    /// </summary>
    public partial class SioDatoprieDTO : EntityBase, ICloneable
    {
        public int Dpriecodi { get; set; }
        public string Dprievalor { get; set; }
        public DateTime? Dprieperiodo { get; set; }
        public DateTime? Dpriefechadia { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Barrcodi { get; set; }
        public int? Emprcodi { get; set; }
        public int? Emprcodi2 { get; set; }
        public string Dprieusuario { get; set; }
        public DateTime? Dpriefecha { get; set; }
        public int? Cabpricodi { get; set; }
        public int Ptomedicodi { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Clase parcial que mapea propiedades adicionales a la tabla SIO_DATOPRIE
    /// </summary>
    public partial class SioDatoprieDTO
    {
        public SioReporteDTO SioReporte { get; set; }


        

        //prie03
        public int Areacodi { get; set; }
        public string Areanomb { get; set; }
        public string Barrtension { get; set; }
        public int Hora { get; set; }
        public decimal CMarPromHor { get; set; }
        public decimal DemHorProm { get; set; }
        //prie04
        public string Gruponomb { get; set; }
        public int Divisor { get; set; }
        public decimal? Cc { get; set; }
        public decimal? Cvc { get; set; }
        public decimal? Cvnc { get; set; }
        public decimal? Cv { get; set; }
        //prie07
        public decimal? Energentr { get; set; }
        public decimal? EnergRet { get; set; }
        public decimal? EntrGener { get; set; }
        public decimal? RetContBil { get; set; }
        public decimal? RetContLic { get; set; }
        public decimal? RetSinCont { get; set; }
        public string Emprambito { get; set; }
        //prie08
        public decimal? Potventa { get; set; }
        public decimal? Potcompra { get; set; }
        public string Emprambc { get; set; }
        //prie26
        public string Equinomb { get; set; }
        public string Emprnomb { get; set; }
        public string Fenergnomb { get; set; }
        public int Fenergcodi { get; set; }

        public int Tgenercodi { get; set; }
        public string Tgenernomb { get; set; }

        public int Orden { get; set; }

        #region SIOSEIN
        public decimal? Valor { get; set; }
        #endregion

        #region SIOSEIN-PRIE-2021

        //Tabla 05
        public DateTime? Medifecha { get; set; }
        public decimal? MaxDemanda { get; set; }
        public int Codcentral { get; set; } //ya existe CodigoCentral pero esta en SioReporteDTO
        public string Central { get; set; }
        public int Famcodi { get; set; }
        public decimal Total { get; set; }
        public int Tipogrupocodi { get; set; }
        public string Grupomiembro { get; set; }
        public decimal Valorrenovable { get; set; }
        public decimal Valorhidroelectrico { get; set; }
        public decimal Valortermoelectrico { get; set; }
        public string Ctgdetnomb { get; set; }
        public string Tipogenerrer { get; set; }

        //prie30
        public string Osinergcodi { get; set; }
        public string Grupnomb { get; set; }

        //tabla 6, 21 y 29
        public string Osicodi { get; set; }
        public int Tptomedicodi { get; set; }
        public string Tptomedinomb { get; set; }
        public string Ptomedielenomb { get; set; }
        public string Ptomedidesc { get; set; }
        public decimal Medinth1 { get; set; }
        public string Equiabrev { get; set; }
        #endregion
    }

    public class SioReporteDTO : EntityBase, ICloneable
    {
        public string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int LengthColumnasReporte { get; set; }
        public string DetalleNombreColumnas { get; set; }
        public string DataMemedicion96 { get; set; }
        public string DataMemedicion48 { get; set; }
        public string Periodo { get; set; }
        public string CodigoCentral { get; set; }
        public string CodigoModoOperacion { get; set; }
        public string CodigoTipoCombustible { get; set; }
        public string CodigoGrupo { get; set; }
        public string CodigoEmpresa { get; set; }
        public string CodigoBarra { get; set; }
        public string CodigoEntregaRetiro { get; set; }
        public string CodigoEmpresaEntrega { get; set; }
        public string CodigoEmpresaRecibe { get; set; }
        public string CodigoClasificacTransmision { get; set; }
        public string TipoOperacion { get; set; }
        public string SistemaElectrico { get; set; }
        public string LimTransm { get; set; }
        public string Observacion { get; set; }
        public string CodigoInterconexion { get; set; }
        public string TipoFlujo { get; set; }
        public string CodigoCuenca { get; set; }
        public string PaisIntercambio { get; set; }
        public string Tipo { get; set; }
        public string CodigoReservorioDiario { get; set; }
        public string CodigoLago { get; set; }
        public string CodigoEmbalse { get; set; }
        public string CodigoTipoEmpresa { get; set; }
        public string CodigoTipoIndisponibilidad { get; set; }
        public string Motivo { get; set; }
        public string Condicion { get; set; }
        public string Correlativo { get; set; }
        public string MesActual { get; set; }
        public string MesInicio { get; set; }
        public string MesFin { get; set; }
        public string MesProgramado { get; set; }
        public string CodigoBloqueHorario { get; set; }
        public string CodigoResultados { get; set; }
        public string Fecha { get; set; }
        public decimal Programada { get; set; }
        public decimal Real { get; set; }
        public decimal Valor { get; set; }
        public decimal RendimientoTermico { get; set; }
        public decimal CostoCombustible { get; set; }
        public decimal ValorCombustible { get; set; }
        public decimal ValorNoCombustible { get; set; }
        public decimal ValorCostoVariable { get; set; }
        public decimal PotenciaActiva { get; set; }
        public decimal PotenciaReactiva { get; set; }
        public decimal EnergiaActivaEntrega { get; set; }
        public decimal EnergiaActivaRetiro { get; set; }
        public decimal ValorTransferencia { get; set; }
        public decimal TransferenciasEnergia { get; set; }
        public decimal ProrrateoSaldoResultante { get; set; }
        public decimal RetirosSinContratosDist { get; set; }
        public decimal RetirosSinContratosULibres { get; set; }
        public decimal ValorBajaEficienciaCombustible { get; set; }
        public decimal ValorRegulacionFrecuencia { get; set; }
        public decimal ValorOperacionInflexibilidadOperativa { get; set; }
        public decimal ValorPorPruebasAleatorias { get; set; }
        public decimal SaldoMesesAnteriores { get; set; }
        public decimal OtrasCompensaciones { get; set; }
        public decimal RecaudacionTransmision { get; set; }
        public decimal RecaudacionGenerAdicional { get; set; }
        public decimal RecaudacionSegSumNRF { get; set; }
        public decimal RecaudacionSegSumReservaFria { get; set; }
        public decimal RecaudacionPrimaRER { get; set; }
        public decimal RecaudacionPrimaFise { get; set; }
        public decimal RecaudacionPrimaCase { get; set; }
        public decimal RecaudacionConfiabilidadSum { get; set; }
        public decimal RecaudacionOtrosCargos { get; set; }
        public decimal ValorDeficit { get; set; }
        public decimal Caudal { get; set; }
        public decimal VolumenInicial { get; set; }
        public decimal VolumenFinal { get; set; }
        public decimal Volumen { get; set; }
        public decimal PotenciaIndisponible { get; set; }
        public decimal CapacidadAnterior { get; set; }
        public decimal CapacidadNueva { get; set; }
        public decimal ValorProgramado { get; set; }
        public decimal SumaHorasOperacion { get; set; }

        public DateTime FechaHora { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }

    }

    public class SioReporteDifusion
    {
        public int Tgenercodi { get; set; }
        public string Fenergnomb { get; set; }
        public decimal? Valor { get; set; }

        public int Hora { get; set; }
        public decimal? Costomarginalprom { get; set; }
        public decimal? Demandahorarioprom { get; set; }

        public int Grupocodi { get; set; }
        public int? Fenergcodi { get; set; }
        public string Gruponomb { get; set; }
        public decimal? CComb { get; set; }
        public decimal? Cvc { get; set; }
        public decimal? Cvnc { get; set; }
        public decimal? Cv { get; set; }

        public DateTime Periodo { get; set; }
        public decimal? Programado { get; set; }
        public decimal? Ejecutado { get; set; }
    }

}
