using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class PeriodoDeclaracionRepository : RepositoryBase, IPeriodoDeclaracionRepository
    {
        public PeriodoDeclaracionRepository(string strConn) : base(strConn)
        {
        }

        PeriodoDeclaracionHelper helper = new PeriodoDeclaracionHelper();
        public PeriodoDeclaracionDTO GetById(int peridcCodi)
        {
            PeriodoDeclaracionDTO entitys = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.PeridcCodi, DbType.Int32, peridcCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entitys = helper.Create(dr);
                }
            }
            return entitys;
        }
        public List<PeriodoDeclaracionDTO> GetListaCombobox()
        {
            List<PeriodoDeclaracionDTO> entitys = new List<PeriodoDeclaracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaCombobox);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<PeriodoDeclaracionDTO> GetListaPeriodoDeclaracion()
        {

            List<PeriodoDeclaracionDTO> entitys = new List<PeriodoDeclaracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaPeriodoDeclaracion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public PeriodoDeclaracionDTO SaveUpdate(PeriodoDeclaracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveUpdate);

            dbProvider.AddInParameter(command, helper.PeridcCodi, DbType.Int32, entity.PeridcCodi);
            dbProvider.AddInParameter(command, helper.PeridcAnio, DbType.Int32, entity.PeridcAnio);
            dbProvider.AddInParameter(command, helper.PeridcMes, DbType.Int32, entity.PeridcMes);
            dbProvider.AddInParameter(command, helper.PeridcNombre, DbType.String, entity.PeridcNombre);
            dbProvider.AddInParameter(command, helper.PeridcUsuarioRegi, DbType.String, entity.PeridcUsuarioRegi);
            dbProvider.AddInParameter(command, helper.PeridcEstado, DbType.String, entity.PeridcEstado);
            dbProvider.AddOutParameter(command, helper.Mensaje, DbType.String, 500);
            dbProvider.ExecuteNonQuery(command);
            entity.Mensaje = dbProvider.GetParameterValue(command, helper.Mensaje) == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, helper.Mensaje);
            return entity;
        }

    }
}
