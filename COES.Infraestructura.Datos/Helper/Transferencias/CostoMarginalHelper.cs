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
    /// Clase que contiene el mapeo de la tabla TRN_COSTO_MARGINAL
    /// </summary>
    public class CostoMarginalHelper : HelperBase
    {
        public CostoMarginalHelper() : base(Consultas.CostoMarginalSql)
        {
        }

        public CostoMarginalDTO Create(IDataReader dr)
        {
            CostoMarginalDTO entity = new CostoMarginalDTO();

            int iCosMarCodi = dr.GetOrdinal(this.CosMarCodi);
            if (!dr.IsDBNull(iCosMarCodi)) entity.CosMarCodi = dr.GetInt32(iCosMarCodi);

            int iBarrCodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

            int iCosMarBarraTransferencia = dr.GetOrdinal(this.CosMarBarraTransferencia);
            if (!dr.IsDBNull(iCosMarBarraTransferencia)) entity.CosMarBarraTransferencia = dr.GetString(iCosMarBarraTransferencia);

            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

             int iFacPerCodi = dr.GetOrdinal(this.FacPerCodi);
            if (!dr.IsDBNull(iFacPerCodi)) entity.FacPerCodi= dr.GetInt32(iFacPerCodi);
        
            int iCosMarVersion = dr.GetOrdinal(this.CosMarVersion);
            if (!dr.IsDBNull(iCosMarVersion)) entity.CosMarVersion = dr.GetInt32(iCosMarVersion);

            int iCosMarDia = dr.GetOrdinal(this.CosMarDia);
            if (!dr.IsDBNull(iCosMarDia)) entity.CosMarDia = dr.GetInt32(iCosMarDia);

            int iCosMar1 = dr.GetOrdinal(this.CosMar1);
            if (!dr.IsDBNull(iCosMar1)) entity.CosMar1 = dr.GetDecimal(iCosMar1);

            int iCosMar2 = dr.GetOrdinal(this.CosMar2);
            if (!dr.IsDBNull(iCosMar2)) entity.CosMar2 = dr.GetDecimal(iCosMar2);

            int iCosMar3 = dr.GetOrdinal(this.CosMar3);
            if (!dr.IsDBNull(iCosMar3)) entity.CosMar3 = dr.GetDecimal(iCosMar3);

            int iCosMar4 = dr.GetOrdinal(this.CosMar4);
            if (!dr.IsDBNull(iCosMar4)) entity.CosMar4 = dr.GetDecimal(iCosMar4);

            int iCosMar5 = dr.GetOrdinal(this.CosMar5);
            if (!dr.IsDBNull(iCosMar5)) entity.CosMar5 = dr.GetDecimal(iCosMar5);

            int iCosMar6 = dr.GetOrdinal(this.CosMar6);
            if (!dr.IsDBNull(iCosMar6)) entity.CosMar6 = dr.GetDecimal(iCosMar6);

            int iCosMar7 = dr.GetOrdinal(this.CosMar7);
            if (!dr.IsDBNull(iCosMar7)) entity.CosMar7 = dr.GetDecimal(iCosMar7);

            int iCosMar8 = dr.GetOrdinal(this.CosMar8);
            if (!dr.IsDBNull(iCosMar8)) entity.CosMar8 = dr.GetDecimal(iCosMar8);

            int iCosMar9 = dr.GetOrdinal(this.CosMar9);
            if (!dr.IsDBNull(iCosMar9)) entity.CosMar9 = dr.GetDecimal(iCosMar9);

            int iCosMar10 = dr.GetOrdinal(this.CosMar10);
            if (!dr.IsDBNull(iCosMar10)) entity.CosMar10 = dr.GetDecimal(iCosMar10);

            int iCosMar11 = dr.GetOrdinal(this.CosMar11);
            if (!dr.IsDBNull(iCosMar11)) entity.CosMar11 = dr.GetDecimal(iCosMar11);

            int iCosMar12 = dr.GetOrdinal(this.CosMar12);
            if (!dr.IsDBNull(iCosMar12)) entity.CosMar12 = dr.GetDecimal(iCosMar12);

            int iCosMar13 = dr.GetOrdinal(this.CosMar13);
            if (!dr.IsDBNull(iCosMar13)) entity.CosMar13 = dr.GetDecimal(iCosMar13);

            int iCosMar14 = dr.GetOrdinal(this.CosMar14);
            if (!dr.IsDBNull(iCosMar14)) entity.CosMar14 = dr.GetDecimal(iCosMar14);

            int iCosMar15 = dr.GetOrdinal(this.CosMar15);
            if (!dr.IsDBNull(iCosMar15)) entity.CosMar15 = dr.GetDecimal(iCosMar15);

            int iCosMar16 = dr.GetOrdinal(this.CosMar16);
            if (!dr.IsDBNull(iCosMar16)) entity.CosMar16 = dr.GetDecimal(iCosMar16);

            int iCosMar17 = dr.GetOrdinal(this.CosMar17);
            if (!dr.IsDBNull(iCosMar17)) entity.CosMar17 = dr.GetDecimal(iCosMar17);

            int iCosMar18 = dr.GetOrdinal(this.CosMar18);
            if (!dr.IsDBNull(iCosMar18)) entity.CosMar18 = dr.GetDecimal(iCosMar18);

            int iCosMar19 = dr.GetOrdinal(this.CosMar19);
            if (!dr.IsDBNull(iCosMar19)) entity.CosMar19 = dr.GetDecimal(iCosMar19);

            int iCosMar20 = dr.GetOrdinal(this.CosMar20);
            if (!dr.IsDBNull(iCosMar20)) entity.CosMar20 = dr.GetDecimal(iCosMar20);

            int iCosMar21 = dr.GetOrdinal(this.CosMar21);
            if (!dr.IsDBNull(iCosMar21)) entity.CosMar21 = dr.GetDecimal(iCosMar21);

            int iCosMar22 = dr.GetOrdinal(this.CosMar22);
            if (!dr.IsDBNull(iCosMar22)) entity.CosMar22 = dr.GetDecimal(iCosMar22);

            int iCosMar23 = dr.GetOrdinal(this.CosMar23);
            if (!dr.IsDBNull(iCosMar23)) entity.CosMar23 = dr.GetDecimal(iCosMar23);

            int iCosMar24 = dr.GetOrdinal(this.CosMar24);
            if (!dr.IsDBNull(iCosMar24)) entity.CosMar24 = dr.GetDecimal(iCosMar24);

            int iCosMar25 = dr.GetOrdinal(this.CosMar25);
            if (!dr.IsDBNull(iCosMar25)) entity.CosMar25 = dr.GetDecimal(iCosMar25);

            int iCosMar26 = dr.GetOrdinal(this.CosMar26);
            if (!dr.IsDBNull(iCosMar26)) entity.CosMar26 = dr.GetDecimal(iCosMar26);

            int iCosMar27 = dr.GetOrdinal(this.CosMar27);
            if (!dr.IsDBNull(iCosMar27)) entity.CosMar27 = dr.GetDecimal(iCosMar27);

            int iCosMar28 = dr.GetOrdinal(this.CosMar28);
            if (!dr.IsDBNull(iCosMar28)) entity.CosMar28 = dr.GetDecimal(iCosMar28);

            int iCosMar29 = dr.GetOrdinal(this.CosMar29);
            if (!dr.IsDBNull(iCosMar29)) entity.CosMar29 = dr.GetDecimal(iCosMar29);

            int iCosMar30 = dr.GetOrdinal(this.CosMar30);
            if (!dr.IsDBNull(iCosMar30)) entity.CosMar30 = dr.GetDecimal(iCosMar30);

            int iCosMar31 = dr.GetOrdinal(this.CosMar31);
            if (!dr.IsDBNull(iCosMar31)) entity.CosMar31 = dr.GetDecimal(iCosMar31);

            int iCosMar32 = dr.GetOrdinal(this.CosMar32);
            if (!dr.IsDBNull(iCosMar32)) entity.CosMar32 = dr.GetDecimal(iCosMar32);

            int iCosMar33 = dr.GetOrdinal(this.CosMar33);
            if (!dr.IsDBNull(iCosMar33)) entity.CosMar33 = dr.GetDecimal(iCosMar33);

            int iCosMar34 = dr.GetOrdinal(this.CosMar34);
            if (!dr.IsDBNull(iCosMar34)) entity.CosMar34 = dr.GetDecimal(iCosMar34);

            int iCosMar35 = dr.GetOrdinal(this.CosMar35);
            if (!dr.IsDBNull(iCosMar35)) entity.CosMar35 = dr.GetDecimal(iCosMar35);

            int iCosMar36 = dr.GetOrdinal(this.CosMar36);
            if (!dr.IsDBNull(iCosMar36)) entity.CosMar36 = dr.GetDecimal(iCosMar36);

            int iCosMar37 = dr.GetOrdinal(this.CosMar37);
            if (!dr.IsDBNull(iCosMar37)) entity.CosMar37 = dr.GetDecimal(iCosMar37);

            int iCosMar38 = dr.GetOrdinal(this.CosMar38);
            if (!dr.IsDBNull(iCosMar38)) entity.CosMar38 = dr.GetDecimal(iCosMar38);

            int iCosMar39 = dr.GetOrdinal(this.CosMar39);
            if (!dr.IsDBNull(iCosMar39)) entity.CosMar39 = dr.GetDecimal(iCosMar39);

            int iCosMar40 = dr.GetOrdinal(this.CosMar40);
            if (!dr.IsDBNull(iCosMar40)) entity.CosMar40 = dr.GetDecimal(iCosMar40);

            int iCosMar41 = dr.GetOrdinal(this.CosMar41);
            if (!dr.IsDBNull(iCosMar41)) entity.CosMar41 = dr.GetDecimal(iCosMar41);

            int iCosMar42 = dr.GetOrdinal(this.CosMar42);
            if (!dr.IsDBNull(iCosMar42)) entity.CosMar42 = dr.GetDecimal(iCosMar42);

            int iCosMar43 = dr.GetOrdinal(this.CosMar43);
            if (!dr.IsDBNull(iCosMar43)) entity.CosMar43 = dr.GetDecimal(iCosMar43);

            int iCosMar44 = dr.GetOrdinal(this.CosMar44);
            if (!dr.IsDBNull(iCosMar44)) entity.CosMar44 = dr.GetDecimal(iCosMar44);

            int iCosMar45 = dr.GetOrdinal(this.CosMar45);
            if (!dr.IsDBNull(iCosMar45)) entity.CosMar45 = dr.GetDecimal(iCosMar45);

            int iCosMar46 = dr.GetOrdinal(this.CosMar46);
            if (!dr.IsDBNull(iCosMar46)) entity.CosMar46 = dr.GetDecimal(iCosMar46);

            int iCosMar47 = dr.GetOrdinal(this.CosMar47);
            if (!dr.IsDBNull(iCosMar47)) entity.CosMar47 = dr.GetDecimal(iCosMar47);

            int iCosMar48 = dr.GetOrdinal(this.CosMar48);
            if (!dr.IsDBNull(iCosMar48)) entity.CosMar48 = dr.GetDecimal(iCosMar48);

            int iCosMar49 = dr.GetOrdinal(this.CosMar49);
            if (!dr.IsDBNull(iCosMar49)) entity.CosMar49 = dr.GetDecimal(iCosMar49);

            int iCosMar50 = dr.GetOrdinal(this.CosMar50);
            if (!dr.IsDBNull(iCosMar50)) entity.CosMar50 = dr.GetDecimal(iCosMar50);

            int iCosMar51 = dr.GetOrdinal(this.CosMar51);
            if (!dr.IsDBNull(iCosMar51)) entity.CosMar51 = dr.GetDecimal(iCosMar51);

            int iCosMar52 = dr.GetOrdinal(this.CosMar52);
            if (!dr.IsDBNull(iCosMar52)) entity.CosMar52 = dr.GetDecimal(iCosMar52);

            int iCosMar53 = dr.GetOrdinal(this.CosMar53);
            if (!dr.IsDBNull(iCosMar53)) entity.CosMar53 = dr.GetDecimal(iCosMar53);

            int iCosMar54 = dr.GetOrdinal(this.CosMar54);
            if (!dr.IsDBNull(iCosMar54)) entity.CosMar54 = dr.GetDecimal(iCosMar54);

            int iCosMar55 = dr.GetOrdinal(this.CosMar55);
            if (!dr.IsDBNull(iCosMar55)) entity.CosMar55 = dr.GetDecimal(iCosMar55);

            int iCosMar56 = dr.GetOrdinal(this.CosMar56);
            if (!dr.IsDBNull(iCosMar56)) entity.CosMar56 = dr.GetDecimal(iCosMar56);

            int iCosMar57 = dr.GetOrdinal(this.CosMar57);
            if (!dr.IsDBNull(iCosMar57)) entity.CosMar57 = dr.GetDecimal(iCosMar57);

            int iCosMar58 = dr.GetOrdinal(this.CosMar58);
            if (!dr.IsDBNull(iCosMar58)) entity.CosMar58 = dr.GetDecimal(iCosMar58);

            int iCosMar59 = dr.GetOrdinal(this.CosMar59);
            if (!dr.IsDBNull(iCosMar59)) entity.CosMar59 = dr.GetDecimal(iCosMar59);

            int iCosMar60 = dr.GetOrdinal(this.CosMar60);
            if (!dr.IsDBNull(iCosMar60)) entity.CosMar60 = dr.GetDecimal(iCosMar60);

            int iCosMar61 = dr.GetOrdinal(this.CosMar61);
            if (!dr.IsDBNull(iCosMar61)) entity.CosMar61 = dr.GetDecimal(iCosMar61);

            int iCosMar62 = dr.GetOrdinal(this.CosMar62);
            if (!dr.IsDBNull(iCosMar62)) entity.CosMar62 = dr.GetDecimal(iCosMar62);

            int iCosMar63 = dr.GetOrdinal(this.CosMar63);
            if (!dr.IsDBNull(iCosMar63)) entity.CosMar63 = dr.GetDecimal(iCosMar63);

            int iCosMar64 = dr.GetOrdinal(this.CosMar64);
            if (!dr.IsDBNull(iCosMar64)) entity.CosMar64 = dr.GetDecimal(iCosMar64);

            int iCosMar65 = dr.GetOrdinal(this.CosMar65);
            if (!dr.IsDBNull(iCosMar65)) entity.CosMar65 = dr.GetDecimal(iCosMar65);

            int iCosMar66 = dr.GetOrdinal(this.CosMar66);
            if (!dr.IsDBNull(iCosMar66)) entity.CosMar66 = dr.GetDecimal(iCosMar66);

            int iCosMar67 = dr.GetOrdinal(this.CosMar67);
            if (!dr.IsDBNull(iCosMar67)) entity.CosMar67 = dr.GetDecimal(iCosMar67);

            int iCosMar68 = dr.GetOrdinal(this.CosMar68);
            if (!dr.IsDBNull(iCosMar68)) entity.CosMar68 = dr.GetDecimal(iCosMar68);

            int iCosMar69 = dr.GetOrdinal(this.CosMar69);
            if (!dr.IsDBNull(iCosMar6)) entity.CosMar69 = dr.GetDecimal(iCosMar69);

            int iCosMar70 = dr.GetOrdinal(this.CosMar70);
            if (!dr.IsDBNull(iCosMar70)) entity.CosMar70 = dr.GetDecimal(iCosMar70);

            int iCosMar71 = dr.GetOrdinal(this.CosMar71);
            if (!dr.IsDBNull(iCosMar71)) entity.CosMar71 = dr.GetDecimal(iCosMar71);

            int iCosMar72 = dr.GetOrdinal(this.CosMar72);
            if (!dr.IsDBNull(iCosMar72)) entity.CosMar72 = dr.GetDecimal(iCosMar72);

            int iCosMar73 = dr.GetOrdinal(this.CosMar73);
            if (!dr.IsDBNull(iCosMar73)) entity.CosMar73 = dr.GetDecimal(iCosMar73);

            int iCosMar74 = dr.GetOrdinal(this.CosMar74);
            if (!dr.IsDBNull(iCosMar74)) entity.CosMar74 = dr.GetDecimal(iCosMar74);

            int iCosMar75 = dr.GetOrdinal(this.CosMar75);
            if (!dr.IsDBNull(iCosMar75)) entity.CosMar75 = dr.GetDecimal(iCosMar75);

            int iCosMar76 = dr.GetOrdinal(this.CosMar76);
            if (!dr.IsDBNull(iCosMar76)) entity.CosMar76 = dr.GetDecimal(iCosMar76);

            int iCosMar77 = dr.GetOrdinal(this.CosMar77);
            if (!dr.IsDBNull(iCosMar77)) entity.CosMar77 = dr.GetDecimal(iCosMar77);

            int iCosMar78 = dr.GetOrdinal(this.CosMar78);
            if (!dr.IsDBNull(iCosMar78)) entity.CosMar78 = dr.GetDecimal(iCosMar78);

            int iCosMar79 = dr.GetOrdinal(this.CosMar79);
            if (!dr.IsDBNull(iCosMar79)) entity.CosMar79 = dr.GetDecimal(iCosMar79);

            int iCosMar80 = dr.GetOrdinal(this.CosMar80);
            if (!dr.IsDBNull(iCosMar80)) entity.CosMar80 = dr.GetDecimal(iCosMar80);

            int iCosMar81 = dr.GetOrdinal(this.CosMar81);
            if (!dr.IsDBNull(iCosMar81)) entity.CosMar81 = dr.GetDecimal(iCosMar81);

            int iCosMar82 = dr.GetOrdinal(this.CosMar82);
            if (!dr.IsDBNull(iCosMar82)) entity.CosMar82 = dr.GetDecimal(iCosMar82);

            int iCosMar83 = dr.GetOrdinal(this.CosMar83);
            if (!dr.IsDBNull(iCosMar83)) entity.CosMar83 = dr.GetDecimal(iCosMar83);

            int iCosMar84 = dr.GetOrdinal(this.CosMar84);
            if (!dr.IsDBNull(iCosMar84)) entity.CosMar84 = dr.GetDecimal(iCosMar84);

            int iCosMar85 = dr.GetOrdinal(this.CosMar85);
            if (!dr.IsDBNull(iCosMar85)) entity.CosMar85 = dr.GetDecimal(iCosMar85);

            int iCosMar86 = dr.GetOrdinal(this.CosMar86);
            if (!dr.IsDBNull(iCosMar86)) entity.CosMar86 = dr.GetDecimal(iCosMar86);

            int iCosMar87 = dr.GetOrdinal(this.CosMar87);
            if (!dr.IsDBNull(iCosMar87)) entity.CosMar87 = dr.GetDecimal(iCosMar87);

            int iCosMar88 = dr.GetOrdinal(this.CosMar88);
            if (!dr.IsDBNull(iCosMar88)) entity.CosMar88 = dr.GetDecimal(iCosMar88);

            int iCosMar89 = dr.GetOrdinal(this.CosMar89);
            if (!dr.IsDBNull(iCosMar89)) entity.CosMar89 = dr.GetDecimal(iCosMar89);

            int iCosMar90 = dr.GetOrdinal(this.CosMar90);
            if (!dr.IsDBNull(iCosMar90)) entity.CosMar90 = dr.GetDecimal(iCosMar90);

            int iCosMar91 = dr.GetOrdinal(this.CosMar91);
            if (!dr.IsDBNull(iCosMar91)) entity.CosMar91 = dr.GetDecimal(iCosMar91);

            int iCosMar92 = dr.GetOrdinal(this.CosMar92);
            if (!dr.IsDBNull(iCosMar92)) entity.CosMar92 = dr.GetDecimal(iCosMar92);

            int iCosMar93 = dr.GetOrdinal(this.CosMar93);
            if (!dr.IsDBNull(iCosMar93)) entity.CosMar93 = dr.GetDecimal(iCosMar93);

            int iCosMar94 = dr.GetOrdinal(this.CosMar94);
            if (!dr.IsDBNull(iCosMar94)) entity.CosMar94 = dr.GetDecimal(iCosMar94);

            int iCosMar95 = dr.GetOrdinal(this.CosMar95);
            if (!dr.IsDBNull(iCosMar95)) entity.CosMar95 = dr.GetDecimal(iCosMar95);

            int iCosMar96 = dr.GetOrdinal(this.CosMar96);
            if (!dr.IsDBNull(iCosMar96)) entity.CosMar96 = dr.GetDecimal(iCosMar96);    
          
            int iCosMarPromedioDia = dr.GetOrdinal(this.CosMarPromedioDia);
            if (!dr.IsDBNull(iCosMarPromedioDia)) entity.CosMarPromedioDia = dr.GetDecimal(iCosMarPromedioDia);

            int iCosMarUserName = dr.GetOrdinal(this.CosMarUserName);
            if (!dr.IsDBNull(iCosMarUserName)) entity.CosMarUserName = dr.GetString(iCosMarUserName);

            int iCosMarFecIns = dr.GetOrdinal(this.CosMarFecIns);
            if (!dr.IsDBNull(iCosMarFecIns)) entity.CosMarFecIns = dr.GetDateTime(iCosMarFecIns);

            // Inicio de Agregado 31/05/2017 - Sistema de Compensaciones
            //int iRecaCodi = dr.GetOrdinal(this.RecaCodi);
            //if (!dr.IsDBNull(iRecaCodi)) entity.RecaCodi = dr.GetInt32(iRecaCodi);

            //int iRecaNombre = dr.GetOrdinal(this.RecaNombre);
            //if (!dr.IsDBNull(iRecaNombre)) entity.RecaNombre = dr.GetString(iRecaNombre);
            // Fin de Agregado

            return entity;

        }


        #region Mapeo de Campos

        public string CosMarCodi = "COSMARCODI";
        public string BarrCodi = "BARRCODI";
        public string CosMarBarraTransferencia = "COSMARBARRATRANSFERENCIA";
        public string PeriCodi = "PERICODI";
        public string FacPerCodi = "FACPERCODI";
        public string CosMarVersion = "COSMARVERSION";
        public string CosMarDia = "COSMARDIA";
        public string CosMar1 = "COSMAR1";
        public string CosMar2 = "COSMAR2";
        public string CosMar3 = "COSMAR3";
        public string CosMar4 = "COSMAR4";
        public string CosMar5 = "COSMAR5";
        public string CosMar6 = "COSMAR6";
        public string CosMar7 = "COSMAR7";
        public string CosMar8 = "COSMAR8";
        public string CosMar9 = "COSMAR9";
        public string CosMar10 = "COSMAR10";
        public string CosMar11 = "COSMAR11";
        public string CosMar12 = "COSMAR12";
        public string CosMar13 = "COSMAR13";
        public string CosMar14 = "COSMAR14";
        public string CosMar15 = "COSMAR15";
        public string CosMar16 = "COSMAR16";
        public string CosMar17 = "COSMAR17";
        public string CosMar18 = "COSMAR18";
        public string CosMar19 = "COSMAR19";
        public string CosMar20 = "COSMAR20";
        public string CosMar21 = "COSMAR21";
        public string CosMar22 = "COSMAR22";
        public string CosMar23 = "COSMAR23";
        public string CosMar24 = "COSMAR24";
        public string CosMar25 = "COSMAR25";
        public string CosMar26 = "COSMAR26";
        public string CosMar27 = "COSMAR27";
        public string CosMar28 = "COSMAR28";
        public string CosMar29 = "COSMAR29";
        public string CosMar30 = "COSMAR30";
        public string CosMar31 = "COSMAR31";
        public string CosMar32 = "COSMAR32";
        public string CosMar33 = "COSMAR33";
        public string CosMar34 = "COSMAR34";
        public string CosMar35 = "COSMAR35";
        public string CosMar36 = "COSMAR36";
        public string CosMar37 = "COSMAR37";
        public string CosMar38 = "COSMAR38";
        public string CosMar39 = "COSMAR39";
        public string CosMar40 = "COSMAR40";
        public string CosMar41 = "COSMAR41";
        public string CosMar42 = "COSMAR42";
        public string CosMar43 = "COSMAR43";
        public string CosMar44 = "COSMAR44";
        public string CosMar45 = "COSMAR45";
        public string CosMar46 = "COSMAR46";
        public string CosMar47 = "COSMAR47";
        public string CosMar48 = "COSMAR48";
        public string CosMar49 = "COSMAR49";
        public string CosMar50 = "COSMAR50";
        public string CosMar51 = "COSMAR51";
        public string CosMar52 = "COSMAR52";
        public string CosMar53 = "COSMAR53";
        public string CosMar54 = "COSMAR54";
        public string CosMar55 = "COSMAR55";
        public string CosMar56 = "COSMAR56";
        public string CosMar57 = "COSMAR57";
        public string CosMar58 = "COSMAR58";
        public string CosMar59 = "COSMAR59";
        public string CosMar60 = "COSMAR60";
        public string CosMar61 = "COSMAR61";
        public string CosMar62 = "COSMAR62";
        public string CosMar63 = "COSMAR63";
        public string CosMar64 = "COSMAR64";
        public string CosMar65 = "COSMAR65";
        public string CosMar66 = "COSMAR66";
        public string CosMar67 = "COSMAR67";
        public string CosMar68 = "COSMAR68";
        public string CosMar69 = "COSMAR69";
        public string CosMar70 = "COSMAR70";
        public string CosMar71 = "COSMAR71";
        public string CosMar72 = "COSMAR72";
        public string CosMar73 = "COSMAR73";
        public string CosMar74 = "COSMAR74";
        public string CosMar75 = "COSMAR75";
        public string CosMar76 = "COSMAR76";
        public string CosMar77 = "COSMAR77";
        public string CosMar78 = "COSMAR78";
        public string CosMar79 = "COSMAR79";
        public string CosMar80 = "COSMAR80";
        public string CosMar81 = "COSMAR81";
        public string CosMar82 = "COSMAR82";
        public string CosMar83 = "COSMAR83";
        public string CosMar84 = "COSMAR84";
        public string CosMar85 = "COSMAR85";
        public string CosMar86 = "COSMAR86";
        public string CosMar87 = "COSMAR87";
        public string CosMar88 = "COSMAR88";
        public string CosMar89 = "COSMAR89";
        public string CosMar90 = "COSMAR90";
        public string CosMar91 = "COSMAR91";
        public string CosMar92 = "COSMAR92";
        public string CosMar93 = "COSMAR93";
        public string CosMar94 = "COSMAR94";
        public string CosMar95 = "COSMAR95";
        public string CosMar96 = "COSMAR96";
        public string CosMarPromedioDia = "COSMARPROMEDIODIA";
        public string CosMarUserName = "COSMARUSERNAME";
        public string CosMarFecIns = "COSMARFECINS";
        public string CosMar = "CosMar";
        public string H = "H";
        public string TableName = "TRN_COSTO_MARGINAL";
        // Inicio de Agregado 31/05/2017 - Sistema de Compensaciones
        public string RecaCodi = "RECACODI";
        public string RecaNombre = "RECANOMBRE";
        // Fin de Agregado

        #region MonitoreoMME
        public string Emprcodi = "Emprcodi";
        public string Emprnomb = "EmprNomb";
        public string Grupocodi = "GrupoCodi";
        public string Barrnombre = "BarrNombre";
        #endregion

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListByBarraPeriodoVers
        {
            get { return base.GetSqlXml("ListByBarrPeriodoVer"); }
        }

        public string SqlGetByCodigo
        {
            get { return base.GetSqlXml("GetByCodigo"); }
        }
        public string SqlGetConsultaCostosMarginales
        {
            get { return base.GetSqlXml("GetConsultaCostosMarginales"); }
        }

        public string SqlGetByBarraTransferencia
        {
            get { return base.GetSqlXml("GetByBarraTransferencia"); }
        }

        public string SqlListByFactorPerdida
        {
            get { return base.GetSqlXml("ListByFactorPerdida"); }
        }

        public string SqlListByReporte 
        {
            get { return base.GetSqlXml("ListByReporte"); }
        }       public string SqlGetBarrasMarginales
        {
            get { return base.GetSqlXml("GetBarrasMarginales"); }
        }

        public string SqlObtenerReporteCostoMarginalDTR
        {
            get { return base.GetSqlXml("ObtenerReporteCostoMarginalDTR"); }
        }

        // Inicio de Agregado - Sistema de Compensaciones
        public string SqlListByPeriCodi
        {
            get { return base.GetSqlXml("ListByPeriCodi"); }
        }

        // Fin de Agregado - Sistema de Compensaciones
        public string SqlGetByIdPorURS
        {
            get { return base.GetSqlXml("GetByIdPorURS"); }
        }

        public string SqlDeleteCongene
        {
            get { return base.GetSqlXml("DeleteCongene"); }
        }

        #region Siosein
        
        public string SqlListByPeriodo
        {
            get { return base.GetSqlXml("ListByPeriodo"); }
        }

        public string SqlListBarrasByPeriodo
        {
            get { return base.GetSqlXml("ListBarrasByPeriodo"); }
        }

        public string SqlListByDiaXBarra
        {
            get { return base.GetSqlXml("ListByDiaXBarra"); }
        }
        
        #endregion

        public string SqlCodigoGeneradoDec
        {
            get { return base.GetSqlXml("GetMinId"); }
        }

        #region MonitoreoMME

        public string SqlListCostoMarginalWithGrupo
        {
            get { return base.GetSqlXml("ListCostoMarginalWithGrupo"); }
        }

        #endregion

        #region Siosein2
        public string Barrzarea = "BARRZAREA";

        public string SqlListCostoMarginalByPeriodoVersionZona
        {
            get { return base.GetSqlXml("ListCostoMarginalByPeriodoVersionZona"); }
        }
        #endregion

        #region AJUSTE COSTOS MARGINALES
        public string SqlAjustarCostosMarginales
        {
            get { return base.GetSqlXml("AjustarCostosMarginales"); }
        }
        #endregion

        //CU21
        public string SqlListarByCodigoEntrega
        {
            get { return base.GetSqlXml("ListarByCodigoEntrega"); }
        }
    }
}
