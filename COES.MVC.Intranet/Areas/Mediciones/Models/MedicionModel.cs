using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Mediciones.Models
{
    public class MedicionModel
    {
    }

    public class ConfiguracionModel
    {
        public List<PrTipogrupoDTO> ListaTipoGrupo { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public int IdGrupo { get; set; }
        public string EmpresaNombre { get; set; }
        public string GrupoNombre { get; set; }
        public string GrupoAbreviacion { get; set; }
        public int TipoGrupoCodi { get; set; }
        public int TipoGrupoCodi2 { get; set; }
        public string IndAdjudicada { get; set; }
        
    }

    public class FormatoModel
    {
        public List<WbGeneracionrerDTO> ListaPuntos { get; set; }
        public List<WbGeneracionrerDTO> ListaEmpresa { get; set; }
        public List<WbGeneracionrerDTO> ListaCentrales { get; set; }
        public List<WbGeneracionrerDTO> ListaUnidades { get; set; }
        public string IndCentral { get; set; }
    }
}