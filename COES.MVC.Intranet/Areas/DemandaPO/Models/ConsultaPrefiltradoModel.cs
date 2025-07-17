using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.DPODemanda.Helper;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class ConsultaPrefiltradoModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<DpoFiltro> ListaCargas { get; set; }
        public List<MePtomedicionDTO> ListaPuntosTNA { get; set; }
        public List<MePtomedicionDTO> ListaPuntosSICLI { get; set; }
        public List<DpoTrafoBarraDTO> ListaTransformadores { get; set; }
        public List<PrnVersiongrpDTO> ListaVersiones { get; set; }
        public List<PrnVersiongrpDTO> ListaVersionesPop { get; set; }
        public List<PrnVariableDTO> ListaVariables { get; set; }
        public List<DpoVersionRelacionDTO> ListaFormulas { get; set; }
        //Para las vistas de la grafica
        public List<DpoConsultaPrefiltrado> ListaData { get; set; }
        public DpoConsultaPrefiltrado ListaMaximo { get; set; }
        public DpoConsultaPrefiltrado ListaMinimo { get; set; }
        public List<int> ListaPosiciones { get; set; }
        public List<MePerfilRuleDTO> ListaFormulasDpo { get; set; }
        public List<DpoRelSplFormulaDTO> ListaAreaDemanda { get; set; }
        public List<DpoConsultaPrefiltrado> ListaAlgoritmo { get; set; }
    }
}