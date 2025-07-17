using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Drawing;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPR_AREA
    /// </summary>
    public class EprAreaHelper : HelperBase
    {
        public EprAreaHelper(): base(Consultas.EprAreaSql)
        {
        }

        public void ObtenerMetaDatos(IDataReader dr, ref Dictionary<int, MetadataDTO> metadatos)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                metadatos.Add(i, new MetadataDTO
                {
                    FieldName = dr.GetName(i),
                    TipoDato = dr.GetFieldType(i)
                });
            }
        }
        public EprAreaDTO Create(IDataReader dr)
        {
            EprAreaDTO entity = new EprAreaDTO();

            int iEpareacodi = dr.GetOrdinal(this.Epareacodi);
            if (!dr.IsDBNull(iEpareacodi)) entity.Epareacodi = Convert.ToInt32(dr.GetValue(iEpareacodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iAreacodizona = dr.GetOrdinal(this.Areacodizona);
            if (!dr.IsDBNull(iAreacodizona)) entity.Areacodizona = Convert.ToInt32(dr.GetValue(iAreacodizona));

            int iEpareanomb = dr.GetOrdinal(this.Epareanomb);
            if (!dr.IsDBNull(iEpareanomb)) entity.Epareanomb = dr.GetValue(iEpareanomb).ToString();

            int iEpareaestregistro = dr.GetOrdinal(this.Epareaestregistro);
            if (!dr.IsDBNull(iEpareaestregistro)) entity.Epareaestregistro = dr.GetValue(iEpareaestregistro).ToString();

            int iEpareausucreacion = dr.GetOrdinal(this.Epareausucreacion);
            if (!dr.IsDBNull(iEpareausucreacion)) entity.Epareausucreacion = dr.GetValue(iEpareausucreacion).ToString();

            int iEpareafeccreacion = dr.GetOrdinal(this.Epareafeccreacion);
            if (!dr.IsDBNull(iEpareafeccreacion)) entity.Epareafeccreacion = Convert.ToString(dr.GetValue(iEpareafeccreacion));

            int iEpareausumodificacion = dr.GetOrdinal(this.Epareausumodificacion);
            if (!dr.IsDBNull(iEpareausumodificacion)) entity.Epareausumodificacion = dr.GetValue(iEpareausumodificacion).ToString();

            int iEpareafecmodificacion = dr.GetOrdinal(this.Epareafecmodificacion);
            if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Epareafecmodificacion = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

            return entity;
        }

        //GESPROTEC - 20241029
        #region GESPROTECT
        bool validaColumna(IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Mapeo de Campos
        public string Epareacodi = "EPAREACODI";
        public string Areacodi = "AREACODI";
        public string Areacodizona = "AREACODIZONA";
        public string Epareanomb = "EPAREANOMB";
        public string Epareaestregistro = "EPAREAESTREGISTRO";
        public string Epareausucreacion = "EPAREAUSUCREACION";
        public string Epareafeccreacion = "EPAREAFECCREACION";

        public string Epareausumodificacion = "EPAREAUSUMODIFICACION";
        public string Epareafecmodificacion = "EPAREAFECMODIFICACION";
        #endregion

        #region ProyectoActualizacion
        public string Areanomb = "AREANOMB";
        public string Zona = "ZONA";
        #endregion

        #region Validación de Eliminacion 
        public string NroEquipos = "NRO_EQUIPOS";
        #endregion
        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        #region Lista Area x Celda
        public string AreaCodi = "areacodi";
        public string Tareacodi = "tareacodi";
        public string Areaabrev = "areaabrev";
        public string AreaNomb = "areanomb";
        public string Areapadre = "areapadre";
        public string Areaestado = "areaestado";
        public string Usuariocreacion = "usuariocreacion";
        public string Fechacreacion = "fechacreacion";
        public string Usuarioupdate = "usuarioupdate";
        public string Fechaupdate = "fechaupdate";
        public string Anivelcodi = "anivelcodi";
        public string Tareacodi1 = "tareacodi_1";
        public string Tareaabrev = "tareaabrev";
        public string Tareanomb = "tareanomb";
        public string Anivelcodi1 = "anivelcodi_1";
        public string Idcelda1 = "id_celda_1";
        public string Idcelda2 = "id_celda_2";
        #endregion

        public string ListSubEstacion
        {
            get { return base.GetSqlXml("ListSubEstacion"); }
        }

        public string SqlCantidadUbicacionSGOCOESEliminar
        {
            get { return base.GetSqlXml("SqlCantidadUbicacionSGOCOESEliminar"); }
        }

        public string SqlListAreaxCelda
        {
            get { return base.GetSqlXml("SqlListAreaxCelda"); }
        }

    }
}
