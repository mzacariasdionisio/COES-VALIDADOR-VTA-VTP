using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class AgrupacionesModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        
        public List<Tuple<int, int, string, bool>> ListArea { get; set; }
        public List<MePtomedicionDTO> ListAgrupacion { get; set; }
        public List<SiEmpresaDTO> ListEmpresa { get; set; }

        //Id de los filtros
        public int? SelById { get; set; }
        public List<int> SelArea { get; set; }
        public List<int> SelEmpresa { get; set; }
        public List<int> SelAgrupacion { get; set; }
        public int EsPronostico { get; set; }

    }
}