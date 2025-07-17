using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea el tipo de evento
    /// </summary>
    //[DataContract]
    //[Serializable]
    public class TipoEventoDTO :EntityBase
    {
        [DataMember]
        public short TIPOEVENCODI { get; set; }
        [DataMember]
        public string TIPOEVENDESC { get; set; }
        [DataMember]
        public Nullable<short> SUBCAUSACODI { get; set; }
        [DataMember]
        public string TIPOEVENABREV { get; set; }
    }
}

