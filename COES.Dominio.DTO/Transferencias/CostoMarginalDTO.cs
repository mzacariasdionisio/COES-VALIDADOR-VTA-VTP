using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_COSTO_MARGINAL
    /// </summary>
    [Serializable]
    public class CostoMarginalDTO
    {
        public int tipoCongene { get; set; }
        public string tipoPromedio { get; set; }
        public decimal? CMGRCongestion { get; set; }
        public decimal? CMGREnergia { get; set; }
        public string TipoCostoMarginal { get; set; }
        public List<int> ListaBarras { get; set; }
        public string FechaColumna { get; set; }
        public DateTime FechaIntervalo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiaInicio { get; set; }
        public int DiaFin { get; set; }
        public System.Int32 CosMarCodi { get; set; }
        public System.Int32 BarrCodi { get; set; }
        public System.String CosMarBarraTransferencia { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Int32 PeriCodiFin { get; set; }
        public System.Int32 FacPerCodi { get; set; }
        public System.Int32 CosMarVersion { get; set; }
        public System.Int32 CosMarDia { get; set; }
        public System.Decimal? CMGRTotal { get; set; }
        public System.Decimal? CMGRPromedio { get; set; }
        public System.Decimal CosMar1 { get; set; }
        public System.Decimal CosMar2 { get; set; }
        public System.Decimal CosMar3 { get; set; }
        public System.Decimal CosMar4 { get; set; }
        public System.Decimal CosMar5 { get; set; }
        public System.Decimal CosMar6 { get; set; }
        public System.Decimal CosMar7 { get; set; }
        public System.Decimal CosMar8 { get; set; }
        public System.Decimal CosMar9 { get; set; }
        public System.Decimal CosMar10 { get; set; }
        public System.Decimal CosMar11 { get; set; }
        public System.Decimal CosMar12 { get; set; }
        public System.Decimal CosMar13 { get; set; }
        public System.Decimal CosMar14 { get; set; }
        public System.Decimal CosMar15 { get; set; }
        public System.Decimal CosMar16 { get; set; }
        public System.Decimal CosMar17 { get; set; }
        public System.Decimal CosMar18 { get; set; }
        public System.Decimal CosMar19 { get; set; }
        public System.Decimal CosMar20 { get; set; }
        public System.Decimal CosMar21 { get; set; }
        public System.Decimal CosMar22 { get; set; }
        public System.Decimal CosMar23 { get; set; }
        public System.Decimal CosMar24 { get; set; }
        public System.Decimal CosMar25 { get; set; }
        public System.Decimal CosMar26 { get; set; }
        public System.Decimal CosMar27 { get; set; }
        public System.Decimal CosMar28 { get; set; }
        public System.Decimal CosMar29 { get; set; }
        public System.Decimal CosMar30 { get; set; }
        public System.Decimal CosMar31 { get; set; }
        public System.Decimal CosMar32 { get; set; }
        public System.Decimal CosMar33 { get; set; }
        public System.Decimal CosMar34 { get; set; }
        public System.Decimal CosMar35 { get; set; }
        public System.Decimal CosMar36 { get; set; }
        public System.Decimal CosMar37 { get; set; }
        public System.Decimal CosMar38 { get; set; }
        public System.Decimal CosMar39 { get; set; }
        public System.Decimal CosMar40 { get; set; }
        public System.Decimal CosMar41 { get; set; }
        public System.Decimal CosMar42 { get; set; }
        public System.Decimal CosMar43 { get; set; }
        public System.Decimal CosMar44 { get; set; }
        public System.Decimal CosMar45 { get; set; }
        public System.Decimal CosMar46 { get; set; }
        public System.Decimal CosMar47 { get; set; }
        public System.Decimal CosMar48 { get; set; }
        public System.Decimal CosMar49 { get; set; }
        public System.Decimal CosMar50 { get; set; }
        public System.Decimal CosMar51 { get; set; }
        public System.Decimal CosMar52 { get; set; }
        public System.Decimal CosMar53 { get; set; }
        public System.Decimal CosMar54 { get; set; }
        public System.Decimal CosMar55 { get; set; }
        public System.Decimal CosMar56 { get; set; }
        public System.Decimal CosMar57 { get; set; }
        public System.Decimal CosMar58 { get; set; }
        public System.Decimal CosMar59 { get; set; }
        public System.Decimal CosMar60 { get; set; }
        public System.Decimal CosMar61 { get; set; }
        public System.Decimal CosMar62 { get; set; }
        public System.Decimal CosMar63 { get; set; }
        public System.Decimal CosMar64 { get; set; }
        public System.Decimal CosMar65 { get; set; }
        public System.Decimal CosMar66 { get; set; }
        public System.Decimal CosMar67 { get; set; }
        public System.Decimal CosMar68 { get; set; }
        public System.Decimal CosMar69 { get; set; }
        public System.Decimal CosMar70 { get; set; }
        public System.Decimal CosMar71 { get; set; }
        public System.Decimal CosMar72 { get; set; }
        public System.Decimal CosMar73 { get; set; }
        public System.Decimal CosMar74 { get; set; }
        public System.Decimal CosMar75 { get; set; }
        public System.Decimal CosMar76 { get; set; }
        public System.Decimal CosMar77 { get; set; }
        public System.Decimal CosMar78 { get; set; }
        public System.Decimal CosMar79 { get; set; }
        public System.Decimal CosMar80 { get; set; }
        public System.Decimal CosMar81 { get; set; }
        public System.Decimal CosMar82 { get; set; }
        public System.Decimal CosMar83 { get; set; }
        public System.Decimal CosMar84 { get; set; }
        public System.Decimal CosMar85 { get; set; }
        public System.Decimal CosMar86 { get; set; }
        public System.Decimal CosMar87 { get; set; }
        public System.Decimal CosMar88 { get; set; }
        public System.Decimal CosMar89 { get; set; }
        public System.Decimal CosMar90 { get; set; }
        public System.Decimal CosMar91 { get; set; }
        public System.Decimal CosMar92 { get; set; }
        public System.Decimal CosMar93 { get; set; }
        public System.Decimal CosMar94 { get; set; }
        public System.Decimal CosMar95 { get; set; }
        public System.Decimal CosMar96 { get; set; }
        public System.Decimal CosMarPromedioDia { get; set; }
        public System.String CosMarUserName { get; set; }
        public System.DateTime CosMarFecIns { get; set; }
        public System.Int32 RecaCodi { get; set; }
        public System.String RecaNombre { get; set; }
        public string BarrCodigo { get; set; }

        public System.String Emprnomb { get; set; }
        public System.Int32 Emprcodi { get; set; }
        public System.Int32 GrupoCodi { get; set; }
        public System.Int32 GrupoPadre { get; set; }
        public System.String GrupoNomb { get; set; }
        public System.String Barrnombre { get; set; }

        #region PR5_Informe_Ejecutivo_Semanal
        public int AnioSemana { get; set; }
        #endregion

        #region siosein2 
        public int PeriAnioMes { get; set; }
        public int Barrzarea { get; set; }
        public decimal? CosMarPromMes { get; set; }
        public decimal? CosMarPromDia { get; set; }
        public decimal? CosMarMaxMes { get; set; }
        public decimal? CosMarMinMes { get; set; }
        public string Osinergcodi { get; set; }
        public DateTime Fecha { get; set; }
        #endregion
        //CU21
        public System.Decimal CosMarTotalDia { get; set; }
    }

    public class TrnCostoMarginalBullk
    {
        public System.Int32 Cosmarcodi { get; set; }
        public System.Int32 Pericodi { get; set; }
        public System.Int32 Barrcodi { get; set; }
        public System.Int32 Facpercodi { get; set; }
        public System.Int32 Cosmarversion { get; set; }
        public System.Int32 Cosmardia { get; set; }
        public System.String Cosmarbarratransferencia { get; set; }
        public System.Decimal Cosmarpromediodia { get; set; }
        public System.Decimal Cosmar1 { get; set; }
        public System.Decimal Cosmar2 { get; set; }
        public System.Decimal Cosmar3 { get; set; }
        public System.Decimal Cosmar4 { get; set; }
        public System.Decimal Cosmar5 { get; set; }
        public System.Decimal Cosmar6 { get; set; }
        public System.Decimal Cosmar7 { get; set; }
        public System.Decimal Cosmar8 { get; set; }
        public System.Decimal Cosmar9 { get; set; }
        public System.Decimal Cosmar10 { get; set; }
        public System.Decimal Cosmar11 { get; set; }
        public System.Decimal Cosmar12 { get; set; }
        public System.Decimal Cosmar13 { get; set; }
        public System.Decimal Cosmar14 { get; set; }
        public System.Decimal Cosmar15 { get; set; }
        public System.Decimal Cosmar16 { get; set; }
        public System.Decimal Cosmar17 { get; set; }
        public System.Decimal Cosmar18 { get; set; }
        public System.Decimal Cosmar19 { get; set; }
        public System.Decimal Cosmar20 { get; set; }
        public System.Decimal Cosmar21 { get; set; }
        public System.Decimal Cosmar22 { get; set; }
        public System.Decimal Cosmar23 { get; set; }
        public System.Decimal Cosmar24 { get; set; }
        public System.Decimal Cosmar25 { get; set; }
        public System.Decimal Cosmar26 { get; set; }
        public System.Decimal Cosmar27 { get; set; }
        public System.Decimal Cosmar28 { get; set; }
        public System.Decimal Cosmar29 { get; set; }
        public System.Decimal Cosmar30 { get; set; }
        public System.Decimal Cosmar31 { get; set; }
        public System.Decimal Cosmar32 { get; set; }
        public System.Decimal Cosmar33 { get; set; }
        public System.Decimal Cosmar34 { get; set; }
        public System.Decimal Cosmar35 { get; set; }
        public System.Decimal Cosmar36 { get; set; }
        public System.Decimal Cosmar37 { get; set; }
        public System.Decimal Cosmar38 { get; set; }
        public System.Decimal Cosmar39 { get; set; }
        public System.Decimal Cosmar40 { get; set; }
        public System.Decimal Cosmar41 { get; set; }
        public System.Decimal Cosmar42 { get; set; }
        public System.Decimal Cosmar43 { get; set; }
        public System.Decimal Cosmar44 { get; set; }
        public System.Decimal Cosmar45 { get; set; }
        public System.Decimal Cosmar46 { get; set; }
        public System.Decimal Cosmar47 { get; set; }
        public System.Decimal Cosmar48 { get; set; }
        public System.Decimal Cosmar49 { get; set; }
        public System.Decimal Cosmar50 { get; set; }
        public System.Decimal Cosmar51 { get; set; }
        public System.Decimal Cosmar52 { get; set; }
        public System.Decimal Cosmar53 { get; set; }
        public System.Decimal Cosmar54 { get; set; }
        public System.Decimal Cosmar55 { get; set; }
        public System.Decimal Cosmar56 { get; set; }
        public System.Decimal Cosmar57 { get; set; }
        public System.Decimal Cosmar58 { get; set; }
        public System.Decimal Cosmar59 { get; set; }
        public System.Decimal Cosmar60 { get; set; }
        public System.Decimal Cosmar61 { get; set; }
        public System.Decimal Cosmar62 { get; set; }
        public System.Decimal Cosmar63 { get; set; }
        public System.Decimal Cosmar64 { get; set; }
        public System.Decimal Cosmar65 { get; set; }
        public System.Decimal Cosmar66 { get; set; }
        public System.Decimal Cosmar67 { get; set; }
        public System.Decimal Cosmar68 { get; set; }
        public System.Decimal Cosmar69 { get; set; }
        public System.Decimal Cosmar70 { get; set; }
        public System.Decimal Cosmar71 { get; set; }
        public System.Decimal Cosmar72 { get; set; }
        public System.Decimal Cosmar73 { get; set; }
        public System.Decimal Cosmar74 { get; set; }
        public System.Decimal Cosmar75 { get; set; }
        public System.Decimal Cosmar76 { get; set; }
        public System.Decimal Cosmar77 { get; set; }
        public System.Decimal Cosmar78 { get; set; }
        public System.Decimal Cosmar79 { get; set; }
        public System.Decimal Cosmar80 { get; set; }
        public System.Decimal Cosmar81 { get; set; }
        public System.Decimal Cosmar82 { get; set; }
        public System.Decimal Cosmar83 { get; set; }
        public System.Decimal Cosmar84 { get; set; }
        public System.Decimal Cosmar85 { get; set; }
        public System.Decimal Cosmar86 { get; set; }
        public System.Decimal Cosmar87 { get; set; }
        public System.Decimal Cosmar88 { get; set; }
        public System.Decimal Cosmar89 { get; set; }
        public System.Decimal Cosmar90 { get; set; }
        public System.Decimal Cosmar91 { get; set; }
        public System.Decimal Cosmar92 { get; set; }
        public System.Decimal Cosmar93 { get; set; }
        public System.Decimal Cosmar94 { get; set; }
        public System.Decimal Cosmar95 { get; set; }
        public System.Decimal Cosmar96 { get; set; }
        public System.String Cosmarusername { get; set; }
        public System.DateTime Cosmarfecins { get; set; }
        public System.Int32 Recacodi { get; set; }
        public System.String Recanombre { get; set; }

        public TrnCostoMarginalBullk(CostoMarginalDTO entity)
        {
            this.Cosmarcodi = entity.CosMarCodi;
            this.Pericodi = entity.PeriCodi;
            this.Barrcodi = entity.BarrCodi;
            this.Facpercodi = entity.FacPerCodi;
            this.Cosmarversion = entity.CosMarVersion;
            this.Cosmardia = entity.CosMarDia;
            this.Cosmarbarratransferencia = entity.CosMarBarraTransferencia;
            this.Cosmarpromediodia = entity.CosMarPromedioDia;
            this.Cosmar1 = entity.CosMar1;
            this.Cosmar2 = entity.CosMar2;
            this.Cosmar3 = entity.CosMar3;
            this.Cosmar4 = entity.CosMar4;
            this.Cosmar5 = entity.CosMar5;
            this.Cosmar6 = entity.CosMar6;
            this.Cosmar7 = entity.CosMar7;
            this.Cosmar8 = entity.CosMar8;
            this.Cosmar9 = entity.CosMar9;
            this.Cosmar10 = entity.CosMar10;
            this.Cosmar11 = entity.CosMar11;
            this.Cosmar12 = entity.CosMar12;
            this.Cosmar13 = entity.CosMar13;
            this.Cosmar14 = entity.CosMar14;
            this.Cosmar15 = entity.CosMar15;
            this.Cosmar16 = entity.CosMar16;
            this.Cosmar17 = entity.CosMar17;
            this.Cosmar18 = entity.CosMar18;
            this.Cosmar19 = entity.CosMar19;
            this.Cosmar20 = entity.CosMar20;
            this.Cosmar21 = entity.CosMar21;
            this.Cosmar22 = entity.CosMar22;
            this.Cosmar23 = entity.CosMar23;
            this.Cosmar24 = entity.CosMar24;
            this.Cosmar25 = entity.CosMar25;
            this.Cosmar26 = entity.CosMar26;
            this.Cosmar27 = entity.CosMar27;
            this.Cosmar28 = entity.CosMar28;
            this.Cosmar29 = entity.CosMar29;
            this.Cosmar30 = entity.CosMar30;
            this.Cosmar31 = entity.CosMar31;
            this.Cosmar32 = entity.CosMar32;
            this.Cosmar33 = entity.CosMar33;
            this.Cosmar34 = entity.CosMar34;
            this.Cosmar35 = entity.CosMar35;
            this.Cosmar36 = entity.CosMar36;
            this.Cosmar37 = entity.CosMar37;
            this.Cosmar38 = entity.CosMar38;
            this.Cosmar39 = entity.CosMar39;
            this.Cosmar40 = entity.CosMar40;
            this.Cosmar41 = entity.CosMar41;
            this.Cosmar42 = entity.CosMar42;
            this.Cosmar43 = entity.CosMar43;
            this.Cosmar44 = entity.CosMar44;
            this.Cosmar45 = entity.CosMar45;
            this.Cosmar46 = entity.CosMar46;
            this.Cosmar47 = entity.CosMar47;
            this.Cosmar48 = entity.CosMar48;
            this.Cosmar49 = entity.CosMar49;
            this.Cosmar50 = entity.CosMar50;
            this.Cosmar51 = entity.CosMar51;
            this.Cosmar52 = entity.CosMar52;
            this.Cosmar53 = entity.CosMar53;
            this.Cosmar54 = entity.CosMar54;
            this.Cosmar55 = entity.CosMar55;
            this.Cosmar56 = entity.CosMar56;
            this.Cosmar57 = entity.CosMar57;
            this.Cosmar58 = entity.CosMar58;
            this.Cosmar59 = entity.CosMar59;
            this.Cosmar60 = entity.CosMar60;
            this.Cosmar61 = entity.CosMar61;
            this.Cosmar62 = entity.CosMar62;
            this.Cosmar63 = entity.CosMar63;
            this.Cosmar64 = entity.CosMar64;
            this.Cosmar65 = entity.CosMar65;
            this.Cosmar66 = entity.CosMar66;
            this.Cosmar67 = entity.CosMar67;
            this.Cosmar68 = entity.CosMar68;
            this.Cosmar69 = entity.CosMar69;
            this.Cosmar70 = entity.CosMar70;
            this.Cosmar71 = entity.CosMar71;
            this.Cosmar72 = entity.CosMar72;
            this.Cosmar73 = entity.CosMar73;
            this.Cosmar74 = entity.CosMar74;
            this.Cosmar75 = entity.CosMar75;
            this.Cosmar76 = entity.CosMar76;
            this.Cosmar77 = entity.CosMar77;
            this.Cosmar78 = entity.CosMar78;
            this.Cosmar79 = entity.CosMar79;
            this.Cosmar80 = entity.CosMar80;
            this.Cosmar81 = entity.CosMar81;
            this.Cosmar82 = entity.CosMar82;
            this.Cosmar83 = entity.CosMar83;
            this.Cosmar84 = entity.CosMar84;
            this.Cosmar85 = entity.CosMar85;
            this.Cosmar86 = entity.CosMar86;
            this.Cosmar87 = entity.CosMar87;
            this.Cosmar88 = entity.CosMar88;
            this.Cosmar89 = entity.CosMar89;
            this.Cosmar90 = entity.CosMar90;
            this.Cosmar91 = entity.CosMar91;
            this.Cosmar92 = entity.CosMar92;
            this.Cosmar93 = entity.CosMar93;
            this.Cosmar94 = entity.CosMar94;
            this.Cosmar95 = entity.CosMar95;
            this.Cosmar96 = entity.CosMar96;
            this.Cosmarusername = entity.CosMarUserName;
            this.Cosmarfecins = entity.CosMarFecIns;
            this.Recacodi = entity.RecaCodi;
            this.Recanombre = entity.RecaNombre;
        }
    }
}
