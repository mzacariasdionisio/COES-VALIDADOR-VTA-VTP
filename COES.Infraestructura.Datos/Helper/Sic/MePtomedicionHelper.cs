using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_PTOMEDICION
    /// </summary>
    public class MePtomedicionHelper : HelperBase
    {
        public MePtomedicionHelper(): base(Consultas.MePtomedicionSql)
        {
        }

        public MePtomedicionDTO Create(IDataReader dr)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iOsicodi = dr.GetOrdinal(this.Osicodi);
            if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iCodref = dr.GetOrdinal(this.Codref);
            if (!dr.IsDBNull(iCodref)) entity.Codref = Convert.ToInt32(dr.GetValue(iCodref));

            int iPtomedidesc = dr.GetOrdinal(this.Ptomedidesc);
            if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

            int iOrden = dr.GetOrdinal(this.Orden);
            if (!dr.IsDBNull(iOrden)) entity.Orden = Convert.ToInt32(dr.GetValue(iOrden));

            int iPtomedielenomb = dr.GetOrdinal(this.Ptomedielenomb);
            if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

            int iPtomedibarranomb = dr.GetOrdinal(this.Ptomedibarranomb);
            if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

            int iOriglectcodi = dr.GetOrdinal(this.Origlectcodi);
            if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

            int iTipoptomedicodi = dr.GetOrdinal(this.Tipoptomedicodi);
            if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt16(dr.GetValue(iTipoptomedicodi));

            int iPtomediestado = dr.GetOrdinal(this.Ptomediestado);
            if (!dr.IsDBNull(iPtomediestado)) entity.Ptomediestado = dr.GetString(iPtomediestado);

            int iLastcodi = dr.GetOrdinal(this.Lastcodi);
            if (!dr.IsDBNull(iLastcodi)) entity.Lastcodi = Convert.ToInt32(dr.GetValue(iLastcodi));

            int iTipoSerie;
            if (columnsExist(this.TipoSerie, dr))
            {
                iTipoSerie = dr.GetOrdinal(this.TipoSerie);
                if (!dr.IsDBNull(iTipoSerie)) entity.TipoSerie = Convert.ToInt32(dr.GetValue(iTipoSerie));
            }
            
            return entity;
        }

        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        #region Mapeo de Campos

        public string Ptomedicodi = "PTOMEDICODI";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Osicodi = "OSICODI";
        public string Equicodi = "EQUICODI";
        public string Codref = "CODREF";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Orden = "ORDEN";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Tipoptomedicodi = "TPTOMEDICODI";
        public string Ptomediestado = "PTOMEDIESTADO";
        public string Famcodi = "FAMCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Centralnomb = "CENTRALNOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Origlectnombre = "Origlectnombre";
        public string Equinomb = "Equinomb";
        public string Famnomb = "Famnomb";
        public string Tipoptomedinomb = "Tptomedinomb";
        public string AreaOperativa = "Areaoperativa";
        public string NivelTension = "Niveltension";
        public string DesUbicacion = "Desubicacion";
        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Ptomedicalculado = "Ptomedicalculado";
        public string TipoSerie = "TipoSerieCodi";
        public string Equipadre = "Equipadre";
        public string Central = "Central";
        public string Lastcodi = "LASTCODI";
        public string Tipoinfoabrev = "Tipoinfoabrev";
        public string Emprabrev = "Emprabrev";
        public string Tptomedinomb = "Tptomedinomb";
        public string Grupocentral = "Grupocentral";
        public string Grupopadre = "GRUPOPADRE";
        public string Equiabrev = "EQUIABREV";
        public string TipoRelacioncodi = "TRPTOCODI";
        public string PtomedicodiCalculado = "PTOMEDICODI_CALCULADO";
        public string PtomedicodidescCalculado = "PTOMEDICODIDESC_CALCULADO";
        public string FactorOrigen = "FACTOR_ORIGEN";
        public string Grupoabrev = "GRUPOABREV";
        public string FactorPotencia = "RELPTOPOTENCIA";

        public string EquipadreOrigen = "EQUIPADRE_ORIGEN";
        public string CentralOrigen = "CENTRAL_ORIGEN";

        public string EquicodiOrigen = "EQUICODI_ORIGEN";
        public string EquinombOrigen = "EQUINOMB_ORIGEN";
        public string EquiabrevOrigen = "EQUIABREV_ORIGEN";

        public string FamcodiOrigen = "FAMCODI_ORIGEN";
        public string FamnombOrigen = "FAMNOMB_ORIGEN";
        public string FamabrevOrigen = "FAMABREV_ORIGEN";
        public string Famabrev = "Famabrev";
        public string Catenomb = "CATENOMB";
        public string GrupoEstado = "GRUPOESTADO";
        public string Emprestado = "EMPRESTADO";

        public string PtomedibarranombOrigen = "Ptomedibarranomb_ORIGEN";
        public string PtomedielenombOrigen = "Ptomedielenomb_ORIGEN";
        public string EmprcodiOrigen = "EMPRCODI_ORIGEN";
        public string EmprabrevOrigen = "EMPRABREV_ORIGEN";
        public string EmprnombOrigen = "EMPRNOMB_ORIGEN";
        public string Relptocodi = "RELPTOCODI";
        public string PtomedicodiOrigen = "PTOMEDICODI_ORIGEN";
        public string PtomedicodidescOrigen = "PTOMEDICODIDESC_ORIGEN";
        public string Lectcodi = "Lectcodi";
        public string Lectnomb = "Lectnomb";
        public string Tgenercodi = "Tgenercodi";
        public string Hojaptoactivo = "HPTOACTIVO";
        public string Catecodi = "CATECODI";

        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Repptotabmed = "RELPTOTABMED";

        public string Emprcodisuministrador = "EMPRCODISUMINISTRADOR";

        public string Fenergcodi = "FENERGCODI";
        public string Fenergnomb = "Fenergnomb";
        public string Tgenernomb = "TGENERNOMB";
        public string Tipogenerrer = "TIPOGENERRER";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Grupointegrante = "GRUPOINTEGRANTE";

        public string Repptonomb = "REPPTONOMB";
        public string Colorcelda = "COLORCELDA";

        #region MigracionSGOCOES-GrupoB
        public string Equitension = "EQUITENSION";
        public string Equiestado = "Equiestado";
        public string Grupoactivo = "Grupoactivo";
        #endregion

        // -----------------------------------------------------------------------------------------------------------------
        // ASSETEC 01-07-2022
        // -----------------------------------------------------------------------------------------------------------------
        public string Grupocodibarra = "GRUPOCODIBARRA";
        // -----------------------------------------------------------------------------------------------------------------

        public string Canales = "CANALES";

        #endregion

        public string SqlListarPtoMedicion
        {
            get { return base.GetSqlXml("ListarPtoMedicion"); }
        }

        public string SqlGetByIdEquipo
        {
            get { return base.GetSqlXml("GetByIdEquipo"); }
        }

        public string SqlGetByIdGrupo
        {
            get { return base.GetSqlXml("GetByIdGrupo"); }
        }

        public string SqlGetPtoDuplicado
        {
            get { return base.GetSqlXml("GetPtoDuplicado"); }
        }

        public string SqlGetPtoDuplicadoNombreEmpresa
        {
            get { return base.GetSqlXml("GetPtoDuplicadoNombreEmpresa"); }
            
        }

        public string SqlGetPtoDuplicadoGrupo
        {
            get { return base.GetSqlXml("GetPtoDuplicadoGrupo"); }
        }

        public string SqlTotalListaPtoMedicion
        {
            get { return base.GetSqlXml("TotalListaPtoMedicion"); }
        }

        public string SqlTotalListaPtoMedicionGrupo
        {
            get { return base.GetSqlXml("TotalListaPtoMedicionGrupo"); }
        }

        public string SqlListarDetallePtoMedicionFiltro
        {
            get { return base.GetSqlXml("ListarDetallePtoMedicionFiltro"); }
        }

        public string SqlListarDetallePtoMedicionFiltroGrupo
        {
            get { return base.GetSqlXml("ListarDetallePtoMedicionFiltroGrupo"); }
        }

        public string SqlVerificarRelaciones
        {
            get { return base.GetSqlXml("VerificarRelaciones"); }
        }

        public string SqlObtenerAreasFiltro
        {
            get { return base.GetSqlXml("ObtenerAreasFiltro"); }
        }

        public string SqlVerificarExistencia
        {
            get { return base.GetSqlXml("VerificarExistencia"); }
        }

        public string SqlListarPotencia
        {
            get { return base.GetSqlXml("ListarPotencia"); }
        }

        public string SqlListarPotenciaEquipo
        {
            get { return base.GetSqlXml("ListarPotenciaEquipo"); }
        }
        
        public string SqlListarCostoVariableAGC
        {
            get { return base.GetSqlXml("ListarCostoVariableAGC"); }
        }

        public string SqlListarTipoGrupo
        {
            get { return base.GetSqlXml("ListarTipoGrupo"); }
        }

        public string SqlGetByIdAgc
        {
            get { return base.GetSqlXml("GetByIdAgc"); }
        }

        public string SqlListarControlCentralizado
        {
            get { return base.GetSqlXml("ListarControlCentralizado"); }
        }

        public string SqlGetByCriteria3
        {
            get { return base.GetSqlXml("GetByCriteria3"); }
        }

        public string SqlUpdateMePtomedicion
        {
            get { return base.GetSqlXml("UpdateMePtomedicion"); }
        }

        public string SqlUpdateMePtomedicionCVariable
        {
            get { return base.GetSqlXml("UpdateMePtomedicionCVariable"); }
        }

        public string SqlListOrigenLectura
        {
            get { return base.GetSqlXml("ListOrigenLectura"); }
        }
        
	    // Inicio de Agregados - Sistema de Compensaciones
        public string SqlObtenerMaximoOrden
        {
            get { return base.GetSqlXml("ObtenerMaximoOrden"); }
        }

        public string SqlListPtoMedicionCompensaciones
        {
            get { return base.GetSqlXml("ListPtoMedicionCompensaciones"); }
        }
        // Fin de Agregados - Sistema de Compensaciones

        public string SqlListarCentralByOriglectcodi
        {
            get { return base.GetSqlXml("ListarCentralByOriglectcodi"); }
        }

        #region SIOSEIN

        public string SqlListPtoMedicionMeLectura
        {
            get { return base.GetSqlXml("ListPtoMedicionMeLectura"); }
        }

        public string SqlGetByCriteria2
        {
            get { return base.GetSqlXml("GetByCriteria2"); }
        }

        #endregion

        //- Pruebas aleatorias
        public string SqlObtenerMedicionSorteo
        {
            get { return base.GetSqlXml("ObtenerMedicionSorteo"); }
        }

        #region Transferencia de Equipos
        public string ListarPtosMedicionXEmpresa {
            get { return base.GetSqlXml("ListarPtosMedicionXEmpresa"); }
        }
        #endregion

        #region PR5

        public string SqlListarPtoMedicionByOriglectcodiAndFormato
        {
            get { return base.GetSqlXml("ListarPtoMedicionByOriglectcodiAndFormato"); }
        }

        public string SqlListarByEquiOriglectcodi
        {
            get { return base.GetSqlXml("ListarByEquiOriglectcodi"); }
        }

        public string SqlListarPuntosCalculados
        {
            get { return base.GetSqlXml("ListarPuntosCalculados"); }
        }

        public string SqlListarByEquiOriglectcodiHisto
        {
            get { return base.GetSqlXml("ListarByEquiOriglectcodiHisto"); }
        }

        public string SqlListarPtoMedicionFromCalculado
        {
            get { return base.GetSqlXml("ListarPtoMedicionFromCalculado"); }
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlObtenerPuntoMedicionExtranet
        {
            get { return base.GetSqlXml("ObtenerPuntoMedicionExtranet"); }
        }

        public string SqlGetListaPuntoMedicionXEmpresa
        {
            get { return base.GetSqlXml("ListaPuntoMedicionXEmpresa"); }
        }

        #endregion

        #region MDCOES


        public string SqlListarPorRecurcodiTermo
        {
            get { return base.GetSqlXml("ListByRecurcodiTermo"); }
        }

        public string SqlListarPorRecurcodiHidro
        {
            get { return base.GetSqlXml("ListByRecurcodiHidro"); }
        }

        public string SqlLeerMedidores
        {
            get { return base.GetSqlXml("LeerMedidores"); }
        }
        public string SqlLeerPtoMedicionHidrologia
        {
            get { return base.GetSqlXml("LeerPtoMedicionHidrologia"); }
        }
        public string SqlDatosHidrologia
        {
            get { return base.GetSqlXml("DatosHidrologia"); }
        }
        public string SqlListaCentralesPMPO
        {
            get { return base.GetSqlXml("ListaCentralesPMPO"); }
        }
        public string SqlListaBarrasPMPO
        {
            get { return base.GetSqlXml("ListaBarrasPMPO"); }
        }
        #endregion

        #region Mejoras IEOD

        public string SqlListarPtomedicionByOriglectcodi
        {
            get { return base.GetSqlXml("ListarPtomedicionByOriglectcodi"); }
        }

        public string SqlListarPtomedicionDespachoAntiguo
        {
            get { return base.GetSqlXml("ListarPtomedicionDespachoAntiguo"); }
        }

        #endregion

        #region Titularidad-Instalaciones-Empresas

        public string SqlListarPtomedicionByMigracodi
        {
            get { return base.GetSqlXml("ListarPtomedicionByMigracodi"); }
        }

        #endregion

        #region FIT - VALORIZACION DIARIA

        public string Clientecodi = "CLIENTECODI";
        public string PuntoConexion = "PTOCONEXION";
        public string Barrcodi = "BARRCODI";
        public string Cliennomb = "CLIENNOMB";
        public string Barranomb = "BARRANOMB";

        public string SqlGetPtoDuplicadoTransferencia
        {
            get { return GetSqlXml("GetPtoDuplicadoTransferencia"); }
        }

        public string SqlGetByIdCliente
        {
            get { return GetSqlXml("GetByIdCliente"); }
        }

        public string SqlTotalListaPtoMedicionTransferencia
        {
            get { return GetSqlXml("TotalListaPtoMedicionTransferencia"); }
        }

        public string SqlListarDetallePtoMedicionFiltroTransferencias
        {
            get { return GetSqlXml("ListarDetallePtoMedicionFiltroTransferencia"); }
        }

        public string SqlGetByIdClienteBarraMePtomedicion
        {
            get { return base.GetSqlXml("GetByIdClienteBarraMePtomedicion"); }
        }

        #endregion

        #region Numerales Datos Base


        public string Nombre = "NOMBRE";
        public string Fecha = "FECHA";
        public string Emprruc = "EMPRRUC";
        public string Emprrazsocial = "EMPRRAZSOCIAL";

        public string SqlDatosBase_5_8_1
        {
            get { return base.GetSqlXml("ListaDatosBase_5_8_1"); }
        }
        public string SqlDatosBase_5_8_2
        {
            get { return base.GetSqlXml("ListaDatosBase_5_8_2"); }
        }
        public string SqlListaPtoUsuariosLibres
        {
            get { return base.GetSqlXml("ListaPtoUsuariosLibres"); }
        }
        #endregion

        public string SqlGetByIdEquipoUsuarioLibre
        {
            get { return base.GetSqlXml("GetByIdEquipoUsuarioLibre"); }
        }

        #region Demanda DPO - Iteracion 2
        public string SqlListaPuntoByEquipo
        {
            get { return base.GetSqlXml("ListaPuntoByEquipo"); }
        }

        public string SqlListaPuntoByOrigenEmpresa
        {
            get { return base.GetSqlXml("ListaPuntoByOrigenEmpresa"); }
        }
        public string SqlListaPuntoSicliByEmpresa
        {
            get { return base.GetSqlXml("ListaPuntoSicliByEmpresa"); }
        }

        public string SqlListaPuntoMedicionByLista
        {
            get { return base.GetSqlXml("ListaPuntoMedicionByLista"); }
        }
        #endregion
        public string SqlPtoMedicionModificados
        {
            get { return base.GetSqlXml("PuntosModificados"); }
        }        

        public string SqlObtenerPuntosMedicionReporte
        {
            get { return base.GetSqlXml("ObtenerPuntosMedicionReporte"); }
        }

        public string SqlTotalListaPtoMedicionNoDefinido
        {
            get { return base.GetSqlXml("TotalListaPtoMedicionNoDefinido"); }
        }
        public string SqlListarDetallePtoMedicionNoDefinido
        {
            get { return GetSqlXml("ListarDetallePtoMedicionNoDefinido"); }
        }
        
    }
}
