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
    /// Clase de acceso a datos de la tabla SI_EMPRESA
    /// </summary>
    public class SiEmpresaRepository : RepositoryBase, ISiEmpresaRepository
    {
        public SiEmpresaRepository(string strConn)
            : base(strConn)
        {
        }

        SiEmpresaHelper helper = new SiEmpresaHelper();

        public int Save(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Emprdire, DbType.String, entity.Emprdire);
            dbProvider.AddInParameter(command, helper.Emprtele, DbType.String, entity.Emprtele);
            dbProvider.AddInParameter(command, helper.Emprnumedocu, DbType.String, entity.Emprnumedocu);
            dbProvider.AddInParameter(command, helper.Tipodocucodi, DbType.String, entity.Tipodocucodi);
            dbProvider.AddInParameter(command, helper.Emprruc, DbType.String, entity.Emprruc);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            dbProvider.AddInParameter(command, helper.Emprorden, DbType.Int32, entity.Emprorden);
            dbProvider.AddInParameter(command, helper.Emprdom, DbType.String, entity.Emprdom);
            dbProvider.AddInParameter(command, helper.Emprsein, DbType.String, entity.Emprsein);
            dbProvider.AddInParameter(command, helper.Emprrazsocial, DbType.String, entity.Emprrazsocial);
            dbProvider.AddInParameter(command, helper.Emprcoes, DbType.String, entity.Emprcoes);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Inddemanda, DbType.String, entity.Inddemanda);
            dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, entity.Emprestado);
            dbProvider.AddInParameter(command, helper.Emprdomiciliada, DbType.String, entity.Emprdomiciliada);
            dbProvider.AddInParameter(command, helper.Emprambito, DbType.String, entity.Emprambito);
            dbProvider.AddInParameter(command, helper.Emprrubro, DbType.Int32, entity.Emprrubro);
            dbProvider.AddInParameter(command, helper.Empragente, DbType.String, entity.Empragente);

            dbProvider.AddInParameter(command, helper.Emprnombrecomercial, DbType.String, entity.Emprnombrecomercial);
            dbProvider.AddInParameter(command, helper.Emprdomiciliolegal, DbType.String, entity.Emprdomiciliolegal);
            dbProvider.AddInParameter(command, helper.Emprsigla, DbType.String, entity.Emprsigla);
            dbProvider.AddInParameter(command, helper.Emprnumpartidareg, DbType.String, entity.Emprnumpartidareg);
            dbProvider.AddInParameter(command, helper.Emprtelefono, DbType.String, entity.Emprtelefono);
            dbProvider.AddInParameter(command, helper.Emprfax, DbType.String, entity.Emprfax);
            dbProvider.AddInParameter(command, helper.Emprpagweb, DbType.String, entity.Emprpagweb);
            dbProvider.AddInParameter(command, helper.Emprcartaadjunto, DbType.String, entity.Emprcartadjunto);
            dbProvider.AddInParameter(command, helper.Emprestadoregistro, DbType.String, entity.Emprestadoregistro);
            dbProvider.AddInParameter(command, helper.Emprfecinscripcion, DbType.DateTime, entity.Emprfechainscripcion);
            dbProvider.AddInParameter(command, helper.Emprcartaadjuntofilename, DbType.String, entity.Emprcartadjuntofilename);
            dbProvider.AddInParameter(command, helper.Emprcondicion, DbType.String, entity.Emprcondicion);
            dbProvider.AddInParameter(command, helper.Emprnroconstancia, DbType.Int32, entity.Emprnroconstancia);
            dbProvider.AddInParameter(command, helper.EmprNroRegistro, DbType.Int32, entity.Emprnroregistro);
            dbProvider.AddInParameter(command, helper.Emprusucreacion, DbType.String, entity.Emprusucreacion);
            dbProvider.AddInParameter(command, helper.Emprfeccreacion, DbType.DateTime, entity.Emprfeccreacion);
            dbProvider.AddInParameter(command, helper.Emprusumodificacion, DbType.String, entity.Emprusumodificacion);
            dbProvider.AddInParameter(command, helper.Emprfecmodificacion, DbType.DateTime, entity.Emprfecmodificacion);
            dbProvider.AddInParameter(command, helper.Emprindproveedor, DbType.String, entity.Emprindproveedor);

            dbProvider.ExecuteNonQueryAudit(command, entity.Lastuser);
            return id;
        }

        public void Update(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Emprdire, DbType.String, entity.Emprdire);
            dbProvider.AddInParameter(command, helper.Emprtele, DbType.String, entity.Emprtele);
            dbProvider.AddInParameter(command, helper.Emprnumedocu, DbType.String, entity.Emprnumedocu);
            dbProvider.AddInParameter(command, helper.Tipodocucodi, DbType.String, entity.Tipodocucodi);
            dbProvider.AddInParameter(command, helper.Emprruc, DbType.String, entity.Emprruc);
            //dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            //Comentado pues siempre inserta nulo
            dbProvider.AddInParameter(command, helper.Emprcodosinergmin, DbType.String, entity.EmprCodOsinergmin);
            dbProvider.AddInParameter(command, helper.Emprorden, DbType.Int32, entity.Emprorden);
            dbProvider.AddInParameter(command, helper.Emprdom, DbType.String, entity.Emprdom);
            dbProvider.AddInParameter(command, helper.Emprsein, DbType.String, entity.Emprsein);
            dbProvider.AddInParameter(command, helper.Emprrazsocial, DbType.String, entity.Emprrazsocial);
            dbProvider.AddInParameter(command, helper.Emprcoes, DbType.String, entity.Emprcoes);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Inddemanda, DbType.String, entity.Inddemanda);
            dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, entity.Emprestado);
            // Agregado por STS
            dbProvider.AddInParameter(command, helper.Emprdomiciliada, DbType.String, entity.Emprdomiciliada);
            dbProvider.AddInParameter(command, helper.Emprambito, DbType.String, entity.Emprambito);
            dbProvider.AddInParameter(command, helper.Emprrubro, DbType.Int32, entity.Emprrubro);
            dbProvider.AddInParameter(command, helper.Empragente, DbType.String, entity.Empragente);

            //dbProvider.AddInParameter(command, helper.Emprnombrecomercial, DbType.String, entity.Emprnombrecomercial);
            //dbProvider.AddInParameter(command, helper.Emprdomiciliolegal, DbType.String, entity.Emprdomiciliolegal);
            //dbProvider.AddInParameter(command, helper.Emprsigla, DbType.String, entity.Emprsigla);
            //dbProvider.AddInParameter(command, helper.Emprnumpartidareg, DbType.String, entity.Emprnumpartidareg);
            //dbProvider.AddInParameter(command, helper.Emprtelefono, DbType.String, entity.Emprtelefono);
            //dbProvider.AddInParameter(command, helper.Emprfax, DbType.String, entity.Emprfax);
            //dbProvider.AddInParameter(command, helper.Emprpagweb, DbType.String, entity.Emprpagweb);
            //dbProvider.AddInParameter(command, helper.Emprcartaadjunto, DbType.String, entity.Emprcartadjunto);
            //dbProvider.AddInParameter(command, helper.Emprestadoregistro, DbType.String, entity.Emprestadoregistro);
            //dbProvider.AddInParameter(command, helper.Emprfecinscripcion, DbType.DateTime, entity.Emprfechainscripcion);
            //dbProvider.AddInParameter(command, helper.Emprcartaadjuntofilename, DbType.String, entity.Emprcartadjuntofilename);
            //dbProvider.AddInParameter(command, helper.Emprcondicion, DbType.String, entity.Emprcondicion);
            //dbProvider.AddInParameter(command, helper.Emprnroconstancia, DbType.Int32, entity.Emprnroconstancia);
            //dbProvider.AddInParameter(command, helper.EmprNroRegistro, DbType.Int32, entity.Emprnroregistro);
            dbProvider.AddInParameter(command, helper.Emprusumodificacion, DbType.String, entity.Emprusumodificacion);
            dbProvider.AddInParameter(command, helper.Emprfecmodificacion, DbType.DateTime, entity.Emprfecmodificacion);
            dbProvider.AddInParameter(command, helper.Emprindproveedor, DbType.String, entity.Emprindproveedor);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQueryAudit(command, entity.Lastuser);
        }

        public void UpdateRI(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateRI);

            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Emprdire, DbType.String, entity.Emprdire);
            dbProvider.AddInParameter(command, helper.Emprtele, DbType.String, entity.Emprtele);
            dbProvider.AddInParameter(command, helper.Emprnumedocu, DbType.String, entity.Emprnumedocu);
            dbProvider.AddInParameter(command, helper.Tipodocucodi, DbType.String, entity.Tipodocucodi);
            dbProvider.AddInParameter(command, helper.Emprruc, DbType.String, entity.Emprruc);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            dbProvider.AddInParameter(command, helper.Emprorden, DbType.Int32, entity.Emprorden);
            dbProvider.AddInParameter(command, helper.Emprdom, DbType.String, entity.Emprdom);
            dbProvider.AddInParameter(command, helper.Emprsein, DbType.String, entity.Emprsein);
            dbProvider.AddInParameter(command, helper.Emprrazsocial, DbType.String, entity.Emprrazsocial);
            dbProvider.AddInParameter(command, helper.Emprcoes, DbType.String, entity.Emprcoes);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Inddemanda, DbType.String, entity.Inddemanda);
            dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, entity.Emprestado);
            // Agregado por STS
            dbProvider.AddInParameter(command, helper.Emprdomiciliada, DbType.String, entity.Emprdomiciliada);
            dbProvider.AddInParameter(command, helper.Emprambito, DbType.String, entity.Emprambito);
            dbProvider.AddInParameter(command, helper.Emprrubro, DbType.Int32, entity.Emprrubro);
            dbProvider.AddInParameter(command, helper.Empragente, DbType.String, entity.Empragente);

            dbProvider.AddInParameter(command, helper.Emprnombrecomercial, DbType.String, entity.Emprnombrecomercial);
            dbProvider.AddInParameter(command, helper.Emprdomiciliolegal, DbType.String, entity.Emprdomiciliolegal);
            dbProvider.AddInParameter(command, helper.Emprsigla, DbType.String, entity.Emprsigla);
            dbProvider.AddInParameter(command, helper.Emprnumpartidareg, DbType.String, entity.Emprnumpartidareg);
            dbProvider.AddInParameter(command, helper.Emprtelefono, DbType.String, entity.Emprtelefono);
            dbProvider.AddInParameter(command, helper.Emprfax, DbType.String, entity.Emprfax);
            dbProvider.AddInParameter(command, helper.Emprpagweb, DbType.String, entity.Emprpagweb);
            dbProvider.AddInParameter(command, helper.Emprcartaadjunto, DbType.String, entity.Emprcartadjunto);
            dbProvider.AddInParameter(command, helper.Emprestadoregistro, DbType.String, entity.Emprestadoregistro);
            dbProvider.AddInParameter(command, helper.Emprfecinscripcion, DbType.DateTime, entity.Emprfechainscripcion);
            dbProvider.AddInParameter(command, helper.Emprcartaadjuntofilename, DbType.String, entity.Emprcartadjuntofilename);
            dbProvider.AddInParameter(command, helper.Emprcondicion, DbType.String, entity.Emprcondicion);
            dbProvider.AddInParameter(command, helper.Emprnroconstancia, DbType.Int32, entity.Emprnroconstancia);
            dbProvider.AddInParameter(command, helper.EmprNroRegistro, DbType.Int32, entity.Emprnroregistro);
            dbProvider.AddInParameter(command, helper.Emprusumodificacion, DbType.String, entity.Emprusumodificacion);
            dbProvider.AddInParameter(command, helper.Emprfecmodificacion, DbType.DateTime, entity.Emprfecmodificacion);
            dbProvider.AddInParameter(command, helper.Emprindproveedor, DbType.String, entity.Emprindproveedor);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQueryAudit(command, entity.Lastuser);
        }

        public void Delete(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiEmpresaDTO GetById(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            SiEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iScadacodi = dr.GetOrdinal("SCADACODI");
                    if (!dr.IsDBNull(iScadacodi)) entity.Scadacodi = Convert.ToInt32(dr.GetValue(iScadacodi));

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprestado = dr.GetOrdinal(helper.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.EmpresaEstado = dr.GetString(iEmprestado);

                    int iEmprindproveedor = dr.GetOrdinal(helper.Emprindproveedor);
                    if (!dr.IsDBNull(iEmprindproveedor)) entity.Emprindproveedor = dr.GetString(iEmprindproveedor);
                }
            }

            return entity;
        }

        public List<SiEmpresaDTO> List(int tipoemprcodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public List<SiEmpresaDTO> ListGeneral()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListGeneral);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public List<SiEmpresaDTO> GetByCriteria(string tiposEmpresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlGetByCriteria, tiposEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> GetEmpresaSistemaPorTipo(string tiposEmpresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlGetEmpresaSistemaPorTipo, tiposEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasSEIN()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresasSein);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void ActualizarEstado(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEstado);

            dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, entity.Emprestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQueryAudit(command, entity.Lastuser);
        }


        public List<SiEmpresaDTO> GetByUser(string userlogin)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlGetByUser, userlogin);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasxCombustible(string tipocombustible)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string query = string.Format(helper.sqlGetEmpresasxCombustible, tipocombustible);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();
                    int iEmprnomb = dr.GetOrdinal("Emprnomb");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcodi = dr.GetOrdinal("Emprcodi");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<SiEmpresaDTO> ObtenerEmpresasxCombustiblexUsuario(string tipocombustible, string usuario)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string query = string.Format(helper.SqlGetEmpresasxCombustiblexUsuario, tipocombustible, usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();
                    int iEmprnomb = dr.GetOrdinal("Emprnomb");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcodi = dr.GetOrdinal("Emprcodi");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<SiEmpresaDTO> ListEmpresasGeneradoras()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListComboEmprGeneradora);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        public List<SiEmpresaDTO> ListEmpresasClientes()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListComboClientes);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<SiEmpresaDTO> ListEmprResponsables(string criterio)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmprResponsables);
            dbProvider.AddInParameter(command, "criterio", DbType.String, criterio);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public SiEmpresaDTO ListInfoCliente(string nombre)
        {
            SiEmpresaDTO entity = new SiEmpresaDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListInfoCliente);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombre);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiEmpresaDTO> ListadoComboEmpresasPorTipo(int tipoemprcodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListComboPorTipoEmpresa);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoemprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasHidro()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresasHidro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();
                    int iEmprnomb = dr.GetOrdinal("Emprnomb");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcodi = dr.GetOrdinal("Emprcodi");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasPtoMedicion()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresasPtoMedicion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();
                    int iEmprnomb = dr.GetOrdinal("Emprnomb");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcodi = dr.GetOrdinal("Emprcodi");
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresaFormato(int idFormato)
        {
            string query = string.Format(helper.SqlGetEmpresasFormato, idFormato);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
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

        public List<SiEmpresaDTO> ObtenerEmpresaFormatoMultiple(string idsFormatos)
        {
            string query = string.Format(helper.SqlGetEmpresasFormato, idsFormatos);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
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

        public List<SiEmpresaDTO> ObtenerEmpresaFormatoPorEstado(int idFormato, string estado)
        {
            string query = string.Format(helper.SqlGetEmpresasFormatoPorEstado, idFormato, estado);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
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

        public List<SiEmpresaDTO> ListarEmpresaXFormato(string idFormatos)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlGetEmpresasFormato, idFormatos);
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

        public List<SiEmpresaDTO> ObtenerEmpresaActivasFormato(int idFormato)
        {
            string query = string.Format(helper.SqlGetEmpresasActivaFormato, idFormato);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
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

        public List<SiEmpresaDTO> ObtenerEmpresaFormatoxUsuario(int idFormato, int IdEmpresa)
        {
            string query = string.Format(helper.SqlGetEmpresasFormatoxUsuario, idFormato, IdEmpresa);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
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
        public SiEmpresaDTO ListResponsable(string empresa)
        {
            SiEmpresaDTO entity = new SiEmpresaDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListResponsable);

            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, empresa);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int ObtenerNroRegistrosBusqueda(string nombre, string ruc, int idTipoEmpresa, string empresein, string emprestado)
        {
            string query = string.Format(helper.SqlObtenerNroRegistroBusqueda, nombre, ruc, idTipoEmpresa, empresein, emprestado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<SiEmpresaDTO> BuscarEmpresas(string nombre, string ruc, int idTipoEmpresa, string empresein, string emprestado,
            int nroPagina, int nroFilas)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string query = string.Format(helper.SqlBuscarEmpresas, nombre, ruc, idTipoEmpresa, empresein, emprestado, nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = helper.Create(dr);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprusucreacion = dr.GetOrdinal(helper.Emprusucreacion);
                    if (!dr.IsDBNull(iEmprusucreacion)) entity.Emprusucreacion = dr.GetString(iEmprusucreacion);

                    int iEmprfeccreacion = dr.GetOrdinal(helper.Emprfeccreacion);
                    if (!dr.IsDBNull(iEmprfeccreacion)) entity.Emprfeccreacion = dr.GetDateTime(iEmprfeccreacion);

                    int iEmprusumodificacion = dr.GetOrdinal(helper.Emprusumodificacion);
                    if (!dr.IsDBNull(iEmprusumodificacion)) entity.Emprusumodificacion = dr.GetString(iEmprusumodificacion);

                    int iEmprfecmodificacion = dr.GetOrdinal(helper.Emprfecmodificacion);
                    if (!dr.IsDBNull(iEmprfecmodificacion)) entity.Emprfecmodificacion = dr.GetDateTime(iEmprfecmodificacion);



                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ExportarEmpresas(string nombre, string nroRuc, int idTipoEmpresa, string empresaSein,
            string estado)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string query = string.Format(helper.SqlExportarEmpresas, nombre, nroRuc, idTipoEmpresa, empresaSein, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = helper.Create(dr);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasPuntoMedicion()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresasPuntosMedicion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));

                }
            }

            return entitys;
        }


        public void DarBajaEmpresa(int idEmpresa, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDarBajaEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);

            dbProvider.ExecuteNonQueryAudit(command, usuario);
        }

        public bool VerificarExistenciaPorNombre(string nombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerificarExistenciaNombre);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombre);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                int contador = Convert.ToInt32(result);
                if (contador > 0) return true;
            }

            return false;
        }

        public bool VerificarExistenciaPorRuc(string ruc)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerificarExistenciaRuc);
            dbProvider.AddInParameter(command, helper.Emprruc, DbType.String, ruc);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                int contador = Convert.ToInt32(result);
                if (contador > 0) return true;
            }

            return false;
        }

        public SiEmpresaDTO ObtenerEmpresaPorRuc(string ruc)
        {
            SiEmpresaDTO entity = null;
            string query = string.Format(helper.SqlObtenerEmpresaPorRuc, ruc);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                }
            }


            return entity;
        }

        public List<SiEmpresaDTO> ListaEmpresasPorTipoCumplimiento(int tipoemprcodi, int formato)
        {
            string sqlQuery = string.Format(this.helper.SqlListaEmpresasPorTipo, tipoemprcodi);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);
                    int iEtiqueta = dr.GetOrdinal(helper.Etiqueta);
                    if (!dr.IsDBNull(iEtiqueta)) entity.Etiqueta = dr.GetString(iEtiqueta);

                    if (entity.Etiqueta.Trim().Equals(string.Empty))
                    {
                        entity.Etiqueta = entity.Emprnomb;
                    }

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<SiEmpresaDTO> ListaEmailUsuariosEmpresas(string periodo, int formato, int modulo)
        {
            string sqlQuery = string.Format(this.helper.SqlListaEmailUsuariosEmpresas, periodo, formato, modulo);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iUserEmail = dr.GetOrdinal(helper.UserEmail);
                    if (!dr.IsDBNull(iUserEmail)) entity.UserEmail = dr.GetString(iUserEmail);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<SiEmpresaDTO> List(int minRowToFetch, int maxRowToFetch, SiEmpresaDTO siEmpresaDto)
        {
            var entities = new List<SiEmpresaDTO>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, string.IsNullOrEmpty(siEmpresaDto.Emprnomb) ? string.Empty : siEmpresaDto.Emprnomb);
            dbProvider.AddInParameter(command, SiEmpresaHelper.MaxRowToFetch, DbType.Int32, maxRowToFetch);
            dbProvider.AddInParameter(command, SiEmpresaHelper.MinRowToFetch, DbType.Int32, minRowToFetch);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    entities.Add(helper.Create(dataReader));
                }
            }
            return entities;
        }
        public SiEmpresaDTO GetById(SiEmpresaDTO siEmpresaDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, siEmpresaDto.Emprcodi);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                return dataReader.Read() ? helper.Create(dataReader) : null;
            }
        }
        public int GetTotalRecords(SiEmpresaDTO siEmpresaDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecords);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, siEmpresaDto.Emprnomb);
            var result = dbProvider.ExecuteScalar(command);
            return (result != null ? Convert.ToInt32(result) : 0);
        }

        public void GetAll(out List<string[]> registros, out Dictionary<int, MetadataDTO> metaDatosDictionary)
        {
            registros = new List<string[]>();
            metaDatosDictionary = new Dictionary<int, MetadataDTO>();
            var maxRowToFetch = GetTotalRecords(new SiEmpresaDTO());

            var command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, string.Empty);
            dbProvider.AddInParameter(command, SiEmpresaHelper.MaxRowToFetch, DbType.Int32, maxRowToFetch);
            dbProvider.AddInParameter(command, SiEmpresaHelper.MinRowToFetch, DbType.Int32, 1);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {

                helper.ObtenerMetaDatos(dataReader, ref metaDatosDictionary);

                while (dataReader.Read())
                {
                    var objeto = new object[dataReader.FieldCount];
                    var conteo = dataReader.GetValues(objeto);
                    var valores = new string[conteo];

                    for (int i = 0; i < conteo; i++)
                    {
                        var valor = objeto[i].ToString();
                        valores[i] = valor;
                    }

                    registros.Add(valores);

                }
            }
        }

        public void ActualizarRucFicticio(int id, string ruc)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarRucFicticio);
            dbProvider.AddInParameter(command, helper.Emprruc, DbType.String, ruc);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<SiEmpresaDTO> ListaEmpresasSuministrador()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(this.helper.SqlListaEmpresasSuministrador);
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

        public SiEmpresaDTO GetByCodOsinergmin(string codigo)
        {
            SiEmpresaDTO entity = new SiEmpresaDTO();

            string sqlQuery = string.Format(this.helper.SqlObtenerEmpresaOsinergmin, codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                }
            }
            return entity;
        }

        //- alpha.HDT - 31/10/2016: Cambio para atender el requerimiento. 
        public SiEmpresaDTO GetByIdOsinergmin(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByOsinergmin);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            SiEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(iEmprcodosinergmin)) entity.EmprCodOsinergmin = dr.GetString(iEmprcodosinergmin);
                }
            }

            return entity;
        }

        //- alpha.HDT - 31/10/2016: Cambio para atender el requerimiento. 
        public List<SiEmpresaDTO> ListarEmpresasPorTipo(List<int> listaIdTipoEmpresa)
        {
            string listaInTipoEmpresa = string.Empty;
            foreach (int idTipoEmpresa in listaIdTipoEmpresa)
            {
                if (listaInTipoEmpresa == string.Empty)
                {
                    listaInTipoEmpresa = idTipoEmpresa.ToString();
                }
                else
                {
                    listaInTipoEmpresa = listaInTipoEmpresa + ", " + idTipoEmpresa.ToString();
                }
            }

            List<SiEmpresaDTO> entities = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlGetByCriteria, listaInTipoEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            SiEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(iEmprcodosinergmin)) entity.EmprCodOsinergmin = dr.GetString(iEmprcodosinergmin);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        //- alpha.HDT - 31/10/2016: Cambio para atender el requerimiento. 
        public void UpdateOsinergmin(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateOsinergmin);

            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.Tipoemprcodi);
            dbProvider.AddInParameter(command, helper.Emprdire, DbType.String, entity.Emprdire);
            dbProvider.AddInParameter(command, helper.Emprtele, DbType.String, entity.Emprtele);
            dbProvider.AddInParameter(command, helper.Emprnumedocu, DbType.String, entity.Emprnumedocu);
            dbProvider.AddInParameter(command, helper.Tipodocucodi, DbType.String, entity.Tipodocucodi);
            dbProvider.AddInParameter(command, helper.Emprruc, DbType.String, entity.Emprruc);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            dbProvider.AddInParameter(command, helper.Emprorden, DbType.Int32, entity.Emprorden);
            dbProvider.AddInParameter(command, helper.Emprdom, DbType.String, entity.Emprdom);
            dbProvider.AddInParameter(command, helper.Emprsein, DbType.String, entity.Emprsein);
            dbProvider.AddInParameter(command, helper.Emprrazsocial, DbType.String, entity.Emprrazsocial);
            dbProvider.AddInParameter(command, helper.Emprcoes, DbType.String, entity.Emprcoes);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Inddemanda, DbType.String, entity.Inddemanda);
            dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, entity.Emprestado);
            dbProvider.AddInParameter(command, helper.Emprcodosinergmin, DbType.String, entity.EmprCodOsinergmin);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        //- alpha.JDEL - Inicio 03/11/2016: Cambio para atender el requerimiento.

        public SiEmpresaDTO GetByCodigoOsinergmin(string codOsinergmin)
        {
            string queryString = string.Format(helper.SqlGetByCodOsinergmin, codOsinergmin);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            SiEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprcodOsinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(iEmprcodOsinergmin)) entity.EmprCodOsinergmin = dr.GetString(iEmprcodOsinergmin);
                }
            }
            return entity;
        }

        //- JDEL Fin

        public List<SiEmpresaDTO> ObtenerEmpresasGeneracion()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(this.helper.SqlObtenerEmpresasGeneracion);

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
        // Inicio de Agregado - Sistema de Compensaciones
        public List<SiEmpresaDTO> ListaEmpresasCompensacion()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(this.helper.SqlListaEmpresasCompensacion);
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
        // Fin de Agregado - Sistema de Compensaciones

        //Agregado por STS
        public List<SiEmpresaDTO> ListaEmpresasSeleccionadas(SgdEstadisticasDTO filterEmpresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            String query = String.Format(helper.SqlListEmprDocSelected, filterEmpresa.FilterSein, filterEmpresa.FilterEmprCoes, filterEmpresa.FilterAmbito, filterEmpresa.FilterDomiciliada, filterEmpresa.FilterRubro, filterEmpresa.FilterAgente, filterEmpresa.FilterTipoEmpr);

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
        //- alpha.HDT - 14/07/2017: Cambio para atender el requerimiento. 
        public SiEmpresaDTO GetUsuarioLibreByRuc(string ruc)
        {
            string queryString = string.Format(helper.SqlGetByRuc, ruc);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            SiEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SiEmpresaDTO();
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        #region Indisponibilidades       
        public List<SiEmpresaDTO> ListarEmpresasConCentralTermoxUsuario(string userlogin)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlGetEmpresasConCentralTermoxUsuario, userlogin);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
        public List<SiEmpresaDTO> ListarEmpresasConCentralTermo()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlGetEmpresasConCentralTermo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
        #endregion
        public List<SiEmpresaDTO> ListarEmpresasxTipoEquipos(string tipoEquipos, string tiposEmpresa, string estadoEquipo)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlListarEmpresasxTipoEquipos, tipoEquipos, tiposEmpresa, estadoEquipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprnrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprnrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprnrazsocial);
                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));
                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);
                    int iEmprorden = dr.GetOrdinal(helper.Emprorden);
                    if (!dr.IsDBNull(iEmprorden)) entity.Emprorden = Convert.ToInt32(dr.GetValue(iEmprorden));
                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<SiEmpresaDTO> ObtenerEmpresasPorTipo(string tipoEmpresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlObtenerEmpresasPorTipo, tipoEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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

        #region Sistema de Rechazo Carga
        // Inicio de Agregado - Sistema de Rechazo Carga
        public List<SiEmpresaDTO> ListaEmpresasRechazoCarga(string empresa, int tipoEmpresa, string estadoRegistro)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string condicion = "";

            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + " AND NVL(EMPRRAZSOCIAL, EMPRNOMB) LIKE '%" + empresa.ToUpper() + "%'";
            }
            string queryString = string.Format(helper.ListaEmpresasRechazoCarga, condicion);


            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, tipoEmpresa);
            dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        // Fin de Agregado - Sistema de Rechazo Carga
        #endregion
        #region PR5
        public List<SiEmpresaDTO> ListarEmpresasXID(string idsEmpresas)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarEmpresasXID, idsEmpresas);
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
        #endregion

        public int ObtenerEmpresaMigra(int empresa, string fecha)
        {
            int emprsa = 0;

            string query = string.Format(helper.SqlEmpresaMigracion, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                emprsa = Convert.ToInt32(result);
            }

            return emprsa;
        }

        public List<SiEmpresaDTO> ObtenerEmpresasPorTipoSEIN(string tipoEmpresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlObtenerEmpresasPorTipoSEIN, tipoEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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

        #region transferencia equipos
        public List<SiEmpresaDTO> ObtenerEmpresasActivasBajas()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresasActivaBajas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        public List<SiEmpresaDTO> ObtenerIdNameEmpresasActivasBajas()
        {
            List<SiEmpresaDTO> entities = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetIdNameEmpresasActivaBajas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprcodosinergmin = dr.GetOrdinal(helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(iEmprcodosinergmin)) entity.EmprCodOsinergmin = dr.GetString(iEmprcodosinergmin);

                    int iEmprnombAnidado = dr.GetOrdinal(helper.EmprnombAnidado);
                    if (!dr.IsDBNull(iEmprnombAnidado)) entity.EmprnombAnidado = dr.GetString(iEmprnombAnidado);

                    entities.Add(entity);
                }
            }
            return entities;
        }

        /// <summary>
        /// Lista las empresas que han estado activas segun la fecha de consulta, la informaci�n del estado se obtiene de la tabla SI_MIGRACION*
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresaEstadoActual(DateTime fechaConsulta)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlListarEmpresaEstadoActual, fechaConsulta.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprestado = dr.GetOrdinal(helper.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprestadoFecha = dr.GetOrdinal(helper.EmprestadoFecha);
                    if (!dr.IsDBNull(iEmprestadoFecha)) entity.EmprestadoFecha = dr.GetDateTime(iEmprestadoFecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public List<SiEmpresaDTO> ListarEmpresasxCategoria(string catecodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlListarEmpresasxCategoria, catecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
        #endregion

        #region Intervenciones
        public List<SiEmpresaDTO> ListarComboEmpresas()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarComboEmpresasConsulta);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprestado = dr.GetOrdinal(helper.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarAbreviatura(int emprcodi, string emprabrev)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarAbreviatura);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, emprabrev);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        #endregion

        #region MonitoreoMME
        public List<SiEmpresaDTO> ListarEmpresaIntegranteMonitoreoMME(DateTime fechaIniPeriodo, DateTime fechaFinPeriodo)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string sql = string.Format(helper.SqlListarEmpresaIntegranteMonitoreoMME, fechaIniPeriodo.ToString(ConstantesBase.FormatoFecha), fechaFinPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprnrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprnrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprnrazsocial);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region FIT - SE�ALES NO DISPONIBLES - ASOCIACION DE EMPRESA

        public void UpdateAsociacionEmpresa(int emprcodi, int emprcodisp7)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateAsociacionEmpresa);

            dbProvider.AddInParameter(command, helper.Scadacodi, DbType.String, emprcodisp7);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void EliminarAsociacionEmpresa(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarAsociocionesEmpresa);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }


        #endregion

        #region Titularidad-Instalaciones-Empresas

        public List<SiEmpresaDTO> ListarEmpresaVigenteByRango(DateTime fechaIniPeriodo, DateTime fechaFinPeriodo)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string sql = string.Format(helper.SqlListarEmpresaVigenteByRango, fechaIniPeriodo.ToString(ConstantesBase.FormatoFecha), fechaFinPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprnrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprnrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprnrazsocial);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Mejoras IEOD

        public List<SiEmpresaDTO> ListarEmpresaPorOrigenPtoMedicion(int origlectcodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlListarEmpresaPorOrigenPtoMedicion, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iScadacodi = dr.GetOrdinal(helper.Scadacodi);
                    if (!dr.IsDBNull(iScadacodi)) entity.Scadacodi = Convert.ToInt32(dr.GetValue(iScadacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ListarEmpresaScada()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlListarEmpresaScada);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iScadacodi = dr.GetOrdinal(helper.Scadacodi);
                    if (!dr.IsDBNull(iScadacodi)) entity.Scadacodi = Convert.ToInt32(dr.GetValue(iScadacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region FIT - VALORIZACION DIARIA
        public List<SiEmpresaDTO> ListarEmpresasPorIds(string ids)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string sql = string.Format(helper.SqlListarEmpresasPorIDs, ids);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region DevolucionAportes
        public List<SiEmpresaDTO> ListarEmpresaDevolucion()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresaDevolucion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = helper.Create(dr);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region Proveedores

        public List<SiEmpresaDTO> ObtenerEmpresasProveedores(int tipoEmpresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string sql = string.Format(helper.SqlObtenerProveedores, tipoEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = helper.Create(dr);

                    int iEmprindproveedor = dr.GetOrdinal(helper.Emprindproveedor);
                    if (!dr.IsDBNull(iEmprindproveedor)) entity.Emprindproveedor = dr.GetString(iEmprindproveedor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<SiEmpresaDTO> ObtenerEmpresaPortalTramite(string tipoAgentes, int tipoEmpresa, string indicador, string ruc, string razonSocial,
            int nroPagina, int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string sql = string.Format(helper.SqlObtenerEmpresasPortalTramite, tipoAgentes, tipoEmpresa, indicador, ruc, razonSocial, nroPagina, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = helper.Create(dr);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprindusutramite = dr.GetOrdinal(helper.Emprindusutramite);
                    if (!dr.IsDBNull(iEmprindusutramite)) entity.Emprindusutramite = dr.GetString(iEmprindusutramite);

                    int iEmprfecusutramite = dr.GetOrdinal(helper.Emprfecusutramite);
                    if (!dr.IsDBNull(iEmprfecusutramite)) entity.Emprfecusutramite = dr.GetDateTime(iEmprfecusutramite);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistrosBusquedaTramite(string tipoAgente, int tipoEmpresa, string indicador, string ruc, string razonSocial)
        {
            string sql = string.Format(helper.SqlObtenerNroRegistrosBusquedaTramite, tipoAgente, tipoEmpresa, indicador, ruc, razonSocial);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public void ActualizarDatosUsuarioTramite(int emprcodi, string indicador, DateTime? fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarDatosUsuarioTramite);
            dbProvider.AddInParameter(command, helper.Emprindusutramite, DbType.String, indicador);
            dbProvider.AddInParameter(command, helper.Emprfecusutramite, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        #endregion

        #region Aplicativo Extranet CTAF

        public List<SiEmpresaDTO> ListarEmpresasEventoCTAF()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresasEventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region FICHA T�CNICA

        public List<SiEmpresaDTO> ListarEmpresasFT()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresasFT);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        /// <summary>
        /// Listado de empresas que tienen centrales activas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListadoEmpresasCentralesActivas()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresasCentralesActivas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #region EMPRESA MME

        public SiEmpresaMMEDTO GetByIdMME(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdMME);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            SiEmpresaMMEDTO entity = new SiEmpresaMMEDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iEmprmmecodi = dr.GetOrdinal(helper.emprmmecodi);
                    if (!dr.IsDBNull(iEmprmmecodi)) entity.Emprmmecodi = dr.GetInt32(iEmprmmecodi);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprestado = dr.GetOrdinal(helper.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.EmprTipoParticipante = dr.GetString(iTipoemprdesc);

                    int iEmprfecparticipacion = dr.GetOrdinal(helper.emprfecparticipacion);
                    if (!dr.IsDBNull(iEmprfecparticipacion)) entity.Emprfecparticipacion = dr.GetDateTime(iEmprfecparticipacion);

                    int iEmprfecretiro = dr.GetOrdinal(helper.emprfecretiro);
                    if (!dr.IsDBNull(iEmprfecretiro)) entity.Emprfecretiro = dr.GetDateTime(iEmprfecretiro);

                    int iEmprcomentario = dr.GetOrdinal(helper.emprcomentario);
                    if (!dr.IsDBNull(iEmprcomentario)) entity.Emprcomentario = dr.GetString(iEmprcomentario);

                    int iEmprusucreacion = dr.GetOrdinal(helper.Emprusucreacion);
                    if (!dr.IsDBNull(iEmprusucreacion)) entity.Emprusucreacion = dr.GetString(iEmprusucreacion);

                    int iEmprfeccreacion = dr.GetOrdinal(helper.Emprfeccreacion);
                    if (!dr.IsDBNull(iEmprfeccreacion)) entity.Emprfeccreacion = dr.GetDateTime(iEmprfeccreacion);

                    int iEmprusumodificacion = dr.GetOrdinal(helper.Emprusumodificacion);
                    if (!dr.IsDBNull(iEmprusumodificacion)) entity.Emprusumodificacion = dr.GetString(iEmprusumodificacion);

                    int iEmprfecmodificacion = dr.GetOrdinal(helper.Emprfecmodificacion);
                    if (!dr.IsDBNull(iEmprfecmodificacion)) entity.Emprfecmodificacion = dr.GetDateTime(iEmprfecmodificacion);
                }
            }

            return entity;
        }

        public List<SiEmpresaMMEDTO> BuscarEmpresasMME(int idTipoEmpresa)
        {
            List<SiEmpresaMMEDTO> entitys = new List<SiEmpresaMMEDTO>();
            string query = string.Format(helper.SqlBuscarEmpresasMME, idTipoEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaMMEDTO entity = new SiEmpresaMMEDTO();

                    int iEmprmmecodi = dr.GetOrdinal(helper.emprmmecodi);
                    if (!dr.IsDBNull(iEmprmmecodi)) entity.Emprmmecodi = dr.GetInt32(iEmprmmecodi);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprestado = dr.GetOrdinal(helper.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.EmprTipoParticipante = dr.GetString(iTipoemprdesc);

                    int iEmprfecparticipacion = dr.GetOrdinal(helper.emprfecparticipacion);
                    if (!dr.IsDBNull(iEmprfecparticipacion)) entity.Emprfecparticipacion = dr.GetDateTime(iEmprfecparticipacion);

                    int iEmprfecretiro = dr.GetOrdinal(helper.emprfecretiro);
                    if (!dr.IsDBNull(iEmprfecretiro)) entity.Emprfecretiro = dr.GetDateTime(iEmprfecretiro);

                    int iEmprcomentario = dr.GetOrdinal(helper.emprcomentario);
                    if (!dr.IsDBNull(iEmprcomentario)) entity.Emprcomentario = dr.GetString(iEmprcomentario);

                    int iEmprusucreacion = dr.GetOrdinal(helper.Emprusucreacion);
                    if (!dr.IsDBNull(iEmprusucreacion)) entity.Emprusucreacion = dr.GetString(iEmprusucreacion);

                    int iEmprfeccreacion = dr.GetOrdinal(helper.Emprfeccreacion);
                    if (!dr.IsDBNull(iEmprfeccreacion)) entity.Emprfeccreacion = dr.GetDateTime(iEmprfeccreacion);

                    int iEmprusumodificacion = dr.GetOrdinal(helper.Emprusumodificacion);
                    if (!dr.IsDBNull(iEmprusumodificacion)) entity.Emprusumodificacion = dr.GetString(iEmprusumodificacion);

                    int iEmprfecmodificacion = dr.GetOrdinal(helper.Emprfecmodificacion);
                    if (!dr.IsDBNull(iEmprfecmodificacion)) entity.Emprfecmodificacion = dr.GetDateTime(iEmprfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaMMEDTO> BuscarHistorialEmpresasMME(int Emprcodi)
        {
            List<SiEmpresaMMEDTO> entitys = new List<SiEmpresaMMEDTO>();
            string query = string.Format(helper.SqlBuscarHistorialEmpresasMME, Emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaMMEDTO entity = new SiEmpresaMMEDTO();

                    int iEmprfecparticipacion = dr.GetOrdinal(helper.emprfecparticipacion);
                    if (!dr.IsDBNull(iEmprfecparticipacion)) entity.Emprfecparticipacion = dr.GetDateTime(iEmprfecparticipacion);

                    int iEmprfecretiro = dr.GetOrdinal(helper.emprfecretiro);
                    if (!dr.IsDBNull(iEmprfecretiro)) entity.Emprfecretiro = dr.GetDateTime(iEmprfecretiro);

                    int iEmprcomentario = dr.GetOrdinal(helper.emprcomentario);
                    if (!dr.IsDBNull(iEmprcomentario)) entity.Emprcomentario = dr.GetString(iEmprcomentario);

                    int iEmprmmeestado = dr.GetOrdinal(helper.emprmmeestado);
                    if (!dr.IsDBNull(iEmprmmeestado)) entity.Emprmmeestado = dr.GetInt32(iEmprmmeestado);

                    int iEmprusucreacion = dr.GetOrdinal(helper.Emprusucreacion);
                    if (!dr.IsDBNull(iEmprusucreacion)) entity.Emprusucreacion = dr.GetString(iEmprusucreacion);

                    int iEmprfeccreacion = dr.GetOrdinal(helper.Emprfeccreacion);
                    if (!dr.IsDBNull(iEmprfeccreacion)) entity.Emprfeccreacion = dr.GetDateTime(iEmprfeccreacion);

                    int iEmprusumodificacion = dr.GetOrdinal(helper.Emprusumodificacion);
                    if (!dr.IsDBNull(iEmprusumodificacion)) entity.Emprusumodificacion = dr.GetString(iEmprusumodificacion);

                    int iEmprfecmodificacion = dr.GetOrdinal(helper.Emprfecmodificacion);
                    if (!dr.IsDBNull(iEmprfecmodificacion)) entity.Emprfecmodificacion = dr.GetDateTime(iEmprfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int SaveMME(SiEmpresaMMEDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdMME);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveMME);

            dbProvider.AddInParameter(command, helper.emprmmecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Tipoemprcodi, DbType.Int32, entity.TipoEmprcodi);
            dbProvider.AddInParameter(command, helper.emprfecparticipacion, DbType.DateTime, entity.Emprfecparticipacion);
            dbProvider.AddInParameter(command, helper.emprfecretiro, DbType.DateTime, entity.Emprfecretiro);
            dbProvider.AddInParameter(command, helper.emprcomentario, DbType.String, entity.Emprcomentario);
            dbProvider.AddInParameter(command, helper.Emprusucreacion, DbType.String, entity.Emprusucreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void UpdateMME(SiEmpresaMMEDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMME);
            
            dbProvider.AddInParameter(command, helper.Emprusumodificacion, DbType.String, entity.Emprusumodificacion);
            dbProvider.AddInParameter(command, helper.Emprfecmodificacion, DbType.DateTime, entity.Emprfecmodificacion);

            dbProvider.AddInParameter(command, helper.emprmmecodi, DbType.Int32, entity.Emprmmecodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public List<SiEmpresaDTO> BuscarEmpresasporParticipacion(int tipo)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            string query = string.Format(helper.SqlBuscarEmpresasporParticipante, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Ficha Tecnica 2023

        public List<SiEmpresaDTO> ListarPorTipo(int tipo)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            
            String sql = String.Format(helper.SqlListarPorTipo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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

        public List<SiEmpresaDTO> ListarEmpresaExtranetFT()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlListarEmpresaExtranetFT);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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

        #endregion

        #region Informes SGI

        public List<SiEmpresaDTO> ObtenerEmpresasPorFormato(string formatos)
        {
            string query = string.Format(helper.SqlGetEmpresasFormato, formatos);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
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

        public List<SiEmpresaDTO> ObtenerEmpresasSEINPorFormato(string formatos)
        {
            string query = string.Format(helper.SqlGetEmpresasSEINFormato, formatos);
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
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


        #endregion

        #region Demanda DPO - Iteracion 2
        public List<SiEmpresaDTO> ListaEmpresasByTipo(string tipo)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            String sql = String.Format(helper.SqlListaEmpresasByTipo, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
        #endregion

        #region Proteccion Equipos
        public List<SiEmpresaDTO> ListarMaestroEmpresasProteccion()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarMaestroEmpresasProteccion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprabrev = dr.GetString(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region CPPA.ASSETEC.2024 - Iteracion 1 - 2
        public List<SiEmpresaDTO> ListaEmpresasCPPA()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            String sql = String.Format(helper.SqlListaEmpresasCPPA);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnombConcatenado = dr.GetOrdinal(helper.EmprnombConcatenado);
                    if (!dr.IsDBNull(iEmprnombConcatenado)) entity.EmprnombConcatenado = dr.GetString(iEmprnombConcatenado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion


        #region GESPROTEC-20241031
        public List<SiEmpresaDTO> ListObtenerEmpresaSEINProtecciones()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String sql = String.Format(helper.SqlObtenerEmpresaSEINProtecciones);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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

        public List<SiEmpresaDTO> ListaEmpresasDescargaIntegrantes()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            String sql = String.Format(helper.SqlListaEmpresasDescargaIntegrantes);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
        #endregion
    }
}
