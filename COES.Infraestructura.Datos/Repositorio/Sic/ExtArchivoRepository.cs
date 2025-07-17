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
    /// Clase de acceso a datos de la tabla EXT_ARCHIVO
    /// </summary>
    public class ExtArchivoRepository: RepositoryBase, IExtArchivoRepository
    {
        public ExtArchivoRepository(string strConn): base(strConn)
        {
        }

        ExtArchivoHelper helper = new ExtArchivoHelper();

        /// <summary>
        /// Graba el registro del archivo con el codigo del envio insertado en el nombre del archivo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveUpload(ExtArchivoDTO entity, string nombreFile, string extension)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result) + 1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Earcopiado, DbType.String, entity.Earcopiado);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Earfecha, DbType.DateTime, entity.Earfecha);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Earip, DbType.String, entity.Earip);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Eararchruta, DbType.String, entity.Eararchruta);
            dbProvider.AddInParameter(command, helper.Eararchver, DbType.String, entity.Eararchver);
            dbProvider.AddInParameter(command, helper.Eararchtammb, DbType.Decimal, entity.Eararchtammb);
            dbProvider.AddInParameter(command, helper.Eararchnomb, DbType.String, nombreFile + id.ToString() + "." + extension);
            dbProvider.AddInParameter(command, helper.Etacodi, DbType.Int32, entity.Etacodi);
            dbProvider.AddInParameter(command, helper.Earcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(ExtArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result) + 1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Earcopiado, DbType.String, entity.Earcopiado);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Earfecha, DbType.DateTime, entity.Earfecha);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Earip, DbType.String, entity.Earip);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Eararchruta, DbType.String, entity.Eararchruta);
            dbProvider.AddInParameter(command, helper.Eararchver, DbType.String, entity.Eararchver);
            dbProvider.AddInParameter(command, helper.Eararchtammb, DbType.Decimal, entity.Eararchtammb);
            dbProvider.AddInParameter(command, helper.Eararchnomb, DbType.String, entity.Eararchnomb);
            dbProvider.AddInParameter(command, helper.Etacodi, DbType.Int32, entity.Etacodi);
            dbProvider.AddInParameter(command, helper.Earcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ExtArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            //dbProvider.AddInParameter(command, helper.Earcopiado, DbType.String, entity.Earcopiado);
            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            //dbProvider.AddInParameter(command, helper.Earfecha, DbType.DateTime, entity.Earfecha);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            //dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            //dbProvider.AddInParameter(command, helper.Earip, DbType.String, entity.Earip);
            //dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            //dbProvider.AddInParameter(command, helper.Eararchruta, DbType.String, entity.Eararchruta);
            //dbProvider.AddInParameter(command, helper.Eararchver, DbType.String, entity.Eararchver);
            //dbProvider.AddInParameter(command, helper.Eararchtammb, DbType.Decimal, entity.Eararchtammb);
            //dbProvider.AddInParameter(command, helper.Eararchnomb, DbType.String, entity.Eararchnomb);
            //dbProvider.AddInParameter(command, helper.Etacodi, DbType.Int32, entity.Etacodi);
            dbProvider.AddInParameter(command, helper.Earcodi, DbType.Int32, entity.Earcodi);
 
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int earcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Earcodi, DbType.Int32, earcodi);
            dbProvider.AddInParameter(command, helper.Earcodi, DbType.Int32, earcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ExtArchivoDTO GetById(int earcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Earcodi, DbType.Int32, earcodi);
            ExtArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ExtArchivoDTO> List()
        {
            List<ExtArchivoDTO> entitys = new List<ExtArchivoDTO>();
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

        public List<ExtArchivoDTO> GetByCriteria(int empresa, int estado, DateTime fechaInicial, DateTime fechaFinal)
        {
            List<ExtArchivoDTO> entitys = new List<ExtArchivoDTO>();
            string estadoFinal = string.Empty;
            switch (estado)
            {
                case 2:
                    estadoFinal = "(1,2,4)";
                    break;
                case 1:
                    estadoFinal = "(3,5)";
                    break;
                case 0:
                    estadoFinal = "0";
                    break;
            }
            string querySql = string.Format(helper.SqlGetByCriteria,fechaInicial.ToString(ConstantesBase.FormatoFecha), 
                fechaFinal.ToString(ConstantesBase.FormatoFecha), empresa,estado);
            DbCommand command = dbProvider.GetSqlStringCommand(querySql);
            ExtArchivoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprenomb = dr.GetOrdinal("emprnomb");
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);
                    int iEstenvnomb = dr.GetOrdinal("estenvnomb");
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);
                    int iUsername = dr.GetOrdinal("username");
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);
                    int iUsertlf = dr.GetOrdinal("usertlf");
                    if (!dr.IsDBNull(iUsertlf)) entity.Usertelf = dr.GetString(iUsertlf);
                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateMaxId(int idEnvio)
        {
            string sqlUpdate = string.Format(helper.SqlUpdateMaxId, idEnvio);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlUpdate);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<ExtArchivoDTO> ListaEnvioPagInterconexion(int empresa, int estado,DateTime fechaInicial, DateTime fechaFinal, int nroPagina, int nroFilas)
        {
            List<ExtArchivoDTO> entitys = new List<ExtArchivoDTO>();
            string estadoFinal = string.Empty;
            switch (estado)
            { 
                case 2:
                    estadoFinal = "(1,2,4)";
                    break;
                case 1:
                    estadoFinal = "(3,5)";
                    break;
                case 0:
                    estadoFinal = "0";
                    break;
            }
            string querySql = string.Format(helper.SqlListaEnvioPagInterconexion, fechaInicial.ToString(ConstantesBase.FormatoFecha),
                fechaFinal.ToString(ConstantesBase.FormatoFecha), empresa,estadoFinal,nroPagina,nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(querySql);
            ExtArchivoDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprenomb = dr.GetOrdinal("emprnomb");
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);
                    int iEstenvnomb = dr.GetOrdinal("estenvnomb");
                    if (!dr.IsDBNull(iEstenvnomb)) entity.Estenvnomb = dr.GetString(iEstenvnomb);
                    int iUsername = dr.GetOrdinal("username");
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);
                    int iUsertlf = dr.GetOrdinal("usertlf");
                    if (!dr.IsDBNull(iUsertlf)) entity.Usertelf = dr.GetString(iUsertlf);
                    int iEstado = dr.GetOrdinal("estado");
                    if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);
                    entitys.Add(entity);
                }
            }

            return entitys;        
        }

        public int TotalEnvioInterconexion(DateTime fechaini, DateTime fechafin,int empresa)
        {
            string sqlTotal = string.Format(helper.SqlTotalEnvioInterconexion,
               fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha),empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;           
        }
    }
}
