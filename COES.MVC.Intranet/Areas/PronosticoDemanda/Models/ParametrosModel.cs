using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class ParametrosModel
    {
        public int IdModulo { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }

        //Listas filtros
        public List<MePtomedicionDTO> ListArea { get; set; }
        public List<PrnClasificacionDTO> ListSubestacion { get; set; }
        public List<PrnClasificacionDTO> ListEmpresa { get; set; }
        public List<PrnClasificacionDTO> ListPtomedicion { get; set; }
        public List<MePtomedicionDTO> ListAgrupacion { get; set; }
        public List<PrGrupoDTO> ListBarra { get; set; }

        //Id de los filtros
        public int? SelById { get; set; }
        public List<int> SelAreas { get; set; }
        public List<int> SelSubestacion { get; set; }
        public List<int> SelEmpresa { get; set; }
        public List<int> SelPuntos { get; set; }
        public List<int> SelAgrupacion { get; set; }
        public string StrPuntos { get; set; }
        //agregado para barra 10/12/2019
        public List<int> SelBarraPM { get; set; }
        public List<int> SelBarraCP { get; set; }
        public List<int> SelBarra { get; set; }

        public decimal[][] data { get; set; }

        public int[] id { get; set; }

        //Barras PM y CP
        public List<PrGrupoDTO> ListBarraPM { get; set; }
        public List<PrGrupoDTO> ListBarraCP { get; set; }

        //Configuracion Estimador
        public List<int> SelFormula { get; set; }
        public decimal[] dArray { get; set; }
        public  string barraNombre { get; set; }
    }
}