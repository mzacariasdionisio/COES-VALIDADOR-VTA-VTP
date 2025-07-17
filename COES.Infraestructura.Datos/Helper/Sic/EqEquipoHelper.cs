using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_EQUIPO
    /// </summary>
    public class EqEquipoHelper : HelperBase
    {
        public EqEquipoHelper(): base(Consultas.EqEquipoSql)
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
        public EqEquipoDTO Create(IDataReader dr)
        {
            EqEquipoDTO entity = new EqEquipoDTO();

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iElecodi = dr.GetOrdinal(this.Elecodi);
            if (!dr.IsDBNull(iElecodi)) entity.Elecodi = Convert.ToInt32(dr.GetValue(iElecodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iEquiabrev2 = dr.GetOrdinal(this.Equiabrev2);
            if (!dr.IsDBNull(iEquiabrev2)) entity.Equiabrev2 = dr.GetString(iEquiabrev2);

            int iEquitension = dr.GetOrdinal(this.Equitension);
            if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iEquipot = dr.GetOrdinal(this.Equipot);
            if (!dr.IsDBNull(iEquipot)) entity.Equipot = dr.GetDecimal(iEquipot);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEcodigo = dr.GetOrdinal(this.Ecodigo);
            if (!dr.IsDBNull(iEcodigo)) entity.Ecodigo = dr.GetString(iEcodigo);

            int iEquiestado = dr.GetOrdinal(this.Equiestado);
            if (!dr.IsDBNull(iEquiestado)) entity.Equiestado = dr.GetString(iEquiestado);

            //int iOsigrupocodi = dr.GetOrdinal(this.Osigrupocodi);
            //if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

            int iLastcodi = dr.GetOrdinal(this.Lastcodi);
            if (!dr.IsDBNull(iLastcodi)) entity.Lastcodi = Convert.ToInt32(dr.GetValue(iLastcodi));

            int iEquifechiniopcom = dr.GetOrdinal(this.Equifechiniopcom);
            if (!dr.IsDBNull(iEquifechiniopcom)) entity.Equifechiniopcom = dr.GetDateTime(iEquifechiniopcom);

            int iEquifechfinopcom = dr.GetOrdinal(this.Equifechfinopcom);
            if (!dr.IsDBNull(iEquifechfinopcom)) entity.Equifechfinopcom = dr.GetDateTime(iEquifechfinopcom);

            int iUsuarioUpdate = dr.GetOrdinal(this.UsuarioUpdate);
            if (!dr.IsDBNull(iUsuarioUpdate)) entity.UsuarioUpdate = dr.GetString(iUsuarioUpdate);

            int iFechaUpdate = dr.GetOrdinal(this.FechaUpdate);
            if (!dr.IsDBNull(iFechaUpdate)) entity.FechaUpdate = Convert.ToDateTime(dr.GetValue(iFechaUpdate));

            int iequimaniobra = dr.GetOrdinal(this.Equimaniobra);
            if (!dr.IsDBNull(iequimaniobra)) entity.EquiManiobra = Convert.ToString(dr.GetValue(iequimaniobra));

            try
            {
                int iOsinergcodi = dr.GetOrdinal(this.Osinergcodi);
                if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);
            }
            catch
            {
                entity.Osinergcodi = "";
            }
            try
            {
                int iOsinergcodiGen = dr.GetOrdinal(this.OsinergcodiGen);                                           // ticket-6068
                if (!dr.IsDBNull(iOsinergcodiGen)) entity.OsinergcodiGen = dr.GetString(iOsinergcodiGen);           // ticket-6068
            }
            catch
            {
                entity.OsinergcodiGen = "";
            }

            #region GESPROTECT
            if (validaColumna(dr, this.Flaggenprotec))
            {
                int iFlaggenprotec = dr.GetOrdinal(this.Flaggenprotec);
                if (!dr.IsDBNull(iFlaggenprotec)) entity.Flaggenprotec = Convert.ToString(dr.GetValue(iFlaggenprotec));
            }

            if (validaColumna(dr, this.Epequinombenprotec))
            {
                int iAreanombenprotec = dr.GetOrdinal(this.Epequinombenprotec);
                if (!dr.IsDBNull(iAreanombenprotec)) entity.Epequinombenprotec = Convert.ToString(dr.GetValue(iAreanombenprotec));
            }

            #endregion
            //int iOperadoremprcodi = dr.GetOrdinal(this.Operadoremprcodi);
            //if (!dr.IsDBNull(iOperadoremprcodi)) entity.Operadoremprcodi = Convert.ToInt32(dr.GetValue(iOperadoremprcodi));

            return entity;
        }

        //GESPROTEC - 20241029
        #region GESPROTECT
        bool validaColumna(IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Mapeo de Campos
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Elecodi = "ELECODI";
        public string Areacodi = "AREACODI";
        public string Famcodi = "FAMCODI";
        public string Equiabrev = "EQUIABREV";
        public string Equinomb = "EQUINOMB";
        public string Equiabrev2 = "EQUIABREV2";
        public string Equitension = "EQUITENSION";
        public string Equipadre = "EQUIPADRE";
        public string Equipot = "EQUIPOT";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Ecodigo = "ECODIGO";
        public string Equiestado = "EQUIESTADO";
        public string Osigrupocodi = "OSIGRUPOCODI";
        public string Lastcodi = "LASTCODI";
        public string Equifechiniopcom = "EQUIFECHINIOPCOM";
        public string Equifechfinopcom = "EQUIFECHFINOPCOM";
        public string UsuarioUpdate = "USUARIOUPDATE";
        public string FechaUpdate = "FECHAUPDATE";
        public string EmpresaOrigen = "EMPRESAORIGEN";
        public string Equimaniobra = "EQUIMANIOBRA";
        public string Propcodi = "PROPCODI";
        public string EMPRNOMB = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string AREANOMB = "AREANOMB";
        public string Famnomb = "FAMNOMB";
        public string TAREAABREV = "TAREAABREV";
        public string FAMABREV = "FAMABREV";
        public string DESCENTRAL = "DESCENTRAL";
        public string Urlmaniobra = "URLMANIOBRA";
        public string TableName = "EQ_EQUIPO";        
        public string Osinergcodi = "OSINERGCODI";
        public string OsinergcodiGen = "OSINERGCODIGEN";                // ticket-6068
        public string OsinergcodiDespacho = "OSINERGCODIDESPACHO";
        public string B2 = "B2";
        public string B3 = "B3";
        public string Equidesc = "EQUIDESC";
        public string Operadoremprcodi = "OPERADOREMPRCODI";
        public string Operadornomb = "OPERADORNOMB";

        public string Equirelfecmodificacion = "EQUIRELFECMODIFICACION";
        public string Equirelusumodificacion = "EQUIRELUSUMODIFICACION";

        public string Equicodiactual = "EQUICODIACTUAL";
        public string Heqdatfecha = "HEQDATFECHA";
        public string Heqdatestado = "HEQDATESTADO";
        public string Gruponomb = "Gruponomb";
        public string Grupoabrev = "Grupoabrev";
        public string Areadesc = "AREADESC";

        public string Grupotipocogen = "GRUPOTIPOCOGEN";

        #endregion

        #region CPPA.ASSETEC.2024
        public string EquinombConcatenado = "EQUINOMBCONCATENADO";
        #endregion

        #region Campos Paginacion

        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";

        #endregion

        #region SIOSEIN
        public string Tgenercodi = "TGENERCODI";
        public string Mes = "MES";
        public string Promedio = "PROMEDIO";
        public string CodCentral = "CODCENTRAL";
        public string Tgenernomb = "TGENERNOMB";
        public string Fenergcodi = "FENERGCODI";
        public string Fenergnomb = "FENERGNOMB";
        public string Central = "CENTRAL";
        public string Ctgdetnomb = "CTGDETNOMB";
        public string Gruporeservafria = "GRUPORESERVAFRIA";
        public string Grupoemergencia = "GRUPOEMERGENCIA";
        public string Gruponodoenergetico = "GRUPONODOENERGETICO";
        public string Tipogenerrer = "TIPOGENERRER";
        public string Fecha = "FECHA";
        public string Condicion = "CONDICION";
        public string Codigotipoempresa = "CODIGOTIPOEMPRESA";
        public string Codigogrupo = "CODIGOGRUPO";
        public string Correlativo = "CORRELATIVO";
        public string Capacidadanterior = "CAPACIDADANTERIOR";
        public string Capacidadnueva = "CAPACIDADNUEVA";
        public string Observaciones = "OBSERVACIONES";
        #endregion

        #region Ficha tecnica 2023
        public string Idelemento = "IDELEMENTO";
        public string Idempresaelemento = "IDEMPRESAELEMENTO";
        public string Nombempresaelemento = "NOMBEMPRESAELEMENTO";
        public string Idempresacopelemento = "IDEMPRESACOPELEMENTO";
        public string Nombempresacopelemento = "NOMBEMPRESACOPELEMENTO";
        public string Nombreelemento = "NOMBREELEMENTO";
        public string Tipoelemento = "TIPOELEMENTO";
        public string Areaelemento = "AREAELEMENTO";
        public string Estadoelemento = "ESTADOELEMENTO";
        #endregion

        //GESPROTEC - 20241029
        #region GESPROTECT
        public string Flaggenprotec = "flagenprotec";
        public string Epequinombenprotec = "epequinombenprotec";
        public string Epequicodi = "epequicodi";
        public string EquiestadoDesc = "EQUIESTADODESC";
        public string Codigo = "Codigo";
        public string TipoUso = "tipo_uso";
        #endregion

        public string SqlGetBasicList
        {
            get { return base.GetSqlXml("GetBasicList"); }
        }
        public string SqlListadoCentralesOsinergmin
        {
            get { return base.GetSqlXml("ListadoCentralesOsinergmin"); }
        }

        public string SQLListarEquiposxFiltro
        {
            get { return base.GetSqlXml("SQLListarEquiposxFiltro"); }
        }

        public string SqlBuscarEquipoPorNombrePaginado
        {
            get { return base.GetSqlXml("SqlBuscarEquipoPorNombrePaginado"); }
        }

        public string SqlEquiposxFamilias
        {
            get { return base.GetSqlXml("EquiposxFamilias"); }
        }
        
        public string SqlListarEquipoxFamiliasActivosyProyectos
        {
            get { return base.GetSqlXml("EquiposxFamiliasActivosyProyectos"); }
        }

        public string SqlListarUnidadesxEnsayo
        {
            get { return base.GetSqlXml("UnidadesFitradasXEnsayo"); }
        } 

        public string SQLListarEquiposxFiltroPaginado
        {
            get { return base.GetSqlXml("SQLListarEquiposxFiltroPaginado"); }
        }

        public string SqlObtenerDetalleEquipo
        {
            get { return base.GetSqlXml("ObtenerDetalleEquipo"); }
        }

        public string SqlObtenerEquipoPorAreaEmpresa
        {
            get { return base.GetSqlXml("ObtenerEquipoPorAreaEmpresa"); }
        }

        public string SqlCentralesXCombustible
        {
            get { return base.GetSqlXml("CentralesXCombustible"); }
        }

        public string SqlCentralesXEmpresaGEN
        {
            get { return base.GetSqlXml("CentralesXEmpresaGEN"); }
        }

        public string SqlCentralesXEmpresaGEN2
        {
            get { return base.GetSqlXml("CentralesXEmpresaGEN2"); }
        }

        public string SqlListaEquiposEnsayo
        {
            get { return base.GetSqlXml("ListaEquiposEnsayo"); }
        }

        public string SqlListadoEquipamientoPaginado
        {
            get { return base.GetSqlXml("SqlListadoEquipamientoPaginado"); }
        }

        public string SqlTotalListadoEquipamiento
        {
            get { return base.GetSqlXml("SQLTotalListadoEquipamiento"); }
        }

        public string SqlObtenerEquiposProcManiobras
        {
            get { return base.GetSqlXml("ObtenerEquiposProcManiobras"); }
        }

        public string SqlListaRecursosxCuenca
        {
            get { return base.GetSqlXml("ListaRecursosxCuenca"); }
        }
        
        public string SqlGeneradoresPorModoOperacionSimple
        {
            get { return base.GetSqlXml("GeneradoresPorModoOperacionSimple"); }
        }

        public string SqlGeneradoresPorModoOperacionCombinado
        {
            get { return base.GetSqlXml("GeneradoresPorModoOperacionCombinado"); }
        }

        public string SqlObtenerEquiposPorFamilia
        {
            get { return base.GetSqlXml("ObtenerEquiposPorFamilia"); }
        }
        
        public string SqlReportePotenciaEfectivaHidraulicas
        {
            get { return base.GetSqlXml("ReportePotenciaEfectivaHidraulicas"); }
        }
        
        public string SqlReportePotenciaEfectivaSolares
        {
            get { return base.GetSqlXml("ReportePotenciaEfectivaSolares"); }
        }
        
        public string SqlReportePotenciaEfectivaEolicas
        {
            get { return base.GetSqlXml("ReportePotenciaEfectivaEolicas"); }
        }

        public string SqlObtenerPorPadre
        {
            get { return base.GetSqlXml("ObtenerPorPadre"); }
        }

        public string SqlEquiposxFamiliasEmpresas
        {
            get { return base.GetSqlXml("EquiposxFamiliasxEmpresas"); }
        }
                       
        public string SqlEquiposXFamilia
        {
            get { return base.GetSqlXml("ListaEquiposXFamilia"); }
        }

        public string SqlGetByIdEquipo
        {
            get { return base.GetSqlXml("GetByIdEquipo"); }
        }

        public string SqlListByIdEquipo
        {
            get { return base.GetSqlXml("ListByIdEquipo"); }
        }

        public string UpdateOsinergmin
        {
            get { return base.GetSqlXml("UpdateOsinergmin"); }
        }
        public string SqlUpdateOsinergminCodigo
        {
            get { return base.GetSqlXml("UpdateOsinergminCodigo"); }
        }

        public string SqlGetByCodOsinergmin
        {
            get { return base.GetSqlXml("GetByCodOsinergmin"); }
        }

        public string SqlGetEquipoParaCreacion
        {
            get { return GetSqlXml("GetEquipoParaCreacion"); }
        }
        
        public string SqlGetByOsinergmin
        {
            get { return GetSqlXml("GetByIdOsinergmin"); }
        }

        public string SqlObtenerEquipoPropiedadAGC
        {
            get { return base.GetSqlXml("EquipoPropiedadAGC"); }
        }

        public string SqlListaEquipoAGC
        {
            get { return base.GetSqlXml("EquipoAGC"); }
        }

        public string SqlListaEquipoNombre
        {
            get { return base.GetSqlXml("ListaEquipoNombre"); }
        }


        public string SqlObtenerEquipoPropiedad
        {
            get { return base.GetSqlXml("EquipoPropiedad"); }
        }

        public string SqlTotalEquipoPropiedad
        {
            get { return base.GetSqlXml("TotalEquipoPropiedad"); }
        }

        public string SqlListaEquipoPrGrupo
        {
            get { return base.GetSqlXml("ListaEquipoPrGrupo"); }
        }

        public string SqlCentralesXEmpresaHorasOperacion
        {
            get { return base.GetSqlXml("CentralesXEmpresaHorasOperacion"); }
        }

        public string SqlCentralesXEmpresaXFamiliaGEN
        {
            get { return base.GetSqlXml("CentralesXEmpresaXFamiliaGEN"); }
        }

        public string SqlListadoXEmpresa
        {
            get { return base.GetSqlXml("ListadoXEmpresa"); }
        }

        public string SqlGetByEmprFam2
        {
            get { return base.GetSqlXml("GetByEmprFam2"); }
        }

        public string SqlCentralesXEmpresaXFamiliaGEN2
        {
            get { return base.GetSqlXml("CentralesXEmpresaXFamiliaGEN2"); }
        }

        public string SqlListarTopologiaEquipoPadres
        {
            get { return base.GetSqlXml("ListarTopologiaEquipoPadres"); }
        }

        public string SqlObtenerPadresHidrologia
        {
            get { return base.GetSqlXml("ObtenerPadresHidrologia"); }
        }
        
        public string SqlObtenerPadresHidrologiaTodos
        {
            get { return base.GetSqlXml("ObtenerPadresHidrologiaTodos"); }
        }

        public string SqlObtenerEquipoPorAreaEmpresaTodos
        {
            get { return base.GetSqlXml("ObtenerEquipoPorAreaEmpresaTodos"); }
        }

        #region PR5

        public string SqlListarEquiposXTipoEquipos
        {
            get { return base.GetSqlXml("ListarEquiposXTipoEquipos"); }
        }

        public string SqlListarEquiposXEmpresasXArea
        {
            get { return base.GetSqlXml("ListarEquiposXEmpresasXArea"); }
        }

        public string SqlListarIngresoSalidaOperacionComercialSEIN
        {
            get { return base.GetSqlXml("ListarIngresoSalidaOperacionComercialSEIN"); }
        }

        public string SqlListarIngresoOperacionComercialSEIN
        {
            get { return base.GetSqlXml("ListarIngresoOperacionComercialSEIN"); }
        }

        public string SqlObtenerEquiposPorFamiliaOriglectcodi
        {
            get { return base.GetSqlXml("ObtenerEquiposPorFamiliaOriglectcodi"); }
        }

        #endregion

        #region Transferencia de Equipos
        public string SqlListarEquiposXEmpresas
        {
            get { return base.GetSqlXml("ListarEquiposXEmpresas"); }
        }
        public string SqlListarEquiposXEmpresaOrigenMigracion {            
            get{return base.GetSqlXml("ListadoEquiposXEmpresaOrigen");}
        }
        public string SqlGetListaEquiposSiEquipoMigrarByMigracodi
        {
            get { return base.GetSqlXml("GetListaEquiposSiEquipoMigrarByMigracodi");}
        }
        #endregion

        #region Indisponibilidad

        public string SqlListarEquiposTTIE
        {
            get { return base.GetSqlXml("ListarEquiposTTIE"); }
        }

        #endregion

        #region Rechazo Carga
        public string SqlObtenerEquiposPorFamiliaRechazoCarga
        {
            get { return base.GetSqlXml("ObtenerEquiposPorFamiliaRechazoCarga"); }
        }
        #endregion

        #region EMS
        public string SqlGetByEmprFamCentral
        {
            get { return base.GetSqlXml("GetByEmprFamCentral"); }
        }
        #endregion

        #region BarrasModeladas

        public string SqlListEquiposGrupocodiFamilia
        {
            get { return base.GetSqlXml("ListEquiposGrupocodiFamilia"); }
        }
        public string SqlUpdateGrupoCodiPorCodigoFamilia
        {
            get { return base.GetSqlXml("UpdateGrupoCodiPorCodigoFamilia"); }
        }       
        #endregion

        #region NotificacionesCambiosEquipamiento
        public string SqlEquiposModificados
        {
            get { return base.GetSqlXml("EquiposModificados"); }
        }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaLineasDigsilent
        {
            get { return base.GetSqlXml("ListaLineasDigsilent"); }
        }

        public string SqlListaPruebasAleatorias
        {
            get { return base.GetSqlXml("ListaPruebasAleatorias"); }
        }

        public string SqlGetListaPotencias
        {
            get { return base.GetSqlXml("GetListaPotencias"); }
        }
        #endregion

        #region INTERVENCIONES
        public string SqlListarComboEquiposXUbicaciones
        {
            get { return base.GetSqlXml("ListarComboEquiposXUbicaciones"); }
        }

        public string SqlTraerDatosEquipamientoByIdEquipo
        {
            get { return base.GetSqlXml("TraerDatosEquipamientoByIdEquipo"); }
        }

        // Metodos Agregado para Procedimiento Maniobra
        public string SqlListarEquiposByIds
        {
            get { return base.GetSqlXml("ListarEquiposByIds"); }
        }

        public string SqlObtenerEnlacesByIds
        {
            get { return base.GetSqlXml("ObtenerEnlacesByIds"); }
        }
        

        public string SqlListarComboEquiposXUbicacionesXFamilia
        {
            get { return base.GetSqlXml("ListarComboEquiposXUbicacionesXFamilia"); }
        }

        public string SqlListarEquiposXPrograma
        {
            get { return GetSqlXml("ListarEquiposXPrograma"); }
        }

        public string SqlListarLineasValidas
        {
            get { return GetSqlXml("ListarLineasValidas"); }
        }
        public string SqlListarCeldasValidas
        {
            get { return GetSqlXml("ListarCeldasValidas"); }
        }

        #endregion

        #region siosein2
        public string Tipoemprcodi = "Tipoemprcodi";
        public string Tipoemprdesc = "Tipoemprdesc";
        #endregion

        #region Numerales Datos Base 
        public string SqlDatosBase_5_6_6
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_6"); }
        }
        #endregion

        #region Medidores de Generación PR15
        public string SqlEquipoByPtoMedicion
        {
            get { return base.GetSqlXml("EquiposPorPuntoMedicion"); }
        }
        #endregion

        #region Equipos sin datos de ficha técnica
        public string SqlListaEqEmpresaFamilia
        {
            get { return base.GetSqlXml("ListaEqEmpresaFamilia"); }
        }
        #endregion

        #region Mejoras Yupana
        public string SqlListarUnidadesxPlanta2
        {
            get { return base.GetSqlXml("ListarUnidadesxPlanta2"); }
        }
        #endregion

        #region FICHA TÉCNICA
        public string SqlListarSubestacionFT
        {
            get { return base.GetSqlXml("ListarSubestacionFT"); }
        }

        #endregion

        #region Ficha tecnica 2023
        public string SqlListarPorEmpresaPropietaria
        {
            get { return base.GetSqlXml("ListarPorEmpresaPropietaria"); }
        }

        public string SqlListarPorEmpresaCopropietaria
        {
            get { return base.GetSqlXml("ListarPorEmpresaCopropietaria"); }
        }

        public string SqlGetByEmprFamCentral2
        {
            get { return base.GetSqlXml("GetByEmprFamCentral2"); }
        }
        #endregion

        #region DEMANDA DPO - ITERACION 2
        public string SqlListaEquipoByEmpresa
        {
            get { return base.GetSqlXml("ListaEquipoByEmpresa"); }
        }
        
        #endregion

        #region GESPROTEC - 20241031
        public string ListaEquipoCOES
        {
            get { return base.GetSqlXml("ListaEquipoCOES"); }
        }

        public string ListaReporteEquipoCOES
        {
            get { return base.GetSqlXml("ListaReporteEquipoCOES"); }
        }

        #endregion

        #region Equipo Proteccion 2024 


        public string SqlListMaestroCeldaProteccion
        {
            get { return base.GetSqlXml("ListMaestroCeldaProteccion"); }
        }

        public string SqlListMaestroReleProteccion
        {
            get { return base.GetSqlXml("ListMaestroReleProteccion"); }
        }

        public string SqlListMaestroInterruptorProteccion
        {
            get { return base.GetSqlXml("ListMaestroInterruptorProteccion"); }
        }

        public string SqlListLineaEvaluacion
        {
            get { return base.GetSqlXml("ListLineaEvaluacion"); }
        }
        #endregion
        #region CPPA.ASSETEC.2024
        public string SqlListaCentralesGeneradoras
        {
            get { return base.GetSqlXml("ListaCentralesGeneradoras"); }
        }
        #endregion
        
        
        #region GESPROTECT 20250206

        public string SqlListMaestroEquiposLinea
        {
            get { return base.GetSqlXml("ListMaestroEquiposLinea"); }
        }

        public string SqlListMaestroEquiposArea
        {
            get { return base.GetSqlXml("ListMaestroEquiposArea"); }
        }

        public string SqlListMaestroEquiposCondensador
        {
            get { return base.GetSqlXml("ListMaestroEquiposCondensador"); }
        }

        public string SqlListMaestroEquiposReactor
        {
            get { return base.GetSqlXml("ListMaestroEquiposReactor"); }
        }

        public string SqlListMaestroEquiposCeldasAcoplamiento
        {
            get { return base.GetSqlXml("ListMaestroEquiposCeldasAcoplamiento"); }
        }

        public string SqlListMaestroEquiposTransformador
        {
            get { return base.GetSqlXml("ListMaestroEquiposTransformador"); }
        }

        #endregion
    }
}
