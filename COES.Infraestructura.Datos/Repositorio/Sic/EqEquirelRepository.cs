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
    /// Clase de acceso a datos de la tabla EQ_EQUIREL
    /// </summary>
    public class EqEquirelRepository : RepositoryBase, IEqEquirelRepository
    {
        public EqEquirelRepository(string strConn)
            : base(strConn)
        {
        }

        EqEquirelHelper helper = new EqEquirelHelper();

        public void Save(EqEquirelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Equicodi1, DbType.Int32, entity.Equicodi1);
            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, entity.Tiporelcodi);
            dbProvider.AddInParameter(command, helper.Equicodi2, DbType.Int32, entity.Equicodi2);
            dbProvider.AddInParameter(command, helper.Equirelagrup, DbType.Int32, entity.Equirelagrup);
            dbProvider.AddInParameter(command, helper.Equirelfecmodificacion, DbType.DateTime, entity.Equirelfecmodificacion);
            dbProvider.AddInParameter(command, helper.Equirelusumodificacion, DbType.String, entity.Equirelusumodificacion);
            dbProvider.AddInParameter(command, helper.Equirelexcep, DbType.Int32, entity.Equirelexcep);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EqEquirelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi1, DbType.Int32, entity.Equicodi1);
            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, entity.Tiporelcodi);
            dbProvider.AddInParameter(command, helper.Equicodi2, DbType.Int32, entity.Equicodi2);
            dbProvider.AddInParameter(command, helper.Equirelagrup, DbType.Int32, entity.Equirelagrup);
            dbProvider.AddInParameter(command, helper.Equirelfecmodificacion, DbType.DateTime, entity.Equirelfecmodificacion);
            dbProvider.AddInParameter(command, helper.Equirelusumodificacion, DbType.String, entity.Equirelusumodificacion);
            dbProvider.AddInParameter(command, helper.Equirelexcep, DbType.Int32, entity.Equirelexcep);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int equicodi1, int tiporelcodi, int equicodi2)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Equicodi1, DbType.Int32, equicodi1);
            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, tiporelcodi);
            dbProvider.AddInParameter(command, helper.Equicodi2, DbType.Int32, equicodi2);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int equicodi1, int tiporelcodi, int equicodi2, string user)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Equirelusumodificacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Equicodi1, DbType.Int32, equicodi1);
            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, tiporelcodi);
            dbProvider.AddInParameter(command, helper.Equicodi2, DbType.Int32, equicodi2);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqEquirelDTO GetById(int equicodi1, int tiporelcodi, int equicodi2)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Equicodi1, DbType.Int32, equicodi1);
            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, tiporelcodi);
            dbProvider.AddInParameter(command, helper.Equicodi2, DbType.Int32, equicodi2);
            EqEquirelDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqEquirelDTO> List()
        {
            List<EqEquirelDTO> entitys = new List<EqEquirelDTO>();
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

        public List<EqEquirelDTO> GetByCriteria(int equicodi, string tiporelcodi)
        {
            List<EqEquirelDTO> entitys = new List<EqEquirelDTO>();
            string strComando = string.Format(helper.SqlGetByCriteria, equicodi, tiporelcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFamcodi1 = dr.GetOrdinal(helper.Famcodi1);
                    if (!dr.IsDBNull(iFamcodi1)) entity.Famcodi1 = Convert.ToInt32(dr.GetValue(iFamcodi1));

                    int iFamcodi2 = dr.GetOrdinal(helper.Famcodi2);
                    if (!dr.IsDBNull(iFamcodi2)) entity.Famcodi2 = Convert.ToInt32(dr.GetValue(iFamcodi2));

                    int iEquinomb1 = dr.GetOrdinal(helper.Equinomb1);
                    if (!dr.IsDBNull(iEquinomb1)) entity.Equinomb1 = dr.GetString(iEquinomb1);

                    int iEquinomb2 = dr.GetOrdinal(helper.Equinomb2);
                    if (!dr.IsDBNull(iEquinomb2)) entity.Equinomb2 = dr.GetString(iEquinomb2);

                    int iEmprnomb1 = dr.GetOrdinal(helper.Emprnomb1);
                    if (!dr.IsDBNull(iEmprnomb1)) entity.Emprnomb1 = dr.GetString(iEmprnomb1);

                    int iEmprnomb2 = dr.GetOrdinal(helper.Emprnomb2);
                    if (!dr.IsDBNull(iEmprnomb2)) entity.Emprnomb2 = dr.GetString(iEmprnomb2);

                    int iFamnomb1 = dr.GetOrdinal(helper.Famnomb1);
                    if (!dr.IsDBNull(iFamnomb1)) entity.Famnomb1 = dr.GetString(iFamnomb1);

                    int iFamnomb2 = dr.GetOrdinal(helper.Famnomb2);
                    if (!dr.IsDBNull(iFamnomb2)) entity.Famnomb2 = dr.GetString(iFamnomb2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquirelDTO> GetByCriteriaTopologia(int equicodi1, int tiporelcodi)
        {
            List<EqEquirelDTO> entitys = new List<EqEquirelDTO>();
            string strComando = string.Format(helper.SqlGetByCriteriaTopologia, equicodi1, tiporelcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new EqEquirelDTO();
                    entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);


                    int iEquipotopologia = dr.GetOrdinal(this.helper.Equipotopologia);
                    if (!dr.IsDBNull(iEquipotopologia)) entity.Equipotopologia = dr.GetString(iEquipotopologia);
                    int iEmpresatopologia = dr.GetOrdinal(this.helper.Empresatopologia);
                    if (!dr.IsDBNull(iEmpresatopologia)) entity.Empresatopologia = dr.GetString(iEmpresatopologia);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Intervenciones
        public List<EqEquirelDTO> ListarXRelacionesXIds(string idsEquipos)
        {
            List<EqEquirelDTO> entitys = new List<EqEquirelDTO>();
            string strComando = string.Format(helper.SqlListarRelacionesByIdsEquicodi, idsEquipos);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquirelDTO entity = new EqEquirelDTO();

                    int iEquicodi1 = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi1)) entity.Equicodi1 = dr.GetInt32(iEquicodi1);

                    int iEquicodi2 = dr.GetOrdinal(helper.Equicodi2);
                    if (!dr.IsDBNull(iEquicodi2)) entity.Equicodi2 = dr.GetInt32(iEquicodi2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquirelDTO> ListarXRelacionesBarraXIds(string idsEquipos)
        {
            List<EqEquirelDTO> entitys = new List<EqEquirelDTO>();
            string strComando = string.Format(helper.SqlListarRelacionesBarraByIdsEquicodi, idsEquipos);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquirelDTO entity = new EqEquirelDTO();

                    int iEquicodi1 = dr.GetOrdinal(helper.Equicodi1);
                    if (!dr.IsDBNull(iEquicodi1)) entity.Equicodi1 = dr.GetInt32(iEquicodi1);

                    int iEquicodi2 = dr.GetOrdinal(helper.Equicodi2);
                    if (!dr.IsDBNull(iEquicodi2)) entity.Equicodi2 = dr.GetInt32(iEquicodi2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

    }
}