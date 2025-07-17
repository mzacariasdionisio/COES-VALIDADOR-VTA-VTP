using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_RELEQPRY
    /// </summary>
    public class FtExtReleqpryHelper : HelperBase
    {
        public FtExtReleqpryHelper() : base(Consultas.FtExtReleqprySql)
        {
        }

        public FtExtReleqpryDTO Create(IDataReader dr)
        {
            FtExtReleqpryDTO entity = new FtExtReleqpryDTO();

            int iFtreqpcodi = dr.GetOrdinal(this.Ftreqpcodi);
            if (!dr.IsDBNull(iFtreqpcodi)) entity.Ftreqpcodi = Convert.ToInt32(dr.GetValue(iFtreqpcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iFtprycodi = dr.GetOrdinal(this.Ftprycodi);
            if (!dr.IsDBNull(iFtprycodi)) entity.Ftprycodi = Convert.ToInt32(dr.GetValue(iFtprycodi));

            int iFtreqpestado = dr.GetOrdinal(this.Ftreqpestado);
            if (!dr.IsDBNull(iFtreqpestado)) entity.Ftreqpestado = Convert.ToInt32(dr.GetValue(iFtreqpestado));

            int iFtreqpusucreacion = dr.GetOrdinal(this.Ftreqpusucreacion);
            if (!dr.IsDBNull(iFtreqpusucreacion)) entity.Ftreqpusucreacion = dr.GetString(iFtreqpusucreacion);

            int iFtreqpfeccreacion = dr.GetOrdinal(this.Ftreqpfeccreacion);
            if (!dr.IsDBNull(iFtreqpfeccreacion)) entity.Ftreqpfeccreacion = dr.GetDateTime(iFtreqpfeccreacion);

            int iFtreqpusumodificacion = dr.GetOrdinal(this.Ftreqpusumodificacion);
            if (!dr.IsDBNull(iFtreqpusumodificacion)) entity.Ftreqpusumodificacion = dr.GetString(iFtreqpusumodificacion);

            int iFtreqpfecmodificacion = dr.GetOrdinal(this.Ftreqpfecmodificacion);
            if (!dr.IsDBNull(iFtreqpfecmodificacion)) entity.Ftreqpfecmodificacion = dr.GetDateTime(iFtreqpfecmodificacion);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Ftreqpcodi = "FTREQPCODI";
        public string Equicodi = "EQUICODI";
        public string Ftprycodi = "FTPRYCODI";
        public string Ftreqpestado = "FTREQPESTADO";
        public string Ftreqpusucreacion = "FTREQPUSUCREACION";
        public string Ftreqpfeccreacion = "FTREQPFECCREACION";
        public string Ftreqpusumodificacion = "FTREQPUSUMODIFICACION";
        public string Ftreqpfecmodificacion = "FTREQPFECMODIFICACION";
        public string Grupocodi = "GRUPOCODI";

        public string Emprnomb = "EMPRNOMB";
        public string Ftpryeocodigo = "FTPRYEOCODIGO";
        public string Ftpryeonombre = "FTPRYEONOMBRE";
        public string Ftprynombre = "FTPRYNOMBRE";

        public string Nombempresaelemento = "NOMBEMPRESAELEMENTO";
        public string Idempresaelemento = "IDEMPRESAELEMENTO";
        public string Nombreelemento = "NOMBREELEMENTO";
        public string Tipoelemento = "TIPOELEMENTO";
        public string Areaelemento = "AREAELEMENTO";
        public string Estadoelemento = "ESTADOELEMENTO";

        public string Idempresacopelemento = "IDEMPRESACOPELEMENTO";
        public string Nombempresacopelemento = "NOMBEMPRESACOPELEMENTO";

        public string Famcodi = "FAMCODI";
        public string Catecodi = "CATECODI";

        #endregion

        public string SqlListarPorEquipo
        {
            get { return base.GetSqlXml("ListarPorEquipo"); }
        }

        public string SqlListarPorGrupo
        {
            get { return base.GetSqlXml("ListarPorGrupo"); }
        }

        public string SqlListarSoloEquipos
        {
            get { return base.GetSqlXml("ListarSoloEquipos"); }
        }

        public string SqlListarSoloGrupos
        {
            get { return base.GetSqlXml("ListarSoloGrupos"); }
        }

        public string SqlListarPorEmpresaPropYProyecto
        {
            get { return base.GetSqlXml("ListarPorEmpresaPropYProyecto"); }
        }

        public string SqlListarPorEmpresaCopropYProyecto
        {
            get { return base.GetSqlXml("ListarPorEmpresaCopropYProyecto"); }
        }

    }
}
