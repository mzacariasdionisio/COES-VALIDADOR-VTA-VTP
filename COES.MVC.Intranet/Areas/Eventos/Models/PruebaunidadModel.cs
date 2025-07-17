using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class EvePruebaunidadModel
    {
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public EvePruebaunidadDTO EvePruebaunidad { get; set; }
        public int PrundCodi { get; set; }
        public string PrundFecha { get; set; }
        public int? PrundEscenario { get; set; }
        public string PrundHoraordenarranque { get; set; }
        public string PrundHorasincronizacion { get; set; }
        public string PrundHorainiplenacarga { get; set; }
        public string PrundHorafalla { get; set; }
        public string PrundHoraordenarranque2 { get; set; }
        public string PrundHorasincronizacion2 { get; set; }
        public string PrundHorainiplenacarga2 { get; set; }
        public string PrundSegundadesconx { get; set; }
        public string PrundFallaotranosincronz { get; set; }
        public string PrundFallaotraunidsincronz { get; set; }
        public string PrundFallaequiposinreingreso { get; set; }
        public string PrundCalchayregmedid { get; set; }
        public string PrundCalchorafineval { get; set; }
        public string PrundCalhayindisp { get; set; }
        public string PrundCalcpruebaexitosa { get; set; }
        public decimal? PrundCalcperiodoprogprueba { get; set; }
        public string PrundCalccondhoratarr { get; set; }
        public string PrundCalccondhoraprogtarr { get; set; }
        public string PrundCalcindispprimtramo { get; set; }
        public string PrundCalcindispsegtramo { get; set; }
        //public decimal? PrundPotencia { get; set; }
        public decimal? PrundRpf { get; set; }
        public decimal? PrundTiempoprueba { get; set; }
        public string PrundUsucreacion { get; set; }
        public string PrundFeccreacion { get; set; }
        public string PrundUsumodificacion { get; set; }
        public string PrundFecmodificacion { get; set; }
        //public string PrundTiempoarranque { get; set; }        
        public int Accion { get; set; }
        public int Grupocodi { get; set; }
        public string Prundeliminado { get; set; }

        //parametros por modo de operacion
        public decimal PotenciaEfectiva { get; set; }
        public decimal TiempoEntreArranques { get; set; }
        public decimal TiempoArranqueSinc { get; set; }
        public decimal TiempoSincPotEf { get; set; }
        public decimal Rpf { get; set; }
        public decimal TiempoPrueba { get; set; }
        public decimal? PrundRatio { get; set; }

    }

    public class BusquedaEvePruebaunidadModel
    {
        public List<EvePruebaunidadDTO> ListaEvePruebaunidad { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
    }

}
