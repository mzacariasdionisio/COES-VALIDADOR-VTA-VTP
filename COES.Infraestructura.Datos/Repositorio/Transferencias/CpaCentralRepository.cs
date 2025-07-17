using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CPA_CENTRAL
    /// </summary>
    public class CpaCentralRepository : RepositoryBase, ICpaCentralRepository
    {
        public CpaCentralRepository(string strConn)
            : base(strConn)
        {
        }

        CpaCentralHelper helper = new CpaCentralHelper();

        public int Save(CpaCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            string query = string.Format(helper.SqlGetMaxIdCentral, entity.Cpaempcodi, entity.Cparcodi, entity.Equicodi);
            command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteScalar(command);
            int correlativo = 1;
            if (result != null) correlativo = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, entity.Cpaempcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, (object)entity.Barrcodi ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, (object)entity.Ptomedicodi ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntestado, DbType.String, entity.Cpacntestado);
            dbProvider.AddInParameter(command, helper.Cpacnttipo, DbType.String, entity.Cpacnttipo);
            dbProvider.AddInParameter(command, helper.Cpacntcorrelativo, DbType.Int32, correlativo);
            dbProvider.AddInParameter(command, helper.Cpacntfecejecinicio, DbType.DateTime, entity.Cpacntfecejecinicio);
            dbProvider.AddInParameter(command, helper.Cpacntfecejecfin, DbType.DateTime, entity.Cpacntfecejecfin);
            dbProvider.AddInParameter(command, helper.Cpacntfecproginicio, DbType.DateTime, (object)entity.Cpacntfecproginicio ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntfecprogfin, DbType.DateTime, (object)entity.Cpacntfecprogfin ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntusucreacion, DbType.String, entity.Cpacntusucreacion);
            dbProvider.AddInParameter(command, helper.Cpacntfeccreacion, DbType.DateTime, entity.Cpacntfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpacntusumodificacion, DbType.String, (object)entity.Cpacntusumodificacion ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntfecmodificacion, DbType.DateTime, (object)entity.Cpacntfecmodificacion ?? DBNull.Value);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpaCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, entity.Cpacntcodi);
            dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, entity.Cpaempcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, (object)entity.Barrcodi ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, (object)entity.Ptomedicodi ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntestado, DbType.String, entity.Cpacntestado);
            dbProvider.AddInParameter(command, helper.Cpacnttipo, DbType.String, entity.Cpacnttipo);
            dbProvider.AddInParameter(command, helper.Cpacntfecejecinicio, DbType.DateTime, entity.Cpacntfecejecinicio);
            dbProvider.AddInParameter(command, helper.Cpacntfecejecfin, DbType.DateTime, entity.Cpacntfecejecfin);
            dbProvider.AddInParameter(command, helper.Cpacntfecproginicio, DbType.DateTime, (object)entity.Cpacntfecproginicio ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntfecprogfin, DbType.DateTime, (object)entity.Cpacntfecprogfin ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntusucreacion, DbType.String, entity.Cpacntusucreacion);
            dbProvider.AddInParameter(command, helper.Cpacntfeccreacion, DbType.DateTime, entity.Cpacntfeccreacion);
            dbProvider.AddInParameter(command, helper.Cpacntusumodificacion, DbType.String, (object)entity.Cpacntusumodificacion ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntfecmodificacion, DbType.DateTime, (object)entity.Cpacntfecmodificacion ?? DBNull.Value);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cpaCntcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, cpaCntcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpaCentralDTO GetById(int cpaCntcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, cpaCntcodi);
            CpaCentralDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpaCentralDTO> List()
        {
            List<CpaCentralDTO> entities = new List<CpaCentralDTO>();
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

        #region CU07-CU08-CU09-CU10
        public List<CpaCentralDTO> ListByRevision(int cparcodi)
        {
            List<CpaCentralDTO> entities = new List<CpaCentralDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByRevision);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, cparcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaCentralDTO entity = helper.Create(dr);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }
        #endregion

        #region CU03
        public List<CpaCentralDTO> ListaCentralesIntegrantes(int empresa, int revision, string estado)
        {
            CpaCentralDTO entity = new CpaCentralDTO();
            List<CpaCentralDTO> entitys = new List<CpaCentralDTO>();
            string query = string.Format(helper.SqlListaCentralesIntegrantes, empresa, revision, estado);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaCentralDTO();

                    int iCpacntcodi = dr.GetOrdinal(helper.Cpacntcodi);
                    if (!dr.IsDBNull(iCpacntcodi)) entity.Cpacntcodi = dr.GetInt32(iCpacntcodi);

                    int iCpaempcodi = dr.GetOrdinal(helper.Cpaempcodi);
                    if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Equinomb = dr.GetString(iEmprnomb);

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = dr.GetInt32(iBarrcodi);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iCpacntestado = dr.GetOrdinal(helper.Cpacntestado);
                    if (!dr.IsDBNull(iCpacntestado)) entity.Cpacntestado = dr.GetString(iCpacntestado);

                    int iCpacnttipo = dr.GetOrdinal(helper.Cpacnttipo);
                    if (!dr.IsDBNull(iCpacnttipo)) entity.Cpacnttipo = dr.GetString(iCpacnttipo);

                    int iCpacntfecejecinicio = dr.GetOrdinal(helper.Cpacntfecejecinicio);
                    if (!dr.IsDBNull(iCpacntfecejecinicio)) entity.Cpacntfecejecinicio = dr.GetDateTime(iCpacntfecejecinicio);

                    int iCpacntfecejecfin = dr.GetOrdinal(helper.Cpacntfecejecfin);
                    if (!dr.IsDBNull(iCpacntfecejecfin)) entity.Cpacntfecejecfin = dr.GetDateTime(iCpacntfecejecfin);

                    int iCpacntfecproginicio = dr.GetOrdinal(helper.Cpacntfecproginicio);
                    if (!dr.IsDBNull(iCpacntfecproginicio)) entity.Cpacntfecproginicio = dr.GetDateTime(iCpacntfecproginicio);

                    int iCpacntfecprogfin = dr.GetOrdinal(helper.Cpacntfecprogfin);
                    if (!dr.IsDBNull(iCpacntfecprogfin)) entity.Cpacntfecprogfin = dr.GetDateTime(iCpacntfecprogfin);

                    int iCpacntusucreacion = dr.GetOrdinal(helper.Cpacntusucreacion);
                    if (!dr.IsDBNull(iCpacntusucreacion)) entity.Cpacntusucreacion = dr.GetString(iCpacntusucreacion);

                    int iCpacntfeccreacion = dr.GetOrdinal(helper.Cpacntfeccreacion);
                    if (!dr.IsDBNull(iCpacntfeccreacion)) entity.Cpacntfeccreacion = dr.GetDateTime(iCpacntfeccreacion);

                    int iCpacntusumodificacion = dr.GetOrdinal(helper.Cpacntusumodificacion);
                    if (!dr.IsDBNull(iCpacntusumodificacion)) entity.Cpacntusumodificacion = dr.GetString(iCpacntusumodificacion);

                    int iCpacntfecmodificacion = dr.GetOrdinal(helper.Cpacntfecmodificacion);
                    if (!dr.IsDBNull(iCpacntfecmodificacion)) entity.Cpacntfecmodificacion = dr.GetDateTime(iCpacntfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateEstadoCentralIntegrante(CpaCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstadoCentralGeneradora);

            dbProvider.AddInParameter(command, helper.Cpacntestado, DbType.String, entity.Cpacntestado);
            dbProvider.AddInParameter(command, helper.Cpacntusumodificacion, DbType.String, entity.Cpacntusumodificacion);
            dbProvider.AddInParameter(command, helper.Cpacntfecmodificacion, DbType.DateTime, entity.Cpacntfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, entity.Cpacntcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateCentralPMPO(CpaCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCentralPMPO);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, (object)entity.Barrcodi ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, (object)entity.Ptomedicodi ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacnttipo, DbType.String, entity.Cpacnttipo);
            dbProvider.AddInParameter(command, helper.Cpacntfecejecinicio, DbType.DateTime, (object)entity.Cpacntfecejecinicio ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntfecejecfin, DbType.DateTime, (object)entity.Cpacntfecejecfin ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntfecproginicio, DbType.DateTime, (object)entity.Cpacntfecproginicio ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntfecprogfin, DbType.DateTime, (object)entity.Cpacntfecprogfin ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntusumodificacion, DbType.String, (object)entity.Cpacntusumodificacion ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntfecmodificacion, DbType.DateTime, (object)entity.Cpacntfecmodificacion ?? DBNull.Value);
            dbProvider.AddInParameter(command, helper.Cpacntcodi, DbType.Int32, entity.Cpacntcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CpaCentralDTO> FiltroCentralesIntegrantes(int revision)
        {
            CpaCentralDTO entity = new CpaCentralDTO();
            List<CpaCentralDTO> entitys = new List<CpaCentralDTO>();
            string query = string.Format(helper.SqlFiltroCentralesIntegrantes, revision);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaCentralDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Equinomb = dr.GetString(iEmprnomb);

                    int iEmprnombconcatenado = dr.GetOrdinal(helper.Equinombconcatenado);
                    if (!dr.IsDBNull(iEmprnombconcatenado)) entity.Equinombconcatenado = dr.GetString(iEmprnombconcatenado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpaCentralDTO> ListaCentralesEmpresasParticipantes(int revision, int central, int empresa, int barraTrans)
        {
            CpaCentralDTO entity = new CpaCentralDTO();
            List<CpaCentralDTO> entitys = new List<CpaCentralDTO>();
            string query = string.Format(helper.SqlListaCentralesEmpresasParticipantes, revision, central, empresa, barraTrans);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaCentralDTO();

                    int iCpacntcodi = dr.GetOrdinal(helper.Cpacntcodi);
                    if (!dr.IsDBNull(iCpacntcodi)) entity.Cpacntcodi = dr.GetInt32(iCpacntcodi);

                    int iCpaempcodi = dr.GetOrdinal(helper.Cpaempcodi);
                    if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);


                    int iEquifechiniopcom = dr.GetOrdinal(helper.Equifechiniopcom);
                    if (!dr.IsDBNull(iEquifechiniopcom)) entity.Equifechiniopcom = dr.GetDateTime(iEquifechiniopcom);

                    int iEquifechfinopcom= dr.GetOrdinal(helper.Equifechfinopcom);
                    if (!dr.IsDBNull(iEquifechfinopcom)) entity.Equifechfinopcom = dr.GetDateTime(iEquifechfinopcom);


                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = dr.GetInt32(iBarrcodi);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iCpacntestado = dr.GetOrdinal(helper.Cpacntestado);
                    if (!dr.IsDBNull(iCpacntestado)) entity.Cpacntestado = dr.GetString(iCpacntestado);

                    int iCpacnttipo = dr.GetOrdinal(helper.Cpacnttipo);
                    if (!dr.IsDBNull(iCpacnttipo)) entity.Cpacnttipo = dr.GetString(iCpacnttipo);

                    int iCpacntfecejecinicio = dr.GetOrdinal(helper.Cpacntfecejecinicio);
                    if (!dr.IsDBNull(iCpacntfecejecinicio)) entity.Cpacntfecejecinicio = dr.GetDateTime(iCpacntfecejecinicio);

                    int iCpacntfecejecfin = dr.GetOrdinal(helper.Cpacntfecejecfin);
                    if (!dr.IsDBNull(iCpacntfecejecfin)) entity.Cpacntfecejecfin = dr.GetDateTime(iCpacntfecejecfin);

                    int iCpacntfecproginicio = dr.GetOrdinal(helper.Cpacntfecproginicio);
                    if (!dr.IsDBNull(iCpacntfecproginicio)) entity.Cpacntfecproginicio = dr.GetDateTime(iCpacntfecproginicio);

                    int iCpacntfecprogfin = dr.GetOrdinal(helper.Cpacntfecprogfin);
                    if (!dr.IsDBNull(iCpacntfecprogfin)) entity.Cpacntfecprogfin = dr.GetDateTime(iCpacntfecprogfin);

                    int iCpacntusumodificacion = dr.GetOrdinal(helper.Cpacntusumodificacion);
                    if (!dr.IsDBNull(iCpacntusumodificacion)) entity.Cpacntusumodificacion = dr.GetString(iCpacntusumodificacion);

                    int iCpacntfecmodificacion = dr.GetOrdinal(helper.Cpacntfecmodificacion);
                    if (!dr.IsDBNull(iCpacntfecmodificacion)) entity.Cpacntfecmodificacion = dr.GetDateTime(iCpacntfecmodificacion);

                    int iCentralespmpo = dr.GetOrdinal(helper.Centralespmpo);
                    if (!dr.IsDBNull(iCentralespmpo)) entity.Centralespmpo = dr.GetString(iCentralespmpo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpaCentralDTO> ListaCentralesPorEmpresaRevison(int empresa, int revision, int central)
        {
            CpaCentralDTO entity = new CpaCentralDTO();
            List<CpaCentralDTO> entitys = new List<CpaCentralDTO>();
            string query = string.Format(helper.SqlListaCentralesPorEmpresaRevison, empresa, revision, central);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaCentralDTO();

                    int iCpacntcodi = dr.GetOrdinal(helper.Cpacntcodi);
                    if (!dr.IsDBNull(iCpacntcodi)) entity.Cpacntcodi = dr.GetInt32(iCpacntcodi);

                    int iCpaempcodi = dr.GetOrdinal(helper.Cpaempcodi);
                    if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Equinomb = dr.GetString(iEmprnomb);

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = dr.GetInt32(iBarrcodi);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iCpacntestado = dr.GetOrdinal(helper.Cpacntestado);
                    if (!dr.IsDBNull(iCpacntestado)) entity.Cpacntestado = dr.GetString(iCpacntestado);

                    int iCpacnttipo = dr.GetOrdinal(helper.Cpacnttipo);
                    if (!dr.IsDBNull(iCpacnttipo)) entity.Cpacnttipo = dr.GetString(iCpacnttipo);

                    int iCpacntfecejecinicio = dr.GetOrdinal(helper.Cpacntfecejecinicio);
                    if (!dr.IsDBNull(iCpacntfecejecinicio)) entity.Cpacntfecejecinicio = dr.GetDateTime(iCpacntfecejecinicio);

                    int iCpacntfecejecfin = dr.GetOrdinal(helper.Cpacntfecejecfin);
                    if (!dr.IsDBNull(iCpacntfecejecfin)) entity.Cpacntfecejecfin = dr.GetDateTime(iCpacntfecejecfin);

                    int iCpacntfecproginicio = dr.GetOrdinal(helper.Cpacntfecproginicio);
                    if (!dr.IsDBNull(iCpacntfecproginicio)) entity.Cpacntfecproginicio = dr.GetDateTime(iCpacntfecproginicio);

                    int iCpacntfecprogfin = dr.GetOrdinal(helper.Cpacntfecprogfin);
                    if (!dr.IsDBNull(iCpacntfecprogfin)) entity.Cpacntfecprogfin = dr.GetDateTime(iCpacntfecprogfin);

                    int iCpacntusucreacion = dr.GetOrdinal(helper.Cpacntusucreacion);
                    if (!dr.IsDBNull(iCpacntusucreacion)) entity.Cpacntusucreacion = dr.GetString(iCpacntusucreacion);

                    int iCpacntfeccreacion = dr.GetOrdinal(helper.Cpacntfeccreacion);
                    if (!dr.IsDBNull(iCpacntfeccreacion)) entity.Cpacntfeccreacion = dr.GetDateTime(iCpacntfeccreacion);

                    int iCpacntusumodificacion = dr.GetOrdinal(helper.Cpacntusumodificacion);
                    if (!dr.IsDBNull(iCpacntusumodificacion)) entity.Cpacntusumodificacion = dr.GetString(iCpacntusumodificacion);

                    int iCpacntfecmodificacion = dr.GetOrdinal(helper.Cpacntfecmodificacion);
                    if (!dr.IsDBNull(iCpacntfecmodificacion)) entity.Cpacntfecmodificacion = dr.GetDateTime(iCpacntfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpaCentralDTO> ListaCentralesPorRevison(int revision, int central)
        {
            CpaCentralDTO entity = new CpaCentralDTO();
            List<CpaCentralDTO> entitys = new List<CpaCentralDTO>();
            string query = string.Format(helper.SqlListaCentralesPorRevison, revision, central);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaCentralDTO();

                    int iCpacntcodi = dr.GetOrdinal(helper.Cpacntcodi);
                    if (!dr.IsDBNull(iCpacntcodi)) entity.Cpacntcodi = dr.GetInt32(iCpacntcodi);

                    int iCpaempcodi = dr.GetOrdinal(helper.Cpaempcodi);
                    if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = dr.GetInt32(iBarrcodi);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iCpacntestado = dr.GetOrdinal(helper.Cpacntestado);
                    if (!dr.IsDBNull(iCpacntestado)) entity.Cpacntestado = dr.GetString(iCpacntestado);

                    int iCpacnttipo = dr.GetOrdinal(helper.Cpacnttipo);
                    if (!dr.IsDBNull(iCpacnttipo)) entity.Cpacnttipo = dr.GetString(iCpacnttipo);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region CU011
        public List<CpaCentralDTO> GetByRevisionByTipoEmpresaByEstadoEmpresaByEstadoCentral(int cparcodi, string cpaemptipo, string cpaempestado, string cpacntestado)
        {
            string query = string.Format(helper.SqlGetByRevisionByTipoEmpresaByEstadoEmpresaByEstadoCentral, cparcodi, cpaemptipo, cpaempestado, cpacntestado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CpaCentralDTO> entitys = new List<CpaCentralDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpaCentralDTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iCpaemptipo = dr.GetOrdinal(helper.Cpaemptipo);
                    if (!dr.IsDBNull(iCpaemptipo)) entity.Cpaemptipo = dr.GetString(iCpaemptipo);

                    int iCpaempestado = dr.GetOrdinal(helper.Cpaempestado);
                    if (!dr.IsDBNull(iCpaempestado)) entity.Cpaempestado = dr.GetString(iCpaempestado);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }
        #endregion

        public List<CpaCentralDTO> ListaCentralesByEmpresa(int codigo)
        {
            CpaCentralDTO entity = new CpaCentralDTO();
            List<CpaCentralDTO> entitys = new List<CpaCentralDTO>();
            string query = string.Format(helper.SqlListaCentralesByEmpresa, codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaCentralDTO();

                    int iCpacntcodi = dr.GetOrdinal(helper.Cpacntcodi);
                    if (!dr.IsDBNull(iCpacntcodi)) entity.Cpacntcodi = dr.GetInt32(iCpacntcodi);

                    int iCpaempcodi = dr.GetOrdinal(helper.Cpaempcodi);
                    if (!dr.IsDBNull(iCpaempcodi)) entity.Cpaempcodi = dr.GetInt32(iCpaempcodi);

                    int iCpacntestado = dr.GetOrdinal(helper.Cpacntestado);
                    if (!dr.IsDBNull(iCpacntestado)) entity.Cpacntestado = dr.GetString(iCpacntestado);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }
    }
}
