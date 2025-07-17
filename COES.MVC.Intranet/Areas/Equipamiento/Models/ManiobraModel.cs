using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Equipamiento.Models
{
    /// <summary>
    /// Model para manejar los datos de procedimientos de maniobra
    /// </summary>
    public class ManiobraModel
    {
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqEquipoDTO> ListaEquipos { get; set; }
    }

    /// <summary>
    /// Model para tratar la imagen
    /// </summary>
    public class MapaModel
    {
        public string Emprnomb { get; set; }
        public string Tipoequipo { get; set; }
        public string Equinomb { get; set; }
        public string CoordenadaX { get; set; }
        public string CoordenadaY { get; set; }
        public int Equicodi { get; set; }
    }

    /// <summary>
    /// Model para manejo de todo el mapa
    /// </summary>
    public class MapaEquipoModel
    {
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; } 
    }
}