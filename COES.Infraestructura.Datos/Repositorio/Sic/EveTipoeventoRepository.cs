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
    /// Clase de acceso a datos de la tabla EVE_TIPOEVENTO
    /// </summary>
    public class EveTipoeventoRepository: RepositoryBase, IEveTipoeventoRepository
    {
        public EveTipoeventoRepository(string strConn): base(strConn)
        {

        }

        EveTipoeventoHelper helper = new EveTipoeventoHelper();

        public int Save(EveTipoeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipoevendesc, DbType.String, entity.Tipoevendesc);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Tipoevenabrev, DbType.String, entity.Tipoevenabrev);
            dbProvider.AddInParameter(command, helper.Cateevencodi, DbType.Int32, entity.Cateevencodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveTipoeventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Tipoevendesc, DbType.String, entity.Tipoevendesc);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Tipoevenabrev, DbType.String, entity.Tipoevenabrev);
            dbProvider.AddInParameter(command, helper.Cateevencodi, DbType.Int32, entity.Cateevencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipoevencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, tipoevencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveTipoeventoDTO GetById(int tipoevencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, tipoevencodi);
            EveTipoeventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveTipoeventoDTO> List()
        {
            List<EveTipoeventoDTO> entitys = new List<EveTipoeventoDTO>();
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

        public List<EveTipoeventoDTO> GetByCriteria()
        {
            List<EveTipoeventoDTO> entitys = new List<EveTipoeventoDTO>();
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

        #region INTERVENCIONES
        public List<EveTipoeventoDTO> ListarComboTiposIntervenciones(int iEscenario)
        {
            List<EveTipoeventoDTO> entitys = new List<EveTipoeventoDTO>();
            DbCommand command = null;

            if (iEscenario == 1) // 1 = Mantenimiento
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTipoIntervencionesMantenimiento);
            else if (iEscenario == 2) // 2 = Consulta
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTipoIntervencionesConsulta);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveTipoeventoDTO entity = new EveTipoeventoDTO();

                    int iTipoevencodi = dr.GetOrdinal(helper.Tipoevencodi);
                    if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

    }
}
