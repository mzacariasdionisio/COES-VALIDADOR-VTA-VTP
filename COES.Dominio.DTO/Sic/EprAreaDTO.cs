using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPR_AREA
    /// </summary>
    [DataContract]
    [Serializable]
    public class EprAreaDTO : EntityBase
    {
        [DataMember]
        public int Epareacodi { get; set; }
        [DataMember]
        public int? Areacodi { get; set; }
        [DataMember]
        public int? Areacodizona { get; set; }
        [DataMember]
        public string Epareanomb { get; set; }
        [DataMember]
        public string Epareaestregistro { get; set; }
        [DataMember]
        public string Epareausucreacion { get; set; }
        [DataMember]
        public string Epareausumodificacion { get; set; }
        [DataMember]
        public string Epareafeccreacion { get; set; }

        [DataMember]
        public string Epareafecmodificacion { get; set; }
        [DataMember]
        public string Areanomb { get; set; }
        [DataMember]
        public string Zona { get; set; }

        #region Validacion de Eliminacion
        [DataMember]
        public int? NroEquipos { get; set; }
        #endregion

        public int AreaCodi { get; set; }
        public string Tareacodi { get; set; }
        public string Areaabrev { get; set; }
        public string AreaNomb { get; set; }
        public string Areapadre { get; set; }
        public string Areaestado { get; set; }
        public string Usuariocreacion { get; set; }
        public string Fechacreacion { get; set; }
        public string Usuarioupdate { get; set; }
        public string Fechaupdate { get; set; }
        public string Anivelcodi { get; set; }
        public string Tareacodi1 { get; set; }
        public string Tareaabrev { get; set; }
        public string Tareanomb { get; set; }
        public string Anivelcodi1 { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
