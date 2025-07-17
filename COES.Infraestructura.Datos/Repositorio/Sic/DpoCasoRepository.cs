using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoCasoRepository : RepositoryBase, IDpoCasoRepository
    {
        public DpoCasoRepository(string strConn) : base(strConn)
        {
        }

        DpoCasoHelper helper = new DpoCasoHelper();


        #region Métodos Tabla DPO_CASO
        public int Save(DpoCasoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dpocsocnombre, DbType.String, entity.Dpocsocnombre);
            dbProvider.AddInParameter(command, helper.Areaabrev, DbType.String, entity.Areaabrev);
            dbProvider.AddInParameter(command, helper.Dpocsousucreacion, DbType.String, entity.Dpocsousucreacion);
            dbProvider.AddInParameter(command, helper.Dpocsofeccreacion, DbType.DateTime, entity.Dpocsofeccreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(DpoCasoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpocsocnombre, DbType.String, entity.Dpocsocnombre);
            dbProvider.AddInParameter(command, helper.Areaabrev, DbType.String, entity.Areaabrev);
            dbProvider.AddInParameter(command, helper.Dpocsousumodificacion, DbType.String, entity.Dpocsousumodificacion);
            dbProvider.AddInParameter(command, helper.Dposcofecmodificacion, DbType.DateTime, entity.Dposcofecmodificacion);
            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, entity.Dpocsocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public DpoCasoDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dpocsocodi, DbType.Int32, id);
            
            DpoCasoDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateDpoCaso(dr);
                }
            }

            return entity;
        }

        public List<DpoCasoDTO> List()
        {
            List<DpoCasoDTO> entitys = new List<DpoCasoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoCaso(dr));
                }
            }

            return entitys;
        }

        public List<DpoCasoDTO> Filter(string nombre, string areaOperativa, string usuario)
        {
            List<DpoCasoDTO> entitys = new List<DpoCasoDTO>();

            string parNombre = (nombre == "" ? "0" : nombre);
            string parAreaOperativa = (areaOperativa == "" ? "0" : areaOperativa);
            string parUsuario = (usuario == "" ? "0" : usuario);

            string query = string.Format(helper.SqlFilter, parNombre, parAreaOperativa, parUsuario);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateDpoCaso(dr));
                }
            }

            return entitys;
        }

        public List<DpoNombreCasoDTO> ListNombreCasos()
        {
            List<DpoNombreCasoDTO> entitys = new List<DpoNombreCasoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListNombreCasos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoNombreCasoDTO entity = new DpoNombreCasoDTO();

                    int iNombre = dr.GetOrdinal(helper.Nombre);
                    if (!dr.IsDBNull(iNombre)) entity.Nombre = dr.GetString(iNombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoUsuarioDTO> ListUsuarios()
        {
            List<DpoUsuarioDTO> entitys = new List<DpoUsuarioDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListUsuarios);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoUsuarioDTO entity = new DpoUsuarioDTO();

                    int iUsuario = dr.GetOrdinal(helper.Usuario);
                    if (!dr.IsDBNull(iUsuario)) entity.Usuario = dr.GetString(iUsuario);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
