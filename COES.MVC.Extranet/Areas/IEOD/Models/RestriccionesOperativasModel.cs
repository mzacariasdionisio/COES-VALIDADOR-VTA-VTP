using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.IEOD.Models
{
    public class RestriccionesOperativasModel
    {
        public List<EqEquipoDTO> ListaTipoEquipo { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EveIeodcuadroDTO> ListaRestricciones { get; set; }
        public List<EveIeodcuadroDTO> ListaRestriccionesAnt { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeEnvioDTO> ListaEnvios { get; set; }
        public SiPlazoenvioDTO PlazoEnvio { get; set; }
        public int Enviocodi { get; set; }
        public int Equicodi { get; set; }
        public string HoraIni { get; set; }
        public string HoraFin { get; set; }
        public string Fecha { get; set; }
        public string FechaFin { get; set; }
        public string IcDescrip1 { get; set; }
        public string FamcodiSelect { get; set; }
        public string ActFiltro { get; set; }
        public int Iccodi { get; set; }
        public string Lastuser { get; set; }
        public Boolean EnabledRestriccion { get; set; }
        public string Icnombarchenvio { get; set; }
        public string Icnombarchfisico { get; set; }
        public int CambioArchivo { get; set; }
        public int IdEnvio { get; set; }
        public string FechaEnvio { get; set; }
        public int Subcausacodi { get; set; }
        public bool EsEmpresaVigente { get; set; }
    }
}