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
    /// Clase de acceso a datos de la tabla FW_USER
    /// </summary>
    public class FwUserRepository: RepositoryBase, IFwUserRepository
    {
        public FwUserRepository(string strConn): base(strConn)
        {
        }

        FwUserHelper helper = new FwUserHelper();

        public int Save(FwUserDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Username, DbType.String, entity.Username);
            dbProvider.AddInParameter(command, helper.Userpass, DbType.String, entity.Userpass);
            dbProvider.AddInParameter(command, helper.Userconn, DbType.Int32, entity.Userconn);
            dbProvider.AddInParameter(command, helper.Usermaxconn, DbType.Int32, entity.Usermaxconn);
            dbProvider.AddInParameter(command, helper.Userlogin, DbType.String, entity.Userlogin);
            dbProvider.AddInParameter(command, helper.Uservalidate, DbType.Int32, entity.Uservalidate);
            dbProvider.AddInParameter(command, helper.Usercheck, DbType.Int32, entity.Usercheck);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Userstate, DbType.String, entity.Userstate);
            dbProvider.AddInParameter(command, helper.Empresas, DbType.String, entity.Empresas);
            dbProvider.AddInParameter(command, helper.Userfcreacion, DbType.DateTime, entity.Userfcreacion);
            dbProvider.AddInParameter(command, helper.Userfactivacion, DbType.DateTime, entity.Userfactivacion);
            dbProvider.AddInParameter(command, helper.Userfbaja, DbType.DateTime, entity.Userfbaja);
            dbProvider.AddInParameter(command, helper.Usertlf, DbType.String, entity.Usertlf);
            dbProvider.AddInParameter(command, helper.Motivocontacto, DbType.String, entity.Motivocontacto);
            dbProvider.AddInParameter(command, helper.Usercargo, DbType.String, entity.Usercargo);
            dbProvider.AddInParameter(command, helper.Arealaboral, DbType.String, entity.Arealaboral);
            dbProvider.AddInParameter(command, helper.Useremail, DbType.String, entity.Useremail);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Usersolicitud, DbType.String, entity.Usersolicitud);
            dbProvider.AddInParameter(command, helper.Userindreprleg, DbType.String, entity.Userindreprleg);
            dbProvider.AddInParameter(command, helper.Userucreacion, DbType.String, entity.Userucreacion);
            dbProvider.AddInParameter(command, helper.Userad, DbType.String, entity.Userad);
            dbProvider.AddInParameter(command, helper.Usermovil, DbType.String, entity.Usermovil);
            dbProvider.AddInParameter(command, helper.Userflagpermiso, DbType.Int32, entity.Userflagpermiso);
            dbProvider.AddInParameter(command, helper.Userdoc, DbType.String, entity.Userdoc);
            dbProvider.AddInParameter(command, helper.Userfecregistro, DbType.DateTime, entity.Userfecregistro);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FwUserDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Username, DbType.String, entity.Username);
            dbProvider.AddInParameter(command, helper.Userpass, DbType.String, entity.Userpass);
            dbProvider.AddInParameter(command, helper.Userconn, DbType.Int32, entity.Userconn);
            dbProvider.AddInParameter(command, helper.Usermaxconn, DbType.Int32, entity.Usermaxconn);
            dbProvider.AddInParameter(command, helper.Userlogin, DbType.String, entity.Userlogin);
            dbProvider.AddInParameter(command, helper.Uservalidate, DbType.Int32, entity.Uservalidate);
            dbProvider.AddInParameter(command, helper.Usercheck, DbType.Int32, entity.Usercheck);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Userstate, DbType.String, entity.Userstate);
            dbProvider.AddInParameter(command, helper.Empresas, DbType.String, entity.Empresas);
            dbProvider.AddInParameter(command, helper.Userfcreacion, DbType.DateTime, entity.Userfcreacion);
            dbProvider.AddInParameter(command, helper.Userfactivacion, DbType.DateTime, entity.Userfactivacion);
            dbProvider.AddInParameter(command, helper.Userfbaja, DbType.DateTime, entity.Userfbaja);
            dbProvider.AddInParameter(command, helper.Usertlf, DbType.String, entity.Usertlf);
            dbProvider.AddInParameter(command, helper.Motivocontacto, DbType.String, entity.Motivocontacto);
            dbProvider.AddInParameter(command, helper.Usercargo, DbType.String, entity.Usercargo);
            dbProvider.AddInParameter(command, helper.Arealaboral, DbType.String, entity.Arealaboral);
            dbProvider.AddInParameter(command, helper.Useremail, DbType.String, entity.Useremail);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Usersolicitud, DbType.String, entity.Usersolicitud);
            dbProvider.AddInParameter(command, helper.Userindreprleg, DbType.String, entity.Userindreprleg);
            dbProvider.AddInParameter(command, helper.Userucreacion, DbType.String, entity.Userucreacion);
            dbProvider.AddInParameter(command, helper.Userad, DbType.String, entity.Userad);
            dbProvider.AddInParameter(command, helper.Usermovil, DbType.String, entity.Usermovil);
            dbProvider.AddInParameter(command, helper.Userflagpermiso, DbType.Int32, entity.Userflagpermiso);
            dbProvider.AddInParameter(command, helper.Userdoc, DbType.String, entity.Userdoc);
            dbProvider.AddInParameter(command, helper.Userfecregistro, DbType.DateTime, entity.Userfecregistro);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int usercode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);

            dbProvider.ExecuteNonQuery(command);
        }

        public FwUserDTO GetById(int usercode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);
            FwUserDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FwUserDTO> List()
        {
            List<FwUserDTO> entitys = new List<FwUserDTO>();
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

        public List<FwUserDTO> GetByCriteria()
        {
            List<FwUserDTO> entitys = new List<FwUserDTO>();
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

        #region Ficha Tecnica
        public List<FwUserDTO> ObtenerCorreos()
        {
            List<FwUserDTO> entitys = new List<FwUserDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCorreos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FwUserDTO entity = new FwUserDTO();

                    int iUsercode = dr.GetOrdinal(helper.Usercode);
                    if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

                    int iUsername = dr.GetOrdinal(helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iUseremail = dr.GetOrdinal(helper.Useremail);
                    if (!dr.IsDBNull(iUseremail)) entity.Useremail = dr.GetString(iUseremail);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
