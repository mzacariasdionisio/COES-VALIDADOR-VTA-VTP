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
    /// Clase de acceso a datos de la tabla EVE_INFORME
    /// </summary>
    public class EveInformeRepository: RepositoryBase, IEveInformeRepository
    {
        public EveInformeRepository(string strConn): base(strConn)
        {
        }

        EveInformeHelper helper = new EveInformeHelper();

        public int Save(EveInformeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Infestado, DbType.String, entity.Infestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Infversion, DbType.String, entity.Infversion);
            dbProvider.AddInParameter(command, helper.Indestado, DbType.String, entity.Indestado);
            dbProvider.AddInParameter(command, helper.Indplazo, DbType.String, entity.Indplazo);


            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveInformeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);            
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Infestado, DbType.String, entity.Infestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Infversion, DbType.String, entity.Infversion);
            dbProvider.AddInParameter(command, helper.Indestado, DbType.String, entity.Indestado);
            dbProvider.AddInParameter(command, helper.Indplazo, DbType.String, entity.Indplazo);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, entity.Eveninfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void FinalizarInforme(int idInforme, string indPlazo, string estado, string username) 
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlFinalizarInforme);

            dbProvider.AddInParameter(command, helper.Indplazo, DbType.String, indPlazo);
            dbProvider.AddInParameter(command, helper.Infestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, idInforme);

            dbProvider.ExecuteNonQuery(command);
        }

        public void RevisarInforme(int idInforme, string estado, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRevisarInforme);

            dbProvider.AddInParameter(command, helper.Infestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Lastuserrev, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, idInforme);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int eveninfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, eveninfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveInformeDTO GetById(int eveninfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, eveninfcodi);
            EveInformeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public EveInformeDTO ObtenerInformePorTipo(int idEvento, int idEmpresa, string tipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerInformePorTipo);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);
            dbProvider.AddInParameter(command, helper.Infversion, DbType.String, tipo);

            EveInformeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveInformeDTO> List()
        {
            List<EveInformeDTO> entitys = new List<EveInformeDTO>();
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

        public List<EveInformeDTO> GetByCriteria()
        {
            List<EveInformeDTO> entitys = new List<EveInformeDTO>();
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

        public List<EveInformeDTO> ObtenerInformesEmpresa(int idEvento, int idEmpresa)
        {
            List<EveInformeDTO> entitys = new List<EveInformeDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerInformeEmpresa);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ObtenerEquiposSeleccion(int idEmpresa)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string sql = String.Format(helper.SqlListarEquiposInforme, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            EqEquipoHelper equipoHelper = new EqEquipoHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();
                    
                    int iTareaAbrev = dr.GetOrdinal(equipoHelper.TAREAABREV);
                    if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    int iAreaNomb = dr.GetOrdinal(equipoHelper.AREANOMB);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AREANOMB = dr.GetString(iAreaNomb);

                    int iFamAbrev = dr.GetOrdinal(equipoHelper.FAMABREV);
                    if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    int iDesCentral = dr.GetOrdinal(equipoHelper.DESCENTRAL);
                    if (!dr.IsDBNull(iDesCentral)) entity.DESCENTRAL = dr.GetString(iDesCentral);

                    int iEquiAbrev = dr.GetOrdinal(equipoHelper.Equiabrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.Equiabrev = dr.GetString(iEquiAbrev);

                    int iEquiCodi = dr.GetOrdinal(equipoHelper.Equicodi);
                    if (!dr.IsDBNull(iEquiCodi)) entity.Equicodi = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    int iEquiNomb = dr.GetOrdinal(equipoHelper.Equinomb);
                    if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

                    int iFamCodi = dr.GetOrdinal(equipoHelper.Famcodi);
                    if (!dr.IsDBNull(iFamCodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveInformeDTO> ObtenerReporteEmpresaGeneral(int idEvento)
        {
            List<EveInformeDTO> entitys = new List<EveInformeDTO>();
            string sql = string.Format(helper.SqlObtenerReporteEmpresaGeneral, idEvento);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformeDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveInformeDTO> ObtenerReporteEmpresa(int idEvento, string empresas)
        {
            List<EveInformeDTO> entitys = new List<EveInformeDTO>();
            string sql = String.Format(helper.SqlObtenerReporteEmpresa, idEvento, empresas);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformeDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveInformeDTO> ObtenerEstadoReporte(int idEvento, string empresas)
        {
            List<EveInformeDTO> entitys = new List<EveInformeDTO>();
            string sql = String.Format(helper.SqlObtenerEstadoReporte, idEvento, empresas);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);    

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformeDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveInformeDTO> ObtenerEmpresaInforme(string empresas)
        {
            List<EveInformeDTO> entitys = new List<EveInformeDTO>();
            string sql = String.Format(helper.SqlObtenerEmpresaInforme, empresas);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);                        

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformeDTO entity = new EveInformeDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }
    }
}
