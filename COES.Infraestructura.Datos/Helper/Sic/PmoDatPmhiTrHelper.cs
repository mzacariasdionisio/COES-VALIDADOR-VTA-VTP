using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PmoDatPmhiTrHelper : HelperBase
    {
        public PmoDatPmhiTrHelper()
            : base(Consultas.PmoDatPmhiTr)
        {
        }

        public PmoDatPmhiTrDTO Create(IDataReader dr)
        {
            PmoDatPmhiTrDTO entity = new PmoDatPmhiTrDTO();

            int iPmPmhtCodi = dr.GetOrdinal(this.PmPmhtCodi);
            if (!dr.IsDBNull(iPmPmhtCodi)) entity.PmPmhtCodi = dr.GetInt32(iPmPmhtCodi);

            int iPmPeriCodi = dr.GetOrdinal(this.PmPeriCodi);
            if (!dr.IsDBNull(iPmPeriCodi)) entity.PmPeriCodi = dr.GetInt32(iPmPeriCodi);

            int iSddpcodi = dr.GetOrdinal(this.Sddpcodi);
            if (!dr.IsDBNull(iSddpcodi)) entity.Sddpcodi = dr.GetInt32(iSddpcodi);

            int iPmPmhtAnhio = dr.GetOrdinal(this.PmPmhtAnhio);
            if (!dr.IsDBNull(iPmPmhtAnhio)) entity.PmPmhtAnhio = dr.GetInt32(iPmPmhtAnhio);

            int iPmPmhtDisp01 = dr.GetOrdinal(this.PmPmhtDisp01);
            if (!dr.IsDBNull(iPmPmhtDisp01)) entity.PmPmhtDisp01 = dr.GetDecimal(iPmPmhtDisp01);

            int iPmPmhtDisp02 = dr.GetOrdinal(this.PmPmhtDisp02);
            if (!dr.IsDBNull(iPmPmhtDisp02)) entity.PmPmhtDisp02 = dr.GetDecimal(iPmPmhtDisp02);

            int iPmPmhtDisp03 = dr.GetOrdinal(this.PmPmhtDisp03);
            if (!dr.IsDBNull(iPmPmhtDisp03)) entity.PmPmhtDisp03 = dr.GetDecimal(iPmPmhtDisp03);

            int iPmPmhtDisp04 = dr.GetOrdinal(this.PmPmhtDisp04);
            if (!dr.IsDBNull(iPmPmhtDisp04)) entity.PmPmhtDisp04 = dr.GetDecimal(iPmPmhtDisp04);

            int iPmPmhtDisp05 = dr.GetOrdinal(this.PmPmhtDisp05);
            if (!dr.IsDBNull(iPmPmhtDisp05)) entity.PmPmhtDisp05 = dr.GetDecimal(iPmPmhtDisp05);

            int iPmPmhtDisp06 = dr.GetOrdinal(this.PmPmhtDisp06);
            if (!dr.IsDBNull(iPmPmhtDisp06)) entity.PmPmhtDisp06 = dr.GetDecimal(iPmPmhtDisp06);

            int iPmPmhtDisp07 = dr.GetOrdinal(this.PmPmhtDisp07);
            if (!dr.IsDBNull(iPmPmhtDisp07)) entity.PmPmhtDisp07 = dr.GetDecimal(iPmPmhtDisp07);

            int iPmPmhtDisp08 = dr.GetOrdinal(this.PmPmhtDisp08);
            if (!dr.IsDBNull(iPmPmhtDisp08)) entity.PmPmhtDisp08 = dr.GetDecimal(iPmPmhtDisp08);

            int iPmPmhtDisp09 = dr.GetOrdinal(this.PmPmhtDisp09);
            if (!dr.IsDBNull(iPmPmhtDisp09)) entity.PmPmhtDisp09 = dr.GetDecimal(iPmPmhtDisp09);

            int iPmPmhtDisp10 = dr.GetOrdinal(this.PmPmhtDisp10);
            if (!dr.IsDBNull(iPmPmhtDisp10)) entity.PmPmhtDisp10 = dr.GetDecimal(iPmPmhtDisp10);

            int iPmPmhtDisp11 = dr.GetOrdinal(this.PmPmhtDisp11);
            if (!dr.IsDBNull(iPmPmhtDisp11)) entity.PmPmhtDisp11 = dr.GetDecimal(iPmPmhtDisp11);

            int iPmPmhtDisp12 = dr.GetOrdinal(this.PmPmhtDisp12);
            if (!dr.IsDBNull(iPmPmhtDisp12)) entity.PmPmhtDisp12 = dr.GetDecimal(iPmPmhtDisp12);

            int iPmPmhtDisp13 = dr.GetOrdinal(this.PmPmhtDisp13);
            if (!dr.IsDBNull(iPmPmhtDisp13)) entity.PmPmhtDisp13 = dr.GetDecimal(iPmPmhtDisp13);

            int iPmPmhtDisp14 = dr.GetOrdinal(this.PmPmhtDisp14);
            if (!dr.IsDBNull(iPmPmhtDisp14)) entity.PmPmhtDisp14 = dr.GetDecimal(iPmPmhtDisp14);

            int iPmPmhtDisp15 = dr.GetOrdinal(this.PmPmhtDisp15);
            if (!dr.IsDBNull(iPmPmhtDisp15)) entity.PmPmhtDisp15 = dr.GetDecimal(iPmPmhtDisp15);

            int iPmPmhtDisp16 = dr.GetOrdinal(this.PmPmhtDisp16);
            if (!dr.IsDBNull(iPmPmhtDisp16)) entity.PmPmhtDisp16 = dr.GetDecimal(iPmPmhtDisp16);

            int iPmPmhtDisp17 = dr.GetOrdinal(this.PmPmhtDisp17);
            if (!dr.IsDBNull(iPmPmhtDisp17)) entity.PmPmhtDisp17 = dr.GetDecimal(iPmPmhtDisp17);

            int iPmPmhtDisp18 = dr.GetOrdinal(this.PmPmhtDisp18);
            if (!dr.IsDBNull(iPmPmhtDisp18)) entity.PmPmhtDisp18 = dr.GetDecimal(iPmPmhtDisp18);

            int iPmPmhtDisp19 = dr.GetOrdinal(this.PmPmhtDisp19);
            if (!dr.IsDBNull(iPmPmhtDisp19)) entity.PmPmhtDisp19 = dr.GetDecimal(iPmPmhtDisp19);

            int iPmPmhtDisp20 = dr.GetOrdinal(this.PmPmhtDisp20);
            if (!dr.IsDBNull(iPmPmhtDisp20)) entity.PmPmhtDisp20 = dr.GetDecimal(iPmPmhtDisp20);

            int iPmPmhtDisp21 = dr.GetOrdinal(this.PmPmhtDisp21);
            if (!dr.IsDBNull(iPmPmhtDisp21)) entity.PmPmhtDisp21 = dr.GetDecimal(iPmPmhtDisp21);

            int iPmPmhtDisp22 = dr.GetOrdinal(this.PmPmhtDisp22);
            if (!dr.IsDBNull(iPmPmhtDisp22)) entity.PmPmhtDisp22 = dr.GetDecimal(iPmPmhtDisp22);

            int iPmPmhtDisp23 = dr.GetOrdinal(this.PmPmhtDisp23);
            if (!dr.IsDBNull(iPmPmhtDisp23)) entity.PmPmhtDisp23 = dr.GetDecimal(iPmPmhtDisp23);

            int iPmPmhtDisp24 = dr.GetOrdinal(this.PmPmhtDisp24);
            if (!dr.IsDBNull(iPmPmhtDisp24)) entity.PmPmhtDisp24 = dr.GetDecimal(iPmPmhtDisp24);

            int iPmPmhtDisp25 = dr.GetOrdinal(this.PmPmhtDisp25);
            if (!dr.IsDBNull(iPmPmhtDisp25)) entity.PmPmhtDisp25 = dr.GetDecimal(iPmPmhtDisp25);

            int iPmPmhtDisp26 = dr.GetOrdinal(this.PmPmhtDisp26);
            if (!dr.IsDBNull(iPmPmhtDisp26)) entity.PmPmhtDisp26 = dr.GetDecimal(iPmPmhtDisp26);

            int iPmPmhtDisp27 = dr.GetOrdinal(this.PmPmhtDisp27);
            if (!dr.IsDBNull(iPmPmhtDisp27)) entity.PmPmhtDisp27 = dr.GetDecimal(iPmPmhtDisp27);

            int iPmPmhtDisp28 = dr.GetOrdinal(this.PmPmhtDisp28);
            if (!dr.IsDBNull(iPmPmhtDisp28)) entity.PmPmhtDisp28 = dr.GetDecimal(iPmPmhtDisp28);

            int iPmPmhtDisp29 = dr.GetOrdinal(this.PmPmhtDisp29);
            if (!dr.IsDBNull(iPmPmhtDisp29)) entity.PmPmhtDisp29 = dr.GetDecimal(iPmPmhtDisp29);

            int iPmPmhtDisp30 = dr.GetOrdinal(this.PmPmhtDisp30);
            if (!dr.IsDBNull(iPmPmhtDisp30)) entity.PmPmhtDisp30 = dr.GetDecimal(iPmPmhtDisp30);

            int iPmPmhtDisp31 = dr.GetOrdinal(this.PmPmhtDisp31);
            if (!dr.IsDBNull(iPmPmhtDisp31)) entity.PmPmhtDisp31 = dr.GetDecimal(iPmPmhtDisp31);

            int iPmPmhtDisp32 = dr.GetOrdinal(this.PmPmhtDisp32);
            if (!dr.IsDBNull(iPmPmhtDisp32)) entity.PmPmhtDisp32 = dr.GetDecimal(iPmPmhtDisp32);

            int iPmPmhtDisp33 = dr.GetOrdinal(this.PmPmhtDisp33);
            if (!dr.IsDBNull(iPmPmhtDisp33)) entity.PmPmhtDisp33 = dr.GetDecimal(iPmPmhtDisp33);

            int iPmPmhtDisp34 = dr.GetOrdinal(this.PmPmhtDisp34);
            if (!dr.IsDBNull(iPmPmhtDisp34)) entity.PmPmhtDisp34 = dr.GetDecimal(iPmPmhtDisp34);

            int iPmPmhtDisp35 = dr.GetOrdinal(this.PmPmhtDisp35);
            if (!dr.IsDBNull(iPmPmhtDisp35)) entity.PmPmhtDisp35 = dr.GetDecimal(iPmPmhtDisp35);

            int iPmPmhtDisp36 = dr.GetOrdinal(this.PmPmhtDisp36);
            if (!dr.IsDBNull(iPmPmhtDisp36)) entity.PmPmhtDisp36 = dr.GetDecimal(iPmPmhtDisp36);

            int iPmPmhtDisp37 = dr.GetOrdinal(this.PmPmhtDisp37);
            if (!dr.IsDBNull(iPmPmhtDisp37)) entity.PmPmhtDisp37 = dr.GetDecimal(iPmPmhtDisp37);

            int iPmPmhtDisp38 = dr.GetOrdinal(this.PmPmhtDisp38);
            if (!dr.IsDBNull(iPmPmhtDisp38)) entity.PmPmhtDisp38 = dr.GetDecimal(iPmPmhtDisp38);

            int iPmPmhtDisp39 = dr.GetOrdinal(this.PmPmhtDisp39);
            if (!dr.IsDBNull(iPmPmhtDisp39)) entity.PmPmhtDisp39 = dr.GetDecimal(iPmPmhtDisp39);

            int iPmPmhtDisp40 = dr.GetOrdinal(this.PmPmhtDisp40);
            if (!dr.IsDBNull(iPmPmhtDisp40)) entity.PmPmhtDisp40 = dr.GetDecimal(iPmPmhtDisp40);

            int iPmPmhtDisp41 = dr.GetOrdinal(this.PmPmhtDisp41);
            if (!dr.IsDBNull(iPmPmhtDisp41)) entity.PmPmhtDisp41 = dr.GetDecimal(iPmPmhtDisp41);

            int iPmPmhtDisp42 = dr.GetOrdinal(this.PmPmhtDisp42);
            if (!dr.IsDBNull(iPmPmhtDisp42)) entity.PmPmhtDisp42 = dr.GetDecimal(iPmPmhtDisp42);

            int iPmPmhtDisp43 = dr.GetOrdinal(this.PmPmhtDisp43);
            if (!dr.IsDBNull(iPmPmhtDisp43)) entity.PmPmhtDisp43 = dr.GetDecimal(iPmPmhtDisp43);

            int iPmPmhtDisp44 = dr.GetOrdinal(this.PmPmhtDisp44);
            if (!dr.IsDBNull(iPmPmhtDisp44)) entity.PmPmhtDisp44 = dr.GetDecimal(iPmPmhtDisp44);

            int iPmPmhtDisp45 = dr.GetOrdinal(this.PmPmhtDisp45);
            if (!dr.IsDBNull(iPmPmhtDisp45)) entity.PmPmhtDisp45 = dr.GetDecimal(iPmPmhtDisp45);

            int iPmPmhtDisp46 = dr.GetOrdinal(this.PmPmhtDisp46);
            if (!dr.IsDBNull(iPmPmhtDisp46)) entity.PmPmhtDisp46 = dr.GetDecimal(iPmPmhtDisp46);

            int iPmPmhtDisp47 = dr.GetOrdinal(this.PmPmhtDisp47);
            if (!dr.IsDBNull(iPmPmhtDisp47)) entity.PmPmhtDisp47 = dr.GetDecimal(iPmPmhtDisp47);

            int iPmPmhtDisp48 = dr.GetOrdinal(this.PmPmhtDisp48);
            if (!dr.IsDBNull(iPmPmhtDisp48)) entity.PmPmhtDisp48 = dr.GetDecimal(iPmPmhtDisp48);

            int iPmPmhtDisp49 = dr.GetOrdinal(this.PmPmhtDisp49);
            if (!dr.IsDBNull(iPmPmhtDisp49)) entity.PmPmhtDisp49 = dr.GetDecimal(iPmPmhtDisp49);

            int iPmPmhtDisp50 = dr.GetOrdinal(this.PmPmhtDisp50);
            if (!dr.IsDBNull(iPmPmhtDisp50)) entity.PmPmhtDisp50 = dr.GetDecimal(iPmPmhtDisp50);

            int iPmPmhtDisp51 = dr.GetOrdinal(this.PmPmhtDisp51);
            if (!dr.IsDBNull(iPmPmhtDisp51)) entity.PmPmhtDisp51 = dr.GetDecimal(iPmPmhtDisp51);

            int iPmPmhtDisp52 = dr.GetOrdinal(this.PmPmhtDisp52);
            if (!dr.IsDBNull(iPmPmhtDisp52)) entity.PmPmhtDisp52 = dr.GetDecimal(iPmPmhtDisp52);

            int iPmPmhtDisp53 = dr.GetOrdinal(this.PmPmhtDisp53);
            if (!dr.IsDBNull(iPmPmhtDisp53)) entity.PmPmhtDisp53 = dr.GetDecimal(iPmPmhtDisp53);

            int iPmPmhtTipo = dr.GetOrdinal(this.PmPmhtTipo);
            if (!dr.IsDBNull(iPmPmhtTipo)) entity.PmPmhtTipo = dr.GetString(iPmPmhtTipo);

            return entity;
        }

        #region Mapeo de Campos

        public string Cant = "CANT";

        public string Planta = "PLANTA";
        public string GrupoNomb = "GRUPONOMB";
        public string GrupoCodiSDDP = "GRUPOCODISDDP";
        public string PmPmhtCodi = "PMPMHTCODI";
        public string PmPeriCodi = "PMPERICODI";
        public string Sddpcodi = "Sddpcodi";
        public string GrupoCodi = "GRUPOCODI";
        public string PmPmhtAnhio = "PMPMHTANHIO";
        public string PmPmhtDisp01 = "PMPMHTDISP01";
        public string PmPmhtDisp02 = "PMPMHTDISP02";
        public string PmPmhtDisp03 = "PMPMHTDISP03";
        public string PmPmhtDisp04 = "PMPMHTDISP04";
        public string PmPmhtDisp05 = "PMPMHTDISP05";
        public string PmPmhtDisp06 = "PMPMHTDISP06";
        public string PmPmhtDisp07 = "PMPMHTDISP07";
        public string PmPmhtDisp08 = "PMPMHTDISP08";
        public string PmPmhtDisp09 = "PMPMHTDISP09";
        public string PmPmhtDisp10 = "PMPMHTDISP10";
        public string PmPmhtDisp11 = "PMPMHTDISP11";
        public string PmPmhtDisp12 = "PMPMHTDISP12";
        public string PmPmhtDisp13 = "PMPMHTDISP13";
        public string PmPmhtDisp14 = "PMPMHTDISP14";
        public string PmPmhtDisp15 = "PMPMHTDISP15";
        public string PmPmhtDisp16 = "PMPMHTDISP16";
        public string PmPmhtDisp17 = "PMPMHTDISP17";
        public string PmPmhtDisp18 = "PMPMHTDISP18";
        public string PmPmhtDisp19 = "PMPMHTDISP19";
        public string PmPmhtDisp20 = "PMPMHTDISP20";
        public string PmPmhtDisp21 = "PMPMHTDISP21";
        public string PmPmhtDisp22 = "PMPMHTDISP22";
        public string PmPmhtDisp23 = "PMPMHTDISP23";
        public string PmPmhtDisp24 = "PMPMHTDISP24";
        public string PmPmhtDisp25 = "PMPMHTDISP25";
        public string PmPmhtDisp26 = "PMPMHTDISP26";
        public string PmPmhtDisp27 = "PMPMHTDISP27";
        public string PmPmhtDisp28 = "PMPMHTDISP28";
        public string PmPmhtDisp29 = "PMPMHTDISP29";
        public string PmPmhtDisp30 = "PMPMHTDISP30";
        public string PmPmhtDisp31 = "PMPMHTDISP31";
        public string PmPmhtDisp32 = "PMPMHTDISP32";
        public string PmPmhtDisp33 = "PMPMHTDISP33";
        public string PmPmhtDisp34 = "PMPMHTDISP34";
        public string PmPmhtDisp35 = "PMPMHTDISP35";
        public string PmPmhtDisp36 = "PMPMHTDISP36";
        public string PmPmhtDisp37 = "PMPMHTDISP37";
        public string PmPmhtDisp38 = "PMPMHTDISP38";
        public string PmPmhtDisp39 = "PMPMHTDISP39";
        public string PmPmhtDisp40 = "PMPMHTDISP40";
        public string PmPmhtDisp41 = "PMPMHTDISP41";
        public string PmPmhtDisp42 = "PMPMHTDISP42";
        public string PmPmhtDisp43 = "PMPMHTDISP43";
        public string PmPmhtDisp44 = "PMPMHTDISP44";
        public string PmPmhtDisp45 = "PMPMHTDISP45";
        public string PmPmhtDisp46 = "PMPMHTDISP46";
        public string PmPmhtDisp47 = "PMPMHTDISP47";
        public string PmPmhtDisp48 = "PMPMHTDISP48";
        public string PmPmhtDisp49 = "PMPMHTDISP49";
        public string PmPmhtDisp50 = "PMPMHTDISP50";
        public string PmPmhtDisp51 = "PMPMHTDISP51";
        public string PmPmhtDisp52 = "PMPMHTDISP52";
        public string PmPmhtDisp53 = "PMPMHTDISP53";
        public string PmPmhtTipo = "PMPMHTTIPO";

        public string Tptomedicodi = "TPTOMEDICODI";
        public string Sddpnum = "SDDPNUM";
        public string Sddpnomb = "SDDPNOMB";

        #endregion

        public string SqlGetCount
        {
            get { return base.GetSqlXml("GetCount"); }
        }
    }
}
