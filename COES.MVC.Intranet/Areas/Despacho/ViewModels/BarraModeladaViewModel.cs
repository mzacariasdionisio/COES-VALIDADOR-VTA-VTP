using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Despacho.ViewModels
{
    public class BarraModeladaViewModel
    {
    }

    public class ListaBarrasModeladasViewModel
    {
        public List<PrGrupoDTO> ListaBarras { get; set; }
    }

    public class DetalleBarraModelado
    {
        public List<EqEquipoDTO> ListabarrasEquipo { get; set; }
        public PrGrupoDTO BarraModelada { get; set; }
    }

}