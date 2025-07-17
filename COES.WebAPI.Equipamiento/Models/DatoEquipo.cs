using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.WebAPI.Equipamiento.Models
{
    public class DatoEquipo
    {       
        /// <summary>
        /// Codigo COES del equipo
        /// </summary>
        public int EQUICODI { get; set; }
        /// <summary>
        /// Abreviatura del Equipo
        /// </summary>
        public string EQUIABREV { get; set; }
        /// <summary>
        /// Nombre del Equipo
        /// </summary>
        public string EQUINOMB { get; set; }
        /// <summary>
        /// Codigo COES de Ubicacion (Area)
        /// </summary>
        public int AREACODI { get; set; }
        /// <summary>
        /// Codigo COES de la empresa
        /// </summary>
        public int EMPRCODI { get; set; }
        /// <summary>
        /// Codigo COES del Tipo de Equipo
        /// </summary>
        public int FAMCODI { get; set; }
        /// <summary>
        /// Estado del Equipo 'A': Activo, 'F': Fuera de COES 'B': Equipo de Baja y "P":En Proyecto
        /// </summary>       
        public string EQUIESTADO { get; set; }
    }
    //<xs:complexType name = "ArrayOfDatoEquipo" >
    //< xs:sequence>
    //<xs:element minOccurs = "0" maxOccurs="unbounded" name="DatoEquipo" nillable="true" type="tns:DatoEquipo"/>
    //</xs:sequence>
    //</xs:complexType>
    //<xs:element name = "ArrayOfDatoEquipo" nillable="true" type="tns:ArrayOfDatoEquipo"/>
    //<xs:complexType name = "DatoEquipo" >
    //< xs:sequence>
    //<xs:element minOccurs = "0" name="AREACODI" type="xs:int"/>
    //<xs:element minOccurs = "0" name="EMPRCODI" type="xs:int"/>
    //<xs:element minOccurs = "0" name="EQUIABREV" nillable="true" type="xs:string"/>
    //<xs:element minOccurs = "0" name="EQUICODI" type="xs:int"/>
    //<xs:element minOccurs = "0" name="EQUINOMB" nillable="true" type="xs:string"/>
    //<xs:element minOccurs = "0" name="ESTADO" nillable="true" type="xs:string"/>
    //</xs:sequence>
    //</xs:complexType>
    //<xs:element name = "DatoEquipo" nillable="true" type="tns:DatoEquipo"/>
}