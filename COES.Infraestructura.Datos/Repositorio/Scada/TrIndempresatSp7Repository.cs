using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_INDEMPRESAT_SP7
    /// </summary>
    public class TrIndempresatSp7Repository: RepositoryBase, ITrIndempresatSp7Repository
    {
        public TrIndempresatSp7Repository(string strConn): base(strConn)
        {
        }

        TrIndempresatSp7Helper helper = new TrIndempresatSp7Helper();

        public void Save(TrIndempresatSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, entity.Fecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Media, DbType.Int32, entity.Media);
            dbProvider.AddInParameter(command, helper.Factor, DbType.Int32, entity.Factor);
            dbProvider.AddInParameter(command, helper.Media2, DbType.Int32, entity.Media2);
            dbProvider.AddInParameter(command, helper.Factor2, DbType.Int32, entity.Factor2);
            dbProvider.AddInParameter(command, helper.Findispon, DbType.Int32, entity.Findispon);
            dbProvider.AddInParameter(command, helper.Ciccpe, DbType.Int32, entity.Ciccpe);
            dbProvider.AddInParameter(command, helper.Ciccpea, DbType.Int32, entity.Ciccpea);
            dbProvider.AddInParameter(command, helper.Factorg, DbType.Int32, entity.Factorg);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TrIndempresatSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, entity.Fecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Media, DbType.Int32, entity.Media);
            dbProvider.AddInParameter(command, helper.Factor, DbType.Int32, entity.Factor);
            dbProvider.AddInParameter(command, helper.Media2, DbType.Int32, entity.Media2);
            dbProvider.AddInParameter(command, helper.Factor2, DbType.Int32, entity.Factor2);
            dbProvider.AddInParameter(command, helper.Findispon, DbType.Int32, entity.Findispon);
            dbProvider.AddInParameter(command, helper.Ciccpe, DbType.Int32, entity.Ciccpe);
            dbProvider.AddInParameter(command, helper.Ciccpea, DbType.Int32, entity.Ciccpea);
            dbProvider.AddInParameter(command, helper.Factorg, DbType.Int32, entity.Factorg);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int emprcodi,DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrIndempresatSp7DTO GetById(int emprcodi,DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            TrIndempresatSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }


        public List<TrIndempresatSp7DTO> List()
        {
            List<TrIndempresatSp7DTO> entitys = new List<TrIndempresatSp7DTO>();
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

        public List<TrIndempresatSp7DTO> GetByCriteria()
        {
            List<TrIndempresatSp7DTO> entitys = new List<TrIndempresatSp7DTO>();
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





        public List<TrIndempresatSp7DTO> GetListDispMensual(int emprcodi, DateTime fecha)
        {

            string query = string.Format(helper.SqlGetDispMensual, emprcodi, fecha.ToString(ConstantesBase.FormatoFechaSoloMes), fecha.ToString(ConstantesBase.FormatoFechaAn));
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            List<TrIndempresatSp7DTO> entitys = new List<TrIndempresatSp7DTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrIndempresatSp7DTO entity = new TrIndempresatSp7DTO();

                    int iFecha = dr.GetOrdinal("FECHA");
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);


                    int idisponibilidad = dr.GetOrdinal("DISPONIBILIDAD");
                    if (!dr.IsDBNull(idisponibilidad)) entity.disponibilidad = dr.GetDecimal(idisponibilidad);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }



        public int GetPaginadoDispMensual(int emprcodi, DateTime fecha)
        {
            string query = string.Format(helper.SqlObtenerPaginado, emprcodi, fecha.ToString(ConstantesBase.FormatoFechaSoloMes), fecha.ToString(ConstantesBase.FormatoFechaAn));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
                return Convert.ToInt32(result);

            return 0;
        }     



















    }
}
