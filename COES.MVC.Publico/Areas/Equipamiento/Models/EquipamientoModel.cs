using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Publico.Areas.Equipamiento.Models
{
    public class EquipamientoModel
    {
    }
    public class IndexEquipoModel
    {
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public int iTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public int iEmpresa { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public int iTipoEquipo { get; set; }
        public string sEstadoCodi { get; set; }
        public string CodigoEquipo { get; set; }
        public int NroPagina { get; set; }
        public List<EqEquipoDTO> ListadoEquipamiento { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
    }
}