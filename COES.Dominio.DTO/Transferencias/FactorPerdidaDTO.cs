using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_FACTOR_PERDIDA
    /// </summary>
    public class FactorPerdidaDTO
    {
        public int FacPerCodi { get; set; }
        public int BarrCodi { get; set; }
        public string FacPerBarrNombre { get; set; }
        public int PeriCodi { get; set; }
        public string FacPerBase { get; set; }
        public int FacPerVersion { get; set; }
        public int FacPerDia { get; set; }
        public decimal FacPer1 { get; set; }
        public decimal FacPer2 { get; set; }
        public decimal FacPer3 { get; set; }
        public decimal FacPer4 { get; set; }
        public decimal FacPer5 { get; set; }
        public decimal FacPer6 { get; set; }
        public decimal FacPer7 { get; set; }
        public decimal FacPer8 { get; set; }
        public decimal FacPer9 { get; set; }
        public decimal FacPer10 { get; set; }
        public decimal FacPer11 { get; set; }
        public decimal FacPer12 { get; set; }
        public decimal FacPer13 { get; set; }
        public decimal FacPer14 { get; set; }
        public decimal FacPer15 { get; set; }
        public decimal FacPer16 { get; set; }
        public decimal FacPer17 { get; set; }
        public decimal FacPer18 { get; set; }
        public decimal FacPer19 { get; set; }
        public decimal FacPer20 { get; set; }
        public decimal FacPer21 { get; set; }
        public decimal FacPer22 { get; set; }
        public decimal FacPer23 { get; set; }
        public decimal FacPer24 { get; set; }
        public decimal FacPer25 { get; set; }
        public decimal FacPer26 { get; set; }
        public decimal FacPer27 { get; set; }
        public decimal FacPer28 { get; set; }
        public decimal FacPer29 { get; set; }
        public decimal FacPer30 { get; set; }
        public decimal FacPer31 { get; set; }
        public decimal FacPer32 { get; set; }
        public decimal FacPer33 { get; set; }
        public decimal FacPer34 { get; set; }
        public decimal FacPer35 { get; set; }
        public decimal FacPer36 { get; set; }
        public decimal FacPer37 { get; set; }
        public decimal FacPer38 { get; set; }
        public decimal FacPer39 { get; set; }
        public decimal FacPer40 { get; set; }
        public decimal FacPer41 { get; set; }
        public decimal FacPer42 { get; set; }
        public decimal FacPer43 { get; set; }
        public decimal FacPer44 { get; set; }
        public decimal FacPer45 { get; set; }
        public decimal FacPer46 { get; set; }
        public decimal FacPer47 { get; set; }
        public decimal FacPer48 { get; set; }
        public string FacPerUserName { get; set; }
        public DateTime FacPerFecIns { get; set; }

    }

    public class TrnFactorPerdidaBullkDTO
    {
        public int Facpercodi { get; set; }
        public int Pericodi { get; set; }
        public int Barrcodi { get; set; }
        public int Facperversion { get; set; }
        public int Facperdia { get; set; }
        public string Facperbase { get; set; }
        public string Facperbarrnombre { get; set; }
        public decimal Facper1 { get; set; }
        public decimal Facper2 { get; set; }
        public decimal Facper3 { get; set; }
        public decimal Facper4 { get; set; }
        public decimal Facper5 { get; set; }
        public decimal Facper6 { get; set; }
        public decimal Facper7 { get; set; }
        public decimal Facper8 { get; set; }
        public decimal Facper9 { get; set; }
        public decimal Facper10 { get; set; }
        public decimal Facper11 { get; set; }
        public decimal Facper12 { get; set; }
        public decimal Facper13 { get; set; }
        public decimal Facper14 { get; set; }
        public decimal Facper15 { get; set; }
        public decimal Facper16 { get; set; }
        public decimal Facper17 { get; set; }
        public decimal Facper18 { get; set; }
        public decimal Facper19 { get; set; }
        public decimal Facper20 { get; set; }
        public decimal Facper21 { get; set; }
        public decimal Facper22 { get; set; }
        public decimal Facper23 { get; set; }
        public decimal Facper24 { get; set; }
        public decimal Facper25 { get; set; }
        public decimal Facper26 { get; set; }
        public decimal Facper27 { get; set; }
        public decimal Facper28 { get; set; }
        public decimal Facper29 { get; set; }
        public decimal Facper30 { get; set; }
        public decimal Facper31 { get; set; }
        public decimal Facper32 { get; set; }
        public decimal Facper33 { get; set; }
        public decimal Facper34 { get; set; }
        public decimal Facper35 { get; set; }
        public decimal Facper36 { get; set; }
        public decimal Facper37 { get; set; }
        public decimal Facper38 { get; set; }
        public decimal Facper39 { get; set; }
        public decimal Facper40 { get; set; }
        public decimal Facper41 { get; set; }
        public decimal Facper42 { get; set; }
        public decimal Facper43 { get; set; }
        public decimal Facper44 { get; set; }
        public decimal Facper45 { get; set; }
        public decimal Facper46 { get; set; }
        public decimal Facper47 { get; set; }
        public decimal Facper48 { get; set; }
        public string Facperusername { get; set; }
        public DateTime Facperfecins { get; set; }

        public TrnFactorPerdidaBullkDTO(FactorPerdidaDTO entity)
        {
            this.Facpercodi = entity.FacPerCodi;
            this.Barrcodi = entity.BarrCodi;
            this.Facperbarrnombre = entity.FacPerBarrNombre;
            this.Pericodi = entity.PeriCodi;
            this.Facperbase = entity.FacPerBase;
            this.Facperversion = entity.FacPerVersion;
            this.Facperdia = entity.FacPerDia;
            this.Facper1 = entity.FacPer1;
            this.Facper2 = entity.FacPer2;
            this.Facper3 = entity.FacPer3;
            this.Facper4 = entity.FacPer4;
            this.Facper5 = entity.FacPer5;
            this.Facper6 = entity.FacPer6;
            this.Facper7 = entity.FacPer7;
            this.Facper8 = entity.FacPer8;
            this.Facper9 = entity.FacPer9;
            this.Facper10 = entity.FacPer10;
            this.Facper11 = entity.FacPer11;
            this.Facper12 = entity.FacPer12;
            this.Facper13 = entity.FacPer13;
            this.Facper14 = entity.FacPer14;
            this.Facper15 = entity.FacPer15;
            this.Facper16 = entity.FacPer16;
            this.Facper17 = entity.FacPer17;
            this.Facper18 = entity.FacPer18;
            this.Facper19 = entity.FacPer19;
            this.Facper20 = entity.FacPer20;
            this.Facper21 = entity.FacPer21;
            this.Facper22 = entity.FacPer22;
            this.Facper23 = entity.FacPer23;
            this.Facper24 = entity.FacPer24;
            this.Facper25 = entity.FacPer25;
            this.Facper26 = entity.FacPer26;
            this.Facper27 = entity.FacPer27;
            this.Facper28 = entity.FacPer28;
            this.Facper29 = entity.FacPer29;
            this.Facper30 = entity.FacPer30;
            this.Facper31 = entity.FacPer31;
            this.Facper32 = entity.FacPer32;
            this.Facper33 = entity.FacPer33;
            this.Facper34 = entity.FacPer34;
            this.Facper35 = entity.FacPer35;
            this.Facper36 = entity.FacPer36;
            this.Facper37 = entity.FacPer37;
            this.Facper38 = entity.FacPer38;
            this.Facper39 = entity.FacPer39;
            this.Facper40 = entity.FacPer40;
            this.Facper41 = entity.FacPer41;
            this.Facper42 = entity.FacPer42;
            this.Facper43 = entity.FacPer43;
            this.Facper44 = entity.FacPer44;
            this.Facper45 = entity.FacPer45;
            this.Facper46 = entity.FacPer46;
            this.Facper47 = entity.FacPer47;
            this.Facper48 = entity.FacPer48;
            this.Facperusername = entity.FacPerUserName;
            this.Facperfecins = entity.FacPerFecIns;

        }
    }
}
