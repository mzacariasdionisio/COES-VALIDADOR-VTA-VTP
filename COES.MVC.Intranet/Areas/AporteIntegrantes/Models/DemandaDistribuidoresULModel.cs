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
    public class DemandaDistribuidoresULModel : BaseModel
    {
        public List<CaiAjusteempresaDTO> ListaEmpresasCumplimiento { get; set; }
        public List<CaiAjusteempresaDTO> ListaAjusteEmpresa { get; set; }
        //entidad
        public CaiAjusteempresaDTO EntidadAjusteEmpresa { get; set; }
    }

    public class DemandaDistribuidoresULFormatoModel : FormatoModel
    {
        //Identificadores a tablas
        public int Caiprscodi { get; set; }
        public int Caiajcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Tipoemprcodi { get; set; }
        public string Intervalo { get; set; }

        //Entidades
        public SiEmpresaDTO EntidadEmpresa { get; set; }

        //Listas
        public List<CaiAjusteempresaDTO> ListaPeriodos { get; set; }
    }
}