using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class ActualizarTrasEmpFusionRepository : RepositoryBase, IActualizarTrasEmpFusionRepository
    {
        public ActualizarTrasEmpFusionRepository(string strConn) : base(strConn)
        {
        }

        ActualizarTrasEmpFusionHelper helper = new ActualizarTrasEmpFusionHelper();

        public List<ActualizarTrasEmpFusionDTO> GetListaSaldosSobrantes(int? pericodi)
        {
            List<ActualizarTrasEmpFusionDTO> entitys = new List<ActualizarTrasEmpFusionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaSaldosSobrantes);
            dbProvider.AddInParameter(command, helper.PeriCodiDes, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<ActualizarTrasEmpFusionDTO> GetListaSaldosSobrantesVTP(int? pericodi)
        {
            List<ActualizarTrasEmpFusionDTO> entitys = new List<ActualizarTrasEmpFusionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaSaldosSobrantesVTP);
            dbProvider.AddInParameter(command, helper.PeriCodiDes, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public ActualizarTrasEmpFusionDTO SaveUpdate(ActualizarTrasEmpFusionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveUpdate);

            dbProvider.AddInParameter(command, helper.SalsoVTEAVTP, DbType.String, entity.SalsoVTEAVTP);
            dbProvider.AddInParameter(command, helper.PeriCodiDes, DbType.Int32, entity.PeriCodiDes);
            dbProvider.AddInParameter(command, helper.PeriCodiOri, DbType.Int32, entity.PeriCodiOri);
            dbProvider.AddInParameter(command, helper.RecaCodi, DbType.Int32, entity.RecaCodi);
            dbProvider.AddInParameter(command, helper.EmprCodiOri, DbType.Int32, entity.EmprCodiOri);
            dbProvider.AddInParameter(command, helper.SalsoSaldoOri, DbType.Decimal, entity.SalsoSaldoOri);
            dbProvider.AddInParameter(command, helper.SalsoTipOpe, DbType.Int32, entity.SalsoTipOpe);
            dbProvider.AddInParameter(command, helper.SalsoFecMigracion, DbType.DateTime, entity.SalsoFecMigracion);
            dbProvider.AddInParameter(command, helper.EmprCodiDes, DbType.Int32, entity.EmprCodiDes);
            dbProvider.AddInParameter(command, helper.SalsoSaldoDes, DbType.Decimal, entity.SalsoSaldoDes);
            dbProvider.AddInParameter(command, helper.SalsoUsuCreacion, DbType.String, entity.SalsoUsuCreacion);
            dbProvider.AddInParameter(command, helper.SalsoUsuModificacion, DbType.String, entity.SalsoUsuModificacion);

            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }
        public ActualizarTrasEmpFusionDTO DeleteSaldosSobrantes()
        {
            ActualizarTrasEmpFusionDTO entity = new ActualizarTrasEmpFusionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteSaldosSobrantes);
            dbProvider.AddInParameter(command, helper.SalsoEstado, DbType.String, "P");

            //dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = "OK";
            return entity;
        }

        public List<ActualizarTrasEmpFusionDTO> GetLista(int? pericodi, string salsovteavtp)
        {
            List<ActualizarTrasEmpFusionDTO> entitys = new List<ActualizarTrasEmpFusionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlLista);
            dbProvider.AddInParameter(command, helper.PeriCodiDes, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.SalsoVTEAVTP, DbType.Int32, salsovteavtp);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<ActualizarTrasEmpFusionDTO> GetListaVTP(int? pericodi, string salsovteavtp)
        {
            List<ActualizarTrasEmpFusionDTO> entitys = new List<ActualizarTrasEmpFusionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaVTP);
            dbProvider.AddInParameter(command, helper.PeriCodiDes, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.SalsoVTEAVTP, DbType.Int32, salsovteavtp);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }
        public ActualizarTrasEmpFusionDTO SaveUpdateSaldos(ActualizarTrasEmpFusionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveUpdateSaldos);

            dbProvider.AddInParameter(command, helper.SalsoCodi, DbType.Int32, entity.SalsoCodi);
            dbProvider.AddInParameter(command, helper.SalsoUsuModificacion, DbType.String, entity.SalsoUsuModificacion);

            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }

        public ActualizarTrasEmpFusionDTO SaveUpdateSaldosVTP(ActualizarTrasEmpFusionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveUpdateSaldosVTP);

            dbProvider.AddInParameter(command, helper.SalsoCodi, DbType.Int32, entity.SalsoCodi);
            dbProvider.AddInParameter(command, helper.SalsoUsuModificacion, DbType.String, entity.SalsoUsuModificacion);

            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }
        public List<ActualizarTrasEmpFusionDTO> GetListaSaldosNoIdentificados(int? pericodi)
        {
            List<ActualizarTrasEmpFusionDTO> entitys = new List<ActualizarTrasEmpFusionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaSaldosNoIdentificados);
            dbProvider.AddInParameter(command, helper.PeriCodiOri, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<ActualizarTrasEmpFusionDTO> GetListaSaldosNoIdentificadosVTP(int? pericodi)
        {
            List<ActualizarTrasEmpFusionDTO> entitys = new List<ActualizarTrasEmpFusionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaSaldosNoIdentificadosVTP);
            dbProvider.AddInParameter(command, helper.PeriCodiOri, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

    }
}
