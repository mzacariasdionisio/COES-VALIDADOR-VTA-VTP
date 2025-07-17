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
    /// Clase de acceso a datos de la tabla SI_PERSONA
    /// </summary>
    public class SiPersonaRepository: RepositoryBase, ISiPersonaRepository
    {
        public SiPersonaRepository(string strConn): base(strConn)
        {
        }

        SiPersonaHelper helper = new SiPersonaHelper();

        public int Save(SiPersonaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tipopercodi, DbType.Int32, entity.Tipopercodi);
            dbProvider.AddInParameter(command, helper.Pernomb, DbType.String, entity.Pernomb);
            dbProvider.AddInParameter(command, helper.Perapellido, DbType.String, entity.Perapellido);
            dbProvider.AddInParameter(command, helper.Pertelefono, DbType.String, entity.Pertelefono);
            dbProvider.AddInParameter(command, helper.Perfax, DbType.String, entity.Perfax);
            dbProvider.AddInParameter(command, helper.Percargo, DbType.String, entity.Percargo);
            dbProvider.AddInParameter(command, helper.Pertitulo, DbType.String, entity.Pertitulo);
            dbProvider.AddInParameter(command, helper.Peremail, DbType.String, entity.Peremail);
            dbProvider.AddInParameter(command, helper.Percelular, DbType.String, entity.Percelular);
            dbProvider.AddInParameter(command, helper.Perg1, DbType.String, entity.Perg1);
            dbProvider.AddInParameter(command, helper.Perasunto, DbType.String, entity.Perasunto);
            dbProvider.AddInParameter(command, helper.Perg2, DbType.String, entity.Perg2);
            dbProvider.AddInParameter(command, helper.Perg3, DbType.String, entity.Perg3);
            dbProvider.AddInParameter(command, helper.Perg4, DbType.String, entity.Perg4);
            dbProvider.AddInParameter(command, helper.Perg5, DbType.String, entity.Perg5);
            dbProvider.AddInParameter(command, helper.Perg6, DbType.String, entity.Perg6);
            dbProvider.AddInParameter(command, helper.Perg7, DbType.String, entity.Perg7);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Perclientelibre, DbType.String, entity.Perclientelibre);
            dbProvider.AddInParameter(command, helper.Percomision, DbType.String, entity.Percomision);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Perestado, DbType.String, entity.Perestado);
            dbProvider.AddInParameter(command, helper.Perorden, DbType.Int32, entity.Perorden);
            dbProvider.AddInParameter(command, helper.Peradminrolturno, DbType.String, entity.Peradminrolturno);
            dbProvider.AddInParameter(command, helper.Perg8, DbType.String, entity.Perg8);
            dbProvider.AddInParameter(command, helper.Perg9, DbType.String, entity.Perg9);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiPersonaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, entity.Percodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tipopercodi, DbType.Int32, entity.Tipopercodi);
            dbProvider.AddInParameter(command, helper.Pernomb, DbType.String, entity.Pernomb);
            dbProvider.AddInParameter(command, helper.Perapellido, DbType.String, entity.Perapellido);
            dbProvider.AddInParameter(command, helper.Pertelefono, DbType.String, entity.Pertelefono);
            dbProvider.AddInParameter(command, helper.Perfax, DbType.String, entity.Perfax);
            dbProvider.AddInParameter(command, helper.Percargo, DbType.String, entity.Percargo);
            dbProvider.AddInParameter(command, helper.Pertitulo, DbType.String, entity.Pertitulo);
            dbProvider.AddInParameter(command, helper.Peremail, DbType.String, entity.Peremail);
            dbProvider.AddInParameter(command, helper.Percelular, DbType.String, entity.Percelular);
            dbProvider.AddInParameter(command, helper.Perg1, DbType.String, entity.Perg1);
            dbProvider.AddInParameter(command, helper.Perasunto, DbType.String, entity.Perasunto);
            dbProvider.AddInParameter(command, helper.Perg2, DbType.String, entity.Perg2);
            dbProvider.AddInParameter(command, helper.Perg3, DbType.String, entity.Perg3);
            dbProvider.AddInParameter(command, helper.Perg4, DbType.String, entity.Perg4);
            dbProvider.AddInParameter(command, helper.Perg5, DbType.String, entity.Perg5);
            dbProvider.AddInParameter(command, helper.Perg6, DbType.String, entity.Perg6);
            dbProvider.AddInParameter(command, helper.Perg7, DbType.String, entity.Perg7);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Perclientelibre, DbType.String, entity.Perclientelibre);
            dbProvider.AddInParameter(command, helper.Percomision, DbType.String, entity.Percomision);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Perestado, DbType.String, entity.Perestado);
            dbProvider.AddInParameter(command, helper.Perorden, DbType.Int32, entity.Perorden);
            dbProvider.AddInParameter(command, helper.Peradminrolturno, DbType.String, entity.Peradminrolturno);
            dbProvider.AddInParameter(command, helper.Perg8, DbType.String, entity.Perg8);
            dbProvider.AddInParameter(command, helper.Perg9, DbType.String, entity.Perg9);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int percodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int percodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiPersonaDTO GetById(int percodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Percodi, DbType.Int32, percodi);
            SiPersonaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiPersonaDTO> List()
        {
            List<SiPersonaDTO> entitys = new List<SiPersonaDTO>();
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

        public SiPersonaDTO GetByCriteria(int usercode)
        {
            SiPersonaDTO entity = null;
            string sqlstr = string.Format(helper.SqlGetByCriteria, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlstr);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiPersonaDTO> GetByCriteriaArea(int areacodi)
        {
            string query = string.Format(helper.SqlGetByCriteriaArea, areacodi);

            List<SiPersonaDTO> entitys = new List<SiPersonaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        

        public string GetCargo(string Nombre)
        {

            String sql = String.Format(this.helper.Cargo,Nombre);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null) return Convert.ToString(result);

            return "";

        }

        public string GetArea(string Nombre)
        {

            String sql = String.Format(this.helper.Area, Nombre);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null) return Convert.ToString(result);

            return "";

        }


        public string GetTelefono(string Nombre)
        {

            String sql = String.Format(this.helper.Telefono, Nombre);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null) return Convert.ToString(result);

            return "";


        }


        public string GetMail(string Nombre)
        {

            String sql = String.Format(this.helper.Mail, Nombre);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null) return Convert.ToString(result);

            return "";


        }

        public List<SiPersonaDTO> ListaEspecialistasSME()
        {
            List<SiPersonaDTO> entitys = new List<SiPersonaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaEspecialistasSME);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #region MigracionSGOCOES-GrupoB
        public List<SiPersonaDTO> ListaPersonalRol(string areacodi, DateTime fecIni, DateTime fecFin)
        {
            List<SiPersonaDTO> entitys = new List<SiPersonaDTO>();
            string query = string.Format(helper.SqlListaPersonalRol, areacodi, fecIni.ToString(ConstantesBase.FormatoFechaPE), fecFin.ToString(ConstantesBase.FormatoFechaPE));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            SiPersonaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiPersonaDTO();

                    int iPercodi = dr.GetOrdinal(this.helper.Percodi);
                    if (!dr.IsDBNull(iPercodi)) entity.Percodi = dr.GetInt32(iPercodi);

                    int iPernomb = dr.GetOrdinal(this.helper.Pernomb);
                    if (!dr.IsDBNull(iPernomb)) entity.Pernomb = dr.GetString(iPernomb);

                    int iPerdni = dr.GetOrdinal(this.helper.Perdni);
                    if (!dr.IsDBNull(iPerdni)) entity.Perdni = dr.GetString(iPerdni);

                    int iPerorden = dr.GetOrdinal(this.helper.Perorden);
                    if (!dr.IsDBNull(iPerorden)) entity.Perorden = dr.GetInt32(iPerorden);

                    int iPertelefono = dr.GetOrdinal(helper.Pertelefono);
                    if (!dr.IsDBNull(iPertelefono)) entity.Pertelefono = dr.GetString(iPertelefono);

                    int iPeremail = dr.GetOrdinal(helper.Peremail);
                    if (!dr.IsDBNull(iPeremail)) entity.Peremail = dr.GetString(iPeremail);

                    int iPercargo = dr.GetOrdinal(helper.Percargo);
                    if (!dr.IsDBNull(iPercargo)) entity.Percargo = dr.GetString(iPercargo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
