using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class MeDemandaMercadoLibreHelper : HelperBase
    {
        public MeDemandaMercadoLibreHelper()
            : base(Consultas.MeDemandaMercadoLibreSql)
        {
        }

        
        #region Mapeo de Campos

        public string Dmelibcodi = "DMELIBCODI";
        public string Dmelibperiodo = "DMELIBPERIODO";
        public string Dmelibfecmaxdem = "DMELIBFECMAXDEM";
        //public string Rcdeuldemandahp = "RCDEULDEMANDAHP";
        //public string Rcdeuldemandahfp = "RCDEULDEMANDAHFP";
        public string Dmelibfuente = "DMELIBFUENTE";        
        public string Equicodi = "EQUICODI";
        //public string Equinomb = "EQUINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Razonsocial = "RAZONSOCIAL";

        //public string Rcdeulfeccreacion = "RCDEULFECCREACION";
        //public string Rcdeulusucreacion = "RCDEULUSUCREACION";
        
        public string DMELIBH1 = "DMELIBH1";
        public string DMELIBH2 = "DMELIBH2";
        public string DMELIBH3 = "DMELIBH3";
        public string DMELIBH4 = "DMELIBH4";
        public string DMELIBH5 = "DMELIBH5";
        public string DMELIBH6 = "DMELIBH6";
        public string DMELIBH7 = "DMELIBH7";
        public string DMELIBH8 = "DMELIBH8";
        public string DMELIBH9 = "DMELIBH9";
        public string DMELIBH10 = "DMELIBH10";
        public string DMELIBH11 = "DMELIBH11";
        public string DMELIBH12 = "DMELIBH12";
        public string DMELIBH13 = "DMELIBH13";
        public string DMELIBH14 = "DMELIBH14";
        public string DMELIBH15 = "DMELIBH15";
        public string DMELIBH16 = "DMELIBH16";
        public string DMELIBH17 = "DMELIBH17";
        public string DMELIBH18 = "DMELIBH18";
        public string DMELIBH19 = "DMELIBH19";
        public string DMELIBH20 = "DMELIBH20";
        public string DMELIBH21 = "DMELIBH21";
        public string DMELIBH22 = "DMELIBH22";
        public string DMELIBH23 = "DMELIBH23";
        public string DMELIBH24 = "DMELIBH24";
        public string DMELIBH25 = "DMELIBH25";
        public string DMELIBH26 = "DMELIBH26";
        public string DMELIBH27 = "DMELIBH27";
        public string DMELIBH28 = "DMELIBH28";
        public string DMELIBH29 = "DMELIBH29";
        public string DMELIBH30 = "DMELIBH30";
        public string DMELIBH31 = "DMELIBH31";
        public string DMELIBH32 = "DMELIBH32";
        public string DMELIBH33 = "DMELIBH33";
        public string DMELIBH34 = "DMELIBH34";
        public string DMELIBH35 = "DMELIBH35";
        public string DMELIBH36 = "DMELIBH36";
        public string DMELIBH37 = "DMELIBH37";
        public string DMELIBH38 = "DMELIBH38";
        public string DMELIBH39 = "DMELIBH39";
        public string DMELIBH40 = "DMELIBH40";
        public string DMELIBH41 = "DMELIBH41";
        public string DMELIBH42 = "DMELIBH42";
        public string DMELIBH43 = "DMELIBH43";
        public string DMELIBH44 = "DMELIBH44";
        public string DMELIBH45 = "DMELIBH45";
        public string DMELIBH46 = "DMELIBH46";
        public string DMELIBH47 = "DMELIBH47";
        public string DMELIBH48 = "DMELIBH48";
        public string DMELIBH49 = "DMELIBH49";
        public string DMELIBH50 = "DMELIBH50";
        public string DMELIBH51 = "DMELIBH51";
        public string DMELIBH52 = "DMELIBH52";
        public string DMELIBH53 = "DMELIBH53";
        public string DMELIBH54 = "DMELIBH54";
        public string DMELIBH55 = "DMELIBH55";
        public string DMELIBH56 = "DMELIBH56";
        public string DMELIBH57 = "DMELIBH57";
        public string DMELIBH58 = "DMELIBH58";
        public string DMELIBH59 = "DMELIBH59";
        public string DMELIBH60 = "DMELIBH60";
        public string DMELIBH61 = "DMELIBH61";
        public string DMELIBH62 = "DMELIBH62";
        public string DMELIBH63 = "DMELIBH63";
        public string DMELIBH64 = "DMELIBH64";
        public string DMELIBH65 = "DMELIBH65";
        public string DMELIBH66 = "DMELIBH66";
        public string DMELIBH67 = "DMELIBH67";
        public string DMELIBH68 = "DMELIBH68";
        public string DMELIBH69 = "DMELIBH69";
        public string DMELIBH70 = "DMELIBH70";
        public string DMELIBH71 = "DMELIBH71";
        public string DMELIBH72 = "DMELIBH72";
        public string DMELIBH73 = "DMELIBH73";
        public string DMELIBH74 = "DMELIBH74";
        public string DMELIBH75 = "DMELIBH75";
        public string DMELIBH76 = "DMELIBH76";
        public string DMELIBH77 = "DMELIBH77";
        public string DMELIBH78 = "DMELIBH78";
        public string DMELIBH79 = "DMELIBH79";
        public string DMELIBH80 = "DMELIBH80";
        public string DMELIBH81 = "DMELIBH81";
        public string DMELIBH82 = "DMELIBH82";
        public string DMELIBH83 = "DMELIBH83";
        public string DMELIBH84 = "DMELIBH84";
        public string DMELIBH85 = "DMELIBH85";
        public string DMELIBH86 = "DMELIBH86";
        public string DMELIBH87 = "DMELIBH87";
        public string DMELIBH88 = "DMELIBH88";
        public string DMELIBH89 = "DMELIBH89";
        public string DMELIBH90 = "DMELIBH90";
        public string DMELIBH91 = "DMELIBH91";
        public string DMELIBH92 = "DMELIBH92";
        public string DMELIBH93 = "DMELIBH93";
        public string DMELIBH94 = "DMELIBH94";
        public string DMELIBH95 = "DMELIBH95";
        public string DMELIBH96 = "DMELIBH96";


        /// <summary>
        /// Aplicativo PR16
        /// </summary>      
        public string Osinergcodi = "OSINERGCODI";
        public string Item = "ITEM";
        //public string Periodo = "PERIODO";
        //public string IniRemision = "INI_REMISION";        
        public string Suministrador = "SUMINISTRADOR";
        public string Emprruc = "EMPRRUC";
        public string Nombresicli = "NOMBRE_SICLI";
        public string Areanomb = "AREANOMB";
        public string Equitension = "EQUITENSION";        
       
        public string Qregistros = "Q_REGISTROS";
               
        #endregion


        public string SqlObtenerReporteDemandaMercadoLibre
        {
            get { return base.GetSqlXml("ObtenerReporteDemandaMercadoLibre"); }
        }
        public string SqlObtenerReporteDemandaMercadoLibreCount
        {
            get { return base.GetSqlXml("ObtenerReporteDemandaMercadoLibreCount"); }
        }
        public string SqlObtenerReporteDemandaMercadoLibreExcel
        {
            get { return base.GetSqlXml("ObtenerReporteDemandaMercadoLibreExcel"); }
        }
        public string SqlObtenerPeriodoSicli
        {
            get { return base.GetSqlXml("ObtenerPeriodoSicli"); }
        }

        public string SqlUpdatePeriodoDemandaSicli
        {
            get { return base.GetSqlXml("UpdatePeriodoDemandaSicli"); }
        }
        
    }
}
