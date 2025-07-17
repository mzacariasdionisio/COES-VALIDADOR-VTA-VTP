using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_PERIODO_SICLI
    /// </summary>
    public class IioPeriodoSicliHelper : HelperBase
    {

        private const string KeyListarAnios = "ListarAnios";

        public IioPeriodoSicliHelper() : base(Consultas.IioPeriodoSicliSql)
        {
            
        }

        public IioPeriodoSicliDTO Create(IDataReader dr)
        {
            int iPsicliCodi = dr.GetOrdinal(PsicliCodi);
            int iPsicliFecUltActCoes = dr.GetOrdinal(PsicliFecUltActCoes);
            int iPsicliFecUltActOsi = dr.GetOrdinal(PsicliFecUltActOsi);
            int iPsicliEstado = dr.GetOrdinal(PsicliEstado);
            int iPsicliEstRegistro = dr.GetOrdinal(PsicliEstRegistro);
            int iPsicliUsuCreacion = dr.GetOrdinal(PsicliUsuCreacion);
            int iPsicliFecCreacion = dr.GetOrdinal(PsicliFecCreacion);
            int iPsicliUsuModificacion = dr.GetOrdinal(PsicliUsuModificacion);
            int iPsicliFecModificacion = dr.GetOrdinal(PsicliFecModificacion);
            int iPsicliAnioMesPerrem = dr.GetOrdinal(PsicliAnioMesPerrem);

            //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
            int iPsicliCerrado = dr.GetOrdinal(PsicliCerrado);
            int iPsicliCerradoDemanda = dr.GetOrdinal(PsicliCerradoDemanda);

            return  new IioPeriodoSicliDTO
            {
                PsicliCodi = (!dr.IsDBNull(iPsicliCodi) ? dr.GetInt32(iPsicliCodi) : default(int)),
                PsicliFecUltActCoes = (!dr.IsDBNull(iPsicliFecUltActCoes) ? dr.GetDateTime(iPsicliFecUltActCoes) : new DateTime()),
                PsicliFecUltActOsi = (!dr.IsDBNull(iPsicliFecUltActOsi) ? dr.GetDateTime(iPsicliFecUltActOsi) : new DateTime()),
                PsicliEstado = (!dr.IsDBNull(iPsicliEstado) ? dr.GetString(iPsicliEstado) : null),
                PsicliEstRegistro = (!dr.IsDBNull(iPsicliEstRegistro) ? dr.GetString(iPsicliEstRegistro) : null),
                PsicliUsuCreacion = (!dr.IsDBNull(iPsicliUsuCreacion) ? dr.GetString(iPsicliUsuCreacion) : null),
                PsicliFecCreacion = (!dr.IsDBNull(iPsicliFecCreacion) ? dr.GetDateTime(iPsicliFecCreacion) : new DateTime()),
                PsicliUsuModificacion = (!dr.IsDBNull(iPsicliUsuModificacion) ? dr.GetString(iPsicliUsuModificacion) : null),
                PsicliFecModificacion = (!dr.IsDBNull(iPsicliFecModificacion) ? dr.GetDateTime(iPsicliFecModificacion) : new DateTime()),                               
                PsicliAnioMesPerrem = (!dr.IsDBNull(iPsicliAnioMesPerrem) ? dr.GetString(iPsicliAnioMesPerrem) : null),
                //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
                PSicliCerrado = (!dr.IsDBNull(iPsicliCerrado) ? dr.GetString(iPsicliCerrado) : null),
                PSicliCerradoDemanda = (!dr.IsDBNull(iPsicliCerradoDemanda) ? dr.GetString(iPsicliCerradoDemanda) : null)
                //- HDT Fin
            };//psiclicodi, psiclifecultactcoes, psiclifecultactosi, psicliestado, psicliestregistro, psicliusucreacion, psiclifeccreacion, psicliusumodificacion, psiclifecmodificacion, psiclianiomesperrem
        }

        #region Mapeo de Campos

        public static string TableName = "IIO_PERIODO_SEIN";
        public static string PsicliCodi = "PSICLICODI";
        public static string PsicliAnioMesPerrem = "PSICLIANIOMESPERREM";
        public static string PsicliFecUltActCoes = "PSICLIFECULTACTCOES";
        public static string PsicliFecUltActOsi = "PSICLIFECULTACTOSI";
        public static string PsicliEstado = "PSICLIESTADO";
        public static string PsicliEstRegistro = "PSICLIESTREGISTRO";
        public static string PsicliUsuCreacion = "PSICLIUSUCREACION";
        public static string PsicliFecCreacion = "PSICLIFECCREACION";
        public static string PsicliUsuModificacion = "PSICLIUSUMODIFICACION";
        public static string PsicliFecModificacion = "PSICLIFECMODIFICACION";
        
        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public static string PsicliCerrado = "PSICLICERRADO";

        public static string PsicliCerradoDemanda = "PSICLICERRADODEMANDA";

        public static string Anio = "ANIO";

        public static string PSicliFecSincronizacion = "PSICLIFECSINCRONIZACION";
        public static string TablasEmpresasProcesar = "TABLASEMPRESASPROCESAR";

        #endregion

        #region Custom SQL

        public string SqlListAnios
        {
            get { return GetSqlXml(KeyListarAnios); }
        }

        public string SqlGetByPeriodo
        {
            get { return GetSqlXml("GetByPeriodo"); }
        }
        #endregion
    }
}