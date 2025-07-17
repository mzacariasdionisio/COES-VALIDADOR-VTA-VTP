using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.IEOD.Models
{
    /// <summary>
    /// Model para el la depuración del proostico de demanda
    /// </summary>
    public class PronosticoModel
    {
        public int IdEmpresa { get; set; }
        public int IdEnvio { get; set; }
        public int IdHojaPadre { get; set; }
        public int NumPtosDepurar { get; set; }
        public int Formatresolucion { get; set; }
        public int IdTipoEmpresa { get; set; }
        //Entidad
        public MeEnvioDTO EntidadEnvio { get; set; }
        public MeFormatoDTO EntidadFormato { get; set; }
        //public PrnMotivoDTO EntidadMotivo { get; set; }
        //Listas
        public List<PrnMedicion48DTO> ListaPrnMed48 { get; set; }
        //public List<PrnMedicion96DTO> ListaPrnMed96 { get; set; }
        //public List<PrnMotivoDTO> ListaMotivo { get; set; }
    }

    /// <summary>
    /// Estructura para manajear los datos horarios (15 o 30 minutos)
    /// </summary>
    public class ComparativoItemModel
    {
        public string PtomediDesc { get; set; }
        public string Medifecha { get; set; }
        public string Lectura { get; set; }
        public decimal PorcDesviacion { get; set; }
        public string Hora { get; set; }
        public decimal ValorPatron { get; set; }
        public decimal ValorMedicion { get; set; }
        public decimal ValorPrevisto { get; set; }
        public decimal Desviacion { get; set; }
        public decimal Pronostico { get; set; }
        //Lista
        public List<EveSubcausaeventoDTO> ListaJustificacion { get; set; }
        public List<MeJustificacionDTO> ListaMeJustificacion { get; set; }
        

    }
}