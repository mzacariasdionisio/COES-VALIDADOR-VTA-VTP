using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_MANTTO
    /// </summary>     
    [Serializable]
    [DataContract]
    public class EveManttoDTO : EntityBase
    {
        [DataMember]
        public int Manttocodi { get; set; }
        [DataMember]
        public int? Equicodi { get; set; }
        [DataMember]
        public int? Evenclasecodi { get; set; }
        [DataMember]
        public int? Tipoevencodi { get; set; }
        [DataMember]
        public int? Compcode { get; set; }
        [DataMember]
        public DateTime? Evenini { get; set; }
        [DataMember]
        public DateTime? Evenpreini { get; set; }
        [DataMember]
        public DateTime? Evenfin { get; set; }
        [DataMember]
        public int? Subcausacodi { get; set; }
        [DataMember]
        public DateTime? Evenprefin { get; set; }
        [DataMember]
        public decimal? Evenmwindisp { get; set; }
        [DataMember]
        public int? Evenpadre { get; set; }
        [DataMember]
        public string Evenindispo { get; set; }
        [DataMember]
        public string Eveninterrup { get; set; }
        [DataMember]
        public string Eventipoprog { get; set; }
        [DataMember]
        public string Evendescrip { get; set; }
        [DataMember]
        public string Evenobsrv { get; set; }
        [DataMember]
        public string Evenestado { get; set; }
        [DataMember]
        public string Lastuser { get; set; }
        [DataMember]
        public DateTime? Lastdate { get; set; }
        [DataMember]
        public int? Evenrelevante { get; set; }
        [DataMember]
        public int? Deleted { get; set; }
        public string Eventipoindisp { get; set; }
        public decimal? Evenpr { get; set; }
        public string Evenasocproc { get; set; }
        [DataMember]
        public int? Mancodi { get; set; }
        [DataMember]
        public int Emprcodi { get; set; }
        [DataMember]
        public string Emprnomb { get; set; }
        [DataMember]
        public string Emprabrev { get; set; }
        [DataMember]
        public string Evenclasedesc { get; set; }
        [DataMember]
        public string Areanomb { get; set; }
        [DataMember]
        public string Areadesc { get; set; }
        [DataMember]
        public int Famcodi { get; set; }
        [DataMember]
        public string Famnomb { get; set; }
        [DataMember]
        public string Equiabrev { get; set; }
        [DataMember]
        public string Causaevenabrev { get; set; }
        [DataMember]
        public Nullable<decimal> Equitension { get; set; }
        [DataMember]
        public string Tipoevenabrev { get; set; }
        [DataMember]
        public string Tipoevendesc { get; set; }
        [DataMember]
        public string Tipoemprdesc { get; set; }
        [DataMember]
        public string Osigrupocodi { get; set; }
        [DataMember]
        public int? Equipadre { get; set; }
        [DataMember]
        public int Tipoemprcodi { get; set; }
        [DataMember]

        public string Mantipcodi { get; set; }
        [DataMember]

        #region PR5
        public string Famabrev { get; set; }
        public string Evenclaseabrev { get; set; }
        public string Equinomb { get; set; }
        #endregion

        #region SIOSEIN
        public string Subcausadesc { get; set; }
        public string Tipoindisponibilidad { get; set; }
        public int Areacodi { get; set; }
        public string Osinergcodi { get; set; }
        public string OsiCodigoTipoEmpresa { get; set; }
        #endregion

        #region INTERVENCIONES
        public string Equimantrelev { get; set; }
        public string Mantrelevlastuser { get; set; }
        public DateTime? Mantrelevlastdate { get; set; }
        public int InterCodi { get; set; }
        #endregion 

        #region INDISPONIBILIDADES
        public int Ieventcodi { get; set; }
        public int Evencodi { get; set; }
        public int Iiccocodi { get; set; }
        public int Iccodi { get; set; }
        public int Indmancodi { get; set; }
        public int Grupocodi { get; set; }
        public int EventoGenerado { get; set; }
        public int FuenteDatos { get; set; }
        public string FuenteDatosDesc { get; set; }
        public string EvenindispoDesc { get; set; }
        public string EveninterrupDesc { get; set; }
        public int Indmanflagcalculo { get; set; }

        public string Indmanusarencalculo { get; set; }
        public string IndmanusarencalculoDesc { get; set; }
        public string Indmantipoaccion { get; set; }
        public string Indmanomitir7d { get; set; }
        public string Indmanomitirexcesopr { get; set; }

        public string LastdateDesc { get; set; }
        public int Grupocodidet { get; set; }
        public string EvenestadoDesc { get; set; }
        public string ClaseFila { get; set; }
        public DateTime? Eveniniprog { get; set; }
        public DateTime? Evenfinprog { get; set; }
        public string Grupotipocogen { get; set; }
        public List<int> ListaEquicodi { get; set; } = new List<int>();
        public string ListaEquicodiStr { get; set; } = "";
        public bool TieneAmbosTipoMantto { get; set; }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public int Tareacodi { get; set; }
        public string Tipoevento { get; set; }
        public string Evenindispoparcial { get; set; }
        public string TEOsinerg { get; set; }
        #endregion

        #region NET 20190228 - Cálculo de disponibilidad de las unidades de generación Hidráulico y Térmico
        public string Emprdomiciliolegal { get; set; }
        public int? Grupocodisddp { get; set; }
        public decimal? Pmcindporcentaje { get; set; }
        public string Tipounidad { get; set; }
        public string Gruponomb { get; set; }

        public int Anio { get; set; }
        public int Mes { get; set; }
        public int NroSemana { get; set; }

        #endregion

        #region SIOSEIN2
        public decimal? Valor { get; set; }
        public int Fenergcodi { get; set; }
        public int TipoCombustible { get; set; }
        public decimal? Potencia { get; set; }
        public decimal? Potencia2 { get; set; }
        public decimal NumeroHoras { get; set; }
        public string Central { get; set; }
        public int Grupocodimodo { get; set; }
        public string Grupointegrante { get; set; }
        public decimal ValorPEequipo { get; set; }
        public decimal ValorPEVertido { get; set; }
        public decimal ValorVertimiento { get; set; }
        public decimal ValorRendimientoGenH { get; set; }
        #endregion

        #region Numerales Datos Base
        public string Dia { get; set; }
        public string Osicodi { get; set; }
        #endregion
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }


    public class ReporteManttoDTO : EntityBase
    {
        public int Equicodi { get; set; }
        public string Equiabrev { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Emprabrev { get; set; }
        public int Famcodi { get; set; }
        public string Famnomb { get; set; }
        public int Tipoevencodi { get; set; }
        public string Tipoevendesc { get; set; }
        public string Tipoemprdesc { get; set; }
        public int Subtotal { get; set; }
        public int? Evenclasecodi { get; set; }
        public string Evenclasedesc { get; set; }
        public decimal Porcentajemantto { get; set; }
        public string Osigrupocodi { get; set; }
    }

    /// <summary>
    /// Permite obtener filtros del mantto
    /// </summary>
    public class EveManttoTipoDTO
    {
        public int Mantipcodi { get; set; }
        public string Mantipdesc { get; set; }
        public string Mantipcolor { get; set; }
    }
}

