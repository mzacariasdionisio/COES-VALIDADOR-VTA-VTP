using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class EprCargaMasivaLineaDTO
    {
        
        public string Item {  get; set; }
        public string Codigo { get; set; }
        public string Ubicacion { get; set; }

        public string CapacidadA { get; set; }

        public string Celda { get; set; }

        public string Celda2 { get; set; }

        public string BancoCondensador { get; set; }

        public string BancoCapacidadA { get; set; }
        public string BancoCapacidadMVAr { get; set; }

        public string CapacTransCond1Porcen { get; set; }
        public string CapacTransCond1Min { get; set; }
        public string CapacTransCond1A { get; set; }
        public string CapacTransCond2Porcen { get; set; }
        public string CapacTransCond2Min { get; set; }
        public string CapacTransCond2A { get; set; }
        public string LimiteSegCoes { get; set; }

        public string Observaciones { get; set; }
        public string Motivo { get; set; }
        public string NombreUsuario { get; set; }

        public string ComentarioCodigo { get; set; }
        public string ComentarioUbicacion { get; set; }
        public string ComentarioCapacidadA { get; set; }            
        public string ComentarioCelda { get; set; }
        public string ComentarioCelda2 { get; set; }
        public string ComentarioBancoCondensador { get; set; }

        public string ComentarioBancoCapacidadA { get; set; }
        public string ComentarioBancoCapacidadMVAr { get; set; }

        public string ComentarioCapacTransCond1Porcen { get; set; }
        public string ComentarioCapacTransCond1Min { get; set; }
        public string ComentarioCapacTransCond1A { get; set; }
        public string ComentarioCapacTransCond2Porcen { get; set; }
        public string ComentarioCapacTransCond2Min { get; set; }
        public string ComentarioCapacTransCond2A { get; set; }
        public string ComentarioLimiteSegCoes { get; set; }

        public string ComentarioObservaciones { get; set; }
        public string ComentarioMotivo { get; set; }

        public string Error {  get; set; }

        public string CapacidadMvar { get; set; }
        public string ComentarioCapacidadMvar { get; set; }
    }

    public class EprCargaMasivaCeldaAcoplamientoDTO
    {
        public string Item { get; set; }
        public string Codigo { get; set; }
        public string CodigoInterruptorAcoplamiento { get; set; }
        public string CapacidadInterruptorAcoplamiento { get; set; }
        public string Observaciones { get; set; }
        public string Motivo { get; set; }
        public string NombreUsuario { get; set; }
        public string Error { get; set; }
        public string ComentarioCodigo { get; set; }
        public string ComentarioCodigoInterruptorAcoplamiento { get; set; }
        public string ComentarioCapacidadInterruptorAcoplamiento { get; set; }
        public string ComentarioObservaciones { get; set; }
        public string ComentarioMotivo { get; set; }

    }

    public class EprCargaMasivaTransformadorDTO
    {

        public string Item { get; set; }
        public string Codigo { get; set; }
        public string DevanadoCodigo { get; set; }
        public string DevanadoCapacidadONAN { get; set; }
        public string DevanadoCapacidadONAF { get; set; }

        public string DevanadoDosCodigo { get; set; }
        public string DevanadoDosCapacidadONAN { get; set; }
        public string DevanadoDosCapacidadONAF { get; set; }

        public string DevanadoTresCodigo { get; set; }
        public string DevanadoTresCapacidadONAN { get; set; }
        public string DevanadoTresCapacidadONAF { get; set; }

        public string DevanadoCuatroCodigo { get; set; }
        public string DevanadoCuatroCapacidadONAN { get; set; }
        public string DevanadoCuatroCapacidadONAF { get; set; }

        public string Observaciones { get; set; }
        public string Motivo { get; set; }
        public string NombreUsuario { get; set; }

        public string ComentarioCodigo { get; set; }
        public string ComentarioDevanadoCodigo { get; set; }
        public string ComentarioDevanadoCapacidadONAN { get; set; }
        public string ComentarioDevanadoCapacidadONAF { get; set; }

        public string ComentarioDevanadoDosCodigo { get; set; }
        public string ComentarioDevanadoDosCapacidadONAN { get; set; }
        public string ComentarioDevanadoDosCapacidadONAF { get; set; }

        public string ComentarioDevanadoTresCodigo { get; set; }
        public string ComentarioDevanadoTresCapacidadONAN { get; set; }
        public string ComentarioDevanadoTresCapacidadONAF { get; set; }

        public string ComentarioDevanadoCuatroCodigo { get; set; }
        public string ComentarioDevanadoCuatroCapacidadONAN { get; set; }
        public string ComentarioDevanadoCuatroCapacidadONAF { get; set; }

        public string ComentarioObservaciones { get; set; }
        public string ComentarioMotivo { get; set; }

        public string Error { get; set; }

      
    }
}
