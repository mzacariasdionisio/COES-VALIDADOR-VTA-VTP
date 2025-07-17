using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Models
{
    public class ValorTransfPCModel : BaseModel
    {
        //Objetos del Modelo 
        public List<VtpPeajeIngresoDTO> ListaPeajeIngresoCargo { get; set; }
        public List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioPago { get; set; }
        public List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro { get; set; }
        public List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaPago { get; set; }
        public List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro { get; set; }
        public List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC { get; set; }
        public List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoEmpresa { get; set; }
        public List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgreso { get; set; }
        public List<IngresoRetiroSCDTO> ListaFactorProporcion { get; set; }
        public VtpPeajeCargoDTO EntidadPeajeCargo { get; set; }
        public List<VtpPeajeCargoDTO> ListaPeajeCargo { get; set; }
        public List<VtpPeajeCargoDTO> ListaPeajeCargoEmpresa { get; set; }
        public List<VtpRepaRecaPeajeDetalleDTO> ListaRepartosEmpresa { get; set; }
        public List<VtpPeajeSaldoTransmisionDTO> ListaPeajeSaldoTransmision { get; set; }
        public VtpPeajeSaldoTransmisionDTO EntidadPeajeSaldoTransmision { get; set; }
        public List<IngresoPotenciaDTO> ListaIngresoPotencia { get; set; }
        public List<VtpIngresoPotefrDTO> ListaIngresoPotEFR { get; set; }
        public List<VtpIngresoPotefrDetalleDTO> ListaIngresoPotEFRDetalle { get; set; }
        public List<VtpIngresoPotUnidIntervlDTO> ListaIngresoPotenciaUnidad { get; set; }
        public List<VtpIngresoPotUnidPromdDTO> ListaIngresoPotenciaEmpresa { get; set; }
        public List<VtpSaldoEmpresaDTO> ListaSaldoEmpresa { get; set; }
        public List<VtpSaldoEmpresaDTO> ListaSaldoEmpresaPositiva { get; set; }
        public List<VtpSaldoEmpresaDTO> ListaSaldoEmpresaNegativa { get; set; }
        public List<VtpEmpresaPagoDTO> ListaEmpresaPago { get; set; }
        public List<VtpEmpresaPagoDTO> ListaEmpresaCobro { get; set; }
        public List<VtpPeajeSaldoTransmisionDTO> ListaEmpresaEgreso { get; set; }
        public List<VtpIngresoPotenciaDTO> ListaVtpIngresoPotencia { get; set; }

    }
}
