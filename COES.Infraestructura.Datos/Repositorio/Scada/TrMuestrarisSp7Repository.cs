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
    /// Clase de acceso a datos de la tabla TR_MUESTRARIS_SP7
    /// </summary>
    public class TrMuestrarisSp7Repository: RepositoryBase, ITrMuestrarisSp7Repository
    {
        public TrMuestrarisSp7Repository(string strConn): base(strConn)
        {
        }

        TrMuestrarisSp7Helper helper = new TrMuestrarisSp7Helper();

        public void Save(TrMuestrarisSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Canalfecha, DbType.DateTime, entity.Canalfecha);
            dbProvider.AddInParameter(command, helper.Canalcalidad, DbType.Int32, entity.Canalcalidad);
            dbProvider.AddInParameter(command, helper.Canalfhora, DbType.DateTime, entity.Canalfhora);
            dbProvider.AddInParameter(command, helper.Canalfhora2, DbType.DateTime, entity.Canalfhora2);
            dbProvider.AddInParameter(command, helper.Canalnomb, DbType.String, entity.Canalnomb);
            dbProvider.AddInParameter(command, helper.Canaliccp, DbType.String, entity.Canaliccp);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Canalvalor, DbType.Decimal, entity.Canalvalor);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.Estado);
            dbProvider.AddInParameter(command, helper.Muerisusucreacion, DbType.String, entity.Muerisusucreacion);
            dbProvider.AddInParameter(command, helper.Muerisfeccreacion, DbType.DateTime, entity.Muerisfeccreacion);
            dbProvider.AddInParameter(command, helper.Muerisusumodificacion, DbType.String, entity.Muerisusumodificacion);
            dbProvider.AddInParameter(command, helper.Muerisfecmodificacion, DbType.DateTime, entity.Muerisfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TrMuestrarisSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Canalfecha, DbType.DateTime, entity.Canalfecha);
            dbProvider.AddInParameter(command, helper.Canalcalidad, DbType.Int32, entity.Canalcalidad);
            dbProvider.AddInParameter(command, helper.Canalfhora, DbType.DateTime, entity.Canalfhora);
            dbProvider.AddInParameter(command, helper.Canalfhora2, DbType.DateTime, entity.Canalfhora2);
            dbProvider.AddInParameter(command, helper.Canalnomb, DbType.String, entity.Canalnomb);
            dbProvider.AddInParameter(command, helper.Canaliccp, DbType.String, entity.Canaliccp);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Canalvalor, DbType.Decimal, entity.Canalvalor);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.Estado);
            dbProvider.AddInParameter(command, helper.Muerisusucreacion, DbType.String, entity.Muerisusucreacion);
            dbProvider.AddInParameter(command, helper.Muerisfeccreacion, DbType.DateTime, entity.Muerisfeccreacion);
            dbProvider.AddInParameter(command, helper.Muerisusumodificacion, DbType.String, entity.Muerisusumodificacion);
            dbProvider.AddInParameter(command, helper.Muerisfecmodificacion, DbType.DateTime, entity.Muerisfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int canalcodi, DateTime canalfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Canalfecha, DbType.DateTime, canalfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrMuestrarisSp7DTO GetById(int canalcodi, DateTime canalfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Canalfecha, DbType.DateTime, canalfecha);
            TrMuestrarisSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrMuestrarisSp7DTO> List()
        {
            List<TrMuestrarisSp7DTO> entitys = new List<TrMuestrarisSp7DTO>();
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

        public List<TrMuestrarisSp7DTO> GetByCriteria()
        {
            List<TrMuestrarisSp7DTO> entitys = new List<TrMuestrarisSp7DTO>();
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


        public List<TrMuestrarisSp7DTO> GetListMuestraRis(int emprcodi)
        {

            string query = string.Format(helper.SqlGetMuestraRis, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            List<TrMuestrarisSp7DTO> entitys = new List<TrMuestrarisSp7DTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    TrMuestrarisSp7DTO entity = new TrMuestrarisSp7DTO();


                    int iCanalIccp = dr.GetOrdinal("CANALICCP");
                    if (!dr.IsDBNull(iCanalIccp)) entity.CanalIccp = dr.GetString(iCanalIccp);


                    int iDelta = dr.GetOrdinal("DELTA");
                    if (!dr.IsDBNull(iDelta)) entity.Delta = Convert.ToInt32(dr.GetValue(iDelta));

                    int icanalnomb = dr.GetOrdinal("CANALNOMB");
                    if (!dr.IsDBNull(icanalnomb)) entity.canalnomb = dr.GetString(icanalnomb);

                    int ifecharep = dr.GetOrdinal("FECHAREP");
                    if (!dr.IsDBNull(ifecharep)) entity.fecharep = dr.GetString(ifecharep);

                    int iCALIDAD2 = dr.GetOrdinal("CALIDAD2");
                    if (!dr.IsDBNull(iCALIDAD2)) entity.CALIDAD2 = dr.GetString(iCALIDAD2);

                    int icanalvalor = dr.GetOrdinal("CANALVALOR");
                    if (!dr.IsDBNull(icanalvalor)) entity.CANALVALOR = Convert.ToDouble(dr.GetValue(icanalvalor));

                    int iestado = dr.GetOrdinal("ESTADO");
                    if (!dr.IsDBNull(icanalnomb)) entity.estado  = dr.GetString(iestado);

                    int iHoraEmpresa = dr.GetOrdinal("HORAEMPRESA");
                    if (!dr.IsDBNull(iHoraEmpresa)) entity.HoraEmpresa = dr.GetString(iHoraEmpresa);

                    int iHoraCoes = dr.GetOrdinal("HORACOES");
                    if (!dr.IsDBNull(iHoraCoes)) entity.HoraCoes = dr.GetString(iHoraCoes);
                    //Modifiacion Movisoft - 2024-01-04
                    int iCanalcalidad = dr.GetOrdinal("CANALCALIDAD");
                    if (!dr.IsDBNull(iCALIDAD2)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetPaginadoMuestraRis(int empresa)
        {
            string query = string.Format(helper.SqlObtenerPaginado, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
                return Convert.ToInt32(result);

            return 0;
        }     
    }
}


