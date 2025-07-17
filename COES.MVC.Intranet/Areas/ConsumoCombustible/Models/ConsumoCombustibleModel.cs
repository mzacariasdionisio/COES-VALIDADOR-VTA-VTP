using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.ConsumoCombustible;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.ConsumoCombustible.Models
{
    public class ConsumoCombustibleModel
    {
        public string FechaPeriodo { get; set; }
        public int IdVersion { get; set; }
        public List<LeyendaCCC> ListaLeyenda { get; set; }
        public CccVersionDTO Version { get; set; }
        public List<GraficoWeb> ListaGrafico { get; set; }

        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqEquipoDTO> ListaCentral { get; set; }
        public List<SiTipoinformacionDTO> ListaMedida { get; set; }
        public SiFuenteenergiaDTO FuenteEnergia { get; set; }
        public SiFactorconversionDTO FactorConversion { get; set; }

        public List<CccVcomDTO> ListaDetalleVCOM { get; set; }
        public List<PrGrupoDTO> ListaModoOp { get; set; }
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia { get; set; }
        public List<VCOMCambio> ListaCambio { get; set; }
        public bool TieneLogCambio { get; set; }
        public string NombreArchivoLogCambio { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public bool TienePermiso { get; set; }
    }
}