using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class RelacionBarrasModel
    {
        public string Fecha { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<Tuple<int, int, string, bool>> ListArea { get; set; }
        public List<PrGrupoDTO> ListBarraPM { get; set; }
        public List<PrnAgrupacionDTO> ListAgrupaciones { get; set; }
        public List<SiEmpresaDTO> ListEmpresas { get; set; }
        public List<EqAreaDTO> ListUbicaciones { get; set; }

        //Id de los filtros
        public List<int> SelArea { get; set; }
        public List<int> SelBarra { get; set; }
        public List<int> SelUbicaciones { get; set; }
        public List<int> SelEmpresas { get; set; }

        //Para el popup
        public List<object> DtSeleccionados { get; set; }
        public List<object> DtPuntos { get; set; }
        public List<object> DtAgrupaciones { get; set; }
        public List<MePtomedicionDTO> ListPuntos { get; set; }

        //adicionales 14042020
        public List<SiEmpresaDTO> ListEmpresa { get; set; }
    }
}