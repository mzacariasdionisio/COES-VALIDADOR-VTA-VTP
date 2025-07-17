using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.MVC.Extranet.Areas.RechazoCarga.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Models
{
    public class EnvioArchivosMagnitudModel
    {
        public List<RcaProgramaDTO> Programas { get; set; }
        public List<RcaSuministradorDTO> Suministradores { get; set; }

        public bool Exito { get; set; }

        public List<ErrorHelper> ListaErrores { get; set; }

        public FormatoModel formatoClientes { get; set; }

        public List<RcaCuadroEjecUsuarioDTO> ListClientes { get; set; }

        public string RangoFechas { get; set; }

        public string fechaInicioDetalle { get; set; }
        public string fechaFinDetalle { get; set; }

        public string MensajeError { get; set; }
    }
}