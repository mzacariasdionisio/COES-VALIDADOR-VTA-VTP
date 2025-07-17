using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using System.Data;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnPronosticoDemandaHelper : HelperBase
    {
        public PrnPronosticoDemandaHelper() : base(Consultas.PrnPronosticoDemandaSql)
        {
        }

        #region Mapeo de los campos

        //CONFIGURACIÓN GENERAL
        //---------------------------------------------------------------------------------------------------
        public string Pmedatcodi = "PMEDATCODI";
        public string Anivelcodi = "ANIVELCODI";
        public string Anivelnomb = "ANIVELNOMB";

        public string Areacodi = "AREACODI";
        public string Areaabrev = "AREAABREV";
        public string Areanomb = "AREANOMB";
        public string Areapadre = "AREAPADRE";

        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptomedidesc = "PTOMEDIDESC";

        public string Pmedatfecha = "PMEDATFECHA";
        public string Parptoporcerrormin = "PARPTOPORCERRORMIN";
        public string Parptoporcerrormax = "PARPTOPORCERRORMAX";
        public string Parptoporcmuestra = "PARPTOPORCMUESTRA";
        public string Parptoporcdesvia = "PARPTOPORCDESVIA";
        public string Parptomagcargamin = "PARPTOMAGCARGAMIN";
        public string Parptomagcargamax = "PARPTOMAGCARGAMAX";
        public string Parptoferiado = "PARPTOFERIADO";
        public string Parptoatipico = "PARPTOATIPICO";
        public string Parptodepurauto = "PARPTODEPURAUTO";
        public string Parptopatron = "PARPTOPATRON";
        public string Parptoveda = "PARPTOVEDA";
        public string Parptonumcoincidencia = "PARPTONUMCOINCIDENCIA";

        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";

        public string Tareaabrev = "TAREAABREV";
        public string Areaoperativa = "AREAOPERATIVA";

        public string AreaRlCodi = "AREARLCODI";

        public string Barracodi = "BARRACODI";

        public string Gruponomb = "GRUPONOMB";

        public string Grupocodi = "GRUPOCODI";

        public string Catecodi = "CATECODI";

        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";


        //2020
        public string Prnvercodi = "PRNVERCODI";

        //---------------------------------------------------------------------------------------------------

        //DEPURACIÓN MANUAL
        //---------------------------------------------------------------------------------------------------
        public string Origlectcodi = "ORIGLECTCODI";
        public string Lectcodi = "LECTODI";

        public string Medifecha = "MEDIFECHA";
        public string Meditotal = "MEDITOTAL";
        public string Prnm48tipo = "PRNM48TIPO";
        public string Prnm96tipo = "PRNM96TIPO";
        public string Prnmestado = "PRNMESTADO";
        public string Prnmintervalo = "PRNMINTERVALO";
        public string Prnclsclasificacion = "PRNCLSCLASIFICACION";

        public string Equicodi = "EQUICODI";        
        public string Equinomb = "EQUINOMB";

        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        //---------------------------------------------------------------------------------------------------

        //INFORMACIÓN USUARIOS LIBRES
        //---------------------------------------------------------------------------------------------------
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Prnitv = "PRNITV";
        //---------------------------------------------------------------------------------------------------

        //20200128
        public string Areapadreabrev = "AREAPADREABREV";
        public string Areapadrenomb = "AREAPADRENOMB";
        public string Grupoabrev = "GRUPOABREV";
        public string Ptomedibarranomb = "PTOMEDIBARRANOMB";

        //RelacionBarras
        public string Ptogrphijocodi = "PTOGRPHIJOCODI";
        public string Ptogrphijodesc = "PTOGRPHIJODESC";
        public string Grupocodibarra = "GRUPOCODIBARRA";
        #endregion

        //Perdida Transversal
        public string Prdtrncodi = "PRDTRNCODI";
        public string Prdtrnetqnomb = "PRDTRNETQNOMB";
        //public string Grupocodi = "GRUPOCODI";
        public string Prdtrnperdida = "PRDTRNPERDIDA";
        //public string Prnvercodi = "PRNVERCODI";

        #region Bitacora
        public string Subcausadesc = "SUBCAUSADESC";
        public string Fechainicio = "FECHAINICIO";
        public string Horainicio = "HORAINICIO";
        public string Fechafin = "FECHAFIN";
        public string Horafin = "HORAFIN";
        //public string Ocurrencia = "Ocurrencia";
        public string ConsumoPrevisto = "CONSUMOPREVISTO";
        public string Area = "AREA";
        //PRODEM3 - AGREGADO PARA LAS FORMULAS           
        public string Prrucodi = "PRRUCODI";
        public string Prruabrev = "PRRUABREV";
        #endregion

        #region PRODEM3
        public string Vergrpcodi = "VERGRPCODI";
        public string Vergrpnomb = "VERGRPNOMB";
        public string Prnmgrtipo = "PRNMGRTIPO";
        public string Prnmgrtraslado = "PRNMGRTRASLADO";
        public string Prnmgrtrasladocount = "PRNMGRTRASLADOCOUNT";
        public string H1 = "H1";
        public string H2 = "H2";
        public string H3 = "H3";
        public string H4 = "H4";
        public string H5 = "H5";
        public string H6 = "H6";
        public string H7 = "H7";
        public string H8 = "H8";
        public string H9 = "H9";
        public string H10 = "H10";
        public string H11 = "H11";
        public string H12 = "H12";
        public string H13 = "H13";
        public string H14 = "H14";
        public string H15 = "H15";
        public string H16 = "H16";
        public string H17 = "H17";
        public string H18 = "H18";
        public string H19 = "H19";
        public string H20 = "H20";
        public string H21 = "H21";
        public string H22 = "H22";
        public string H23 = "H23";
        public string H24 = "H24";
        public string H25 = "H25";
        public string H26 = "H26";
        public string H27 = "H27";
        public string H28 = "H28";
        public string H29 = "H29";
        public string H30 = "H30";
        public string H31 = "H31";
        public string H32 = "H32";
        public string H33 = "H33";
        public string H34 = "H34";
        public string H35 = "H35";
        public string H36 = "H36";
        public string H37 = "H37";
        public string H38 = "H38";
        public string H39 = "H39";
        public string H40 = "H40";
        public string H41 = "H41";
        public string H42 = "H42";
        public string H43 = "H43";
        public string H44 = "H44";
        public string H45 = "H45";
        public string H46 = "H46";
        public string H47 = "H47";
        public string H48 = "H48";
        public string Prnmgrusucreacion = "PRNMGRUSUCREACION";
        public string Prnmgrfeccreacion = "PRNMGRFECCREACION";
        public string Prnmgrusumodificacion = "PRNMGRUSUMODIFICACION";
        public string Prnmgrfecmodificacion = "PRNMGRFECMODIFICACION";
        #endregion

        #region Consultas a la BD

        //CONFIGURACIÓN GENERAL
        //---------------------------------------------------------------------------------------------------
        public string SqlListPrnNivel
        {
            get { return base.GetSqlXml("ListPrnNivel"); }
        }

        public string SqlListPrnArea
        {
            get { return base.GetSqlXml("ListPrnArea"); }
        }

        public string SqlListPrnAreaGrupo
        {
            get { return base.GetSqlXml("ListPrnAreaGrupo"); }
        }

        public string SqlListPrnPtoMedicion
        {
            get { return base.GetSqlXml("ListPrnPtoMedicion"); }
        }

        public string SqlListPrnPtoMedicionDistr
        {
            get { return base.GetSqlXml("ListPrnPtoMedicionDistr"); }
        }

        public string SqlValidarPlantillaAnio
        {
            get { return base.GetSqlXml("ValidarPlantillaAnio"); }
        }

        public string SqlListPtomedicionActivos
        {
            get { return base.GetSqlXml("ListPtomedicionActivos"); }
        }

        //---------------------------------------------------------------------------------------------------

        //DEPURACIÓN MANUAL
        //---------------------------------------------------------------------------------------------------
        public string SqlListPuntosClasificadosByFecha
        {
            get { return base.GetSqlXml("ListPuntosClasificadosByFecha"); }
        }

        public string SqlListEmpresasByTipo
        {
            get { return base.GetSqlXml("ListEmpresasByTipo"); }
        }

        public string SqlListSubestacionesByEmpresa
        {
            get { return base.GetSqlXml("ListSubestacionesByEmpresa"); }
        }
        //---------------------------------------------------------------------------------------------------

        //FLUJOS DE AREA
        //---------------------------------------------------------------------------------------------------
        public string SqlListMePtomedicionAO
        {
            get { return base.GetSqlXml("ListMePtomedicionAO"); }
        }


        //---------------------------------------------------------------------------------------------------

        //INFORMACIÓN USUARIOS LIBRES
        //---------------------------------------------------------------------------------------------------
        public string SqlCountDespEjecByTipoEmpresa
        {
            get { return base.GetSqlXml("CountDespEjecByTipoEmpresa"); }
        }

        public string SqlListSubestacionEmpresa
        {
            get { return base.GetSqlXml("ListSubestacionEmpresa"); }
        }

        public string SqlListPuntosBySubestacionEmpresa
        {
            get { return base.GetSqlXml("ListPuntosBySubestacionEmpresa"); }
        }

        //---------------------------------------------------------------------------------------------------

        //ADICIONALES
        //---------------------------------------------------------------------------------------------------
        public string SqlListPtoMedicionByEmpresaArea
        {
            get { return base.GetSqlXml("ListPtoMedicionByEmpresaArea"); }
        }

        public string SqlGetAreaOperativaByEquipo
        {
            get { return base.GetSqlXml("GetAreaOperativaByEquipo"); }
        }

        //21/10/2019
        public string SqlGetAreaOperativaByNivel
        {
            get { return base.GetSqlXml("GetAreaOperativaByNivel"); }
        }

        //0705 Agregado para el sql de solo Areas por Nivel
        public string SqlGetSubestacionCentralByNivel
        {
            get { return base.GetSqlXml("GetSubestacionCentralByNivel"); }
        }

        public string SqlGetSubEstacionSeleccionadas
        {
            get { return base.GetSqlXml("GetSubEstacionSeleccionadas"); }
        }

        public string SqlGetSubEstacionDisponibles
        {
            get { return base.GetSqlXml("GetSubEstacionDisponibles"); }
        }

        public string SqlGetAreaOperativaBySubestacion
        {
            get { return base.GetSqlXml("GetAreaOperativaBySubestacion"); }
        }

        public string SqlDeleteRelacion
        {
            get { return base.GetSqlXml("GetDeleteRelacion"); }
        }

        public string SqlDeleteByPadre
        {
            get { return base.GetSqlXml("GetDeleteByPadre"); }
        }

        public string SqlGetSubestacionRel
        {
            get { return base.GetSqlXml("GetSubestacionRel"); }
        }

        //GetBarrasSeleccionadas
        public string SqlGetBarrasSeleccionadas
        {
            get { return base.GetSqlXml("GetBarrasSeleccionadas"); }
        }

        public string SqlGetBarrasDisponibles
        {
            get { return base.GetSqlXml("GetBarrasDisponibles"); }
        }

        public string SqlUpdateGrupo
        {
            get { return base.GetSqlXml("UpdateGrupo"); }
        }
        //Parametros Barras
        public string SqlListBarra
        {
            get { return base.GetSqlXml("GetListBarra"); }
        }

        //reduccionred
        public string SqlBarraPM
        {
            get { return base.GetSqlXml("ListBarraPM"); }
        }

        //16032020
        public string SqlBarraPMEdit
        {
            get { return base.GetSqlXml("ListBarraPMEdit"); }
        }


        public string SqlBarraCPDisponible
        {
            get { return base.GetSqlXml("ListaBarrasCPDisponible"); }
        }

        //17032020
        public string SqlBarraPMDisponible
        {
            get { return base.GetSqlXml("ListaBarrasPMDisponible"); }
        }

        //Métodos para el modulo de Relación de Barras
        public string SqlListRelacionBarrasPM
        {
            get { return base.GetSqlXml("ListRelacionBarrasPM"); }
        }
        public string SqlListRelacionPtosPorBarraPM
        {
            get { return base.GetSqlXml("ListRelacionPtosPorBarraPM"); }
        }
        //Fin

        //SqlListRelacionAgrupacionPunto
        public string SqlListRelacionAgrupacionPunto
        {
            get { return base.GetSqlXml("ListRelacionAgrupacionPunto"); }
        }

        public string SqlAgrupacionesList
        {
            get { return base.GetSqlXml("ListaAgrupaciones"); }
        }

        public string SqlPuntosNoAgrupacionesList
        {
            get { return base.GetSqlXml("ListaPuntosNoAgrupaciones"); }
        }

        public string SqlPuntosBarraList
        {
            get { return base.GetSqlXml("PuntosBarraList"); }
        }

        public string SqlUpdateMeMedicionBarra
        {
            get { return base.GetSqlXml("UpdateMeMedicionBarra"); }
        }

        public string SqlListBarrasPMNombre
        {
            get { return base.GetSqlXml("ListBarrasPMNombre"); }
        }

        public string SqlListBarrasCPNombre
        {
            get { return base.GetSqlXml("ListBarrasCPNombre"); }
        }

        public string SqlListBarrasSoloPM
        {
            get { return base.GetSqlXml("ListBarrasSoloPM"); }
        }
        public string SqlListBarrasSoloCP
        {
            get { return base.GetSqlXml("ListBarrasSoloCP"); }
        }

        //14042020
        //Lista Empresas para Relacion Barras
        public string SqlListEmpresaBarrasRel
        {
            get { return base.GetSqlXml("ListEmpresaBarrasRel"); }
        }

        //Lista Puntos por Empresa SqlListPuntosByEmpresa
        public string SqlListPuntosByEmpresa
        {
            get { return base.GetSqlXml("ListPuntosByEmpresa"); }
        }

        //Lista Empresas por Barra
        public string SqlListEmpresaByBarra
        {
            get { return base.GetSqlXml("ListEmpresaByBarra"); }
        }

        //Lista de Barras registradas en Ptomedicion
        public string SqlListBarrasInPtoMedicion
        {
            get { return base.GetSqlXml("ListBarrasInPtoMedicion"); }
        }

        //Perdidas Transversales
        //public string SqlListaBarrasPerdidasTransversales
        //{
        //    get { return base.GetSqlXml("ListaBarrasPerdidasTransversales"); }
        //}
        public string SqlListaPerdidasTransversalesByBarra
        {
            get { return base.GetSqlXml("ListaPerdidasTransversalesByBarra"); }
        }
        public string SqlPerdidasTransversalesCPDisponibles
        {
            get { return base.GetSqlXml("PerdidasTransversalesCPDisponibles"); }
        }
        public string SqlPerdidasTransversalesCPSeleccionadas
        {
            get { return base.GetSqlXml("PerdidasTransversalesCPSeleccionadas"); }
        }
        public string SqlDeletePerdidaTransversal
        {
            get { return base.GetSqlXml("DeletePerdidaTransversal"); }
        }
        public string SqlVersionActiva
        {
            get { return base.GetSqlXml("VersionActiva"); }
        }
        public string SqlBarraDefecto
        {
            get { return base.GetSqlXml("BarraDefecto"); }
        }

        public string SqlListaBitacora
        {
            get { return base.GetSqlXml("ListaBitacora"); }
        }


        #region consulta de estimador - PRODEM3
        public string SqlListUnidadByTipo
        {
            get { return base.GetSqlXml("ListUnidadByTipo"); }
        }

        public string SqlListPuntosFormulas
        {
            get { return base.GetSqlXml("ListPuntosFormulas"); }
        }

        public string SqlListPerfilRuleByEstimador
        {
            get { return base.GetSqlXml("ListPerfilRuleByEstimador"); }
        }
        public string SqlListPtomedicionByOriglectcodi
        {
            get { return base.GetSqlXml("ListPtomedicionByOriglectcodi"); }
        }
        #endregion

        #region traslado de carga - PRODEM3
        public string SqlListVersionMedicionGrp
        {
            get { return base.GetSqlXml("ListVersionMedicionGrp"); }
        }
        public string SqlMedicionBarraByFechaVersionBarra
        {
            get { return base.GetSqlXml("MedicionBarraByFechaVersionBarra"); }
        }
        public string SqlUpdateMedicionTrasladoCarga
        {
            get { return base.GetSqlXml("UpdateMedicionTrasladoCarga"); }
        }
        public string SqlListBarrasSoloCPTraslado
        {
            get { return base.GetSqlXml("ListBarrasSoloCPTraslado"); }
        }
        public string SqlGetBarrasCPGroupByFechaTipo
        {
            get { return base.GetSqlXml("GetBarrasCPGroupByFechaTipo"); }
        }
        #endregion

        //Assetec 20220201
        public string SqlGetListFormatoDemandaCPByVersion
        {
            get { return base.GetSqlXml("GetListFormatoDemandaCPByVersion"); }
        }

        //Assetec 20220321
        public string SqlGetAgrupacionByBarraPM
        {
            get { return base.GetSqlXml("GetAgrupacionByBarraPM"); }
        }
        #endregion

        // -----------------------------------------------------------------------------------------------------------------
        // ASSETEC 07-03-2022 metodos tabla BITACORA
        // -----------------------------------------------------------------------------------------------------------------
        #region Bitacora E3
        public string Prnbitcodi = "PRNBITCODI";

        //public string Emprcodi = "EMPRCODI";
        //public string Medifecha = "MEDIFECHA";

        public string Prnbithorainicio = "PRNBITHORAINICIO";
        public string Prnbithorafin = "PRNBITHORAFIN";
        public string Prnbittipregistro = "PRNBITREGISTRO";
        public string Prnbitocurrencia = "PRNBITOCURRENCIA";

        //public string Grupocodi = "GRUPOCODI";

        public string Prnbitconstipico = "PRNBITTIPICO";
        public string Prnbitconsprevisto = "PRNBITPREVISTO";
        public string Prnbitptodiferencial = "PRNBITDIFERENCIAL";

        //public string Ptomedicodi = "PTOMEDICODI";
        public string Lectodi = "LECTCODI";
        //public string Tipoemprcodi = "TIPOEMPRCODI";

        public string Prnbitvalor = "PRNBITVALOR"; // valor

        //public string Emprnomb = "EMPRNOMB";
        //public string Gruponomb = "GRUPONOMB";
        //public string Ptomedidesc = "PTOMEDIDESC";


        public string SqlGetMaxIdBitacora
        {
            get { return GetSqlXml("GetMaxIdBitacora"); }
        }

        public string SqlSaveBitacora
        {
            get { return GetSqlXml("SaveBitacora"); }
        }

        public string SqlListBitacora
        {
            get { return base.GetSqlXml("ListBitacora"); }
        }
        #endregion
        // ------------------------------------ FIN ASSETEC 14-03-2022 -----------------------------------------------------

        #region Mejoras PRODEM.E3 40 horas
        public string SqlTotalRegConsultaBitacora
        {
            get { return GetSqlXml("TotalRegConsultaBitacora"); }
        }
        #endregion
    }
}
