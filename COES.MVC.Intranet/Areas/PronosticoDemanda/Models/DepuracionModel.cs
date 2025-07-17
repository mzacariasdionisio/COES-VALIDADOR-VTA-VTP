using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class DepuracionModel
    {
        public string Fecha { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public int IdModulo { get; set; }

        public string FechaIni { get; set; }
        public string FechaFin { get; set; }

        //Listas filtros
        public List<Tuple<int, int, string, bool>> ListArea { get; set; }
        public List<Tuple<int, string, bool>> ListTipoDemanda { get; set; }
        public List<Tuple<int, string, bool>> ListTipoEmpresa { get; set; }
        public List<EqAreaDTO> ListUbicacion { get; set; }
        public List<SiEmpresaDTO> ListEmpresa { get; set; }
        public List<MePtomedicionDTO> ListPtomedicion { get; set; }
        public List<MePtomedicionDTO> ListAgrupacion { get; set; }
        public List<EveSubcausaeventoDTO> ListJustificacion { get; set; }

        //Id de los filtros
        public int? SelById { get; set; }
        public int SelTipoDemanda { get; set; }
        public int SelTipoEmpresa { get; set; }
        public List<int> SelListTipoEmpresa { get; set; }
        public List<int> SelArea { get; set; }
        public List<int> SelUbicacion { get; set; }
        public List<int> SelEmpresa { get; set; }
        public List<int> SelPuntos { get; set; }
        public List<int> SelAgrupacion { get; set; }
        public List<int> SelPerfil { get; set; }
        public List<int> SelClasificacion { get; set; }
        public string SelPronostico { get; set; }
        public int SelFuente { get; set; }
        public List<string> SelAreaOperativa { get; set; }
        public string SelOrden { get; set; }
        public List<string> SelJustificacion { get; set; }
        public bool SelBarra { get; set; }
    }
}