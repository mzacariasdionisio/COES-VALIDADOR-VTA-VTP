using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnPronosticoDemandaRepository
    {
        #region Configuración General
        List<EqAreaNivelDTO> ListPrnNivel();
        List<EqAreaDTO> ListPrnArea(int anivelcodi);
        List<EqAreaDTO> ListPrnAreaGrupo(int anivelcodi, int areacodi);
        List<MePtomedicionDTO> ListPrnPtoMedicion(int origlectcodi, int anivelcodi, int areapadre, int areacodi);
        List<MePtomedicionDTO> ListPrnPtoMedicionDistr(int origlectcodi, int tipoemprcodi, int anivelcodi, int areapadre, int areacodi);
        bool ValidarPlantillaAnio(DateTime fecha);
        List<MePtomedicionDTO> ListPtomedicionActivos();

        #endregion

        #region Depuración Manual
        List<PrnMedicion48DTO> ListPuntosClasificadosByFecha(int ejec48, int ejec96, int prntipo, DateTime medifecha);
        List<SiEmpresaDTO> ListEmpresasByTipo(int tipoemprcodi, int origlectcodi);
        List<EqAreaDTO> ListSubestacionesByEmpresa(int nivelao, int nivelsubest, int emprcodi);
        #endregion

        #region Información Usuarios Libres
        List<EqEquipoDTO> ListSubestacionEmpresa();
        List<MePtomedicionDTO> ListPuntosBySubestacionEmpresa(int emprcodi, int areacodi, DateTime medifecha);
        int CountDespEjecByTipoEmpresa(int tipoemprcodi, int ptomedicodi, int lectcodi, int tipoinfocodi, DateTime fecini, DateTime fecfin);
        
        #endregion

        #region Adicionales
        List<MePtomedicionDTO> ListPtoMedicionByEmpresaArea(int tipoemprcodi, int areapadre);
        List<MePtomedicionDTO> ListMePtomedicionAO();
        EqAreaDTO GetAreaOperativaByEquipo(int equicodi);
        List<EqAreaDTO> GetAreaOperativaByNivel(int anivelcodi);
        //07-05
        List<EqAreaDTO> GetSubestacionCentralByNivel(int anivelcodi);
        List<EqAreaDTO> GetSubEstacionDisponibles(int areanivel, int areapadre, int relpadre, int relnivel, int arearelnivel);  
        List<EqAreaDTO> GetSubEstacionSeleccionadas(int area, int nivel);

        void DeleteRelacion(int areacodi, int areapadre);

        void DeleteByPadre(int areapadre);
        EqAreaDTO GetAreaOperativaBySubestacion(int areacodi);
        EqAreaRelDTO GetSubestacionRel(int areacodi);
        List<PrGrupoDTO> GetBarrasSeleccionadas(int areacodi);
        List<PrGrupoDTO> GetBarrasDisponibles(int areacodi);
        void UpdatePrGrupo(int grupo, int subestacion);

        List<PrGrupoDTO> GetListBarras();
        List<PrGrupoDTO> ListBarrasPM(int catecodi, string barrascp, int version);
        List<PrGrupoDTO> ListBarraCPDisponibles(int catecodi, int prnvercodi);

        //Métodos para el modulo de Relación de Barras
        List<PrGrupoDTO> ListRelacionBarrasPM(int anivelcodi, int anivelcodi2, int catecodi, string areapadre, string grupocodi);
        List<MePtomedicionDTO> ListRelacionPtosPorBarraPM(string grupocodibarra);
        List<PrnAgrupacionDTO> AgrupacionesPuntosList(int origen, int barra);
        List<PrnAgrupacionDTO> AgrupacionesList(int origen, int barra);
        List<MePtomedicionDTO> ListPuntosNoAgrupaciones(int orgagrupacion, int barra, int orgpunto);
        List<MePtomedicionDTO> ListPuntosBarra(int grupobarra);

        void UpdateMeMedicionBarra(int punto, int barra, string user);

        List<PrGrupoDTO> ListBarrasPMNombre(int grupocodi, int catecodi);
        PrGrupoDTO ListBarrasCPNombre(int grupocodi, int catecodi);
        //Fin

        List<PrGrupoDTO> GetLisBarrasSoloPM(int catecodi, string barracp);
        List<PrGrupoDTO> GetLisBarrasSoloCP(int catecodi);

        //16032020
        List<PrGrupoDTO> ListBarrasPMEdit(int catecodi, int version);

        //17032020
        List<PrGrupoDTO> ListBarraPMDisponibles(int catecodi, int prnvercodi);

        //14042020
        List<SiEmpresaDTO> ListEmpresaBarrasRel();
        List<PrGrupoDTO> ListPuntosByEmpresa(string empresa);
        List<SiEmpresaDTO> ListEmpresaByBarra(string barra);
        List<PrGrupoDTO> ListBarrasInPtoMedicion(int anivelcodi, int anivelcodi2, int catecodi, string areapadre, string grupocodi);

        //Perdidas transversales
        //List<PrGrupoDTO> ListaBarrasPerdidasTransversales();
        List<PrnPrdTransversalDTO> ListaPerdidasTransversalesByBarra(string barras);
        List<PrGrupoDTO> PerdidasTransversalesCPDisponibles(string barra);
        List<PrnPrdTransversalDTO> PerdidasTransversalesCPSeleccionadas(string barra);
        void DeletePerdidaTransversal(int barra);
        int VersionActiva();
        List<PrGrupoDTO> ListBarraDefecto(int catecodi, int prnvercodi);

        //Bitacora
        List<MeJustificacionDTO> ListaBitacora(string fechaIni,  
            string fechaFin, string lectcodi,
            string tipoempresa, int regIni,
            int regFin);
        #endregion

        #region PRODEM3

        #region Filtros consulta/configuracion de estimador
        List<MePtomedicionDTO> ListUnidadByTipo(int tipo);

        List<MePtomedicionDTO> ListPuntosFormulas(int tipo);

        List<MePerfilRuleDTO> ListPerfilRuleByEstimador(string prefijo);
        #endregion

        List<MePtomedicionDTO> ListPtomedicionByOriglectcodi(int origlectcodi);

        #region Traslado de carga     
        List<PrnMediciongrpDTO> ListVersionMedicionGrp();

        PrnMediciongrpDTO MedicionBarraByFechaVersionBarra(string fecha, int version, int barra);

        void UpdateMedicionTrasladoCarga(PrnMediciongrpDTO entity);

        List<PrnMediciongrpDTO> GetLisBarrasSoloCPTraslado(int catecodi, int vergrpcodi);
        #endregion

        #region Desviacion
        PrnMediciongrpDTO GetBarrasCPGroupByFechaTipo(int prnmgrtipo, DateTime medifecha);
        #endregion

        //Assetec 20220201
        List<PrnMediciongrpDTO> GetDataFormatoPronosticoDemandaByVersion(int formatcodi, DateTime fechaini, DateTime fechafin, int version);

        //Assetec 20220321
        List<MePtomedicionDTO> GetAgrupacionByBarraPM(int pm);
        #endregion

        // -----------------------------------------------------------------------------------------------------------------
        // ASSETEC 07-03-2022 metodos tabla BITACORA
        // -----------------------------------------------------------------------------------------------------------------
        #region BITACORA.E3
        void SaveBitacora3(PrnBitacoraDTO entity);
        List<PrnBitacoraDTO> ListBitacora(string fechaIni,
            string fechaFin, string tipregistro, 
            string tipoemprcod, string lectcodi);
        #endregion
        // ------------------------------------ FIN ASSETEC 14-03-2022 -----------------------------------------------------

        #region Mejoras PRODEM.E3 40 horas
        int TotalRegConsultaBitacora(string idLectura,
            string idTipoEmpresa,
            string fechaIni,
            string fechaFin);
        #endregion
    }
}
