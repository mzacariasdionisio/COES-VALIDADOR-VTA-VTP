using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.WebAPI.Equipamiento.Models
{
    public class DatoArea
    {
        /// <summary>
        /// Codigo COES de la ubicación (area)
        /// </summary>
        public int AREACODI { get; set; }
        /// <summary>
        /// Nombre de la Ubicación (area)
        /// </summary>
        public string AREANOMB { get; set; }
        /// <summary>
        /// Codigo COES de tipo de Ubicación (area)
        /// </summary>
        public int TAREACODI { get; set; }

    }
    //xs:complexType name = "ArrayOfDatoArea" >
    //< xs:sequence>
    //<xs:element minOccurs = "0" maxOccurs="unbounded" name="DatoArea" nillable="true" type="tns:DatoArea"/>
    //</xs:sequence>
    //</xs:complexType>
    //<xs:element name = "ArrayOfDatoArea" nillable="true" type="tns:ArrayOfDatoArea"/>
    //<xs:complexType name = "DatoArea" >
    //< xs:sequence>
    //<xs:element minOccurs = "0" name="AREACODI" type="xs:int"/>
    //<xs:element minOccurs = "0" name="AREANOMB" nillable="true" type="xs:string"/>
    //<xs:element minOccurs = "0" name="TAREACODI" type="xs:int"/>
    //</xs:sequence>
    //</xs:complexType>
    //<xs:element name = "DatoArea" nillable="true" type="tns:DatoArea"/>
}