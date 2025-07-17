/*****************************************************************************************
* Fecha de Creación: 29-05-2014
* Creado por: COES SINAC
* Descripción: Clase que contiene metodos para acceder a los querys o sp
*****************************************************************************************/

using System;
using System.Xml;
using System.Configuration;

namespace COES.Base.Core
{
    public class HelperBase
    {
        private string className;
        private string xmlFile;

        #region Constructores

        public HelperBase(string file)
        {
            this.xmlFile = file;
        }

        public HelperBase()
        {
        }

        #endregion


        /// <summary>
        /// Retorna los nombres de los stores procedures
        /// </summary>
        #region Stores Procedures

        public string SpGetById
        {
            get { return ConstantesBase.ObtenerPorId + className.ToUpper(); }
        }

        public string SpList
        {
            get { return ConstantesBase.Listar + className.ToUpper(); }
        }

        public string SpGetByCriteria
        {
            get { return ConstantesBase.ObtenerPorCriterio + className.ToUpper(); }
        }

        public string SpSave
        {
            get { return ConstantesBase.Insertar + className.ToUpper(); }
        }

        public string SpUpdate
        {
            get { return ConstantesBase.Actualizar + className.ToUpper(); }
        }

        public string SpDelete
        {
            get { return ConstantesBase.Eliminar + className.ToUpper(); }
        }


        #endregion


        /// <summary>
        /// Retorna las sentencias SQL
        /// </summary>
        #region Sentencias SQL

