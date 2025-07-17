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
    /// Clase de acceso a datos de la tabla INT_DIRECTORIO
    /// </summary>
    public class IntDirectorioRepository: RepositoryBase, IIntDirectorioRepository
    {
        public IntDirectorioRepository(string strConn): base(strConn)
        {
        }

        IntDirectorioHelper helper = new IntDirectorioHelper();

        public int Save(IntDirectorioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dircodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dirnombre, DbType.String, entity.Dirnombre);
            dbProvider.AddInParameter(command, helper.Dirapellido, DbType.String, entity.Dirapellido);
            dbProvider.AddInParameter(command, helper.Dircorreo, DbType.String, entity.Dircorreo);
            dbProvider.AddInParameter(command, helper.Diranexo, DbType.String, entity.Diranexo);
            dbProvider.AddInParameter(command, helper.Dirtelefono, DbType.String, entity.Dirtelefono);
            dbProvider.AddInParameter(command, helper.Dirfuncion, DbType.String, entity.Dirfuncion);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Dircumpleanios, DbType.DateTime, entity.Dircumpleanios);
            dbProvider.AddInParameter(command, helper.Dirfoto, DbType.String, entity.Dirfoto);
            dbProvider.AddInParameter(command, helper.Direstado, DbType.String, entity.Direstado);
            dbProvider.AddInParameter(command, helper.Dirpadre, DbType.Int32, entity.Dirpadre);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Dirusucreacion, DbType.String, entity.Dirusucreacion);
            dbProvider.AddInParameter(command, helper.Dirfeccreacion, DbType.DateTime, entity.Dirfeccreacion);
            dbProvider.AddInParameter(command, helper.Dirusumodificacion, DbType.String, entity.Dirusumodificacion);
            dbProvider.AddInParameter(command, helper.Dirfecmodificacion, DbType.DateTime, entity.Dirfecmodificacion);
            dbProvider.AddInParameter(command, helper.Dircargo, DbType.String, entity.Dircargo);
            dbProvider.AddInParameter(command, helper.Dirapoyo, DbType.String, entity.Dirapoyo);
            dbProvider.AddInParameter(command, helper.Dirorganigrama, DbType.String, entity.Dirorganigrama);
            dbProvider.AddInParameter(command, helper.Dirsecretaria, DbType.String, entity.Dirsecretaria);
            dbProvider.AddInParameter(command, helper.Dirsuperiores, DbType.String, entity.Dirsuperiores);
            dbProvider.AddInParameter(command, helper.Dirindsuperior, DbType.String, entity.Dirindsuperior);
            dbProvider.AddInParameter(command, helper.Dirnivel, DbType.Int32, entity.Dirnivel);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IntDirectorioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dircodi, DbType.Int32, entity.Dircodi);
            dbProvider.AddInParameter(command, helper.Dirnombre, DbType.String, entity.Dirnombre);
            dbProvider.AddInParameter(command, helper.Dirapellido, DbType.String, entity.Dirapellido);
            dbProvider.AddInParameter(command, helper.Dircorreo, DbType.String, entity.Dircorreo);
            dbProvider.AddInParameter(command, helper.Diranexo, DbType.String, entity.Diranexo);
            dbProvider.AddInParameter(command, helper.Dirtelefono, DbType.String, entity.Dirtelefono);
            dbProvider.AddInParameter(command, helper.Dirfuncion, DbType.String, entity.Dirfuncion);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Dircumpleanios, DbType.DateTime, entity.Dircumpleanios);
            dbProvider.AddInParameter(command, helper.Dirfoto, DbType.String, entity.Dirfoto);
            dbProvider.AddInParameter(command, helper.Direstado, DbType.String, entity.Direstado);
            dbProvider.AddInParameter(command, helper.Dirpadre, DbType.Int32, entity.Dirpadre);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Dirusucreacion, DbType.String, entity.Dirusucreacion);
            dbProvider.AddInParameter(command, helper.Dirfeccreacion, DbType.DateTime, entity.Dirfeccreacion);
            dbProvider.AddInParameter(command, helper.Dirusumodificacion, DbType.String, entity.Dirusumodificacion);
            dbProvider.AddInParameter(command, helper.Dirfecmodificacion, DbType.DateTime, entity.Dirfecmodificacion);
            dbProvider.AddInParameter(command, helper.Dircargo, DbType.String, entity.Dircargo);
            dbProvider.AddInParameter(command, helper.Dirapoyo, DbType.String, entity.Dirapoyo);
            dbProvider.AddInParameter(command, helper.Dirorganigrama, DbType.String, entity.Dirorganigrama);
            dbProvider.AddInParameter(command, helper.Dirsecretaria, DbType.String, entity.Dirsecretaria);
            dbProvider.AddInParameter(command, helper.Dirsuperiores, DbType.String, entity.Dirsuperiores);
            dbProvider.AddInParameter(command, helper.Dirindsuperior, DbType.String, entity.Dirindsuperior);
            dbProvider.AddInParameter(command, helper.Dirnivel, DbType.Int32, entity.Dirnivel);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int dircodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dircodi, DbType.Int32, dircodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IntDirectorioDTO GetById(int dircodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dircodi, DbType.Int32, dircodi);
            IntDirectorioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IntDirectorioDTO> List()
        {
            List<IntDirectorioDTO> entitys = new List<IntDirectorioDTO>();
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

        public List<IntDirectorioDTO> GetByCriteria()
        {
            List<IntDirectorioDTO> entitys = new List<IntDirectorioDTO>();
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
    }
}
