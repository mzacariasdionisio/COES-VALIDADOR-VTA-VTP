using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MAP_EMPSINREP
    /// </summary>
    public class MapEmpsinrepHelper : HelperBase
    {
        public MapEmpsinrepHelper(): base(Consultas.MapEmpsinrepSql)
        {
        }

        public MapEmpsinrepDTO Create(IDataReader dr)
        {
            MapEmpsinrepDTO entity = new MapEmpsinrepDTO();

            int iEmpsrcodi = dr.GetOrdinal(this.Empsrcodi);
            if (!dr.IsDBNull(iEmpsrcodi)) entity.Empsrcodi = Convert.ToInt32(dr.GetValue(iEmpsrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iMediccodi = dr.GetOrdinal(this.Mediccodi);
            if (!dr.IsDBNull(iMediccodi)) entity.Mediccodi = Convert.ToInt32(dr.GetValue(iMediccodi));

            int iEmpsrperiodo = dr.GetOrdinal(this.Empsrperiodo);
            if (!dr.IsDBNull(iEmpsrperiodo)) entity.Empsrperiodo = dr.GetDateTime(iEmpsrperiodo);

            int iEmpsrfecha = dr.GetOrdinal(this.Empsrfecha);
            if (!dr.IsDBNull(iEmpsrfecha)) entity.Empsrfecha = dr.GetDateTime(iEmpsrfecha);

            int iEmpsrusucreacion = dr.GetOrdinal(this.Empsrusucreacion);
            if (!dr.IsDBNull(iEmpsrusucreacion)) entity.Empsrusucreacion = dr.GetString(iEmpsrusucreacion);

            int iEmpsrfeccreacion = dr.GetOrdinal(this.Empsrfeccreacion);
            if (!dr.IsDBNull(iEmpsrfeccreacion)) entity.Empsrfeccreacion = dr.GetDateTime(iEmpsrfeccreacion);

            int iEmpsrusumodificacion = dr.GetOrdinal(this.Empsrusumodificacion);
            if (!dr.IsDBNull(iEmpsrusumodificacion)) entity.Empsrusumodificacion = dr.GetString(iEmpsrusumodificacion);

            int iEmpsrfecmodificacion = dr.GetOrdinal(this.Empsrfecmodificacion);
            if (!dr.IsDBNull(iEmpsrfecmodificacion)) entity.Empsrfecmodificacion = dr.GetDateTime(iEmpsrfecmodificacion);


            return entity;
        }


        #region Mapeo de Campos

        public string Empsrcodi = "EMPSRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Mediccodi = "MEDICCODI";
        public string Empsrperiodo = "EMPSRPERIODO";
        public string Empsrfecha = "EMPSRFECHA";
        public string Empsrusucreacion = "EMPSRUSUCREACION";
        public string Empsrfeccreacion = "EMPSRFECCREACION";
        public string Empsrusumodificacion = "EMPSRUSUMODIFICACION";
        public string Empsrfecmodificacion = "EMPSRFECMODIFICACION";

        #endregion
    }
}
