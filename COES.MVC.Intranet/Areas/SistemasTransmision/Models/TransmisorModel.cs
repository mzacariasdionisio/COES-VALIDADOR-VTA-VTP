using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Models
{
    public class TransmisorModel : BaseModel
    {
        public List<StTransmisorDTO> ListaEmpresaTransmisor { get; set; }
        public StTransmisorDTO Entidad { get; set; }

        public List<StRecalculoDTO> ListaStRecalculo { get; set; }
        public StRecalculoDTO EntidadStRecalculo { get; set; }

        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public EmpresaDTO EntidadEmpresa { get; set; }

        public List<StPeriodoDTO> ListaPerido { get; set; }
        public StPeriodoDTO EntidadPeriodo { get; set; }
    }
}