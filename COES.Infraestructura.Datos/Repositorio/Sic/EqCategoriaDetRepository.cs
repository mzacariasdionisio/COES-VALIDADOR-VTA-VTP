using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EQ_CATEGORIA_DETALLE
    /// </summary>
    public class EqCategoriaDetRepository : RepositoryBase, IEqCategoriaDetRepository
    {
        public EqCategoriaDetRepository(string strConn) : base(strConn) { }

        EqCategoriaDetHelper helper = new EqCategoriaDetHelper();

        public int Save(EqCategoriaDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ctgcodi, DbType.Int32, entity.Ctgcodi);
            dbProvider.AddInParameter(command, helper.Ctgdetnomb, DbType.String, entity.Ctgdetnomb);
            dbProvider.AddInParameter(command, helper.Ctgdetestado, DbType.String, entity.Ctgdetestado);
            dbProvider.AddInParameter(command, helper.UsuarioCreacion, DbType.String, entity.UsuarioCreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqCategoriaDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ctgdetnomb, DbType.String, entity.Ctgdetnomb);
            dbProvider.AddInParameter(command, helper.Ctgdetestado, DbType.String, entity.Ctgdetestado);
            dbProvider.AddInParameter(command, helper.UsuarioUpdate, DbType.String, entity.UsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, entity.Ctgdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        //inicio agregado
        public void Delete(int ctgdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, ctgdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqCategoriaDetDTO GetById(int ctgdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, ctgdetcodi);
            EqCategoriaDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);
                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);
                    int iCtgpadrecodi = dr.GetOrdinal(helper.Ctgpadrecodi);
                    if (!dr.IsDBNull(iCtgpadrecodi)) entity.Ctgpadrecodi = Convert.ToInt32(dr.GetValue(iCtgpadrecodi));
                    int iCantidadEquipo = dr.GetOrdinal(helper.TotalEquipo);
                    if (!dr.IsDBNull(iCantidadEquipo)) entity.TotalEquipo = Convert.ToInt32(dr.GetValue(iCantidadEquipo));
                }
            }

            return entity;
        }

        public List<EqCategoriaDetDTO> ListByCategoriaAndEstado(int ctgcodi, string estado)
        {
            List<EqCategoriaDetDTO> entitys = new List<EqCategoriaDetDTO>();

            string sqlQuery = string.Format(helper.SqlListByCategoriaAndEstado, ctgcodi, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDetDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);
                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);
                    int iCantidadEquipo = dr.GetOrdinal(helper.TotalEquipo);
                    if (!dr.IsDBNull(iCantidadEquipo)) entity.TotalEquipo = Convert.ToInt32(dr.GetValue(iCantidadEquipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCategoriaDetDTO> ListByCategoriaAndEstadoAndEmpresa(int ctgcodi, string estado, int emprcodi)
        {
            List<EqCategoriaDetDTO> entitys = new List<EqCategoriaDetDTO>();

            string sqlQuery = string.Format(helper.SqlListByCategoriaAndEstadoAndEmpresa, ctgcodi, estado, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDetDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);
                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);
                    int iCantidadEquipo = dr.GetOrdinal(helper.TotalEquipo);
                    if (!dr.IsDBNull(iCantidadEquipo)) entity.TotalEquipo = Convert.ToInt32(dr.GetValue(iCantidadEquipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCategoriaDetDTO> GetByCriteria(int ctgcodi)
        {
            List<EqCategoriaDetDTO> entitys = new List<EqCategoriaDetDTO>();

            string sqlQuery = string.Format(helper.SqlGetByCriteria, ctgcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDetDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
