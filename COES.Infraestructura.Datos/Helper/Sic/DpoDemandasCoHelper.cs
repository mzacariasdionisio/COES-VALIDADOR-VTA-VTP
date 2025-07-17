using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoDemandaScoHelper : HelperBase
    {
        public DpoDemandaScoHelper() : base(Consultas.DpoDemandaScoSql)
        {
        }

        public DpoDemandaScoDTO Create(IDataReader dr)
        {
            DpoDemandaScoDTO entity = new DpoDemandaScoDTO();

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            int iPrnvarcodi = dr.GetOrdinal(this.Prnvarcodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

            int iMeditotal = dr.GetOrdinal(this.Meditotal);
            if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

            int ih1 = dr.GetOrdinal(this.h1);
            if (!dr.IsDBNull(ih1)) entity.H1 = dr.GetDecimal(ih1);

            int ih2 = dr.GetOrdinal(this.h2);
            if (!dr.IsDBNull(ih2)) entity.H2 = dr.GetDecimal(ih2);

            int ih3 = dr.GetOrdinal(this.h3);
            if (!dr.IsDBNull(ih3)) entity.H3 = dr.GetDecimal(ih3);

            int ih4 = dr.GetOrdinal(this.h4);
            if (!dr.IsDBNull(ih4)) entity.H4 = dr.GetDecimal(ih4);

            int ih5 = dr.GetOrdinal(this.h5);
            if (!dr.IsDBNull(ih5)) entity.H5 = dr.GetDecimal(ih5);

            int ih6 = dr.GetOrdinal(this.h6);
            if (!dr.IsDBNull(ih6)) entity.H6 = dr.GetDecimal(ih6);

            int ih7 = dr.GetOrdinal(this.h7);
            if (!dr.IsDBNull(ih7)) entity.H7 = dr.GetDecimal(ih7);

            int ih8 = dr.GetOrdinal(this.h8);
            if (!dr.IsDBNull(ih8)) entity.H8 = dr.GetDecimal(ih8);

            int ih9 = dr.GetOrdinal(this.h9);
            if (!dr.IsDBNull(ih9)) entity.H9 = dr.GetDecimal(ih9);

            int ih10 = dr.GetOrdinal(this.h10);
            if (!dr.IsDBNull(ih10)) entity.H10 = dr.GetDecimal(ih10);

            int ih11 = dr.GetOrdinal(this.h11);
            if (!dr.IsDBNull(ih11)) entity.H11 = dr.GetDecimal(ih11);

            int ih12 = dr.GetOrdinal(this.h12);
            if (!dr.IsDBNull(ih12)) entity.H12 = dr.GetDecimal(ih12);

            int ih13 = dr.GetOrdinal(this.h13);
            if (!dr.IsDBNull(ih13)) entity.H13 = dr.GetDecimal(ih13);

            int ih14 = dr.GetOrdinal(this.h14);
            if (!dr.IsDBNull(ih14)) entity.H14 = dr.GetDecimal(ih14);

            int ih15 = dr.GetOrdinal(this.h15);
            if (!dr.IsDBNull(ih15)) entity.H15 = dr.GetDecimal(ih15);

            int ih16 = dr.GetOrdinal(this.h16);
            if (!dr.IsDBNull(ih16)) entity.H16 = dr.GetDecimal(ih16);

            int ih17 = dr.GetOrdinal(this.h17);
            if (!dr.IsDBNull(ih17)) entity.H17 = dr.GetDecimal(ih17);

            int ih18 = dr.GetOrdinal(this.h18);
            if (!dr.IsDBNull(ih18)) entity.H18 = dr.GetDecimal(ih18);

            int ih19 = dr.GetOrdinal(this.h19);
            if (!dr.IsDBNull(ih19)) entity.H19 = dr.GetDecimal(ih19);

            int ih20 = dr.GetOrdinal(this.h20);
            if (!dr.IsDBNull(ih20)) entity.H20 = dr.GetDecimal(ih20);

            int ih21 = dr.GetOrdinal(this.h21);
            if (!dr.IsDBNull(ih21)) entity.H21 = dr.GetDecimal(ih21);

            int ih22 = dr.GetOrdinal(this.h22);
            if (!dr.IsDBNull(ih22)) entity.H22 = dr.GetDecimal(ih22);

            int ih23 = dr.GetOrdinal(this.h23);
            if (!dr.IsDBNull(ih23)) entity.H23 = dr.GetDecimal(ih23);

            int ih24 = dr.GetOrdinal(this.h24);
            if (!dr.IsDBNull(ih24)) entity.H24 = dr.GetDecimal(ih24);

            int ih25 = dr.GetOrdinal(this.h25);
            if (!dr.IsDBNull(ih25)) entity.H25 = dr.GetDecimal(ih25);

            int ih26 = dr.GetOrdinal(this.h26);
            if (!dr.IsDBNull(ih26)) entity.H26 = dr.GetDecimal(ih26);

            int ih27 = dr.GetOrdinal(this.h27);
            if (!dr.IsDBNull(ih27)) entity.H27 = dr.GetDecimal(ih27);

            int ih28 = dr.GetOrdinal(this.h28);
            if (!dr.IsDBNull(ih28)) entity.H28 = dr.GetDecimal(ih28);

            int ih29 = dr.GetOrdinal(this.h29);
            if (!dr.IsDBNull(ih29)) entity.H29 = dr.GetDecimal(ih29);

            int ih30 = dr.GetOrdinal(this.h30);
            if (!dr.IsDBNull(ih30)) entity.H30 = dr.GetDecimal(ih30);

            int ih31 = dr.GetOrdinal(this.h31);
            if (!dr.IsDBNull(ih31)) entity.H31 = dr.GetDecimal(ih31);

            int ih32 = dr.GetOrdinal(this.h32);
            if (!dr.IsDBNull(ih32)) entity.H32 = dr.GetDecimal(ih32);

            int ih33 = dr.GetOrdinal(this.h33);
            if (!dr.IsDBNull(ih33)) entity.H33 = dr.GetDecimal(ih33);

            int ih34 = dr.GetOrdinal(this.h34);
            if (!dr.IsDBNull(ih34)) entity.H34 = dr.GetDecimal(ih34);

            int ih35 = dr.GetOrdinal(this.h35);
            if (!dr.IsDBNull(ih35)) entity.H35 = dr.GetDecimal(ih35);

            int ih36 = dr.GetOrdinal(this.h36);
            if (!dr.IsDBNull(ih36)) entity.H36 = dr.GetDecimal(ih36);

            int ih37 = dr.GetOrdinal(this.h37);
            if (!dr.IsDBNull(ih37)) entity.H37 = dr.GetDecimal(ih37);

            int ih38 = dr.GetOrdinal(this.h38);
            if (!dr.IsDBNull(ih38)) entity.H38 = dr.GetDecimal(ih38);

            int ih39 = dr.GetOrdinal(this.h39);
            if (!dr.IsDBNull(ih39)) entity.H39 = dr.GetDecimal(ih39);

            int ih40 = dr.GetOrdinal(this.h40);
            if (!dr.IsDBNull(ih40)) entity.H40 = dr.GetDecimal(ih40);

            int ih41 = dr.GetOrdinal(this.h41);
            if (!dr.IsDBNull(ih41)) entity.H41 = dr.GetDecimal(ih41);

            int ih42 = dr.GetOrdinal(this.h42);
            if (!dr.IsDBNull(ih42)) entity.H42 = dr.GetDecimal(ih42);

            int ih43 = dr.GetOrdinal(this.h43);
            if (!dr.IsDBNull(ih43)) entity.H43 = dr.GetDecimal(ih43);

            int ih44 = dr.GetOrdinal(this.h44);
            if (!dr.IsDBNull(ih44)) entity.H44 = dr.GetDecimal(ih44);

            int ih45 = dr.GetOrdinal(this.h45);
            if (!dr.IsDBNull(ih45)) entity.H45 = dr.GetDecimal(ih45);

            int ih46 = dr.GetOrdinal(this.h46);
            if (!dr.IsDBNull(ih46)) entity.H46 = dr.GetDecimal(ih46);

            int ih47 = dr.GetOrdinal(this.h47);
            if (!dr.IsDBNull(ih47)) entity.H47 = dr.GetDecimal(ih47);

            int ih48 = dr.GetOrdinal(this.h48);
            if (!dr.IsDBNull(ih48)) entity.H48 = dr.GetDecimal(ih48);

            int ih49 = dr.GetOrdinal(this.h49);
            if (!dr.IsDBNull(ih49)) entity.H49 = dr.GetDecimal(ih49);

            int ih50 = dr.GetOrdinal(this.h50);
            if (!dr.IsDBNull(ih50)) entity.H50 = dr.GetDecimal(ih50);

            int ih51 = dr.GetOrdinal(this.h51);
            if (!dr.IsDBNull(ih51)) entity.H51 = dr.GetDecimal(ih51);

            int ih52 = dr.GetOrdinal(this.h52);
            if (!dr.IsDBNull(ih50)) entity.H52 = dr.GetDecimal(ih52);

            int ih53 = dr.GetOrdinal(this.h53);
            if (!dr.IsDBNull(ih53)) entity.H53 = dr.GetDecimal(ih53);

            int ih54 = dr.GetOrdinal(this.h54);
            if (!dr.IsDBNull(ih54)) entity.H54 = dr.GetDecimal(ih54);

            int ih55 = dr.GetOrdinal(this.h55);
            if (!dr.IsDBNull(ih55)) entity.H55 = dr.GetDecimal(ih55);

            int ih56 = dr.GetOrdinal(this.h56);
            if (!dr.IsDBNull(ih56)) entity.H56 = dr.GetDecimal(ih56);

            int ih57 = dr.GetOrdinal(this.h57);
            if (!dr.IsDBNull(ih57)) entity.H57 = dr.GetDecimal(ih57);

            int ih58 = dr.GetOrdinal(this.h58);
            if (!dr.IsDBNull(ih58)) entity.H58 = dr.GetDecimal(ih58);

            int ih59 = dr.GetOrdinal(this.h59);
            if (!dr.IsDBNull(ih59)) entity.H59 = dr.GetDecimal(ih59);

            int ih60 = dr.GetOrdinal(this.h60);
            if (!dr.IsDBNull(ih60)) entity.H60 = dr.GetDecimal(ih60);

            int ih61 = dr.GetOrdinal(this.h61);
            if (!dr.IsDBNull(ih61)) entity.H61 = dr.GetDecimal(ih61);

            int ih62 = dr.GetOrdinal(this.h62);
            if (!dr.IsDBNull(ih62)) entity.H62 = dr.GetDecimal(ih62);

            int ih63 = dr.GetOrdinal(this.h63);
            if (!dr.IsDBNull(ih63)) entity.H63 = dr.GetDecimal(ih63);

            int ih64 = dr.GetOrdinal(this.h64);
            if (!dr.IsDBNull(ih64)) entity.H64 = dr.GetDecimal(ih64);

            int ih65 = dr.GetOrdinal(this.h65);
            if (!dr.IsDBNull(ih65)) entity.H65 = dr.GetDecimal(ih65);

            int ih66 = dr.GetOrdinal(this.h66);
            if (!dr.IsDBNull(ih66)) entity.H66 = dr.GetDecimal(ih66);

            int ih67 = dr.GetOrdinal(this.h67);
            if (!dr.IsDBNull(ih67)) entity.H67 = dr.GetDecimal(ih67);

            int ih68 = dr.GetOrdinal(this.h68);
            if (!dr.IsDBNull(ih68)) entity.H68 = dr.GetDecimal(ih68);

            int ih69 = dr.GetOrdinal(this.h69);
            if (!dr.IsDBNull(ih69)) entity.H69 = dr.GetDecimal(ih69);

            int ih70 = dr.GetOrdinal(this.h70);
            if (!dr.IsDBNull(ih70)) entity.H70 = dr.GetDecimal(ih70);

            int ih71 = dr.GetOrdinal(this.h71);
            if (!dr.IsDBNull(ih70)) entity.H71 = dr.GetDecimal(ih71);

            int ih72 = dr.GetOrdinal(this.h72);
            if (!dr.IsDBNull(ih72)) entity.H72 = dr.GetDecimal(ih72);

            int ih73 = dr.GetOrdinal(this.h73);
            if (!dr.IsDBNull(ih73)) entity.H73 = dr.GetDecimal(ih73);

            int ih74 = dr.GetOrdinal(this.h74);
            if (!dr.IsDBNull(ih74)) entity.H74 = dr.GetDecimal(ih74);

            int ih75 = dr.GetOrdinal(this.h75);
            if (!dr.IsDBNull(ih75)) entity.H75 = dr.GetDecimal(ih75);

            int ih76 = dr.GetOrdinal(this.h76);
            if (!dr.IsDBNull(ih76)) entity.H76 = dr.GetDecimal(ih76);

            int ih77 = dr.GetOrdinal(this.h77);
            if (!dr.IsDBNull(ih77)) entity.H77 = dr.GetDecimal(ih77);

            int ih78 = dr.GetOrdinal(this.h78);
            if (!dr.IsDBNull(ih78)) entity.H78 = dr.GetDecimal(ih78);

            int ih79 = dr.GetOrdinal(this.h79);
            if (!dr.IsDBNull(ih79)) entity.H79 = dr.GetDecimal(ih79);

            int ih80 = dr.GetOrdinal(this.h80);
            if (!dr.IsDBNull(ih80)) entity.H80 = dr.GetDecimal(ih80);

            int ih81 = dr.GetOrdinal(this.h81);
            if (!dr.IsDBNull(ih81)) entity.H81 = dr.GetDecimal(ih81);

            int ih82 = dr.GetOrdinal(this.h82);
            if (!dr.IsDBNull(ih82)) entity.H82 = dr.GetDecimal(ih82);

            int ih83 = dr.GetOrdinal(this.h83);
            if (!dr.IsDBNull(ih83)) entity.H83 = dr.GetDecimal(ih83);

            int ih84 = dr.GetOrdinal(this.h84);
            if (!dr.IsDBNull(ih84)) entity.H84 = dr.GetDecimal(ih84);

            int ih85 = dr.GetOrdinal(this.h85);
            if (!dr.IsDBNull(ih85)) entity.H85 = dr.GetDecimal(ih85);

            int ih86 = dr.GetOrdinal(this.h86);
            if (!dr.IsDBNull(ih86)) entity.H86 = dr.GetDecimal(ih86);

            int ih87 = dr.GetOrdinal(this.h87);
            if (!dr.IsDBNull(ih87)) entity.H87 = dr.GetDecimal(ih87);

            int ih88 = dr.GetOrdinal(this.h88);
            if (!dr.IsDBNull(ih88)) entity.H88 = dr.GetDecimal(ih88);

            int ih89 = dr.GetOrdinal(this.h89);
            if (!dr.IsDBNull(ih89)) entity.H89 = dr.GetDecimal(ih89);

            int ih90 = dr.GetOrdinal(this.h90);
            if (!dr.IsDBNull(ih90)) entity.H90 = dr.GetDecimal(ih90);

            int ih91 = dr.GetOrdinal(this.h91);
            if (!dr.IsDBNull(ih91)) entity.H91 = dr.GetDecimal(ih91);

            int ih92 = dr.GetOrdinal(this.h92);
            if (!dr.IsDBNull(ih92)) entity.H92 = dr.GetDecimal(ih92);

            int ih93 = dr.GetOrdinal(this.h93);
            if (!dr.IsDBNull(ih93)) entity.H93 = dr.GetDecimal(ih93);

            int ih94 = dr.GetOrdinal(this.h94);
            if (!dr.IsDBNull(ih94)) entity.H64 = dr.GetDecimal(ih94);

            int ih95 = dr.GetOrdinal(this.h95);
            if (!dr.IsDBNull(ih95)) entity.H95 = dr.GetDecimal(ih95);

            int ih96 = dr.GetOrdinal(this.h96);
            if (!dr.IsDBNull(ih96)) entity.H96 = dr.GetDecimal(ih96);

            int iDemscofeccreacion = dr.GetOrdinal(this.Demscofeccreacion);
            if (!dr.IsDBNull(iDemscofeccreacion)) entity.Demscofeccreacion = dr.GetDateTime(iDemscofeccreacion);

            return entity;
        }

        #region Mapeo de los campos
        public string Ptomedicodi = "PTOMEDICODI";
        public string Medifecha = "MEDIFECHA";
        public string Prnvarcodi = "PRNVARCODI";
        public string Meditotal = "MEDITOTAL";        
        public string h1 = "H1";
        public string h2 = "H2";
        public string h3 = "H3";
        public string h4 = "H4";
        public string h5 = "H5";
        public string h6 = "H6";
        public string h7 = "H7";
        public string h8 = "H8";
        public string h9 = "H9";
        public string h10 = "H10";
        public string h11 = "H11";
        public string h12 = "H12";
        public string h13 = "H13";
        public string h14 = "H14";
        public string h15 = "H15";
        public string h16 = "H16";
        public string h17 = "H17";
        public string h18 = "H18";
        public string h19 = "H19";
        public string h20 = "H20";
        public string h21 = "H21";
        public string h22 = "H22";
        public string h23 = "H23";
        public string h24 = "H24";
        public string h25 = "H25";
        public string h26 = "H26";
        public string h27 = "H27";
        public string h28 = "H28";
        public string h29 = "H29";
        public string h30 = "H30";
        public string h31 = "H31";
        public string h32 = "H32";
        public string h33 = "H33";
        public string h34 = "H34";
        public string h35 = "H35";
        public string h36 = "H36";
        public string h37 = "H37";
        public string h38 = "H38";
        public string h39 = "H39";
        public string h40 = "H40";
        public string h41 = "H41";
        public string h42 = "H42";
        public string h43 = "H43";
        public string h44 = "H44";
        public string h45 = "H45";
        public string h46 = "H46";
        public string h47 = "H47";
        public string h48 = "H48";
        public string h49 = "H49";
        public string h50 = "H50";
        public string h51 = "H51";
        public string h52 = "H52";
        public string h53 = "H53";
        public string h54 = "H54";
        public string h55 = "H55";
        public string h56 = "H56";
        public string h57 = "H57";
        public string h58 = "H58";
        public string h59 = "H59";
        public string h60 = "H60";
        public string h61 = "H61";
        public string h62 = "H62";
        public string h63 = "H63";
        public string h64 = "H64";
        public string h65 = "H65";
        public string h66 = "H66";
        public string h67 = "H67";
        public string h68 = "H68";
        public string h69 = "H69";
        public string h70 = "H70";
        public string h71 = "H71";
        public string h72 = "H72";
        public string h73 = "H73";
        public string h74 = "H74";
        public string h75 = "H75";
        public string h76 = "H76";
        public string h77 = "H77";
        public string h78 = "H78";
        public string h79 = "H79";
        public string h80 = "H80";
        public string h81 = "H81";
        public string h82 = "H82";
        public string h83 = "H83";
        public string h84 = "H84";
        public string h85 = "H85";
        public string h86 = "H86";
        public string h87 = "H87";
        public string h88 = "H88";
        public string h89 = "H89";
        public string h90 = "H90";
        public string h91 = "H91";
        public string h92 = "H92";
        public string h93 = "H93";
        public string h94 = "H94";
        public string h95 = "H95";
        public string h96 = "H96";
        public string Demscousucreacion = "demscousucreacion";
        public string Demscofeccreacion = "demscofeccreacion";
        public string Demscousumodificacion = "demscousumodificacion";
        public string Demscofecmodificacion = "demscofecmodificacion";
        public string TableName = "DPO_DEMANDASCO";

        //Adcionales
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        #endregion

        #region Consultas a la BD
        public string SqlTruncateTablaTemporal
        {
            get { return base.GetSqlXml("TruncateTablaTemporal"); }
        }
        public string SqlReporteEstadoProceso
        {
            get { return base.GetSqlXml("ReporteEstadoProceso"); }
        }
        public string SqlDeleteRangoFecha
        {
            get { return base.GetSqlXml("DeleteRangoFecha"); }
        }
        public string SqlListGroupByMonthYear
        {
            get { return base.GetSqlXml("ListGroupByMonthYear"); }
        }
        public string SqlListMedidorDemandaTna
        {
            get { return base.GetSqlXml("ListMedidorDemandaTna"); }
        }
        public string SqlListDatosTNA
        {
            get { return base.GetSqlXml("ListDatosTNA"); }
        }
        public string SqlObtenerDemandaSco
        {
            get { return base.GetSqlXml("ObtenerDemandaSco"); }
        }
        #endregion
    }
}
