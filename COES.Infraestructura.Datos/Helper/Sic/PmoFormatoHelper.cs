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
    public class PmoFormatoHelper : HelperBase
    {
        public PmoFormatoHelper()
            : base(Consultas.PmoFormato)
        {
        }

        public PmoFormatoDTO Create(IDataReader dr)
        {
            PmoFormatoDTO entity = new PmoFormatoDTO();

            int iPmFTabCodi = dr.GetOrdinal(this.PmFTabCodi);
            if (!dr.IsDBNull(iPmFTabCodi)) entity.PmFTabCodi = dr.GetInt32(iPmFTabCodi);

            int iPmFTabNombreTabla = dr.GetOrdinal(this.PmFTabNombreTabla);
            if (!dr.IsDBNull(iPmFTabNombreTabla)) entity.PmFTabNombreTabla = dr.GetString(iPmFTabNombreTabla);

            int iPmFTabDescripcionTabla = dr.GetOrdinal(this.PmFTabDescripcionTabla);
            if (!dr.IsDBNull(iPmFTabDescripcionTabla)) entity.PmFTabDescripcionTabla = dr.GetString(iPmFTabDescripcionTabla);

            int iPmFTabGrupoArchivo = dr.GetOrdinal(this.PmFTabGrupoArchivo);
            if (!dr.IsDBNull(iPmFTabGrupoArchivo)) entity.PmFTabGrupoArchivo = dr.GetString(iPmFTabGrupoArchivo);

            int iPmFTabTipoArchivo = dr.GetOrdinal(this.PmFTabTipoArchivo);
            if (!dr.IsDBNull(iPmFTabTipoArchivo)) entity.PmFTabTipoArchivo = dr.GetString(iPmFTabTipoArchivo);

            int iPmFTabNombArchivo = dr.GetOrdinal(this.PmFTabNombArchivo);
            if (!dr.IsDBNull(iPmFTabNombArchivo)) entity.PmFTabNombArchivo = dr.GetString(iPmFTabNombArchivo);

            int iPmFTabQueryCab = dr.GetOrdinal(this.PmFTabQueryCab);
            if (!dr.IsDBNull(iPmFTabQueryCab)) entity.PmFTabQueryCab = dr.GetString(iPmFTabQueryCab);

            int iPmFTabQueryDet = dr.GetOrdinal(this.PmFTabQueryDet);
            if (!dr.IsDBNull(iPmFTabQueryDet)) entity.PmFTabQueryDet = dr.GetString(iPmFTabQueryDet);

            int iPmFTabQueryCount = dr.GetOrdinal(this.PmFTabQueryCount);
            if (!dr.IsDBNull(iPmFTabQueryCount)) entity.PmFTabQueryCount = dr.GetString(iPmFTabQueryCount);

            int iPmFTabOrden = dr.GetOrdinal(this.PmFTabOrden);
            if (!dr.IsDBNull(iPmFTabOrden)) entity.PmFTabOrden = dr.GetDecimal(iPmFTabOrden);

            int iPmFTabEstRegistro = dr.GetOrdinal(this.PmFTabEstRegistro);
            if (!dr.IsDBNull(iPmFTabEstRegistro)) entity.PmFTabEstRegistro = dr.GetString(iPmFTabEstRegistro);

            int iPmFTabUsuCreacion = dr.GetOrdinal(this.PmFTabUsuCreacion);
            if (!dr.IsDBNull(iPmFTabUsuCreacion)) entity.PmFTabUsuCreacion = dr.GetString(iPmFTabUsuCreacion);

            int iPmFTabFecCreacion = dr.GetOrdinal(this.PmFTabFecCreacion);
            if (!dr.IsDBNull(iPmFTabFecCreacion)) entity.PmFTabFecCreacion = dr.GetDateTime(iPmFTabFecCreacion);

            int iPmFTabUsuModificacion = dr.GetOrdinal(this.PmFTabUsuModificacion);
            if (!dr.IsDBNull(iPmFTabUsuModificacion)) entity.PmFTabUsuModificacion = dr.GetString(iPmFTabUsuModificacion);

            int iPmFTabFecModificacion = dr.GetOrdinal(this.PmFTabFecModificacion);
            if (!dr.IsDBNull(iPmFTabFecModificacion)) entity.PmFTabQueryCount = dr.GetString(iPmFTabFecModificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string PmFTabCodi = "PMFTABCODI";
        public string PmFTabNombreTabla = "PMFTABNOMBRETABLA";
        public string PmFTabDescripcionTabla = "PMFTABDESCRIPCIONTABLA";
        public string PmFTabGrupoArchivo = "PMFTABGRUPOARCHIVO";
        public string PmFTabTipoArchivo = "PMFTABTIPOARCHIVO";
        public string PmFTabNombArchivo = "PMFTABNOMBARCHIVO";
        public string PmFTabQueryCab = "PMFTABQUERYCAB";
        public string PmFTabQueryDet = "PMFTABQUERYDET";
        public string PmFTabQueryCount = "PMFTABQUERYCOUNT";
        public string PmFTabOrden = "PMFTABORDEN";
        public string PmFTabEstRegistro = "PMFTABESTREGISTRO";
        public string PmFTabUsuCreacion = "PMFTABUSUCREACION";
        public string PmFTabFecCreacion = "PMFTABFECCREACION";
        public string PmFTabUsuModificacion = "PMFTABUSUMODIFICACION";
        public string PmFTabFecModificacion = "PMFTABFECMODIFICACION";

        #region SIOSEIN
        public string Ptomedicodi = "PTOMEDICODI";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "Equinomb";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Osinergcodi = "OSINERGCODI";
        public string Osicodi = "OSICODI";
        #endregion

        #endregion

        #region SIOSEIN
        public string SqlGetFormatPtomedicion
        {
            get { return GetSqlXml("GetFormatPtomedicion"); }
        }
        #endregion
    }
}
