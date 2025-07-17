using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MENUREPORTE
    /// </summary>
    public class SiMenureporteHelper : HelperBase
    {
        public SiMenureporteHelper()
            : base(Consultas.SiMenureporteSql)
        {
        }

        public SiMenureporteDTO Create(IDataReader dr)
        {
            SiMenureporteDTO entity = new SiMenureporteDTO();

            int iRepcodi = dr.GetOrdinal(this.Repcodi);
            if (!dr.IsDBNull(iRepcodi)) entity.Repcodi = Convert.ToInt32(dr.GetValue(iRepcodi));

            int iRepdescripcion = dr.GetOrdinal(this.Repdescripcion);
            if (!dr.IsDBNull(iRepdescripcion)) entity.Repdescripcion = dr.GetString(iRepdescripcion);

            int iRepabrev = dr.GetOrdinal(this.Repabrev);
            if (!dr.IsDBNull(iRepabrev)) entity.Repabrev = dr.GetString(iRepabrev);

            int iReptiprepcodi = dr.GetOrdinal(this.Reptiprepcodi);
            if (!dr.IsDBNull(iReptiprepcodi)) entity.Reptiprepcodi = Convert.ToInt32(dr.GetValue(iReptiprepcodi));

            int iRepcatecodi = dr.GetOrdinal(this.Repcatecodi);
            if (!dr.IsDBNull(iRepcatecodi)) entity.Repcatecodi = Convert.ToInt32(dr.GetValue(iRepcatecodi));

            int iRepstado = dr.GetOrdinal(this.Repstado);
            if (!dr.IsDBNull(iRepstado)) entity.Repstado = Convert.ToInt32(dr.GetValue(iRepstado));

            int iRepusucreacion = dr.GetOrdinal(this.Repusucreacion);
            if (!dr.IsDBNull(iRepusucreacion)) entity.Repusucreacion = dr.GetString(iRepusucreacion);

            int iRepffeccreacion = dr.GetOrdinal(this.Repffeccreacion);
            if (!dr.IsDBNull(iRepffeccreacion)) entity.Repffeccreacion = dr.GetDateTime(iRepffeccreacion);

            int iRepusumodificacion = dr.GetOrdinal(this.Repusumodificacion);
            if (!dr.IsDBNull(iRepusumodificacion)) entity.Repusumodificacion = dr.GetString(iRepusumodificacion);

            int iRepfecmodificacion = dr.GetOrdinal(this.Repfecmodificacion);
            if (!dr.IsDBNull(iRepfecmodificacion)) entity.Repfecmodificacion = dr.GetDateTime(iRepfecmodificacion);

            int iReporden = dr.GetOrdinal(this.Reporden);
            if (!dr.IsDBNull(iReporden)) entity.Reporden = dr.GetInt32(iReporden);

            int iMreptituloweb = dr.GetOrdinal(this.Mreptituloweb);
            if (!dr.IsDBNull(iMreptituloweb)) entity.Mreptituloweb = dr.GetString(iMreptituloweb);

            int iMreptituloexcel = dr.GetOrdinal(this.Mreptituloexcel);
            if (!dr.IsDBNull(iMreptituloexcel)) entity.Mreptituloexcel = dr.GetString(iMreptituloexcel);

            return entity;
        }


        #region Mapeo de Campos

        public string Repcodi = "MREPCODI";
        public string Repdescripcion = "MREPDESCRIPCION";
        public string Repabrev = "MREPABREV";
        public string Reptiprepcodi = "tmrepcodi";
        public string Repcatecodi = "MREPCATECODI";
        public string Repstado = "MREPESTADO";
        public string Repusucreacion = "MREPUSUCREACION";
        public string Repffeccreacion = "MREPFFECCREACION";
        public string Repusumodificacion = "MREPUSUMODIFICACION";
        public string Repfecmodificacion = "MREPFECMODIFICACION";
        public string Reporden = "MREPORDEN";
        public string Repprojectcodi = "MPROJCODI";
        public string Mreptituloweb = "MREPTITULOWEB";
        public string Mreptituloexcel = "MREPTITULOEXCEL";

        #endregion

        public string SqlGetListaAdmReporte
        {
            get { return base.GetSqlXml("GetListaAdmReporte"); }
        }

        #region siosein2
        public string SqlGetSimenureportebyIndex
        {
            get { return base.GetSqlXml("GetSimenureportebyIndex"); }
        }
        #endregion
    }
}
