using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Models
{
    public class GeneracionCuadrosRechazoCargaModel
    {
        public List<RcaCuadroProgDTO> ListCuadroProgramaCabecera{ get; set; }
        public List<RcaCuadroProgDTO> ListCuadroProgramaDetalle { get; set; }
        public List<RcaProgramaDTO> ListPrograma { get; set; }

        public List<RcaConfiguracionProgDTO> ListConfiguracionProg { get; set; }

        public List<RcaHorizonteProgDTO> ListHorizonteProg { get; set; }

        public bool bNuevo { get; set; }

        public bool bAdicional { get; set; }

        public List<RcaCuadroEstadoDTO> ListEstadoCuadroProg { get; set; }

        public List<RcaHorizonteProgDTO> Horizontes { get; set; }

        public List<RcaProgramaDTO> Programas { get; set; }

        public int SemanaActual { get; set; }

        public List<EventoDTO> ListEventos { get; set; }

        /// <summary>
        /// Tipo Perfil: SPR=1, SCO=2, SEV = 3
        /// </summary>
        public int Perfil { get; set; }
    }
}