using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.CalculoResarcimiento.Models
{
    public class MedicionModel
    {
        public ReEventoProductoDTO Entidad { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string AnioMes { get; set; }
        public List<ReEmpresaDTO> ListaEmpresa { get; set; }
        public string Suministrador { get; set; }
        public string Acceso { get; set; }
        public int IdEmpresa { get; set; }
    }
}