using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTD_VALORIZACIONDETALLE
    /// </summary>
    public class VtdValorizacionDetalleRepository : RepositoryBase, IVtdValorizacionDetalleRepository
    {
        public VtdValorizacionDetalleRepository(string strConn) : base(strConn)
        {
        }

        VtdValorizacionDetalleHelper helper = new VtdValorizacionDetalleHelper();
        MeMedicion48Helper helperM48 = new MeMedicion48Helper();
        MeMedicion96Helper helperM96 = new MeMedicion96Helper();
        VtpIngresoPotefrDetalleHelper helperIPD = new VtpIngresoPotefrDetalleHelper();
        VtpPeajeIngresoHelper helperPI = new VtpPeajeIngresoHelper();

        public int Save(VtdValorizacionDetalleDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandUp = (DbCommand)conn.CreateCommand();
            commandUp.CommandText = helper.SqlSave;
            commandUp.Transaction = tran;
            commandUp.Connection = (DbConnection)conn;

            IDbDataParameter param = null;

            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdcodi; param.Value = entity.Valdcodi; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valocodi; param.Value = entity.Valocodi; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Emprcodi; param.Value = entity.Emprcodi; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdretiro; param.Value = entity.Valdretiro; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdentrega; param.Value = entity.Valdentrega; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdpfirremun; param.Value = entity.Valdpfirremun; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valddemandacoincidente; param.Value = entity.Valddemandacoincidente; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdmoncapacidad; param.Value = entity.Valdmoncapacidad; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdpeajeuni; param.Value = entity.Valdpeajeuni; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdfactorp; param.Value = entity.Valdfactorp; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdpagoio; param.Value = entity.Valdpagoio; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdpagosc; param.Value = entity.Valdpagosc; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdfpgm; param.Value = entity.Valdfpgm; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdmcio; param.Value = entity.Valdmcio; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdpdsc; param.Value = entity.Valdpdsc; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdcargoconsumo; param.Value = entity.Valdcargoconsumo; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdportesadicional; param.Value = entity.Valdaportesadicional; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdusucreacion; param.Value = entity.Valdusucreacion; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdfeccreacion; param.Value = entity.Valdfeccreacion; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdusumodificacion; param.Value = entity.Valdusumodificacion; commandUp.Parameters.Add(param);
            param = commandUp.CreateParameter(); param.ParameterName = helper.Valdfecmodificacion; param.Value = entity.Valdfecmodificacion; commandUp.Parameters.Add(param);

            try
            {
                commandUp.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return -1;
            }

            return entity.Valdcodi;
        }

        public void Update(VtdValorizacionDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Valdcodi, DbType.Decimal, entity.Valdcodi);
            dbProvider.AddInParameter(command, helper.Valocodi, DbType.Decimal, entity.Valocodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Decimal, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Valdretiro, DbType.Decimal, entity.Valdretiro);
            dbProvider.AddInParameter(command, helper.Valdentrega, DbType.Decimal, entity.Valdentrega);
            dbProvider.AddInParameter(command, helper.Valdpfirremun, DbType.Decimal, entity.Valdpfirremun);
            dbProvider.AddInParameter(command, helper.Valddemandacoincidente, DbType.Decimal, entity.Valddemandacoincidente);
            dbProvider.AddInParameter(command, helper.Valdmoncapacidad, DbType.Decimal, entity.Valdmoncapacidad);
            dbProvider.AddInParameter(command, helper.Valdpeajeuni, DbType.Decimal, entity.Valdpeajeuni);
            dbProvider.AddInParameter(command, helper.Valdfactorp, DbType.Decimal, entity.Valdfactorp);
            dbProvider.AddInParameter(command, helper.Valdpagoio, DbType.Decimal, entity.Valdpagoio);
            dbProvider.AddInParameter(command, helper.Valdpagosc, DbType.Decimal, entity.Valdpagosc);
            dbProvider.AddInParameter(command, helper.Valdfpgm, DbType.Decimal, entity.Valdfpgm);
            dbProvider.AddInParameter(command, helper.Valdmcio, DbType.Decimal, entity.Valdmcio);
            dbProvider.AddInParameter(command, helper.Valdpdsc, DbType.Decimal, entity.Valdpdsc);
            dbProvider.AddInParameter(command, helper.Valdcargoconsumo, DbType.Decimal, entity.Valdcargoconsumo);
            dbProvider.AddInParameter(command, helper.Valdportesadicional, DbType.Decimal, entity.Valdaportesadicional);
            dbProvider.AddInParameter(command, helper.Valdusucreacion, DbType.Decimal, entity.Valdusucreacion);
            dbProvider.AddInParameter(command, helper.Valocodi, DbType.String, entity.Valocodi);
            dbProvider.AddInParameter(command, helper.Valdfeccreacion, DbType.DateTime, entity.Valdfeccreacion);
            dbProvider.AddInParameter(command, helper.Valdusumodificacion, DbType.String, entity.Valdusumodificacion);
            dbProvider.AddInParameter(command, helper.Valdfecmodificacion, DbType.DateTime, entity.Valdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Valdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Valdcodi, DbType.Decimal, Valdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtdValorizacionDetalleDTO GetById(int Valdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Valdcodi, DbType.Int32, Valdcodi);
            VtdValorizacionDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtdValorizacionDetalleDTO> List()
        {
            List<VtdValorizacionDetalleDTO> entitys = new List<VtdValorizacionDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VtdValorizacionDetalleDTO> GetByCriteria()
        {
            List<VtdValorizacionDetalleDTO> entitys = new List<VtdValorizacionDetalleDTO>();
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

        public List<VtdValorizacionDetalleDTO> ObtenerEntregaParticipante(DateTime date)
        {
            List<VtdValorizacionDetalleDTO> entitys = new List<VtdValorizacionDetalleDTO>();
            string query = helperM48.SqlGetEmpresaEnergiaEntregada;
            query = string.Format(query.Replace("@date", date.ToString("yyyy/MM/dd")), date.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdValorizacionDetalleDTO entity = new VtdValorizacionDetalleDTO();

                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iValdentrega = dr.GetOrdinal("VALDENTREGA");
                    if (!dr.IsDBNull(iValdentrega)) entity.Valdentrega = Convert.ToDecimal(dr.GetValue(iValdentrega));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtdValorizacionDetalleDTO> ObtenerEnergiaEntregadaByEmpresas(DateTime date, string particpantes)
        {
            List<VtdValorizacionDetalleDTO> entitys = new List<VtdValorizacionDetalleDTO>();
            string query = helperM48.SqlGetEnergiaEntregadabyEmpresas;
            query = string.Format(query.Replace("@date", date.ToString("yyyy/MM/dd")), date.ToString(ConstantesBase.FormatoFecha), particpantes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdValorizacionDetalleDTO entity = new VtdValorizacionDetalleDTO();

                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iValdentrega = dr.GetOrdinal("VALDENTREGA");
                    if (!dr.IsDBNull(iValdentrega)) entity.Valdentrega = Convert.ToDecimal(dr.GetValue(iValdentrega));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtdValorizacionDetalleDTO> ObtenerEnergiaPrevista(DateTime date)
        {
            List<VtdValorizacionDetalleDTO> entitys = new List<VtdValorizacionDetalleDTO>();
            string query = helperM96.SqlGetEmpresaEnergiaPrevista;
            query = query.Replace("@date", date.ToString("yyyy/MM/dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdValorizacionDetalleDTO entity = new VtdValorizacionDetalleDTO();
                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iValdretiro = dr.GetOrdinal("VALDRETIRO");
                    if (!dr.IsDBNull(iValdretiro)) entity.Valdretiro = Convert.ToDecimal(dr.GetValue(iValdretiro));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public decimal ObtenerEnergiaPrevistaRetirar(DateTime date, int emprcodi, int tpEPR)
        {
            string sCommand = string.Format(helperM96.SqlGetEnergiaPrevistaRetirar, date.ToString(ConstantesBase.FormatoFecha), emprcodi, tpEPR);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);
            decimal EnergiaPR = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iValdretiro = dr.GetOrdinal("SUMAEPR");
                    if (!dr.IsDBNull(iValdretiro)) EnergiaPR = Convert.ToDecimal(dr.GetValue(iValdretiro));

                }
                else
                {
                    EnergiaPR = 0;
                }
            }

            return EnergiaPR;
        }

        public decimal ObtenerEnergiaPrevistaRetirarTotal(DateTime date, int tpEPR)
        {
            string sCommand = string.Format(helper.SqlGetEnergiaPrevistaRetirarTotal, date.ToString(ConstantesBase.FormatoFecha), tpEPR);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);
            decimal EnergiaPR = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iValdretiro = dr.GetOrdinal("SUMAEPR");
                    if (!dr.IsDBNull(iValdretiro)) EnergiaPR = Convert.ToDecimal(dr.GetValue(iValdretiro));

                }
                else
                {
                    EnergiaPR = 0;
                }
            }

            return EnergiaPR;
        }

        public List<VtdValorizacionDetalleDTO> ObtenerPotenciaFirmeRemunerable(int pericodi)
        {
            List<VtdValorizacionDetalleDTO> entities = new List<VtdValorizacionDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helperIPD.SqlGetPotenciaFirmeRemunerable);

            dbProvider.AddInParameter(command, "pericodi", DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdValorizacionDetalleDTO entity = new VtdValorizacionDetalleDTO();
                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iValdpfirremun = dr.GetOrdinal("VALDPFIRREMUN");
                    if (!dr.IsDBNull(iValdpfirremun)) entity.Valdpfirremun = Convert.ToDecimal(dr.GetValue(iValdpfirremun));


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<VtdValorizacionDetalleDTO> ObtenerDemandaCoincidente(DateTime fechaIntervalo, int hora)
        {
            List<VtdValorizacionDetalleDTO> entities = new List<VtdValorizacionDetalleDTO>();
            string query = helperM96.SqlGetDemandaCoincidente;
            query = query.Replace("@h", hora.ToString());
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(query, fechaIntervalo.ToString(ConstantesBase.FormatoFecha)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdValorizacionDetalleDTO entity = new VtdValorizacionDetalleDTO();
                    int iEmprcodi = dr.GetOrdinal("EMPRCODI");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iValddemandacoincidente = dr.GetOrdinal("VALDDEMANDACOINCIDENTE");
                    if (!dr.IsDBNull(iValddemandacoincidente)) entity.Valddemandacoincidente = Convert.ToDecimal(dr.GetValue(iValddemandacoincidente));

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public decimal ObtenerPrecioPeaje(int pericodi)
        {
            decimal precioPeaje = 0;

            DbCommand command = dbProvider.GetSqlStringCommand(helperPI.SqlGetPeajeUnitario);

            dbProvider.AddInParameter(command, "pericodi", DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    VtdValorizacionDetalleDTO entity = new VtdValorizacionDetalleDTO();
                    

                    int iValdpeajeuni = dr.GetOrdinal("VALDPEAJEUNI");
                    if (!dr.IsDBNull(iValdpeajeuni)) precioPeaje = Convert.ToDecimal(dr.GetValue(iValdpeajeuni));
                }
            }

            return precioPeaje;
        }

        #region Monto por exceso de consumo de energía reactiva

        public VtdValorizacionDetalleDTO GetValorizacionDetalleporFechaParticipante(DateTime date, int participante)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetValorizacionDetalleporFechaParticipante);

            dbProvider.AddInParameter(command, "emprcodi", DbType.Int32, participante);
            dbProvider.AddInParameter(command, "valofecha", DbType.DateTime, date);

            VtdValorizacionDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        #endregion
    }
}
