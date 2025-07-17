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
    public class PrnRelacionTnaRepository : RepositoryBase, IPrnRelacionTnaRepository
    {
        public PrnRelacionTnaRepository(string strConn)
         : base(strConn)
        {
        }

        PrnRelacionTnaHelper helper = new PrnRelacionTnaHelper();

        public List<PrnRelacionTnaDTO> List()
        {
            List<PrnRelacionTnaDTO> entitys = new List<PrnRelacionTnaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnRelacionTnaDTO entity = new PrnRelacionTnaDTO();

                    int iReltnacodi = dr.GetOrdinal(helper.Reltnacodi);
                    if (!dr.IsDBNull(iReltnacodi)) entity.Reltnacodi = Convert.ToInt32(dr.GetValue(iReltnacodi));

                    int iReltnaformula = dr.GetOrdinal(helper.Reltnaformula);
                    if (!dr.IsDBNull(iReltnaformula)) entity.Reltnaformula = Convert.ToInt32(dr.GetValue(iReltnaformula));

                    int iReltnanom = dr.GetOrdinal(helper.Reltnanom);
                    if (!dr.IsDBNull(iReltnanom)) entity.Reltnanom = dr.GetString(iReltnanom);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(PrnRelacionTnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reltnaformula, DbType.Int32, entity.Reltnaformula);
            dbProvider.AddInParameter(command, helper.Reltnanom, DbType.String, entity.Reltnanom);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnRelacionTnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);            
            dbProvider.AddInParameter(command, helper.Reltnaformula, DbType.Int32, entity.Reltnaformula);
            dbProvider.AddInParameter(command, helper.Reltnanom, DbType.String, entity.Reltnanom);
            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, entity.Reltnacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public PrnRelacionTnaDTO GetById(int codigo)
        {
            PrnRelacionTnaDTO entity = new PrnRelacionTnaDTO();

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

            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, codigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnRelacionTnaDTO> ListRelacionTnaDetalleById(int reltnacodi)
        {

            List<PrnRelacionTnaDTO> entitys = new List<PrnRelacionTnaDTO>();
            string query = string.Format(helper.SqlListRelacionTnaDetalleById, reltnacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnRelacionTnaDTO entity = new PrnRelacionTnaDTO();

                    int iReltnadetcodi = dr.GetOrdinal(helper.Reltnadetcodi);
                    if (!dr.IsDBNull(iReltnadetcodi)) entity.Reltnadetcodi = Convert.ToInt32(dr.GetValue(iReltnadetcodi));

                    int iBarracodi = dr.GetOrdinal(helper.Barracodi);
                    if (!dr.IsDBNull(iBarracodi)) entity.Barracodi = Convert.ToInt32(dr.GetValue(iBarracodi));

                    int iBarranom = dr.GetOrdinal(helper.Barranom);
                    if (!dr.IsDBNull(iBarranom)) entity.Barranom = dr.GetString(iBarranom);

                    int iReltnadetformula = dr.GetOrdinal(helper.Reltnadetformula);
                    if (!dr.IsDBNull(iReltnadetformula)) entity.Reltnadetformula = Convert.ToInt32(dr.GetValue(iReltnadetformula));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnRelacionTnaDTO> ListRelacionTnaDetalle()
        {

            List<PrnRelacionTnaDTO> entitys = new List<PrnRelacionTnaDTO>();
            string query = string.Format(helper.SqlListRelacionTnaDetalle);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnRelacionTnaDTO entity = new PrnRelacionTnaDTO();

                    int iReltnadetcodi = dr.GetOrdinal(helper.Reltnadetcodi);
                    if (!dr.IsDBNull(iReltnadetcodi)) entity.Reltnadetcodi = Convert.ToInt32(dr.GetValue(iReltnadetcodi));

                    int iBarracodi = dr.GetOrdinal(helper.Barracodi);
                    if (!dr.IsDBNull(iBarracodi)) entity.Barracodi = Convert.ToInt32(dr.GetValue(iBarracodi));

                    int iBarranom = dr.GetOrdinal(helper.Barranom);
                    if (!dr.IsDBNull(iBarranom)) entity.Barranom = dr.GetString(iBarranom);

                    int iReltnadetformula = dr.GetOrdinal(helper.Reltnadetformula);
                    if (!dr.IsDBNull(iReltnadetformula)) entity.Reltnadetformula = Convert.ToInt32(dr.GetValue(iReltnadetformula));

                    int iFormulanomb = dr.GetOrdinal(helper.Formulanomb);
                    if (!dr.IsDBNull(iFormulanomb)) entity.Formulanomb = dr.GetString(iFormulanomb);

                    int iReltnacodi = dr.GetOrdinal(helper.Reltnacodi);
                    if (!dr.IsDBNull(iReltnacodi)) entity.Reltnacodi = Convert.ToInt32(dr.GetValue(iReltnacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int SaveGetId(PrnRelacionTnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reltnaformula, DbType.Int32, entity.Reltnaformula);
            dbProvider.AddInParameter(command, helper.Reltnanom, DbType.String, entity.Reltnanom);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        #region Métodos de la tabla detalle PRN_RELACIONTNADETALLE
        public void SavePrnRelacionTnaDetalle(PrnRelacionTnaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdRelacionTnaDetalle);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveRelacionTnaDetalle);
            dbProvider.AddInParameter(command, helper.Reltnadetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, entity.Reltnacodi);
            dbProvider.AddInParameter(command, helper.Barracodi, DbType.Int32, entity.Barracodi);
            dbProvider.AddInParameter(command, helper.Reltnadetformula, DbType.Int32, entity.Reltnadetformula);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeletePrnRelacionTnaDetalle(int reltnacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteRelacionTnaDetalle);
            dbProvider.AddInParameter(command, helper.Reltnacodi, DbType.Int32, reltnacodi);
            dbProvider.ExecuteNonQuery(command);
        }
        #endregion

    }
}
