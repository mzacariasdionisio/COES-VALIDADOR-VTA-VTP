using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class SiEmpresaRIRepository : RepositoryBase, ISiEmpresaRIRepository
    {
        private string strConexion;
        public SiEmpresaRIRepository(string strConn)
            : base(strConn)
        {
            strConexion = strConn;
        }


        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction)conn.BeginTransaction();
        }

        SiEmpresaRIHelper helper = new SiEmpresaRIHelper();
        SiEmpresaHelper helperEmpresa = new SiEmpresaHelper();

        public int ObtenerNroConstancia()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxNroRegistro);

            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int ObtenerNroRegistro()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxNroConstancia);

            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        /// <summary>
        /// Devuelve la empresa para modoficación en la gestión de modificación de identificación
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public SiEmpresaDTO GetByIdGestionModificacion(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdGestionModificacion);

            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, emprcodi);
            SiEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iEmprnroregistro = dr.GetOrdinal(helperEmpresa.EmprNroRegistro);
                    if (!dr.IsDBNull(iEmprnroregistro)) entity.Emprnroregistro = Convert.ToInt32(dr.GetValue(iEmprnroregistro));

                }
            }

            return entity;
        }

        /// <summary>
        /// Devuelve cabecera de a solicitud por codigo de empresa
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public SiEmpresaDTO GetCabeceraSolicitudById(int emprcodi)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetCabeceraSolicitudById);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, emprcodi);
            dbProvider.ExecuteNonQuery(command);

            SiEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SiEmpresaDTO();

                    // Inicio - Fit Software Registro Integrantes

                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    // Fin - Fit Software Registro Integrantes
                }
            }
            return entity;

        }

        public SiEmpresaDTO GetByIdConRevision(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdConRevision);


            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, emprcodi);
            SiEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helperEmpresa.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helperEmpresa.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

                    int iEmprdire = dr.GetOrdinal(helperEmpresa.Emprdire);
                    if (!dr.IsDBNull(iEmprdire)) entity.Emprdire = dr.GetString(iEmprdire);

                    int iEmprtele = dr.GetOrdinal(helperEmpresa.Emprtele);
                    if (!dr.IsDBNull(iEmprtele)) entity.Emprtele = dr.GetString(iEmprtele);

                    int iEmprnumedocu = dr.GetOrdinal(helperEmpresa.Emprnumedocu);
                    if (!dr.IsDBNull(iEmprnumedocu)) entity.Emprnumedocu = dr.GetString(iEmprnumedocu);

                    int iTipodocucodi = dr.GetOrdinal(helperEmpresa.Tipodocucodi);
                    if (!dr.IsDBNull(iTipodocucodi)) entity.Tipodocucodi = dr.GetString(iTipodocucodi);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprabrev = dr.GetOrdinal(helperEmpresa.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprorden = dr.GetOrdinal(helperEmpresa.Emprorden);
                    if (!dr.IsDBNull(iEmprorden)) entity.Emprorden = Convert.ToInt32(dr.GetValue(iEmprorden));

                    int iEmprdom = dr.GetOrdinal(helperEmpresa.Emprdom);
                    if (!dr.IsDBNull(iEmprdom)) entity.Emprdom = dr.GetString(iEmprdom);

                    int iEmprsein = dr.GetOrdinal(helperEmpresa.Emprsein);
                    if (!dr.IsDBNull(iEmprsein)) entity.Emprsein = dr.GetString(iEmprsein);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprcoes = dr.GetOrdinal(helperEmpresa.Emprcoes);
                    if (!dr.IsDBNull(iEmprcoes)) entity.Emprcoes = dr.GetString(iEmprcoes);

                    int iLastuser = dr.GetOrdinal(helperEmpresa.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(helperEmpresa.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iCompcode = dr.GetOrdinal(helperEmpresa.Compcode);
                    if (!dr.IsDBNull(iCompcode)) entity.Compcode = Convert.ToInt32(dr.GetValue(iCompcode));

                    int iInddemanda = dr.GetOrdinal(helperEmpresa.Inddemanda);
                    if (!dr.IsDBNull(iInddemanda)) entity.Inddemanda = dr.GetString(iInddemanda);

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int IEmprcodosinergmin = dr.GetOrdinal(helperEmpresa.Emprcodosinergmin);
                    if (!dr.IsDBNull(IEmprcodosinergmin)) entity.EmprCodOsinergmin = dr.GetString(IEmprcodosinergmin);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iReviCodiSGI = dr.GetOrdinal(helper.ReviCodiSGI);
                    if (!dr.IsDBNull(iReviCodiSGI)) entity.ReviCodiSGI = Convert.ToInt32(dr.GetValue(iReviCodiSGI));

                    int iReviCodiDJR = dr.GetOrdinal(helper.ReviCodiDJR);
                    if (!dr.IsDBNull(iReviCodiDJR)) entity.ReviCodiDJR = Convert.ToInt32(dr.GetValue(iReviCodiDJR));

                    int iReviEstadoSGI = dr.GetOrdinal(helper.ReviEstadoSGI);
                    if (!dr.IsDBNull(iReviEstadoSGI)) entity.ReviEstadoSGI = dr.GetString(iReviEstadoSGI);

                    int iReviEstadoDJR = dr.GetOrdinal(helper.ReviEstadoDJR);
                    if (!dr.IsDBNull(iReviEstadoDJR)) entity.ReviEstadoDJR = dr.GetString(iReviEstadoDJR);

                    int iEmprnroconstancia = dr.GetOrdinal(helperEmpresa.Emprnroconstancia);
                    if (!dr.IsDBNull(iEmprnroconstancia)) entity.Emprnroconstancia = Convert.ToInt32(dr.GetValue(iEmprnroconstancia));

                    int iFecregistro = dr.GetOrdinal(helperEmpresa.Fecregistro);
                    if (!dr.IsDBNull(iFecregistro)) entity.Fecregistro = dr.GetDateTime(iFecregistro);

                    int iEmprfecinscripcion = dr.GetOrdinal(helperEmpresa.Emprfecinscripcion);
                    if (!dr.IsDBNull(iEmprfecinscripcion)) { entity.EmprfechainscripcionR = dr.GetDateTime(iEmprfecinscripcion); }
                    else entity.EmprfechainscripcionR = dr.GetDateTime(dr.GetOrdinal(helperEmpresa.Fecregistro));

                    int iEmprnroregistro = dr.GetOrdinal(helperEmpresa.EmprNroRegistro);
                    if (!dr.IsDBNull(iEmprnroregistro)) entity.Emprnroregistro = Convert.ToInt32(dr.GetValue(iEmprnroregistro));

                    //int iSoliFecModificacion = dr.GetOrdinal(helperEmpresa.SoliFecModificacion);
                    //if (!dr.IsDBNull(iSoliFecModificacion)) entity.SoliFecModificacion = dr.GetDateTime(iSoliFecModificacion);

                }
            }

            return entity;
        }


        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public int ActualizarEmpresaGestionModificacion(int idEmpresa, string nombreComercial, string razonSocial,
            string domicilioLegal, string sigla, string nroPartida, string telefono, string fax, string paginaWeb, string nroRegistro)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEmpresaGestionModificacion);

            dbProvider.AddInParameter(command, helperEmpresa.Emprnombrecomercial, DbType.String, nombreComercial);
            dbProvider.AddInParameter(command, helperEmpresa.Emprrazsocial, DbType.String, razonSocial);
            dbProvider.AddInParameter(command, helperEmpresa.Emprdomiciliolegal, DbType.String, domicilioLegal);
            dbProvider.AddInParameter(command, helperEmpresa.Emprsigla, DbType.String, sigla);
            dbProvider.AddInParameter(command, helperEmpresa.Emprnumpartidareg, DbType.String, nroPartida);
            dbProvider.AddInParameter(command, helperEmpresa.Emprtelefono, DbType.String, telefono);
            dbProvider.AddInParameter(command, helperEmpresa.Emprfax, DbType.String, fax);
            dbProvider.AddInParameter(command, helperEmpresa.Emprpagweb, DbType.String, paginaWeb);
            dbProvider.AddInParameter(command, helperEmpresa.EmprNroRegistro, DbType.Int32, nroRegistro);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, idEmpresa);

            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int ActualizarEmpresaGestionModificacion(int idEmpresa, string domicilioLegal, string telefono, string fax, string paginaWeb)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEmpresaGestionModificacion_Agente);

            dbProvider.AddInParameter(command, helperEmpresa.Emprdomiciliolegal, DbType.String, domicilioLegal);
            dbProvider.AddInParameter(command, helperEmpresa.Emprtelefono, DbType.String, telefono);
            dbProvider.AddInParameter(command, helperEmpresa.Emprfax, DbType.String, fax);
            dbProvider.AddInParameter(command, helperEmpresa.Emprpagweb, DbType.String, paginaWeb);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, idEmpresa);

            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        public int ActualizarEmpresaCambioDenom(int idEmpresa, string nombreComercial, string razonSocial, string sigla)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEmpresaCambioDenom);

            dbProvider.AddInParameter(command, helperEmpresa.Emprnombrecomercial, DbType.String, nombreComercial);
            dbProvider.AddInParameter(command, helperEmpresa.Emprrazsocial, DbType.String, razonSocial);
            dbProvider.AddInParameter(command, helperEmpresa.Emprsigla, DbType.String, sigla);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, idEmpresa);

            int count = dbProvider.ExecuteNonQuery(command);
            return count;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarIntegrantesporTipo(string tipoemprcodi, string nombre, int nroPage, int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            String query = String.Format(helper.SqlListarIntegrantesporTipo, tipoemprcodi, nombre, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iEmprnroconstancia = dr.GetOrdinal(helperEmpresa.Emprnroconstancia);
                    if (!dr.IsDBNull(iEmprnroconstancia)) entity.Emprnroconstancia = Convert.ToInt32(dr.GetValue(iEmprnroconstancia));

                    int iEmprnroregistro = dr.GetOrdinal(helperEmpresa.EmprNroRegistro);
                    if (!dr.IsDBNull(iEmprnroregistro)) entity.Emprnroregistro = Convert.ToInt32(dr.GetValue(iEmprnroregistro));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalListarIntegrantesporTipo(string tipoemprcodi, string nombre)
        {
            String query = String.Format(helper.SqlNroRegListarIntegrantesporTipo, tipoemprcodi, nombre);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="emprnomb">empresa</param>
        /// <param name="rptetiprepresentantelegal">tipo rpte legal</param>
        /// <param name="rptetiprepresentantelegalcontacto">tipo rpte legal, contacto</param>
        /// <param name="estado">estado</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarporTipoNombreRepresentante(string tipoemprcodi,
            string emprnomb,
            string rptetiprepresentantelegal,
            string rptetiprepresentantelegalcontacto,
            string estado,
            DateTime fecha,
            int nroPage,
            int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            String query = "";

            //No se esta usando Fecha Vigencia
            if (fecha == DateTime.MaxValue)
            {
                query = String.Format(helper.SqlListarporTipoNombreRepresentante,
                    tipoemprcodi,
                    emprnomb,
                    rptetiprepresentantelegal,
                    rptetiprepresentantelegalcontacto,
                    estado,
                    nroPage,
                    pageSize);
            }
            else
            {
                query = String.Format(helper.SqlListarporTipoNombreRepresentanteconFechaVigencia,
                    tipoemprcodi,
                    emprnomb,
                    rptetiprepresentantelegal,
                    rptetiprepresentantelegalcontacto,
                    estado,
                    fecha.ToString(ConstantesBase.FormatoFecha),
                    nroPage,
                    pageSize);

            }

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iRpteTelfMovil = dr.GetOrdinal(helper.RpteTelfMovil);
                    if (!dr.IsDBNull(iRpteTelfMovil)) entity.RpteTelfMovil = dr.GetString(iRpteTelfMovil);

                    int iRpteTipo = dr.GetOrdinal(helper.RpteTipo);
                    if (!dr.IsDBNull(iRpteTipo)) entity.RpteTipo = dr.GetString(iRpteTipo);

                    int iRpteTipRepresentanteLegal = dr.GetOrdinal(helper.RpteTipRepresentanteLegal);
                    if (!dr.IsDBNull(iRpteTipRepresentanteLegal)) entity.RpteTipRepresentanteLegal = dr.GetString(iRpteTipRepresentanteLegal);

                    int iRpteDocIdentidad = dr.GetOrdinal(helper.RpteDocIdentidad);
                    if (!dr.IsDBNull(iRpteDocIdentidad)) entity.RpteDocIdentidad = dr.GetString(iRpteDocIdentidad);

                    int iRpteCargoEmpresa = dr.GetOrdinal(helper.RpteCargoEmpresa);
                    if (!dr.IsDBNull(iRpteCargoEmpresa)) entity.RpteCargoEmpresa = dr.GetString(iRpteCargoEmpresa);

                    int iRpteBaja = dr.GetOrdinal(helper.RpteBaja);
                    if (!dr.IsDBNull(iRpteBaja)) entity.RpteBaja = dr.GetString(iRpteBaja);

                    int iRpteFechaVigenciaPoder = dr.GetOrdinal(helper.RpteFechaVigenciaPoder);
                    if (!dr.IsDBNull(iRpteFechaVigenciaPoder)) entity.RpteFechaVigenciaPoder = dr.GetDateTime(iRpteFechaVigenciaPoder);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de rpte legales según criterio
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="emprnomb">nombre de empresa</param>
        /// <param name="rptetiprepresentantelegal">tipo representante legal</param>
        /// <param name="rptetiprepresentantelegalcontacto">tipo representante legal, contacto</param>
        /// <param name="estado">estado</param>
        /// /// <param name="fecha">Fecha Vigencia Poder</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalListarporTipoNombreRepresentante(string tipoemprcodi,
            string emprnomb,
            string rptetiprepresentantelegal,
            string rptetiprepresentantelegalcontacto,
            string estado,
            DateTime fecha)
        {
            String query = "";
            //No se esta usando Fecha Vigencia
            if (fecha == DateTime.MaxValue)
            {
                query = String.Format(helper.SqlNroRegListarporTipoNombreRepresentante,
                    tipoemprcodi,
                    emprnomb,
                    rptetiprepresentantelegal,
                    rptetiprepresentantelegalcontacto,
                    estado);

            }
            else
            {
                query = String.Format(helper.SqlNroRegListarporTipoNombreRepresentanteconFechaVigencia,
                    tipoemprcodi,
                    emprnomb,
                    rptetiprepresentantelegal,
                    rptetiprepresentantelegalcontacto,
                    estado,
                    fecha.ToString(ConstantesBase.FormatoFecha));
            }


            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresas(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string nombre,
            string tipomodalidad,
            int nroPage,
            int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarIntegrantesporFechaTipo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                nombre,
                tipomodalidad,
                nroPage,
                pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    //REQ 2024-002848 - Mapeo de campo TipoAgente como cadena vacía : ''
                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarEmpresas(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string nombre,
            string tipomodalidad)
        {
            String query = String.Format(helper.SqlNroRegListarIntegrantesporFechaTipo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                nombre,
                tipomodalidad);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }


        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string nombre,
            string tipomodalidad)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarIntegrantesporFechaTipoFiltroXls,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                nombre,
                tipomodalidad);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEvolucionEmpresas(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud,
            int nroPage,
            int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = String.Format(helper.SqlListarIntegrantesporFechaTipoSolicitud,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud,
                nroPage,
                pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);

                    int iTisoNombre = dr.GetOrdinal(helper.TisoNombre);
                    if (!dr.IsDBNull(iTisoNombre)) entity.TisoNombre = dr.GetString(iTisoNombre);

                    int iSoliEstado = dr.GetOrdinal(helper.SoliEstado);
                    if (!dr.IsDBNull(iSoliEstado)) entity.SoliEstado = dr.GetString(iSoliEstado);

                    int iSoliFecSolicitud = dr.GetOrdinal(helper.SoliFecSolicitud);
                    if (!dr.IsDBNull(iSoliFecSolicitud)) entity.SoliFecSolicitud = dr.GetDateTime(iSoliFecSolicitud);

                    int iSoliFecEnviado = dr.GetOrdinal(helper.SoliFecEnviado);
                    if (!dr.IsDBNull(iSoliFecEnviado)) entity.SoliFecEnviado = dr.GetDateTime(iSoliFecEnviado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarEvolucionEmpresas(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud)
        {
            string query = string.Format(helper.SqlNroRegListarIntegrantesporFechaTipoSolicitud,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEvolucionEmpresasFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarIntegrantesporFechaTipoFiltroXlsSolicitud,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);

                    int iTisoNombre = dr.GetOrdinal(helper.TisoNombre);
                    if (!dr.IsDBNull(iTisoNombre)) entity.TisoNombre = dr.GetString(iTisoNombre);

                    int iSoliEstado = dr.GetOrdinal(helper.SoliEstado);
                    if (!dr.IsDBNull(iSoliEstado)) entity.SoliEstado = dr.GetString(iSoliEstado);

                    int iSoliFecSolicitud = dr.GetOrdinal(helper.SoliFecSolicitud);
                    if (!dr.IsDBNull(iSoliFecSolicitud)) entity.SoliFecSolicitud = dr.GetDateTime(iSoliFecSolicitud);

                    int iSoliFecEnviado = dr.GetOrdinal(helper.SoliFecEnviado);
                    if (!dr.IsDBNull(iSoliFecEnviado)) entity.SoliFecEnviado = dr.GetDateTime(iSoliFecEnviado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarporTipoNombreRepresentanteFiltroXls(string tipoemprcodi,
            string emprnomb,
            string rptetiprepresentantelegal,
            string rptetiprepresentantelegalcontacto,
            string estado,
            DateTime fecha)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = "";
            //No se esta usando Fecha Vigencia
            if (fecha == DateTime.MaxValue)
            {
                query = String.Format(helper.SqlListarporTipoNombreRepresentanteFiltroXls,
                tipoemprcodi,
                emprnomb,
                rptetiprepresentantelegal,
                rptetiprepresentantelegalcontacto,
                estado);
            }
            else
            {
                query = String.Format(helper.SqlListarporTipoNombreRepresentanteFiltroXlsconFechaVigencia,
                tipoemprcodi,
                emprnomb,
                rptetiprepresentantelegal,
                rptetiprepresentantelegalcontacto,
                estado,
                fecha.ToString(ConstantesBase.FormatoFecha));
            }


            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iRpteTelfMovil = dr.GetOrdinal(helper.RpteTelfMovil);
                    if (!dr.IsDBNull(iRpteTelfMovil)) entity.RpteTelfMovil = dr.GetString(iRpteTelfMovil);

                    int iRpteTipo = dr.GetOrdinal(helper.RpteTipo);
                    if (!dr.IsDBNull(iRpteTipo)) entity.RpteTipo = dr.GetString(iRpteTipo);

                    int iRpteTipRepresentanteLegal = dr.GetOrdinal(helper.RpteTipRepresentanteLegal);
                    if (!dr.IsDBNull(iRpteTipRepresentanteLegal)) entity.RpteTipRepresentanteLegal = dr.GetString(iRpteTipRepresentanteLegal);

                    int iRpteDocIdentidad = dr.GetOrdinal(helper.RpteDocIdentidad);
                    if (!dr.IsDBNull(iRpteDocIdentidad)) entity.RpteDocIdentidad = dr.GetString(iRpteDocIdentidad);

                    int iRpteCargoEmpresa = dr.GetOrdinal(helper.RpteCargoEmpresa);
                    if (!dr.IsDBNull(iRpteCargoEmpresa)) entity.RpteCargoEmpresa = dr.GetString(iRpteCargoEmpresa);

                    int iRpteBaja = dr.GetOrdinal(helper.RpteBaja);
                    if (!dr.IsDBNull(iRpteBaja)) entity.RpteBaja = dr.GetString(iRpteBaja);

                    int iRpteFechaVigenciaPoder = dr.GetOrdinal(helper.RpteFechaVigenciaPoder);
                    if (!dr.IsDBNull(iRpteFechaVigenciaPoder)) entity.RpteFechaVigenciaPoder = dr.GetDateTime(iRpteFechaVigenciaPoder);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoSolicitudes(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud,
            string empresa,
            int nroPage, int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = String.Format(helper.SqlListarHistoricoSolicitudes,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud,
                empresa,
                nroPage,
                pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);

                    int iTisoNombre = dr.GetOrdinal(helper.TisoNombre);
                    if (!dr.IsDBNull(iTisoNombre)) entity.TisoNombre = dr.GetString(iTisoNombre);

                    int iSoliEstado = dr.GetOrdinal(helper.SoliEstado);
                    if (!dr.IsDBNull(iSoliEstado)) entity.SoliEstado = dr.GetString(iSoliEstado);

                    int iSoliFecSolicitud = dr.GetOrdinal(helper.SoliFecSolicitud);
                    if (!dr.IsDBNull(iSoliFecSolicitud)) entity.SoliFecSolicitud = dr.GetDateTime(iSoliFecSolicitud);

                    int iSoliFecEnviado = dr.GetOrdinal(helper.SoliFecEnviado);
                    if (!dr.IsDBNull(iSoliFecEnviado)) entity.SoliFecEnviado = dr.GetDateTime(iSoliFecEnviado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarHistoricoSolicitudes(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud,
            string empresa)
        {
            string query = string.Format(helper.SqlNroRegListarHistoricoSolicitudes,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud,
                empresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoSolicitudesFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud,
            string empresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarHistoricoSolicitudesFiltroXls,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud,
                empresa);


            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);

                    int iTisoNombre = dr.GetOrdinal(helper.TisoNombre);
                    if (!dr.IsDBNull(iTisoNombre)) entity.TisoNombre = dr.GetString(iTisoNombre);

                    int iSoliEstado = dr.GetOrdinal(helper.SoliEstado);
                    if (!dr.IsDBNull(iSoliEstado)) entity.SoliEstado = dr.GetString(iSoliEstado);

                    int iSoliFecSolicitud = dr.GetOrdinal(helper.SoliFecSolicitud);
                    if (!dr.IsDBNull(iSoliFecSolicitud)) entity.SoliFecSolicitud = dr.GetDateTime(iSoliFecSolicitud);

                    int iSoliFecEnviado = dr.GetOrdinal(helper.SoliFecEnviado);
                    if (!dr.IsDBNull(iSoliFecEnviado)) entity.SoliFecEnviado = dr.GetDateTime(iSoliFecEnviado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        /// <summary>
        /// Devuelve un listado de revisiones segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <param name="empresa">Empresa</param>>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoRevisiones(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiporevision,
            string empresa,
            int nroPage,
            int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = String.Format(helper.SqlListarHistoricoRevisiones,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiporevision,
                empresa,
                nroPage,
                pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);

                    int ireviiteracion = dr.GetOrdinal(helper.reviiteracion);
                    if (!dr.IsDBNull(ireviiteracion)) entity.reviiteracion = Convert.ToInt32(dr.GetValue(ireviiteracion));

                    int itiporevision = dr.GetOrdinal(helper.tiporevision);
                    if (!dr.IsDBNull(itiporevision)) entity.tiporevision = dr.GetString(itiporevision);

                    int irevifeccreacion = dr.GetOrdinal(helper.revifeccreacion);
                    if (!dr.IsDBNull(irevifeccreacion)) entity.revifeccreacion = dr.GetDateTime(irevifeccreacion);

                    int irevifecrevision = dr.GetOrdinal(helper.revifecrevision);
                    if (!dr.IsDBNull(irevifecrevision)) entity.revifecrevision = dr.GetDateTime(irevifecrevision);

                    int ireviestado = dr.GetOrdinal(helper.reviestado);
                    if (!dr.IsDBNull(ireviestado)) entity.reviestado = dr.GetString(ireviestado);

                    int ihora = dr.GetOrdinal(helper.hora);
                    if (!dr.IsDBNull(ihora)) entity.hora = Convert.ToInt32(dr.GetValue(ihora));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        /// <summary>
        /// Devuelve total registros de historico de revisiones
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <param name="empresa">Empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarHistoricoRevisiones(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiporevision,
            string empresa)
        {
            string query = string.Format(helper.SqlNroRegListarHistoricoRevisiones,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiporevision,
                empresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        /// <summary>
        /// Devuelve un listado de revisiones segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <param name="empresa">Empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoRevisionesFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiporevision,
            string empresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarHistoricoRevisionesFiltroXls,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiporevision,
                empresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);

                    int ireviiteracion = dr.GetOrdinal(helper.reviiteracion);
                    if (!dr.IsDBNull(ireviiteracion)) entity.reviiteracion = Convert.ToInt32(dr.GetValue(ireviiteracion));

                    int itiporevision = dr.GetOrdinal(helper.tiporevision);
                    if (!dr.IsDBNull(itiporevision)) entity.tiporevision = dr.GetString(itiporevision);

                    int irevifeccreacion = dr.GetOrdinal(helper.revifeccreacion);
                    if (!dr.IsDBNull(irevifeccreacion)) entity.revifeccreacion = dr.GetDateTime(irevifeccreacion);

                    int irevifecrevision = dr.GetOrdinal(helper.revifecrevision);
                    if (!dr.IsDBNull(irevifecrevision)) entity.revifecrevision = dr.GetDateTime(irevifecrevision);

                    int ireviestado = dr.GetOrdinal(helper.reviestado);
                    if (!dr.IsDBNull(ireviestado)) entity.reviestado = dr.GetString(ireviestado);

                    int ihora = dr.GetOrdinal(helper.hora);
                    if (!dr.IsDBNull(ihora)) entity.hora = Convert.ToInt32(dr.GetValue(ihora));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>        
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="tiposolicitud">empresa</param>
        /// <param name="empresa">empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoModificaciones(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud,
            string empresa,
            int nroPage,
            int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = String.Format(helper.SqlListarHistoricoModificaciones,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud,
                empresa,
                nroPage,
                pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>        
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="tiposolicitud">tipo de solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarHistoricoModificaciones(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud,
            string empresa)
        {
            string query = string.Format(helper.SqlNroRegListarHistoricoModificaciones,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud,
                empresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>        
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="tiposolicitud">tipo solicitud</param>
        /// <param name="empresa">empresa</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarHistoricoModificacionesFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud,
            string empresa)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarHistoricoModificacionesFiltroXls,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud,
                empresa);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iRpteNombres = dr.GetOrdinal(helper.RpteNombres);
                    if (!dr.IsDBNull(iRpteNombres)) entity.RpteNombres = dr.GetString(iRpteNombres);

                    int iRpteCorreoElectronico = dr.GetOrdinal(helper.RpteCorreoElectronico);
                    if (!dr.IsDBNull(iRpteCorreoElectronico)) entity.RpteCorreoElectronico = dr.GetString(iRpteCorreoElectronico);

                    int iRpteTelefono = dr.GetOrdinal(helper.RpteTelefono);
                    if (!dr.IsDBNull(iRpteTelefono)) entity.RpteTelefono = dr.GetString(iRpteTelefono);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarTiempoProceso(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud,
            int nroPage,
            int pageSize)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = String.Format(helper.SqlListarTiempoProceso,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud,
                nroPage,
                pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);

                    int ireviiteracion = dr.GetOrdinal(helper.reviiteracion);
                    if (!dr.IsDBNull(ireviiteracion)) entity.reviiteracion = Convert.ToInt32(dr.GetValue(ireviiteracion));

                    int itiporevision = dr.GetOrdinal(helper.tiporevision);
                    if (!dr.IsDBNull(itiporevision)) entity.tiporevision = dr.GetString(itiporevision);

                    int irevifeccreacion = dr.GetOrdinal(helper.revifeccreacion);
                    if (!dr.IsDBNull(irevifeccreacion)) entity.revifeccreacion = dr.GetDateTime(irevifeccreacion);

                    int irevifecrevision = dr.GetOrdinal(helper.revifecrevision);
                    if (!dr.IsDBNull(irevifecrevision)) entity.revifecrevision = dr.GetDateTime(irevifecrevision);

                    int ireviestado = dr.GetOrdinal(helper.reviestado);
                    if (!dr.IsDBNull(ireviestado)) entity.reviestado = dr.GetString(ireviestado);

                    int ihora = dr.GetOrdinal(helper.hora);
                    if (!dr.IsDBNull(ihora)) entity.hora = Convert.ToInt32(dr.GetValue(ihora));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        /// <summary>
        /// Devuelve total registros de los tiempos del proceso
        /// </summary>
        /// <param name="fechaInicio">fecha inicio</param>
        /// <param name="fechaFin">fecha fin</param>
        /// <param name="tipoempresa">Tipo de empresa</param>
        /// <param name="tiporevision">Tipo revision</param>
        /// <returns>Numero de registros</returns>
        public int ObtenerTotalRegListarTiempoProceso(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiporevision)
        {
            string query = string.Format(helper.SqlNroRegListarTiempoProceso,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiporevision);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        /// <summary>
        /// Devuelve un listado de empresas segun el tipo
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="nroPage">nro de pagina</param>
        /// <param name="pageSize">tamaño de pagina</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarTiempoProcesoFiltroXls(DateTime fechaInicio,
            DateTime fechaFin,
            string tipoemprcodi,
            string tiposolicitud)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarTiempoProcesoFiltroXls,
                fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha),
                tipoemprcodi,
                tiposolicitud);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprnombrecomercial = dr.GetOrdinal(helperEmpresa.Emprnombrecomercial);
                    if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprsigla = dr.GetOrdinal(helperEmpresa.Emprsigla);
                    if (!dr.IsDBNull(iEmprsigla)) entity.Emprsigla = dr.GetString(iEmprsigla);

                    int iEmprnumpartidareg = dr.GetOrdinal(helperEmpresa.Emprnumpartidareg);
                    if (!dr.IsDBNull(iEmprnumpartidareg)) entity.Emprnumpartidareg = dr.GetString(iEmprnumpartidareg);

                    int iEmprtelefono = dr.GetOrdinal(helperEmpresa.Emprtelefono);
                    if (!dr.IsDBNull(iEmprtelefono)) entity.Emprtelefono = dr.GetString(iEmprtelefono);

                    int iEmprfax = dr.GetOrdinal(helperEmpresa.Emprfax);
                    if (!dr.IsDBNull(iEmprfax)) entity.Emprfax = dr.GetString(iEmprfax);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iModalidad = dr.GetOrdinal(helper.Modalidad);
                    if (!dr.IsDBNull(iModalidad)) entity.Modalidad = dr.GetString(iModalidad);

                    int iTipoAgente = dr.GetOrdinal(helper.TipoAgente);
                    if (!dr.IsDBNull(iTipoAgente)) entity.TipoAgente = dr.GetString(iTipoAgente);

                    int ireviiteracion = dr.GetOrdinal(helper.reviiteracion);
                    if (!dr.IsDBNull(ireviiteracion)) entity.reviiteracion = Convert.ToInt32(dr.GetValue(ireviiteracion));

                    int itiporevision = dr.GetOrdinal(helper.tiporevision);
                    if (!dr.IsDBNull(itiporevision)) entity.tiporevision = dr.GetString(itiporevision);

                    int irevifeccreacion = dr.GetOrdinal(helper.revifeccreacion);
                    if (!dr.IsDBNull(irevifeccreacion)) entity.revifeccreacion = dr.GetDateTime(irevifeccreacion);

                    int irevifecrevision = dr.GetOrdinal(helper.revifecrevision);
                    if (!dr.IsDBNull(irevifecrevision)) entity.revifecrevision = dr.GetDateTime(irevifecrevision);

                    int ireviestado = dr.GetOrdinal(helper.reviestado);
                    if (!dr.IsDBNull(ireviestado)) entity.reviestado = dr.GetString(ireviestado);

                    int ihora = dr.GetOrdinal(helper.hora);
                    if (!dr.IsDBNull(ihora)) entity.hora = Convert.ToInt32(dr.GetValue(ihora));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        /// <summary>
        /// Devuelve un listado de de flujo SGODC por empresa
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarFlujoEmpresa(int emprcodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = String.Format(helper.SqlListarFlujoEmpresa, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmflcodi = dr.GetOrdinal(helper.Emflcodi);
                    if (!dr.IsDBNull(iEmflcodi)) entity.Emflcodi = Convert.ToInt32(dr.GetValue(iEmflcodi));

                    int iFljcodi = dr.GetOrdinal(helper.Fljcodi);
                    if (!dr.IsDBNull(iFljcodi)) entity.Fljcodi = Convert.ToInt32(dr.GetValue(iFljcodi));

                    int iFLJFECHAORIG = dr.GetOrdinal(helper.FLJFECHAORIG);
                    if (!dr.IsDBNull(iFLJFECHAORIG)) entity.FLJFECHAORIG = dr.GetDateTime(iFLJFECHAORIG);

                    int iFLJFECHARECEP = dr.GetOrdinal(helper.FLJFECHARECEP);
                    if (!dr.IsDBNull(iFLJFECHARECEP)) entity.FLJFECHARECEP = dr.GetDateTime(iFLJFECHARECEP);

                    int iFLJFECHAPROCE = dr.GetOrdinal(helper.FLJFECHAPROCE);
                    if (!dr.IsDBNull(iFLJFECHAPROCE)) entity.FLJFECHAPROCE = dr.GetDateTime(iFLJFECHAPROCE);

                    int iFLJESTADO = dr.GetOrdinal(helper.FLJESTADO);
                    if (!dr.IsDBNull(iFLJESTADO)) entity.FLJESTADO = dr.GetString(iFLJESTADO);

                    int iobservacion = dr.GetOrdinal(helper.observacion);
                    if (!dr.IsDBNull(iobservacion)) entity.observacion = dr.GetString(iobservacion);

                    //int iDocumentoAdjuntoFileName = dr.GetOrdinal(helper.DocumentoAdjuntoFileName);
                    //if (!dr.IsDBNull(iDocumentoAdjuntoFileName)) entity.DocumentoAdjuntoFileName = dr.GetString(iDocumentoAdjuntoFileName);

                    //int iDocumentoAdjunto = dr.GetOrdinal(helper.DocumentoAdjunto);
                    //if (!dr.IsDBNull(iDocumentoAdjunto)) entity.DocumentoAdjunto = dr.GetString(iDocumentoAdjunto);

                    int icorrnumproc = dr.GetOrdinal(helper.corrnumproc);
                    if (!dr.IsDBNull(icorrnumproc)) entity.corrnumproc = Convert.ToDecimal(dr.GetValue(icorrnumproc));

                    int ifilecodi = dr.GetOrdinal(helper.filecodi);
                    if (!dr.IsDBNull(ifilecodi)) entity.filecodi = Convert.ToInt32(dr.GetValue(ifilecodi));

                    int iEmprfecinscripcion = dr.GetOrdinal(helperEmpresa.Emprfecinscripcion);
                    if (!dr.IsDBNull(iEmprfecinscripcion)) entity.Emprfechainscripcion = dr.GetDateTime(iEmprfecinscripcion);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprdomiciliolegal = dr.GetOrdinal(helperEmpresa.Emprdomiciliolegal);
                    if (!dr.IsDBNull(iEmprdomiciliolegal)) entity.Emprdomiciliolegal = dr.GetString(iEmprdomiciliolegal);

                    int iEmprcondicion = dr.GetOrdinal(helperEmpresa.Emprcondicion);
                    if (!dr.IsDBNull(iEmprcondicion)) entity.Emprcondicion = dr.GetString(iEmprcondicion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve un listado de de flujo SGODC por empresa
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <param name="solicodi">codigo de solicitud</param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarFlujoEmpresaSolicitud(int emprcodi, int solicodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = String.Format(helper.SqlListarFlujoSolicitud, emprcodi, solicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmflcodi = dr.GetOrdinal(helper.Emflcodi);
                    if (!dr.IsDBNull(iEmflcodi)) entity.Emflcodi = Convert.ToInt32(dr.GetValue(iEmflcodi));

                    int iFljcodi = dr.GetOrdinal(helper.Fljcodi);
                    if (!dr.IsDBNull(iFljcodi)) entity.Fljcodi = Convert.ToInt32(dr.GetValue(iFljcodi));

                    int iFLJFECHAORIG = dr.GetOrdinal(helper.FLJFECHAORIG);
                    if (!dr.IsDBNull(iFLJFECHAORIG)) entity.FLJFECHAORIG = dr.GetDateTime(iFLJFECHAORIG);

                    int iFLJFECHARECEP = dr.GetOrdinal(helper.FLJFECHARECEP);
                    if (!dr.IsDBNull(iFLJFECHARECEP)) entity.FLJFECHARECEP = dr.GetDateTime(iFLJFECHARECEP);

                    int iFLJFECHAPROCE = dr.GetOrdinal(helper.FLJFECHAPROCE);
                    if (!dr.IsDBNull(iFLJFECHAPROCE)) entity.FLJFECHAPROCE = dr.GetDateTime(iFLJFECHAPROCE);

                    int iFLJESTADO = dr.GetOrdinal(helper.FLJESTADO);
                    if (!dr.IsDBNull(iFLJESTADO)) entity.FLJESTADO = dr.GetString(iFLJESTADO);

                    int iobservacion = dr.GetOrdinal(helper.observacion);
                    if (!dr.IsDBNull(iobservacion)) entity.observacion = dr.GetString(iobservacion);

                    int icorrnumproc = dr.GetOrdinal(helper.corrnumproc);
                    if (!dr.IsDBNull(icorrnumproc)) entity.corrnumproc = Convert.ToDecimal(dr.GetValue(icorrnumproc));

                    int ifilecodi = dr.GetOrdinal(helper.filecodi);
                    if (!dr.IsDBNull(ifilecodi)) entity.filecodi = Convert.ToInt32(dr.GetValue(ifilecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public void ActualizarEstadoRegistro(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEstadoRegistro);

            dbProvider.AddInParameter(command, helperEmpresa.Emprestadoregistro, DbType.String, entity.Emprestadoregistro);
            dbProvider.AddInParameter(command, helperEmpresa.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helperEmpresa.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQueryAudit(command, entity.Lastuser);
        }

        public void ActualizarCondicion(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarCondicion);

            dbProvider.AddInParameter(command, helperEmpresa.Emprcondicion, DbType.String, entity.Emprcondicion);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQueryAudit(command, entity.Lastuser);
        }

        public void ActualizarFechaIngreso(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarFechaIngreso);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQueryAudit(command, entity.Lastuser);
        }

        public void ActualizarFechaBaja(SiEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarFechaIngreso);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQueryAudit(command, entity.Lastuser);
        }

        public List<SiEmpresaMMEDTO> ObtenerAgentesParticipantes(int tipo)
        {
            List<SiEmpresaMMEDTO> entitys = new List<SiEmpresaMMEDTO>();
            SiEmpresaHelper empresaHelper = new SiEmpresaHelper();
            string sql = "";

            if(tipo == 3)
            {
                sql = string.Format(helper.SqlObtenerAgentesParticipantes, tipo);
            }else sql = string.Format(helperEmpresa.SqlBuscarEmpresasMME, tipo);
            
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaMMEDTO entity = new SiEmpresaMMEDTO();

                    int iEmprmmecodi = dr.GetOrdinal(helperEmpresa.emprmmecodi);
                    if (!dr.IsDBNull(iEmprmmecodi)) entity.Emprmmecodi = dr.GetInt32(iEmprmmecodi);

                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helperEmpresa.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    int iEmprestado = dr.GetOrdinal(helperEmpresa.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iTipoemprdesc = dr.GetOrdinal(helperEmpresa.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.EmprTipoParticipante = dr.GetString(iTipoemprdesc);

                    int iEmprfecparticipacion = dr.GetOrdinal(helperEmpresa.emprfecparticipacion);
                    if (!dr.IsDBNull(iEmprfecparticipacion)) entity.Emprfecparticipacion = dr.GetDateTime(iEmprfecparticipacion);

                    int iEmprfecretiro = dr.GetOrdinal(helperEmpresa.emprfecretiro);
                    if (!dr.IsDBNull(iEmprfecretiro)) entity.Emprfecretiro = dr.GetDateTime(iEmprfecretiro);

                    int iEmprcomentario = dr.GetOrdinal(helperEmpresa.emprcomentario);
                    if (!dr.IsDBNull(iEmprcomentario)) entity.Emprcomentario = dr.GetString(iEmprcomentario);

                    int iEmprusucreacion = dr.GetOrdinal(helperEmpresa.Emprusucreacion);
                    if (!dr.IsDBNull(iEmprusucreacion)) entity.Emprusucreacion = dr.GetString(iEmprusucreacion);

                    int iEmprfeccreacion = dr.GetOrdinal(helperEmpresa.Emprfeccreacion);
                    if (!dr.IsDBNull(iEmprfeccreacion)) entity.Emprfeccreacion = dr.GetDateTime(iEmprfeccreacion);

                    int iEmprusumodificacion = dr.GetOrdinal(helperEmpresa.Emprusumodificacion);
                    if (!dr.IsDBNull(iEmprusumodificacion)) entity.Emprusumodificacion = dr.GetString(iEmprusumodificacion);

                    int iEmprfecmodificacion = dr.GetOrdinal(helperEmpresa.Emprfecmodificacion);
                    if (!dr.IsDBNull(iEmprfecmodificacion)) entity.Emprfecmodificacion = dr.GetDateTime(iEmprfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void ActualizarEmpresaNroRegistro(int emprcodi, int emprnroregistro)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEmpresaNroRegistro);

            dbProvider.AddInParameter(command, helperEmpresa.EmprNroRegistro, DbType.String, emprnroregistro);
            dbProvider.AddInParameter(command, helperEmpresa.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        
        /// Devuelve un listado de empresas publico
        /// </summary>
        /// <param name="tipoemprcodi">tipo de empresa</param>
        /// <param name="tiposolicitud">tipo de solicitud</param>        
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasPublico(string tipoemprcodi,
            string tiposolicitud)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            String query = String.Format(helper.SqlListarEmpresasPublico,
                tipoemprcodi,
                tiposolicitud);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();


                    int iEmprcodi = dr.GetOrdinal(helperEmpresa.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helperEmpresa.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprfecinscripcion = dr.GetOrdinal(helperEmpresa.Emprfecinscripcion);
                    if (!dr.IsDBNull(iEmprfecinscripcion)) entity.Emprfechainscripcion = dr.GetDateTime(iEmprfecinscripcion);

                    int iEmprfecregistro = dr.GetOrdinal(helperEmpresa.Emprfecingreso);
                    if (!dr.IsDBNull(iEmprfecregistro)) entity.Emprfecingreso = dr.GetDateTime(iEmprfecregistro);

                    int iEmprpagweb = dr.GetOrdinal(helperEmpresa.Emprpagweb);
                    if (!dr.IsDBNull(iEmprpagweb)) entity.Emprpagweb = dr.GetString(iEmprpagweb);

                    int iEmprrazsocial = dr.GetOrdinal(helperEmpresa.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprruc = dr.GetOrdinal(helperEmpresa.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region FIT - VALORIZACION DIARIA
        public List<SiEmpresaDTO> ObtenerAgentesParticipantesMME()
        {

            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            SiEmpresaHelper empresaHelper = new SiEmpresaHelper();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerAgentesParticipantesMME);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(empresaHelper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(empresaHelper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }
        #endregion
    }
}