        public string SqlGetById
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorId); }
        }


        public string SqlGetByIncumplimiento
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorIncumplimiento); }
        }

        public string SqlGetByEmpresaGeneradora
        {
            get { return GetSqlXml("GetByEmpresaGeneradora"); }
        }
        
        public string SqlGetRelacionDetallePorVTEA
        {
            get { return GetSqlXml("GetRelacionDetallePorVTEA"); }
        }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public string SqlGetByCodigo
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorCodigo); }
        }

        public string SqlList
        {
            get { return GetSqlXml(ConstantesBase.KeyListar); }
        }

        public string SqlListByEmprCodiAndCatecodi
        {
            get { return GetSqlXml("GetByEmprcodiAndCatecodi"); }
        }

        public string SqlListAnio
        {
            get { return GetSqlXml(ConstantesBase.KeyAnio); }
        }

        public string SqlListMigracion
        {
            get { return GetSqlXml("ListMigracion"); }
        }

        public string SqlListGeneradoras
        {
            get { return GetSqlXml("ListGeneradoras"); }
        }

        public string SqlGetByCriteria
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorCriterio); }
        }

        public string SqlListTipoSerie
        {
            get { return GetSqlXml("SqlListTipoSerie"); }

        }
        public string SqlListTipoPuntoMedicion
        {
            get { return GetSqlXml("SqlListTipoPuntoMedicion"); }

        }
        public string SqlListPuntoMedicionPorEmpresa
        {
            get { return GetSqlXml("SqlListPuntoMedicionPorEmpresa"); }

        }
        public string SqlListPuntoMedicionPorCuenca
        {
            get { return GetSqlXml("SqlListPuntoMedicionPorCuenca"); }

        }
        public string SqlListPuntoMedicionPorCuencaNaturalEvaporado
        {
            get { return GetSqlXml("SqlListPuntoMedicionPorCuencaNaturalEvaporado"); }

        }

        public string SqlListPuntoMedicionPorCuencaNaturalEvaporadoPorTipoPuntoMedicion
        {
            get { return GetSqlXml("SqlListPuntoMedicionPorCuencaNaturalEvaporadoPorTipoPuntoMedicion"); }

        }

        public string SqlObtenerGraficoAnual
        {
            get { return GetSqlXml("SqlObtenerGraficoAnual"); }

        }
        public string SqlObtenerPuntosCalculados
        {
            get { return GetSqlXml("SqlObtenerPuntosCalculados"); }

        }
        public string SqlObtenerGraficoMensual
        {
            get { return GetSqlXml("SqlObtenerGraficoMensual"); }

        }
        public string SqlObtenerGraficoComparativaVolumen
        {
            get { return GetSqlXml("SqlObtenerGraficoComparativaVolumen"); }

        }
        public string SqlObtenerGraficoComparativaNaturalEvaporada
        {
            get { return GetSqlXml("SqlObtenerGraficoComparativaNaturalEvaporada"); }

        }
        public string SqlObtenerGraficoComparativaLineaTendencia
        {
            get { return GetSqlXml("SqlObtenerGraficoComparativaLineaTendencia"); }

        }
        

        public string SqlObtenerGraficoTotal
        {
            get { return GetSqlXml("SqlObtenerGraficoTotal"); }

        }
        public string SqlObtenerGraficoTotalNaturalEvaporada
        {
            get { return GetSqlXml("SqlObtenerGraficoTotalNaturalEvaporada"); }

        }
        public string SqlObtenerGraficoTotalLineaTendencia
        {
            get { return GetSqlXml("SqlObtenerGraficoTotalLineaTendencia"); }

        }
        



        public string SqlInfoPuntoMedicionPorEmpresa
        {
            get { return GetSqlXml("SqlInfoPuntoMedicionPorEmpresa"); }

        }
        public string SqlGetByCriteriaPeriodoAnterior
        {
            get { return GetSqlXml("GetByCriteriaPeriodoAnterior"); }
        }
        public string SqlObtenerPtoMedicionCuenca
        {
            get { return GetSqlXml("SqlObtenerPtoMedicionCuenca"); }
        }

        public string SqlObtenerPtoMedicionCuencaTipoPuntoMedicion
        {
            get { return GetSqlXml("SqlObtenerPtoMedicionCuencaTipoPuntoMedicion"); }
        }
        public string SqlObtenerListaTablaVertical
        {
            get { return GetSqlXml("SqlObtenerListaTablaVertical"); }
        }
        
        public string SqlNroRegistros
        {
            get { return GetSqlXml(ConstantesBase.KeyNroRegistros); }
        }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public string SqlListaPeriodoActivo
        {
            get { return GetSqlXml(ConstantesBase.SqlListaPeriodoActivo); }
        }

        public string SqlSave
        {
            get { return GetSqlXml(ConstantesBase.KeyInsertar); }
        }

        public string SqlSaveLectura
        {
            get { return GetSqlXml("SaveLectura"); }
        }

        public string SqlSaveExterno
        {
            get { return GetSqlXml("SaveExterno"); }
        }

        public string SqlGetEquipoGPS
        {
            get { return GetSqlXml("GetEquipoGPS"); }
        }

        public string SqlUpdate
        {
            get { return GetSqlXml(ConstantesBase.KeyActualizar); }
        }

        public string SqlDelete
        {
            get { return GetSqlXml(ConstantesBase.KeyEliminar); }
        }

        public string SqlDelete_UpdateAuditoria
        {
            get { return GetSqlXml(ConstantesBase.KeyAuditoria); }
        }

        public string SqlUpdateRetirosInactivo
        {
            get { return GetSqlXml("UpdateRetirosInactivo"); }
        }
        
        public string SqlTotalRecords
        {
            get { return GetSqlXml(ConstantesBase.KeyTotalRecords); }
        }

        public string SqlGetMaxId
        {
            get { return GetSqlXml(ConstantesBase.KeyGetMaxId); }
        }

        public string SqlObtenerCanalesTrcoes
        {
            get { return GetSqlXml("ObtenerCanalesTrcoes"); }
        }

        public string SqlEliminadoLogicoDeCanales
        {
            get { return GetSqlXml("EliminadoLogicoDeCanales"); }
        }

        public string SqlCrearCanalConDataDeScada
        {
            get { return GetSqlXml("CrearCanalConDataDeScada"); }
        }

        public string SqlActualizarCanalConDataDeScada
        {
            get { return GetSqlXml("ActualizarCanalConDataDeScada"); }
        }

        public string SqlGetMaxCodigoTrCargaArchXmlSp7
        {
            get { return GetSqlXml("GetMaxCodigoTrCargaArchXmlSp7"); }
        }
        
        public string SqlGenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales
        {
            get { return GetSqlXml("GenerarRegistroTrCargaArchXmlSp7SiHayActualizacionDeCanales"); }
        }
        
        public string SqlUpdateCanalCambioSiHayActualizacionDeCanales
        {
            get { return GetSqlXml("UpdateCanalCambioSiHayActualizacionDeCanales"); }
        }

        public string SqlObtenerEmpresasDesdeTrcoes
        {
            get { return GetSqlXml("ObtenerEmpresasDesdeTrcoes"); }
        }

        public string SqlGenerarEmpresasEnTrcoes
        {
            get { return GetSqlXml("GenerarEmpresasEnTrcoes"); }
        }

        public string SqlActualizarCanalesIccpXml
        {
            get { return GetSqlXml("ActualizarCanalesIccpXml"); }
        }

        public string SqlObtenerTotalZonasPorZonaId
        {
            get { return GetSqlXml("ObtenerTotalZonasPorZonaId"); }
        }

        public string SqlObtenerTotalEmpresaPorEmprcodi
        {
            get { return GetSqlXml("ObtenerTotalEmpresaPorEmprcodi"); }
        }

        public string SqlGenerarRegistroZona
        {
            get { return GetSqlXml("GenerarRegistroZona"); }
        }

        public string SqlActualizarRegistroZona
        {
            get { return GetSqlXml("ActualizarRegistroZona"); }
        }

        public string SqlActualizarCanalSiid
        {
            get { return GetSqlXml("ActualizarCanalSiid"); }
        }

        #endregion

        /// <summary>
        /// Obtiene la sentencia SQL del archivo xml
        /// </summary>
        /// <param name="idMessage"></param>
        /// <returns></returns>
        public string GetSqlXml(string idSql)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xmlFile);

                XmlNodeList nSqls = xDoc.GetElementsByTagName(ConstantesBase.MainNodeSql);
                XmlNodeList nSql = ((XmlElement)nSqls[0]).GetElementsByTagName(ConstantesBase.SubNodeSql);

                foreach (XmlElement nodo in nSql)
                {
                    XmlNodeList nKey = nodo.GetElementsByTagName(ConstantesBase.KeyNode);

                    if (nKey[0].InnerText == idSql)
                    {
                        XmlNodeList nQuery = nodo.GetElementsByTagName(ConstantesBase.QueryNode);
                        return nQuery[0].InnerText;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #region Campos para paginado

        public string PageSize = "PageSize";
        public string PageNumber = "PageNumber";
        public string TotalRecord = "TotalRecord";
        public string Campo = "campo";
        public string Valor = "valor";
        public string Ok = "OK";
        public string CaracterGuion = "-";
        public char Cero = '0';

        #endregion

        #region Mejoras EO-EPO
        public string SqlListVigenciaAnioIngreso
        {
            get { return GetSqlXml(ConstantesBase.KeyListVigenciaAnioIngreso); }
        }
        public string SqlGetByCriteriaEstadosVigencia
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorCriterioEstadosVigencia); }
        }
        public string SqlListVigencia36Meses
        {
            get { return GetSqlXml(ConstantesBase.KeyListVigencia36Meses); }
        }
        public string SqlListVigencia48Meses
        {
            get { return GetSqlXml(ConstantesBase.KeyListVigencia48Meses); }
        }
        public string SqlListVigencia12Meses
        {
            get { return GetSqlXml(ConstantesBase.KeyListVigencia12Meses); }
        }        
        #endregion

        #region Mejoras RDO

        public string SqlSaveHorario
        {
            get { return GetSqlXml(ConstantesBase.KeyInsertarHorario); }
        }

        public string SqlSaveEjecutados
        {
            get { return GetSqlXml(ConstantesBase.KeyInsertarEjecutados); }
        }

        public string GetByCriteriaCaudalVolumen
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorCriterioCaudalVolumen); }
        }

        public string SqlSaveEjecutados48
        {
            get { return GetSqlXml(ConstantesBase.KeyInsertarEjecutados); }
        }
        public string SqlSaveIntranet
        {
            get { return GetSqlXml(ConstantesBase.KeyInsertarIntranet); }
        }
        public string SqlSaveintervaloRDO
        {
            get { return GetSqlXml(ConstantesBase.KeyInsertarIntervaloRDO); }
        }
        public string SqlGetRdoByCriteria
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerRdoPorCriterio); }
        }
        public string SqlGetByCriteriaxHorario
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorCriterioxHorario); }
        }
        #endregion

        #region Mejoras EO-EPO-II
        public string SqlListExcesoAbsObs
        {
            get { return GetSqlXml(ConstantesBase.KeyListExcesoAbsObs); }
        }
        #endregion

        #region Mejoras CTAF
        public string SqlUpdateEventoCtaf
        {
            get { return GetSqlXml(ConstantesBase.KeyUpdateEventoCtaf); }
        }
        public string SqlinsertarEventoEvento
        {
            get { return GetSqlXml(ConstantesBase.KeyinsertarEventoEvento); }
        }
        public string SqlinsertarCriteriosEvento
        {
            get { return GetSqlXml("SqlinsertarCriteriosEvento"); }
        }
        public string SqlListInformesSco
        {
            get { return GetSqlXml(ConstantesBase.KeyListInformesSco); }
        }
        public string SqlActualizarInformePortalWeb
        {
            get { return GetSqlXml(ConstantesBase.KeyActualizarInformePortalWeb); }
        }
        public string SqlObtenerInformeSco
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerInformeSco); }
        }
        public string SqlGetByIdxEmprcodi
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorEmprcodi); }
        }
        public string GetByIdxAfrrec
        {
            get { return GetSqlXml("GetByIdxAfrrec"); }
        }
        public string SqlUpdateHoraCoordSuministradora
        {
            get { return GetSqlXml("UpdateHoraCoordSuministradora"); }
        }
        #endregion

        #region Mejoras RDO-II
        public string SqlGetByCriteriaMeEnviosUltimoEjecutado
        {
            get { return GetSqlXml(ConstantesBase.KeyObtenerPorCriterioxUltimoEjecutado); }
        }
        #endregion

        //GESPROTEC - 20241029
        #region GESPROTEC
        public string SqlListAreaEquipamientoCOES
        {
            get { return GetSqlXml("ListarAreaEquipamientoCOES"); }
        }

        public string SqlListFamiliaEquipamientoCOES
        {
            get { return GetSqlXml("ListFamiliaEquipamientoCOES"); }
        }
        public string SqlListProtecciones
        {
            get { return GetSqlXml("ListProtecciones"); }
        }

        #endregion
    }

}
