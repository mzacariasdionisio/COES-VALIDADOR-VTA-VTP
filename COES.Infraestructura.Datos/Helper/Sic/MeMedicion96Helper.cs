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
    public class MeMedicion96Helper : HelperBase
    {
        public MeMedicion96Helper()
            : base(Consultas.MeMedicion96Sql)
        {
        }

        public MeMedicion96DTO Create(IDataReader dr)
        {
            MeMedicion96DTO entity = new MeMedicion96DTO();

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi))
            {
                entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));
                entity.Lectocodi = entity.Lectcodi;
            }

            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iMeditotal = dr.GetOrdinal(this.Meditotal);
            if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

            this.GetH1To96(dr, entity);

            int iTptomedicodi = dr.GetOrdinal(this.Tipoptomedicodi);
            if (!dr.IsDBNull(iTptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTptomedicodi));
            entity.Tipoptomedicodi = entity.Tipoptomedicodi > 0 ? entity.Tipoptomedicodi : -1;

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }

        #region PR5
        public void GetH1To96(IDataReader dr, MeMedicion96DTO entity)
        {
            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iH25 = dr.GetOrdinal(this.H25);
            if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

            int iH26 = dr.GetOrdinal(this.H26);
            if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

            int iH27 = dr.GetOrdinal(this.H27);
            if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

            int iH28 = dr.GetOrdinal(this.H28);
            if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

            int iH29 = dr.GetOrdinal(this.H29);
            if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

            int iH30 = dr.GetOrdinal(this.H30);
            if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

            int iH31 = dr.GetOrdinal(this.H31);
            if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

            int iH32 = dr.GetOrdinal(this.H32);
            if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

            int iH33 = dr.GetOrdinal(this.H33);
            if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

            int iH34 = dr.GetOrdinal(this.H34);
            if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

            int iH35 = dr.GetOrdinal(this.H35);
            if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

            int iH36 = dr.GetOrdinal(this.H36);
            if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

            int iH37 = dr.GetOrdinal(this.H37);
            if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

            int iH38 = dr.GetOrdinal(this.H38);
            if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

            int iH39 = dr.GetOrdinal(this.H39);
            if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

            int iH40 = dr.GetOrdinal(this.H40);
            if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

            int iH41 = dr.GetOrdinal(this.H41);
            if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

            int iH42 = dr.GetOrdinal(this.H42);
            if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

            int iH43 = dr.GetOrdinal(this.H43);
            if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

            int iH44 = dr.GetOrdinal(this.H44);
            if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

            int iH45 = dr.GetOrdinal(this.H45);
            if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

            int iH46 = dr.GetOrdinal(this.H46);
            if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

            int iH47 = dr.GetOrdinal(this.H47);
            if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

            int iH48 = dr.GetOrdinal(this.H48);
            if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

            int iH49 = dr.GetOrdinal(this.H49);
            if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

            int iH50 = dr.GetOrdinal(this.H50);
            if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

            int iH51 = dr.GetOrdinal(this.H51);
            if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

            int iH52 = dr.GetOrdinal(this.H52);
            if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

            int iH53 = dr.GetOrdinal(this.H53);
            if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

            int iH54 = dr.GetOrdinal(this.H54);
            if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

            int iH55 = dr.GetOrdinal(this.H55);
            if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

            int iH56 = dr.GetOrdinal(this.H56);
            if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

            int iH57 = dr.GetOrdinal(this.H57);
            if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

            int iH58 = dr.GetOrdinal(this.H58);
            if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

            int iH59 = dr.GetOrdinal(this.H59);
            if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);

            int iH60 = dr.GetOrdinal(this.H60);
            if (!dr.IsDBNull(iH60)) entity.H60 = dr.GetDecimal(iH60);

            int iH61 = dr.GetOrdinal(this.H61);
            if (!dr.IsDBNull(iH61)) entity.H61 = dr.GetDecimal(iH61);

            int iH62 = dr.GetOrdinal(this.H62);
            if (!dr.IsDBNull(iH62)) entity.H62 = dr.GetDecimal(iH62);

            int iH63 = dr.GetOrdinal(this.H63);
            if (!dr.IsDBNull(iH63)) entity.H63 = dr.GetDecimal(iH63);

            int iH64 = dr.GetOrdinal(this.H64);
            if (!dr.IsDBNull(iH64)) entity.H64 = dr.GetDecimal(iH64);

            int iH65 = dr.GetOrdinal(this.H65);
            if (!dr.IsDBNull(iH65)) entity.H65 = dr.GetDecimal(iH65);

            int iH66 = dr.GetOrdinal(this.H66);
            if (!dr.IsDBNull(iH66)) entity.H66 = dr.GetDecimal(iH66);

            int iH67 = dr.GetOrdinal(this.H67);
            if (!dr.IsDBNull(iH67)) entity.H67 = dr.GetDecimal(iH67);

            int iH68 = dr.GetOrdinal(this.H68);
            if (!dr.IsDBNull(iH68)) entity.H68 = dr.GetDecimal(iH68);

            int iH69 = dr.GetOrdinal(this.H69);
            if (!dr.IsDBNull(iH69)) entity.H69 = dr.GetDecimal(iH69);

            int iH70 = dr.GetOrdinal(this.H70);
            if (!dr.IsDBNull(iH70)) entity.H70 = dr.GetDecimal(iH70);

            int iH71 = dr.GetOrdinal(this.H71);
            if (!dr.IsDBNull(iH71)) entity.H71 = dr.GetDecimal(iH71);

            int iH72 = dr.GetOrdinal(this.H72);
            if (!dr.IsDBNull(iH72)) entity.H72 = dr.GetDecimal(iH72);

            int iH73 = dr.GetOrdinal(this.H73);
            if (!dr.IsDBNull(iH73)) entity.H73 = dr.GetDecimal(iH73);

            int iH74 = dr.GetOrdinal(this.H74);
            if (!dr.IsDBNull(iH74)) entity.H74 = dr.GetDecimal(iH74);

            int iH75 = dr.GetOrdinal(this.H75);
            if (!dr.IsDBNull(iH75)) entity.H75 = dr.GetDecimal(iH75);

            int iH76 = dr.GetOrdinal(this.H76);
            if (!dr.IsDBNull(iH76)) entity.H76 = dr.GetDecimal(iH76);

            int iH77 = dr.GetOrdinal(this.H77);
            if (!dr.IsDBNull(iH77)) entity.H77 = dr.GetDecimal(iH77);

            int iH78 = dr.GetOrdinal(this.H78);
            if (!dr.IsDBNull(iH78)) entity.H78 = dr.GetDecimal(iH78);

            int iH79 = dr.GetOrdinal(this.H79);
            if (!dr.IsDBNull(iH79)) entity.H79 = dr.GetDecimal(iH79);

            int iH80 = dr.GetOrdinal(this.H80);
            if (!dr.IsDBNull(iH80)) entity.H80 = dr.GetDecimal(iH80);

            int iH81 = dr.GetOrdinal(this.H81);
            if (!dr.IsDBNull(iH81)) entity.H81 = dr.GetDecimal(iH81);

            int iH82 = dr.GetOrdinal(this.H82);
            if (!dr.IsDBNull(iH82)) entity.H82 = dr.GetDecimal(iH82);

            int iH83 = dr.GetOrdinal(this.H83);
            if (!dr.IsDBNull(iH83)) entity.H83 = dr.GetDecimal(iH83);

            int iH84 = dr.GetOrdinal(this.H84);
            if (!dr.IsDBNull(iH84)) entity.H84 = dr.GetDecimal(iH84);

            int iH85 = dr.GetOrdinal(this.H85);
            if (!dr.IsDBNull(iH85)) entity.H85 = dr.GetDecimal(iH85);

            int iH86 = dr.GetOrdinal(this.H86);
            if (!dr.IsDBNull(iH86)) entity.H86 = dr.GetDecimal(iH86);

            int iH87 = dr.GetOrdinal(this.H87);
            if (!dr.IsDBNull(iH87)) entity.H87 = dr.GetDecimal(iH87);

            int iH88 = dr.GetOrdinal(this.H88);
            if (!dr.IsDBNull(iH88)) entity.H88 = dr.GetDecimal(iH88);

            int iH89 = dr.GetOrdinal(this.H89);
            if (!dr.IsDBNull(iH89)) entity.H89 = dr.GetDecimal(iH89);

            int iH90 = dr.GetOrdinal(this.H90);
            if (!dr.IsDBNull(iH90)) entity.H90 = dr.GetDecimal(iH90);

            int iH91 = dr.GetOrdinal(this.H91);
            if (!dr.IsDBNull(iH91)) entity.H91 = dr.GetDecimal(iH91);

            int iH92 = dr.GetOrdinal(this.H92);
            if (!dr.IsDBNull(iH92)) entity.H92 = dr.GetDecimal(iH92);

            int iH93 = dr.GetOrdinal(this.H93);
            if (!dr.IsDBNull(iH93)) entity.H93 = dr.GetDecimal(iH93);

            int iH94 = dr.GetOrdinal(this.H94);
            if (!dr.IsDBNull(iH94)) entity.H94 = dr.GetDecimal(iH94);

            int iH95 = dr.GetOrdinal(this.H95);
            if (!dr.IsDBNull(iH95)) entity.H95 = dr.GetDecimal(iH95);

            int iH96 = dr.GetOrdinal(this.H96);
            if (!dr.IsDBNull(iH96)) entity.H96 = dr.GetDecimal(iH96);
        }
        #endregion

        #region Mapeo de Campos

        public string Lectcodi = "LECTCODI";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Medifecha = "MEDIFECHA";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Meditotal = "MEDITOTAL";
        public string Mediestado = "MEDIESTADO";
        public string Equipadre = "EQUIPADRE";
        public string Central = "CENTRAL";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Fenergcodi = "FENERGCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Fenergnomb = "FENERGNOMB";
        public string Gruponomb = "GRUPONOMB";
        public string Fenercolor = "FENERCOLOR";
        public string Fenergabrev = "FENERGABREV";
        public string Tipogrupocodi = "TIPOGRUPOCODI";
        public string Tgenercodi = "TGENERCODI";
        public string Tgenernomb = "TGENERNOMB";
        public string Equitension = "EQUITENSION";
        public string Areanomb = "AREANOMB";       
        public string Areaoperativa = "AREAOPERATIVA";
        public string GrupSSAA = "GrupSSAA";
        public string Total = "TOTAL";
        public string Tipogeneracion = "TIPOGENERACION";
        public string Tipoptomedicodi = "TPTOMEDICODI";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
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


        /// <summary>
        /// Aplicativo PR16
        /// </summary>      
        public string Enviocodi = "ENVIOCODI";
        public string Item = "ITEM";
        public string Periodo = "PERIODO";
        public string Fuente = "FUENTE";
        public string FechaFila = "FECHA_FILA";
        public string Cumplimiento = "CUMPLIMIENTO";
        public string CodigoCliente = "CODIGO_CLIENTE";
        public string Suministrador = "SUMINISTRADOR";
        public string RucEmpresa = "RUC_EMPRESA";
        public string NombreEmpresa = "NOMBRE_EMPRESA";
        public string Subestacion = "SUBESTACION";
        public string Tension = "TENSION";
        public string NroEnvios = "NRO_ENVIOS";
        public string FechaPrimerEnvio = "FECHA_PRIMER_ENVIO";
        public string FechaUltimoEnvio = "FECHA_ULTIMO_ENVIO";
        public string IniRemision = "INI_REMISION";
        public string FinRemision = "FIN_REMISION";
        public string IniPeriodo = "INI_PERIODO";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";
        public string Enviofechaperiodoact = "ENVIOFECHAPERIODOACT";
        public string Enviofechaperiodoant = "ENVIOFECHAPERIODOANT";
        public string Meditotalact = "MEDITOTALACT";
        public string Meditotalant = "MEDITOTALANT";
        public string Qmedact = "QMEDACT";
        public string Qmedant = "QMEDANT";
        public string Varmensual = "VARMENSUAL";
        public string Varpromdiaria = "VARPROMDIARIA";
        public string Qregistros = "Q_REGISTROS";

        #region PR5
        public string Areacodi = "AREACODI";
        public string Areaoperativacodi = "AREAOPERATIVACODI";
        #endregion
        public string TptoMediCodi = "TPTOMEDICODI";

        #region SIOSEIN
        /// <summary>
        /// SIOSEIN
        /// </summary>        
        public string CodCentral = "CODCENTRAL";
        public string Grupomiembro = "GRUPOMIEMBRO";
        public string Tipogenerrer = "TIPOGENERRER";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Ctgdetnomb = "CTGDETNOMB";
        public string Grupointegrante = "Grupointegrante";
        public string Osinergcodi = "OSINERGCODI";
        public string OsinergcodiGen = "OSINERGCODIGEN";    //Ticket-6068
        public string Osinergcodi2 = "OSINERGCODI2"; //Movisoft 15-02-2022
        #endregion

        // Numerales Datos Base
        public string Osicodi = "OSICODI";

        #endregion

        public string SqlActualizarValor
        {
            get { return base.GetSqlXml("ActualizarValor"); }
        }

        public string SqlObtenerConsultaTipoGeneracion
        {
            get { return base.GetSqlXml("ObtenerConsultaPorTipoGeneracion"); }
        }
        
        public string SqlObtenerConsultaServiciosAuxiliares
        {
            get { return base.GetSqlXml("ObtenerConsultaServicioAuxiliar"); }
        }

        public string SqlNroRegistrosConsultasTipoGeneracion
        {
            get { return base.GetSqlXml("NroRegistrosConsultasTipoGeneracion"); }
        }
        
        public string SqlNroRegistrosServiciosAuxiliares
        {
            get { return base.GetSqlXml("NroRegistrosServiciosAuxiliares"); }
        }

        public string SqlTotalConsultaMedidores
        {
            get { return base.GetSqlXml("TotalConsultaMedidores"); }
        }
        
        public string SqlTotalServiciosAuxiliares
        {
            get { return base.GetSqlXml("TotalServiciosAuxiliares"); }
        }

        public string SqlListarTotalH
        {
            get { return base.GetSqlXml("ListarTotalH"); }
        }

        public string SqlListarDetalle
        {
            get { return base.GetSqlXml("ListarDetalle"); }
        }

        public string SqlObtenerExportacionConsultaMedidores
        {
            get { return base.GetSqlXml("ObtenerExportacionConsultaMedidores"); }
        }
        
        public string SqlObtenerExportacionServiciosAuxiliares
        {
            get { return base.GetSqlXml("ObtenerExportacionServiciosAuxiliares"); }
        }
        
        public string SqlObtenerReporteMedidores
        {
            get { return base.GetSqlXml("ObtenerReporteMedidores"); }
        }

        public string SqlObtenerReporteMedidoresConsolidado
        {
            get { return base.GetSqlXml("ObtenerReporteMedidoresConsolidado"); }
        }

        public string SqlObtenerDatosReporteMD
        {
            get { return base.GetSqlXml("ObtenerDatosReporteMD"); }
        }

        public string SqlObtenerMaximaDemandaRecursoEnergetico
        {
            get { return base.GetSqlXml("ObtenerMDPorRecursoEnergetico"); }
        }

        public string SqlObtenerMaximaDemandaRecursoEnergeticoRER
        {
            get { return base.GetSqlXml("ObtenerMDPorRecursoEnergeticoRER"); }
        }

        public string SqlObtenerFechaMaximaDemanda
        {
            get { return base.GetSqlXml("ObtenerFechaMaximaDemanda"); }
        }

        public string SqlObtenerFechaMinimaDemanda
        {
            get { return base.GetSqlXml("ObtenerFechaMinimaDemanda"); }
        }

        public string SqlObtenerEnvioPorEmpresa
        {
            get { return base.GetSqlXml("ObtenerEnvioPorEmpresa"); }
        }

        public string SqlDeleteEnvioPorEmpresa
        {
            get { return base.GetSqlXml("DeleteEnvioPorEmpresa"); }
        }

        public string SqlConsolidadoEnvioEmpresa
        {
            get { return base.GetSqlXml("ConsolidadoEnvioEmpresa"); }
        }

        public string SqlConsolidadoEnvioByEmpresaAndFormato
        {
            get { return base.GetSqlXml("ConsolidadoEnvioByEmpresaAndFormato"); }
        }

        public string SqlObtenerEnvioInterconexion
        {
            get { return base.GetSqlXml("ObtenerEnvioInterconexion"); }
        }

        public string SqlDeleteEnvioInterconexion
        {
            get { return base.GetSqlXml("DeleteEnvioInterconexion"); }
        }

        public string SqlObtenerHistoricoInterconexion
        {
            get { return base.GetSqlXml("ObtenerHistoricoInterconexion"); }
        }

        public string SqlTotalHistoricoInterconexion
        {
            get { return base.GetSqlXml("TotalHistoricoInterconexion"); }
        }

        public string SqlHistoricoPagInterconexion
        {
            get { return base.GetSqlXml("HistoricoPagInterconexion"); }
        }

        public string SqlDeleteEnvioArchivo
        {
            get { return base.GetSqlXml("DeleteEnvioArchivo"); }
        }

        public string SqlDeleteEnvioArchivo2
        {
            get { return base.GetSqlXml("DeleteEnvioArchivo2"); }
        }

        public string SqlDeleteEnvioArchivo3
        {
            get { return base.GetSqlXml("DeleteEnvioArchivo3"); }
        }

        public string SqlGetEnvioArchivo
        {
            get { return base.GetSqlXml("GetEnvioArchivo"); }
        }

        public string SqlObtenerNroRegistrosMedDistribucion
        {
            get { return base.GetSqlXml("ObtenerNroRegistrosMedDistribucion"); }
        }

        public string SqlObtenerConsultaMedDistribucion
        {
            get { return base.GetSqlXml("ObtenerConsultaMedDistribucion"); }
        }

        public string SqlTotalConsultaMesDistribucion
        {
            get { return base.GetSqlXml("TotalConsultaMesDistribucion"); }
        }

        public string SqlObtenerExportacionConsultaMedDistribucion
        {
            get { return base.GetSqlXml("ObtenerExportacionConsultaMedDistribucion"); }
        }

        public string SqlObtenerExportacionMasivaMedDistribucion
        {
            get { return base.GetSqlXml("ObtenerExportacionMasivaMedDistribucion"); }
        }

        public string SqlGetByPtoMedicion
        {
            get { return base.GetSqlXml("GetByPtoMedicion"); }
        }

        public string SqlObtenerConsultaWeb
        {
            get { return base.GetSqlXml("ObtenerConsultaWeb"); }
        }

        public string SqlObtenerConsultaWebReporte
        {
            get { return base.GetSqlXml("ObtenerConsultaWebReporte"); }
        }

        public string SqlPaginacionInterconexHistorico
        {
            get { return base.GetSqlXml("PaginacionInterconexHistorico"); }
        }
        
        public string SqlObtenerDatosEjecutado
        {
            get { return base.GetSqlXml("ObtenerDatosEjecutado"); }
        }

        public string SqlObtenerObservacionesCoherenciaMensual
        {
            get { return base.GetSqlXml("ObtenerObservacionesCoherenciaMensual"); }
        }

        public string SqlObtenerObservacionesCoherenciaDiaria
        {
            get { return base.GetSqlXml("ObtenerObservacionesCoherenciaDiaria"); }
        }

        public string SqlListReporteInformacion
        {
            get { return base.GetSqlXml("ListReporteInformacion15min"); }
        }
        
        public string SqlListReporteInformacionCount
        {
            get { return base.GetSqlXml("ListReporteInformacion15minCount"); }
        }

        public string SqlDeleteFechas
        {
            get { return base.GetSqlXml("DeleteFechas"); }
        }

        public string SqlObtenerMedicion96
        {
            get { return base.GetSqlXml("ObtenerMedicion96"); }
        }

        public string SqlGetResumenMaximaDemanda
        {
            get { return base.GetSqlXml("GetResumenMaximaDemanda"); }
        }

        public string SqlGetConsolidadoMaximaDemanda
        {
            get { return base.GetSqlXml("GetConsolidadoMaximaDemanda"); }
        }

        public string SqlGetConsolidadoExcesoPotenReact
        {
            get { return base.GetSqlXml("GetConsolidadoExcesoPotenReact"); }
        }

        public string SqlGetEnvioArchivoSinHojaPuntoMed
        {
            get { return base.GetSqlXml("GetEnvioArchivoSinHojaPuntoMed"); }
        }

        public string SqlSaveDemandaMercadoLibre
        {
            get { return base.GetSqlXml("SaveDemandaMercadoLibre"); }
        }

        #region Aplicativo VTD 

        public string ClienteNomb = "CLIENTENOMB";
        public string BarrNomb = "BARRNOMB";

        public string SqlGetEmpresaEnergiaPrevista
        {
            get { return base.GetSqlXml("GetEmpresaEnergiaPrevista"); }
        }

        public string SqlGetDemandaCoincidente
        {
            get { return base.GetSqlXml("GetDemandaCoincidente"); }
        }

        public string SqlGetEnergiaPrevistaRetirar
        {
            get { return GetSqlXml("GetEnergiaPrevistaRetirar"); }
        }

        public string SqlListByFilter
        {
            get { return GetSqlXml("ListByFilter"); }
        }

        public string SqlListPagedByFilter
        {
            get { return GetSqlXml("ListPagedByFilter"); }
        }

        #endregion Aplicativo VTD

        #region Numerales Datos Base
        public string SqlDatosBase_5_1_1
        {
            get { return base.GetSqlXml("ListaDatosBase_5_1_1"); }
        }
        #endregion

        public string SqlObtenerDatosMedicionComparativo
        {
            get { return base.GetSqlXml("ObtenerDatosMedicionComparativo"); }
        }
    }
}
