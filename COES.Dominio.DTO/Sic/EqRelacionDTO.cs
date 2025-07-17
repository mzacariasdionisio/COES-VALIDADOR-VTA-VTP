using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_RELACION
    /// </summary>
    public class EqRelacionDTO : EntityBase
    {
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public int Relacioncodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Codincp { get; set; }
        public string Nombrencp { get; set; }
        public string Codbarra { get; set; }
        public string Codbarra1 { get; set; }
        public string Idgener { get; set; }
        public string Descripcion { get; set; }
        public string Nombarra { get; set; }
        public string Estado { get; set; }
        public string IndTipo { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public int Emprcodi { get; set; }
        public int Indcc { get; set; }
        public string Modosoperacion { get; set; }
        public string Tension { get; set; }
        public string Tension1 { get; set; }
        public int Contador { get; set; }
        public int Grupocodi { get; set; }
        public int Grupopadre { get; set; }
        public int? Ccombcodi { get; set; }
        public string Indtvcc { get; set; }
        public string Indfuente { get; set; }
        public int? Ptomedicodi { get; set; }
        public decimal? PotenciaActiva { get; set; }
        public decimal? PotenciaReactiva { get; set; }
        public int Famcodi { get; set; }
        public string Desubicacion { get; set; }
        public string Famnomb { get; set; }
        public string Subcausacmg { get; set; }
        public int Subcausacodi { get; set; }
        public decimal Valor { get; set; }
        public int Propcodi { get; set; }
        public string Propiedad { get; set; }
        public int? Equipadre { get; set; }
        public string Gruponomb { get; set; }

        /// Datos Adicionales
        public double CostoCombustible { get; set; }
        public double CostoVariableOYM { get; set; }
        public double PotenciaMaxima { get; set; }
        public double PotenciaMinima { get; set; }
        public double? VelocidadCarga { get; set; }
        public double? VelocidadDescarga { get; set; }
        public string Calificacion { get; set; }
        public int IdModoOperacion { get; set; }
        public string IndModoOperacion { get; set; }
        public double PotGenerada { get; set; }
        public string IndOperacion { get; set; }
        public List<CoordenadaConsumo> ListaCurva { get; set; }
        public List<RestriccionUnidad> ListaRestriccion { get; set; }
        public string CapacidadRegulacion { get; set; }
        public double FactorConversion { get; set; }
        public bool Procesado { get; set; } //indica si el item fue procesado para la condicion 1
        public bool CumplePotMinimaUnidades { get; set; } //indica que el item cumple con la potencia minima de todas las unidades que conforman la central
        public bool Forzada { get; set; } //indica si la unidad es forzada        
        public bool CentralEvaluada { get; set; } //indica si la central ha sido evaluada
        public bool IndEspecial { get; set; }
        public string Indcoes { get; set; }
        public string Codbarrancp { get; set; }
        public int IdCalificacion { get; set; }
        public bool IndicadorEliminar { get; set; }
        //- Campos para evaluacion RSF
        public bool ExistenciaRsf { get; set; }
        public decimal ValorRsf { get; set; }
        public int PadreRsf { get; set; }
        public int CantidadRsf { get; set; }
        public int IdCalificacionCompensacion { get; set; }
        public decimal ValorRsfUp { get; set; }
        public decimal ValorRsfDown { get; set; }

        /// <summary>
        /// Agregados para gráfico de supervisión de la demanda
        /// </summary>
        public int? Canalcodi { get; set; }
        public string Canaliccp { get; set; }
        public string Indrvarte { get; set; }
        public string Estadorvarte { get; set; }

        public List<SupDemandaDato> ListaGeneracion { get; set; }
        public int IndCompletado { get; set; }

        public string Canaliccpint { get; set; }
        public string Canalsigno { get; set; }
        public string Canaluso { get; set; }
        public string Canalcero { get; set; }
        public int Tgenercodi { get; set; }
        public string Tgenernomb { get; set; }
        public int? Recurcodibarra { get; set; }

        public string LimiteTransmision { get; set; }

        public bool IndLimTran { get; set; }

        //- Campos para EMS TNA
        public string Nombretna { get; set; }
        //- Fin de cambios EMS TNA
        public string Indnoforzada { get; set; }

        public decimal PotMax { get; set; }
        public decimal PotMin { get; set; }

        //- Linea agregada movisoft 25.02.2021
        public string Indgeneracionrer { get; set; }

        #region Ticket 2022-004245
        public string Indnomodeladatna { get; set; }
        public string IndBarraNoEncontrada { get; set; }
        #endregion

        //- Cambio por CMgCP_PR07
        public string IndPasada { get; set; }
        public double Cvh { get; set; }
        public double Rendimiento { get; set; }
        public string IndVolumenB { get; set; }

        public string Indtnaadicional { get; set; }
        public string EquipoAdicionalTNA { get; set; }
        public string Usucreacion { get; set; }
    }

    /// <summary>
    /// Clave para el manejo de curva de consumo
    /// </summary>
    public class CoordenadaConsumo
    {
        public decimal Potencia { get; set; }
        public decimal Consumo { get; set; }
    }

    /// <summary>
    /// Para almacenar las restricciones de una unidad
    /// </summary>
    public class RestriccionUnidad
    {
        public int Tipo { get; set; }
        public decimal Valor { get; set; }
    }

    /// <summary>
    /// Entidad para poder trabajar el reporte
    /// </summary>
    public class SupDemandaDato
    {
        public int Grupocodi { get; set; }
        public int Gruponomb { get; set; }
        public DateTime Fecha { get; set; }
        public decimal? Valor { get; set; }
        public decimal? PotenciaMaxima { get; set; }
        public int Tipo { get; set; }
        public int Indice { get; set; }
    }

    /// <summary>
    /// Entidad para el manejo de gráfico de demanda
    /// </summary>
    public class SupDemandaGraficoDatos
    {
        public string Hora { get; set; }
        public decimal Programado { get; set; }
        public decimal Ejecutado { get; set; }
        public decimal DemandaDiaTipico { get; set; }
        public decimal DemandaDiaAnterior { get; set; }
        public decimal ReservaRotante { get; set; }
        public decimal PotenciaMaxima { get; set; }
        public int Indice { get; set; }

    }

    /// <summary>
    /// Entidad para obtener los datos del grafico de demanda
    /// </summary>
    public class SupDemandaGrafico
    {
        public List<SupDemandaGraficoDatos> ListaTotal { get; set; }
        public List<SupDemandaGraficoDatos> ListaSolar { get; set; }
        public List<SupDemandaGraficoDatos> ListaEolica { get; set; }
        public List<SupDemandaGraficoDatos> ListaReservaRotante { get; set; }
        public decimal Indicador1 { get; set; }
        public decimal Indicador2 { get; set; }
        public decimal Indicador3 { get; set; }
        public decimal Indicador4 { get; set; }
        public decimal Indicador5 { get; set; }
        public int IndicadorHora { get; set; }
    }
}
