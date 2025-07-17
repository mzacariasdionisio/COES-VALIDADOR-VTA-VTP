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
    public class PrnServiciosAuxiliaresHelper : HelperBase 
    {
        public PrnServiciosAuxiliaresHelper() : base(Consultas.PrnServiciosAuxiliaresSql)
        {

        }

        public PrnServiciosAuxiliaresDTO Create(IDataReader dr)
        {
            PrnServiciosAuxiliaresDTO entity = new PrnServiciosAuxiliaresDTO();


            int IPrnserauxcodi = dr.GetOrdinal(this.Prnserauxcodi);
            if (!dr.IsDBNull(IPrnserauxcodi)) entity.Prnauxcodi = Convert.ToInt32(dr.GetValue(IPrnserauxcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPrrucodi = dr.GetOrdinal(this.Prrucodi);
            if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = Convert.ToInt32(dr.GetValue(iPrrucodi));

            int iFlagesManual = dr.GetOrdinal(this.FlagesManual);
            if (!dr.IsDBNull(iFlagesManual)) entity.Prnauxflagesmanual = Convert.ToInt32(dr.GetValue(iFlagesManual)) == 1 ? true : false;


            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = Convert.ToInt32(dr.GetValue(iH1));

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = Convert.ToInt32(dr.GetValue(iH2));

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH3)) entity.H3 = Convert.ToInt32(dr.GetValue(iH3));

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = Convert.ToInt32(dr.GetValue(iH4));

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = Convert.ToInt32(dr.GetValue(iH5));

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = Convert.ToInt32(dr.GetValue(iH6));

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = Convert.ToInt32(dr.GetValue(iH7));

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = Convert.ToInt32(dr.GetValue(iH8));

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = Convert.ToInt32(dr.GetValue(iH9));

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = Convert.ToInt32(dr.GetValue(iH10));

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = Convert.ToInt32(dr.GetValue(iH11));

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = Convert.ToInt32(dr.GetValue(iH12));

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = Convert.ToInt32(dr.GetValue(iH13));

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = Convert.ToInt32(dr.GetValue(iH14));

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = Convert.ToInt32(dr.GetValue(iH15));

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = Convert.ToInt32(dr.GetValue(iH16));

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = Convert.ToInt32(dr.GetValue(iH17));

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = Convert.ToInt32(dr.GetValue(iH18));

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = Convert.ToInt32(dr.GetValue(iH19));

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = Convert.ToInt32(dr.GetValue(iH20));

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = Convert.ToInt32(dr.GetValue(iH21));

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = Convert.ToInt32(dr.GetValue(iH22));

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = Convert.ToInt32(dr.GetValue(iH23));

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = Convert.ToInt32(dr.GetValue(iH24));

            int iH25 = dr.GetOrdinal(this.H25);
            if (!dr.IsDBNull(iH25)) entity.H25 = Convert.ToInt32(dr.GetValue(iH25));

            int iH26 = dr.GetOrdinal(this.H26);
            if (!dr.IsDBNull(iH26)) entity.H26 = Convert.ToInt32(dr.GetValue(iH26));

            int iH27 = dr.GetOrdinal(this.H27);
            if (!dr.IsDBNull(iH27)) entity.H27 = Convert.ToInt32(dr.GetValue(iH27));

            int iH28 = dr.GetOrdinal(this.H28);
            if (!dr.IsDBNull(iH28)) entity.H28 = Convert.ToInt32(dr.GetValue(iH28));

            int iH29 = dr.GetOrdinal(this.H29);
            if (!dr.IsDBNull(iH29)) entity.H29 = Convert.ToInt32(dr.GetValue(iH29));

            int iH30 = dr.GetOrdinal(this.H30);
            if (!dr.IsDBNull(iH30)) entity.H30 = Convert.ToInt32(dr.GetValue(iH30));

            int iH31 = dr.GetOrdinal(this.H31);
            if (!dr.IsDBNull(iH31)) entity.H31 = Convert.ToInt32(dr.GetValue(iH31));

            int iH32 = dr.GetOrdinal(this.H32);
            if (!dr.IsDBNull(iH32)) entity.H32 = Convert.ToInt32(dr.GetValue(iH32));

            int iH33 = dr.GetOrdinal(this.H33);
            if (!dr.IsDBNull(iH33)) entity.H33 = Convert.ToInt32(dr.GetValue(iH33));

            int iH34 = dr.GetOrdinal(this.H34);
            if (!dr.IsDBNull(iH34)) entity.H34 = Convert.ToInt32(dr.GetValue(iH34));

            int iH35 = dr.GetOrdinal(this.H35);
            if (!dr.IsDBNull(iH35)) entity.H35 = Convert.ToInt32(dr.GetValue(iH35));

            int iH36 = dr.GetOrdinal(this.H36);
            if (!dr.IsDBNull(iH36)) entity.H36 = Convert.ToInt32(dr.GetValue(iH36));

            int iH37 = dr.GetOrdinal(this.H37);
            if (!dr.IsDBNull(iH37)) entity.H37 = Convert.ToInt32(dr.GetValue(iH37));

            int iH38 = dr.GetOrdinal(this.H38);
            if (!dr.IsDBNull(iH38)) entity.H38 = Convert.ToInt32(dr.GetValue(iH38));

            int iH39 = dr.GetOrdinal(this.H39);
            if (!dr.IsDBNull(iH39)) entity.H39 = Convert.ToInt32(dr.GetValue(iH39));

            int iH40 = dr.GetOrdinal(this.H40);
            if (!dr.IsDBNull(iH40)) entity.H40 = Convert.ToInt32(dr.GetValue(iH40));

            int iH41 = dr.GetOrdinal(this.H41);
            if (!dr.IsDBNull(iH41)) entity.H41 = Convert.ToInt32(dr.GetValue(iH41));

            int iH42 = dr.GetOrdinal(this.H42);
            if (!dr.IsDBNull(iH42)) entity.H42 = Convert.ToInt32(dr.GetValue(iH42));

            int iH43 = dr.GetOrdinal(this.H43);
            if (!dr.IsDBNull(iH43)) entity.H43 = Convert.ToInt32(dr.GetValue(iH43));

            int iH44 = dr.GetOrdinal(this.H44);
            if (!dr.IsDBNull(iH44)) entity.H44 = Convert.ToInt32(dr.GetValue(iH44));

            int iH45 = dr.GetOrdinal(this.H45);
            if (!dr.IsDBNull(iH45)) entity.H45 = Convert.ToInt32(dr.GetValue(iH45));

            int iH46 = dr.GetOrdinal(this.H46);
            if (!dr.IsDBNull(iH46)) entity.H46 = Convert.ToInt32(dr.GetValue(iH46));

            int iH47 = dr.GetOrdinal(this.H47);
            if (!dr.IsDBNull(iH47)) entity.H47 = Convert.ToInt32(dr.GetValue(iH47));

            int iH48 = dr.GetOrdinal(this.H48);
            if (!dr.IsDBNull(iH48)) entity.H48 = Convert.ToInt32(dr.GetValue(iH48));


            int iPrruabrev = dr.GetOrdinal(this.Prruabrev);
            if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);


            return entity;
        }

        #region Mapeo de los campos
        public string Prnserauxcodi = "PRNAUXCODI";
        public string PrGrupocodi = "GRUPOCODI";
        public string Prrucodi = "PRRUCODI";
        public string FlagesManual = "PRNAUXFLAGESMANUAL";

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

        // -----------------------------------------------------------------------------------------------------------------
        // ASSETEC 04-04-2022
        // -----------------------------------------------------------------------------------------------------------------
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Prruabrev = "PRRUABREV";
        // -----------------------------------------------------------------------------------------------------------------
        #endregion


        #region Consultas a la BD
        // -----------------------------------------------------------------------------------------------------------------
        // ASSETEC 04-04-2022
        // -----------------------------------------------------------------------------------------------------------------
        public string SqlGetServiciosAuxiliaresByGrupo
        {
            get { return base.GetSqlXml("GetServiciosAuxiliaresByGrupo"); }
        }

        public string SqlListBarraFormulas
        {
            get { return base.GetSqlXml("ListBarraFormulas"); }
        }

        public string SqlListFormulas
        {
            get { return base.GetSqlXml("ListFormulas"); }
        }

        public string SqlListFormulasRelaciones
        {
            get { return base.GetSqlXml("ListFormulasRelaciones"); }
        }

        public string SqlDeleteRelaciones
        {
            get { return base.GetSqlXml("DeleteRelaciones"); }
        }

        // -----------------------------------------------------------------------------------------------------------------
        #endregion
    }

}
