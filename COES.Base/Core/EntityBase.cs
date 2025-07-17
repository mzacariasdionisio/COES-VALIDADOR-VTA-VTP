/*****************************************************************************************
* Fecha de Creación: 29-05-2014
* Creado por: COES SINAC
* Descripción: Clase padre de las entidades de la aplicación
*****************************************************************************************/

using System;
using System.IO;
using System.Xml.Serialization;
namespace COES.Base.Core
{
    /// <summary>
    /// </summary>
    [Serializable]
    public abstract class EntityBase
    {
        public int nroPagina { get; set; }
        public int nroFilas { get; set; }
        
        public String SerializarParaXml()
        {
            XmlSerializer xmlSerializer = null;
            StringWriter sw = null;
            try
            {
                sw = new StringWriter();
                xmlSerializer = new XmlSerializer(this.GetType());
                xmlSerializer.Serialize(sw, this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Close();
            }
            return sw.ToString();
        }

        public String RetornaNombreTablaXMLRoot()
        {
            String name = "";
            XmlAttributes tempAttrs = null;
            try
            {
                tempAttrs = new XmlAttributes(this.GetType());
                name = tempAttrs.XmlRoot.ElementName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return name.ToString();
        }
    }
}