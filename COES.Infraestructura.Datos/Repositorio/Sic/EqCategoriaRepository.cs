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
    /// Clase de acceso a datos de la tabla EQ_CATEGORIA
    /// </summary>
    public class EqCategoriaRepository : RepositoryBase, IEqCategoriaRepository
    {
        public EqCategoriaRepository(string strConn) : base(strConn) { }

        EqCategoriaHelper helper = new EqCategoriaHelper();

        public int Save(EqCategoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ctgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ctgpadre, DbType.Int32, entity.Ctgpadre);
            dbProvider.AddInParameter(command, helper.Ctgnomb, DbType.String, entity.Ctgnomb);
            dbProvider.AddInParameter(command, helper.CtgFlagExcluyente, DbType.String, entity.CtgFlagExcluyente);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Ctgestado, DbType.String, entity.Ctgestado);
            dbProvider.AddInParameter(command, helper.UsuarioCreacion, DbType.String, entity.UsuarioCreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqCategoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ctgnomb, DbType.String, entity.Ctgnomb);
            dbProvider.AddInParameter(command, helper.CtgFlagExcluyente, DbType.String, entity.CtgFlagExcluyente);
            dbProvider.AddInParameter(command, helper.Ctgestado, DbType.String, entity.Ctgestado);
            dbProvider.AddInParameter(command, helper.UsuarioUpdate, DbType.String, entity.UsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Ctgcodi, DbType.Int32, entity.Ctgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        //inicio agregado
        public void Delete(int ctgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Ctgcodi, DbType.Int32, ctgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqCategoriaDTO GetById(int ctgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ctgcodi, DbType.Int32, ctgcodi);
            EqCategoriaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);

                    int iTotalDetalle = dr.GetOrdinal(helper.TotalDetalle);
                    if (!dr.IsDBNull(iTotalDetalle)) entity.TotalDetalle = Convert.ToInt32(dr.GetValue(iTotalDetalle));

                    int iTotalHijo = dr.GetOrdinal(helper.TotalHijo);
                    if (!dr.IsDBNull(iTotalHijo)) entity.TotalHijo = Convert.ToInt32(dr.GetValue(iTotalHijo));
                }
            }

            return entity;
        }
        //fin agregado

        public List<EqCategoriaDTO> ListPadre(int famcodi, int ctgcodi)
        {
            List<EqCategoriaDTO> entitys = new List<EqCategoriaDTO>();

            string sqlQuery = string.Format(helper.SqlListPadre, famcodi, ctgcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //inicio agregado
        public List<EqCategoriaDTO> ListByFamiliaAndEstado(int famcodi, string estado)
        {
            List<EqCategoriaDTO> entitys = new List<EqCategoriaDTO>();

            string sqlQuery = string.Format(helper.SqlListByFamiliaAndEstado, famcodi, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);

                    int iTotalDetalle = dr.GetOrdinal(helper.TotalDetalle);
                    if (!dr.IsDBNull(iTotalDetalle)) entity.TotalDetalle = Convert.ToInt32(dr.GetValue(iTotalDetalle));

                    int iTotalHijo = dr.GetOrdinal(helper.TotalHijo);
                    if (!dr.IsDBNull(iTotalHijo)) entity.TotalHijo = Convert.ToInt32(dr.GetValue(iTotalHijo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //fin agregado

        public List<EqCategoriaDTO> ListaCategoriaClasificacionByFamiliaAndEstado(int famcodi, string estado)
        {
            List<EqCategoriaDTO> entitys = new List<EqCategoriaDTO>();

            string sqlQuery = string.Format(helper.SqlListCategoriaClasificacion, famcodi, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCategoriaDTO> ListCategoriaHijoByIdPadre(int famcodi, int ctgpadrecodi)
        {
            List<EqCategoriaDTO> entitys = new List<EqCategoriaDTO>();

            string sqlQuery = string.Format(helper.SqlListCategoriaHijoByIdPadre, famcodi, ctgpadrecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //inicio agregado
        public List<EqCategoriaDTO> ListCategoriaHijoByIdPadreAndEmpresa(int famcodi, int ctgpadrecodi, int emprcodi)
        {
            List<EqCategoriaDTO> entitys = new List<EqCategoriaDTO>();

            string sqlQuery = string.Format(helper.SqlListCategoriaHijoByIdPadreAndEmpresa, famcodi, ctgpadrecodi, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int ITotalEquipo = dr.GetOrdinal(helper.TotalEquipo);
                    if (!dr.IsDBNull(ITotalEquipo)) entity.TotalEquipo = Convert.ToInt32(dr.GetValue(ITotalEquipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCategoriaDTO> GetByCriteriaEqCategorias()
        {
            List<EqCategoriaDTO> entitys = new List<EqCategoriaDTO>();

            string sqlQuery = string.Format(helper.SqlGetByCriteria);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
