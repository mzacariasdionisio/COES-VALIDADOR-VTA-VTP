using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class CasoModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public int idModulo { get; set; }

        public string Fecha { get; set; }
        public string FechaMes { get; set; }
        public string FechaDia { get; set; }

        public int idCaso { get; set; }
        public DpoCasoDTO Caso { get; set; }
        public List<DpoCasoDTO> ListaCasos { get; set; }
        public List<EqAreaDTO> ListaAreaOperativa { get; set; }
        public List<MePerfilRuleDTO> ListaFormulas { get; set; }
        public List<DpoNombreCasoDTO> ListaNombreCasos { get; set; }
        public List<DpoUsuarioDTO> ListaUsuarios { get; set; }
        public List<DpoFeriadosDTO> ListaFeriados { get; set; }

        public DpoCasoDetalleDTO DetalleCaso { get; set; }
        public List<DpoCasoDetalleDTO> ListaDetalleCasos { get; set; }
        public List<DpoFuncionDataMaestraDTO> ListaFuncionesDataMaestra { get; set; }
        public List<DpoFuncionDataProcesarDTO> ListaFuncionesDataProcesar { get; set; }
        
        public List<DpoCasoDetalleDTO> ListaParametrosDataMaestra { get; set; }
        public List<DpoCasoDetalleDTO> ListaParametrosDataProcesar { get; set; }

        public List<DpoParametrosR1DTO> ListaParametrosR1 { get; set; }
        public List<DpoParametrosR2DTO> ListaParametrosR2 { get; set; }
        public List<DpoParametrosF1DTO> ListaParametrosF1 { get; set; }
        public List<DpoParametrosF2DTO> ListaParametrosF2 { get; set; }
        public List<DpoParametrosA1DTO> ListaParametrosA1 { get; set; }
        public List<DpoParametrosA2DTO> ListaParametrosA2 { get; set; }


        public List<DpoDiasTipicosR1DTO> ListaDiasTipicosR1 { get; set; }
        public List<DpoDiasTipicosR2DTO> ListaDiasTipicosR2 { get; set; }
        public List<DpoDiasTipicosA1DTO> ListaDiasTipicosA1 { get; set; }
        public List<DpoParametrosA2DTO> ListaDiasTipicosA2 { get; set; }

        public List<DpoHistorico48DTO> lstDatos48 { get; set; }
        public List<DpoHistorico96DTO> lstDatos96 { get; set; }
        public List<List<DpoHistorico48DTO>> lstlstDatos48 { get; set; }
        public List<List<DpoHistorico96DTO>> lstlstDatos96 { get; set; }

    }
}