using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Hidrologia.Models
{
    public class PtoDespachoModel
    {
        public List<Dominio.DTO.Sic.EmpresaDTO> ListarEmpresas { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public List<EstadoModel> EstadoLista { get; set; }
        public List<EqAreaDTO> ListaAreas { get; set; }
        public PrGrupoDTO PrGrupo { get; set; }
        public string EstadoId { get; set; }
        public int RepCodi { get; set; }
        public string NombreGrupo { get; set; }
        public int NroPagina { get; set; }        
        public bool OpcionEditar { get; set; }
        public bool OpcionEspecial { get; set; }
        public Int32 EmpresaId { get; set; }
        public int NroMostrar { get; set; }
        public int NroPaginas { get; set; }
        public int IndicadorTipo { get; set; }
        public bool IndicadorPagina { get; set; }
        public List<Dominio.DTO.Sic.SiFuenteenergiaDTO> ListarFEnergia { get; set; }
        public List<Dominio.DTO.Sic.PrTipogrupoDTO> ListarTipoGrupo { get; set; }
    }

        public class BusquedaPtoDespachoModel
        {
            public List<SiEmpresaDTO> ListaEmpresas { get; set; }
            public List<EstadoModel> EstadoLista { get; set; }
            public int iTipoEquipo { get; set; }
             public string EstadoId { get; set; }
             public List<EqAreaDTO> ListaAreas { get; set; }
            public List<EqFamiliaDTO> ListaFamilia { get; set; }
            public List<PrCategoriaDTO> ListaTipoGrupo { get; set; }
            public List<EqEquipoDTO> ListaEquipo { get; set; }
            public List<PrGrupoDTO> ListaGrupo { get; set; }
            public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
            public List<EstadoModel> ListaEstados { get; set; }
            public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
            public string NombreEquipo { get; set; }
            public int iEmpresa { get; set; }
            public int iTipoEmpresa { get; set; }
            public string sEstadoCodi { get; set; }
            public string CodigoEquipo { get; set; }
            public string EnableNuevo { get; set; }
            public string EnableEditar { get; set; }
            public bool AccesoNuevo { get; set; }
            public bool AccesoEditar { get; set; }

    }
        public class DatoComboBox
        {
            public string Descripcion { get; set; }
            public string Valor { get; set; }
        }
}





