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
    /// Clase de acceso a datos de la tabla EN_ENSAYOFORMATO
    /// </summary>
    public class EnEnsayoformatoRepository : RepositoryBase, IEnEnsayoformatoRepository
    {
        public EnEnsayoformatoRepository(string strConn)
            : base(strConn)
        {
        }

        EnEnsayoformatoHelper helper = new EnEnsayoformatoHelper();

        public void UpdateEstado(int ensformtestado, int enunidadcodi, int formatocodi)
        {
            string query = string.Format(helper.SqlUpdateEstado, ensformtestado, enunidadcodi, formatocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EnEnsayoformatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ensformatnomblogico, DbType.String, entity.Ensformatnomblogico);
            dbProvider.AddInParameter(command, helper.Ensformtestado, DbType.Int32, entity.Ensformtestado);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Ensformatnombfisico, DbType.String, entity.Ensformatnombfisico);
            dbProvider.AddInParameter(command, helper.Formatocodi, DbType.Int32, entity.Formatocodi);
            dbProvider.AddInParameter(command, helper.Enunidadcodi, DbType.Int32, entity.Enunidadcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Save(EnEnsayoformatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Formatocodi, DbType.Int32, entity.Formatocodi);
            dbProvider.AddInParameter(command, helper.Enunidadcodi, DbType.Int32, entity.Enunidadcodi);
            dbProvider.AddInParameter(command, helper.Ensformatnomblogico, DbType.String, entity.Ensformatnomblogico);
            dbProvider.AddInParameter(command, helper.Ensformtestado, DbType.Int32, entity.Ensformtestado);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Ensformatnombfisico, DbType.String, entity.Ensformatnombfisico);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int formatocodi, int enunidadcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Formatocodi, DbType.Int32, formatocodi);
            dbProvider.AddInParameter(command, helper.Enunidadcodi, DbType.Int32, enunidadcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EnEnsayoformatoDTO GetById(int formatocodi, int enunidadcodi)
        {

            string query = string.Format(helper.SqlGetById, formatocodi, enunidadcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EnEnsayoformatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnEnsayoformatoDTO> List()
        {
            List<EnEnsayoformatoDTO> entitys = new List<EnEnsayoformatoDTO>();
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

        public List<EnEnsayoformatoDTO> GetByCriteria()
        {
            List<EnEnsayoformatoDTO> entitys = new List<EnEnsayoformatoDTO>();
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

        public List<EnEnsayoformatoDTO> ListaFormatoXEnsayo(int ensayocodi)
        {
            List<EnEnsayoformatoDTO> entitys = new List<EnEnsayoformatoDTO>();
            string query = string.Format(helper.SqlListaFormatoXEnsayo, ensayocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EnEnsayoformatoDTO entity = new EnEnsayoformatoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iformatodesc = dr.GetOrdinal("Enfmtdesc");
                    int iEstenvnomb = dr.GetOrdinal("Estadonombre");
                    int iEstadocolor = dr.GetOrdinal("Estadocolor");
                    int iEquicodi = dr.GetOrdinal("equicodi");
                    int iEquinomb = dr.GetOrdinal("Equinomb");
                    if (!dr.IsDBNull(iformatodesc)) entity.Formatodesc = dr.GetString(iformatodesc);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);
                    if (!dr.IsDBNull(iEstadocolor)) entity.Estadocolor = dr.GetString(iEstadocolor);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EnEnsayoformatoDTO> ListaFormatoXEnsayo(int ensayocodi, int equicodi)
        {
            List<EnEnsayoformatoDTO> entitys = new List<EnEnsayoformatoDTO>();
            string query = string.Format(helper.SqlListaFormatoXEnsayo2, ensayocodi, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EnEnsayoformatoDTO entity = new EnEnsayoformatoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iformatodesc = dr.GetOrdinal("Enfmtdesc");
                    int iEstenvnomb = dr.GetOrdinal("Estadonombre");
                    int iEstadocolor = dr.GetOrdinal("Estadocolor");
                    int iEquicodi = dr.GetOrdinal("equicodi");
                    int iEnsayocodi = dr.GetOrdinal("Ensayocodi");
                    if (!dr.IsDBNull(iformatodesc)) entity.Formatodesc = dr.GetString(iformatodesc);
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);
                    if (!dr.IsDBNull(iEstadocolor)) entity.Estadocolor = dr.GetString(iEstadocolor);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    if (!dr.IsDBNull(iEnsayocodi)) entity.Ensayocodi = Convert.ToInt32(dr.GetValue(iEnsayocodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EnEnsayoformatoDTO> ListaFormatoXEnsayoEmpresaCtral(int emprcodi, int equicodi)
        {
            List<EnEnsayoformatoDTO> entitys = new List<EnEnsayoformatoDTO>();
            string query = string.Format(helper.SqlListaFormatosEmpresaCentral, emprcodi, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EnEnsayoformatoDTO entity = new EnEnsayoformatoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        ///       
        /// </summary>
        /// <returns></returns>
        public List<EnEnsayoformatoDTO> ListarUnidadesConFormatos(int idEnsayo)
        {
            List<EnEnsayoformatoDTO> entitys = new List<EnEnsayoformatoDTO>();
            string query = string.Format(helper.SqlListarUnidadesConFormatos, idEnsayo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EnEnsayoformatoDTO entity = helper.Create(dr);
                    int iEnsayocodi = dr.GetOrdinal(this.helper.Ensayocodi);
                    if (!dr.IsDBNull(iEnsayocodi)) entity.Ensayocodi = Convert.ToInt32(dr.GetValue(iEnsayocodi));
                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    entitys.Add(entity);

                }
            }
            return entitys;
        }

    }
}
