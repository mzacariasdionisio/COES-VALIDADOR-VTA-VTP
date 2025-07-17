using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class VtpVariacionEmpresaHelper: HelperBase
    {
        public VtpVariacionEmpresaHelper()
            : base(Consultas.VtpVariacionEmpresaSql)
        {
        }

        public VtpVariacionEmpresaDTO Create(IDataReader dr)
        {
            VtpVariacionEmpresaDTO entity = new VtpVariacionEmpresaDTO();

            
            int iVarempusucreacion = dr.GetOrdinal(this.VarEmpUsuCreacion);
            if (!dr.IsDBNull(iVarempusucreacion)) entity.Varempusucreacion = dr.GetString(iVarempusucreacion);

            int iVarempfeccreacion = dr.GetOrdinal(this.VarEmpFecCreacion);
            if (!dr.IsDBNull(iVarempfeccreacion)) entity.Varempfeccreacion = dr.GetDateTime(iVarempfeccreacion);

            int iVarempusumodificacion = dr.GetOrdinal(this.VarEmpUsuModificacion);
            if (!dr.IsDBNull(iVarempusumodificacion)) entity.Varempusumodificacion = dr.GetString(iVarempusumodificacion);

            int iVarempfecmodificacion = dr.GetOrdinal(this.VarEmpFecModificacion);
            if (!dr.IsDBNull(iVarempfecmodificacion)) entity.Varempfecmodificacion = dr.GetDateTime(iVarempfecmodificacion);

            int iVarempcodi = dr.GetOrdinal(this.VarEmpCodi);
            if (!dr.IsDBNull(iVarempcodi)) entity.Varempcodi = Convert.ToInt32(dr.GetValue(iVarempcodi));

            int iEmprcodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iVarempprocentaje = dr.GetOrdinal(this.VarEmpProcentaje);
            if (!dr.IsDBNull(iVarempprocentaje)) entity.Varempprocentaje = dr.GetDecimal(iVarempprocentaje);

            int iVaremptipocomp = dr.GetOrdinal(this.VarEmpTipoComp);
            if (!dr.IsDBNull(iVaremptipocomp)) entity.Varemptipocomp = dr.GetString(iVaremptipocomp);

            int iVarempvigencia = dr.GetOrdinal(this.VarEmpVigencia);
            if (!dr.IsDBNull(iVarempvigencia)) entity.Varempvigencia = dr.GetDateTime(iVarempvigencia);

            int iVarempestado = dr.GetOrdinal(this.VarEmpEstado);
            if (!dr.IsDBNull(iVarempestado)) entity.Varempestado = dr.GetString(iVarempestado);

            return entity;
        }

        #region Mapeo de Campos

        public string VarEmpUsuCreacion = "VAREMPUSUCREACION";
        public string VarEmpFecCreacion = "VAREMPFECCREACION";
        public string VarEmpUsuModificacion = "VAREMPUSUMODIFICACION";
        public string VarEmpFecModificacion = "VAREMPFECMODIFICACION";
        public string VarEmpCodi = "VAREMPCODI";
        public string EmprCodi = "EMPRCODI";
        public string VarEmpProcentaje = "VAREMPPROCENTAJE";
        public string VarEmpTipoComp = "VAREMPTIPOCOMP";
        public string VarEmpVigencia = "VAREMPVIGENCIA";
        public string VarEmpEstado = "VAREMPESTADO";

        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string SiEmprCodi = "siemprcodi";
        public string EmprNomb = "emprnomb";
        public string NroPagina = "nropagina";
        public string PageSize = "pagesize";
        public string Fila = "Fila";

        #endregion

        public string SqlGetDefaultPercentVariationByTipoComp
        {
            get { return base.GetSqlXml("GetDefaultPercentVariationByTipoComp"); }
        }

        public string SqlUpdateStatusVariationByTipoComp
        {
            get { return base.GetSqlXml("UpdateStatusVariationByTipoComp"); }
        }

        public string SqlListVariacionEmpresaByTipoComp
        {
            get { return base.GetSqlXml("ListVariacionEmpresaByTipoComp"); }
        }

        public string SqlGetNroRecordsVariacionEmpresaByTipoComp
        {
            get { return base.GetSqlXml("GetNroRecordsVariacionEmpresaByTipoComp"); }
        }

        public string SqlUpdateStatusVariationByTipoCompAndEmpresa
        {
            get { return base.GetSqlXml("UpdateStatusVariationByTipoCompAndEmpresa"); }
        }

        public string SqlListHistoryVariacionEmpresaByEmprCodiAndTipoComp
        {
            get { return base.GetSqlXml("ListHistoryVariacionEmpresaByEmprCodiAndTipoComp"); }
        }

        public string SqlGetPercentVariationByEmprCodiAndTipoComp
        {
            get { return base.GetSqlXml("GetPercentVariationByEmprCodiAndTipoComp"); }
        }

    }
}
