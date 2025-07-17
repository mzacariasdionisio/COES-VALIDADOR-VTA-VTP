using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_PROGRAMA
    /// </summary>
    public class RcaProgramaHelper : HelperBase
    {
        public RcaProgramaHelper(): base(Consultas.RcaProgramaSql)
        {
        }

        public RcaProgramaDTO Create(IDataReader dr)
        {
            RcaProgramaDTO entity = new RcaProgramaDTO();

            int iRcprogcodi = dr.GetOrdinal(this.Rcprogcodi);
            if (!dr.IsDBNull(iRcprogcodi)) entity.Rcprogcodi = Convert.ToInt32(dr.GetValue(iRcprogcodi));

            int iRchorpcodi = dr.GetOrdinal(this.Rchorpcodi);
            if (!dr.IsDBNull(iRchorpcodi)) entity.Rchorpcodi = Convert.ToInt32(dr.GetValue(iRchorpcodi));

            int iRcprogabrev = dr.GetOrdinal(this.Rcprogabrev);
            if (!dr.IsDBNull(iRcprogabrev)) entity.Rcprogabrev = dr.GetString(iRcprogabrev);

            int iRcprognombre = dr.GetOrdinal(this.Rcprognombre);
            if (!dr.IsDBNull(iRcprognombre)) entity.Rcprognombre = dr.GetString(iRcprognombre);

            int iRcprogestregistro = dr.GetOrdinal(this.Rcprogestregistro);
            if (!dr.IsDBNull(iRcprogestregistro)) entity.Rcprogestregistro = dr.GetString(iRcprogestregistro);

            int iRcprogestado = dr.GetOrdinal(this.Rcprogestado);
            if (!dr.IsDBNull(iRcprogestado)) entity.Rcprogestado = dr.GetString(iRcprogestado);

            int iRcprogusucreacion = dr.GetOrdinal(this.Rcprogusucreacion);
            if (!dr.IsDBNull(iRcprogusucreacion)) entity.Rcprogusucreacion = dr.GetString(iRcprogusucreacion);

            int iRcprogfeccreacion = dr.GetOrdinal(this.Rcprogfeccreacion);
            if (!dr.IsDBNull(iRcprogfeccreacion)) entity.Rcprogfeccreacion = dr.GetDateTime(iRcprogfeccreacion);

            int iRcprogusumodificacion = dr.GetOrdinal(this.Rcprogusumodificacion);
            if (!dr.IsDBNull(iRcprogusumodificacion)) entity.Rcprogusumodificacion = dr.GetString(iRcprogusumodificacion);

            int iRcprogfecmodificacion = dr.GetOrdinal(this.Rcprogfecmodificacion);
            if (!dr.IsDBNull(iRcprogfecmodificacion)) entity.Rcprogfecmodificacion = dr.GetDateTime(iRcprogfecmodificacion);

            int iRcprogfecinicio = dr.GetOrdinal(this.Rcprogfecinicio);
            if (!dr.IsDBNull(iRcprogfecinicio)) entity.Rcprogfecinicio = dr.GetDateTime(iRcprogfecinicio);

            int iRcprogfecfin = dr.GetOrdinal(this.Rcprogfecfin);
            if (!dr.IsDBNull(iRcprogfecfin)) entity.Rcprogfecfin = dr.GetDateTime(iRcprogfecfin);

            return entity;
        }


        #region Mapeo de Campos

        public string Rcprogcodi = "RCPROGCODI";
        public string Rcproghorizonte = "RCPROGHORIZONTE";
        public string Rcprogabrev = "RCPROGABREV";
        public string Rchorpcodi = "RCHORPCODI";
        public string Rcprognombre = "RCPROGNOMBRE";
        public string Rcprogestregistro = "RCPROGESTREGISTRO";
        public string Rcprogestado = "RCPROGESTADO";
        public string Rcprogusucreacion = "RCPROGUSUCREACION";
        public string Rcprogfeccreacion = "RCPROGFECCREACION";
        public string Rcprogusumodificacion = "RCPROGUSUMODIFICACION";
        public string Rcprogfecmodificacion = "RCPROGFECMODIFICACION";
        public string Rcprogcodipadre = "RCPROGCODIPADRE";
        public string Rcprogfecinicio = "RCPROGFECINICIO";
        public string Rcprogfecfin = "RCPROGFECFIN";

        public string NroCuadros = "NROCUADROS";

        #endregion

        public string SqlListProgramaRechazoCarga
        {
            get { return base.GetSqlXml("ListProgramaRechazoCarga"); }
        }

        public string SqListEmpresasProgramaRechazoCarga
        {
            get { return base.GetSqlXml("ListEmpresasProgramaRechazoCarga"); }
        }

        public string SqListProgramaEnvioArchivo
        {
            get { return base.GetSqlXml("ListProgramaEnvioArchivo"); }
        }

        public string SqListFiltro
        {
            get { return base.GetSqlXml("ListFiltro"); }
        }
    }
}
