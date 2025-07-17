using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PR_CVARIABLES
    /// </summary>
    public class PrCvariablesRepository: RepositoryBase, IPrCvariablesRepository
    {
        public PrCvariablesRepository(string strConn): base(strConn)
        {
        }
        PrCvariablesHelper helper = new PrCvariablesHelper();
        public void Update(PrCvariablesDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Cvc, DbType.Decimal, entity.Cvc);
            dbProvider.AddInParameter(command, helper.Cvnc, DbType.Decimal, entity.Cvnc);
            dbProvider.AddInParameter(command, helper.Fpmin, DbType.Decimal, entity.Fpmin);
            dbProvider.AddInParameter(command, helper.Fpmed, DbType.Decimal, entity.Fpmed);
            dbProvider.AddInParameter(command, helper.Fpmax, DbType.Decimal, entity.Fpmax);
            dbProvider.AddInParameter(command, helper.Ccomb, DbType.Decimal, entity.Ccomb);
            dbProvider.AddInParameter(command, helper.Pe, DbType.Decimal, entity.Pe);
            dbProvider.AddInParameter(command, helper.Eficbtukwh, DbType.Decimal, entity.Eficbtukwh);
            dbProvider.AddInParameter(command, helper.Eficterm, DbType.Decimal, entity.Eficterm);
            dbProvider.AddInParameter(command, helper.Escecodi, DbType.Int32, entity.Escecodi);
            dbProvider.AddInParameter(command, helper.CecSi, DbType.Decimal, entity.CecSi);
            dbProvider.AddInParameter(command, helper.RendSi, DbType.Decimal, entity.RendSi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public PrCvariablesDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            PrCvariablesDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        
        public List<PrCvariablesDTO> ListPrCvariabless(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, id);
            
            List<PrCvariablesDTO> entitys = new List<PrCvariablesDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<PrCvariablesDTO> GetByCriteria()
        {
            List<PrCvariablesDTO> entitys = new List<PrCvariablesDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public List<PrCvariablesDTO> GetCostosVariablesPorRepCv(int repcvCodi)
        {
            var resultado = new List<PrCvariablesDTO>();
            var sComando = string.Format(helper.SqlCostosVariablesPorRepCV, repcvCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oDatoConc = new PrCvariablesDTO();
                    if (!dr.IsDBNull(dr.GetOrdinal("EMPRCODI"))) oDatoConc.Emprcodi = dr.GetInt32(dr.GetOrdinal("EMPRCODI"));
                    if (!dr.IsDBNull(dr.GetOrdinal("EMPRNOMB"))) oDatoConc.Emprnomb = dr.GetString(dr.GetOrdinal("EMPRNOMB"));
                    if (!dr.IsDBNull(dr.GetOrdinal("GRUPOCODI"))) oDatoConc.Grupocodi = dr.GetInt32(dr.GetOrdinal("GRUPOCODI"));
                    oDatoConc.Grupotipo = dr.IsDBNull(dr.GetOrdinal("GRUPOTIPO")) ? "" : dr.GetString(dr.GetOrdinal("GRUPOTIPO"));
                    oDatoConc.Gruponomb = dr.IsDBNull(dr.GetOrdinal("GRUPONOMB")) ? "" : dr.GetString(dr.GetOrdinal("GRUPONOMB"));
                    oDatoConc.Grupoabrev = dr.IsDBNull(dr.GetOrdinal("GRUPOABREV")) ? "" : dr.GetString(dr.GetOrdinal("GRUPOABREV"));
                    oDatoConc.Escenomb = dr.IsDBNull(dr.GetOrdinal("ESCENOMB")) ? "" : dr.GetString(dr.GetOrdinal("ESCENOMB"));
                    oDatoConc.CecSi = dr.IsDBNull(dr.GetOrdinal("CEC_SI")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CEC_SI"));
                    oDatoConc.RendSi = dr.IsDBNull(dr.GetOrdinal("REND_SI")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("REND_SI"));
                    oDatoConc.Pe = dr.IsDBNull(dr.GetOrdinal("PE")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("PE"));
                    oDatoConc.Eficbtukwh = dr.IsDBNull(dr.GetOrdinal("EFICBTUKWH")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("EFICBTUKWH"));
                    oDatoConc.Eficterm = dr.IsDBNull(dr.GetOrdinal("EFICTERM")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("EFICTERM"));
                    oDatoConc.Ccomb = dr.IsDBNull(dr.GetOrdinal("CCOMB")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CCOMB"));
                    oDatoConc.Cvc = dr.IsDBNull(dr.GetOrdinal("CVC")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVC"));
                    oDatoConc.Cvnc = dr.IsDBNull(dr.GetOrdinal("CVNC")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVNC"));
                    oDatoConc.Fpmax = dr.IsDBNull(dr.GetOrdinal("FPMAX")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("FPMAX"));
                    oDatoConc.Fpmed = dr.IsDBNull(dr.GetOrdinal("FPMED")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("FPMED"));
                    oDatoConc.Fpmin = dr.IsDBNull(dr.GetOrdinal("FPMIN")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("FPMIN"));

                    oDatoConc.TipoGenerRer = dr.IsDBNull(dr.GetOrdinal("TIPOGENERRER")) ? "" : dr.GetString(dr.GetOrdinal("TIPOGENERRER"));
                    oDatoConc.Grupotipocogen = dr.IsDBNull(dr.GetOrdinal("GRUPOTIPOCOGEN")) ? "" : dr.GetString(dr.GetOrdinal("GRUPOTIPOCOGEN"));

                    /*6 campos nuevos*/
                    if (!dr.IsDBNull(dr.GetOrdinal("CVARTRAMO1"))) oDatoConc.Tramo1 = dr.GetString(dr.GetOrdinal("CVARTRAMO1"));
                    oDatoConc.CIncremental1 = dr.IsDBNull(dr.GetOrdinal("CVARCINCREM1")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVARCINCREM1"));

                    if (!dr.IsDBNull(dr.GetOrdinal("CVARTRAMO2"))) oDatoConc.Tramo2 = dr.GetString(dr.GetOrdinal("CVARTRAMO2"));
                    oDatoConc.CIncremental2 = dr.IsDBNull(dr.GetOrdinal("CVARCINCREM2")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVARCINCREM2"));

                    if (!dr.IsDBNull(dr.GetOrdinal("CVARTRAMO3"))) oDatoConc.Tramo3 = dr.GetString(dr.GetOrdinal("CVARTRAMO3"));
                    oDatoConc.CIncremental3 = dr.IsDBNull(dr.GetOrdinal("CVARCINCREM3")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVARCINCREM3"));

                    oDatoConc.Pe1 = dr.IsDBNull(dr.GetOrdinal("CVARPE1")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVARPE1"));
                    oDatoConc.Pe2 = dr.IsDBNull(dr.GetOrdinal("CVARPE2")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVARPE2"));
                    oDatoConc.Pe3 = dr.IsDBNull(dr.GetOrdinal("CVARPE3")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVARPE3"));
                    oDatoConc.Pe4 = dr.IsDBNull(dr.GetOrdinal("CVARPE4")) ? (Decimal?)null : dr.GetDecimal(dr.GetOrdinal("CVARPE4"));

                    oDatoConc.TipoCombustible = dr.IsDBNull(dr.GetOrdinal("FENERGNOMB")) ? "" : dr.GetString(dr.GetOrdinal("FENERGNOMB"));

                    resultado.Add(oDatoConc);
                }
            }
            return resultado;
        }
        
        public void EliminarCostosVariablesPorRepCv(int repcvCodi)
        {
            string strComando = String.Format(helper.SqlEliminarCostosVariablesPorRepCv, repcvCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
            dbProvider.ExecuteNonQuery(command);
        }

        public void EjecutarComandoCv(string strComando)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
            dbProvider.ExecuteNonQuery(command);
        }

        #region MonitoreoMME

        public List<PrCvariablesDTO> ListCostoVariablesxRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<PrCvariablesDTO> entitys = new List<PrCvariablesDTO>();

            string query = string.Format(this.helper.SqlListCostoVariablesxRangoFecha, fechaInicio.ToString(ConstantesBase.FormatoFechaPE), fechaFin.ToString(ConstantesBase.FormatoFechaPE));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrCvariablesDTO entity = helper.Create(dr);

                    int EmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(EmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(EmprCodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iRepfecha = dr.GetOrdinal(helper.Repfecha);
                    if (!dr.IsDBNull(iRepfecha)) entity.Repfecha = dr.GetDateTime(iRepfecha);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region SIOSEIN
        public List<PrCvariablesDTO> ObtenerCVariablePorRepcodiYCatecodi(string repcodi, string catecodi, string fenergcodi)
        {
            List<PrCvariablesDTO> entitys = new List<PrCvariablesDTO>();
            var query = string.Format(helper.SqlObtenerCVariablePorRepcodiYCatecodi, repcodi, catecodi, fenergcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCv = dr.GetOrdinal(helper.Cv);
                    if (!dr.IsDBNull(iCv)) entity.Cv = dr.GetDecimal(iCv);

                    int iRepfecha = dr.GetOrdinal(helper.Repfecha);
                    if (!dr.IsDBNull(iRepfecha)) entity.Repfecha = dr.GetDateTime(iRepfecha);
                    int iReptipo = dr.GetOrdinal(helper.Reptipo);
                    if (!dr.IsDBNull(iReptipo)) entity.Reptipo = dr.GetString(iReptipo);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iOsinergcodiFe = dr.GetOrdinal(helper.OsinergcodiFe);
                    if (!dr.IsDBNull(iOsinergcodiFe)) entity.OsinergcodiFe = dr.GetString(iOsinergcodiFe);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion
    }
}
