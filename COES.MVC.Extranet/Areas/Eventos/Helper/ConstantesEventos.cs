using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Eventos.Helper
{
    /// <summary>
    /// Constantes usadas en el módulo
    /// </summary>
    public class ConstantesEventos
    {
        public const char SeparadorFila = '#';
        public const char SeparadorCol = ',';
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;
        public const int FilaExcelData = 13;
        public const int LectcodiPruebaUnidad = 106;
        public const int IdFormatoPruebaUnidad = 84;
        public const int OriglectcodiDespacho = 1;
        public const int TipoinfocodiMw = 1;
        public const int EmprcodiCoes = 1;

    }


    public class ParamFormatoEvento
    {
        public const int RowTitulo = 6;
        public const int RowCodigo = 1;
        public const int RowArea = 4;
        public const int ColDatos = 2;
        public const int ColFecha = 1;
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;
    }
    public class ConstantesEnviarCorreo
    {
        //Carpetas de informes SCO
        public const string CarpetaInformeFallaN1 = "InformedePerturbaciones";
        public const string CarpetaInformeFallaN2 = "InformedePerturbacionesN2";
        public const string CarpetaInformeMinisterio = "InformeMinisterio";
    }

    }