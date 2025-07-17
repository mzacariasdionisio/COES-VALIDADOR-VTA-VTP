using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnMedicion48Helper : HelperBase
    {
        public PrnMedicion48Helper() : base(Consultas.PrnMedicion48Sql)
        {
        }

        public PrnMedicion48DTO Create(IDataReader dr)
        {
            PrnMedicion48DTO entity = new PrnMedicion48DTO();

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPrnm48tipo = dr.GetOrdinal(this.Prnm48tipo);
            if (!dr.IsDBNull(iPrnm48tipo)) entity.Prnm48tipo = Convert.ToInt32(dr.GetValue(iPrnm48tipo));

            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            int iPrnm48estado = dr.GetOrdinal(this.Prnm48estado);
            if (!dr.IsDBNull(iPrnm48estado)) entity.Prnm48estado = Convert.ToInt32(dr.GetValue(iPrnm48estado));

            int iMeditotal = dr.GetOrdinal(this.Meditotal);
            if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iH25 = dr.GetOrdinal(this.H25);
            if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

            int iH26 = dr.GetOrdinal(this.H26);
            if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

            int iH27 = dr.GetOrdinal(this.H27);
            if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

            int iH28 = dr.GetOrdinal(this.H28);
            if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

            int iH29 = dr.GetOrdinal(this.H29);
            if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

            int iH30 = dr.GetOrdinal(this.H30);
            if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

            int iH31 = dr.GetOrdinal(this.H31);
            if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

            int iH32 = dr.GetOrdinal(this.H32);
            if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

            int iH33 = dr.GetOrdinal(this.H33);
            if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

            int iH34 = dr.GetOrdinal(this.H34);
            if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

            int iH35 = dr.GetOrdinal(this.H35);
            if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

            int iH36 = dr.GetOrdinal(this.H36);
            if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

            int iH37 = dr.GetOrdinal(this.H37);
            if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

            int iH38 = dr.GetOrdinal(this.H38);
            if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

            int iH39 = dr.GetOrdinal(this.H39);
            if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

            int iH40 = dr.GetOrdinal(this.H40);
            if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

            int iH41 = dr.GetOrdinal(this.H41);
            if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

            int iH42 = dr.GetOrdinal(this.H42);
            if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

            int iH43 = dr.GetOrdinal(this.H43);
            if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

            int iH44 = dr.GetOrdinal(this.H44);
            if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

            int iH45 = dr.GetOrdinal(this.H45);
            if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

            int iH46 = dr.GetOrdinal(this.H46);
            if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

            int iH47 = dr.GetOrdinal(this.H47);
            if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

            int iH48 = dr.GetOrdinal(this.H48);
            if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

            int iPrnm48usucreacion = dr.GetOrdinal(this.Prnm48usucreacion);
            if (!dr.IsDBNull(iPrnm48usucreacion)) entity.Prnm48usucreacion = dr.GetString(iPrnm48usucreacion);

            int iPrnm48feccreacion = dr.GetOrdinal(this.Prnm48feccreacion);
            if (!dr.IsDBNull(iPrnm48feccreacion)) entity.Prnm48feccreacion = dr.GetDateTime(iPrnm48feccreacion);

            int iPrnm48usumodificacion = dr.GetOrdinal(this.Prnm48usumodificacion);
            if (!dr.IsDBNull(iPrnm48usumodificacion)) entity.Prnm48usumodificacion = dr.GetString(iPrnm48usumodificacion);

            int iPrnm48fecmodificacion = dr.GetOrdinal(this.Prnm48fecmodificacion);
            if (!dr.IsDBNull(iPrnm48fecmodificacion)) entity.Prnm48fecmodificacion = dr.GetDateTime(iPrnm48fecmodificacion);

            return entity;
        }

        #region Mapeo de los campos
        public string Ptomedicodi = "PTOMEDICODI";
        public string Prnm48tipo = "PRNM48TIPO";
        public string Medifecha = "MEDIFECHA";
        public string Prnm48estado = "PRNM48ESTADO";
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
        public string Prnm48usucreacion = "PRNM48USUCREACION";
        public string Prnm48feccreacion = "PRNM48FECCREACION";
        public string Prnm48usumodificacion = "PRNM48USUMODIFICACION";
        public string Prnm48fecmodificacion = "PRNM48FECMODIFICACION";

        //Adicionales
        public string Areacodi = "AREACODI";
        public string Areapadre = "AREAPADRE";

        public string Lectcodi = "LECTCODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Tipoemprcodi = "TIPOEMPRCODI";

        public string Meditotal = "MEDITOTAL";
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

        public string Ptomedidesc = "PTOMEDIDESC";
        public string Equicodi = "EQUICODI";

        public string Emprcodi = "EMPRCODI";
        public string AreaOperativa = "AREAOPERATIVA";
        public string Tareaabrev = "TAREAABREV";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Grupocodibarra = "GRUPOCODIBARRA";

        //Parametros Barras
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Grupoabrev = "GRUPOABREV";

        //Agregado para Reportes
        public string Equinomb = "EQUINOMB";
        public string Equitension = "EQUITENSION";
        public string Areanomb = "AREANOMB";
        public string Emprnomb = "EMPRNOMB";

        public string Prnredbarracp = "PRNREDBARRACP";
        public string Prnredbarrapm = "PRNREDBARRAPM";

        //Para el BulkInsert
        public string TableName = "PRN_MEDICION48";

        

        #endregion

        #region Consultas a la BD

        public string SqlListById
        {
            get { return base.GetSqlXml("ListById"); }
        }

        public string SqlListByIdEnvio
        {
            get { return base.GetSqlXml("ListByIdEnvio"); }
        }

        //Old - Inicio

        public string SqlListMeMed48Historicos
        {
            get { return base.GetSqlXml("ListMeMed48Historicos"); }
        }

        public string SqlListPrnMed48Historicos
        {
            get { return base.GetSqlXml("ListPrnMed48Historicos"); }
        }

        public string SqlListPrnMed48HistoricosNoConfig
        {
            get { return base.GetSqlXml("ListPrnMed48HistoricosNoConfig"); }
        }

        public string SqlGetDespachoEjecutadoByArea
        {
            get { return base.GetSqlXml("GetDespachoEjecutadoByArea"); }
        }

        public string SqlListDespachoAreaULByFecha
        {
            get { return base.GetSqlXml("ListDespachoAreaULByFecha"); }
        }

        public string SqlGetMeMedicionesULByArea
        {
            get { return base.GetSqlXml("GetMeMedicionesULByArea"); }
        }

        //Old -Fin

        //Obtención de datos para la formación del perfíl patrón segun proceso - INICIO

        public string SqlDataPatronDemandaPorPunto
        {
            get { return base.GetSqlXml("DataPatronDemandaPorPunto"); }
        }
        public string SqlDataPatronDemandaPorPuntoPorFecha
        {
            get { return base.GetSqlXml("DataPatronDemandaPorPuntoPorFecha"); }
        }

        public string SqlDataPatronDemandaPorAgrupacion
        {
            get { return base.GetSqlXml("DataPatronDemandaPorAgrupacion"); }
        }

        public string SqlDataPatronDemandaPorArea
        {
            get { return base.GetSqlXml("DataPatronDemandaPorArea"); }
        }

        //Obtención de datos para la formación del perfíl patrón segun proceso - FIN

        public string SqlDataHistoricaBarraPMDistr
        {
            get { return base.GetSqlXml("DataHistoricaBarraPMDistr"); }
        }

        public string SqlDataHistoricaBarraPMUlibre
        {
            get { return base.GetSqlXml("DataHistoricaBarraPMUlibre"); }
        }

        public string SqlDataBarraPMUlibrePorDia
        {
            get { return base.GetSqlXml("DataBarraPMUlibrePorDia"); }
        }

        public string SqlDataPtrBarraPMUlibrePorDia
        {
            get { return base.GetSqlXml("DataPtrBarraPMUlibrePorDia"); }
        }

        public string SqlGetNombrePtomedicion
        {
            get { return base.GetSqlXml("GetNombrePtomedicion"); }
        }

        public string SqlGetDemandaPorPunto
        {
            get { return base.GetSqlXml("GetDemandaPorPunto"); }
        }

        public string SqlGetDemandaPorAgrupacion
        {
            get { return base.GetSqlXml("GetDemandaPorAgrupacion"); }
        }

        public string SqlGetDemandaEjecutadaPorPunto
        {
            get { return base.GetSqlXml("GetDemandaEjecutadaPorPunto"); }
        }

        public string SqlGetDemandaEjecutadaPorAgrupacion
        {
            get { return base.GetSqlXml("GetDemandaEjecutadaPorAgrupacion"); }
        }

        public string SqlGetDemandaPrevistaPorPunto
        {
            get { return base.GetSqlXml("GetDemandaPrevistaPorPunto"); }
        }

        public string SqlGetDemandaPrevistaPorAgrupacion
        {
            get { return base.GetSqlXml("GetDemandaPrevistaPorAgrupacion"); }
        }

        public string SqlGetDespachoEjecutadoPorArea
        {
            get { return base.GetSqlXml("GetDespachoEjecutadoPorArea"); }
        }

        public string SqlGetAjusteAlCentroPorTipo
        {
            get { return base.GetSqlXml("GetAjusteAlCentroPorTipo"); }
        }

        public string SqlGetDemandaULibresPorArea
        {
            get { return base.GetSqlXml("GetDemandaULibresPorArea"); }
        }

        //agregado para reprote
        public string SqlObtenerConsultaPronostico48
        {
            get { return base.GetSqlXml("ObtenerConsultaPronostico48"); }
        }

        public string SqlDeleteRangoPrnMedicion48
        {
            get { return base.GetSqlXml("DeleteRangoPrnMedicion48"); }
        }

        //Parametros - Barras 
        public string SqlGetBarraBy
        {
            get { return base.GetSqlXml("GetListBarraBy"); }
        }

        public string SqlDeleteSA
        {
            get { return base.GetSqlXml("GetSqlDeleteSA"); }
        }

        //Filtros PR03
        public string SqlPR03Puntos
        {
            get { return base.GetSqlXml("PR03Puntos"); }
        }

        public string SqlPR03PuntosNoAgrupados
        {
            get { return base.GetSqlXml("PR03PuntosNoAgrupados"); }
        }

        public string SqlPR03Empresas
        {
            get { return base.GetSqlXml("PR03Empresas"); }
        }

        public string SqlPR03Ubicaciones
        {
            get { return base.GetSqlXml("PR03Ubicaciones"); }
        }

        public string SqlPR03PuntosPorBarrasCP
        {
            get { return base.GetSqlXml("PR03PuntosPorBarrasCP"); }
        }

        public string SqlPR03PuntosPorBarrasCPPP
        {
            get { return base.GetSqlXml("PR03PuntosPorBarrasCPPronosticoDemanda"); }
        }

        public string SqlGetDemandaPorTipoPorRango
        {
            get { return base.GetSqlXml("GetDemandaPorTipoPorRango"); }
        }

        public string SqlValidarEjecucionPronosticoPorAreas
        {
            get { return base.GetSqlXml("ValidarEjecucionPronosticoPorAreas"); }
        }

        #endregion

    }
}
