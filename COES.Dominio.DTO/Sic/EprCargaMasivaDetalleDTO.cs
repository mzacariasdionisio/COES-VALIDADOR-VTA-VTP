namespace COES.Dominio.DTO.Sic
{
    public class EprCargaMasivaDetalleDTO
    {
        public string Error {  get; set; }
        public string Item { get; set; }
        public string CodigoEmpresa { get; set; }
        public string CodigoCelda { get; set; }
        public string CodigoRele { get; set; }
        public string NombreRele { get; set; }
        public string Fecha { get; set; }
        public string Estado { get; set; }
        public string NivelTension { get; set; }
        public string SistemaRele { get; set; }
        public string Fabricante { get; set; }
        public string Modelo { get; set; }
        public string RTC_Ip { get; set; }
        public string RTC_Is { get; set; }
        public string RTT_Vp { get; set; }
        public string RTT_Vs { get; set; }

        public string ProteccionesCoordinables { get; set; }

        public string SincActivo { get; set; }
        public string SincInterruptor { get; set; }
        public string SincTension { get; set; }
        public string SincAngulo { get; set; }
        public string SincFrecuencia { get; set; }

        public string SobretActivo { get; set; }
        public string SobretInterruptor { get; set; }
        public string SobretTension { get; set; }
        public string SobretAngulo { get; set; }
        public string SobretFrecuencia { get; set; }

        public string SobreCorrienteActivo { get; set; }
        public string SobreCorrienteActivoDelta { get; set; }

        public string PmuActivo { get; set; }
        public string PmuAccion { get; set; }       
        public string Proyecto { get; set; }
        public string InterruptorComanda { get; set; }
        public string Mando { get; set; }

        public string MedidaMitigacion { get; set; }
        public string Implementado { get; set; }

        public string NombreUsuario { get; set; }
    }
}