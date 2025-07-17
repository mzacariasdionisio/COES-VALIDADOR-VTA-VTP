using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class VariacionCodigoModel: BaseModel
    {
        //Objetos del Modelo VariacionCodigoModel
        public VtpVariacionCodigoDTO Entidad { get; set; }
        public List<VtpVariacionCodigoDTO> ListaVaricionCodigo { get; set; }
        public string VarempprocentajeHistorico { get; set; }
        public string VarempprocentajeVTP { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }
    }
}