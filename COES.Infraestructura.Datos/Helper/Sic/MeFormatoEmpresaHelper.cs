using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_FORMATO_EMPRESA
    /// </summary>
    public class MeFormatoEmpresaHelper : HelperBase
    {
        public MeFormatoEmpresaHelper(): base(Consultas.MeFormatoEmpresaSql)
        {
        }

        public MeFormatoEmpresaDTO Create(IDataReader dr)
        {
            MeFormatoEmpresaDTO entity = new MeFormatoEmpresaDTO();

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iForemdiatomamedicion = dr.GetOrdinal(this.Foremdiatomamedicion);
            if (!dr.IsDBNull(iForemdiatomamedicion)) entity.Foremdiatomamedicion = Convert.ToInt32(dr.GetValue(iForemdiatomamedicion));

            int iForemusucreacion = dr.GetOrdinal(this.Foremusucreacion);
            if (!dr.IsDBNull(iForemusucreacion)) entity.Foremusucreacion = dr.GetString(iForemusucreacion);

            int iForemfeccreacion = dr.GetOrdinal(this.Foremfeccreacion);
            if (!dr.IsDBNull(iForemfeccreacion)) entity.Foremfeccreacion = dr.GetDateTime(iForemfeccreacion);

            int iForemusumodificacion = dr.GetOrdinal(this.Foremusumodificacion);
            if (!dr.IsDBNull(iForemusumodificacion)) entity.Foremusumodificacion = dr.GetString(iForemusumodificacion);

            int iForemfecmodificacion = dr.GetOrdinal(this.Foremfecmodificacion);
            if (!dr.IsDBNull(iForemfecmodificacion)) entity.Foremfecmodificacion = dr.GetDateTime(iForemfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Formatcodi = "FORMATCODI";
        public string Emprcodi = "EMPRCODI";
        public string Foremdiatomamedicion = "FOREMDIATOMAMEDICION";
        public string Foremusucreacion = "FOREMUSUCREACION";
        public string Foremfeccreacion = "FOREMFECCREACION";
        public string Foremusumodificacion = "FOREMUSUMODIFICACION";
        public string Foremfecmodificacion = "FOREMFECMODIFICACION";

        #endregion

        //- remision-pr16.JDEL - Inicio 19/05/2016: Cambio para atender el requerimiento.
        public string SqlObtenerListaPeriodoEnvio
        {
            get { return base.GetSqlXml("ObtenerListaPeriodoEnvio"); }
        }
        //- JDEL Fin
        

    }
}
