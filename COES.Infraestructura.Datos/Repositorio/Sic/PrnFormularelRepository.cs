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
    public class PrnFormularelRepository : RepositoryBase, IPrnFormularelRepository
    {
        public PrnFormularelRepository(string strConn) : base(strConn)
        {
        }

        PrnFormularelHelper helper = new PrnFormularelHelper();

        public void Save(PrnFormularelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Prfrelcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodicalc, DbType.Int32, entity.Ptomedicodicalc);
            dbProvider.AddInParameter(command, helper.Prfrelfactor, DbType.Int32, entity.Prfrelfactor);
            dbProvider.AddInParameter(command, helper.Prfrelusucreacion, DbType.String, entity.Prfrelusucreacion);
            dbProvider.AddInParameter(command, helper.Prfrelfeccreacion, DbType.DateTime, entity.Prfrelfeccreacion);
            dbProvider.AddInParameter(command, helper.Prfrelusumodificacion, DbType.String, entity.Prfrelusumodificacion);
            dbProvider.AddInParameter(command, helper.Prfrelfecmodificacion, DbType.DateTime, entity.Prfrelfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnFormularelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Prfrelfactor, DbType.Int32, entity.Prfrelfactor);
            dbProvider.AddInParameter(command, helper.Prfrelusumodificacion, DbType.String, entity.Prfrelusumodificacion);
            dbProvider.AddInParameter(command, helper.Prfrelfecmodificacion, DbType.DateTime, entity.Prfrelfecmodificacion);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodicalc, DbType.Int32, entity.Ptomedicodicalc);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptomedicodi, int ptomedicodicalc)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodicalc, DbType.Int32, ptomedicodicalc);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnFormularelDTO> List()
        {
            List<PrnFormularelDTO> entitys = new List<PrnFormularelDTO>();
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

        public PrnFormularelDTO GetById(int ptomedicodi, int ptomedicodicalc)
        {
            PrnFormularelDTO entity = new PrnFormularelDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodicalc, DbType.Int32, ptomedicodicalc);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrnFormularelDTO> ListFormulasByUsuario(string usuario)
        {
            PrnFormularelDTO entity = new PrnFormularelDTO();
            List<PrnFormularelDTO> entitys = new List<PrnFormularelDTO>();
            string queryString = string.Format(helper.SqlListFormulasByUsuario, usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnFormularelDTO();

                    int iPtomedicodicalc = dr.GetOrdinal(helper.Ptomedicodicalc);
                    if (!dr.IsDBNull(iPtomedicodicalc)) entity.Ptomedicodicalc = Convert.ToInt32(dr.GetValue(iPtomedicodicalc));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entity.Prnselect = -1;
                    entity.Prfrelfactor = -1;//Campo que define el Defecto

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnFormularelDTO> ListFormulasRelacionadas(int ptomedicodi, string usuario)
        {
            PrnFormularelDTO entity = new PrnFormularelDTO();
            List<PrnFormularelDTO> entitys = new List<PrnFormularelDTO>();
            string queryString = string.Format(helper.SqlListFormulasRelacionadas, ptomedicodi, usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnFormularelDTO();

                    int iPtomedicodicalc = dr.GetOrdinal(helper.Ptomedicodicalc);
                    if (!dr.IsDBNull(iPtomedicodicalc)) entity.Ptomedicodicalc = Convert.ToInt32(dr.GetValue(iPtomedicodicalc));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPrfrelfactor = dr.GetOrdinal(helper.Prfrelfactor);
                    if (!dr.IsDBNull(iPrfrelfactor)) entity.Prfrelfactor = Convert.ToInt32(dr.GetValue(iPrfrelfactor));

                    entity.Prnselect = 1;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeleteByPtomedicodi(int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByPtomedicodi);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
