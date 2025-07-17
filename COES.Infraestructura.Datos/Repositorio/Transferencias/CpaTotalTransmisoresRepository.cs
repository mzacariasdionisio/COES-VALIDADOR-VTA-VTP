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
    /// Clase de acceso a datos de la tabla CPA_TOTAL_TRANSMISORES
    /// </summary>
    public class CpaTotalTransmisoresRepository : RepositoryBase, ICpaTotalTransmisoresRepository
    {
        public CpaTotalTransmisoresRepository(string strConn) : base(strConn)
        {
        }

        CpaTotalTransmisoresHelper helper = new CpaTotalTransmisoresHelper();

        #region Métodos Tabla CPA_TOTAL_TRANSMISORES
        public int Save(CpaTotalTransmisoresDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpattcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpattanio, DbType.Int32, entity.Cpattanio);
            dbProvider.AddInParameter(command, helper.Cpattajuste, DbType.String, entity.Cpattajuste);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpattusucreacion, DbType.String, entity.Cpattusucreacion);
            dbProvider.AddInParameter(command, helper.Cpattfeccreacion, DbType.DateTime, entity.Cpattfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpattusumodificacion, DbType.String, entity.Cpattusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpattfecmodificacion, DbType.DateTime, entity.Cpattfecmodificacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Delete(int cpaTtCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpattcodi, DbType.Int32, cpaTtCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaTotalTransmisoresDTO> List()
        {
            List<CpaTotalTransmisoresDTO> entities = new List<CpaTotalTransmisoresDTO>();
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

        public CpaTotalTransmisoresDTO GetById(int cpaTtCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpattcodi, DbType.Int32, cpaTtCodi);
            CpaTotalTransmisoresDTO entity = null;

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

        public int ObtenerNroRegistroEnviosFiltros(int cparcodi)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroEnviosFiltros, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int NroRegistros = 0;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) NroRegistros = Convert.ToInt32(result);

            return NroRegistros;
        }

        public List<CpaTotalTransmisoresDTO> ObtenerEnvios(int cparcodi)
        {
            List<CpaTotalTransmisoresDTO> entitys = new List<CpaTotalTransmisoresDTO>();

            string query = string.Format(helper.SqlObtenerEnvios, cparcodi);
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

        public string ObtenerEstadoRevisionTransmisores(int cparcodi)
        {
            string estado = string.Empty;
            string query = string.Format(helper.SqlObtenerEstadoRevisionTransmisores, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            estado = dbProvider.ExecuteScalar(command).ToString();

            return estado;
        }

        public int ObtenerNroRegistrosCPPEJTransmisores(int cparcodi)
        {
            string query = string.Format(helper.SqlObtenerNroRegistrosCPPEJTransmisores, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int NroRegistros = 0;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) NroRegistros = Convert.ToInt32(result);

            return NroRegistros;
        }

        public void DeleteCPPEJTransmisores(int cparcodi)
        {
            string query = string.Format(helper.SqlDeleteCPPEJTransmisores, cparcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public string ObtenerTipoEmpresaCPAPorNombre(int cparcodi, string emprNom)
        {
            string query = string.Format(helper.SqlObtenerTipoEmpresaCPAPorNombre, cparcodi, emprNom);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object tipo = dbProvider.ExecuteScalar(command);
            if (tipo == null) tipo = "";

            return tipo.ToString();
        }
        #endregion
    }
}