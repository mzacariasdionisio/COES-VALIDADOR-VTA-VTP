using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla trn_valor_trans
    /// </summary>    
    public class ValorTransferenciaHelper : HelperBase
    {
        public ValorTransferenciaHelper()
            : base(Consultas.ValorTransferenciaSql)
        {
        }

        public ValorTransferenciaDTO Create(IDataReader dr)
        {
            ValorTransferenciaDTO entity = new ValorTransferenciaDTO();

            int iValoTranCodi = dr.GetOrdinal(this.ValoTranCodi);
            if (!dr.IsDBNull(iValoTranCodi)) entity.ValoTranCodi = Convert.ToInt32(dr.GetDecimal(iValoTranCodi));

            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

            int iBarrCodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

            int iEmpCodi = dr.GetOrdinal(this.EmpCodi);
            if (!dr.IsDBNull(iEmpCodi)) entity.EmpCodi = dr.GetInt32(iEmpCodi);

            int iCostMargCodi = dr.GetOrdinal(this.CostMargCodi);
            if (!dr.IsDBNull(iCostMargCodi)) entity.CostMargCodi = dr.GetInt32(iCostMargCodi);

            int iValoTranFlag = dr.GetOrdinal(this.ValoTranFlag);
            if (!dr.IsDBNull(iValoTranFlag)) entity.ValoTranFlag = dr.GetString(iValoTranFlag);

            int iValoTranCodEntRet = dr.GetOrdinal(this.ValoTranCodEntRet);
            if (!dr.IsDBNull(iValoTranCodEntRet)) entity.ValoTranCodEntRet = dr.GetString(iValoTranCodEntRet);

            int iValoTranVersion = dr.GetOrdinal(this.ValoTranVersion);
            if (!dr.IsDBNull(iValoTranVersion)) entity.ValoTranVersion = dr.GetInt32(iValoTranVersion);

            int iValoTranDia = dr.GetOrdinal(this.ValoTranDia);
            if (!dr.IsDBNull(iValoTranDia)) entity.ValoTranDia = dr.GetInt32(iValoTranDia);

            int iVTTotalDia = dr.GetOrdinal(this.VTTotalDia);
            if (!dr.IsDBNull(iVTTotalDia)) entity.VTTotalDia = dr.GetDecimal(iVTTotalDia);

            int iVTTotalEnergia = dr.GetOrdinal(this.VTTotalEnergia);
            if (!dr.IsDBNull(iVTTotalEnergia)) entity.VTTotalEnergia = dr.GetDecimal(iVTTotalEnergia);

            int iVTTipoinformacion = dr.GetOrdinal(this.VTTipoinformacion);
            if (!dr.IsDBNull(iVTTipoinformacion)) entity.VTTipoInformacion = dr.GetString(iVTTipoinformacion);

            int iVT1 = dr.GetOrdinal(this.VT1);
            if (!dr.IsDBNull(iVT1)) entity.VT1 = dr.GetDecimal(iVT1);

            int iVT2 = dr.GetOrdinal(this.VT2);
            if (!dr.IsDBNull(iVT2)) entity.VT2 = dr.GetDecimal(iVT2);

            int iVT3 = dr.GetOrdinal(this.VT3);
            if (!dr.IsDBNull(iVT3)) entity.VT3 = dr.GetDecimal(iVT3);

            int iVT4 = dr.GetOrdinal(this.VT4);
            if (!dr.IsDBNull(iVT4)) entity.VT4 = dr.GetDecimal(iVT4);

            int iVT5 = dr.GetOrdinal(this.VT5);
            if (!dr.IsDBNull(iVT5)) entity.VT5 = dr.GetDecimal(iVT5);

            int iVT6 = dr.GetOrdinal(this.VT6);
            if (!dr.IsDBNull(iVT6)) entity.VT6 = dr.GetDecimal(iVT6);

            int iVT7 = dr.GetOrdinal(this.VT7);
            if (!dr.IsDBNull(iVT7)) entity.VT7 = dr.GetDecimal(iVT7);

            int iVT8 = dr.GetOrdinal(this.VT8);
            if (!dr.IsDBNull(iVT8)) entity.VT8 = dr.GetDecimal(iVT8);

            int iVT9 = dr.GetOrdinal(this.VT9);
            if (!dr.IsDBNull(iVT9)) entity.VT9 = dr.GetDecimal(iVT9);

            int iVT10 = dr.GetOrdinal(this.VT10);
            if (!dr.IsDBNull(iVT10)) entity.VT10 = dr.GetDecimal(iVT10);

            int iVT11 = dr.GetOrdinal(this.VT11);
            if (!dr.IsDBNull(iVT11)) entity.VT11 = dr.GetDecimal(iVT11);

            int iVT12 = dr.GetOrdinal(this.VT12);
            if (!dr.IsDBNull(iVT12)) entity.VT12 = dr.GetDecimal(iVT12);

            int iVT13 = dr.GetOrdinal(this.VT13);
            if (!dr.IsDBNull(iVT13)) entity.VT13 = dr.GetDecimal(iVT13);

            int iVT14 = dr.GetOrdinal(this.VT14);
            if (!dr.IsDBNull(iVT14)) entity.VT14 = dr.GetDecimal(iVT14);

            int iVT15 = dr.GetOrdinal(this.VT15);
            if (!dr.IsDBNull(iVT15)) entity.VT15 = dr.GetDecimal(iVT15);

            int iVT16 = dr.GetOrdinal(this.VT16);
            if (!dr.IsDBNull(iVT16)) entity.VT16 = dr.GetDecimal(iVT16);

            int iVT17 = dr.GetOrdinal(this.VT17);
            if (!dr.IsDBNull(iVT17)) entity.VT17 = dr.GetDecimal(iVT17);

            int iVT18 = dr.GetOrdinal(this.VT18);
            if (!dr.IsDBNull(iVT18)) entity.VT18 = dr.GetDecimal(iVT18);

            int iVT19 = dr.GetOrdinal(this.VT19);
            if (!dr.IsDBNull(iVT19)) entity.VT19 = dr.GetDecimal(iVT19);

            int iVT20 = dr.GetOrdinal(this.VT20);
            if (!dr.IsDBNull(iVT20)) entity.VT20 = dr.GetDecimal(iVT20);

            int iVT21 = dr.GetOrdinal(this.VT21);
            if (!dr.IsDBNull(iVT21)) entity.VT21 = dr.GetDecimal(iVT21);

            int iVT22 = dr.GetOrdinal(this.VT22);
            if (!dr.IsDBNull(iVT22)) entity.VT22 = dr.GetDecimal(iVT22);

            int iVT23 = dr.GetOrdinal(this.VT23);
            if (!dr.IsDBNull(iVT23)) entity.VT23 = dr.GetDecimal(iVT23);

            int iVT24 = dr.GetOrdinal(this.VT24);
            if (!dr.IsDBNull(iVT24)) entity.VT24 = dr.GetDecimal(iVT24);

            int iVT25 = dr.GetOrdinal(this.VT25);
            if (!dr.IsDBNull(iVT25)) entity.VT25 = dr.GetDecimal(iVT25);

            int iVT26 = dr.GetOrdinal(this.VT26);
            if (!dr.IsDBNull(iVT26)) entity.VT26 = dr.GetDecimal(iVT26);

            int iVT27 = dr.GetOrdinal(this.VT27);
            if (!dr.IsDBNull(iVT27)) entity.VT27 = dr.GetDecimal(iVT27);

            int iVT28 = dr.GetOrdinal(this.VT28);
            if (!dr.IsDBNull(iVT28)) entity.VT28 = dr.GetDecimal(iVT28);

            int iVT29 = dr.GetOrdinal(this.VT29);
            if (!dr.IsDBNull(iVT29)) entity.VT29 = dr.GetDecimal(iVT29);

            int iVT30 = dr.GetOrdinal(this.VT30);
            if (!dr.IsDBNull(iVT30)) entity.VT30 = dr.GetDecimal(iVT30);

            int iVT31 = dr.GetOrdinal(this.VT31);
            if (!dr.IsDBNull(iVT31)) entity.VT31 = dr.GetDecimal(iVT31);

            int iVT32 = dr.GetOrdinal(this.VT32);
            if (!dr.IsDBNull(iVT32)) entity.VT32 = dr.GetDecimal(iVT32);

            int iVT33 = dr.GetOrdinal(this.VT33);
            if (!dr.IsDBNull(iVT33)) entity.VT33 = dr.GetDecimal(iVT33);

            int iVT34 = dr.GetOrdinal(this.VT34);
            if (!dr.IsDBNull(iVT34)) entity.VT34 = dr.GetDecimal(iVT34);

            int iVT35 = dr.GetOrdinal(this.VT35);
            if (!dr.IsDBNull(iVT35)) entity.VT35 = dr.GetDecimal(iVT35);

            int iVT36 = dr.GetOrdinal(this.VT36);
            if (!dr.IsDBNull(iVT36)) entity.VT36 = dr.GetDecimal(iVT36);

            int iVT37 = dr.GetOrdinal(this.VT37);
            if (!dr.IsDBNull(iVT37)) entity.VT37 = dr.GetDecimal(iVT37);

            int iVT38 = dr.GetOrdinal(this.VT38);
            if (!dr.IsDBNull(iVT38)) entity.VT38 = dr.GetDecimal(iVT38);

            int iVT39 = dr.GetOrdinal(this.VT39);
            if (!dr.IsDBNull(iVT39)) entity.VT39 = dr.GetDecimal(iVT39);

            int iVT40 = dr.GetOrdinal(this.VT40);
            if (!dr.IsDBNull(iVT40)) entity.VT40 = dr.GetDecimal(iVT40);

            int iVT41 = dr.GetOrdinal(this.VT41);
            if (!dr.IsDBNull(iVT41)) entity.VT41 = dr.GetDecimal(iVT41);

            int iVT42 = dr.GetOrdinal(this.VT42);
            if (!dr.IsDBNull(iVT42)) entity.VT42 = dr.GetDecimal(iVT42);

            int iVT43 = dr.GetOrdinal(this.VT43);
            if (!dr.IsDBNull(iVT43)) entity.VT43 = dr.GetDecimal(iVT43);

            int iVT44 = dr.GetOrdinal(this.VT44);
            if (!dr.IsDBNull(iVT44)) entity.VT44 = dr.GetDecimal(iVT44);

            int iVT45 = dr.GetOrdinal(this.VT45);
            if (!dr.IsDBNull(iVT45)) entity.VT45 = dr.GetDecimal(iVT45);

            int iVT46 = dr.GetOrdinal(this.VT46);
            if (!dr.IsDBNull(iVT46)) entity.VT46 = dr.GetDecimal(iVT46);

            int iVT47 = dr.GetOrdinal(this.VT47);
            if (!dr.IsDBNull(iVT47)) entity.VT47 = dr.GetDecimal(iVT47);

            int iVT48 = dr.GetOrdinal(this.VT48);
            if (!dr.IsDBNull(iVT48)) entity.VT48 = dr.GetDecimal(iVT48);

            int iVT49 = dr.GetOrdinal(this.VT49);
            if (!dr.IsDBNull(iVT49)) entity.VT49 = dr.GetDecimal(iVT49);

            int iVT50 = dr.GetOrdinal(this.VT50);
            if (!dr.IsDBNull(iVT50)) entity.VT50 = dr.GetDecimal(iVT50);

            int iVT51 = dr.GetOrdinal(this.VT51);
            if (!dr.IsDBNull(iVT51)) entity.VT51 = dr.GetDecimal(iVT51);

            int iVT52 = dr.GetOrdinal(this.VT52);
            if (!dr.IsDBNull(iVT52)) entity.VT52 = dr.GetDecimal(iVT52);

            int iVT53 = dr.GetOrdinal(this.VT53);
            if (!dr.IsDBNull(iVT53)) entity.VT53 = dr.GetDecimal(iVT53);

            int iVT54 = dr.GetOrdinal(this.VT54);
            if (!dr.IsDBNull(iVT54)) entity.VT54 = dr.GetDecimal(iVT54);

            int iVT55 = dr.GetOrdinal(this.VT55);
            if (!dr.IsDBNull(iVT55)) entity.VT55 = dr.GetDecimal(iVT55);

            int iVT56 = dr.GetOrdinal(this.VT56);
            if (!dr.IsDBNull(iVT56)) entity.VT56 = dr.GetDecimal(iVT56);

            int iVT57 = dr.GetOrdinal(this.VT57);
            if (!dr.IsDBNull(iVT57)) entity.VT57 = dr.GetDecimal(iVT57);

            int iVT58 = dr.GetOrdinal(this.VT58);
            if (!dr.IsDBNull(iVT58)) entity.VT58 = dr.GetDecimal(iVT58);

            int iVT59 = dr.GetOrdinal(this.VT59);
            if (!dr.IsDBNull(iVT59)) entity.VT59 = dr.GetDecimal(iVT59);

            int iVT60 = dr.GetOrdinal(this.VT60);
            if (!dr.IsDBNull(iVT60)) entity.VT60 = dr.GetDecimal(iVT60);

            int iVT61 = dr.GetOrdinal(this.VT61);
            if (!dr.IsDBNull(iVT61)) entity.VT61 = dr.GetDecimal(iVT61);

            int iVT62 = dr.GetOrdinal(this.VT62);
            if (!dr.IsDBNull(iVT62)) entity.VT62 = dr.GetDecimal(iVT62);

            int iVT63 = dr.GetOrdinal(this.VT63);
            if (!dr.IsDBNull(iVT63)) entity.VT63 = dr.GetDecimal(iVT63);

            int iVT64 = dr.GetOrdinal(this.VT64);
            if (!dr.IsDBNull(iVT64)) entity.VT64 = dr.GetDecimal(iVT64);

            int iVT65 = dr.GetOrdinal(this.VT65);
            if (!dr.IsDBNull(iVT65)) entity.VT65 = dr.GetDecimal(iVT65);

            int iVT66 = dr.GetOrdinal(this.VT66);
            if (!dr.IsDBNull(iVT66)) entity.VT66 = dr.GetDecimal(iVT66);

            int iVT67 = dr.GetOrdinal(this.VT67);
            if (!dr.IsDBNull(iVT67)) entity.VT67 = dr.GetDecimal(iVT67);

            int iVT68 = dr.GetOrdinal(this.VT68);
            if (!dr.IsDBNull(iVT68)) entity.VT68 = dr.GetDecimal(iVT68);

            int iVT69 = dr.GetOrdinal(this.VT69);
            if (!dr.IsDBNull(iVT60)) entity.VT69 = dr.GetDecimal(iVT69);

            int iVT70 = dr.GetOrdinal(this.VT70);
            if (!dr.IsDBNull(iVT70)) entity.VT70 = dr.GetDecimal(iVT70);

            int iVT71 = dr.GetOrdinal(this.VT71);
            if (!dr.IsDBNull(iVT71)) entity.VT71 = dr.GetDecimal(iVT71);

            int iVT72 = dr.GetOrdinal(this.VT72);
            if (!dr.IsDBNull(iVT72)) entity.VT72 = dr.GetDecimal(iVT72);

            int iVT73 = dr.GetOrdinal(this.VT73);
            if (!dr.IsDBNull(iVT73)) entity.VT73 = dr.GetDecimal(iVT73);

            int iVT74 = dr.GetOrdinal(this.VT74);
            if (!dr.IsDBNull(iVT74)) entity.VT74 = dr.GetDecimal(iVT74);

            int iVT75 = dr.GetOrdinal(this.VT75);
            if (!dr.IsDBNull(iVT75)) entity.VT75 = dr.GetDecimal(iVT75);

            int iVT76 = dr.GetOrdinal(this.VT76);
            if (!dr.IsDBNull(iVT76)) entity.VT76 = dr.GetDecimal(iVT76);

            int iVT77 = dr.GetOrdinal(this.VT77);
            if (!dr.IsDBNull(iVT77)) entity.VT77 = dr.GetDecimal(iVT77);

            int iVT78 = dr.GetOrdinal(this.VT78);
            if (!dr.IsDBNull(iVT78)) entity.VT78 = dr.GetDecimal(iVT78);

            int iVT79 = dr.GetOrdinal(this.VT79);
            if (!dr.IsDBNull(iVT79)) entity.VT79 = dr.GetDecimal(iVT79);

            int iVT80 = dr.GetOrdinal(this.VT80);
            if (!dr.IsDBNull(iVT80)) entity.VT80 = dr.GetDecimal(iVT80);

            int iVT81 = dr.GetOrdinal(this.VT81);
            if (!dr.IsDBNull(iVT81)) entity.VT81 = dr.GetDecimal(iVT81);

            int iVT82 = dr.GetOrdinal(this.VT82);
            if (!dr.IsDBNull(iVT82)) entity.VT82 = dr.GetDecimal(iVT82);

            int iVT83 = dr.GetOrdinal(this.VT83);
            if (!dr.IsDBNull(iVT83)) entity.VT83 = dr.GetDecimal(iVT83);

            int iVT84 = dr.GetOrdinal(this.VT84);
            if (!dr.IsDBNull(iVT84)) entity.VT84 = dr.GetDecimal(iVT84);

            int iVT85 = dr.GetOrdinal(this.VT85);
            if (!dr.IsDBNull(iVT85)) entity.VT85 = dr.GetDecimal(iVT85);

            int iVT86 = dr.GetOrdinal(this.VT86);
            if (!dr.IsDBNull(iVT86)) entity.VT86 = dr.GetDecimal(iVT86);

            int iVT87 = dr.GetOrdinal(this.VT87);
            if (!dr.IsDBNull(iVT87)) entity.VT87 = dr.GetDecimal(iVT87);

            int iVT88 = dr.GetOrdinal(this.VT88);
            if (!dr.IsDBNull(iVT88)) entity.VT88 = dr.GetDecimal(iVT88);

            int iVT89 = dr.GetOrdinal(this.VT89);
            if (!dr.IsDBNull(iVT89)) entity.VT89 = dr.GetDecimal(iVT89);

            int iVT90 = dr.GetOrdinal(this.VT90);
            if (!dr.IsDBNull(iVT90)) entity.VT90 = dr.GetDecimal(iVT90);

            int iVT91 = dr.GetOrdinal(this.VT91);
            if (!dr.IsDBNull(iVT91)) entity.VT91 = dr.GetDecimal(iVT91);

            int iVT92 = dr.GetOrdinal(this.VT92);
            if (!dr.IsDBNull(iVT92)) entity.VT92 = dr.GetDecimal(iVT92);

            int iVT93 = dr.GetOrdinal(this.VT93);
            if (!dr.IsDBNull(iVT93)) entity.VT93 = dr.GetDecimal(iVT93);

            int iVT94 = dr.GetOrdinal(this.VT94);
            if (!dr.IsDBNull(iVT94)) entity.VT94 = dr.GetDecimal(iVT94);

            int iVT95 = dr.GetOrdinal(this.VT95);
            if (!dr.IsDBNull(iVT95)) entity.VT95 = dr.GetDecimal(iVT95);

            int iVT96 = dr.GetOrdinal(this.VT96);
            if (!dr.IsDBNull(iVT96)) entity.VT96 = dr.GetDecimal(iVT96);

            int iUSERNAME = dr.GetOrdinal(this.USERNAME);
            if (!dr.IsDBNull(iUSERNAME)) entity.VtranUserName = dr.GetString(iUSERNAME);

            int iValoTranFecIns = dr.GetOrdinal(this.ValoTranFecIns);
            if (!dr.IsDBNull(iValoTranFecIns)) entity.ValoTranFecIns = dr.GetDateTime(iValoTranFecIns);

            return entity;
        }

        #region Mapeo de Campos

        public string PeriCodi1 = "PeriCodi1";
        public string VersionCodi1 = "VersionCodi1";
        public string PeriCodi2 = "PeriCodi2";
        public string VersionCodi2 = "VersionCodi2";
        public string CliCodi = "CliCodi";
        public string TipoEntregaCodi = "TipoEntregaCodi";
         
        public string ValoTranCodi = "VTRANCODI";
        public string BarrCodi = "BARRCODI";
        public string EmpCodi = "EMPRCODI";
        public string PeriCodi = "PERICODI";
        public string CostMargCodi = "COSMARCODI";
        public string ValoTranFlag = "VTRANFLAG";
        public string ValoTranCodEntRet = "VTRANCODENTRET";
        public string ValoTranVersion = "VTRANVERSION";
        public string ValoTranDia = "VTRANDIA";
        public string VT1 = "VTRAN1";
        public string VT2 = "VTRAN2";
        public string VT3 = "VTRAN3";
        public string VT4 = "VTRAN4";
        public string VT5 = "VTRAN5";
        public string VT6 = "VTRAN6";
        public string VT7 = "VTRAN7";
        public string VT8 = "VTRAN8";
        public string VT9 = "VTRAN9";
        public string VT10 = "VTRAN10";
        public string VT11 = "VTRAN11";
        public string VT12 = "VTRAN12";
        public string VT13 = "VTRAN13";
        public string VT14 = "VTRAN14";
        public string VT15 = "VTRAN15";
        public string VT16 = "VTRAN16";
        public string VT17 = "VTRAN17";
        public string VT18 = "VTRAN18";
        public string VT19 = "VTRAN19";
        public string VT20 = "VTRAN20";
        public string VT21 = "VTRAN21";
        public string VT22 = "VTRAN22";
        public string VT23 = "VTRAN23";
        public string VT24 = "VTRAN24";
        public string VT25 = "VTRAN25";
        public string VT26 = "VTRAN26";
        public string VT27 = "VTRAN27";
        public string VT28 = "VTRAN28";
        public string VT29 = "VTRAN29";
        public string VT30 = "VTRAN30";
        public string VT31 = "VTRAN31";
        public string VT32 = "VTRAN32";
        public string VT33 = "VTRAN33";
        public string VT34 = "VTRAN34";
        public string VT35 = "VTRAN35";
        public string VT36 = "VTRAN36";
        public string VT37 = "VTRAN37";
        public string VT38 = "VTRAN38";
        public string VT39 = "VTRAN39";
        public string VT40 = "VTRAN40";
        public string VT41 = "VTRAN41";
        public string VT42 = "VTRAN42";
        public string VT43 = "VTRAN43";
        public string VT44 = "VTRAN44";
        public string VT45 = "VTRAN45";
        public string VT46 = "VTRAN46";
        public string VT47 = "VTRAN47";
        public string VT48 = "VTRAN48";
        public string VT49 = "VTRAN49";
        public string VT50 = "VTRAN50";
        public string VT51 = "VTRAN51";
        public string VT52 = "VTRAN52";
        public string VT53 = "VTRAN53";
        public string VT54 = "VTRAN54";
        public string VT55 = "VTRAN55";
        public string VT56 = "VTRAN56";
        public string VT57 = "VTRAN57";
        public string VT58 = "VTRAN58";
        public string VT59 = "VTRAN59";
        public string VT60 = "VTRAN60";
        public string VT61 = "VTRAN61";
        public string VT62 = "VTRAN62";
        public string VT63 = "VTRAN63";
        public string VT64 = "VTRAN64";
        public string VT65 = "VTRAN65";
        public string VT66 = "VTRAN66";
        public string VT67 = "VTRAN67";
        public string VT68 = "VTRAN68";
        public string VT69 = "VTRAN69";
        public string VT70 = "VTRAN70";
        public string VT71 = "VTRAN71";
        public string VT72 = "VTRAN72";
        public string VT73 = "VTRAN73";
        public string VT74 = "VTRAN74";
        public string VT75 = "VTRAN75";
        public string VT76 = "VTRAN76";
        public string VT77 = "VTRAN77";
        public string VT78 = "VTRAN78";
        public string VT79 = "VTRAN79";
        public string VT80 = "VTRAN80";
        public string VT81 = "VTRAN81";
        public string VT82 = "VTRAN82";
        public string VT83 = "VTRAN83";
        public string VT84 = "VTRAN84";
        public string VT85 = "VTRAN85";
        public string VT86 = "VTRAN86";
        public string VT87 = "VTRAN87";
        public string VT88 = "VTRAN88";
        public string VT89 = "VTRAN89";
        public string VT90 = "VTRAN90";
        public string VT91 = "VTRAN91";
        public string VT92 = "VTRAN92";
        public string VT93 = "VTRAN93";
        public string VT94 = "VTRAN94";
        public string VT95 = "VTRAN95";
        public string VT96 = "VTRAN96";
        public string VTTotalDia = "VTRANTOTALDIA";
        public string ValoTranFecIns = "VTRANFECINS";
        public string VTTotalEnergia = "VTRANTOTALENERGIA";
        public string VTTipoinformacion = "VTRANTIPOINFORMACION";
        public string EMPRNOMB = "EMPRNOMB";
        public string BARRNOMBRE = "BARRBARRATRANSFERENCIA";
        public string VALORIZACION = "VALORIZACION";
        public string ENERGIA = "ENERGIA";
        public string SALEMPSALDO = "SALEMPSALDO";
        public string SALRSCSALDO = "SALRSCSALDO";
        public string COMPENSACION = "COMPENSACION";
        public string SALDORECALCULO = "SALDORECALCULO";
        public string VTOTEMPRESA = "VTOTEMPRESA";
        public string VTOTANTERIOR = "VTOTANTERIOR";
        public string USERNAME = "VTRANUSERNAME";
        public string ENTREGA = "ENTREGA";
        public string RETIRO = "RETIRO";
        public string NombEmpresa = "NOMBEMPRESA";
        public string RucEmpresa = "RUCEMPRESA";
        public string Entregas = "ENTREGAS";
        public string Retiros = "RETIROS";
        public string Neto = "NETO";
        public string TIPOEMPRCODI = "TIPOEMPRCODI";

        public string TableName = "TRN_VALOR_TRANS";

        public string Emprcodi = "EmprCodi";
        public string Osinergcodi = "Osinergcodi";

// Agregado rentas congestion
        public string RENTACONGESTION = "RENTACONGESTION";
        public string RCLICITACION = "RCLICITACION";
        public string RCBILATERAL = "RCBILATERAL";

        public string Cliemprcodi = "CLIEMPRCODI";
        public string FechaInicio = "FECHAINICIO";
        public string FechaFin = "FECHAFIN";

        #region SIOSEIN
        public string Emprcodosinergmin = "Emprcodosinergmin";
        #endregion

        //CU21
        public string CodEntCodi = "CODENTCODI";
        public string listaCodigosRetiro = "LISTACODIGOSRETIRO";
        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlGetTotalByTipoFlag
        {
            get { return base.GetSqlXml("GetTotalByTipoFlag"); }
        }

        public string SqlGetValorEmpresaByPeriVer
        {
            get { return base.GetSqlXml("GetValorEmpresaByPeriVer"); }
        }

        public string SqlGetSaldoEmpresaByPeriVer
        {
            get { return base.GetSqlXml("GetSaldoEmpresaByPeriVer"); }
        }

        public string SqlGetBalanceEnergia
        {
            get { return base.GetSqlXml("GetBalanceEnergia"); }
        }

        public string SqlGetBalanceValorTransferencia
        {
            get { return base.GetSqlXml("GetBalanceValorTransferencia"); }
        }

        public string SqlGetValorTransferencia
        {
            get { return base.GetSqlXml("GetValorTransferencia"); }
        }

        public string SqlGetDesviacionRetiros
        {
            get { return base.GetSqlXml("GetDesviacionRetiros"); }
        }

        //ASSETEC - 20181001-----------------------------------------------------------------------------------------------
        public string SqlGrabarValorizacionEntrega
        {
            get { return base.GetSqlXml("GrabarValorizacionEntrega"); }
        }

        public string SqlGrabarValorizacionRetiro
        {
            get { return base.GetSqlXml("GrabarValorizacionRetiro"); }
        }

        #region SIOSEIN2

        public string SqlListarValorTransferenciaTotalXEmpresaYTipoflag
        {
            get { return base.GetSqlXml("ListarValorTransferenciaTotalXEmpresaYTipoflag"); }
        }

        public string SqlGetMaxVersion
        {
            get { return base.GetSqlXml("GetMaxVersion"); }
        }
        public string SqlGetValorTransferenciaAgrpBarra
        {
            get { return base.GetSqlXml("GetValorTransferenciaAgrpBarra"); }
        }
        #endregion

        #region SIOSEIN
        public string SqlObtenerListaValoresTransferencia
        {
            get { return base.GetSqlXml("ObtenerListaValoresTransferencia"); }
        }
        #endregion


        public string SqlListarCodigosValorizados
        {
            get { return base.GetSqlXml("ListarCodigosValorizados"); }
        }
        
        public string SqlListarCodigosValorizadosTransferencia
        {
            get { return base.GetSqlXml("ListarCodigosValorizadosTransferencia"); }
        }
        
        public string SqlListarCodigosValorizadosGrafica
        {
            get { return base.GetSqlXml("ListarCodigosValorizadosGrafica"); }
        }    
        public string SqlListarCodigosValorizadosGraficaNew
        {
            get { return base.GetSqlXml("ListarCodigosValorizadosGraficaNew"); }
        }
        public string SqlListarCodigosValorizadosTransferenciaGraficaNew
        {
            get { return base.GetSqlXml("ListarCodigosValorizadosTransferenciaGraficaNew"); }
        }

        public string SqlListarCodigosValorizadosGraficaTotal
        {
            get { return base.GetSqlXml("ListarCodigosValorizadosGraficaTotal"); }
        }
        
        public string SqlListarCodigosValorizadosGraficaTotalTransferencia
        {
            get { return base.GetSqlXml("ListarCodigosValorizadosGraficaTotalTransferencia"); }
        }

        public string SqlListarCodigos
        {
            get { return base.GetSqlXml("ListarCodigos"); }
        }      public string SqlListarEmpresasAsociadas
        {
            get { return base.GetSqlXml("ListarEmpresasAsociadas"); }
        }

        public string SqlListarComparativo
        {
            get { return base.GetSqlXml("ListarComparativo"); }
        }   
        
        public string SqlListarComparativoEntregaRetiroValor
        {
            get { return base.GetSqlXml("ListarComparativoEntregaRetiroValor"); }
        }
   
        public string SqlListarComparativoEntregaRetiroValorTransferencia
        {
            get { return base.GetSqlXml("ListarComparativoEntregaRetiroValorTransferencia"); }
        }
   
        public string SqlListarComparativoEntregaRetiroValorDET
        {
            get { return base.GetSqlXml("ListarComparativoEntregaRetiroValorDET"); }
        }
                public string SqlListarComparativoEntregaRetiroValorDETTransferencia
        {
            get { return base.GetSqlXml("ListarComparativoEntregaRetiroValorDETTransferencia"); }
        }

        //CU21
        public string SqlListarEnergiaEntregaDetalle
        {
            get { return base.GetSqlXml("ListarEnergiaEntregaDetalle"); }
        }
        public string SqlListarEnergiaRetiroDetalle
        {
            get { return base.GetSqlXml("ListarEnergiaRetiroDetalle"); }
        }
        public string SqlListarValorEnergiaEntregaDetalle
        {
            get { return base.GetSqlXml("ListarValorEnergiaEntregaDetalle"); }
        }
        public string SqlListarValorEnergiaRetiroDetalle
        {
            get { return base.GetSqlXml("ListarValorEnergiaRetiroDetalle"); }
        }
    }
}
