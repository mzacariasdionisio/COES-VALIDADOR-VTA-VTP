using COES.Dominio.DTO.Sic;
using System.Collections.Generic;


namespace COES.MVC.Extranet.Areas.CalculoResarcimiento.Model
{
    public class DeclaracionModel
    {
        public string Emprnombre { get; set; }
        public int Emprcodi { get; set; }
        public string IndicadorEmpresa { get; set; }
        public List<RePeriodoDTO> ListaPeriodo { get; set; }
        public int Anio { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<ReEnvioDTO> ListaEnvios { get; set; }
        public string Indicador { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
    }
}