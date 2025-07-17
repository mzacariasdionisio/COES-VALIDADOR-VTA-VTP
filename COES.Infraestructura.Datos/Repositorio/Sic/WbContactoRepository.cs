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
    /// Clase de acceso a datos de la tabla WB_CONTACTO
    /// </summary>
    public class WbContactoRepository: RepositoryBase, IWbContactoRepository
    {
        public WbContactoRepository(string strConn): base(strConn)
        {
        }

        WbContactoHelper helper = new WbContactoHelper();

        public int Save(WbContactoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Contacnombre, DbType.String, entity.Contacnombre);
            dbProvider.AddInParameter(command, helper.Contacapellido, DbType.String, entity.Contacapellido);
            dbProvider.AddInParameter(command, helper.Contacemail, DbType.String, entity.Contacemail);
            dbProvider.AddInParameter(command, helper.Contaccargo, DbType.String, entity.Contaccargo);
            dbProvider.AddInParameter(command, helper.Contacempresa, DbType.String, entity.Contacempresa);
            dbProvider.AddInParameter(command, helper.Contactelefono, DbType.String, entity.Contactelefono);
            dbProvider.AddInParameter(command, helper.Contacmovil, DbType.String, entity.Contacmovil);
            dbProvider.AddInParameter(command, helper.Contaccomentario, DbType.String, entity.Contaccomentario);
            dbProvider.AddInParameter(command, helper.Contacarea, DbType.String, entity.Contacarea);
            dbProvider.AddInParameter(command, helper.Contacestado, DbType.String, entity.Contacestado);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.ContacFecRegistro, DbType.Date, entity.ContacFecRegistro);
            dbProvider.AddInParameter(command, helper.ContacDoc, DbType.String, entity.Contacdoc);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbContactoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Contacnombre, DbType.String, entity.Contacnombre);
            dbProvider.AddInParameter(command, helper.Contacapellido, DbType.String, entity.Contacapellido);
            dbProvider.AddInParameter(command, helper.Contacemail, DbType.String, entity.Contacemail);
            dbProvider.AddInParameter(command, helper.Contaccargo, DbType.String, entity.Contaccargo);
            dbProvider.AddInParameter(command, helper.Contacempresa, DbType.String, entity.Contacempresa);
            dbProvider.AddInParameter(command, helper.Contactelefono, DbType.String, entity.Contactelefono);
            dbProvider.AddInParameter(command, helper.Contacmovil, DbType.String, entity.Contacmovil);
            dbProvider.AddInParameter(command, helper.Contaccomentario, DbType.String, entity.Contaccomentario);
            dbProvider.AddInParameter(command, helper.Contacarea, DbType.String, entity.Contacarea);
            dbProvider.AddInParameter(command, helper.Contacestado, DbType.String, entity.Contacestado);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.ContacFecRegistro, DbType.Date, entity.ContacFecRegistro);
            dbProvider.AddInParameter(command, helper.ContacDoc, DbType.String, entity.Contacdoc);
            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, entity.Contaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int contaccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Contaccodi, DbType.Int32, contaccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbContactoDTO GetById(int contaccodi, string fuente)
        {
            string sql = string.Format(helper.SqlGetById, contaccodi, fuente);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            WbContactoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);                                      

                    int iEmprdire = dr.GetOrdinal(helper.Emprdire);
                    if (!dr.IsDBNull(iEmprdire)) entity.Emprdire = dr.GetString(iEmprdire);

                    int iFuente = dr.GetOrdinal(helper.Fuente);
                    if (!dr.IsDBNull(iFuente)) entity.Fuente = dr.GetString(iFuente);

                    int iUsereplegal = dr.GetOrdinal(helper.Userreplegal);
                    if (!dr.IsDBNull(iUsereplegal)) entity.Userreplegal = dr.GetString(iUsereplegal);

                    int iUsercontacto = dr.GetOrdinal(helper.Usercontacto);
                    if (!dr.IsDBNull(iUsercontacto)) entity.Usercontacto = dr.GetString(iUsercontacto);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprnomb = dr.GetOrdinal(helper.Tipoemprnomb);
                    if (!dr.IsDBNull(iTipoemprnomb)) entity.Tipoemprnomb = dr.GetString(iTipoemprnomb);

                    int iEmprcoes = dr.GetOrdinal(helper.Emprcoes);
                    if (!dr.IsDBNull(iEmprcoes)) entity.Emprcoes = dr.GetString(iEmprcoes);

                    int iContacdoc = dr.GetOrdinal(helper.ContacDoc);
                    if (!dr.IsDBNull(iContacdoc)) entity.Contacdoc = dr.GetString(iContacdoc);

                    int iContacFecRegistro = dr.GetOrdinal(helper.ContacFecRegistro);
                    if (!dr.IsDBNull(iContacFecRegistro)) entity.ContacFecRegistro = dr.GetDateTime(iContacFecRegistro);
                }
            }

            return entity;
        }

        public List<WbContactoDTO> List()
        {
            List<WbContactoDTO> entitys = new List<WbContactoDTO>();
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

        public List<WbContactoDTO> GetByCriteria(int? idTipoEmpresa, int? idEmpresa, string fuente, int? idComite, int? idProceso, int? idComiteLista)
        {
            List<WbContactoDTO> entitys = new List<WbContactoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idTipoEmpresa, idEmpresa, fuente, idComite, idProceso, idComiteLista);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbContactoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprdire = dr.GetOrdinal(helper.Emprdire);
                    if (!dr.IsDBNull(iEmprdire)) entity.Emprdire = dr.GetString(iEmprdire);

                    int iFuente = dr.GetOrdinal(helper.Fuente);
                    if (!dr.IsDBNull(iFuente)) entity.Fuente = dr.GetString(iFuente);

                    int iUsereplegal = dr.GetOrdinal(helper.Userreplegal);
                    if (!dr.IsDBNull(iUsereplegal)) entity.Userreplegal = dr.GetString(iUsereplegal);

                    int iUsercontacto = dr.GetOrdinal(helper.Usercontacto);
                    if (!dr.IsDBNull(iUsercontacto)) entity.Usercontacto = dr.GetString(iUsercontacto);

                    int iEmprcoes = dr.GetOrdinal(helper.Emprcoes);
                    if (!dr.IsDBNull(iEmprcoes)) entity.Emprcoes = dr.GetString(iEmprcoes);

                    entity.Contacnombre = entity.Contacnombre + " " + entity.Contacapellido;

                    int iNombrecomercial = dr.GetOrdinal(helper.Emprnombcomercial);
                    if (!dr.IsDBNull(iNombrecomercial)) entity.Nombrecomercial = dr.GetString(iNombrecomercial);

                    int iContacdoc = dr.GetOrdinal(helper.ContacDoc);
                    if (!dr.IsDBNull(iContacdoc)) entity.Contacdoc = dr.GetString(iContacdoc);

                    int iContacFecRegistro = dr.GetOrdinal(helper.ContacFecRegistro);
                    if (!dr.IsDBNull(iContacFecRegistro)) entity.ContacFecRegistro = dr.GetDateTime(iContacFecRegistro);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasContacto()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasContacto);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
