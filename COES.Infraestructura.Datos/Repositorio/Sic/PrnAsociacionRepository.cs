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
    public class PrnAsociacionRepository : RepositoryBase, IPrnAsociacionRepository
    {
        public PrnAsociacionRepository(string strConn)
        : base(strConn)
        {
        }

        PrnAsociacionHelper helper = new PrnAsociacionHelper();

        public List<PrnAsociacionDTO> List()
        {
            List<PrnAsociacionDTO> entitys = new List<PrnAsociacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnAsociacionDTO entity = new PrnAsociacionDTO();

                    int iAsociacodi = dr.GetOrdinal(helper.Asociacodi);
                    if (!dr.IsDBNull(iAsociacodi)) entity.Asociacodi = Convert.ToInt32(dr.GetValue(iAsociacodi));

                    int iAsocianom = dr.GetOrdinal(helper.Asocianom);
                    if (!dr.IsDBNull(iAsocianom)) entity.Asocianom = dr.GetString(iAsocianom);

                    int iAsociatipomedi = dr.GetOrdinal(helper.Asociatipomedi);
                    if (!dr.IsDBNull(iAsociatipomedi)) entity.Asociatipomedi = dr.GetString(iAsociatipomedi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public void Save(PrnAsociacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Asociacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Asocianom, DbType.String, entity.Asocianom);
            dbProvider.AddInParameter(command, helper.Asociatipomedi, DbType.String, entity.Asociatipomedi);
            dbProvider.ExecuteNonQuery(command);
        }
        public void Update(PrnAsociacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Asociacodi, DbType.Int32, entity.Asociacodi);
            dbProvider.AddInParameter(command, helper.Asocianom, DbType.String, entity.Asocianom);
            dbProvider.AddInParameter(command, helper.Asociatipomedi, DbType.String, entity.Asociatipomedi);
            dbProvider.ExecuteNonQuery(command);
        }
        public PrnAsociacionDTO GetById(int codigo)
        {
            PrnAsociacionDTO entity = new PrnAsociacionDTO();

            string query = string.Format(helper.SqlGetById, codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        public void Delete(int codigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Asociacodi, DbType.Int32, codigo);
            dbProvider.ExecuteNonQuery(command);
        }
        public List<PrnAsociacionDTO> ListUnidadAgrupadaByTipo(string tipo)
        {

            List<PrnAsociacionDTO> entitys = new List<PrnAsociacionDTO>();
            string query = string.Format(helper.SqlListUnidadAgrupadaByTipo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnAsociacionDTO entity = new PrnAsociacionDTO();

                    int iAsociacodi = dr.GetOrdinal(helper.Asociacodi);
                    if (!dr.IsDBNull(iAsociacodi)) entity.Asociacodi = Convert.ToInt32(dr.GetValue(iAsociacodi));

                    int iAsocianom = dr.GetOrdinal(helper.Asocianom);
                    if (!dr.IsDBNull(iAsocianom)) entity.Asocianom = dr.GetString(iAsocianom);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<PrnAsociacionDTO> ListUnidadByAgrupacion(int asociacodi)
        {

            List<PrnAsociacionDTO> entitys = new List<PrnAsociacionDTO>();
            string query = string.Format(helper.SqlListUnidadByAgrupacion, asociacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnAsociacionDTO entity = new PrnAsociacionDTO();

                    int iAsociacodi = dr.GetOrdinal(helper.Asociacodi);
                    if (!dr.IsDBNull(iAsociacodi)) entity.Asociacodi = Convert.ToInt32(dr.GetValue(iAsociacodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public void DeleteAsociacionDetalleByTipo(string tipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAsociacionDetalleByTipo);

            dbProvider.AddInParameter(command, helper.Asodettipomedi, DbType.String, tipo);
            dbProvider.ExecuteNonQuery(command);
        }
        public void DeleteAsociacionByTipo(string tipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAsociacionByTipo);

            dbProvider.AddInParameter(command, helper.Asodettipomedi, DbType.String, tipo);
            dbProvider.ExecuteNonQuery(command);
        }
        public List<PrnAsociacionDTO> ListAsociacionDetalleByTipo(string tipo)
        {

            List<PrnAsociacionDTO> entitys = new List<PrnAsociacionDTO>();
            string query = string.Format(helper.SqlListAsociacionDetalleByTipo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnAsociacionDTO entity = new PrnAsociacionDTO();

                    int iAsodetcodi = dr.GetOrdinal(helper.Asodetcodi);
                    if (!dr.IsDBNull(iAsodetcodi)) entity.Asodetcodi = Convert.ToInt32(dr.GetValue(iAsodetcodi));

                    int iAsociacodi = dr.GetOrdinal(helper.Asociacodi);
                    if (!dr.IsDBNull(iAsociacodi)) entity.Asociacodi = Convert.ToInt32(dr.GetValue(iAsociacodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public int SaveReturnId(PrnAsociacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Asociacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Asocianom, DbType.String, entity.Asocianom);
            dbProvider.AddInParameter(command, helper.Asociatipomedi, DbType.String, entity.Asociatipomedi);
            dbProvider.ExecuteNonQuery(command);

            return id;
        }
        public void SaveDetalle(PrnAsociacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdDetalle);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveDetalle);
            dbProvider.AddInParameter(command, helper.Asociacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Asociacodi, DbType.Int32, entity.Asociacodi);
            dbProvider.AddInParameter(command, helper.Asocianom, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Asodettipomedi, DbType.String, entity.Asodettipomedi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
