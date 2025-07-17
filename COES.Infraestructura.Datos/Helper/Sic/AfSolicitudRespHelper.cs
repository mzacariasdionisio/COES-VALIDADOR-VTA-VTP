using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AF_SOLICITUD_RESP
    /// </summary>
    public class AfSolicitudRespHelper : HelperBase
    {
        public AfSolicitudRespHelper() : base(Consultas.AfSolicitudRespSql)
        {
        }

        public AfSolicitudRespDTO Create(IDataReader dr)
        {
            AfSolicitudRespDTO entity = new AfSolicitudRespDTO();

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iSoresparchivootros = dr.GetOrdinal(this.Soresparchivootros);
            if (!dr.IsDBNull(iSoresparchivootros)) entity.Soresparchivootros = dr.GetString(iSoresparchivootros);

            int iSoresparchivoinf = dr.GetOrdinal(this.Soresparchivoinf);
            if (!dr.IsDBNull(iSoresparchivoinf)) entity.Soresparchivoinf = dr.GetString(iSoresparchivoinf);

            int iSorespobsarchivo = dr.GetOrdinal(this.Sorespobsarchivo);
            if (!dr.IsDBNull(iSorespobsarchivo)) entity.Sorespobsarchivo = dr.GetString(iSorespobsarchivo);

            int iSorespobs = dr.GetOrdinal(this.Sorespobs);
            if (!dr.IsDBNull(iSorespobs)) entity.Sorespobs = dr.GetString(iSorespobs);

            int iSorespusucreacion = dr.GetOrdinal(this.Sorespusucreacion);
            if (!dr.IsDBNull(iSorespusucreacion)) entity.Sorespusucreacion = dr.GetString(iSorespusucreacion);

            int iSorespfeccreacion = dr.GetOrdinal(this.Sorespfeccreacion);
            if (!dr.IsDBNull(iSorespfeccreacion)) entity.Sorespfeccreacion = dr.GetDateTime(iSorespfeccreacion);

            int iSorespusumodificacion = dr.GetOrdinal(this.Sorespusumodificacion);
            if (!dr.IsDBNull(iSorespusumodificacion)) entity.Sorespusumodificacion = dr.GetString(iSorespusumodificacion);

            int iSorespfecmodificacion = dr.GetOrdinal(this.Sorespfecmodificacion);
            if (!dr.IsDBNull(iSorespfecmodificacion)) entity.Sorespfecmodificacion = dr.GetDateTime(iSorespfecmodificacion);

            int iSorespestadosol = dr.GetOrdinal(this.Sorespestadosol);
            if (!dr.IsDBNull(iSorespestadosol)) entity.Sorespestadosol = dr.GetString(iSorespestadosol);

            int iSorespdesc = dr.GetOrdinal(this.Sorespdesc);
            if (!dr.IsDBNull(iSorespdesc)) entity.Sorespdesc = dr.GetString(iSorespdesc);

            int iSorespfechaevento = dr.GetOrdinal(this.Sorespfechaevento);
            if (!dr.IsDBNull(iSorespfechaevento)) entity.Sorespfechaevento = dr.GetDateTime(iSorespfechaevento);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iSorespcodi = dr.GetOrdinal(this.Sorespcodi);
            if (!dr.IsDBNull(iSorespcodi)) entity.Sorespcodi = Convert.ToInt32(dr.GetValue(iSorespcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Enviocodi = "ENVIOCODI";
        public string Soresparchivootros = "SORESPARCHIVOOTROS";
        public string Soresparchivoinf = "SORESPARCHIVOINF";
        public string Sorespobsarchivo = "SORESPOBSARCHIVO";
        public string Sorespobs = "SORESPOBS";
        public string Sorespusucreacion = "SORESPUSUCREACION";
        public string Sorespfeccreacion = "SORESPFECCREACION";
        public string Sorespusumodificacion = "SORESPUSUMODIFICACION";
        public string Sorespfecmodificacion = "SORESPFECMODIFICACION";
        public string Sorespestadosol = "SORESPESTADOSOL";
        public string Sorespdesc = "SORESPDESC";
        public string Sorespfechaevento = "SORESPFECHAEVENTO";
        public string Emprcodi = "EMPRCODI";
        public string Sorespcodi = "SORESPCODI";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string sqlListarSolicitudesxFiltro
        {
            get { return base.GetSqlXml("ListarSolicitudesxFiltro"); }
        }

    }
}
