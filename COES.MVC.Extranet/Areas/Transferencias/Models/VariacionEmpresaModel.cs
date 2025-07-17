using COES.Dominio.DTO.Transferencias;
using System.Collections.Generic;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class VariacionEmpresaModel: BaseModel
    {
        //Objetos del Modelo RangoValDatos
        public VtpVariacionEmpresaDTO Entidad { get; set; }
        public List<VtpVariacionEmpresaDTO> ListaVaricionEmpresa { get; set; }

        public string VarempprocentajeHistorico { get; set; }
        public string VarempprocentajeVTP { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
    }
}