using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.WebAPI.Equipamiento.Models
{
    public class DatoTipoArea
    {
        /// <summary>
        /// Codigo COES del tipo de ubicación (area)
        /// </summary>
        public int TAREACODI { get; set; }
        /// <summary>
        /// Nombre de la Tipo de Ubicación (area)
        /// </summary>
        public string TAREANOMB { get; set; }
    }
    //< xs:complexType name = "ArrayOfDatoTipoArea" >
    //< xs:sequence>
    //<xs:element minOccurs = "0" maxOccurs="unbounded" name="DatoTipoArea" nillable="true" type="tns:DatoTipoArea"/>
    //</xs:sequence>
    //</xs:complexType>
    //<xs:element name = "ArrayOfDatoTipoArea" nillable="true" type="tns:ArrayOfDatoTipoArea"/>
    //<xs:complexType name = "DatoTipoArea" >
    //< xs:sequence>
    //<xs:element minOccurs = "0" name="TAREACODI" type="xs:int"/>
    //<xs:element minOccurs = "0" name="TAREANOMB" nillable="true" type="xs:string"/>
    //</xs:sequence>
    //</xs:complexType>
    //<xs:element name = "DatoTipoArea" nillable="true" type="tns:DatoTipoArea"/>
}