using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class CrEmpresaSolicitanteRepository : RepositoryBase, ICrEmpresaSolicitanteRepository
    {
        public CrEmpresaSolicitanteRepository(string strConn) : base(strConn)
        {
        }
        CrEmpresaSolicitanteHelper helper = new CrEmpresaSolicitanteHelper();
        public void Delete(int crsolemprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Crsolemprcodi, DbType.Int32, crsolemprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CrEmpresaSolicitanteDTO> ListrEmpresaSolicitante(int cretapacodi)
        {
            List<CrEmpresaSolicitanteDTO> entitys = new List<CrEmpresaSolicitanteDTO>();

            string query = string.Format(helper.ObtenerSolicitanteEtapa, cretapacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CrEmpresaSolicitanteDTO entity = helper.Create(dr);
                    int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(CrEmpresaSolicitanteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crsolemprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EMPRCODI);
            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, entity.CRETAPACODI);
            dbProvider.AddInParameter(command, helper.Crargumento, DbType.String, entity.CRARGUMENTO);
            dbProvider.AddInParameter(command, helper.Crdecision, DbType.String, entity.CRDECISION);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);
        }

        public bool ValidarEmpresaSolicitante(int cretapacodi, int emprcodi)
        {
            int resul = 0;
            bool respuesta = false;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.ValidarEmpresaSolicitante, cretapacodi, emprcodi));
            resul = int.Parse(dbProvider.ExecuteScalar(command).ToString());
            if (resul > 0)
                respuesta = true;
            return respuesta;
        }

        public CrEmpresaSolicitanteDTO ObtenerEmpresaSolicitante(int crsolemprcodi)
        {
            String query = String.Format(helper.SqlGetById, crsolemprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CrEmpresaSolicitanteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Update(int crsolemprcodi, string argumentos, string desicion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);          
            dbProvider.AddInParameter(command, helper.Crargumento, DbType.String, argumentos);
            dbProvider.AddInParameter(command, helper.Crdecision, DbType.String, desicion);
            dbProvider.AddInParameter(command, helper.Crsolemprcodi, DbType.Int32, crsolemprcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteSolicitantexEtapa(int cretapacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteSolicitantexEtapa);
            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, cretapacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<CrEmpresaSolicitanteDTO> SqlObtenerEmpresaSolicitantexEvento(int CREVENCODI)
        {
            List<CrEmpresaSolicitanteDTO> entitys = new List<CrEmpresaSolicitanteDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaSolicitantexEvento, CREVENCODI);
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        CrEmpresaSolicitanteDTO entity = helper.Create(dr);
                        int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                entitys = null;
            }
            return entitys;
        }
    }
}
