using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_PALEATORIA
    /// </summary>
    public class EvePaleatoriaHelper : HelperBase
    {
        public EvePaleatoriaHelper(): base(Consultas.EvePaleatoriaSql)
        {
        }

        public EvePaleatoriaDTO Create(IDataReader dr)
        {
            EvePaleatoriaDTO entity = new EvePaleatoriaDTO();

            int iPafecha = dr.GetOrdinal(this.Pafecha);
            if (!dr.IsDBNull(iPafecha)) entity.Pafecha = dr.GetDateTime(iPafecha);

            int iSic2hop = dr.GetOrdinal(this.Sic2hop);
            if (!dr.IsDBNull(iSic2hop)) entity.Sic2hop = dr.GetString(iSic2hop);

            int iHop2ut30d = dr.GetOrdinal(this.Hop2ut30d);
            if (!dr.IsDBNull(iHop2ut30d)) entity.Hop2ut30d = dr.GetString(iHop2ut30d);

            int iUt30d2sort = dr.GetOrdinal(this.Ut30d2sort);
            if (!dr.IsDBNull(iUt30d2sort)) entity.Ut30d2sort = dr.GetString(iUt30d2sort);

            int iSort2prue = dr.GetOrdinal(this.Sort2prue);
            if (!dr.IsDBNull(iSort2prue)) entity.Sort2prue = dr.GetString(iSort2prue);

            int iPrueno2pa = dr.GetOrdinal(this.Prueno2pa);
            if (!dr.IsDBNull(iPrueno2pa)) entity.Prueno2pa = dr.GetString(iPrueno2pa);

            int iPa2fin = dr.GetOrdinal(this.Pa2fin);
            if (!dr.IsDBNull(iPa2fin)) entity.Pa2fin = dr.GetString(iPa2fin);

            int iPruesi2gprue = dr.GetOrdinal(this.Pruesi2gprue);
            if (!dr.IsDBNull(iPruesi2gprue)) entity.Pruesi2gprue = dr.GetString(iPruesi2gprue);

            int iGprueno2nprue = dr.GetOrdinal(this.Gprueno2nprue);
            if (!dr.IsDBNull(iGprueno2nprue)) entity.Gprueno2nprue = dr.GetString(iGprueno2nprue);

            int iNprue2fin = dr.GetOrdinal(this.Nprue2fin);
            if (!dr.IsDBNull(iNprue2fin)) entity.Nprue2fin = dr.GetString(iNprue2fin);

            int iGpruesi2uprue = dr.GetOrdinal(this.Gpruesi2uprue);
            if (!dr.IsDBNull(iGpruesi2uprue)) entity.Gpruesi2uprue = dr.GetString(iGpruesi2uprue);

            int iUprue2rprue = dr.GetOrdinal(this.Uprue2rprue);
            if (!dr.IsDBNull(iUprue2rprue)) entity.Uprue2rprue = dr.GetString(iUprue2rprue);

            int iRprue2oa = dr.GetOrdinal(this.Rprue2oa);
            if (!dr.IsDBNull(iRprue2oa)) entity.Rprue2oa = dr.GetString(iRprue2oa);

            int iOa2priarr = dr.GetOrdinal(this.Oa2priarr);
            if (!dr.IsDBNull(iOa2priarr)) entity.Oa2priarr = dr.GetString(iOa2priarr);

            int iPriarrsi2exit = dr.GetOrdinal(this.Priarrsi2exit);
            if (!dr.IsDBNull(iPriarrsi2exit)) entity.Priarrsi2exit = dr.GetString(iPriarrsi2exit);

            int iPriarrno2rearr = dr.GetOrdinal(this.Priarrno2rearr);
            if (!dr.IsDBNull(iPriarrno2rearr)) entity.Priarrno2rearr = dr.GetString(iPriarrno2rearr);

            int iRearrno2noexit = dr.GetOrdinal(this.Rearrno2noexit);
            if (!dr.IsDBNull(iRearrno2noexit)) entity.Rearrno2noexit = dr.GetString(iRearrno2noexit);

            int iRearrsi2segarr = dr.GetOrdinal(this.Rearrsi2segarr);
            if (!dr.IsDBNull(iRearrsi2segarr)) entity.Rearrsi2segarr = dr.GetString(iRearrsi2segarr);

            int iSegarrno2noexit = dr.GetOrdinal(this.Segarrno2noexit);
            if (!dr.IsDBNull(iSegarrno2noexit)) entity.Segarrno2noexit = dr.GetString(iSegarrno2noexit);

            int iSegarrsi2exit = dr.GetOrdinal(this.Segarrsi2exit);
            if (!dr.IsDBNull(iSegarrsi2exit)) entity.Segarrsi2exit = dr.GetString(iSegarrsi2exit);

            int iExitno2fallunid = dr.GetOrdinal(this.Exitno2fallunid);
            if (!dr.IsDBNull(iExitno2fallunid)) entity.Exitno2fallunid = dr.GetString(iExitno2fallunid);

            int iFallunidsi2noexit = dr.GetOrdinal(this.Fallunidsi2noexit);
            if (!dr.IsDBNull(iFallunidsi2noexit)) entity.Fallunidsi2noexit = dr.GetString(iFallunidsi2noexit);

            int iExitsi2resprue = dr.GetOrdinal(this.Exitsi2resprue);
            if (!dr.IsDBNull(iExitsi2resprue)) entity.Exitsi2resprue = dr.GetString(iExitsi2resprue);

            int iFallunidno2pabort = dr.GetOrdinal(this.Fallunidno2pabort);
            if (!dr.IsDBNull(iFallunidno2pabort)) entity.Fallunidno2pabort = dr.GetString(iFallunidno2pabort);

            int iPabort2resprue = dr.GetOrdinal(this.Pabort2resprue);
            if (!dr.IsDBNull(iPabort2resprue)) entity.Pabort2resprue = dr.GetString(iPabort2resprue);

            int iResprue2fin = dr.GetOrdinal(this.Resprue2fin);
            if (!dr.IsDBNull(iResprue2fin)) entity.Resprue2fin = dr.GetString(iResprue2fin);

            int iNoexit2resreslt = dr.GetOrdinal(this.Noexit2resreslt);
            if (!dr.IsDBNull(iNoexit2resreslt)) entity.Noexit2resreslt = dr.GetString(iNoexit2resreslt);

            int iResreslt2fin = dr.GetOrdinal(this.Resreslt2fin);
            if (!dr.IsDBNull(iResreslt2fin)) entity.Resreslt2fin = dr.GetString(iResreslt2fin);

            int iFinal = dr.GetOrdinal(this.Final);
            if (!dr.IsDBNull(iFinal)) entity.Final = dr.GetString(iFinal);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iProgramador = dr.GetOrdinal(this.Programador);
            if (!dr.IsDBNull(iProgramador)) entity.Programador = dr.GetString(iProgramador);
                        
            int iPaobservacion = dr.GetOrdinal(this.Paobservacion);
            if (!dr.IsDBNull(iPaobservacion)) entity.Paobservacion = dr.GetString(iPaobservacion);

            int iPaverifdatosing = dr.GetOrdinal(this.Paverifdatosing);
            if (!dr.IsDBNull(iPaverifdatosing)) entity.Paverifdatosing = dr.GetString(iPaverifdatosing);
            

            return entity;
        }


        #region Mapeo de Campos

        public string Pafecha = "PAFECHA";
        public string Sic2hop = "SIC2HOP";
        public string Hop2ut30d = "HOP2UT30D";
        public string Ut30d2sort = "UT30D2SORT";
        public string Sort2prue = "SORT2PRUE";
        public string Prueno2pa = "PRUENO2PA";
        public string Pa2fin = "PA2FIN";
        public string Pruesi2gprue = "PRUESI2GPRUE";
        public string Gprueno2nprue = "GPRUENO2NPRUE";
        public string Nprue2fin = "NPRUE2FIN";
        public string Gpruesi2uprue = "GPRUESI2UPRUE";
        public string Uprue2rprue = "UPRUE2RPRUE";
        public string Rprue2oa = "RPRUE2OA";
        public string Oa2priarr = "OA2PRIARR";
        public string Priarrsi2exit = "PRIARRSI2EXIT";
        public string Priarrno2rearr = "PRIARRNO2REARR";
        public string Rearrno2noexit = "REARRNO2NOEXIT";
        public string Rearrsi2segarr = "REARRSI2SEGARR";
        public string Segarrno2noexit = "SEGARRNO2NOEXIT";
        public string Segarrsi2exit = "SEGARRSI2EXIT";
        public string Exitno2fallunid = "EXITNO2FALLUNID";
        public string Fallunidsi2noexit = "FALLUNIDSI2NOEXIT";
        public string Exitsi2resprue = "EXITSI2RESPRUE";
        public string Fallunidno2pabort = "FALLUNIDNO2PABORT";
        public string Pabort2resprue = "PABORT2RESPRUE";
        public string Resprue2fin = "RESPRUE2FIN";
        public string Noexit2resreslt = "NOEXIT2RESRESLT";
        public string Resreslt2fin = "RESRESLT2FIN";
        public string Final = "FINAL";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Programador = "PROGRAMADOR";
        public string Paobservacion = "PAOBSERVACION";
        public string Paverifdatosing = "PAVERIFDATOSING";
        


        public string Resultado = "RESULTADO";
        public string PruebaExitosa = "PRUEBAEXITOSA";
        public string PrimerIntentoExitoso = "PRIMERINTENTOEXITOSO";
        public string SegundoIntentoExitoso = "SEGUNDOINTENTOEXITOSO";





        #endregion

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }


        public string ProgramadorPrueba
        {
            get { return base.GetSqlXml("ProgramadorPrueba"); }
        }


    }
}
