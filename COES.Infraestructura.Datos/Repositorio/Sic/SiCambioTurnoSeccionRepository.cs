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
    /// Clase de acceso a datos de la tabla SI_CAMBIO_TURNO_SECCION
    /// </summary>
    public class SiCambioTurnoSeccionRepository: RepositoryBase, ISiCambioTurnoSeccionRepository
    {
        public SiCambioTurnoSeccionRepository(string strConn): base(strConn)
        {
        }

        SiCambioTurnoSeccionHelper helper = new SiCambioTurnoSeccionHelper();

        public int Save(SiCambioTurnoSeccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Seccioncodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, entity.Cambioturnocodi);
            dbProvider.AddInParameter(command, helper.Nroseccion, DbType.Int32, entity.Nroseccion);
            dbProvider.AddInParameter(command, helper.Descomentario, DbType.String, entity.Descomentario);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiCambioTurnoSeccionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, entity.Cambioturnocodi);
            dbProvider.AddInParameter(command, helper.Nroseccion, DbType.Int32, entity.Nroseccion);
            dbProvider.AddInParameter(command, helper.Descomentario, DbType.String, entity.Descomentario);
            dbProvider.AddInParameter(command, helper.Seccioncodi, DbType.Int32, entity.Seccioncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int seccioncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Seccioncodi, DbType.Int32, seccioncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiCambioTurnoSeccionDTO GetById(int seccioncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Seccioncodi, DbType.Int32, seccioncodi);
            SiCambioTurnoSeccionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiCambioTurnoSeccionDTO> List()
        {
            List<SiCambioTurnoSeccionDTO> entitys = new List<SiCambioTurnoSeccionDTO>();
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

        public List<SiCambioTurnoSeccionDTO> GetByCriteria(int id)
        {
            List<SiCambioTurnoSeccionDTO> entitys = new List<SiCambioTurnoSeccionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, id);

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
