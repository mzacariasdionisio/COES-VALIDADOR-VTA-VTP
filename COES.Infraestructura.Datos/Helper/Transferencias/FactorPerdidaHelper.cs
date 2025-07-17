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
    /// Clase que contiene el mapeo de la tabla trn_factor_perdida
    /// </summary>
    public class FactorPerdidaHelper : HelperBase
    {
        public FactorPerdidaHelper()
            : base(Consultas.FactorPerdidaSql)
        {
        }

        public FactorPerdidaDTO Create(IDataReader dr)
        {
            FactorPerdidaDTO entity = new FactorPerdidaDTO();

            int iFacPerCodi = dr.GetOrdinal(this.FacPerCodi);
            if (!dr.IsDBNull(iFacPerCodi)) entity.FacPerCodi = dr.GetInt32(iFacPerCodi);

            int iBarrCodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

            int iFacPerBarrNombre = dr.GetOrdinal(this.FacPerBarrNombre);
            if (!dr.IsDBNull(iFacPerBarrNombre)) entity.FacPerBarrNombre = dr.GetString(iFacPerBarrNombre);

            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

            int iFacPerBase = dr.GetOrdinal(this.FacPerBase);
            if (!dr.IsDBNull(iFacPerBase)) entity.FacPerBase = dr.GetString(iFacPerBase);

            int iFacPerVersion = dr.GetOrdinal(this.FacPerVersion);
            if (!dr.IsDBNull(iFacPerVersion)) entity.FacPerVersion = dr.GetInt32(iFacPerVersion);

            int iFacPerDia = dr.GetOrdinal(this.FacPerDia);
            if (!dr.IsDBNull(iFacPerDia)) entity.FacPerDia = dr.GetInt32(iFacPerDia);

            int iFacPer1 = dr.GetOrdinal(this.FacPer1);
            if (!dr.IsDBNull(iFacPer1)) entity.FacPer1 = dr.GetDecimal(iFacPer1);

            int iFacPer2 = dr.GetOrdinal(this.FacPer2);
            if (!dr.IsDBNull(iFacPer2)) entity.FacPer2 = dr.GetDecimal(iFacPer2);

            int iFacPer3 = dr.GetOrdinal(this.FacPer3);
            if (!dr.IsDBNull(iFacPer3)) entity.FacPer3 = dr.GetDecimal(iFacPer3);

            int iFacPer4 = dr.GetOrdinal(this.FacPer4);
            if (!dr.IsDBNull(iFacPer4)) entity.FacPer4 = dr.GetDecimal(iFacPer4);

            int iFacPer5 = dr.GetOrdinal(this.FacPer5);
            if (!dr.IsDBNull(iFacPer5)) entity.FacPer5 = dr.GetDecimal(iFacPer5);

            int iFacPer6 = dr.GetOrdinal(this.FacPer6);
            if (!dr.IsDBNull(iFacPer6)) entity.FacPer6 = dr.GetDecimal(iFacPer6);

            int iFacPer7 = dr.GetOrdinal(this.FacPer7);
            if (!dr.IsDBNull(iFacPer7)) entity.FacPer7 = dr.GetDecimal(iFacPer7);

            int iFacPer8 = dr.GetOrdinal(this.FacPer8);
            if (!dr.IsDBNull(iFacPer8)) entity.FacPer8 = dr.GetDecimal(iFacPer8);

            int iFacPer9 = dr.GetOrdinal(this.FacPer9);
            if (!dr.IsDBNull(iFacPer9)) entity.FacPer9 = dr.GetDecimal(iFacPer9);

            int iFacPer10 = dr.GetOrdinal(this.FacPer10);
            if (!dr.IsDBNull(iFacPer10)) entity.FacPer10 = dr.GetDecimal(iFacPer10);

            int iFacPer11 = dr.GetOrdinal(this.FacPer11);
            if (!dr.IsDBNull(iFacPer11)) entity.FacPer11 = dr.GetDecimal(iFacPer11);

            int iFacPer12 = dr.GetOrdinal(this.FacPer12);
            if (!dr.IsDBNull(iFacPer12)) entity.FacPer12 = dr.GetDecimal(iFacPer12);

            int iFacPer13 = dr.GetOrdinal(this.FacPer13);
            if (!dr.IsDBNull(iFacPer13)) entity.FacPer13 = dr.GetDecimal(iFacPer13);

            int iFacPer14 = dr.GetOrdinal(this.FacPer14);
            if (!dr.IsDBNull(iFacPer14)) entity.FacPer14 = dr.GetDecimal(iFacPer14);

            int iFacPer15 = dr.GetOrdinal(this.FacPer15);
            if (!dr.IsDBNull(iFacPer15)) entity.FacPer15 = dr.GetDecimal(iFacPer15);

            int iFacPer16 = dr.GetOrdinal(this.FacPer16);
            if (!dr.IsDBNull(iFacPer16)) entity.FacPer16 = dr.GetDecimal(iFacPer16);

            int iFacPer17 = dr.GetOrdinal(this.FacPer17);
            if (!dr.IsDBNull(iFacPer17)) entity.FacPer17 = dr.GetDecimal(iFacPer17);

            int iFacPer18 = dr.GetOrdinal(this.FacPer18);
            if (!dr.IsDBNull(iFacPer18)) entity.FacPer18 = dr.GetDecimal(iFacPer18);

            int iFacPer19 = dr.GetOrdinal(this.FacPer19);
            if (!dr.IsDBNull(iFacPer19)) entity.FacPer19 = dr.GetDecimal(iFacPer19);

            int iFacPer20 = dr.GetOrdinal(this.FacPer20);
            if (!dr.IsDBNull(iFacPer20)) entity.FacPer20 = dr.GetDecimal(iFacPer20);

            int iFacPer21 = dr.GetOrdinal(this.FacPer21);
            if (!dr.IsDBNull(iFacPer21)) entity.FacPer21 = dr.GetDecimal(iFacPer21);

            int iFacPer22 = dr.GetOrdinal(this.FacPer22);
            if (!dr.IsDBNull(iFacPer22)) entity.FacPer22 = dr.GetDecimal(iFacPer22);

            int iFacPer23 = dr.GetOrdinal(this.FacPer23);
            if (!dr.IsDBNull(iFacPer23)) entity.FacPer23 = dr.GetDecimal(iFacPer23);

            int iFacPer24 = dr.GetOrdinal(this.FacPer24);
            if (!dr.IsDBNull(iFacPer24)) entity.FacPer24 = dr.GetDecimal(iFacPer24);

            int iFacPer25 = dr.GetOrdinal(this.FacPer25);
            if (!dr.IsDBNull(iFacPer25)) entity.FacPer25 = dr.GetDecimal(iFacPer25);

            int iFacPer26 = dr.GetOrdinal(this.FacPer26);
            if (!dr.IsDBNull(iFacPer26)) entity.FacPer26 = dr.GetDecimal(iFacPer26);

            int iFacPer27 = dr.GetOrdinal(this.FacPer27);
            if (!dr.IsDBNull(iFacPer27)) entity.FacPer27 = dr.GetDecimal(iFacPer27);

            int iFacPer28 = dr.GetOrdinal(this.FacPer28);
            if (!dr.IsDBNull(iFacPer28)) entity.FacPer28 = dr.GetDecimal(iFacPer28);

            int iFacPer29 = dr.GetOrdinal(this.FacPer29);
            if (!dr.IsDBNull(iFacPer29)) entity.FacPer29 = dr.GetDecimal(iFacPer29);

            int iFacPer30 = dr.GetOrdinal(this.FacPer30);
            if (!dr.IsDBNull(iFacPer30)) entity.FacPer30 = dr.GetDecimal(iFacPer30);

            int iFacPer31 = dr.GetOrdinal(this.FacPer31);
            if (!dr.IsDBNull(iFacPer31)) entity.FacPer31 = dr.GetDecimal(iFacPer31);

            int iFacPer32 = dr.GetOrdinal(this.FacPer32);
            if (!dr.IsDBNull(iFacPer32)) entity.FacPer32 = dr.GetDecimal(iFacPer32);

            int iFacPer33 = dr.GetOrdinal(this.FacPer33);
            if (!dr.IsDBNull(iFacPer33)) entity.FacPer33 = dr.GetDecimal(iFacPer33);

            int iFacPer34 = dr.GetOrdinal(this.FacPer34);
            if (!dr.IsDBNull(iFacPer34)) entity.FacPer34 = dr.GetDecimal(iFacPer34);

            int iFacPer35 = dr.GetOrdinal(this.FacPer35);
            if (!dr.IsDBNull(iFacPer35)) entity.FacPer35 = dr.GetDecimal(iFacPer35);

            int iFacPer36 = dr.GetOrdinal(this.FacPer36);
            if (!dr.IsDBNull(iFacPer36)) entity.FacPer36 = dr.GetDecimal(iFacPer36);

            int iFacPer37 = dr.GetOrdinal(this.FacPer37);
            if (!dr.IsDBNull(iFacPer37)) entity.FacPer37 = dr.GetDecimal(iFacPer37);

            int iFacPer38 = dr.GetOrdinal(this.FacPer38);
            if (!dr.IsDBNull(iFacPer38)) entity.FacPer38 = dr.GetDecimal(iFacPer38);

            int iFacPer39 = dr.GetOrdinal(this.FacPer39);
            if (!dr.IsDBNull(iFacPer39)) entity.FacPer39 = dr.GetDecimal(iFacPer39);

            int iFacPer40 = dr.GetOrdinal(this.FacPer40);
            if (!dr.IsDBNull(iFacPer40)) entity.FacPer40 = dr.GetDecimal(iFacPer40);

            int iFacPer41 = dr.GetOrdinal(this.FacPer41);
            if (!dr.IsDBNull(iFacPer41)) entity.FacPer41 = dr.GetDecimal(iFacPer41);

            int iFacPer42 = dr.GetOrdinal(this.FacPer42);
            if (!dr.IsDBNull(iFacPer42)) entity.FacPer42 = dr.GetDecimal(iFacPer42);

            int iFacPer43 = dr.GetOrdinal(this.FacPer43);
            if (!dr.IsDBNull(iFacPer43)) entity.FacPer43 = dr.GetDecimal(iFacPer43);

            int iFacPer44 = dr.GetOrdinal(this.FacPer44);
            if (!dr.IsDBNull(iFacPer44)) entity.FacPer44 = dr.GetDecimal(iFacPer44);

            int iFacPer45 = dr.GetOrdinal(this.FacPer45);
            if (!dr.IsDBNull(iFacPer45)) entity.FacPer45 = dr.GetDecimal(iFacPer45);

            int iFacPer46 = dr.GetOrdinal(this.FacPer46);
            if (!dr.IsDBNull(iFacPer46)) entity.FacPer46 = dr.GetDecimal(iFacPer46);

            int iFacPer47 = dr.GetOrdinal(this.FacPer47);
            if (!dr.IsDBNull(iFacPer47)) entity.FacPer47 = dr.GetDecimal(iFacPer47);

            int iFacPer48 = dr.GetOrdinal(this.FacPer48);
            if (!dr.IsDBNull(iFacPer48)) entity.FacPer48 = dr.GetDecimal(iFacPer48);

            int iFacPerUserName = dr.GetOrdinal(this.FacPerUserName);
            if (!dr.IsDBNull(iFacPerUserName)) entity.FacPerUserName = dr.GetString(iFacPerUserName);

            int iFacPerFecIns = dr.GetOrdinal(this.FacPerFecIns);
            if (!dr.IsDBNull(iFacPerFecIns)) entity.FacPerFecIns = dr.GetDateTime(iFacPerFecIns);

            return entity;
        }

        #region Mapeo de Campos

        public string FacPerCodi = "FACPERCODI";
        public string CongeneCodi = "CONGENECODI";
        public string BarrCodi = "BARRCODI";
        public string FacPerBarrNombre = "FACPERBARRNOMBRE";
        public string PeriCodi = "PERICODI";
        public string FacPerBase = "FACPERBASE";
        public string CongeneBase = "CONGENEBASE";
        public string FacPerVersion = "FACPERVERSION";
        public string CongeneVersion = "CONGENEVERSION";
        public string FacPerDia = "FACPERDIA";
        public string FacPer1 = "FACPER1";
        public string FacPer2 = "FACPER2";
        public string FacPer3 = "FACPER3";
        public string FacPer4 = "FACPER4";
        public string FacPer5 = "FACPER5";
        public string FacPer6 = "FACPER6";
        public string FacPer7 = "FACPER7";
        public string FacPer8 = "FACPER8";
        public string FacPer9 = "FACPER9";
        public string FacPer10 = "FACPER10";
        public string FacPer11 = "FACPER11";
        public string FacPer12 = "FACPER12";
        public string FacPer13 = "FACPER13";
        public string FacPer14 = "FACPER14";
        public string FacPer15 = "FACPER15";
        public string FacPer16 = "FACPER16";
        public string FacPer17 = "FACPER17";
        public string FacPer18 = "FACPER18";
        public string FacPer19 = "FACPER19";
        public string FacPer20 = "FACPER20";
        public string FacPer21 = "FACPER21";
        public string FacPer22 = "FACPER22";
        public string FacPer23 = "FACPER23";
        public string FacPer24 = "FACPER24";
        public string FacPer25 = "FACPER25";
        public string FacPer26 = "FACPER26";
        public string FacPer27 = "FACPER27";
        public string FacPer28 = "FACPER28";
        public string FacPer29 = "FACPER29";
        public string FacPer30 = "FACPER30";
        public string FacPer31 = "FACPER31";
        public string FacPer32 = "FACPER32";
        public string FacPer33 = "FACPER33";
        public string FacPer34 = "FACPER34";
        public string FacPer35 = "FACPER35";
        public string FacPer36 = "FACPER36";
        public string FacPer37 = "FACPER37";
        public string FacPer38 = "FACPER38";
        public string FacPer39 = "FACPER39";
        public string FacPer40 = "FACPER40";
        public string FacPer41 = "FACPER41";
        public string FacPer42 = "FACPER42";
        public string FacPer43 = "FACPER43";
        public string FacPer44 = "FACPER44";
        public string FacPer45 = "FACPER45";
        public string FacPer46 = "FACPER46";
        public string FacPer47 = "FACPER47";
        public string FacPer48 = "FACPER48";
        public string FacPerUserName = "FACPERUSERNAME";
        public string CongeneUserName = "CONGENEUSERNAME";
        public string FacPerFecIns = "FACPERFECINS";
        public string TableName = "TRN_FACTOR_PERDIDA";

        //atributos de consulta
        public string CosMarCodi = "COSMARCODI";
        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListByPeriodoVersion
        {
            get { return base.GetSqlXml("ListByPeriodoVersion"); }
        }

        public string SqlCodigoGeneradoDec
        {
            get { return base.GetSqlXml("GetMinId"); }
        }

        public string SqlCopiarFactorPerdida
        {
            get { return base.GetSqlXml("CopiarFactorPerdida"); }
        }

        public string SqlCopiarFactorPerdidaCongestion
        {
            get { return base.GetSqlXml("CopiarFactorPerdidaCongestion"); }
        }

        public string SqlCopiarCostoMarginal
        {
            get { return base.GetSqlXml("CopiarCostoMarginal"); }
        }

        public string SqlCopiarTemporal
        {
            get { return base.GetSqlXml("CopiarTemporal"); }
        }
        
        public string SqlCopiarSGOCOES
        {
            get { return base.GetSqlXml("CopiarSGOCOES"); }
        }

        public string SqlCopiarSGOCOESCongestion
        {
            get { return base.GetSqlXml("CopiarSGOCOES_CONGESTION"); }
        }
             public string SqlCopiarSGOCOESEnergia
        {
            get { return base.GetSqlXml("CopiarSGOCOES_ENERGIA"); }
        }

        public string SqlCopiarSGOCOESCM
        {
            get { return base.GetSqlXml("CopiarSGOCOESCM"); }
        }

        //ASSETEC 202002
        public string SqlDeleteCMTMP
        {
            get { return base.GetSqlXml("DeleteCMTMP"); }
        }
        
        public string SqlListBarrasSiCostMarg
        {
            get { return base.GetSqlXml("ListBarrasSiCostMarg"); }
        }

        public string SqlSaveCostMargTmp
        {
            get { return base.GetSqlXml("SaveCostMargTmp"); }
        }

        public string SqlListFechaXBarraSiCostMarg
        {
            get { return base.GetSqlXml("ListFechaXBarraSiCostMarg"); }
        }
    }
}
