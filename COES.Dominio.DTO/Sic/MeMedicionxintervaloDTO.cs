using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_MEDICIONXINTERVALO
    /// </summary>
    [Serializable]
    public class MeMedicionxintervaloDTO : EntityBase, ICloneable
    {
        public DateTime Medintfechaini { get; set; }
        public DateTime Medintfechafin { get; set; }
        public int Ptomedicodi { get; set; }
        public int Lectcodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public decimal? Medinth1 { get; set; }
        public decimal? Medinth_1 { get; set; }
        public string Medintusumodificacion { get; set; }
        public DateTime? Medintfecmodificacion { get; set; }
        public string Medintdescrip { get; set; }
        public int Medestcodi { get; set; }

        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public int Grupopadre { get; set; }
        public string Gruponombpadre { get; set; }
        public string Tipoptomedinomb { get; set; }
        public string Tipoinfoabrev { get; set; }
        public string Ptomedibarranomb { get; set; }
        public string Emprnomb { get; set; }
        public int Famcodi { get; set; }
        public string Famabrev { get; set; }
        public string Emprcoes { get; set; }
        public string Ptomedielenomb { get; set; }
        public int Equipadre { get; set; }
        public string Equipopadre { get; set; }
        public string Fenergnomb { get; set; }
        public string Fenercolor { get; set; }
        public int Fenergcodi { get; set; }

        public decimal? H1Recep { get; set; }
        public decimal? H1Fin { get; set; }

        //- Agregado para módulo PMPO

        public int Enviocodi { get; set; }
        public int Medintcodi { get; set; }
        public int Medintsemana { get; set; }
        public DateTime? Medintanio { get; set; }
        public decimal Medintblqhoras { get; set; }
        public int Medintblqnumero { get; set; }
        public int Hojacodi { get; set; }
        public string Hojanomb { get; set; }
        public DateTime? ObraFechaPlanificada { get; set; }
        public int Serie { get; set; }
        public int Orden { get; set; }

        #region SIOSEIN
        public string Osicodi { get; set; }
        public string Osinergcodi { get; set; }
        public int Tgenercodi { get; set; }
        public string Tgenernomb { get; set; }
        public int Emprcodi { get; set; }
        public decimal EnergiaProd { get; set; }
        public decimal? H1Hidro { get; set; }
        public decimal? H1Termo { get; set; }
        public decimal? H1Solar { get; set; }
        public decimal? H1Eolica { get; set; }
        public decimal EnergActivaInf { get; set; }
        public decimal EnergBD { get; set; }
        public decimal Participacion { get; set; }
        public decimal ParticipacionConComb { get; set; }
        public decimal Rendimiento { get; set; }
        public string Ptomedidesc { get; set; }
        public decimal? ProyActual { get; set; }
        public decimal? ProyAnterior { get; set; }
        public decimal? Variacion { get; set; }
        public decimal? VolInicial { get; set; }
        public decimal? VolFinal { get; set; }
        public int Codcentral { get; set; }
        public string Central { get; set; }
        public string Tipopagente { get; set; }
        public string Tptomedinomb { get; set; }
        public int Tptomedicodi { get; set; }
        public DateTime? Periodo { get; set; }
        public int CodigoResultados { get; set; }
        #endregion

        #region SIOSEIN2

        public string Tipogenerrer { get; set; }
        public string Grupotipocogen { get; set; }
        public string Grupointegrante { get; set; }
        public string Codcomb { get; set; }

        #endregion

        #region PMPO
        public string Semana { get; set; }
        public string Pmbloqnombre { get; set; }
        public int Catecodi { get; set; }
        public int Fuente { get; set; } // de donde obtiene la información
        #endregion

        #region Numerales Datos Base
        public decimal? Valor { get; set; }

        public string Sconnomb { get; set; }
        #endregion

        #region FIT - VALORIZACION DIARIA
        public int? Clientecodi { get; set; }
        public int? Barrcodi { get; set; }
        public string ClientNomb { get; set; }
        public string Barrnombre { get; set; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
