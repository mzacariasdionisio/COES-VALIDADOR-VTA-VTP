using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes.Models
{
    public class IngresoTransmisionModel : BaseModel
    {
        public CaiAjusteempresaDTO EntidadAjusteEmpresa { get; set; }
        public CaiIngtransmisionDTO EntidadIngresoTransmision { get; set; }

        public List<CaiAjusteempresaDTO> ListaAjusteEmpresa { get; set; }
        public List<MePtomedicionDTO> ListaPtomedicion { get; set; }
        
        //Identificador de tabla
        public int Caiajecodi { get; set; }
    }

    public class IngresoTransmisionFormatoModel : FormatoModel
    {
        //Identificadores a tablas
        public int Caiprscodi { get; set; }
        public int Caiajcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Tipoemprcodi { get; set; }
        public string Intervalo { get; set; }
        public string cbPeriodo { get; set; }
        public int NroMeses { get; set; }

        //Entidades
        public SiEmpresaDTO EntidadEmpresa { get; set; }

        //Listas
        public List<CaiAjusteempresaDTO> ListaPeriodos { get; set; }
    }
}