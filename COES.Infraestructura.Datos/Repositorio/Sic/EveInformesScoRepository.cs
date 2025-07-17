using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Framework.Base.Tools;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class EveInformesScoRepository : RepositoryBase, IEveInformesScoRepository
    {
        private string strConexion;
        EveInformesScoHelper helper = new EveInformesScoHelper();

        public EveInformesScoRepository(string strConn)
            : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        public int Save(EveInformesScoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Eveinfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Env_Evencodi, DbType.Int32, entity.Env_Evencodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Eveinfrutaarchivo, DbType.String, entity.Eveinfrutaarchivo);
            dbProvider.AddInParameter(command, helper.Eveinfactivo, DbType.String, entity.Eveinfactivo);

            dbProvider.AddInParameter(command, helper.Anio, DbType.String, entity.Anio);
            dbProvider.AddInParameter(command, helper.Semestre, DbType.String, entity.Semestre);
            dbProvider.AddInParameter(command, helper.Diames, DbType.String, entity.Diames);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public List<EveInformesScoDTO> List(int Evencodi, int Envetapainforme)
        {
            List<EveInformesScoDTO> entitys = new List<EveInformesScoDTO>();
            String query = String.Format(helper.SqlList, Evencodi, Envetapainforme);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveInformesScoDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iCumplimiento = dr.GetOrdinal(helper.Cumplimiento);
                    if (!dr.IsDBNull(iCumplimiento)) entity.Cumplimiento = dr.GetString(iCumplimiento);

                    entitys.Add(entity);
                    
                }
            }

            return entitys;
        }
        public List<EveInformesScoDTO> ListInformesSco(int evencodi, int envetapainforme)
        {
            List<EveInformesScoDTO> entitys = new List<EveInformesScoDTO>();
            String query = String.Format(helper.SqlListInformesSco, evencodi, envetapainforme);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveInformesScoDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iVersion = dr.GetOrdinal(helper.Version);
                    if (!dr.IsDBNull(iVersion)) entity.Version = dr.GetString(iVersion);

                    int iCumplimiento = dr.GetOrdinal(helper.Cumplimiento);
                    if (!dr.IsDBNull(iCumplimiento)) entity.Cumplimiento = dr.GetString(iCumplimiento);

                    int iPortalWeb = dr.GetOrdinal(helper.Portalweb);
                    if (!dr.IsDBNull(iPortalWeb)) entity.Portalweb = dr.GetString(iPortalWeb);

                    int iAfiversion = dr.GetOrdinal(helper.Afiversion);
                    if (!dr.IsDBNull(iAfiversion)) entity.Afiversion = dr.GetInt32(iAfiversion);

                    int iTipodata = dr.GetOrdinal(helper.Tipodata);
                    if (!dr.IsDBNull(iTipodata)) entity.Tipodata = dr.GetString(iTipodata);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iAfecodi = dr.GetOrdinal(helper.Afecodi);
                    if (!dr.IsDBNull(iAfecodi)) entity.Afecodi = dr.GetInt32(iAfecodi);

                    int iSemestre = dr.GetOrdinal(helper.Semestre);
                    if (!dr.IsDBNull(iSemestre)) entity.Semestre = dr.GetString(iSemestre);

                    int iAnio = dr.GetOrdinal(helper.Anio);
                    if (!dr.IsDBNull(iAnio)) entity.Anio = dr.GetString(iAnio);

                    int iDiames = dr.GetOrdinal(helper.Diames);
                    if (!dr.IsDBNull(iDiames)) entity.Diames = dr.GetString(iDiames);

                    int iEvencodi = dr.GetOrdinal(helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = dr.GetInt32(iEvencodi);

                    int iEveinfcodigo = dr.GetOrdinal(helper.Eveinfcodigo);
                    if (!dr.IsDBNull(iEveinfcodigo)) entity.Eveinfcodigo = dr.GetString(iEveinfcodigo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public void ActualizarInformePortalWeb(int eveinfcodi, string portalweb, string eveinfcodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarInformePortalWeb);
            dbProvider.AddInParameter(command, helper.Portalweb, DbType.String, portalweb);
            dbProvider.AddInParameter(command, helper.Eveinfcodigo, DbType.String, eveinfcodigo);
            dbProvider.AddInParameter(command, helper.Eveinfcodi, DbType.Int32, eveinfcodi);          
            dbProvider.ExecuteNonQuery(command);
        }
        public EveInformesScoDTO ObtenerInformeSco(int eveinfcodi)
        {
            EveInformesScoDTO entity = new EveInformesScoDTO();
            String query = String.Format(helper.SqlObtenerInformeSco, eveinfcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entity = helper.Create(dr);

                    int iAfecodi = dr.GetOrdinal(helper.Afecodi);
                    if (!dr.IsDBNull(iAfecodi)) entity.Afecodi = Convert.ToInt32(dr.GetValue(iAfecodi));

                    int iEveinfcodi = dr.GetOrdinal(helper.Eveinfcodi);
                    if (!dr.IsDBNull(iEveinfcodi)) entity.Eveinfcodi = Convert.ToInt32(dr.GetValue(iEveinfcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iLastuser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iEveinfrutaarchivo = dr.GetOrdinal(helper.Eveinfrutaarchivo);
                    if (!dr.IsDBNull(iEveinfrutaarchivo)) entity.Eveinfrutaarchivo = dr.GetString(iEveinfrutaarchivo);

                    int iAfiversion = dr.GetOrdinal(helper.Afiversion);
                    if (!dr.IsDBNull(iAfiversion)) entity.Afiversion = Convert.ToInt32(dr.GetValue(iAfiversion));

                    int iCumplimiento = dr.GetOrdinal(helper.Cumplimiento);
                    if (!dr.IsDBNull(iCumplimiento)) entity.Cumplimiento = dr.GetString(iCumplimiento);

                    int iAnio = dr.GetOrdinal(helper.Anio);
                    if (!dr.IsDBNull(iAnio)) entity.Anio = dr.GetString(iAnio);

                    int iSemestre = dr.GetOrdinal(helper.Semestre);
                    if (!dr.IsDBNull(iSemestre)) entity.Semestre = dr.GetString(iSemestre);

                    int iDiames = dr.GetOrdinal(helper.Diames);
                    if (!dr.IsDBNull(iDiames)) entity.Diames = dr.GetString(iDiames);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEveninffalla = dr.GetOrdinal(helper.Eveninffalla);
                    if (!dr.IsDBNull(iEveninffalla)) entity.Eveninffalla = dr.GetString(iEveninffalla);

                    int iEveninffallan2 = dr.GetOrdinal(helper.Eveninffallan2);
                    if (!dr.IsDBNull(iEveninffallan2)) entity.Eveninffallan2 = dr.GetString(iEveninffallan2);

                    int iEvencodi = dr.GetOrdinal(helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iEnv_Evencodi = dr.GetOrdinal(helper.Env_Evencodi);
                    if (!dr.IsDBNull(iEnv_Evencodi)) entity.Env_Evencodi = Convert.ToInt32(dr.GetValue(iEnv_Evencodi));

                }
            }

            return entity;
        }
    }
}
