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
    public class CrEmpresaResponsableRepository : RepositoryBase, ICrEmpresaResponsableRepository
    {
        public CrEmpresaResponsableRepository(string strConn) : base(strConn)
        {
        }
        CrEmpresaResponsableHelper helper = new CrEmpresaResponsableHelper();

        public List<CrEmpresaResponsableDTO> ListrEmpresaResponsable(int cretapacodi)
        {
            List<CrEmpresaResponsableDTO> entitys = new List<CrEmpresaResponsableDTO>();

            string query = string.Format(helper.ObtenerResponsableEtapa, cretapacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CrEmpresaResponsableDTO entity = helper.Create(dr);
                    int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EMPRNOMB = dr.GetString(iEMPRNOMB);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(CrEmpresaResponsableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crrespemprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EMPRCODI);
            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, entity.CRETAPACODI);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);

        }

        public bool ValidarEmpresaResponsable(int cretapacodi, int emprcodi)
        {
            int resul = 0;
            bool respuesta = false;
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.ValidarEmpresaResponsable, cretapacodi, emprcodi));
            resul = int.Parse(dbProvider.ExecuteScalar(command).ToString());
            if (resul > 0)
                respuesta = true;
            return respuesta;
        }

        public void Delete(int crrespemprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Crrespemprcodi, DbType.Int32, crrespemprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeletexEtapa(int cretapacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteEtapa);
            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, cretapacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<CrEmpresaResponsableDTO> SqlObtenerEmpresaResponsablexEvento(int CREVENCODI)
        {
            List<CrEmpresaResponsableDTO> entitys = new List<CrEmpresaResponsableDTO>();
            try
            {
                string query = string.Format(helper.SqlObtenerEmpresaResponsablexevento, CREVENCODI); //1
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        CrEmpresaResponsableDTO entity = helper.Create(dr);
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
