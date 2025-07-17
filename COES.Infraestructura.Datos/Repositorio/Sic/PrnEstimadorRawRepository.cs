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
    public class PrnEstimadorRawRepository : RepositoryBase, IPrnEstimadorRawRepository
    {
        public PrnEstimadorRawRepository(string strConn)
            : base(strConn)
        {
        }

        PrnEstimadorRawHelper helper = new PrnEstimadorRawHelper();

        public List<PrnEstimadorRawDTO> List()
        {
            List<PrnEstimadorRawDTO> entitys = new List<PrnEstimadorRawDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnEstimadorRawDTO entity = new PrnEstimadorRawDTO();

                    int iEtmrawcodi = dr.GetOrdinal(helper.Etmrawcodi);
                    if (!dr.IsDBNull(iEtmrawcodi)) entity.Etmrawcodi = Convert.ToInt32(dr.GetValue(iEtmrawcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iEtmrawfuente = dr.GetOrdinal(helper.Etmrawfuente);
                    if (!dr.IsDBNull(iEtmrawfuente)) entity.Etmrawfuente = Convert.ToInt32(dr.GetValue(iEtmrawfuente));

                    int iEtmrawtipomedi = dr.GetOrdinal(helper.Etmrawtipomedi);
                    if (!dr.IsDBNull(iEtmrawtipomedi)) entity.Etmrawtipomedi = Convert.ToInt32(dr.GetValue(iEtmrawtipomedi));

                    int iEtmrawfecha = dr.GetOrdinal(helper.Etmrawfecha);
                    if (!dr.IsDBNull(iEtmrawfecha)) entity.Etmrawfecha = dr.GetDateTime(iEtmrawfecha);

                    int iEtmrawvalor = dr.GetOrdinal(helper.Etmrawvalor);
                    if (!dr.IsDBNull(iEtmrawvalor)) entity.Etmrawvalor = dr.GetDecimal(iEtmrawvalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(PrnEstimadorRawDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Etmrawcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);
            dbProvider.AddInParameter(command, helper.Etmrawfuente, DbType.String, entity.Etmrawfuente);
            dbProvider.AddInParameter(command, helper.Etmrawtipomedi, DbType.String, entity.Etmrawtipomedi);
            dbProvider.AddInParameter(command, helper.Etmrawfecha, DbType.DateTime, entity.Etmrawfecha);
            dbProvider.AddInParameter(command, helper.Etmrawvalor, DbType.Decimal, entity.Etmrawvalor);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnEstimadorRawDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);
            dbProvider.AddInParameter(command, helper.Etmrawfuente, DbType.String, entity.Etmrawfuente);
            dbProvider.AddInParameter(command, helper.Etmrawtipomedi, DbType.String, entity.Etmrawtipomedi);
            dbProvider.AddInParameter(command, helper.Etmrawfecha, DbType.DateTime, entity.Etmrawfecha);
            dbProvider.AddInParameter(command, helper.Etmrawvalor, DbType.Decimal, entity.Etmrawvalor);
            dbProvider.AddInParameter(command, helper.Etmrawcodi, DbType.Int32, entity.Etmrawcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public PrnEstimadorRawDTO GetById(int codigo)
        {
            PrnEstimadorRawDTO entity = new PrnEstimadorRawDTO();

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

            dbProvider.AddInParameter(command, helper.Etmrawcodi, DbType.Int32, codigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnEstimadorRawDTO> ListDemandaTnaPorUnidad(string unidades, string fecInicio, string fecFin, string variables)
        {
            List<PrnEstimadorRawDTO> entitys = new List<PrnEstimadorRawDTO>();
            string query = string.Format(helper.SqlListDemandaTnaPorUnidad, unidades, fecInicio, fecFin, variables);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnEstimadorRawDTO entity = new PrnEstimadorRawDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iEtmrawfuente = dr.GetOrdinal(helper.Etmrawfuente);
                    if (!dr.IsDBNull(iEtmrawfuente)) entity.Etmrawfuente = Convert.ToInt32(dr.GetValue(iEtmrawfuente));

                    int iEtmrawtipomedi = dr.GetOrdinal(helper.Etmrawtipomedi);
                    if (!dr.IsDBNull(iEtmrawtipomedi)) entity.Etmrawtipomedi = Convert.ToInt32(dr.GetValue(iEtmrawtipomedi));

                    int iEtmrawfecha = dr.GetOrdinal(helper.Etmrawfecha);
                    if (!dr.IsDBNull(iEtmrawfecha)) entity.Etmrawfecha = dr.GetDateTime(iEtmrawfecha);

                    int iEtmrawvalor = dr.GetOrdinal(helper.Etmrawvalor);
                    if (!dr.IsDBNull(iEtmrawvalor)) entity.Etmrawvalor = dr.GetDecimal(iEtmrawvalor);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrnEstimadorRawDTO> ListEstimadorRawPorRangoPorUnidad(int unidad,
            DateTime fechaInicio, DateTime fechaFin, 
            int idVariable, int idFuente, int modulo)
        {
            List<PrnEstimadorRawDTO> entitys = new List<PrnEstimadorRawDTO>();
            string inicio = fechaInicio.ToString("dd/MM/yyyy");
            string fin = fechaFin.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlListEstimadorRawPorRangoPorUnidad,
                unidad, inicio, fin, idVariable, idFuente, modulo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnEstimadorRawDTO entity = new PrnEstimadorRawDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iEtmrawfuente = dr.GetOrdinal(helper.Etmrawfuente);
                    if (!dr.IsDBNull(iEtmrawfuente)) entity.Etmrawfuente = Convert.ToInt32(dr.GetValue(iEtmrawfuente));

                    int iEtmrawtipomedi = dr.GetOrdinal(helper.Etmrawtipomedi);
                    if (!dr.IsDBNull(iEtmrawtipomedi)) entity.Etmrawtipomedi = Convert.ToInt32(dr.GetValue(iEtmrawtipomedi));

                    int iEtmrawfecha = dr.GetOrdinal(helper.Etmrawfecha);
                    if (!dr.IsDBNull(iEtmrawfecha)) entity.Etmrawfecha = dr.GetDateTime(iEtmrawfecha);

                    int iEtmrawvalor = dr.GetOrdinal(helper.Etmrawvalor);
                    if (!dr.IsDBNull(iEtmrawvalor)) entity.Etmrawvalor = dr.GetDecimal(iEtmrawvalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnEstimadorRawDTO> ListEstimadorRawPorRangoPorAsociacion(int unidad, 
            DateTime fechaInicio, DateTime fechaFin,
            int idVariable, int idFuente, int modulo)
        {
            List<PrnEstimadorRawDTO> entitys = new List<PrnEstimadorRawDTO>();
            string inicio = fechaInicio.ToString("dd/MM/yyyy");
            string fin = fechaFin.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlListEstimadorRawPorRangoPorAsociacion,
                unidad, inicio, fin, idVariable, idFuente, modulo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnEstimadorRawDTO entity = new PrnEstimadorRawDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEtmrawfecha = dr.GetOrdinal(helper.Etmrawfecha);
                    if (!dr.IsDBNull(iEtmrawfecha)) entity.Etmrawfecha = dr.GetDateTime(iEtmrawfecha);

                    int iEtmrawvalor = dr.GetOrdinal(helper.Etmrawvalor);
                    if (!dr.IsDBNull(iEtmrawvalor)) entity.Etmrawvalor = dr.GetDecimal(iEtmrawvalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void BulkInsert(List<PrnEstimadorRawDTO> entitys, string nombreTabla)
        {
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Prnvarcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Etmrawfuente, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Etmrawtipomedi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Etmrawfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Etmrawvalor, DbType.Decimal);
            
            dbProvider.BulkInsert<PrnEstimadorRawDTO>(entitys, nombreTabla);
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            return id;
        }

        public void DeletePorFechaIntervalo(int fuente, string fecha)
        {
            string query = string.Format(helper.SqlDeletePorFechaIntervalo, fuente, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public int GetMaxIdSco()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdSco);
            object result = dbProvider.ExecuteScalar(command);
            int id = -1;
            if (result != null) id = Convert.ToInt32(result);
            return id;
        }

        public void InsertTableIntoPrnEstimadorRaw(string nombreTabla, string fecha)
        {            
            string query = string.Format(helper.SqlInsertTableIntoPrnEstimadorRaw, nombreTabla, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void TruncateTablaTemporal(string nombreTabla)
        {
            string query = string.Format(helper.SqlTruncateTablaTemporal, nombreTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
