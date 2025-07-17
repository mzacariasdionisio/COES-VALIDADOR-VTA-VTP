using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.CalculoResarcimiento.Model
{
    public class ObservacionModel
    {
        public string Emprnombre { get; set; }
        public int Emprcodi { get; set; }
        public string IndicadorEmpresa { get; set; }
        public List<RePeriodoDTO> ListaPeriodo { get; set; }
        public int Anio { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string Plazo { get; set; }
    }
}