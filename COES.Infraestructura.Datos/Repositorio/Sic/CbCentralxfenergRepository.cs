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
    /// Clase de acceso a datos de la tabla CB_CENTRALXFENERG
    /// </summary>
    public class CbCentralxfenergRepository : RepositoryBase, ICbCentralxfenergRepository
    {
        public CbCentralxfenergRepository(string strConn) : base(strConn)
        {
        }

        CbCentralxfenergHelper helper = new CbCentralxfenergHelper();

        public int Save(CbCentralxfenergDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbcxfecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cbcxfenuevo, DbType.Int32, entity.Cbcxfenuevo);
            dbProvider.AddInParameter(command, helper.Cbcxfeexistente, DbType.Int32, entity.Cbcxfeexistente);
            dbProvider.AddInParameter(command, helper.Estcomcodi, DbType.Int32, entity.Estcomcodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cbcxfeactivo, DbType.Int32, entity.Cbcxfeactivo);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Cbcxfevisibleapp, DbType.Int32, entity.Cbcxfevisibleapp);
            dbProvider.AddInParameter(command, helper.Cbcxfeorden, DbType.Int32, entity.Cbcxfeorden);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbCentralxfenergDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbcxfenuevo, DbType.Int32, entity.Cbcxfenuevo);
            dbProvider.AddInParameter(command, helper.Cbcxfeexistente, DbType.Int32, entity.Cbcxfeexistente);
            dbProvider.AddInParameter(command, helper.Estcomcodi, DbType.Int32, entity.Estcomcodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cbcxfeactivo, DbType.Int32, entity.Cbcxfeactivo);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Cbcxfevisibleapp, DbType.Int32, entity.Cbcxfevisibleapp);
            dbProvider.AddInParameter(command, helper.Cbcxfeorden, DbType.Int32, entity.Cbcxfeorden);

            dbProvider.AddInParameter(command, helper.Cbcxfecodi, DbType.Int32, entity.Cbcxfecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbcxfecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbcxfecodi, DbType.Int32, cbcxfecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbCentralxfenergDTO GetById(int cbcxfecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbcxfecodi, DbType.Int32, cbcxfecodi);
            CbCentralxfenergDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                }
            }

            return entity;
        }

        public CbCentralxfenergDTO GetByFenergYGrupocodi(int fenergcodi, int grupocodi)
        {
            string sql = string.Format(helper.SqlGetByFenergYGrupocodi, fenergcodi, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CbCentralxfenergDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                }
            }

            return entity;
        }

        public List<CbCentralxfenergDTO> List()
        {
            List<CbCentralxfenergDTO> entitys = new List<CbCentralxfenergDTO>();
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

        public List<CbCentralxfenergDTO> GetByCriteria(string estcomcodis)
        {
            string sql = string.Format(helper.SqlGetByCriteria, estcomcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            List<CbCentralxfenergDTO> entitys = new List<CbCentralxfenergDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
