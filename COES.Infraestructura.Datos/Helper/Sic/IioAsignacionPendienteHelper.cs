// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 26/10/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{

    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_ASIGNACION_PENDIENTE.
    /// </summary>
    public class IioAsignacionPendienteHelper : HelperBase
    {

        #region CAMPOS: Variables de la clase.

        public string Mapencodi = "MAPENCODI";
        public string Mapenentidad = "MAPENENTIDAD";
        public string Mapencodigo = "MAPENCODIGO";
        public string Mapendescripcion = "MAPENDESCRIPCION";
        public string Mapenestado = "MAPENESTADO";
        public string Mapenindicacionestado = "MAPENINDICACIONESTADO";
        public string Mapenestregistro = "MAPENESTREGISTRO";
        public string Mapenusucreacion = "MAPENUSUCREACION";
        public string Mapenfeccreacion = "MAPENFECCREACION";
        public string Mapenusumodificacion = "MAPENUSUMODIFICACION";
        public string Mapenfecmodificacion = "MAPENFECMODIFICACION";       

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public IioAsignacionPendienteHelper()
            : base(Consultas.IioAsignacionPendienteSql)
        {
        }

        #endregion

        #region EVENTOS: Definicion de eventos de la clase.

        #endregion

        #region PROPIEDADES: Propiedades de la clase.

        #endregion

        #region METODOS: Metodos de la clase.

        public string SqlListByCreationDate
        {
            get { return base.GetSqlXml("SqlListByCreationDate"); }
        }

        public IioAsignacionPendienteDTO Create(IDataReader dr)
        {

            IioAsignacionPendienteDTO entity = new IioAsignacionPendienteDTO();

            int iMapencodi = dr.GetOrdinal(this.Mapencodi);
            if (!dr.IsDBNull(iMapencodi)) entity.Mapencodi = Convert.ToInt32(dr.GetValue(iMapencodi));

            int iMapenentidad = dr.GetOrdinal(this.Mapenentidad);
            if (!dr.IsDBNull(iMapenentidad)) entity.Mapenentidad = dr.GetString(iMapenentidad);

            int iMapencodigo = dr.GetOrdinal(this.Mapencodigo);
            if (!dr.IsDBNull(iMapencodigo)) entity.Mapencodigo = dr.GetString(iMapencodigo);

            int iMapendescripcion = dr.GetOrdinal(this.Mapendescripcion);
            if (!dr.IsDBNull(iMapendescripcion)) entity.Mapendescripcion = dr.GetString(iMapendescripcion);

            int iMapenestado = dr.GetOrdinal(this.Mapenestado);
            if (!dr.IsDBNull(iMapenestado)) entity.Mapenestado = Convert.ToInt32(dr.GetValue(iMapenestado));

            int iMapenindicacionestado = dr.GetOrdinal(this.Mapenindicacionestado);
            if (!dr.IsDBNull(iMapenindicacionestado)) entity.Mapenindicacionestado = dr.GetString(iMapenindicacionestado);

            int iMapenestregistro = dr.GetOrdinal(this.Mapenestregistro);
            if (!dr.IsDBNull(iMapenestregistro)) entity.Mapenestregistro = dr.GetString(iMapenestregistro);

            int iMapenusucreacion = dr.GetOrdinal(this.Mapenusucreacion);
            if (!dr.IsDBNull(iMapenusucreacion)) entity.Mapenusucreacion = dr.GetString(iMapenusucreacion);

            int iMapenfeccreacion = dr.GetOrdinal(this.Mapenfeccreacion);
            if (!dr.IsDBNull(iMapenfeccreacion)) entity.Mapenfeccreacion = dr.GetDateTime(iMapenfeccreacion);

            int iMapenusumodificacion = dr.GetOrdinal(this.Mapenusumodificacion);
            if (!dr.IsDBNull(iMapenusumodificacion)) entity.Mapenusumodificacion = dr.GetString(iMapenusumodificacion);

            int iMapenfecmodificacion = dr.GetOrdinal(this.Mapenfecmodificacion);
            if (!dr.IsDBNull(iMapenfecmodificacion)) entity.Mapenfecmodificacion = dr.GetDateTime(iMapenfecmodificacion);
            
            return entity;
        }

        #endregion        

    }

}
