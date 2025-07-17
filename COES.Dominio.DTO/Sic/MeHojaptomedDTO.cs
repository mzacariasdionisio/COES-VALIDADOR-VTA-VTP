using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_HOJAPTOMED
    /// </summary>
    [DataContract]
    [Serializable]
    public class MeHojaptomedDTO : EntityBase, ICloneable
    {
        [DataMember]
        public int? Hojaptoactivo { get; set; }
        [DataMember]
        public DateTime? Lastdate { get; set; }
        [DataMember]
        public string Lastuser { get; set; }
        [DataMember]
        public decimal? Hojaptoliminf { get; set; }
        [DataMember]
        public int Hojacodi { get; set; }
        [DataMember]
        public int Formatcodi { get; set; }
        [DataMember]
        public int Tipoinfocodi { get; set; }
        [DataMember]
        public decimal? Hojaptolimsup { get; set; }
        [DataMember]
        public int Ptomedicodi { get; set; }
        [DataMember]
        public int Hojaptoorden { get; set; }
        [DataMember]
        public int Hojaptosigno { get; set; }
        [DataMember]
        public string Tipoinfoabrev { get; set; }
        [DataMember]
        public string Equinomb { get; set; }
        [DataMember]
        public string Emprabrev { get; set; }
        [DataMember]
        public string Equiabrev { get; set; }
        [DataMember]
        public int Tptomedicodi { get; set; }
        [DataMember]
        public string Tipoptomedinomb { get; set; }
        [DataMember]
        public int Famcodi { get; set; }
        [DataMember]
        public string Ptomedibarranomb { get; set; }
        [DataMember]
        public string Ptomedidesc { get; set; }
        [DataMember]
        public int Hojanumero { get; set; }
        [DataMember]
        public string Equipopadre { get; set; }
        [DataMember]
        public int Equipadre { get; set; }
        [DataMember]
        public int Equicodi { get; set; }
        [DataMember]
        public string Valor1 { get; set; }
        [DataMember]
        public decimal? Valor2 { get; set; }
        [DataMember]
        public decimal? Valor3 { get; set; }
        [DataMember]
        public int Areacodi { get; set; }
        [DataMember]
        public string Areanomb { get; set; }
        [DataMember]
        public string AreaOperativa { get; set; }
        [DataMember]
        public string Suministrador { get; set; }
        [DataMember]
        public string CodigoOsinergmin { get; set; }
        [DataMember]
        public string Unidad { get; set; }
        [DataMember]
        public string PtoMediEleNomb { get; set; }
        [DataMember]
        public string Famabrev { get; set; }
        [DataMember]
        public string Gruponomb { get; set; }
        [DataMember]
        public string Hptoobservacion { get; set; }

        //- Campos agregados PMPO
        [DataMember]
        public int Hojaptomedcodi { get; set; }
        [DataMember]
        public string Tmpnomb { get; set; }
        [DataMember]
        public string Tptomedinomb { get; set; }
        [DataMember]
        public DateTime? ObraFechaPlanificada { get; set; }
        [DataMember]
        public string Medidornomb { get; set; }
        [DataMember]
        public string Medidorserie { get; set; }
        [DataMember]
        public string Medidorclaseprecision { get; set; }
        [DataMember]
        public string Valor4 { get; set; }
        [DataMember]
        public int Emprcodi { get; set; }
        [DataMember]
        public string Emprnomb { get; set; }
        [DataMember]
        public int Tgenercodi { get; set; }
        [DataMember]
        public string Tgenernomb { get; set; }

        #region Modificación PR5-DemandaDiaria 29-11-2017
        [DataMember]
        public string Valor5 { get; set; }
        #endregion

        #region MigracionSGOCOES-GrupoB
        [DataMember]
        public int Anio { get; set; }
        [DataMember]
        public int Mes { get; set; }
        [DataMember]
        public int Dia { get; set; }
        #endregion

        [DataMember]
        public int Hptominfila { get; set; }
        [DataMember]
        public int Hptodiafinplazo { get; set; }
        [DataMember]
        public int Hptominfinplazo { get; set; }
        [DataMember]
        public MePlazoptoDTO ConfigPto { get; set; }

        #region Mejoras IEOD
        [DataMember]
        public decimal? Equitension { get; set; }
        #endregion

        [DataMember]
        public string Clientenomb { get; set; }

        [DataMember]
        public string PuntoConexion { get; set; }
        [DataMember]
        public string Barranomb { get; set; }

        //cargar puntos
        public List<MeMedicion48DTO> ListaPuntosMedi = new List<MeMedicion48DTO>();
        public string Origlectnombre { get; set; }
        public int Origlectcodi { get; set; }

        public int Grupocodi { get; set; }
        public string Ptomediestado { get; set; }

        #region Mejoras RDO
        public string Cuenca { get; set; }
        #endregion

        public string Hptoindcheck { get; set; }
        public bool TieneCheckExtranet { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string Formatnombre { get; set; }
        public string Lectnomb { get; set; }

        #region Assetec - DemandaPO
        public int Areapadre { get; set; }
        #endregion
    }
}
