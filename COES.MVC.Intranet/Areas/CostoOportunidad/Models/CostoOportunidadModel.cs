using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Models
{
    public class CostoOportunidadModel
    {
        public string Resultado { get; set; }
        public string Fecha { get; set; }        
        public int Retorno { get; set; }
        public int IdEnvio { get; set; }
        public int IdEnvioLast { get; set; }        
        public HandsonModel Handson { get; set; }
        public short[][] MatrizExcelColores { get; set; }

        public int FilasCabecera { get; set; }
        public int ColumnasCabecera { get; set; }
        public List<TipoDatos> ListaHojaPto { get; set; }
        public List<EveMailsDTO> ListaReprograma { get; set; }
        public List<PrGrupodatDTO> ListaPotenciaEfec { get; set; }
    }

    public class TipoDatos
    {
        public int Codi { get; set; }
        public string DetName { get; set; }

    }

    public class FormatResultado
    {
        public int Resultado { get; set; }
        public int IdEnvio { get; set; }
        public string Mensaje { get; set; }
    }
}