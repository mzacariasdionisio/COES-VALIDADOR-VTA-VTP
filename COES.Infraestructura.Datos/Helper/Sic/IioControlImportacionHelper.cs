using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_CONTROL_IMPORTACION
    /// </summary>
    public class IioControlImportacionHelper : HelperBase
    {
        public IioControlImportacionHelper() : base(Consultas.IioControlImportacionSql)
        {        
        }

        public IioControlImportacionDTO Create(IDataReader dr)
        {
            int iRcimCodi = dr.GetOrdinal(RcimCodi);
            int iPeriodoCodi = dr.GetOrdinal(IioPeriodoSicliHelper.PsicliCodi);
            int iTablaCodi = dr.GetOrdinal(IioTablaSyncHelper.RtabCodi);
            int iRcimNroRegistros = dr.GetOrdinal(RcimNroRegistros);
            int iRcimNroRegistrosCoes = dr.GetOrdinal(RcimNroRegistrosCoes);
            int iRcimFecHorImportacion = dr.GetOrdinal(RcimFecHorImportacion);
            int iRcimEstadoImportacion = dr.GetOrdinal(RcimEstadoImportacion);
            int iRcimEstRegistro = dr.GetOrdinal(RcimEstRegistro);
            int iRcimUsuCreacion = dr.GetOrdinal(RcimUsuCreacion);
            int iRcimFecCreacion = dr.GetOrdinal(RcimFecCreacion);
            int iRcimUsuModificacion = dr.GetOrdinal(RcimUsuModificacion);
            int iRcimFecModificacion = dr.GetOrdinal(RcimFecModificacion);
            int iEnviocodi = dr.GetOrdinal(Enviocodi);
            int iRcimEmpresa = dr.GetOrdinal(RcimEmpresa);
            int iRcimEmpresaDesc = dr.GetOrdinal(RcimEmpresaDesc);
            int IRtabCodi = dr.GetOrdinal(RtabCodi);

            return new IioControlImportacionDTO
            {
                Rcimcodi = (!dr.IsDBNull(iRcimCodi) ? dr.GetInt32(iRcimCodi) : default(int)),
                Rcimestadoimportacion = (!dr.IsDBNull(iRcimEstadoImportacion) ? dr.GetString(iRcimEstadoImportacion) : null),
                Rcimestregistro = (!dr.IsDBNull(iRcimEstRegistro) ? dr.GetString(iRcimEstRegistro) : null),
                Rcimfeccreacion = (!dr.IsDBNull(iRcimFecCreacion) ? dr.GetDateTime(iRcimFecCreacion) : new DateTime()),
                Rcimfechorimportacion = (!dr.IsDBNull(iRcimFecHorImportacion) ? dr.GetDateTime(iRcimFecHorImportacion) : new DateTime()),
                Rcimfecmodificacion = (!dr.IsDBNull(iRcimFecModificacion) ? dr.GetDateTime(iRcimFecModificacion) : new DateTime()),
                Rcimnroregistros = (!dr.IsDBNull(iRcimNroRegistros) ? dr.GetInt32(iRcimNroRegistros) : default(int)),
                Rcimnroregistroscoes = (!dr.IsDBNull(iRcimNroRegistrosCoes) ? dr.GetInt32(iRcimNroRegistrosCoes) : default(int)),
                Rcimusucreacion = (!dr.IsDBNull(iRcimUsuCreacion) ? dr.GetString(iRcimUsuCreacion) : null),
                Rcimusumodificacion = (!dr.IsDBNull(iRcimUsuModificacion) ? dr.GetString(iRcimUsuModificacion) : null),
                Psiclicodi = (!dr.IsDBNull(iPeriodoCodi) ? dr.GetInt32(iPeriodoCodi) : default(int)),
                Rtabcodi = (!dr.IsDBNull(iTablaCodi) ? dr.GetString(iTablaCodi) : null),
                Enviocodi = (!dr.IsDBNull(iEnviocodi) ? dr.GetInt32(iEnviocodi) : default(int)),
                Rcimempresa = (!dr.IsDBNull(iRcimEmpresa) ? dr.GetString(iRcimEmpresa) : null),
                Rcimempresadesc = (!dr.IsDBNull(iRcimEmpresaDesc) ? dr.GetString(iRcimEmpresaDesc) : null)
            };

        }

        #region Mapeo de Campos

        public string TableName = "IIO_CONTROL_IMPORTACION";
        public string RcimCodi = "RCIMCODI";
        public string RtabCodi = "RTABCODI";
        public string RcimNroRegistros = "RCIMNROREGISTROS";
        public string RcimNroRegistrosCoes = "RCIMNROREGISTROSCOES";
        public string RcimFecHorImportacion = "RCIMFECHORIMPORTACION";
        public string RcimEstadoImportacion = "RCIMESTADOIMPORTACION";
        public string RcimEstRegistro = "RCIMESTREGISTRO";
        public string RcimUsuCreacion = "RCIMUSUCREACION";
        public string RcimFecCreacion = "RCIMFECCREACION";
        public string RcimUsuModificacion = "RCIMUSUMODIFICACION";
        public string RcimFecModificacion = "RCIMFECMODIFICACION";
        public string Enviocodi = "ENVIOCODI";
        public string RcimEmpresa = "RCIMEMPRESA";
        public string RcimEmpresaDesc = "RCIMEMPRESADESC";

        #region Campos Paginacion
        public string MaxRowToFetch = "MAXROWTOFETCH";
        public string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        #endregion


        public string SqlGetByPeriodo
        {
            get { return base.GetSqlXml("GetByPeriodo"); }
        }

        public string SqlListByTabla
        {
            get { return base.GetSqlXml("ListByTabla"); }
        }

        public string SqlGetByEmpresaTabla
        {
            get { return base.GetSqlXml("GetByEmpresaTabla"); }
        }
        
    }
}