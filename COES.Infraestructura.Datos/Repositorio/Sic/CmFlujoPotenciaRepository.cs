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
    /// Clase de acceso a datos de la tabla CM_FLUJO_POTENCIA
    /// </summary>
    public class CmFlujoPotenciaRepository: RepositoryBase, ICmFlujoPotenciaRepository
    {
        public CmFlujoPotenciaRepository(string strConn): base(strConn)
        {
        }

        CmFlujoPotenciaHelper helper = new CmFlujoPotenciaHelper();

        public int Save(CmFlujoPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Flupotcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Flupotvalor, DbType.Decimal, entity.Flupotvalor);
            dbProvider.AddInParameter(command, helper.Flupotoperativo, DbType.Int32, entity.Flupotoperativo);
            dbProvider.AddInParameter(command, helper.Flupotfecha, DbType.DateTime, entity.Flupotfecha);
            dbProvider.AddInParameter(command, helper.Flupotusucreacion, DbType.String, entity.Flupotusucreacion);
            dbProvider.AddInParameter(command, helper.Flupotfechacreacion, DbType.DateTime, entity.Flupotfechacreacion);
            dbProvider.AddInParameter(command, helper.Flupotvalor1, DbType.Decimal, entity.Flupotvalor1);
            dbProvider.AddInParameter(command, helper.Flupotvalor2, DbType.Decimal, entity.Flupotvalor2);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmFlujoPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Flupotvalor, DbType.Decimal, entity.Flupotvalor);
            dbProvider.AddInParameter(command, helper.Flupotoperativo, DbType.Int32, entity.Flupotoperativo);
            dbProvider.AddInParameter(command, helper.Flupotfecha, DbType.DateTime, entity.Flupotfecha);
            dbProvider.AddInParameter(command, helper.Flupotusucreacion, DbType.String, entity.Flupotusucreacion);
            dbProvider.AddInParameter(command, helper.Flupotfechacreacion, DbType.DateTime, entity.Flupotfechacreacion);            
            dbProvider.AddInParameter(command, helper.Flupotvalor1, DbType.Decimal, entity.Flupotvalor1);
            dbProvider.AddInParameter(command, helper.Flupotvalor2, DbType.Decimal, entity.Flupotvalor2);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Flupotcodi, DbType.Int32, entity.Flupotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int flupotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Flupotcodi, DbType.Int32, flupotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmFlujoPotenciaDTO GetById(int flupotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Flupotcodi, DbType.Int32, flupotcodi);
            CmFlujoPotenciaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmFlujoPotenciaDTO> List()
        {
            List<CmFlujoPotenciaDTO> entitys = new List<CmFlujoPotenciaDTO>();
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

        public List<CmFlujoPotenciaDTO> GetByCriteria()
        {
            List<CmFlujoPotenciaDTO> entitys = new List<CmFlujoPotenciaDTO>();
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

        public List<CmFlujoPotenciaDTO> ObtenerReporteFlujoPotencia(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CmFlujoPotenciaDTO> entitys = new List<CmFlujoPotenciaDTO>();
            string query = string.Format(helper.SqlObtenerReporteFlujoPotencia, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmFlujoPotenciaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iNodobarra1 = dr.GetOrdinal(helper.Nodobarra1);
                    if (!dr.IsDBNull(iNodobarra1)) entity.Nodobarra1 = dr.GetString(iNodobarra1);

                    int iNodobarra2 = dr.GetOrdinal(helper.Nodobarra2);
                    if (!dr.IsDBNull(iNodobarra2)) entity.Nodobarra2 = dr.GetString(iNodobarra2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
