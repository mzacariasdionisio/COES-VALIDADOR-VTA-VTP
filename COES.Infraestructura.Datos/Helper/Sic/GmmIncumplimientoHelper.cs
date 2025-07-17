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
    /// Clase que contiene el mapeo de la tabla FT_FICTECDET
    /// </summary>
    public class GmmIncumplimientoHelper : HelperBase
    {
        public GmmIncumplimientoHelper()
            : base(Consultas.GmmIncumplimientoSql)
        {

        }
        #region Mapeo de Campos
        
        public string Incucodi = "INCUCODI";
        public string Incuanio = "INCUANIO";
        public string Incumes = "INCUMES";
        public string Incuaceptado = "INCUACEPTADO";
        public string Incusubsanado = "INCUSUBSANADO";
        public string Empgcodi = "EMPGCODI";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Incumonto = "INCUMONTO";
        public string Incuusucreacion = "INCUUSUCREACION";
        public string Incufeccreacion = "INCUFECCREACION";
        public string Incuusumodificacion = "INCUUSUMODIFICACION";
        public string Incufecmodificacion = "INCUFECMODIFICACION";

        public string IncumAnioMes = "IncumAnioMes";
        public string IncumEmprAfectada = "IncumEmprAfectada";
        public string IncumEmprDeudora = "IncumEmprDeudora";
        public string IncumMonto = "IncumMonto";
        public string IncumInforme = "IncumInforme";

        public string IncucodiEdit = "IncucodiEdit";
        public string IncuanioEdit = "IncuanioEdit";
        public string IncumesEdit = "IncumesEdit";
        public string IncumplidoraEdit = "IncumplidoraEdit";
        public string AfectadaEdit = "AfectadaEdit";
        public string IncumMontoEdit = "IncumMontoEdit";
        public string EmpgcodiEdit = "EmpgcodiEdit";
        public string EmprcodiEdit = "EmprcodiEdit";

        #endregion

        public GmmIncumplimientoDTO Create(IDataReader dr)
        {
            GmmIncumplimientoDTO entity = new GmmIncumplimientoDTO();

            #region Incumplimiento
            int iIncucodi = dr.GetOrdinal(this.Incucodi);
            if (!dr.IsDBNull(iIncucodi)) entity.INCUCODI = Convert.ToInt32(dr.GetValue(iIncucodi));

            int iIncuanio = dr.GetOrdinal(this.Incuanio);
            if (!dr.IsDBNull(iIncuanio)) entity.INCUANIO = dr.GetInt32(iIncuanio);

            int iIncumes = dr.GetOrdinal(this.Incumes);
            if (!dr.IsDBNull(iIncumes)) entity.INCUMES = dr.GetString(iIncumes);

            int iIncuaceptado = dr.GetOrdinal(this.Incuaceptado);
            if (!dr.IsDBNull(iIncuaceptado)) entity.INCUACEPTADO = dr.GetString(iIncuaceptado);

            int iIncusubsanado = dr.GetOrdinal(this.Incusubsanado);
            if (!dr.IsDBNull(iIncusubsanado)) entity.INCUSUBSANADO = dr.GetString(iIncusubsanado);

            int iEmpgcodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = dr.GetInt32(iEmpgcodi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EMPRCODI = dr.GetInt32(iEmprcodi);

            int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            if (!dr.IsDBNull(iTipoemprcodi)) entity.TIPOEMPRCODI = dr.GetInt32(iTipoemprcodi);

            int iIncumonto = dr.GetOrdinal(this.Incumonto);
            if (!dr.IsDBNull(iIncumonto)) entity.INCUMONTO = dr.GetInt32(iIncumonto);

            int iIncuusucreacion = dr.GetOrdinal(this.Incuusucreacion);
            if (!dr.IsDBNull(iIncuusucreacion)) entity.INCUUSUCREACION = dr.GetString(iIncuusucreacion);

            int iIncufeccreacion = dr.GetOrdinal(this.Incufeccreacion);
            if (!dr.IsDBNull(iIncufeccreacion)) entity.INCUFECCREACION = dr.GetDateTime(iIncufeccreacion);

            int iIncuusumodificacion = dr.GetOrdinal(this.Incuusumodificacion);
            if (!dr.IsDBNull(iIncuusumodificacion)) entity.INCUUSUMODIFICACION = dr.GetString(iIncuusumodificacion);

            int iIncufecmodificacion = dr.GetOrdinal(this.Incufecmodificacion);
            if (!dr.IsDBNull(iIncufecmodificacion)) entity.INCUFECMODIFICACION = dr.GetDateTime(iIncufecmodificacion);
            #endregion

            return entity;
        }
        
        public GmmIncumplimientoDTO CreateListaIncumplimiento(IDataReader dr)
        {
            GmmIncumplimientoDTO entity = new GmmIncumplimientoDTO();

            #region Incumplimiento
            int iIncucodiEdit = dr.GetOrdinal(this.IncucodiEdit);
            if (!dr.IsDBNull(iIncucodiEdit)) entity.IncucodiEdit = dr.GetInt32(iIncucodiEdit);

            int iIncumAnioMes = dr.GetOrdinal(this.IncumAnioMes);
            if (!dr.IsDBNull(iIncumAnioMes)) entity.IncumAnioMes = dr.GetString(iIncumAnioMes);

            int iIncumEmprAfectada = dr.GetOrdinal(this.IncumEmprAfectada);
            if (!dr.IsDBNull(iIncumEmprAfectada)) entity.IncumEmprAfectada = dr.GetString(iIncumEmprAfectada);

            int iIncumEmprDeudora = dr.GetOrdinal(this.IncumEmprDeudora);
            if (!dr.IsDBNull(iIncumEmprDeudora)) entity.IncumEmprDeudora = dr.GetString(iIncumEmprDeudora);

            int iIncumMonto = dr.GetOrdinal(this.IncumMonto);
            if (!dr.IsDBNull(iIncumMonto)) entity.IncumMonto = dr.GetDecimal(iIncumMonto);

            int iIncumInforme = dr.GetOrdinal(this.IncumInforme);
            if (!dr.IsDBNull(iIncumInforme)) entity.IncumInforme = dr.GetString(iIncumInforme);
            #endregion

            return entity;
        }
        public GmmIncumplimientoDTO CreateIncumplimientoEdit(IDataReader dr)
        {
            GmmIncumplimientoDTO entity = new GmmIncumplimientoDTO();

            #region Incumplimiento
            int iIncucodiEdit = dr.GetOrdinal(this.IncucodiEdit);
            if (!dr.IsDBNull(iIncucodiEdit)) entity.IncucodiEdit = Convert.ToInt32(dr.GetValue(iIncucodiEdit));

            int iIncuanioEdit = dr.GetOrdinal(this.IncuanioEdit);
            if (!dr.IsDBNull(iIncuanioEdit)) entity.IncuanioEdit = dr.GetInt32(iIncuanioEdit);

            int iIncumesEdit = dr.GetOrdinal(this.IncumesEdit);
            if (!dr.IsDBNull(iIncumesEdit)) entity.IncumesEdit = dr.GetString(iIncumesEdit);

            int iIncumplidoraEdit = dr.GetOrdinal(this.IncumplidoraEdit);
            if (!dr.IsDBNull(iIncumplidoraEdit)) entity.IncumplidoraEdit = dr.GetString(iIncumplidoraEdit);

            int iAfectadaEdit = dr.GetOrdinal(this.AfectadaEdit);
            if (!dr.IsDBNull(iAfectadaEdit)) entity.AfectadaEdit = dr.GetString(iAfectadaEdit);

            int iIncumMontoEdit = dr.GetOrdinal(this.IncumMontoEdit);
            if (!dr.IsDBNull(iIncumMontoEdit)) entity.IncumMontoEdit = dr.GetDecimal(iIncumMontoEdit);

            int iEmpgcodiEdit = dr.GetOrdinal(this.EmpgcodiEdit);
            if (!dr.IsDBNull(iEmpgcodiEdit)) entity.EmpgcodiEdit = dr.GetInt32(iEmpgcodiEdit);

            int iEmprcodiEdit = dr.GetOrdinal(this.EmprcodiEdit);
            if (!dr.IsDBNull(iEmprcodiEdit)) entity.EmprcodiEdit = dr.GetInt32(iEmprcodiEdit);

            int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            if (!dr.IsDBNull(iTipoemprcodi)) entity.TIPOEMPRCODI = dr.GetInt32(iTipoemprcodi);

            #endregion

            return entity;
        }
        public string SqlGetByIdEdit
        {
            get { return base.GetSqlXml("GetByIdEdit"); }
        }
        public string SqlGetByIdEditGeneradoras
        {
            get { return base.GetSqlXml("GetByIdEditGeneradoras"); }
        }
        public string SqlListarFiltroIncumplimientoDeudora
        {
            get { return base.GetSqlXml("ListarFiltroIncumplimientoDeudora"); }
        }
        public string SqlListarFiltroIncumplimientoAfectada
        {
            get { return base.GetSqlXml("ListarFiltroIncumplimientoAfectada"); }
        }
        public string SqlUpdateTrienio
        {
            get { return base.GetSqlXml("UpdateTrienio"); }
        }
    }
}
