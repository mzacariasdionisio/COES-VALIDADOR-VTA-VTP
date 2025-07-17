using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class TipoContratoRespository : RepositoryBase, ITipoContratoRepository
    {
        public TipoContratoRespository(string strConn)
            : base(strConn)
        {
        }

        TipoContratoHelper helper = new TipoContratoHelper();

        public int Save(TipoContratoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.TIPOCONTCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.TIPOCONTNOMBRE, DbType.String, entity.TipoContNombre);
            dbProvider.AddInParameter(command, helper.TIPOCONTESTADO, DbType.String, entity.TipoContEstado);
            dbProvider.AddInParameter(command, helper.TIPOCONTUSERNAME, DbType.String, entity.TipoContUserName);
            dbProvider.AddInParameter(command, helper.TIPOCONTFECINS, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TipoContratoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.TIPOCONTNOMBRE, DbType.String, entity.TipoContNombre);
            dbProvider.AddInParameter(command, helper.TIPOCONTESTADO, DbType.String, entity.TipoContEstado);
            dbProvider.AddInParameter(command, helper.TIPOCONTFECACT, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.TIPOCONTCODI, DbType.Int32, entity.TipoContCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.TIPOCONTCODI, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public TipoContratoDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.TIPOCONTCODI, DbType.Int32, id);
            TipoContratoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TipoContratoDTO> List()
        {
            List<TipoContratoDTO> entitys = new List<TipoContratoDTO>();
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

        public List<TipoContratoDTO> GetByCriteria(string nombre)
        {
            List<TipoContratoDTO> entitys = new List<TipoContratoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.TIPOCONTNOMBRE, DbType.String, nombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public TipoContratoDTO GetByNombre(string nombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByNombre);

            dbProvider.AddInParameter(command, helper.TIPOCONTNOMBRE, DbType.String, nombre);
            TipoContratoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
