using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Helper
{
    public class ConstantesTransferencia
    {
        public const string FormatoFecha = "dd/MM/yyyy";
        public const string FormatoMes = "MM yyyy";
        public const int FormatoPotenciaMax = 93;
        public const int CabeceraPotenciaMax = 41;
        public const string FormatoFechaCorto = "yyyy-MM-dd";
        public const string NombreExcel = "Consumos no autorizados";
        public const string HojaFormatoExcel = "CNA";
        public const int ColExcelData = 2;
        public const string ReporteCna = "ReporteCna";
        public const string AppExcel = "application/vnd.ms-excel";
        public const string FolderReporte = "Areas\\Transferencias\\Reporte\\";
        public const int Plantillacorreo = 165;
        public const int AccesoEmpresa = 13;
    }

    public class ConstantesGestionCodigosVTEAVTP
    {
        public const int IdRolCodigoVTEA = 80;
        public const int IdRolCodigoVTP = 96;



        public const string Activo = "ACT";
        public const string Baja = "BAJ";
        public const string Rechazado = "REC";
        public const string Pendiente = "PAP";
        public const string PendienteAprobacionVTP = "PVT";
        public const string SolicitudBaja = "SBJ";



    }

    public class ConstantesEstadoSolicitudCambio
    {
        public const string Pendiente = "PEN";
        public const string Aprobado = "APR";
        public const string Rechazado = "REC";

    }

    public enum EnumResultado
    {
        correcto = 1,
        error = -1
    }

    public class ConstanteEstados
    {
        public const string Activo = "ACT";
        public const string Baja = "BAJ";
        public const string Rechazado = "REC";
        public const string Pendiente = "PAP";
        public const string PendienteAprobacionVTP = "PVT";
        public const string SolicitudBaja = "SBJ";
        public const string Inactivo = "INA";
    }
}