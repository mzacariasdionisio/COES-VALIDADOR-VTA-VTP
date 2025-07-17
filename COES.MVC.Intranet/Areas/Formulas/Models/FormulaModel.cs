using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Scada;

namespace COES.MVC.Intranet.Areas.Formulas.Models
{
    public class FormulaModel
    {
        public List<MePerfilRuleDTO> ListaFormulas { get; set; }
        public List<ScadaDTO> ListaScada { get; set; }
        public String FechaDesde { get; set; }
        public String FechaHasta { get; set; }
        public ScadaDTO Entidad { get; set; }
        public PerfilScadaDTO PerfilScada { get; set; }
        public int IdPerfil { get; set; }
        public List<PerfilScadaDTO> Listado { get; set; }
        public string Descripcion { get; set; }
    }

    public class ConfiguracionPerfilModel
    {
        public List<SeguridadServicio.AreaDTO> ListaAreas { get; set; }
        public List<MePerfilRuleDTO> ListaFormulas { get; set; }
        public List<FormulaItem> ListaItems { get; set; }
        public string Prefijo { get; set; }
        public string SubEstacion { get; set; }
        public string AreaOperativa { get; set; }
        public List<int> IdsAreas { get; set; }
        public int IdFormula { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaModificacion { get; set; }
        public List<MePerfilRuleDTO> ListaEmpresas { get; set; }
        public List<MePerfilRuleDTO> ListaPuntos { get; set; }
        public List<EqAreaDTO> ListaAreaOperativa { get; set; }

        //Assetec.PRODEM.E3 - 20211125# Inicio        
        public List<MePtomedicionDTO> ListaPuntosTnaIEOD { get; set; }
        public List<MePtomedicionDTO> ListaPuntosTnaSCO { get; set; }
        //Assetec.PRODEM.E3 - 20211125# Fin
        //Assetec.DemandaDPO - 20230419# Inicio
        public List<MePtomedicionDTO> ListaPuntosTnaDPO { get; set; }
        //Assetec.DemandaDPO - 20230419# Fin
    }



}