using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Ensayo.Models
{
    public class EnsayoModel
    {
        public string Mensaje { get; set; }
        public int Ensayocodi { get; set; }
        public int Enunidadcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public int IdEstado { get; set; }
        public int Estadocodi { get; set; }
        public int Formatocodi { get; set; }
        public int Ensayoacep { get; set; }
        public DateTime EnsayofechaEvento { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Ensayomodoper { get; set; }

        public int Ensayoaprob { get; set; }
        public string ModoOper { get; set; }

        public EnEnsayoDTO Ensayo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EnEnsayoDTO> ListaEnsayo { get; set; }
        public List<EnEnsayoDTO> ListaTotalDeEnsayos { get; set; }
        public List<PrGrupoDTO> ListaModosOperacion { get; set; }
        public List<EnEnsayoformatoDTO> LstArchEnvioEnsayo { get; set; }
        public List<EnEnsayounidadDTO> LstUnidadEnsayo { get; set; }
        public List<EnEstformatoDTO> LstEstFormatosEnsayo { get; set; }


        public int Opt01 { get; set; }
        public int Opt02 { get; set; }
        public string StrVectorUnidad { get; set; }
        public string StrVectorModOp { get; set; }
        public string StrObservacionFormato { get; set; }
        public bool OpAccesoEmpresa { get; set; }
        public bool OpAprobar { get; set; }
        public bool OpEditar { get; set; }
        public string FormatoDesc { get; set; }
        public int RowChange { get; set; }
    }

    public class BusquedaEnsayoModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EnEstadoDTO> ListaEstado { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public bool OpNuevo { get; set; }
        public bool OpAccesoEmpresa { get; set; }
        public bool OpEditar { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
    }


}