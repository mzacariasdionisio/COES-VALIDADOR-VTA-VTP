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
    /// Clase de acceso a datos de la tabla WB_COMUNICADOS
    /// </summary>
    public class WbComunicadosRepository: RepositoryBase, IWbComunicadosRepository
    {
        public WbComunicadosRepository(string strConn): base(strConn)
        {
        }

        WbComunicadosHelper helper = new WbComunicadosHelper();

        public int Save(WbComunicadosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Comcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Comfecha, DbType.DateTime, entity.Comfecha);
            dbProvider.AddInParameter(command, helper.Comtitulo, DbType.String, entity.Comtitulo);
            dbProvider.AddInParameter(command, helper.Comresumen, DbType.String, entity.Comresumen);
            dbProvider.AddInParameter(command, helper.Comdesc, DbType.String, entity.Comdesc);
            dbProvider.AddInParameter(command, helper.Comlink, DbType.String, entity.Comlink);
            dbProvider.AddInParameter(command, helper.Comestado, DbType.String, entity.Comestado);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Comfechaini, DbType.DateTime, entity.Comfechaini);
            dbProvider.AddInParameter(command, helper.Comfechafin, DbType.DateTime, entity.Comfechafin);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Comorden, DbType.Int32, entity.Comorden);
            dbProvider.AddInParameter(command, helper.Composition, DbType.Int32, entity.Composition);
            dbProvider.AddInParameter(command, helper.Comtipo, DbType.String, entity.Comtipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbComunicadosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Comfecha, DbType.DateTime, entity.Comfecha);
            dbProvider.AddInParameter(command, helper.Comtitulo, DbType.String, entity.Comtitulo);
            dbProvider.AddInParameter(command, helper.Comresumen, DbType.String, entity.Comresumen);
            dbProvider.AddInParameter(command, helper.Comdesc, DbType.String, entity.Comdesc);
            dbProvider.AddInParameter(command, helper.Comlink, DbType.String, entity.Comlink);
            dbProvider.AddInParameter(command, helper.Comestado, DbType.String, entity.Comestado);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Comfechaini, DbType.DateTime, entity.Comfechaini);
            dbProvider.AddInParameter(command, helper.Comfechafin, DbType.DateTime, entity.Comfechafin);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Comorden, DbType.Int32, entity.Comorden);
            dbProvider.AddInParameter(command, helper.Composition, DbType.Int32, entity.Composition);
            dbProvider.AddInParameter(command, helper.Comcodi, DbType.Int32, entity.Comcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int comcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Comcodi, DbType.Int32, comcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbComunicadosDTO GetById(int comcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Comcodi, DbType.Int32, comcodi);
            WbComunicadosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbComunicadosDTO> List()
        {
            List<WbComunicadosDTO> entitys = new List<WbComunicadosDTO>();
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

        public List<WbComunicadosDTO> GetByCriteria()
        {
            List<WbComunicadosDTO> entitys = new List<WbComunicadosDTO>();
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
