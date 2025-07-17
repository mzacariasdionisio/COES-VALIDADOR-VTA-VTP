using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Helper
{
    public class ConstantesFormato
    {
        public const string SesionNombreArchivo = "SesionNombreArchivo";
        public const string SesionMatrizExcel = "MatrizExcel";
        public const string FolderUpload = "Uploads\\";
        public const string ExtensionFile = "xlsx";
        public const char SeparadorFila = '#';
        public const char SeparadorCol = ',';
        public const int ColEmpresaExtranet = 1500;
        public const int ColFormatoExtranet = 1501;
        public const int ColEmpresa = 150;
        public const int ColFormato = 151;
        public const int ColExcelData = 2;
        public const int FilaExcelData = 13;
        public const int SubcausacodiRestric = 205;
        public const string SesionListaMatrizExcel = "ListaMatrizExcel";
        public const string SesionListaHoja = "ListaHoja";

        public const int VerUltimoEnvio = 1;
        public const int NoVerUltimoEnvio = 0;

        //Nueva version 
        public const int ColFormatoExtranetNuevo = 2;
        public const int ColEmpresaExtranetNuevo = 1;

        public const int ColExcelDataNuevo = 1;
        public const int FilaExcelDataNuevo = 9;
    }
}