using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;


namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_EMPRESA
    /// </summary>
    public class SiEmpresaHelper : HelperBase
    {
        public SiEmpresaHelper()
            : base(Consultas.SiEmpresaSql)
        {
        }

        public void ObtenerMetaDatos(IDataReader dr, ref Dictionary<int, MetadataDTO> metadatos)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                metadatos.Add(i, new MetadataDTO
                {
                    FieldName = dr.GetName(i),
                    TipoDato = dr.GetFieldType(i)
                });
            }
        }

        public SiEmpresaDTO Create(IDataReader dr)
        {
            SiEmpresaDTO entity = new SiEmpresaDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

            int iEmprdire = dr.GetOrdinal(this.Emprdire);
            if (!dr.IsDBNull(iEmprdire)) entity.Emprdire = dr.GetString(iEmprdire);

            int iEmprtele = dr.GetOrdinal(this.Emprtele);
            if (!dr.IsDBNull(iEmprtele)) entity.Emprtele = dr.GetString(iEmprtele);

            int iEmprnumedocu = dr.GetOrdinal(this.Emprnumedocu);
            if (!dr.IsDBNull(iEmprnumedocu)) entity.Emprnumedocu = dr.GetString(iEmprnumedocu);

            int iTipodocucodi = dr.GetOrdinal(this.Tipodocucodi);
            if (!dr.IsDBNull(iTipodocucodi)) entity.Tipodocucodi = dr.GetString(iTipodocucodi);

            int iEmprruc = dr.GetOrdinal(this.Emprruc);
            if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

            int iEmprabrev = dr.GetOrdinal(this.Emprabrev);
            if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

            int iEmprorden = dr.GetOrdinal(this.Emprorden);
            if (!dr.IsDBNull(iEmprorden)) entity.Emprorden = Convert.ToInt32(dr.GetValue(iEmprorden));

            int iEmprdom = dr.GetOrdinal(this.Emprdom);
            if (!dr.IsDBNull(iEmprdom)) entity.Emprdom = dr.GetString(iEmprdom);

            int iEmprsein = dr.GetOrdinal(this.Emprsein);
            if (!dr.IsDBNull(iEmprsein)) entity.Emprsein = dr.GetString(iEmprsein);

            int iEmprrazsocial = dr.GetOrdinal(this.Emprrazsocial);
            if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

            int iEmprcoes = dr.GetOrdinal(this.Emprcoes);
            if (!dr.IsDBNull(iEmprcoes)) entity.Emprcoes = dr.GetString(iEmprcoes);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iCompcode = dr.GetOrdinal(this.Compcode);
            if (!dr.IsDBNull(iCompcode)) entity.Compcode = Convert.ToInt32(dr.GetValue(iCompcode));

            int iInddemanda = dr.GetOrdinal(this.Inddemanda);
            if (!dr.IsDBNull(iInddemanda)) entity.Inddemanda = dr.GetString(iInddemanda);

            int iEmprestado = dr.GetOrdinal(this.Emprestado);
            if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

            int IEmprcodosinergmin = dr.GetOrdinal(this.Emprcodosinergmin);
            if (!dr.IsDBNull(IEmprcodosinergmin)) entity.EmprCodOsinergmin = dr.GetString(IEmprcodosinergmin);

            int iEmprdomiciliada = dr.GetOrdinal(this.Emprdomiciliada);
            if (!dr.IsDBNull(iEmprdomiciliada)) entity.Emprdomiciliada = dr.GetString(iEmprdomiciliada);

            int iEmprambito = dr.GetOrdinal(this.Emprambito);
            if (!dr.IsDBNull(iEmprambito)) entity.Emprambito = dr.GetString(iEmprambito);

            int iEmprrubro = dr.GetOrdinal(this.Emprrubro);
            if (!dr.IsDBNull(iEmprrubro)) entity.Emprrubro = Convert.ToInt32(dr.GetValue(iEmprrubro));

            int iEmpragente = dr.GetOrdinal(this.Empragente);
            if (!dr.IsDBNull(iEmpragente)) entity.Empragente = dr.GetString(iEmpragente);
            
            int iScadacodi = dr.GetOrdinal(this.Scadacodi);
            if (!dr.IsDBNull(iScadacodi)) entity.Scadacodi = Convert.ToInt32(dr.GetValue(iScadacodi));          

            return entity;
        }


        #region Mapeo de Campos

        public static string TableName = "SI_EMPRESA";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Emprdire = "EMPRDIRE";
        public string Emprtele = "EMPRTELE";
        public string Emprnumedocu = "EMPRNUMEDOCU";
        public string Tipodocucodi = "TIPODOCUCODI";
        public string Emprruc = "EMPRRUC";
        public string Emprabrev = "EMPRABREV";
        public string Emprorden = "EMPRORDEN";
        public string Emprdom = "EMPRDOM";
        public string Emprsein = "EMPRSEIN";
        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Emprcoes = "EMPRCOES";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Compcode = "COMPCODE";
        public string Inddemanda = "INDDEMANDA";
        public string Emprestado = "EMPRESTADO";		
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Etiqueta = "ETIQUETA";
        public string UserEmail = "USEREMAIL";        
        public string Emprcodosinergmin = "EMPRCODOSINERGMIN";
        public string Emprdomiciliada = "EMPRDOMICILIADA";
        public string Emprambito = "EMPRAMBITO";
        public string Emprrubro = "EMPRRUBRO";
        public string Empragente = "EMPRAGENTE";
        public string Scadacodi = "SCADACODI";
        public string Emprindproveedor = "EMPRINDPROVEEDOR";
        public string Emprindusutramite = "EMPRINDUSUTRAMITE";
        public string Emprfecusutramite = "EMPRFECUSUTRAMITE";

        /// <summary>
        /// Registro de Integrantes
        /// </summary>
        public string Emprnombrecomercial = "EMPRNOMBRECOMERCIAL";
        public string Emprdomiciliolegal = "EMPRDOMICILIOLEGAL";
        public string Emprsigla = "EMPRSIGLA";
        public string Emprnumpartidareg = "EMPRNUMPARTIDAREG";
        public string Emprtelefono = "EMPRTELEFONO";
        public string Emprfax = "EMPRFAX";
        public string Emprpagweb = "EMPRPAGWEB";
        public string Emprnroconstancia = "EMPRNROCONSTANCIA";
        public string Emprcartaadjunto = "EMPRCARTAADJUNTO";
        public string Emprestadoregistro = "EMPRESTADOREGISTRO";
        public string Emprfecinscripcion = "EMPRFECINSCRIPCION";       
        public string Emprcartaadjuntofilename = "EMPRCARTAADJUNTOFILENAME";
        public string Emprcondicion = "EMPRCONDICION";
        public string Emprfecingreso = "EMPRFECINGRESO";
        public string Emprfecbaja = "EMPRFECBAJA";
        public string EmprnombAnidado = "EMPRNOMBANIDADO";
        public string EmprNroRegistro = "EMPRNROREGISTRO";
        public string SoliFecModificacion = "SOLIFECMODIFICACION";
        public string Fecregistro = "FECREGISTRO";
        public string EmprestadoFecha = "EMPRESTADOFECHA";
        public string Fechaaceptacion = "REGEMPFECACEP";
        public string Emprusucreacion = "EMPRUSUCREACION";
        public string Emprfeccreacion = "EMPRFECCREACION";
        public string Emprusumodificacion = "EMPRUSUMODIFICACION";
        public string Emprfecmodificacion = "EMPRFECMODIFICACION";

        public string EmprnombConcatenado = "EMPRNOMBCONCATENADO";

        #endregion

        #region MAPEO DE CAMPOS SI_EMPRESA_MME

        public string emprmmecodi = "EMPRMMECODI";
        public string emprfecparticipacion = "EMPRFECPARTICIPACION";
        public string emprfecretiro = "EMPRFECRETIRO";
        public string emprcomentario = "EMPRCOMENTARIO";
        public string emprmmeestado = "EMPRMMEESTADO";

        #endregion

        #region Campos Paginacion

        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";

        #endregion

        public string SqlGetEmpresasSein
        {
            get { return base.GetSqlXml("GetEmpresasSein"); }
        }

        public string SqlUpdateRI
        {
            get { return base.GetSqlXml("UpdateRI"); }
        }

        public string SqlGetEmpresaSistemaPorTipo
        {
            get { return base.GetSqlXml("GetEmpresaSistemaPorTipo"); }
        }

        public string SqlListGeneral
        {
            get { return base.GetSqlXml("ListGeneral"); }
        }

        public string SqlGetByUser
        {
            get { return base.GetSqlXml("GetByUser"); }
        }

        public string SqlGetEmpresasxCombustiblexUsuario
        {
            get { return base.GetSqlXml("GetEmpresasxCombustiblexUsuario"); }
        }

        public string sqlGetEmpresasxCombustible
        {
            get { return base.GetSqlXml("GetEmpresasxCombustible"); }
        }

        public string SqlListComboEmprGeneradora
        {
            get { return base.GetSqlXml("ListComboEmprGeneradora"); }
        }

        public string SqlListComboClientes
        {
            get { return base.GetSqlXml("ListComboClientes"); }
        }

        public string SqlListEmprResponsables
        {
            get { return base.GetSqlXml("ListEmprResponsables"); }
        }

        public string SqlListComboPorTipoEmpresa
        {
            get { return base.GetSqlXml("ListarPorTipoEmpresa"); }
        }

        public string SqlGetEmpresasHidro
        {
            get { return base.GetSqlXml("GetEmpresasHidro"); }
        }

        public string SqlObtenerEquiposCuencaHidro
        {
            get { return base.GetSqlXml("ObtenerEquiposCuencaHidro"); }
        }

        public string SqlGetEmpresasPtoMedicion
        {
            get { return base.GetSqlXml("GetEmpresasPtoMedicion"); }
        }

        public string SqlGetEmpresasFormato
        {
            get { return base.GetSqlXml("GetEmpresasFormato"); }
        }

        public string SqlGetEmpresasSEINFormato
        {
            get { return base.GetSqlXml("GetEmpresasSEINFormato"); }
        }

        public string SqlGetEmpresasFormatoPorEstado
        {
            get { return base.GetSqlXml("GetEmpresasFormatoPorEstado"); }
        }

        public string SqlGetEmpresasActivaFormato
        {
            get { return base.GetSqlXml("GetEmpresasActivaFormato"); }
        }

        public string SqlGetEmpresasFormatoxUsuario
        {
            get { return base.GetSqlXml("GetEmpresasFormatoxUsuario"); }
        }

        public string SqlListInfoCliente
        {
            get { return base.GetSqlXml("ListInfoCliente"); }
        }

        public string SqlListResponsable
        {
            get { return base.GetSqlXml("ListResponsable"); }
        }

        public string SqlObtenerNroRegistroBusqueda
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusqueda"); }
        }

        public string SqlObtenerNroRegistroBusquedaMME
        {
            get { return base.GetSqlXml("ObtenerNroRegistroBusquedaMME"); }
        }

        public string SqlBuscarEmpresas
        {
            get { return base.GetSqlXml("BuscarEmpresas"); }
        }

        public string SqlBuscarEmpresasMME
        {
            get { return base.GetSqlXml("BuscarEmpresasMME"); }
        }

        public string SqlBuscarEmpresasporParticipante
        {
            get { return base.GetSqlXml("BuscarEmpresasporParticipante"); }
        }

        public string SqlBuscarHistorialEmpresasMME
        {
            get { return base.GetSqlXml("BuscarHistorialEmpresasMME"); }
        }

        public string SqlDarBajaEmpresa
        {
            get { return base.GetSqlXml("DarBajaEmpresa"); }
        }

        public string SqlVerificarExistenciaNombre
        {
            get { return base.GetSqlXml("VerificarExistenciaNombre"); }
        }

        public string SqlVerificarExistenciaRuc
        {
            get { return base.GetSqlXml("VerificarExistenciaRuc"); }
        }

        public string SqlObtenerEmpresaPorRuc
        {
            get { return base.GetSqlXml("ObtenerEmpresaPorRuc"); }
        }

        public string SqlActualizarEstado
        {
            get { return base.GetSqlXml("ActualizarEstado"); }
        }

        public string SqlExportarEmpresas
        {
            get { return base.GetSqlXml("ExportarEmpresas"); }
        }
                
        public string SqlListaEmpresasPorTipo
        {
            get { return base.GetSqlXml("ListaEmpresasPorTipoCumplimiento"); }
        }

        public string SqlListaEmailUsuariosEmpresas
        {
            get { return base.GetSqlXml("ListaEmailUsuariosEmpresas"); }
        }

        public string SqlObtenerEmpresasPuntosMedicion
        {
            get { return base.GetSqlXml("ObtenerEmpresasPuntosMedicion"); }
        }
        
        public string SqlListaEmpresasSuministrador
        {
            get { return base.GetSqlXml("ListaEmpresasSuministrador"); }
        }

        public string SqlActualizarRucFicticio
        {
            get { return base.GetSqlXml("ActualizarRucFicticio"); }
        }

        public string SqlListarEmpresasxTipoEquipos
        {
            get { return base.GetSqlXml("ListarEmpresasxTipoEquipos"); }
        }

        //- alpha.JDEL - Inicio 19/10/2016: Cambio para atender el requerimiento.
        public string SqlObtenerEmpresaOsinergmin
        {
            get { return base.GetSqlXml("ObtenerEmpresaOsinergmin"); }
        }
        //- JDEL Fin

        public string SqlUpdateOsinergmin
        {
            get { return base.GetSqlXml("UpdateOsinergmin"); }
        }

        //- alpha.JDEL - Inicio 03/11/2016: Cambio para atender el requerimiento.
        public string SqlGetByCodOsinergmin
        {
            get { return base.GetSqlXml("GetByCodOsinergmin"); }
        }
        //- JDEL Fin
        //- alpha.HDT - 31/10/2016: Cambio para atender el requerimiento. 
        public string SqlGetByOsinergmin
        {
            get { return GetSqlXml("GetByIdOsinergmin"); }
        }       

        // Inicio de Agregado - Sistema de Compensaciones
        public string SqlListaEmpresasCompensacion
        {
            get { return base.GetSqlXml("ListaEmpresasCompensacion"); }
        }
        //Agregado por STS
        //SqlListaEmpresasSeleccionadas
        public string SqlListEmprDocSelected
        {
            get { return base.GetSqlXml("ListaEmpresasSeleccionadas"); }
        }
        // Fin de Agregado - Sistema de Compensaciones
        public string SqlObtenerEmpresasGeneracion
        {
            get { return base.GetSqlXml("ObtenerEmpresasGeneracion"); }
        }
                //- alpha.HDT - 14/07/2017: Cambio para atender el requerimiento. 
        public string SqlGetByRuc
        {
            get { return base.GetSqlXml("GetByRuc"); }
        }

        public string SqlObtenerEmpresasReportaGeneracionSEIN
        {
            get { return base.GetSqlXml("ObtenerEmpresaReportaGeneracionSEIN"); }
        }

        public string SqlObtenerEmpresasPorTipo
        {
            get { return base.GetSqlXml("ObtenerEmpresaPorTipo"); }
        }

        public string SqlObtenerEmpresasPorTipoSEIN
        {
            get { return base.GetSqlXml("ObtenerEmpresaPorTipoSEIN"); }
        }

        public string SqlListarEmpresasCentralesActivas
        {
            get { return base.GetSqlXml("ListarEmpresasCentralesActivas"); }
        }

        public string SqlEmpresaMigracion
        {
            get { return GetSqlXml("EmpresaMigracion"); }
        }

        #region PR5
        public string SqlListarEmpresasXID
        {
            get { return GetSqlXml("ListarEmpresasXID"); }
        }
        #endregion

        #region Indisponibilidad
        public string SqlGetEmpresasConCentralTermoxUsuario
        {
            get { return base.GetSqlXml("GetEmpresasConCentralTermoxUsuario"); }
        }
        public string SqlGetEmpresasConCentralTermo
        {
            get { return base.GetSqlXml("GetEmpresasConCentralTermo"); }
        }
        #endregion

        #region Sistema Rechazo Carga
        public string ListaEmpresasRechazoCarga
        {
            get { return base.GetSqlXml("ListaEmpresasRechazoCarga"); }
        }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListarEmpresasxCategoria
        {
            get { return base.GetSqlXml("ListarEmpresasxCategoria"); }
        }
        #endregion

        #region transferencia Equipos 
        public string SqlGetEmpresasActivaBajas
        {
            get { return base.GetSqlXml("GetEmpresasActivaBajas"); }
        }

        public string SqlGetIdNameEmpresasActivaBajas
        {
            get { return base.GetSqlXml("GetIdNameEmpresasActivaBajas"); }
        }

        public string SqlListarEmpresaEstadoActual
        {
            get { return base.GetSqlXml("ListarEmpresaEstadoActual"); }
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        
        #endregion

        #region Intervenciones

        public string SqlListarComboEmpresasMantenimiento
        {
            get { return base.GetSqlXml("ListarComboEmpresasMantenimiento"); }
        }

        public string SqlListarComboEmpresasConsulta
        {
            get { return base.GetSqlXml("ListarComboEmpresasConsulta"); }
        }

        public string SqlListarComboEmpresasRegistroMantenimiento
        {
            get { return base.GetSqlXml("ListarComboEmpresasRegistroMantenimiento"); }
        }

        public string SqlListarComboEmpresasRegistroConsulta
        {
            get { return base.GetSqlXml("ListarComboEmpresasRegistroConsulta"); }
        }

        public string SqlActualizarAbreviatura
        {
            get { return base.GetSqlXml("ActualizarAbreviatura"); }
        }

        #endregion

        #region MonitoreoMME
        public string SqlListarEmpresaIntegranteMonitoreoMME
        {
            get { return base.GetSqlXml("ListarEmpresaIntegranteMonitoreoMME"); }
        }
        #endregion

        #region FIT - Se�ales no disponibles - Asociacion empresas
        public string SqlUpdateAsociacionEmpresa
        {
            get { return base.GetSqlXml("UpdateAsociacionEmpresa"); }
        }

        public string SqlEliminarAsociocionesEmpresa
        {
            get { return base.GetSqlXml("EliminarAsociocionesEmpresa"); }
        }
        #endregion

        #region Titularidad-Instalaciones-Empresas
        public string SqlListarEmpresaVigenteByRango
        {
            get { return base.GetSqlXml("ListarEmpresaVigenteByRango"); }
        }
        #endregion

        #region Mejoras IEOD

        public string SqlListarEmpresaPorOrigenPtoMedicion
        {
            get { return base.GetSqlXml("ListarEmpresaPorOrigenPtoMedicion"); }
        }

        public string SqlListarEmpresaScada
        {
            get { return base.GetSqlXml("ListarEmpresaScada"); }
        }

        #endregion

        #region FIT - VALORIZACION DIARIA

        public string SqlListarEmpresasPorIDs
        {
            get { return GetSqlXml("ListarEmpresasPorIDs"); }
        }

        #endregion

        #region DevolucionAportes
        public string SqlListarEmpresaDevolucion
        {
            get { return base.GetSqlXml("ListarEmpresaDevolucion"); }
        }

        #endregion

        #region Proveedores

        public string SqlObtenerProveedores
        {
            get { return base.GetSqlXml("ObtenerProveedores"); }
        }

        public string SqlObtenerEmpresasPortalTramite
        {
            get { return base.GetSqlXml("ObtenerEmpresasPortalTramite"); } 
        }

        public string SqlObtenerNroRegistrosBusquedaTramite
        {
            get { return base.GetSqlXml("ObtenerNroRegistrosBusquedaTramite"); }
        }

        public string SqlActualizarDatosUsuarioTramite
        {
            get { return base.GetSqlXml("ActualizarDatosUsuarioTramite"); }
        }
        #endregion

        #region Aplicativo Extranet CTAF
        public string SqlListarEmpresasEventoCTAF
        {
            get { return base.GetSqlXml("ListarEmpresasEventoCTAF"); }
        }
        #endregion

        #region FICHA T�CNICA

        public string SqlListarEmpresasFT
        {
            get { return base.GetSqlXml("ListarEmpresasFT"); }
        }

        #endregion

        #region EMPRESAS MERCADO

        public string SqlGetByIdMME
        {
            get { return base.GetSqlXml("GetByIdMME"); }
        }

        public string SqlGetMaxIdMME
        {
            get { return base.GetSqlXml("GetMaxIdMME"); }
        }

        public string SqlSaveMME
        {
            get { return base.GetSqlXml("SaveMME"); }
        }

        public string SqlUpdateMME
        {
            get { return base.GetSqlXml("UpdateMME"); }
        }

        #endregion

        #region Ficha tecnica 2023
        public string SqlListarPorTipo
        {
            get { return base.GetSqlXml("ListarPorTipo"); }
        }

        public string SqlListarEmpresaExtranetFT
        {
            get { return base.GetSqlXml("ListarEmpresaExtranetFT"); }
        }
        #endregion

        #region Demanda DPO - Iteracion 2
        public string SqlListaEmpresasByTipo
        {
            get { return base.GetSqlXml("ListaEmpresasByTipo"); }
        }
        #endregion

        #region CPPA.ASSETEC.2024 - Iteracion 1
        public string SqlListaEmpresasCPPA
        {
            get { return base.GetSqlXml("ListaEmpresasCPPA"); }
        }

        public string SqlListaEmpresasDescargaIntegrantes
        {
            get { return base.GetSqlXml("ListaEmpresasDescargaIntegrantes"); }
        }
        #endregion
    
        #region GestProtect
        public string SqlListarMaestroEmpresasProteccion
        {
            get { return base.GetSqlXml("ListMaestroEmpresaProteccion"); }
        }
        #endregion

        #region GESPROTEC-20241031
        public string SqlObtenerEmpresaSEINProtecciones
        {
            get { return base.GetSqlXml("ObtenerEmpresaSEINProtecciones"); }
        }
        #endregion
    }

}

