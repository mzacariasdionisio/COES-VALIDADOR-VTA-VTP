using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{

    public class OperacionesVariasModel
    {

        public List<EveEvenclaseDTO> ListaEvenclase { get; set; }
        public List<EveSubcausaeventoDTO> ListaEvensubcausa { get; set; }
        public EveIeodcuadroDTO IeodCuadro { get; set; }
        public List<EveIeodcuadroDetDTO> IeodCuadroDet { get; set; }
        public List<EveCongesgdespachoDTO> ListaCongestion { get; set; }
        public List<EveGpsaisladoDTO> ListaGpsAislado { get; set; }
        public List<PrGrupoDTO> ListaGrupoDespacho { get; set; }
        public List<PrGrupoDTO> ListaGrupoDespachoEdit { get; set; }
        public List<MeGpsDTO> ListaGps { get; set; }
        public int Accion { get; set; }
        public int Accioncuadro { get; set; }
        public int Iccodi { get; set; }
        public int Equicodi { get; set; }
        public int Subcausacodi { get; set; }
        public int Ictipcuadro { get; set; }
        public string Ichorinicarga { get; set; }
        public string Ichorini { get; set; }
        public string Ichorfin { get; set; }
        public string Icdescrip1 { get; set; }
        public string Icdescrip2 { get; set; }
        public string Icdescrip3 { get; set; }
        public string Iccheck1 { get; set; }
        public string Iccheck2 { get; set; }
        public decimal Icvalor1 { get; set; }
        public int? Evenclasecodi { get; set; }
        public string Ichor3 { get; set; }
        public string Ichor4 { get; set; }
        public string Iccheck3 { get; set; }
        public string Iccheck4 { get; set; }
        public decimal Icvalor2 { get; set; }
        public string EquipoInvolucrado { get; set; }
        public string GrupoInvolucrado { get; set; }
        public string StrGpsAislado { get; set; }
        public string StrAislado { get; set; }
        public string StrAisladoFlagPpal { get; set; }
        public bool GrabarEvento { get; set; }

    }


    public class BusquedaOperacionesVariasModel
    {
        public List<EveIeodcuadroDTO> ListaIeodcuadro { get; set; }
        public List<EveEvenclaseDTO> ListaEvenclase { get; set; }
        public List<EveSubcausaeventoDTO> ListaEvensubcausa { get; set; }
        public List<PrGrupoDTO> ListaGrupoDespacho { get; set; }
        public PrGrupoDTO ObjGrupoDespacho { get; set; }
        public string Fechaini { get; set; }
        public string Fechafin { get; set; }
        public string Horizonte { get; set; }
        public string TipoOperacion { get; set; }
        public string Copiar1 { get; set; }
        public string Copiar2 { get; set; }
        public string Copiar3 { get; set; }
        public string Copiar4 { get; set; }
        public string Copiar5 { get; set; }
        public string Copiar6 { get; set; }
        public string Copiar7 { get; set; }
        public string Fecha1 { get; set; }
        public string Fecha2 { get; set; }
        public string Fecha3 { get; set; }
        public string Fecha4 { get; set; }
        public string Fecha5 { get; set; }
        public string Fecha6 { get; set; }
        public string Fecha7 { get; set; }
        public string Horaini1 { get; set; }
        public string Horaini2 { get; set; }
        public string Horaini3 { get; set; }
        public string Horaini4 { get; set; }
        public string Horaini5 { get; set; }
        public string Horaini6 { get; set; }
        public string Horaini7 { get; set; }
        public string Horafin1 { get; set; }
        public string Horafin2 { get; set; }
        public string Horafin3 { get; set; }
        public string Horafin4 { get; set; }
        public string Horafin5 { get; set; }
        public string Horafin6 { get; set; }
        public string Horafin7 { get; set; }
        public string Copiartexto { get; set; }
        public string Iccodis { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }

        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
        public string Resultado { get; set; }
    }

    public class RelacionAreaSubcausaModel
    {
        public List<EveSubcausaeventoDTO> ListaEvensubcausa { get; set; }
        public List<EveAreaSubcausaeventoDTO> ListaRelacionAreaSubcausa { get; set; }
        public List<FwAreaDTO> ListaArea { get; set; }
        public string ListaAreaJson { get; set; }
        public bool AccesoNuevo { get; set; }
        public bool AccesoEditar { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
    }
}
