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
    /// Clase de acceso a datos de la tabla EVE_INFORME_FILE
    /// </summary>
    public class EveInformeFileRepository: RepositoryBase, IEveInformeFileRepository
    {
        public EveInformeFileRepository(string strConn): base(strConn)
        {
        }

        EveInformeFileHelper helper = new EveInformeFileHelper();

        public int Save(EveInformeFileDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eveninffilecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, entity.Eveninfcodi);
            dbProvider.AddInParameter(command, helper.Desfile, DbType.String, entity.Desfile);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);            
            dbProvider.AddInParameter(command, helper.Extfile, DbType.String, entity.Extfile);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(int idFile, string descripcion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Desfile, DbType.String, descripcion);
            dbProvider.AddInParameter(command, helper.Eveninffilecodi, DbType.Int32, idFile);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int eveninffilecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Eveninffilecodi, DbType.Int32, eveninffilecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveInformeFileDTO GetById(int eveninffilecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Eveninffilecodi, DbType.Int32, eveninffilecodi);
            EveInformeFileDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveInformeFileDTO> List()
        {
            List<EveInformeFileDTO> entitys = new List<EveInformeFileDTO>();
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

        public List<EveInformeFileDTO> GetByCriteria()
        {
            List<EveInformeFileDTO> entitys = new List<EveInformeFileDTO>();
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

        public List<EveInformeFileDTO> ObtenerFilesInformeEvento(int idInforme)
        {
            List<EveInformeFileDTO> entitys = new List<EveInformeFileDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerFileInforme);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, idInforme);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;        
        }
    }
}
