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
    /// Clase de acceso a datos de la tabla TR_ESTADCANAL_SP7
    /// </summary>
    public class TrEstadcanalSp7Repository: RepositoryBase, ITrEstadcanalSp7Repository
    {
        public TrEstadcanalSp7Repository(string strConn): base(strConn)
        {
        }

        TrEstadcanalSp7Helper helper = new TrEstadcanalSp7Helper();

        public void Save(TrEstadcanalSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, entity.Fecha);
            dbProvider.AddInParameter(command, helper.Tvalido, DbType.Decimal, entity.Tvalido);
            dbProvider.AddInParameter(command, helper.Tcong, DbType.Decimal, entity.Tcong);
            dbProvider.AddInParameter(command, helper.Tindet, DbType.Decimal, entity.Tindet);
            dbProvider.AddInParameter(command, helper.Tnnval, DbType.Decimal, entity.Tnnval);
            dbProvider.AddInParameter(command, helper.Ultcalidad, DbType.Int32, entity.Ultcalidad);
            dbProvider.AddInParameter(command, helper.Ultcambio, DbType.DateTime, entity.Ultcambio);
            dbProvider.AddInParameter(command, helper.Ultcambioe, DbType.DateTime, entity.Ultcambioe);
            dbProvider.AddInParameter(command, helper.Ultvalor, DbType.Decimal, entity.Ultvalor);
            dbProvider.AddInParameter(command, helper.Tretraso, DbType.Decimal, entity.Tretraso);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Trstdlastdate, DbType.DateTime, entity.Trstdlastdate);
            dbProvider.AddInParameter(command, helper.Numregistros, DbType.Int32, entity.Numregistros);
            dbProvider.AddInParameter(command, helper.Trstdingreso, DbType.String, entity.Trstdingreso);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TrEstadcanalSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, entity.Fecha);
            dbProvider.AddInParameter(command, helper.Tvalido, DbType.Decimal, entity.Tvalido);
            dbProvider.AddInParameter(command, helper.Tcong, DbType.Decimal, entity.Tcong);
            dbProvider.AddInParameter(command, helper.Tindet, DbType.Decimal, entity.Tindet);
            dbProvider.AddInParameter(command, helper.Tnnval, DbType.Decimal, entity.Tnnval);
            dbProvider.AddInParameter(command, helper.Ultcalidad, DbType.Int32, entity.Ultcalidad);
            dbProvider.AddInParameter(command, helper.Ultcambio, DbType.DateTime, entity.Ultcambio);
            dbProvider.AddInParameter(command, helper.Ultcambioe, DbType.DateTime, entity.Ultcambioe);
            dbProvider.AddInParameter(command, helper.Ultvalor, DbType.Decimal, entity.Ultvalor);
            dbProvider.AddInParameter(command, helper.Tretraso, DbType.Decimal, entity.Tretraso);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Trstdlastdate, DbType.DateTime, entity.Trstdlastdate);
            dbProvider.AddInParameter(command, helper.Numregistros, DbType.Int32, entity.Numregistros);
            dbProvider.AddInParameter(command, helper.Trstdingreso, DbType.String, entity.Trstdingreso);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int canalcodi, DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, fecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrEstadcanalSp7DTO GetById(int canalcodi, DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Fecha, DbType.DateTime, fecha);
            TrEstadcanalSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrEstadcanalSp7DTO> List()
        {
            List<TrEstadcanalSp7DTO> entitys = new List<TrEstadcanalSp7DTO>();
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

        public List<TrEstadcanalSp7DTO> GetByCriteria()
        {
            List<TrEstadcanalSp7DTO> entitys = new List<TrEstadcanalSp7DTO>();
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

    




        //_________________________________________________________________

        //_________________________________________________________



         public List<TrEstadcanalSp7DTO> GetDispDiaSignal(DateTime fecha, int emprcodi)
        {

            string query = string.Format(helper.SqlGetDispDiaSignal, fecha.ToString(ConstantesBase.FormatoFecha), emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            List<TrEstadcanalSp7DTO> entitys = new List<TrEstadcanalSp7DTO>();
              


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                TrEstadcanalSp7DTO entity = new TrEstadcanalSp7DTO();
                

                    int iZona = dr.GetOrdinal("ZONA");
                    if (!dr.IsDBNull(iZona)) entity.zona = dr.GetString(iZona);

                      int iDispo = dr.GetOrdinal("DISPO");
                      if (!dr.IsDBNull(iDispo))entity.dispo =dr.GetDecimal(iDispo);

                      int iNombCanal = dr.GetOrdinal("NOMBCANAL");
                   if (!dr.IsDBNull( iNombCanal)) entity.nombcanal = dr.GetString(iNombCanal);

                   int iIccp = dr.GetOrdinal("ICCP");
                   if (!dr.IsDBNull(iIccp)) entity.iccp = dr.GetString(iIccp);

                      int iUnidad = dr.GetOrdinal("UNIDAD");
                   if (!dr.IsDBNull(iZona)) entity.unidad = dr.GetString(iUnidad);

                   entitys.Add(entity);

                }
            }

            return entitys;
        }



        public int GetPaginadoDispDiaSignal( DateTime fecha ,int emprcodi)
        {
            string query = string.Format(helper.SqlGePaginadoDiaSignal,fecha.ToString(ConstantesBase.FormatoFechaExtendido),emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
                return Convert.ToInt32(result);

            return 0;
        }     
    }


    }

