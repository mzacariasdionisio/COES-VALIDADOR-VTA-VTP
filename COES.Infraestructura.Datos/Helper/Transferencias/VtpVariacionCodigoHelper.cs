using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;


namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class VtpVariacionCodigoHelper: HelperBase
    {
        public VtpVariacionCodigoHelper()
            : base(Consultas.VtpVariacionCodigoSql)
        {
        }
        public VtpVariacionCodigoDTO Create(IDataReader dr)
        {
            VtpVariacionCodigoDTO entity = new VtpVariacionCodigoDTO();


            int iVarcodcodi = dr.GetOrdinal(this.VarCodCodi);
            if (!dr.IsDBNull(iVarcodcodi)) entity.VarCodCodi = Convert.ToInt32(dr.GetValue(iVarcodcodi));

            int iEmprcodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iBarrcodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iVarcodcodigovtp = dr.GetOrdinal(this.VarCodCodigoVtp);
            if (!dr.IsDBNull(iVarcodcodigovtp)) entity.VarCodCodigoVtp = dr.GetString(iVarcodcodigovtp);

            int iVarcodporcentaje = dr.GetOrdinal(this.VarCodPorcentaje);
            if (!dr.IsDBNull(iVarcodporcentaje)) entity.VarCodPorcentaje = dr.GetDecimal(iVarcodporcentaje);

            int iVarcodusucreacion = dr.GetOrdinal(this.VarCodUsuCreacion);
            if (!dr.IsDBNull(iVarcodusucreacion)) entity.VarCodUsuCreacion = dr.GetString(iVarcodusucreacion);

            int iVarcodfeccreacion = dr.GetOrdinal(this.VarCodFecCreacion);
            if (!dr.IsDBNull(iVarcodfeccreacion)) entity.VarCodFecCreacion = dr.GetDateTime(iVarcodfeccreacion);

            int iVarcodusumodificacion = dr.GetOrdinal(this.VarCodUsuModificacion);
            if (!dr.IsDBNull(iVarcodusumodificacion)) entity.VarCodUsuModificacion = dr.GetString(iVarcodusumodificacion);

            int iVarcodfecmodificacion = dr.GetOrdinal(this.VarCodFecModificacion);
            if (!dr.IsDBNull(iVarcodfecmodificacion)) entity.VarCodFecModificacion = dr.GetDateTime(iVarcodfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string VarCodCodi = "VARCODCODI";
        public string EmprCodi = "EMPRCODI";
        public string BarrCodi = "BARRCODI";
        public string VarCodCodigoVtp = "VARCODCODIGOVTP";
        public string VarCodPorcentaje = "VARCODPORCENTAJE";
        public string VarCodUsuCreacion = "VARCODUSUCREACION";
        public string VarCodFecCreacion = "VARCODFECCREACION";
        public string VarCodUsuModificacion = "VARCODUSUMODIFICACION";
        public string VarCodFecModificacion = "VARCODFECMODIFICACION";
        public string VarCodEstado = "VARCODESTADO";
        public string VarCodTipoComp = "VARCODTIPOCOMP";

        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string EmprNomb = "emprnomb";
        public string CliCodi = "clicodi";
        public string Cliente = "cliente";
        public string Barra = "barra";
        public string CodCnCodiVtp = "codcncodivtp";
        public string NroPagina = "nropagina";
        public string PageSize = "pagesize";
        public string Fila = "Fila";

        #endregion

        public string SqlGetVariacionCodigoByCodVtp
        {
            get { return base.GetSqlXml("GetVariacionCodigoByCodVtp"); }
        }

        public string SqlListVariacionCodigoByEmprCodi
        {
            get { return base.GetSqlXml("ListVariacionCodigoByEmprCodi"); }
        }

        public string SqlListVariacionCodigoVTEAByEmprCodi
        {
            get { return base.GetSqlXml("ListVariacionCodigoVTEAByEmprCodi"); }
        }

        public string SqlGetNroRecordsVariacionCodigoByEmprCodi
        {
            get { return base.GetSqlXml("GetNroRecordsVariacionCodigoByEmprCodi"); }
        }

        public string SqlGetNroRecordsVariacionCodigoVTEAByEmprCodi
        {
            get { return base.GetSqlXml("GetNroRecordsVariacionCodigoVTEAByEmprCodi"); }
        }
        
        public string SqlUpdateStatusVariationByCodigoVtp
        {
            get { return base.GetSqlXml("UpdateStatusVariationByCodigoVtp"); }
        }
        public string SqlListHistoryVariacionCodigoByCodigoVtp
        {
            get { return base.GetSqlXml("ListHistoryVariacionCodigoByCodigoVtp"); }
        }
        

    }
}
