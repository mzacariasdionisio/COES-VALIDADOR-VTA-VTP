using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;
using COES.Dominio.DTO.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTD_CARGOSENEREAC
    /// </summary>
    public class VtdCargosEneReacRepository : RepositoryBase, IVtdCargosEneReacRepository
    {
        public VtdCargosEneReacRepository(string strConn) : base(strConn)
        {
        }

        VtdCargosEneReacHelper helper = new VtdCargosEneReacHelper();

        //filtro por rango de fechas -Fit

        public int Save(VtdCargoEneReacDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Caercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Caerfecha, DbType.DateTime, entity.Caerfecha);
            dbProvider.AddInParameter(command, helper.Caermonto, DbType.Decimal, entity.Caermonto);
            dbProvider.AddInParameter(command, helper.Caerdeleted, DbType.Int32, entity.Caerdeleted);
            dbProvider.AddInParameter(command, helper.Caersucreacion, DbType.String, entity.Caersucreacion);
            dbProvider.AddInParameter(command, helper.Caerfeccreacion, DbType.DateTime, entity.Caerfeccreacion);
            dbProvider.AddInParameter(command, helper.Caerusumodificacion, DbType.String, entity.Caerusumodificacion);
            dbProvider.AddInParameter(command, helper.Caerfecmodificacion, DbType.DateTime, entity.Caerfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<VtdCargoEneReacDTO> List(DateTime date)
        {
            List<VtdCargoEneReacDTO> entitys = new List<VtdCargoEneReacDTO>();
            var query = string.Format(helper.SqlListByDate, new DateTime(date.Year, date.Month, 1).ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Caerfecha, DbType.DateTime, date);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdCargoEneReacDTO entity = new VtdCargoEneReacDTO();

                    int iCaercodi = dr.GetOrdinal(helper.Caercodi);
                    if (!dr.IsDBNull(iCaercodi)) entity.Caercodi = Convert.ToInt32(dr.GetValue(iCaercodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iCaerfecha = dr.GetOrdinal(helper.Caerfecha);
                    if (!dr.IsDBNull(iCaerfecha)) entity.Caerfecha = dr.GetDateTime(iCaerfecha);

                    int iCaermonto = dr.GetOrdinal(helper.Caermonto);
                    if (!dr.IsDBNull(iCaermonto)) entity.Caermonto = Convert.ToDecimal(dr.GetValue(iCaermonto));

                    int iCaerdeleted = dr.GetOrdinal(helper.Caerdeleted);
                    if (!dr.IsDBNull(iCaerdeleted)) entity.Caerdeleted = Convert.ToInt32(dr.GetValue(iCaerdeleted));

                    int iCaersucreacion = dr.GetOrdinal(helper.Caersucreacion);
                    if (!dr.IsDBNull(iCaersucreacion)) entity.Caersucreacion = dr.GetString(iCaersucreacion);

                    int iCaerfeccreacion = dr.GetOrdinal(helper.Caerfeccreacion);
                    if (!dr.IsDBNull(iCaerfeccreacion)) entity.Caerfeccreacion = dr.GetDateTime(iCaerfeccreacion);

                    int iCaerusumodificacion = dr.GetOrdinal(helper.Caerusumodificacion);
                    if (!dr.IsDBNull(iCaerusumodificacion)) entity.Caerusumodificacion = dr.GetString(iCaerusumodificacion);

                    int iCaerfecmodificacion = dr.GetOrdinal(helper.Caerfecmodificacion);
                    if (!dr.IsDBNull(iCaerfecmodificacion)) entity.Caerfecmodificacion = dr.GetDateTime(iCaerfecmodificacion);

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //no se utiliza
        public List<VtdCargoEneReacDTO> ListByParticipant(int emprcodi)
        {
            List<VtdCargoEneReacDTO> entitys = new List<VtdCargoEneReacDTO>();

            string sCommand = string.Format(helper.SqlList, emprcodi.ToString());
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdCargoEneReacDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VtdCargoEneReacDTO GetMontofromEmpresa(int caermonto)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMontoByEmpresa);
            VtdCargoEneReacDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

        public void UpdateByResultDate(int deleted, DateTime fecha)
        {
            //entity2.Caerdeleted = entity2.Caerdeleted == null ? entity2.Caerdeleted : entity2.Caerdeleted;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateByResultDate);

            dbProvider.AddInParameter(command, helper.Caerdeleted, DbType.Int32, deleted);
            dbProvider.AddInParameter(command, helper.Caerfecha, DbType.DateTime, fecha);

            dbProvider.ExecuteNonQuery(command);
        }


        public void Update(VtdCargoEneReacDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            //verificar

            dbProvider.AddInParameter(command, helper.Caerfecha, DbType.DateTime, entity.Caerfecha);
            dbProvider.AddInParameter(command, helper.Caermonto, DbType.Decimal, entity.Caermonto);
            dbProvider.AddInParameter(command, helper.Caerdeleted, DbType.Int16, entity.Caerdeleted);
            dbProvider.AddInParameter(command, helper.Caersucreacion, DbType.String, entity.Caersucreacion);
            dbProvider.AddInParameter(command, helper.Caerfeccreacion, DbType.DateTime, entity.Caerfeccreacion);
            dbProvider.AddInParameter(command, helper.Caerusumodificacion, DbType.String, entity.Caerusumodificacion);
            dbProvider.AddInParameter(command, helper.Caerfecmodificacion, DbType.DateTime, entity.Caerfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int emprcodi, DateTime date)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.Caerfecha, DbType.DateTime, date);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByEmpresa(int emprcodi)
        {

        }

        public VtdCargoEneReacDTO GetMontoByEmpresa(int emprcodi, DateTime caerfecha)
        {
            var sComando = string.Format(helper.SqlGetMontoByEmpresa, emprcodi, caerfecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);

            VtdCargoEneReacDTO entity = new VtdCargoEneReacDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iCaermonto = dr.GetOrdinal(helper.Caermonto);
                    if (!dr.IsDBNull(iCaermonto)) entity.Caermonto = Convert.ToDecimal(dr.GetValue(iCaermonto));
                }
                else
                {
                    entity.Caermonto = 0;
                }
            }

            return entity;
        }
    }
}
