using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class RangoValDatosModel: BaseModel
    {
        //Objetos del Modelo RangoValDatos
        public VtpVariacionEmpresaDTO Entidad { get; set; }
        public List<VtpVariacionEmpresaDTO> ListaVaricionEmpresa { get; set; }

        public VtpVariacionCodigoDTO EntidadVariacionCodigo { get; set; }
        public List<VtpVariacionCodigoDTO> ListaVariacionCodigo { get; set; }

        public string VarempprocentajeHistorico { get; set; }
        public string VarempprocentajeVTP { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
        public bool Varempcodigo { get; set; }
        public int VarTipoComp { get; set; }
    }
}