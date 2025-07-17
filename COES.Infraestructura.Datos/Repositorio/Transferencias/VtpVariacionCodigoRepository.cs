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
    public class VtpVariacionCodigoRepository : RepositoryBase, IVtpVariacionCodigoRepository
    {
        public VtpVariacionCodigoRepository(string strConn)
            : base(strConn)
        {
        }
        VtpVariacionCodigoHelper helper = new VtpVariacionCodigoHelper();

        public int Save(VtpVariacionCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.VarCodCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.VarCodCodigoVtp, DbType.String, entity.VarCodCodigoVtp);
            dbProvider.AddInParameter(command, helper.VarCodPorcentaje, DbType.Decimal, entity.VarCodPorcentaje);
            dbProvider.AddInParameter(command, helper.VarCodUsuCreacion, DbType.String, entity.VarCodUsuCreacion);
            dbProvider.AddInParameter(command, helper.VarCodFecCreacion, DbType.DateTime, entity.VarCodFecCreacion);
            dbProvider.AddInParameter(command, helper.VarCodUsuModificacion, DbType.String, entity.VarCodUsuModificacion);
            dbProvider.AddInParameter(command, helper.VarCodFecModificacion, DbType.DateTime, entity.VarCodFecModificacion);
            dbProvider.AddInParameter(command, helper.CliCodi, DbType.Int32, entity.CliCodi);
            dbProvider.AddInParameter(command, helper.VarCodEstado, DbType.String, entity.VarCodEstado);
            dbProvider.AddInParameter(command, helper.VarCodTipoComp, DbType.String, entity.VarCodTipoComp);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public VtpVariacionCodigoDTO GetVariacionCodigoByCodVtp(string varcodcodigovtp)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetVariacionCodigoByCodVtp);

            dbProvider.AddInParameter(command, helper.VarCodCodigoVtp, DbType.String, varcodcodigovtp);
            VtpVariacionCodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpVariacionCodigoDTO> ListVariacionCodigoByEmprCodi(int emprcodi, int NroPagina, int PageSize, string varCodiVtp)
        {
            List<VtpVariacionCodigoDTO> entitys = new List<VtpVariacionCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListVariacionCodigoByEmprCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);
            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpVariacionCodigoDTO entity = new VtpVariacionCodigoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iClicodi = dr.GetOrdinal(this.helper.CliCodi);
                    if (!dr.IsDBNull(iClicodi)) entity.CliCodi = Convert.ToInt32(dr.GetValue(iClicodi));

                    int iCliente = dr.GetOrdinal(this.helper.Cliente);
                    if (!dr.IsDBNull(iCliente)) entity.Cliente = Convert.ToString(dr.GetValue(iCliente));

                    int iBarrCodi = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = Convert.ToInt32(dr.GetValue(iBarrCodi));

                    int iBarra = dr.GetOrdinal(this.helper.Barra);
                    if (!dr.IsDBNull(iBarra)) entity.Barra = Convert.ToString(dr.GetValue(iBarra));

                    int iCodCnCodiVtp = dr.GetOrdinal(this.helper.CodCnCodiVtp);
                    if (!dr.IsDBNull(iCodCnCodiVtp)) entity.CodCnCodiVtp = Convert.ToString(dr.GetValue(iCodCnCodiVtp));

                    int iVarCodPorcentaje = dr.GetOrdinal(this.helper.VarCodPorcentaje);
                    if (!dr.IsDBNull(iVarCodPorcentaje)) entity.VarCodPorcentaje = dr.GetDecimal(iVarCodPorcentaje);

                    int iFila = dr.GetOrdinal(this.helper.Fila);
                    if (!dr.IsDBNull(iFila)) entity.Fila = Convert.ToInt32(dr.GetValue(iFila));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<VtpVariacionCodigoDTO> ListVariacionCodigoVTEAByEmprCodi(int emprcodi, int NroPagina, int PageSize, string varCodiVtp)
        {
            List<VtpVariacionCodigoDTO> entitys = new List<VtpVariacionCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListVariacionCodigoVTEAByEmprCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpVariacionCodigoDTO entity = new VtpVariacionCodigoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iClicodi = dr.GetOrdinal(this.helper.CliCodi);
                    if (!dr.IsDBNull(iClicodi)) entity.CliCodi = Convert.ToInt32(dr.GetValue(iClicodi));

                    int iCliente = dr.GetOrdinal(this.helper.Cliente);
                    if (!dr.IsDBNull(iCliente)) entity.Cliente = Convert.ToString(dr.GetValue(iCliente));

                    int iBarrCodi = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = Convert.ToInt32(dr.GetValue(iBarrCodi));

                    int iBarra = dr.GetOrdinal(this.helper.Barra);
                    if (!dr.IsDBNull(iBarra)) entity.Barra = Convert.ToString(dr.GetValue(iBarra));

                    int iCodCnCodiVtp = dr.GetOrdinal(this.helper.CodCnCodiVtp);
                    if (!dr.IsDBNull(iCodCnCodiVtp)) entity.CodCnCodiVtp = Convert.ToString(dr.GetValue(iCodCnCodiVtp));

                    int iVarCodPorcentaje = dr.GetOrdinal(this.helper.VarCodPorcentaje);
                    if (!dr.IsDBNull(iVarCodPorcentaje)) entity.VarCodPorcentaje = dr.GetDecimal(iVarCodPorcentaje);

                    int iFila = dr.GetOrdinal(this.helper.Fila);
                    if (!dr.IsDBNull(iFila)) entity.Fila = Convert.ToInt32(dr.GetValue(iFila));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetNroRecordsVariacionCodigoByEmprCodi(int emprcodi, string varCodiVtp)
        {
            int NroRegistros = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetNroRecordsVariacionCodigoByEmprCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }

        public int GetNroRecordsVariacionCodigoVTEAByEmprCodi(int emprcodi, string varCodiVtp)
        {
            int NroRegistros = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetNroRecordsVariacionCodigoVTEAByEmprCodi);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.String, emprcodi);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, varCodiVtp);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }

        public void UpdateStatusVariationByCodigoVtp(VtpVariacionCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateStatusVariationByCodigoVtp);

            dbProvider.AddInParameter(command, helper.VarCodEstado, DbType.String, entity.VarCodEstado);
            dbProvider.AddInParameter(command, helper.VarCodUsuModificacion, DbType.String, entity.VarCodUsuModificacion);
            dbProvider.AddInParameter(command, helper.VarCodFecModificacion, DbType.DateTime, entity.VarCodFecModificacion);
            dbProvider.AddInParameter(command, helper.VarCodCodigoVtp, DbType.String, entity.VarCodCodigoVtp);
            dbProvider.AddInParameter(command, helper.VarCodTipoComp, DbType.String, entity.VarCodTipoComp);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<VtpVariacionCodigoDTO> ListHistoryVariacionCodigoByCodigoVtp(string varCodCodigoVtp, string varemptipocomp)
        {
            List<VtpVariacionCodigoDTO> entitys = new List<VtpVariacionCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListHistoryVariacionCodigoByCodigoVtp);
            dbProvider.AddInParameter(command, helper.VarCodCodigoVtp, DbType.String, varCodCodigoVtp);
            dbProvider.AddInParameter(command, helper.VarCodTipoComp, DbType.String, varemptipocomp);
            dbProvider.AddInParameter(command, helper.VarCodCodigoVtp, DbType.String, varCodCodigoVtp);
            dbProvider.AddInParameter(command, helper.VarCodTipoComp, DbType.String, varemptipocomp);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpVariacionCodigoDTO entity = new VtpVariacionCodigoDTO();

                    int iVarCodCodi = dr.GetOrdinal(this.helper.VarCodCodi);
                    if (!dr.IsDBNull(iVarCodCodi)) entity.VarCodCodi = Convert.ToInt32(dr.GetValue(iVarCodCodi));

                    int iEmprcodi = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iCliente = dr.GetOrdinal(this.helper.Cliente);
                    if (!dr.IsDBNull(iCliente)) entity.Cliente = Convert.ToString(dr.GetValue(iCliente));

                    int iBarra = dr.GetOrdinal(this.helper.Barra);
                    if (!dr.IsDBNull(iBarra)) entity.Barra = Convert.ToString(dr.GetValue(iBarra));

                    int iCodCodigoVtp = dr.GetOrdinal(this.helper.VarCodCodigoVtp);
                    if (!dr.IsDBNull(iCodCodigoVtp)) entity.VarCodCodigoVtp = Convert.ToString(dr.GetValue(iCodCodigoVtp));

                    int iVarCodPorcentaje = dr.GetOrdinal(this.helper.VarCodPorcentaje);
                    if (!dr.IsDBNull(iVarCodPorcentaje)) entity.VarCodPorcentaje = dr.GetDecimal(iVarCodPorcentaje);

                    int iVarcodestado = dr.GetOrdinal(this.helper.VarCodEstado);
                    if (!dr.IsDBNull(iVarcodestado)) entity.VarCodEstado = dr.GetString(iVarcodestado);

                    int iVarcodusumodificacion = dr.GetOrdinal(this.helper.VarCodUsuModificacion);
                    if (!dr.IsDBNull(iVarcodusumodificacion)) entity.VarCodUsuModificacion = dr.GetString(iVarcodusumodificacion);

                    int iVarcodfecmodificacion = dr.GetOrdinal(this.helper.VarCodFecModificacion);
                    if (!dr.IsDBNull(iVarcodfecmodificacion)) entity.VarCodFecModificacion = dr.GetDateTime(iVarcodfecmodificacion);

                    int iVarcodfeccreacion = dr.GetOrdinal(this.helper.VarCodFecCreacion);
                    if (!dr.IsDBNull(iVarcodfeccreacion)) entity.VarCodFecCreacion = dr.GetDateTime(iVarcodfeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        

    }
}
