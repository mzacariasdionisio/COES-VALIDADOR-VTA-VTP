using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Intervenciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.IEOD.Models
{
    public class ConfiguracionModel
    {
        public List<FwAreaDTO> ListaAreasCoes { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }
        public List<MePtomedicionDTO> ListaPtomedicion { get; set; }
        public List<SiTipoinformacionDTO> ListaMedida { get; set; }
        public string Emprnomb { get; set; }
        public SiTipoinformacionDTO Medida { get; set; }
        public int Origen { get; set; }

        public List<TrEmpresaSp7DTO> ListaTrEmpresa { get; set; }
        public List<TrZonaSp7DTO> ListaTrZona { get; set; }

        public MePtomedcanalDTO PtomedcanalActual { get; set; }
        public List<EstadoParametro> ListaEstado { get; set; }
        public List<MeOrigenlecturaDTO> ListaOrigenlectura { get; set; }

        public EqEquicanalDTO EquicanalActual { get; set; }

        //estado del resultado. 1: OK, -1: ERROR, 0: No hay actualización de datos
        public int Resultado { get; set; }
        
        public MePtomedicionDTO PtoMedicion { get; set; }
        public TrCanalSp7DTO TrCanal { get; set; }
        public List<TrCanalSp7DTO> ListarUnidadPorZona { get; set; }
        public List<TrCanalSp7DTO> ListTrCanal { get; set; }

    }
}