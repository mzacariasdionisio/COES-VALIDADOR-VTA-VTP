using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class DatosTransferenciaHelper : HelperBase
    {
        public DatosTransferenciaHelper() : base(Consultas.EntregaRetiroSql)
        {
        }

        public string SqlGrabarEntregaRetiro
        {
            get { return base.GetSqlXml("GrabarEntregaRetiro"); }
        }

        public string SqlProcesarInformacion
        {
            get { return base.GetSqlXml("ProcesarInformacion"); }
        }

        public string SqlObtenerCodigos
        {
            get { return base.GetSqlXml("ObtenerCodigosResultado"); }
        }

        public string Emprcodi = "EMPRCODI";
        public string Nrodia = "NRODIA";
        public string Tipodato = "TIPODATO";
        public string Codigobarra = "CODIGOBARRA";
        public string Promedio = "PROMEDIO";
        public string Sumadia = "SUMADIA";
        public string Indbarra = "INDBARRA";
        public string H1 = "H1";
        public string H2 = "H2";
        public string H3 = "H3";
        public string H4 = "H4";
        public string H5 = "H5";
        public string H6 = "H6";
        public string H7 = "H7";
        public string H8 = "H8";
        public string H9 = "H9";
        public string H10 = "H10";
        public string H11 = "H11";
        public string H12 = "H12";
        public string H13 = "H13";
        public string H14 = "H14";
        public string H15 = "H15";
        public string H16 = "H16";
        public string H17 = "H17";
        public string H18 = "H18";
        public string H19 = "H19";
        public string H20 = "H20";
        public string H21 = "H21";
        public string H22 = "H22";
        public string H23 = "H23";
        public string H24 = "H24";
        public string H25 = "H25";
        public string H26 = "H26";
        public string H27 = "H27";
        public string H28 = "H28";
        public string H29 = "H29";
        public string H30 = "H30";
        public string H31 = "H31";
        public string H32 = "H32";
        public string H33 = "H33";
        public string H34 = "H34";
        public string H35 = "H35";
        public string H36 = "H36";
        public string H37 = "H37";
        public string H38 = "H38";
        public string H39 = "H39";
        public string H40 = "H40";
        public string H41 = "H41";
        public string H42 = "H42";
        public string H43 = "H43";
        public string H44 = "H44";
        public string H45 = "H45";
        public string H46 = "H46";
        public string H47 = "H47";
        public string H48 = "H48";
        public string H49 = "H49";
        public string H50 = "H50";
        public string H51 = "H51";
        public string H52 = "H52";
        public string H53 = "H53";
        public string H54 = "H54";
        public string H55 = "H55";
        public string H56 = "H56";
        public string H57 = "H57";
        public string H58 = "H58";
        public string H59 = "H59";
        public string H60 = "H60";
        public string H61 = "H61";
        public string H62 = "H62";
        public string H63 = "H63";
        public string H64 = "H64";
        public string H65 = "H65";
        public string H66 = "H66";
        public string H67 = "H67";
        public string H68 = "H68";
        public string H69 = "H69";
        public string H70 = "H70";
        public string H71 = "H71";
        public string H72 = "H72";
        public string H73 = "H73";
        public string H74 = "H74";
        public string H75 = "H75";
        public string H76 = "H76";
        public string H77 = "H77";
        public string H78 = "H78";
        public string H79 = "H79";
        public string H80 = "H80";
        public string H81 = "H81";
        public string H82 = "H82";
        public string H83 = "H83";
        public string H84 = "H84";
        public string H85 = "H85";
        public string H86 = "H86";
        public string H87 = "H87";
        public string H88 = "H88";
        public string H89 = "H89";
        public string H90 = "H90";
        public string H91 = "H91";
        public string H92 = "H92";
        public string H93 = "H93";
        public string H94 = "H94";
        public string H95 = "H95";
        public string H96 = "H96";

        public string NombreTabla = "TRN_ENTREGA_RETIRO_TEMP";

        //ASSETEC 202001
        public string SqlProcesarInformacionEnvio
        {
            get { return base.GetSqlXml("ProcesarInformacionEnvio"); }
        }

        public string SqlDeleteListaEmpresas
        {
            get { return base.GetSqlXml("DeleteListaEmpresas"); }
        }

        public string SqlProcesarInformacionModeloEnvio
        {
            get { return base.GetSqlXml("ProcesarInformacionModeloEnvio"); }
        }

        public string SqlObtenerCodigosModelo
        {
            get { return base.GetSqlXml("ObtenerCodigosModelo"); }
        }
    }
}
