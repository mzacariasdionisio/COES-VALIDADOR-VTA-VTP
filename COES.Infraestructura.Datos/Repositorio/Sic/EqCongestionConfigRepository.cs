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
    /// Clase de acceso a datos de la tabla EQ_CONGESTION_CONFIG
    /// </summary>
    public class EqCongestionConfigRepository: RepositoryBase, IEqCongestionConfigRepository
    {
        public EqCongestionConfigRepository(string strConn): base(strConn)
        {

        }

        EqCongestionConfigHelper helper = new EqCongestionConfigHelper();

        public int Save(EqCongestionConfigDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, id);          
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.Estado);
            dbProvider.AddInParameter(command, helper.Nombrencp, DbType.String, entity.Nombrencp);
            dbProvider.AddInParameter(command, helper.Codincp, DbType.Int32, entity.Codincp);
            dbProvider.AddInParameter(command, helper.Nodobarra1, DbType.String, entity.Nodobarra1);
            dbProvider.AddInParameter(command, helper.Nodobarra2, DbType.String, entity.Nodobarra2);
            dbProvider.AddInParameter(command, helper.Nodobarra3, DbType.String, entity.Nodobarra3);
            dbProvider.AddInParameter(command, helper.Idems, DbType.String, entity.Idems);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Nombretna1, DbType.String, entity.Nombretna1);
            dbProvider.AddInParameter(command, helper.Nombretna2, DbType.String, entity.Nombretna2);
            dbProvider.AddInParameter(command, helper.Nombretna3, DbType.String, entity.Nombretna3);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqCongestionConfigDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.Estado);
            dbProvider.AddInParameter(command, helper.Nombrencp, DbType.String, entity.Nombrencp);
            dbProvider.AddInParameter(command, helper.Codincp, DbType.Int32, entity.Codincp);
            dbProvider.AddInParameter(command, helper.Nodobarra1, DbType.String, entity.Nodobarra1);
            dbProvider.AddInParameter(command, helper.Nodobarra2, DbType.String, entity.Nodobarra2);
            dbProvider.AddInParameter(command, helper.Nodobarra3, DbType.String, entity.Nodobarra3);
            dbProvider.AddInParameter(command, helper.Idems, DbType.String, entity.Idems);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Nombretna1, DbType.String, entity.Nombretna1);
            dbProvider.AddInParameter(command, helper.Nombretna2, DbType.String, entity.Nombretna2);
            dbProvider.AddInParameter(command, helper.Nombretna3, DbType.String, entity.Nombretna3);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int configcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, configcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqCongestionConfigDTO GetById(int configcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, configcodi);
            EqCongestionConfigDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));
                }
            }

            return entity;
        }

        public List<EqCongestionConfigDTO> List()
        {
            List<EqCongestionConfigDTO> entitys = new List<EqCongestionConfigDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCongestionConfigDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCongestionConfigDTO> ObtenerListadoEquipos()
        {
            List<EqCongestionConfigDTO> entitys = new List<EqCongestionConfigDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerListadoEquipos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCongestionConfigDTO entity = new EqCongestionConfigDTO();

                    int iConfigcodi = dr.GetOrdinal(helper.Configcodi);
                    if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iNodobarra1 = dr.GetOrdinal(helper.Nodobarra1);
                    if (!dr.IsDBNull(iNodobarra1)) entity.Nodobarra1 = dr.GetString(iNodobarra1);

                    int iNodobarra2 = dr.GetOrdinal(helper.Nodobarra2);
                    if (!dr.IsDBNull(iNodobarra2)) entity.Nodobarra2 = dr.GetString(iNodobarra2);

                    int iNodobarra3 = dr.GetOrdinal(helper.Nodobarra3);
                    if (!dr.IsDBNull(iNodobarra3)) entity.Nodobarra3 = dr.GetString(iNodobarra3);

                    int iIdems = dr.GetOrdinal(helper.Idems);
                    if (!dr.IsDBNull(iIdems)) entity.Idems = dr.GetString(iIdems);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iNombretna1 = dr.GetOrdinal(helper.Nombretna1);
                    if (!dr.IsDBNull(iNombretna1)) entity.Nombretna1 = dr.GetString(iNombretna1);

                    int iNombretna2 = dr.GetOrdinal(helper.Nombretna2);
                    if (!dr.IsDBNull(iNombretna2)) entity.Nombretna2 = dr.GetString(iNombretna2);

                    int iNombretna3 = dr.GetOrdinal(helper.Nombretna3);
                    if (!dr.IsDBNull(iNombretna3)) entity.Nombretna3 = dr.GetString(iNombretna3);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCongestionConfigDTO> GetByCriteria(int idEmpresa, string estado, int idGrupo, int idFamilia)
        {
            List<EqCongestionConfigDTO> entitys = new List<EqCongestionConfigDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idEmpresa, estado, idGrupo, idFamilia);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);           

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCongestionConfigDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGrulinnomb = dr.GetOrdinal(helper.Grulinnomb);
                    if (!dr.IsDBNull(iGrulinnomb)) entity.Grulinnomb = dr.GetString(iGrulinnomb);

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqCongestionConfigDTO> ObtenerPorGrupo(int idGrupo)
        {
            List<EqCongestionConfigDTO> entitys = new List<EqCongestionConfigDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorGrupo);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, idGrupo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasFiltro()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasFiltro);            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasLineas()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasLineas);
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasLineaTrafo);
            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListarEquipoLineaPorEmpresa(int idEmpresa, int idFamilia)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string sql = string.Format(helper.SqlListarEquipoLineaTrafoEmpresa, idEmpresa, idFamilia);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read()) 
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ValidarExistencia(int idEquipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistencia);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, idEquipo);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<EqCongestionConfigDTO> ObtenerLineasPorGrupo(int idGrupo)
        {
            List<EqCongestionConfigDTO> entitys = new List<EqCongestionConfigDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerLineaPorGrupo);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, idGrupo);

            using(IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCongestionConfigDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);                                        

                    entitys.Add(entity);
                
                }
            }

            return entitys;
        }
    }
}
