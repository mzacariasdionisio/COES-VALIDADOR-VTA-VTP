using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.AporteIntegrantes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class SolicitudCodigoModel : BaseModel
    {
        public List<SolicitudCodigoDTO> ListaCodigoRetiro { get; set; }
        public SolicitudCodigoDTO Entidad { get; set; }

        public List<SolicitudCodigoDetalleDTO> ListaCodigoRetiroDetalle { get; set; }
        public List<TrnPotenciaContratadaDTO> ListarEnvios { get; set; }
        public int IdcodRetiro { get; set; }

        public string Solicodiretifechainicio { get; set; }
        public string Solicodiretifechafin { get; set; }
        public string sError { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
        public List<BarraRelacionDTO> ListaRelacionBarras { get; set; }
        public string ListaPotenciasContratadasVTP { get; set; }

        public string RucCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Comentario { get; set; }



    }
}