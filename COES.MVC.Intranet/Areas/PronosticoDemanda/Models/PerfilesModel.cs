using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class PerfilesModel
    {
        public string Fecha { get; set; }
        public string FechaFin { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }

        //Listas filtros
        public List<EqAreaDTO> ListUbicacion { get; set; }
        public List<Tuple<int, string, bool>> ListTipoEmpresa { get; set; }
        public List<SiEmpresaDTO> ListEmpresa { get; set; }
        public List<MePtomedicionDTO> ListPtomedicion { get; set; }

        //Id de los filtros
        public int? SelById { get; set; }
        public List<int> SelUbicacion { get; set; }
        public List<int> SelTipoEmpresa { get; set; }
        public List<int> SelEmpresa { get; set; }
        public int? SelPunto { get; set; }
    }
}