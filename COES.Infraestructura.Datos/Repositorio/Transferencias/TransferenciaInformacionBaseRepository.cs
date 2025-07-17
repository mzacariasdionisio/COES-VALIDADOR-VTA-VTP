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
    public class TransferenciaInformacionBaseRepository : RepositoryBase, ITransferenciaInformacionBaseRepository
    {
        public TransferenciaInformacionBaseRepository(string strConn)
            : base(strConn)
        {
        }

        TransferenciaInformacionBaseHelper helper = new TransferenciaInformacionBaseHelper();
        public int Save(TransferenciaInformacionBaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            int iTEntCodi = GetCodigoGenerado();

            dbProvider.AddInParameter(command, helper.TINFBCODI, DbType.Int32, iTEntCodi);

            dbProvider.AddInParameter(command, helper.COINFBCODI, DbType.Int32, entity.CoInfbCodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, entity.EquiCodi);
            dbProvider.AddInParameter(command, helper.TINFBCODIGO, DbType.String, entity.TinfbCodigo);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, entity.TinfbVersion);
            dbProvider.AddInParameter(command, helper.TINFBTIPOINFORMACION, DbType.String, entity.TinfbTipoInformacion);
            dbProvider.AddInParameter(command, helper.TINFBESTADO, DbType.String, entity.TinfbEstado);
            dbProvider.AddInParameter(command, helper.TINFBUSERNAME, DbType.String, entity.TinfbUserName);
            dbProvider.AddInParameter(command, helper.TINFBFECINS, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.TRNENVCODI, DbType.Int32, entity.TrnEnvCodi);
            dbProvider.ExecuteNonQuery(command);
            return iTEntCodi;
        }

        public void Update(TransferenciaInformacionBaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.TINFBESTADO, DbType.String, entity.TinfbEstado);
            dbProvider.AddInParameter(command, helper.TINFBFECACT, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, entity.TinfbVersion);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EmprCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateCodigo(TransferenciaInformacionBaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCodigo);
            dbProvider.AddInParameter(command, helper.TINFBESTADO, DbType.String, entity.TinfbEstado);
            dbProvider.AddInParameter(command, helper.TINFBFECACT, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, entity.TinfbVersion);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.COINFBCODI, DbType.Int32, entity.CoInfbCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pericodi, int version, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TINFBCODIGO, DbType.String, sCodigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteListaTransferenciaInfoBase(int pericodi, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTransferenciaEntrega);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, version);

            dbProvider.ExecuteNonQuery(command);
        }

        public TransferenciaInformacionBaseDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.TINFBCODI, DbType.Int32, id);
            TransferenciaInformacionBaseDTO entity = null;

            return entity;
        }

        public List<TransferenciaInformacionBaseDTO> List(int emprcodi, int pericodi, int version)
        {
            List<TransferenciaInformacionBaseDTO> entitys = new List<TransferenciaInformacionBaseDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaInformacionBaseDTO entity = new TransferenciaInformacionBaseDTO();

                    int iTINFBCODIGO = dr.GetOrdinal(helper.TINFBCODIGO);
                    if (!dr.IsDBNull(iTINFBCODIGO)) entity.TinfbCodigo = dr.GetString(iTINFBCODIGO);

                    int iEMPRNOMBRE = dr.GetOrdinal(helper.EMPRNOMBRE);
                    if (!dr.IsDBNull(iEMPRNOMBRE)) entity.EmprNombre = dr.GetString(iEMPRNOMBRE);

                    int iCENTGENENOMBRE = dr.GetOrdinal(helper.CENTGENENOMBRE);
                    if (!dr.IsDBNull(iCENTGENENOMBRE)) entity.CentGeneNombre = dr.GetString(iCENTGENENOMBRE);

                    int iBARRNOMBRE = dr.GetOrdinal(helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombre = dr.GetString(iBARRNOMBRE);

                    int iTINFBTIPOINFORMACION = dr.GetOrdinal(helper.TINFBTIPOINFORMACION);
                    if (!dr.IsDBNull(iTINFBTIPOINFORMACION)) entity.TinfbTipoInformacion = dr.GetString(iTINFBTIPOINFORMACION);

                    int iTOTAL = dr.GetOrdinal(helper.TOTAL);
                    if (!dr.IsDBNull(iTOTAL)) entity.Total = dr.GetDecimal(iTOTAL);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public TransferenciaInformacionBaseDTO GetTransferenciaInfoBaseByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTransferenciaInfoBaseByCodigo);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, iTEntVersion);
            dbProvider.AddInParameter(command, helper.TINFBCODIGO, DbType.String, sCodigo);

            TransferenciaInformacionBaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        //ASSETEC 202001
        public TransferenciaInformacionBaseDTO GetTransferenciaInfoBaseByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetTransferenciaInfoBaseByCodigoEnvio);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, iTEntVersion);
            dbProvider.AddInParameter(command, helper.TRNENVCODI, DbType.Int32, trnenvcodi);
            dbProvider.AddInParameter(command, helper.TINFBCODIGO, DbType.String, sCodigo);

            TransferenciaInformacionBaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TransferenciaInformacionBaseDTO> ListByPeriodoVersion(int iPericodi, int iVersion)
        {
             List<TransferenciaInformacionBaseDTO> entitys = new List<TransferenciaInformacionBaseDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersion);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.TINFBVERSION, DbType.Int32, iVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

    }
}
