using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class GmmIncumplimientoRepository :  RepositoryBase, IGmmIncumplimientoRepository
    {
        public GmmIncumplimientoRepository(string strConn)
            : base(strConn)
        {

        }

        GmmIncumplimientoHelper helper = new GmmIncumplimientoHelper();

        public int Save(GmmIncumplimientoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Incucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Incuanio, DbType.String, entity.INCUANIO);
            dbProvider.AddInParameter(command, helper.Incumes, DbType.String, entity.INCUMES);
            dbProvider.AddInParameter(command, helper.Incuusucreacion, DbType.String, entity.INCUUSUCREACION);
            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.TIPOEMPRCODI);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EMPRCODI);
            dbProvider.AddInParameter(command, helper.Incumonto, DbType.Int32, entity.INCUMONTO);

            var iRslt = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(GmmIncumplimientoDTO entity)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Incuanio, DbType.String, entity.INCUANIO);
            dbProvider.AddInParameter(command, helper.Incumes, DbType.String, entity.INCUMES);
            dbProvider.AddInParameter(command, helper.Incuusumodificacion, DbType.String, entity.INCUUSUMODIFICACION);
            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.TIPOEMPRCODI);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EMPRCODI);
            dbProvider.AddInParameter(command, helper.Incumonto, DbType.Int32, entity.INCUMONTO);
            dbProvider.AddInParameter(command, helper.Incucodi, DbType.Int32, entity.INCUCODI);
            
            dbProvider.ExecuteNonQuery(command);

        }

        public void UpdateTrienio(GmmIncumplimientoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateTrienio);
            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int incucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Incucodi, DbType.Int32, incucodi); ;

            dbProvider.ExecuteNonQuery(command);
        }

        public GmmIncumplimientoDTO GetById(int incucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Incucodi, DbType.Int32, incucodi);
            GmmIncumplimientoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public GmmIncumplimientoDTO GetByIdEdit(int incucodi)
        {
            // Obtener el registro
            GmmIncumplimientoDTO obj = this.GetById(incucodi);
            DbCommand command = null;

            if (obj.TIPOEMPRCODI == 20)
                command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEdit);
            else if (obj.TIPOEMPRCODI == 3)
                command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEditGeneradoras);

            dbProvider.AddInParameter(command, helper.Incucodi, DbType.Int32, incucodi);
            GmmIncumplimientoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateIncumplimientoEdit(dr);
                }
            }

            return entity;
        }

        public List<GmmIncumplimientoDTO> ListarFiltroIncumplimientoDeudora(int anio, string mes, string razonSocial, string numDocumento)
        {
            List<GmmIncumplimientoDTO> entities = new List<GmmIncumplimientoDTO>();
            string queryString = string.Format(helper.SqlListarFiltroIncumplimientoDeudora, anio, mes, razonSocial, numDocumento);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaIncumplimiento(dr));
                }
            }
            return entities;
        }

        public List<GmmIncumplimientoDTO> ListarFiltroIncumplimientoAfectada(int anio, string mes, string razonSocial, string numDocumento)
        {
            List<GmmIncumplimientoDTO> entities = new List<GmmIncumplimientoDTO>();
            string queryString = string.Format(helper.SqlListarFiltroIncumplimientoAfectada, anio, mes, razonSocial, numDocumento);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaIncumplimiento(dr));
                }
            }
            return entities;
        }
    }
}
