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
    public class DpoTrafoBarraRepository : RepositoryBase, IDpoTrafoBarraRepository
    {
        public DpoTrafoBarraRepository(string strConn) : base(strConn)
        {
        }

        DpoTrafoBarraHelper helper = new DpoTrafoBarraHelper();

        public void Save(DpoTrafoBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tnfbarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tnfbartnfcodi, DbType.String, entity.Tnfbartnfcodi);
            dbProvider.AddInParameter(command, helper.Tnfbarbarcodi, DbType.String, entity.Tnfbarbarcodi);
            dbProvider.AddInParameter(command, helper.Tnfbarbarnombre, DbType.String, entity.Tnfbarbarnombre);
            dbProvider.AddInParameter(command, helper.Tnfbarbartension, DbType.Decimal, entity.Tnfbarbartension);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoTrafoBarraDTO> List()
        {
            List<DpoTrafoBarraDTO> entitys = new List<DpoTrafoBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoTrafoBarraDTO entity = new DpoTrafoBarraDTO();

                    int iTnfbarcodi = dr.GetOrdinal(helper.Tnfbarcodi);
                    if (!dr.IsDBNull(iTnfbarcodi)) entity.Tnfbarcodi = Convert.ToInt32(dr.GetValue(iTnfbarcodi));

                    int iTnfbartnfcodi = dr.GetOrdinal(helper.Tnfbartnfcodi);
                    if (!dr.IsDBNull(iTnfbartnfcodi)) entity.Tnfbartnfcodi = dr.GetString(iTnfbartnfcodi);

                    int iTnfbarbarcodi = dr.GetOrdinal(helper.Tnfbarbarcodi);
                    if (!dr.IsDBNull(iTnfbarbarcodi)) entity.Tnfbarbarcodi = dr.GetString(iTnfbarbarcodi);

                    int iTnfbarbarnombre = dr.GetOrdinal(helper.Tnfbarbarnombre);
                    if (!dr.IsDBNull(iTnfbarbarnombre)) entity.Tnfbarbarnombre = dr.GetString(iTnfbarbarnombre);

                    int iTnfbarbartension = dr.GetOrdinal(helper.Tnfbarbartension);
                    if (!dr.IsDBNull(iTnfbarbartension)) entity.Tnfbarbartension = dr.GetDecimal(iTnfbarbartension);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoTrafoBarraDTO> ListTrafoBarraById(string codigo)
        {
            List<DpoTrafoBarraDTO> entitys = new List<DpoTrafoBarraDTO>();
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTrafoBarraById);
            string query = string.Format(helper.SqlListTrafoBarraById, codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoTrafoBarraDTO entity = new DpoTrafoBarraDTO();

                    int iTnfbarcodi = dr.GetOrdinal(helper.Tnfbarcodi);
                    if (!dr.IsDBNull(iTnfbarcodi)) entity.Tnfbarcodi = Convert.ToInt32(dr.GetValue(iTnfbarcodi));

                    int iTnfbartnfcodi = dr.GetOrdinal(helper.Tnfbartnfcodi);
                    if (!dr.IsDBNull(iTnfbartnfcodi)) entity.Tnfbartnfcodi = dr.GetString(iTnfbartnfcodi);

                    int iTnfbarbarcodi = dr.GetOrdinal(helper.Tnfbarbarcodi);
                    if (!dr.IsDBNull(iTnfbarbarcodi)) entity.Tnfbarbarcodi = dr.GetString(iTnfbarbarcodi);

                    int iTnfbarbarnombre = dr.GetOrdinal(helper.Tnfbarbarnombre);
                    if (!dr.IsDBNull(iTnfbarbarnombre)) entity.Tnfbarbarnombre = dr.GetString(iTnfbarbarnombre);

                    int iTnfbarbartension = dr.GetOrdinal(helper.Tnfbarbartension);
                    if (!dr.IsDBNull(iTnfbarbartension)) entity.Tnfbarbartension = dr.GetDecimal(iTnfbarbartension);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoTrafoBarraDTO GetById(int codi)
        {
            DpoTrafoBarraDTO entity = new DpoTrafoBarraDTO();

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
    }
}
