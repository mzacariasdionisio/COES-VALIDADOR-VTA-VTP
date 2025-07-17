using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class ValidacionEnvioModel: BaseModel
    {
        //Objetos del Modelo VariacionCodigoModel
        public VtpValidacionEnvioDTO Entidad { get; set; }
        public List<VtpValidacionEnvioDTO> ListaValidacionEnvio { get; set; }
        public bool Historica { get; set; }
        public bool EnergiaActiva { get; set; }
        public bool FactorPerdida { get; set; }
        public bool PrecioPotencia { get; set; }
        public bool PeajeUnitario { get; set; }
        public decimal PeajeUnitarioRegulado { get; set; }
        public decimal PrecioPotenciaRevision { get; set; }
    }
}