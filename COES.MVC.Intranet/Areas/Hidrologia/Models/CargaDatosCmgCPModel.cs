using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Hidrologia;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Hidrologia.Models
{
    public class CargaDatosCmgCPModel
    {
        public string Fecha { get; set; }
        public string FechaAnterior { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public int Accion { get; set; }

        public int CodigoEnvio { get; set; }
        public HandsonModel Handson { get; set; }
        public List<CmVolumenCalculoDTO> ListaEnvio { get; set; }
        public CmVolumenCalculoDTO CabCalculo { get; set; }

        public CmModeloEmbalseDTO ModeloEmbalse { get; set; }
        public List<CmModeloEmbalseDTO> ListaEmbalse { get; set; }
        public ConfiguracionDefaultEmbalseHidrologia ConfiguracionDefault { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Detalle2 { get; set; }

    }

}