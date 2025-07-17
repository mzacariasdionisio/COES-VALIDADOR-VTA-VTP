using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class SDDPModel
    {
        public string Mes { get; set; }
        public List<GenericoDTO> ListaAnio { get; set; } = new List<GenericoDTO>();
        public List<PmoPeriodoDTO> ListaMes { get; set; } = new List<PmoPeriodoDTO>();

        public List<PmoConfIndispEquipoDTO> Correlaciones { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<PrGrupoDTO> ListaModoOp { get; set; }
        public int Agrupcodi { get; set; }
        public PmoConfIndispEquipoDTO Correlacion { get; set; }

        public List<PmoFormatoDTO> ListaPmpoformato { get; set; }
        public List<PmoLogDTO> ListaLog { get; set; } = new List<PmoLogDTO>();
        public MeEnvioDTO Envio { get; set; }

        public string RutaCarga { get; set; }
        public string RutaDescarga { get; set; }
        public string HtmlReporte { get; set; }
        public string Archivo { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public List<string> ListaVal { get; set; }
        public bool TieneAlerta { get; set; }

        public string Fecha { get; set; }
        public int Anho { get; set; }
        public string Semana { get; set; }
        public int NroSemana { get; set; }
        public int Periodo { get; set; }
        public string TipoFormato { get; set; }
        public string Titulo { get; set; }

        public PmoPeriodoDTO PmoPeriodo { get; set; }
        public string FechaPeriodo { get; set; }
        public string MesPeriodo { get; set; }

        //codigo SDDP
        public List<PmoSddpTipoDTO> ListaTipoSDDP { get; set; }
        public List<PmoSddpCodigoDTO> ListaCodigoSDDP { get; set; }
        public PmoSddpCodigoDTO CodigoSDDP { get; set; }

        //Handson
        public string[][] Data { get; set; }
        public object[] Columnas { get; set; }
    }

    public class CorrelacionModel
    {
        public int PmCindCodi { get; set; }
        public int Sddpcodi { get; set; }
        public int EquiCodi { get; set; }
        public int Grupocodimodo { get; set; }
        public decimal Porcentaje { get; set; }
        public bool Actualizar { get; set; }

        #region NET 20190228
        public string PmCindConJuntoEqp { get; set; }
        public string PmCindRelInversa { get; set; }
        #endregion
    }
}