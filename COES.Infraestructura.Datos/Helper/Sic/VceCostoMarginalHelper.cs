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
    public class VceCostoMarginalHelper : HelperBase
    {
        public VceCostoMarginalHelper() : base(Consultas.VceCostoMarginalSql)
        {
        }

       

        #region Mapeo de Campos

        public string PecaCodi = "PECACODI";
        public string VenePtomedicodi = "VENEPTOMEDICODI";
        public string VeneFecha = "VENEFECHA";
        public string VeneMeditotal = "VENEMEDITOTAL";
        public string VeneH1 = "VENEH1";
        public string VeneH2 = "VENEH2";
        public string VeneH3 = "VENEH3";
        public string VeneH4 = "VENEH4";
        public string VeneH5 = "VENEH5";
        public string VeneH6 = "VENEH6";
        public string VeneH7 = "VENEH7";
        public string VeneH8 = "VENEH8";
        public string VeneH9 = "VENEH9";
        public string VeneH10 = "VENEH10";
        public string VeneH11 = "VENEH11";
        public string VeneH12 = "VENEH12";
        public string VeneH13 = "VENEH13";
        public string VeneH14 = "VENEH14";
        public string VeneH15 = "VENEH15";
        public string VeneH16 = "VENEH16";
        public string VeneH17 = "VENEH17";
        public string VeneH18 = "VENEH18";
        public string VeneH19 = "VENEH19";
        public string VeneH20 = "VENEH20";
        public string VeneH21 = "VENEH21";
        public string VeneH22 = "VENEH22";
        public string VeneH23 = "VENEH23";
        public string VeneH24 = "VENEH24";
        public string VeneH25 = "VENEH25";
        public string VeneH26 = "VENEH26";
        public string VeneH27 = "VENEH27";
        public string VeneH28 = "VENEH28";
        public string VeneH29 = "VENEH29";
        public string VeneH30 = "VENEH30";
        public string VeneH31 = "VENEH31";
        public string VeneH32 = "VENEH32";
        public string VeneH33 = "VENEH33";
        public string VeneH34 = "VENEH34";
        public string VeneH35 = "VENEH35";
        public string VeneH36 = "VENEH36";
        public string VeneH37 = "VENEH37";
        public string VeneH38 = "VENEH38";
        public string VeneH39 = "VENEH39";
        public string VeneH40 = "VENEH40";
        public string VeneH41 = "VENEH41";
        public string VeneH42 = "VENEH42";
        public string VeneH43 = "VENEH43";
        public string VeneH44 = "VENEH44";
        public string VeneH45 = "VENEH45";
        public string VeneH46 = "VENEH46";
        public string VeneH47 = "VENEH47";
        public string VeneH48 = "VENEH48";
        public string VeneH49 = "VENEH49";
        public string VeneH50 = "VENEH50";
        public string VeneH51 = "VENEH51";
        public string VeneH52 = "VENEH52";
        public string VeneH53 = "VENEH53";
        public string VeneH54 = "VENEH54";
        public string VeneH55 = "VENEH55";
        public string VeneH56 = "VENEH56";
        public string VeneH57 = "VENEH57";
        public string VeneH58 = "VENEH58";
        public string VeneH59 = "VENEH59";
        public string VeneH60 = "VENEH60";
        public string VeneH61 = "VENEH61";
        public string VeneH62 = "VENEH62";
        public string VeneH63 = "VENEH63";
        public string VeneH64 = "VENEH64";
        public string VeneH65 = "VENEH65";
        public string VeneH66 = "VENEH66";
        public string VeneH67 = "VENEH67";
        public string VeneH68 = "VENEH68";
        public string VeneH69 = "VENEH69";
        public string VeneH70 = "VENEH70";
        public string VeneH71 = "VENEH71";
        public string VeneH72 = "VENEH72";
        public string VeneH73 = "VENEH73";
        public string VeneH74 = "VENEH74";
        public string VeneH75 = "VENEH75";
        public string VeneH76 = "VENEH76";
        public string VeneH77 = "VENEH77";
        public string VeneH78 = "VENEH78";
        public string VeneH79 = "VENEH79";
        public string VeneH80 = "VENEH80";
        public string VeneH81 = "VENEH81";
        public string VeneH82 = "VENEH82";
        public string VeneH83 = "VENEH83";
        public string VeneH84 = "VENEH84";
        public string VeneH85 = "VENEH85";
        public string VeneH86 = "VENEH86";
        public string VeneH87 = "VENEH87";
        public string VeneH88 = "VENEH88";
        public string VeneH89 = "VENEH89";
        public string VeneH90 = "VENEH90";
        public string VeneH91 = "VENEH91";
        public string VeneH92 = "VENEH92";
        public string VeneH93 = "VENEH93";
        public string VeneH94 = "VENEH94";
        public string VeneH95 = "VENEH95";
        public string VeneH96 = "VENEH96";

        #endregion


        public string SqlListarBarras
        {
            get { return base.GetSqlXml("ListarBarras"); }
        }

    }
}
