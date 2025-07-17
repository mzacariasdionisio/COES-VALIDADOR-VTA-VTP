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
    /// Clase de acceso a datos de la tabla SI_REPRESENTANTE
    /// </summary>
    public class SiRepresentanteRepository: RepositoryBase, ISiRepresentanteRepository
    {
        public SiRepresentanteRepository(string strConn): base(strConn)
        {
        }

        SiRepresentanteHelper helper = new SiRepresentanteHelper();

        public int Save(SiRepresentanteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rptetipo, DbType.String, entity.Rptetipo);
            dbProvider.AddInParameter(command, helper.Rptetiprepresentantelegal, DbType.String, entity.Rptetiprepresentantelegal);
            dbProvider.AddInParameter(command, helper.Rptebaja, DbType.String, entity.Rptebaja);
            dbProvider.AddInParameter(command, helper.Rpteinicial, DbType.String, entity.Rpteinicial);
            dbProvider.AddInParameter(command, helper.Rptetipdocidentidad, DbType.String, entity.Rptetipdocidentidad);
            dbProvider.AddInParameter(command, helper.Rptedocidentidad, DbType.String, entity.Rptedocidentidad);
            dbProvider.AddInParameter(command, helper.Rptedocidentidadadj, DbType.String, entity.Rptedocidentidadadj);
            dbProvider.AddInParameter(command, helper.Rptenombres, DbType.String, entity.Rptenombres);
            dbProvider.AddInParameter(command, helper.Rpteapellidos, DbType.String, entity.Rpteapellidos);
            dbProvider.AddInParameter(command, helper.Rptevigenciapoder, DbType.String, entity.Rptevigenciapoder);
            dbProvider.AddInParameter(command, helper.Rptecargoempresa, DbType.String, entity.Rptecargoempresa);
            dbProvider.AddInParameter(command, helper.Rptetelefono, DbType.String, entity.Rptetelefono);
            dbProvider.AddInParameter(command, helper.Rptetelfmovil, DbType.String, entity.Rptetelfmovil);
            dbProvider.AddInParameter(command, helper.Rptecorreoelectronico, DbType.String, entity.Rptecorreoelectronico);
            dbProvider.AddInParameter(command, helper.Rptefeccreacion, DbType.DateTime, entity.Rptefeccreacion);
            dbProvider.AddInParameter(command, helper.Rpteusucreacion, DbType.String, entity.Rpteusucreacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rpteusumodificacion, DbType.String, entity.Rpteusumodificacion);
            dbProvider.AddInParameter(command, helper.Rptefecmodificacion, DbType.DateTime, entity.Rptefecmodificacion);
            dbProvider.AddInParameter(command, helper.Rptefechavigenciapoder, DbType.DateTime, entity.Rptefechavigenciapoder);
            dbProvider.AddInParameter(command, helper.Rptedocidentidadadjfilename, DbType.String, entity.Rptedocidentidadadjfilename);
            dbProvider.AddInParameter(command, helper.Rptevigenciapoderfilename, DbType.String, entity.Rptevigenciapoderfilename);
            dbProvider.AddInParameter(command, helper.Rpteindnotic, DbType.String, (string.IsNullOrEmpty(entity.Rpteindnotic)? "S": entity.Rpteindnotic));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(SiRepresentanteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            int id = 0;
            if (entity.Rptecodi == 0)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                id = 1;
                if (result != null) id = Convert.ToInt32(result);
            }
            else
            {
                id = entity.Rptecodi + 1;
            }

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Rptecodi;
            param.Value = id;
            command2.Parameters.Add(param);

            //param = command2.CreateParameter(); param.ParameterName = helper.Rptecodi; param.Value = id; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptetipo; param.Value = entity.Rptetipo; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptetiprepresentantelegal; param.Value = entity.Rptetiprepresentantelegal; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptebaja; param.Value = entity.Rptebaja; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpteinicial; param.Value = entity.Rpteinicial; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptetipdocidentidad; param.Value = entity.Rptetipdocidentidad == null ? "" : entity.Rptetipdocidentidad; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptedocidentidad; param.Value = entity.Rptedocidentidad == null ? "" : entity.Rptedocidentidad; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptedocidentidadadj; param.Value = entity.Rptedocidentidadadj == null ? "" : entity.Rptedocidentidadadj; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptenombres; param.Value = entity.Rptenombres; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpteapellidos; param.Value = entity.Rpteapellidos; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptevigenciapoder; param.Value = entity.Rptevigenciapoder == null ? "" : entity.Rptevigenciapoder; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptecargoempresa; param.Value = entity.Rptecargoempresa; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptetelefono; param.Value = entity.Rptetelefono == null ? "" : entity.Rptetelefono; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptetelfmovil; param.Value = entity.Rptetelfmovil == null ? "" : entity.Rptetelfmovil; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptecorreoelectronico; param.Value = entity.Rptecorreoelectronico == null ? "" : entity.Rptecorreoelectronico; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptefeccreacion; param.Value = entity.Rptefeccreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpteusucreacion; param.Value = entity.Rpteusucreacion == null ? "" : entity.Rpteusucreacion; command2.Parameters.Add(param);            
            param = command2.CreateParameter(); param.ParameterName = helper.Emprcodi; param.Value = entity.Emprcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpteusumodificacion; param.Value = entity.Rpteusumodificacion == null ? "" : entity.Rpteusumodificacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptefecmodificacion; param.Value = entity.Rptefeccreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptefechavigenciapoder; param.Value = entity.Rptefeccreacion; command2.Parameters.Add(param);            
            param = command2.CreateParameter(); param.ParameterName = helper.Rptedocidentidadadjfilename; param.Value = entity.Rptedocidentidadadjfilename == null ? "" : entity.Rptedocidentidadadjfilename; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rptevigenciapoderfilename; param.Value = entity.Rptevigenciapoderfilename == null ? "" : entity.Rptevigenciapoderfilename; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpteindnotic; param.Value = entity.Rpteindnotic == null ? "" : entity.Rpteindnotic; command2.Parameters.Add(param);

            command2.ExecuteNonQuery();


            return id;
        }
        public void Update(SiRepresentanteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rptetipo, DbType.String, entity.Rptetipo);
            dbProvider.AddInParameter(command, helper.Rptetiprepresentantelegal, DbType.String, entity.Rptetiprepresentantelegal);
            dbProvider.AddInParameter(command, helper.Rptebaja, DbType.String, entity.Rptebaja);
            dbProvider.AddInParameter(command, helper.Rpteinicial, DbType.String, entity.Rpteinicial);
            dbProvider.AddInParameter(command, helper.Rptetipdocidentidad, DbType.String, entity.Rptetipdocidentidad);
            dbProvider.AddInParameter(command, helper.Rptedocidentidad, DbType.String, entity.Rptedocidentidad);
            dbProvider.AddInParameter(command, helper.Rptedocidentidadadj, DbType.String, entity.Rptedocidentidadadj);
            dbProvider.AddInParameter(command, helper.Rptenombres, DbType.String, entity.Rptenombres);
            dbProvider.AddInParameter(command, helper.Rpteapellidos, DbType.String, entity.Rpteapellidos);
            dbProvider.AddInParameter(command, helper.Rptevigenciapoder, DbType.String, entity.Rptevigenciapoder);
            dbProvider.AddInParameter(command, helper.Rptecargoempresa, DbType.String, entity.Rptecargoempresa);
            dbProvider.AddInParameter(command, helper.Rptetelefono, DbType.String, entity.Rptetelefono);
            dbProvider.AddInParameter(command, helper.Rptetelfmovil, DbType.String, entity.Rptetelfmovil);
            dbProvider.AddInParameter(command, helper.Rptecorreoelectronico, DbType.String, entity.Rptecorreoelectronico);
            dbProvider.AddInParameter(command, helper.Rptefeccreacion, DbType.DateTime, entity.Rptefeccreacion);
            dbProvider.AddInParameter(command, helper.Rpteusucreacion, DbType.String, entity.Rpteusucreacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rpteusumodificacion, DbType.String, entity.Rpteusumodificacion);
            dbProvider.AddInParameter(command, helper.Rptefecmodificacion, DbType.DateTime, entity.Rptefecmodificacion);
            dbProvider.AddInParameter(command, helper.Rptefechavigenciapoder, DbType.DateTime, entity.Rptefechavigenciapoder);
            dbProvider.AddInParameter(command, helper.Rptedocidentidadadjfilename, DbType.String, entity.Rptedocidentidadadjfilename);
            dbProvider.AddInParameter(command, helper.Rptevigenciapoderfilename, DbType.String, entity.Rptevigenciapoderfilename);

            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, entity.Rptecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstadoRegistro(SiRepresentanteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlUpdateEstadoRegistro;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Rptecodi;
            param.Value = entity.Rptecodi;
            command2.Parameters.Add(param);

            command2.ExecuteNonQuery();
        }

        public void Delete(int rptecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, rptecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiRepresentanteDTO GetById(int rptecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, rptecodi);
            SiRepresentanteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iRpteindnotic = dr.GetOrdinal(helper.Rpteindnotic);
                    if (!dr.IsDBNull(iRpteindnotic)) entity.Rpteindnotic = dr.GetString(iRpteindnotic);
                }
            }

            return entity;
        }

        public List<SiRepresentanteDTO> List()
        {
            List<SiRepresentanteDTO> entitys = new List<SiRepresentanteDTO>();
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

        public List<SiRepresentanteDTO> GetByCriteria(int emprcodi)
        {
            List<SiRepresentanteDTO> entitys = new List<SiRepresentanteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiRepresentanteDTO> GetByEmprcodi(int emprcodi)
        {
            List<SiRepresentanteDTO> entitys = new List<SiRepresentanteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByEmprcodi);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiRepresentanteDTO> GetByCriteria()
        {
            throw new NotImplementedException();
        }

        public int ActualizarRepresentanteGestionModificacion(int idRepresentante,
            string tipoRepresentante,
            string dni,
            string nombre,
            string apellido,
            string cargo,
            string telefono,
            string telefonoMovil,
            DateTime? fechaVigenciaPoder,
            string usuario,
            string email)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarRepresentanteGestionModificacion);

            dbProvider.AddInParameter(command, helper.Rptetiprepresentantelegal, DbType.String, tipoRepresentante);
            dbProvider.AddInParameter(command, helper.Rptedocidentidad, DbType.String, dni);
            dbProvider.AddInParameter(command, helper.Rptenombres, DbType.String, nombre);
            dbProvider.AddInParameter(command, helper.Rpteapellidos, DbType.String, apellido);
            dbProvider.AddInParameter(command, helper.Rptecargoempresa, DbType.String, cargo);            
            dbProvider.AddInParameter(command, helper.Rptetelefono, DbType.String, telefono);
            dbProvider.AddInParameter(command, helper.Rptetelfmovil, DbType.String, telefonoMovil);
            dbProvider.AddInParameter(command, helper.Rptefechavigenciapoder, DbType.DateTime, fechaVigenciaPoder);
            dbProvider.AddInParameter(command, helper.Rpteusumodificacion, DbType.String, usuario);
            dbProvider.AddInParameter(command, helper.Rptecorreoelectronico, DbType.String, email);
            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, idRepresentante);            
            
            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int ActualizarRepresentanteGestionModificacion(int idRepresentante, string telefono, string telefonoMovil)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarRepresentanteGestionModificacion_Agente);

            dbProvider.AddInParameter(command, helper.Rptetelefono, DbType.String, telefono);
            dbProvider.AddInParameter(command, helper.Rptetelfmovil, DbType.String, telefonoMovil);
            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, idRepresentante);

            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int ActualizarRepresentanteGestionModificacionVigenciaPoder(int idRepresentante, string telefono, string telefonoMovil, DateTime fechaVigenciaPoder)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarRepresentanteGestionModificacionVigenciaPoder);

            
            dbProvider.AddInParameter(command, helper.Rptetelefono, DbType.String, telefono);
            dbProvider.AddInParameter(command, helper.Rptetelfmovil, DbType.String, telefonoMovil);
            dbProvider.AddInParameter(command, helper.Rptefechavigenciapoder, DbType.DateTime, fechaVigenciaPoder);
            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, idRepresentante);

            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public List<SiRepresentanteDTO> ObtenerRepresentantesTitulares(int idEmpresa)
        {
            List<SiRepresentanteDTO> entitys = new List<SiRepresentanteDTO>();
            string query = string.Format(helper.SqlObtenerRepresentantesTitulares, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiRepresentanteDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iRpteindnotic = dr.GetOrdinal(helper.Rpteindnotic);
                    if (!dr.IsDBNull(iRpteindnotic)) entity.Rpteindnotic = dr.GetString(iRpteindnotic);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarNotificacion(int idRepresentante, string indicador)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarNotificacion);
            
            dbProvider.AddInParameter(command, helper.Rpteindnotic, DbType.String, indicador);            
            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, idRepresentante);

            dbProvider.ExecuteNonQuery(command);
            
        }

        public void ActualizarRepresentante(SiRepresentanteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarRepresentante);

            dbProvider.AddInParameter(command, helper.Rptenombres, DbType.String, entity.Rptenombres);
            dbProvider.AddInParameter(command, helper.Rpteapellidos, DbType.String, entity.Rpteapellidos);
            dbProvider.AddInParameter(command, helper.Rptecorreoelectronico, DbType.String, entity.Rptecorreoelectronico);
            dbProvider.AddInParameter(command, helper.Rptecargoempresa, DbType.String, entity.Rptecargoempresa);
            dbProvider.AddInParameter(command, helper.Rptetelefono, DbType.String, entity.Rptetelefono);
            dbProvider.AddInParameter(command, helper.Rptetelfmovil, DbType.String, entity.Rptetelfmovil);            
            dbProvider.AddInParameter(command, helper.Rpteusumodificacion, DbType.String, entity.Rpteusumodificacion);
            dbProvider.AddInParameter(command, helper.Rptefecmodificacion, DbType.DateTime, entity.Rptefecmodificacion);
            dbProvider.AddInParameter(command, helper.Rpteindnotic, DbType.String, entity.Rpteindnotic);
            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, entity.Rptecodi);

            dbProvider.ExecuteNonQuery(command);

        }

        public void DarBajaRepresentante(int idRepresentante, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDarBajaRepresentante);

            dbProvider.AddInParameter(command, helper.Rpteusumodificacion, DbType.String, usuario);
            dbProvider.AddInParameter(command, helper.Rptecodi, DbType.Int32, idRepresentante);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
