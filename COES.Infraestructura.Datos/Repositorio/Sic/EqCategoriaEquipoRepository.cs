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
    /// Clase de acceso a datos de la tabla EQ_CATEGORIA_EQUIPO
    /// </summary>
    public class EqCategoriaEquipoRepository : RepositoryBase, IEqCategoriaEquipoRepository
    {
        public EqCategoriaEquipoRepository(string strConn) : base(strConn) { }

        EqCategoriaEquipoHelper helper = new EqCategoriaEquipoHelper();
        
        public void Save(EqCategoriaEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, entity.Ctgdetcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Ctgequiestado, DbType.String, entity.Ctgequiestado);
            dbProvider.AddInParameter(command, helper.UsuarioCreacion, DbType.String, entity.UsuarioCreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EqCategoriaEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, entity.Ctgdetcodi);
            dbProvider.AddInParameter(command, helper.Ctgequiestado, DbType.String, entity.Ctgequiestado);
            dbProvider.AddInParameter(command, helper.UsuarioUpdate, DbType.String, entity.UsuarioUpdate);
            dbProvider.AddInParameter(command, helper.CtgdetcodiOld, DbType.Int32, entity.CtgdetcodiOld);
            dbProvider.AddInParameter(command, helper.EquicodiOld, DbType.Int32, entity.EquicodiOld);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ctgequicodi) { }

        public EqCategoriaEquipoDTO GetById(int ctgdetcodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ctgdetcodi, DbType.Int32, ctgdetcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            EqCategoriaEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreaNomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.Areanomb = dr.GetString(iAreaNomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCtgdetnomb = dr.GetOrdinal(helper.Ctgdetnomb);
                    if (!dr.IsDBNull(iCtgdetnomb)) entity.Ctgdetnomb = dr.GetString(iCtgdetnomb);

                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);
                    int iCtgcodi = dr.GetOrdinal(helper.Ctgcodi);
                    if (!dr.IsDBNull(iCtgcodi)) entity.Ctgcodi = Convert.ToInt32(dr.GetValue(iCtgcodi));
                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);
                }
            }

            return entity;
        }

        public EqCategoriaEquipoDTO GetByIdEquipo(int ctgcodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEquipo);

            dbProvider.AddInParameter(command, helper.Ctgcodi, DbType.Int32, ctgcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            EqCategoriaEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreaNomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.Areanomb = dr.GetString(iAreaNomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCtgdetnomb = dr.GetOrdinal(helper.Ctgdetnomb);
                    if (!dr.IsDBNull(iCtgdetnomb)) entity.Ctgdetnomb = dr.GetString(iCtgdetnomb);

                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);
                    int iCtgcodi = dr.GetOrdinal(helper.Ctgcodi);
                    if (!dr.IsDBNull(iCtgcodi)) entity.Ctgcodi = Convert.ToInt32(dr.GetValue(iCtgcodi));
                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);
                }
            }

            return entity;
        }

        public List<EqCategoriaEquipoDTO> List()
        {
            List<EqCategoriaEquipoDTO> entitys = new List<EqCategoriaEquipoDTO>();
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

        public List<EqCategoriaEquipoDTO> ListaPaginado(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, int iCategoria, int iSubclasificacion, string nombre, int nroPagina, int nroFilas)
        {
            List<EqCategoriaEquipoDTO> entitys = new List<EqCategoriaEquipoDTO>();
            string query = string.Format(helper.SqlListPaginadoClasificacion, iEmpresa, iFamilia, iTipoEmpresa, iEquipo, nombre, iCategoria, iSubclasificacion, nroPagina, nroFilas);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaEquipoDTO entity = new EqCategoriaEquipoDTO();
                    entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iAreaNomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.Areanomb = dr.GetString(iAreaNomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCtgdetnomb = dr.GetOrdinal(helper.Ctgdetnomb);
                    if (!dr.IsDBNull(iCtgdetnomb)) entity.Ctgdetnomb = dr.GetString(iCtgdetnomb);
                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);
                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int TotalClasificacion(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, int iCategoria, int iSubclasificacion, string nombre)
        {
            nombre = nombre == null ? string.Empty : nombre.ToLowerInvariant();
            String query = String.Format(helper.SqlTotalListadoClasificacion, iEmpresa, iFamilia, iTipoEmpresa, iEquipo, nombre, iCategoria, iSubclasificacion);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaAndEquipo(int ctgdetcodi, int equicodi)
        {
            List<EqCategoriaEquipoDTO> entitys = new List<EqCategoriaEquipoDTO>();
            string query = string.Format(helper.SqlListClasificacionByCategoriaAndEquipo, ctgdetcodi, equicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaEquipoDTO entity = new EqCategoriaEquipoDTO();

                    int iCtgdetcodi = dr.GetOrdinal(helper.Ctgdetcodi);
                    if (!dr.IsDBNull(iCtgdetcodi)) entity.Ctgdetcodi = Convert.ToInt32(dr.GetValue(iCtgdetcodi));
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCtgcodi = dr.GetOrdinal(helper.Ctgcodi);
                    if (!dr.IsDBNull(iCtgcodi)) entity.Ctgcodi = Convert.ToInt32(dr.GetValue(iCtgcodi));
                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaAndEmpresa(int ctgdetcodi, int emprcodi)
        {
            List<EqCategoriaEquipoDTO> entitys = new List<EqCategoriaEquipoDTO>();
            string query = string.Format(helper.SqlListClasificacionByCategoriaAndEmpresa, ctgdetcodi, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaEquipoDTO entity = new EqCategoriaEquipoDTO();

                    int iCtgdetcodi = dr.GetOrdinal(helper.Ctgdetcodi);
                    if (!dr.IsDBNull(iCtgdetcodi)) entity.Ctgdetcodi = Convert.ToInt32(dr.GetValue(iCtgdetcodi));
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCtgcodi = dr.GetOrdinal(helper.Ctgcodi);
                    if (!dr.IsDBNull(iCtgcodi)) entity.Ctgcodi = Convert.ToInt32(dr.GetValue(iCtgcodi));
                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);

                    int iAreaNomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.Areanomb = dr.GetString(iAreaNomb);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaPadreAndEquipo(int ctgpadrecodi, int equicodi)
        {
            List<EqCategoriaEquipoDTO> entitys = new List<EqCategoriaEquipoDTO>();
            string query = string.Format(helper.SqlListClasificacionByCategoriaPadreAndEquipo, ctgpadrecodi, equicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaEquipoDTO entity = new EqCategoriaEquipoDTO();

                    int iCtgequiestado = dr.GetOrdinal(helper.Ctgequiestado);
                    if (!dr.IsDBNull(iCtgequiestado)) entity.Ctgequiestado = dr.GetString(iCtgequiestado);

                    int iCtgdetcodi = dr.GetOrdinal(helper.Ctgdetcodi);
                    if (!dr.IsDBNull(iCtgdetcodi)) entity.Ctgdetcodi = Convert.ToInt32(dr.GetValue(iCtgdetcodi));
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCtgcodi = dr.GetOrdinal(helper.Ctgcodi);
                    if (!dr.IsDBNull(iCtgcodi)) entity.Ctgcodi = Convert.ToInt32(dr.GetValue(iCtgcodi));
                    int iCtgnomb = dr.GetOrdinal(helper.Ctgnomb);
                    if (!dr.IsDBNull(iCtgnomb)) entity.Ctgnomb = dr.GetString(iCtgnomb);

                    int iCtgpadrenomb = dr.GetOrdinal(helper.Ctgpadrenomb);
                    if (!dr.IsDBNull(iCtgpadrenomb)) entity.Ctgpadrenomb = dr.GetString(iCtgpadrenomb);

                    int iCtgFlagExcluyente = dr.GetOrdinal(helper.CtgFlagExcluyente);
                    if (!dr.IsDBNull(iCtgFlagExcluyente)) entity.CtgFlagExcluyente = dr.GetString(iCtgFlagExcluyente);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaDetalle(int ctgdetcodi)
        {
            List<EqCategoriaEquipoDTO> entitys = new List<EqCategoriaEquipoDTO>();
            string query = string.Format(helper.SqlListClasificacionByCategoriaDetalle, ctgdetcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCategoriaEquipoDTO entity = new EqCategoriaEquipoDTO();

                    int iCtgequiestado = dr.GetOrdinal(helper.Ctgequiestado);
                    if (!dr.IsDBNull(iCtgequiestado)) entity.Ctgequiestado = dr.GetString(iCtgequiestado);

                    int iCtgdetcodi = dr.GetOrdinal(helper.Ctgdetcodi);
                    if (!dr.IsDBNull(iCtgdetcodi)) entity.Ctgdetcodi = Convert.ToInt32(dr.GetValue(iCtgdetcodi));
                    
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;        
        }
    }
}
