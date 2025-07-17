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
    /// Clase de acceso a datos de la tabla EVE_SUBCAUSAEVENTO
    /// </summary>
    public class EveSubcausaeventoRepository: RepositoryBase, IEveSubcausaeventoRepository
    {
        public EveSubcausaeventoRepository(string strConn): base(strConn)
        {

        }

        EveSubcausaeventoHelper helper = new EveSubcausaeventoHelper();

        public int Save(EveSubcausaeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, entity.Causaevencodi);
            dbProvider.AddInParameter(command, helper.Subcausadesc, DbType.String, entity.Subcausadesc);
            dbProvider.AddInParameter(command, helper.Subcausaabrev, DbType.String, entity.Subcausaabrev);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveSubcausaeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Subcausadesc, DbType.String, entity.Subcausadesc);
            dbProvider.AddInParameter(command, helper.Subcausaabrev, DbType.String, entity.Subcausaabrev);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, entity.Causaevencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int subcausacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, subcausacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveSubcausaeventoDTO GetById(int subcausacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, subcausacodi);
            EveSubcausaeventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveSubcausaeventoDTO> List()
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
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

        public List<EveSubcausaeventoDTO> GetByCriteria(int idTipoEvento)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, idTipoEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveSubcausaeventoDTO> ObtenerPorCausa(int idCausaEvento)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorCausa);
            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, idCausaEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveSubcausaeventoDTO> ObtenerSubcausaEvento(int subcausacodi)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerSubcausaEvento);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, subcausacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveSubcausaeventoDTO entity = new EveSubcausaeventoDTO();

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iSubcausaabrev = dr.GetOrdinal(helper.Subcausaabrev);
                    if (!dr.IsDBNull(iSubcausaabrev)) entity.Subcausaabrev = dr.GetString(iSubcausaabrev);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        // Inicio de Agregados - Sistema de Compensaciones
        public List<EveSubcausaeventoDTO> ListTipoOperacion(int pecacodi)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();

            string query = string.Format(helper.SqlListTipoOperacion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EveSubcausaeventoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EveSubcausaeventoDTO();

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = dr.GetInt32(iSubcausacodi);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetSubCasusaCodi(string desc)
        {
            string query = string.Format(helper.SqlGetSubCausaCodi, desc);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            int subCausaCodi = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) subCausaCodi = dr.GetInt32(iSubcausacodi);
                }
            }
            return subCausaCodi;
        }

        public List<EveSubcausaeventoDTO> ListSubCausaEvento(int pecacodi)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();

            string query = string.Format(helper.SqlListSubCausaEvento, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EveSubcausaeventoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EveSubcausaeventoDTO();

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = dr.GetInt32(iSubcausacodi);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        // Fin de Agregados - Sistema de Compensaciones

        #region PR5
        public List<EveSubcausaeventoDTO> ObtenerXCausaXCmg(int idCausaEvento, string subcausacmg)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerXCausaXCmg);
            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, idCausaEvento);
            dbProvider.AddInParameter(command, helper.SubcausaCmg, DbType.String, subcausacmg);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iSubcausaCmg = dr.GetOrdinal(helper.SubcausaCmg);
                    if (!dr.IsDBNull(iSubcausaCmg)) entity.SubcausaCmg = dr.GetString(iSubcausaCmg);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveSubcausaeventoDTO> GetSubcausaEventoXId(int subcausacodi)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerXCausaXID);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, subcausacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveSubcausaeventoDTO entity = new EveSubcausaeventoDTO();

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iCausaeventocodi = dr.GetOrdinal(helper.Causaevencodi);
                    if (!dr.IsDBNull(iCausaeventocodi)) entity.Causaevencodi = Convert.ToInt32(dr.GetValue(iCausaeventocodi));

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iSubcausaabrev = dr.GetOrdinal(helper.Subcausaabrev);
                    if (!dr.IsDBNull(iSubcausaabrev)) entity.Subcausaabrev = dr.GetString(iSubcausaabrev);

                    int iSubcausaCmg = dr.GetOrdinal(helper.SubcausaCmg);
                    if (!dr.IsDBNull(iSubcausaCmg)) entity.SubcausaCmg = dr.GetString(iSubcausaCmg);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Horas Operacion EMS
        public List<EveSubcausaeventoDTO> ListarTipoOperacionHO()
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarTipoOperacionHO);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region INTERVENCIONES
        public List<EveSubcausaeventoDTO> ListarComboCausas(int iEscenario)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            DbCommand command = null;

            if (iEscenario == 1) // 1 = Mantenimiento
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboCausasMantenimiento);
            else if (iEscenario == 2) // 2 = Consulta
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboCausasConsulta);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveSubcausaeventoDTO entity = new EveSubcausaeventoDTO();

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        public List<EveSubcausaeventoDTO> ObtenerSubcausaEventoByAreausuaria(int subcausacodi, int areacode)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            string sql = string.Format(helper.SqlObtenerSubcausaEventoByAreausuaria, subcausacodi, areacode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveSubcausaeventoDTO entity = new EveSubcausaeventoDTO();

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region FIT - VALORIZACION DIARIA
        public int GetCodByAbrev(string calificacion)
        {
            string query = string.Format(helper.GetCodByAbrev, calificacion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            int CausaEvenCodi = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iCausaevencodi = dr.GetOrdinal(helper.Causaevencodi);
                    if (!dr.IsDBNull(iCausaevencodi)) CausaEvenCodi = dr.GetInt32(iCausaevencodi);
                }
            }
            return CausaEvenCodi;
        }
        #endregion

        #region PRONOSTICO DEMANDA
        public void UpdateBy(EveSubcausaeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateBy);

            dbProvider.AddInParameter(command, helper.Subcausadesc, DbType.String, entity.Subcausadesc);
            dbProvider.AddInParameter(command, helper.Subcausaabrev, DbType.String, entity.Subcausaabrev);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Causaevencodi, DbType.Int32, entity.Causaevencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        #endregion

        #region SIOSEIN
        public List<EveSubcausaeventoDTO> GetListByIds(string subcausacodi)
        {
            List<EveSubcausaeventoDTO> entitys = new List<EveSubcausaeventoDTO>();
            string query = string.Format(helper.SqlGetListByIds, subcausacodi);
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

        #endregion
    }
}
