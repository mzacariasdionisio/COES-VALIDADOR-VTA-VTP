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
    public class DpoRelSplFormulaRepository : RepositoryBase, IDpoRelSPlFormulaRepository
    {
        public DpoRelSplFormulaRepository(string strConn) : base(strConn)
        {
        }

        DpoRelSplFormulaHelper helper = new DpoRelSplFormulaHelper();

        public void Save(DpoRelSplFormulaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Splfrmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, entity.Dposplcodi);
            dbProvider.AddInParameter(command, helper.Barsplcodi, DbType.Int32, entity.Barsplcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodifveg, DbType.Int32, entity.Ptomedicodifveg);
            dbProvider.AddInParameter(command, helper.Ptomedicodiful, DbType.Int32, entity.Ptomedicodiful);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoRelSplFormulaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Splfrmcodi, DbType.Int32, entity.Splfrmcodi);
            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, entity.Dposplcodi);
            dbProvider.AddInParameter(command, helper.Barsplcodi, DbType.Int32, entity.Barsplcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodifveg, DbType.Int32, entity.Ptomedicodifveg);
            dbProvider.AddInParameter(command, helper.Ptomedicodiful, DbType.Int32, entity.Ptomedicodiful);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateFormulas(DpoRelSplFormulaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateFormulas);

            dbProvider.AddInParameter(command, helper.Ptomedicodifveg, DbType.Int32, entity.Ptomedicodifveg);
            dbProvider.AddInParameter(command, helper.Ptomedicodiful, DbType.Int32, entity.Ptomedicodiful);
            dbProvider.AddInParameter(command, helper.Splfrmarea, DbType.Int32, entity.Splfrmarea);
            dbProvider.AddInParameter(command, helper.Splfrmcodi, DbType.Int32, entity.Splfrmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Splfrmcodi, DbType.Int32, codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoRelSplFormulaDTO> List()
        {
            List<DpoRelSplFormulaDTO> entitys = new List<DpoRelSplFormulaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoRelSplFormulaDTO entity = new DpoRelSplFormulaDTO();

                    int iSplfrmcodi = dr.GetOrdinal(helper.Splfrmcodi);
                    if (!dr.IsDBNull(iSplfrmcodi)) entity.Splfrmcodi = Convert.ToInt32(dr.GetValue(iSplfrmcodi));

                    int iDposplcodi = dr.GetOrdinal(helper.Dposplcodi);
                    if (!dr.IsDBNull(iDposplcodi)) entity.Dposplcodi = Convert.ToInt32(dr.GetValue(iDposplcodi));

                    int iBarsplcodi = dr.GetOrdinal(helper.Barsplcodi);
                    if (!dr.IsDBNull(iBarsplcodi)) entity.Barsplcodi = Convert.ToInt32(dr.GetValue(iBarsplcodi));

                    int iPtomedicodifveg = dr.GetOrdinal(helper.Ptomedicodifveg);
                    if (!dr.IsDBNull(iPtomedicodifveg)) entity.Ptomedicodifveg = Convert.ToInt32(dr.GetValue(iPtomedicodifveg));

                    int iPtomedicodiful = dr.GetOrdinal(helper.Ptomedicodiful);
                    if (!dr.IsDBNull(iPtomedicodiful)) entity.Ptomedicodiful = Convert.ToInt32(dr.GetValue(iPtomedicodiful));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<DpoRelSplFormulaDTO> ListBarrasxVersion(int version)
        {
            List<DpoRelSplFormulaDTO> entitys = new List<DpoRelSplFormulaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListBarrasxVersion);
            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoRelSplFormulaDTO entity = new DpoRelSplFormulaDTO();

                    int iDposplcodi = dr.GetOrdinal(helper.Dposplcodi);
                    if (!dr.IsDBNull(iDposplcodi)) entity.Dposplcodi = Convert.ToInt32(dr.GetValue(iDposplcodi));

                    int iDposplnombre = dr.GetOrdinal(helper.Dposplnombre);
                    if (!dr.IsDBNull(iDposplnombre)) entity.Dposplnombre = dr.GetString(iDposplnombre);

                    int iSplfrmcodi = dr.GetOrdinal(helper.Splfrmcodi);
                    if (!dr.IsDBNull(iSplfrmcodi)) entity.Splfrmcodi = Convert.ToInt32(dr.GetValue(iSplfrmcodi));

                    int iBarsplcodi = dr.GetOrdinal(helper.Barsplcodi);
                    if (!dr.IsDBNull(iBarsplcodi)) entity.Barsplcodi = Convert.ToInt32(dr.GetValue(iBarsplcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iPtomedicodifveg = dr.GetOrdinal(helper.Ptomedicodifveg);
                    if (!dr.IsDBNull(iPtomedicodifveg)) entity.Ptomedicodifveg = Convert.ToInt32(dr.GetValue(iPtomedicodifveg));

                    int iNombvegetativa = dr.GetOrdinal(helper.Nombvegetativa);
                    if (!dr.IsDBNull(iNombvegetativa)) entity.Nombvegetativa = dr.GetString(iNombvegetativa);

                    int iPtomedicodiful = dr.GetOrdinal(helper.Ptomedicodiful);
                    if (!dr.IsDBNull(iPtomedicodiful)) entity.Ptomedicodiful = Convert.ToInt32(dr.GetValue(iPtomedicodiful));

                    int iNombindustrial = dr.GetOrdinal(helper.Nombindustrial);
                    if (!dr.IsDBNull(iNombindustrial)) entity.Nombindustrial = dr.GetString(iNombindustrial);

                    int iSplfrmarea = dr.GetOrdinal(helper.Splfrmarea);
                    if (!dr.IsDBNull(iSplfrmarea)) entity.Splfrmarea = Convert.ToInt32(dr.GetValue(iSplfrmarea));

                    int iSplareanombre = dr.GetOrdinal(helper.Splareanombre);
                    if (!dr.IsDBNull(iSplareanombre)) entity.Splareanombre = dr.GetString(iSplareanombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public DpoRelSplFormulaDTO GetById(int codi)
        {
            DpoRelSplFormulaDTO entity = new DpoRelSplFormulaDTO();

            string query = string.Format(helper.SqlGetById, codi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DpoRelSplFormulaDTO> ListFormulasVegetativa()
        {
            List<DpoRelSplFormulaDTO> entitys = new List<DpoRelSplFormulaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFormulasVegetativa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoRelSplFormulaDTO entity = new DpoRelSplFormulaDTO();

                    int iPtomedicodifveg = dr.GetOrdinal(helper.Ptomedicodifveg);
                    if (!dr.IsDBNull(iPtomedicodifveg)) entity.Ptomedicodifveg = Convert.ToInt32(dr.GetValue(iPtomedicodifveg));

                    int iNombvegetativa = dr.GetOrdinal(helper.Nombvegetativa);
                    if (!dr.IsDBNull(iNombvegetativa)) entity.Nombvegetativa = dr.GetString(iNombvegetativa);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoRelSplFormulaDTO> ListFormulasIndustrial()
        {
            List<DpoRelSplFormulaDTO> entitys = new List<DpoRelSplFormulaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFormulasIndustrial);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoRelSplFormulaDTO entity = new DpoRelSplFormulaDTO();

                    int iPtomedicodiful = dr.GetOrdinal(helper.Ptomedicodiful);
                    if (!dr.IsDBNull(iPtomedicodiful)) entity.Ptomedicodiful = Convert.ToInt32(dr.GetValue(iPtomedicodiful));

                    int iNombindustrial = dr.GetOrdinal(helper.Nombindustrial);
                    if (!dr.IsDBNull(iNombindustrial)) entity.Nombindustrial = dr.GetString(iNombindustrial);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeleteByVersion(int codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByVersion);

            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, codi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByVersionxBarra(int version, int barra)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByVersionxBarra);

            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.Barsplcodi, DbType.Int32, barra);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
