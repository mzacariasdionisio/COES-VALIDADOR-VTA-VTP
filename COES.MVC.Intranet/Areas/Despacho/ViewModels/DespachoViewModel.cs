using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;

namespace COES.MVC.Intranet.Areas.Despacho.ViewModels
{
    public class HistorialParametroViewModel
    {
        public string NombreConcepto { get; set; }
        public List<PrGrupodatDTO> ListadoHistorico { get; set; }
    }

    public class DetalleDatosMOViewModel
    {
        public int iGrupoCodi { get; set; }
        public string NombreModo { get; set; }
        public string AbreviaturaModo { get; set; }
        public string Empresa { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public bool cbFicha { get; set; }
        public List<PrGrupoConceptoDato> ListaResultado { get; set; }
        public int repCodi { get; set; }
        public DateTime FechaRepCV { get; set; }
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }

    }

    public class EditarNuevoDatoMOViewModel
    {
        public string NombreConcepto { get; set; }
        public string FechaDat { get; set; }
        public string Formula { get; set; }
        public int GrupoCodi { get; set; }
        public int ConcepCodi { get; set; }
        public List<PrGrupoDTO> ModosOperacion { get; set; }
        public List<PrConceptoDTO> ListadoConceptos { get; set; }
        public string sModoEdicion { get; set; }
    }

    public class IndexDatosMOViewModel
    {
        public List<SiEmpresaDTO> EmpresaLista { get; set; }
        public List<EqEquipoDTO> CentralesLista { get; set; }
        public Int32 EmpresaId { get; set; }
        public Int32 CentralId { get; set; }
        public string NombreModo { get; set; }
        public List<EstadoModel> EstadoLista { get; set; }
        public string EstadoId { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public bool IndicadorPagina { get; set; }
        public List<PrGrupoDTO> ResultadoLista { get; set; }
        public DateTime FechaRepCV { get; set; }
        public int RepCodi { get; set; }

    }

   
}