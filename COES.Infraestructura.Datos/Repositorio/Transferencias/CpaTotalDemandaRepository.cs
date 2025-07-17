using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_TOTAL_DEMANDA
    /// </summary>
    public class CpaTotalDemandaRepository : RepositoryBase, ICpaTotalDemandaRepository
    {
        public CpaTotalDemandaRepository(string strConn) : base(strConn)
        {
        }

        CpaTotalDemandaHelper helper = new CpaTotalDemandaHelper();

        #region Métodos Tabla CPA_TOTAL_DEMANDA
        public int Save(CpaTotalDemandaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpatdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpatdanio, DbType.Int32, entity.Cpatdanio);
            dbProvider.AddInParameter(command, helper.Cpatdajuste, DbType.String, entity.Cpatdajuste);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpatdtipo, DbType.String, entity.Cpatdtipo);
            dbProvider.AddInParameter(command, helper.Cpatdmes, DbType.Int32, entity.Cpatdmes);
            dbProvider.AddInParameter(command, helper.Cpatdusucreacion, DbType.String, entity.Cpatdusucreacion);
            dbProvider.AddInParameter(command, helper.Cpatdfeccreacion, DbType.DateTime, entity.Cpatdfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpatdusumodificacion, DbType.String, entity.Cpatdusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpatdfecmodificacion, DbType.DateTime, entity.Cpatdfecmodificacion);

            dbProvider.ExecuteNonQuery(command); 

            return id;
        }

        public void Delete(int cpaTdCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpatdcodi, DbType.Int32, cpaTdCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaTotalDemandaDTO> List()
        {
            List<CpaTotalDemandaDTO> entities = new List<CpaTotalDemandaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public CpaTotalDemandaDTO GetById(int cpaTdCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpatdcodi, DbType.Int32, cpaTdCodi);
            CpaTotalDemandaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        #endregion

        #region Métodos Adicionales
        public int ObtenerNroRegistrosEnvios()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNroRegistroEnvios);
            int NroRegistros = 0;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) NroRegistros = Convert.ToInt32(result);

            return NroRegistros;
        }

        public int ObtenerNroRegistroEnviosFiltros(int cparcodi, string cpaemptipo, int cpatdmes)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroEnviosFiltros, cparcodi, cpaemptipo, cpatdmes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int NroRegistros = 0;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) NroRegistros = Convert.ToInt32(result);

            return NroRegistros;
        }

        public List<CpaTotalDemandaDTO> ObtenerEnvios(int cparcodi, string cpaemptipo, int cpatdmes)
        {
            List<CpaTotalDemandaDTO> entitys = new List<CpaTotalDemandaDTO>();

            string query = string.Format(helper.SqlObtenerEnvios, cparcodi, cpaemptipo, cpatdmes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public string ObtenerEstadoRevisionDemanda(int cparcodi)
        {
            string estado = string.Empty;
            string query = string.Format(helper.SqlObtenerEstadoRevisionDemanda, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            estado = dbProvider.ExecuteScalar(command).ToString();

            return estado;
        }

        public int ObtenerNroRegistrosCPPEJDemanda(int cparcodi)
        {
            string query = string.Format(helper.SqlObtenerNroRegistrosCPPEJDemanda, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int NroRegistros = 0;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) NroRegistros = Convert.ToInt32(result);

            return NroRegistros;
        }

        public void DeleteCPPEJDemanda(int cparcodi)
        {
            string query = string.Format(helper.SqlDeleteCPPEJDemanda, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public string ObtenerTipoEmpresaCPAPorNombre(int cparcodi, string cpaemptipo, string emprNom)
        {
            string query = string.Format(helper.SqlObtenerTipoEmpresaCPAPorNombre, cparcodi, cpaemptipo, emprNom);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object tipo = dbProvider.ExecuteScalar(command);
            if (tipo == null) tipo = "";

            return tipo.ToString();
        }
        #endregion
    }
}
