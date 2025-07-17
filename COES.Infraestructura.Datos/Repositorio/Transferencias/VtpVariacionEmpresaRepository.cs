using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTP_VARIACION_EMPRESA
    /// </summary>
    public class VtpVariacionEmpresaRepository : RepositoryBase, IVtpVariacionEmpresaRepository
    {

        public VtpVariacionEmpresaRepository(string strConn)
            : base(strConn)
        {
        }

        VtpVariacionEmpresaHelper helper = new VtpVariacionEmpresaHelper();

        public int Save(VtpVariacionEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.VarEmpCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.VarEmpProcentaje, DbType.Decimal, entity.Varempprocentaje);
            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, entity.Varemptipocomp);
            dbProvider.AddInParameter(command, helper.VarEmpVigencia, DbType.DateTime, entity.Varempvigencia);
            dbProvider.AddInParameter(command, helper.VarEmpEstado, DbType.String, entity.Varempestado);
            dbProvider.AddInParameter(command, helper.VarEmpUsuCreacion, DbType.String, entity.Varempusucreacion);
            dbProvider.AddInParameter(command, helper.VarEmpFecCreacion, DbType.DateTime, entity.Varempfeccreacion);
            dbProvider.AddInParameter(command, helper.VarEmpUsuModificacion, DbType.String, entity.Varempusumodificacion);
            dbProvider.AddInParameter(command, helper.VarEmpFecModificacion, DbType.DateTime, entity.Varempfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public VtpVariacionEmpresaDTO GetDefaultPercentVariationByTipoComp(string varemptipocomp)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetDefaultPercentVariationByTipoComp);

            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, varemptipocomp);
            VtpVariacionEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void UpdateStatusVariationByTipoComp(VtpVariacionEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateStatusVariationByTipoComp);

            dbProvider.AddInParameter(command, helper.VarEmpEstado, DbType.String, entity.Varempestado);
            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, entity.Varemptipocomp);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateStatusVariationByTipoCompAndEmpresa(VtpVariacionEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateStatusVariationByTipoCompAndEmpresa);

            dbProvider.AddInParameter(command, helper.VarEmpEstado, DbType.String, entity.Varempestado);
            dbProvider.AddInParameter(command, helper.VarEmpUsuModificacion, DbType.String, entity.Varempusumodificacion);
            dbProvider.AddInParameter(command, helper.VarEmpFecModificacion, DbType.DateTime, entity.Varempfecmodificacion);
            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, entity.Varemptipocomp);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.Emprcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<VtpVariacionEmpresaDTO> ListVariacionEmpresaByTipoComp(string varemptipocomp, int NroPagina, int PageSize, string varemprnomb)
        {
            List<VtpVariacionEmpresaDTO> entitys = new List<VtpVariacionEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListVariacionEmpresaByTipoComp);
            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, varemptipocomp);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, varemprnomb);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, varemprnomb);
            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, varemptipocomp);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, varemprnomb);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, varemprnomb);
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpVariacionEmpresaDTO entity = new VtpVariacionEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iSiemprcodi = dr.GetOrdinal(this.helper.SiEmprCodi);
                    if (!dr.IsDBNull(iSiemprcodi)) entity.Siemprcodi = Convert.ToInt32(dr.GetValue(iSiemprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iVarempprocentaje = dr.GetOrdinal(this.helper.VarEmpProcentaje);
                    if (!dr.IsDBNull(iVarempprocentaje)) entity.Varempprocentaje = dr.GetDecimal(iVarempprocentaje);

                    int iVarempvigencia = dr.GetOrdinal(this.helper.VarEmpVigencia);
                    if (!dr.IsDBNull(iVarempvigencia)) entity.Varempvigencia = dr.GetDateTime(iVarempvigencia);

                    int iFila = dr.GetOrdinal(this.helper.Fila);
                    if (!dr.IsDBNull(iFila)) entity.fila = Convert.ToInt32(dr.GetValue(iFila));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpVariacionEmpresaDTO> ListHistoryVariacionEmpresaByEmprCodiAndTipoComp(string varemptipocomp, int emprcodi)
        {
            List<VtpVariacionEmpresaDTO> entitys = new List<VtpVariacionEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListHistoryVariacionEmpresaByEmprCodiAndTipoComp);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, varemptipocomp);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpVariacionEmpresaDTO entity = new VtpVariacionEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iVarempprocentaje = dr.GetOrdinal(this.helper.VarEmpProcentaje);
                    if (!dr.IsDBNull(iVarempprocentaje)) entity.Varempprocentaje = dr.GetDecimal(iVarempprocentaje);

                    int iVarempvigencia = dr.GetOrdinal(this.helper.VarEmpVigencia);
                    if (!dr.IsDBNull(iVarempvigencia)) entity.Varempvigencia = dr.GetDateTime(iVarempvigencia);

                    int iVarempestado = dr.GetOrdinal(this.helper.VarEmpEstado);
                    if (!dr.IsDBNull(iVarempestado)) entity.Varempestado = dr.GetString(iVarempestado);

                    int iVarempusumodificacion = dr.GetOrdinal(this.helper.VarEmpUsuModificacion);
                    if (!dr.IsDBNull(iVarempusumodificacion)) entity.Varempusumodificacion = dr.GetString(iVarempusumodificacion);

                    int iVarempfecmodificacion = dr.GetOrdinal(this.helper.VarEmpFecModificacion);
                    if (!dr.IsDBNull(iVarempfecmodificacion)) entity.Varempfecmodificacion = dr.GetDateTime(iVarempfecmodificacion);

                    int iVarempfeccreacion = dr.GetOrdinal(this.helper.VarEmpFecCreacion);
                    if (!dr.IsDBNull(iVarempfeccreacion)) entity.Varempfeccreacion = dr.GetDateTime(iVarempfeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetNroRecordsVariacionEmpresaByTipoComp(string varemptipocomp, string varemprnomb)
        {
            int NroRegistros = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetNroRecordsVariacionEmpresaByTipoComp);
            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, varemptipocomp);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, varemprnomb);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, varemprnomb);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, varemprnomb);
            dbProvider.AddInParameter(command, helper.EmprNomb, DbType.String, varemprnomb);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }

        public VtpVariacionEmpresaDTO GetPercentVariationByEmprCodiAndTipoComp(int emprcodi, string varemptipocomp)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPercentVariationByEmprCodiAndTipoComp);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.VarEmpTipoComp, DbType.String, varemptipocomp);
            VtpVariacionEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
