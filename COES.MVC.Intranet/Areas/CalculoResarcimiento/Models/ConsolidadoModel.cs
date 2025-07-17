using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Models
{
    public class ConsolidadoModel
    {
        public bool TienePermisoAdmin { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string idReporteZip { get; set; }
        public SiCorreoDTO Correo { get; set; }
       

        //- Atributos para el consolidado
        public List<RePeriodoDTO> ListaPeriodo { get; set; }
        public int Anio { get; set; }
        public List<ReEmpresaDTO> ListaSuministrador { get; set; }
        public List<ReCausaInterrupcionDTO> ListaCausaInterrupcion { get; set; }
        public List<RePeriodoDTO> ListaPeriodos { get; set; }
        public bool Grabar { get; set; }

    }
}