using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_PTOMEDICION
    /// </summary>
    public interface IMePtomedicionRepository
    {
        int Save(MePtomedicionDTO entity);
        void Update(MePtomedicionDTO entity);
        void Delete(int ptomedicodi);
        void Delete_UpdateAuditoria(int ptomedicodi, string username);
        MePtomedicionDTO GetById(int ptomedicodi);
        List<MePtomedicionDTO> List(string ptomedicodi, string origlectcodi);
        List<MePtomedicionDTO> GetByCriteria(string empresas, string idsOriglectura, string idsTipoptomedicion);
        List<MePtomedicionDTO> ListarPtoMedicion(string listapto);
        List<MePtomedicionDTO> GetByIdEquipo(int equicodi);
        List<MePtomedicionDTO> GetByIdGrupo(int grupocodi);
        List<MePtomedicionDTO> ListarPtoDuplicado(int equipo, int origen, int tipopto);
        List<MePtomedicionDTO> ListarPtoDuplicadoNombreEmpresa(string nombrepto, int empresacodi);

        List<MePtomedicionDTO> ListarPtoDuplicadoGrupo(int grupo, int origen, int tipopto);
        #region FIT - VALORIZACION DIARIA
        List<MePtomedicionDTO> ListarDetallePtoMedicionFiltro(string empresas, string idsOriglectura, string idsTipoptomedicion,
                    int nroPaginas, int pageSize, string idsFamilia, string ubicacion, string categoria, int tipoPunto, int codigo,
                    int? cliente, int? barra, string campo, string orden);
        int ObtenerTotalPtomedicion(string empresas, string idsOriglectura, string idsTipoptomedicion, string idsFamilia, string ubicacion, string categoria,
            int tipoPunto, int codigo, int? cliente, int? barra);
        #endregion

        int VerificarRelaciones(int ptoMedicion);
        List<EqAreaDTO> ObtenerAreasFiltro();
        int VerificarExistencia(int equicodi, int origenlectcodi);
        List<MePtomedicionDTO> ListarPotencia(string familia, int idOriglectura, string control);
        MePtomedicionDTO ListarPotenciaEquipo(int ptomedicodi);
        List<MePtomedicionDTO> ListarCostoVariableAGC();
        string ListarTipoGrupo(string puntoMedicion);
        MePtomedicionDTO GetByIdAgc(int ptomedicodi);
        List<MePtomedicionDTO> ListarControlCentralizado();
        List<MePtomedicionDTO> GetByCriteria3(int origlectcodi, string grupocodi, string tipoinfocodis);
        void UpdateMePtoMedicion(MePtomedicionDTO entity);
        void UpdateMePtoMedicionCVariable(MePtomedicionDTO entity);
        List<MePtomedicionDTO> ListByOriglectcodi(string origlectcodi, DateTime fechaIni, DateTime fechaFin);
        // Inicio de Agregado - Sistema de Compensaciones
        List<string> LstGrillaHead(int pecacodi);
        List<ComboCompensaciones> LstGrillaBody(int pecacodi);
        List<MePtomedicionDTO> ListPtoMedicionCompensaciones(int ptoMediCodi, int pecacodi);
        // Fin de Agregado - Sistema de Compensaciones

        #region PR5
        List<MePtomedicionDTO> ListarCentralByOriglectcodiAndFormato(string origlectcodi, string famcodi, int formatcodi);
        List<MePtomedicionDTO> ListarPtoMedicionByOriglectcodiAndFormato(int origlectcodi, int formatcodi, DateTime fechaIni, DateTime fechaFin);
        List<MePtomedicionDTO> ListarByEquiOriglectcodi(int equipo, int origlectcodi, int lectcodi);
        List<MePtomedicionDTO> ListarPuntosCalculados();
        List<MePtomedicionDTO> ListarPtoMedicionFromCalculado(string ptomedicalculado);
        #endregion

        #region SIOSEIN
        List<MePtomedicionDTO> ListPtoMedicionMeLectura(int origlectcodi, int lectcodi, int tipoinfocodi);
        List<MePtomedicionDTO> GetByCriteria2(string equicodi, string origlectcodi);
        #endregion

        //-Pruebas aleatorias
        int ObtenerPtomedicionSorteo(DateTime fecha, int origlectcodi);

        #region MigracionSGOCOES-GrupoB
        List<MePtomedicionDTO> ObtenerPuntoMedicionExtranet(int idEquipo);
        List<MePtomedicionDTO> GetListaPuntoMedicionPorEmpresa(string emprcodi, DateTime fechaPeriodoIni, DateTime fechaPeriodoFin);
        #endregion

        #region Transferencia de Equipos
        List<MePtomedicionDTO> ListarPtosMedicionXEmpresa(int idEmpresa);
        #endregion

        #region Mejoras IEOD

        List<MePtomedicionDTO> ListarPtomedicionByOriglectcodi(string origlectcodi, string famcodi, int emprcodi);
        List<MePtomedicionDTO> ListarPtomedicionDespachoAntiguo(int emprcodi);
        #endregion

        #region Titularidad-Instalaciones-Empresas

        List<MePtomedicionDTO> ListarPtomedicionByMigracodi(int idMigracion);

        #endregion

        #region FIT - VALORIZACION DIARIA
        List<MePtomedicionDTO> ListarPtoDuplicadoTransferencia(int clientecodi, int barracodi, int origen, int tipopto);
        List<MePtomedicionDTO> GetByIdClienteBarraMePtomedicion(int? idEmpresa, int? cliente, int? barra, int idOrigenLectura);
        #endregion

        #region Numerales Datos Base
        List<MePtomedicionDTO> ListaNumerales_DatosBase_5_8_1(string fechaIni, string fechaFin);
        List<MePtomedicionDTO> ListaNumerales_DatosBase_5_8_2(string fechaIni, string fechaFin);
        List<MePtomedicionDTO> ListaPtoUsuariosLibres(int tipoempresa);
        #endregion

        MePtomedicionDTO GetByRecurcodi(int miRecurcodi, int tipo);

        List<MePtomedicionDTO> GetByIdEquipoUsuarioLibre(int equicodi, int emprcodisuministrador);

        #region Demanda DPO - Iteracion 2
        List<MePtomedicionDTO> ListaPuntoByEquipo(int equipo);
        List<MePtomedicionDTO> ListaPuntoByOrigenEmpresa(int origen, int empresa);
        List<MePtomedicionDTO> ListaPuntoMedicionByLista(string puntos);
        List<MePtomedicionDTO> ListaPuntoSicliByEmpresa(int origen, int empresa);
        #endregion

        List<MePtomedicionDTO> ListadoPtoMedicionModificados(DateTime dtFechaInicio, DateTime dtFechaFin);

        List<MePtomedicionDTO> ObtenerPuntosMedicionReporte(int idReporte, int tipoinfocodi);
        List<MeMedicion96DTO> LeerMedidores(DateTime fechaInicio);

        List<MePtomedicionDTO> LeerPtoMedicionHidrologia();
        List<MePtomedicionDTO> ObtenerDatosHidrologia(DateTime dtFechaInicio, DateTime dtFechaFin);

        #region CPPA.ASSETEC.2024
        List<MePtomedicionDTO> ListaCentralesPMPO(int empresa);
        List<MePtomedicionDTO> ListaBarrasPMPO();
        #endregion
    }
}
