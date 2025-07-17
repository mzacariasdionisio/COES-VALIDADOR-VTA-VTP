using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_FORMATO
    /// </summary>
    public class RdoHelper : HelperBase
    {
        public RdoHelper()
            : base(Consultas.MeFormatoSql)
        {
        }

        public MeFormatoDTO Create(IDataReader dr)
        {
            MeFormatoDTO entity = new MeFormatoDTO();

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            int iFormatresolucion = dr.GetOrdinal(this.Formatresolucion);
            if (!dr.IsDBNull(iFormatresolucion)) entity.Formatresolucion = Convert.ToInt32(dr.GetValue(iFormatresolucion));

            int iFormatperiodo = dr.GetOrdinal(this.Formatperiodo);
            if (!dr.IsDBNull(iFormatperiodo)) entity.Formatperiodo = Convert.ToInt32(dr.GetValue(iFormatperiodo));

            int iFormatnombre = dr.GetOrdinal(this.Formatnombre);
            if (!dr.IsDBNull(iFormatnombre)) entity.Formatnombre = dr.GetString(iFormatnombre);

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iFormathorizonte = dr.GetOrdinal(this.Formathorizonte);
            if (!dr.IsDBNull(iFormathorizonte)) entity.Formathorizonte = Convert.ToInt32(dr.GetValue(iFormathorizonte));

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iFormatdiaplazo = dr.GetOrdinal(this.Formatdiaplazo);
            if (!dr.IsDBNull(iFormatdiaplazo)) entity.Formatdiaplazo = Convert.ToInt32(dr.GetValue(iFormatdiaplazo));

            int iFormatminplazo = dr.GetOrdinal(this.Formatminplazo);
            if (!dr.IsDBNull(iFormatminplazo)) entity.Formatminplazo = Convert.ToInt32(dr.GetValue(iFormatminplazo));

            int iFormatdescrip = dr.GetOrdinal(this.Formatdescrip);
            if (!dr.IsDBNull(iFormatdescrip)) entity.Formatdescrip = dr.GetString(iFormatdescrip);

            int iFormatcheckplazo = dr.GetOrdinal(this.Formatcheckplazo);
            if (!dr.IsDBNull(iFormatcheckplazo)) entity.Formatcheckplazo = Convert.ToInt32(dr.GetValue(iFormatcheckplazo));

            int iFormatcheckblanco = dr.GetOrdinal(this.Formatcheckblanco);
            if (!dr.IsDBNull(iFormatcheckblanco)) entity.Formatcheckblanco = Convert.ToInt32(dr.GetValue(iFormatcheckblanco));

            int iFormatallempresa = dr.GetOrdinal(this.Formatallempresa);
            if (!dr.IsDBNull(iFormatallempresa)) entity.Formatallempresa = Convert.ToInt32(dr.GetValue(iFormatallempresa));

            int iCabcodi = dr.GetOrdinal(this.Cabcodi);
            if (!dr.IsDBNull(iCabcodi)) entity.Cabcodi = Convert.ToInt32(dr.GetValue(iCabcodi));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iFormatsecundario = dr.GetOrdinal(this.Formatsecundario);
            if (!dr.IsDBNull(iFormatsecundario)) entity.Formatsecundario = Convert.ToInt32(dr.GetValue(iFormatsecundario));

            int iFormatdiafinplazo = dr.GetOrdinal(this.Formatdiafinplazo);
            if (!dr.IsDBNull(iFormatdiafinplazo)) entity.Formatdiafinplazo = Convert.ToInt32(dr.GetValue(iFormatdiafinplazo));

            int iFormatminfinplazo = dr.GetOrdinal(this.Formatminfinplazo);
            if (!dr.IsDBNull(iFormatminfinplazo)) entity.Formatminfinplazo = Convert.ToInt32(dr.GetValue(iFormatminfinplazo));

            int iFormatnumbloques = dr.GetOrdinal(this.Formatnumbloques);
            if (!dr.IsDBNull(iFormatnumbloques)) entity.Formatnumbloques = Convert.ToInt32(dr.GetValue(iFormatnumbloques));

            int iFormatdiafinfueraplazo = dr.GetOrdinal(this.Formatdiafinfueraplazo);
            if (!dr.IsDBNull(iFormatdiafinfueraplazo)) entity.Formatdiafinfueraplazo = Convert.ToInt32(dr.GetValue(iFormatdiafinfueraplazo));

            int iFormatminfinfueraplazo = dr.GetOrdinal(this.Formatminfinfueraplazo);
            if (!dr.IsDBNull(iFormatminfinfueraplazo)) entity.Formatminfinfueraplazo = Convert.ToInt32(dr.GetValue(iFormatminfinfueraplazo));

            int iFormatmesplazo = dr.GetOrdinal(this.Formatmesplazo);
            if (!dr.IsDBNull(iFormatmesplazo)) entity.Formatmesplazo = Convert.ToInt32(dr.GetValue(iFormatmesplazo));

            int iFormatmesfinplazo = dr.GetOrdinal(this.Formatmesfinplazo);
            if (!dr.IsDBNull(iFormatmesfinplazo)) entity.Formatmesfinplazo = Convert.ToInt32(dr.GetValue(iFormatmesfinplazo));

            int iFormatmesfinfueraplazo = dr.GetOrdinal(this.Formatmesfinfueraplazo);
            if (!dr.IsDBNull(iFormatmesfinfueraplazo)) entity.Formatmesfinfueraplazo = Convert.ToInt32(dr.GetValue(iFormatmesfinfueraplazo));

            int icheckplazopunto = dr.GetOrdinal(this.Formatcheckplazopunto);
            if (!dr.IsDBNull(icheckplazopunto)) entity.Formatcheckplazopunto = Convert.ToInt32(dr.GetValue(icheckplazopunto));

            int iFormatdependeconfigptos = dr.GetOrdinal(this.Formatdependeconfigptos);
            if (!dr.IsDBNull(iFormatdependeconfigptos)) entity.Formatdependeconfigptos = Convert.ToInt32(dr.GetValue(iFormatdependeconfigptos));

            switch (entity.Formatperiodo)
            {
                case 1:
                    entity.Periodo = "Diario";
                    break;
                case 2:
                    entity.Periodo = "Semanal";
                    break;
                case 3:
                    entity.Periodo = "Mensual";
                    break;
                case 4:
                    entity.Periodo = "Anual";
                    break;
                case 5:
                    entity.Periodo = "Mensual x Semana";
                    break;
                case 6:
                    entity.Periodo = "Periodo PMPO";
                    break;
                default:
                    entity.Periodo = "No Definido";
                    break;
            }
            switch (entity.Formatresolucion)
            {
                case 15:
                    entity.Resolucion = "15 Minutos";
                    break;
                case 30:
                    entity.Resolucion = "30 Minutos";
                    break;
                case 60:
                    entity.Resolucion = "1 Hora";
                    break;
                case 1440:
                    entity.Resolucion = "1 Día";
                    break;
                case 10080:
                    entity.Resolucion = "1 Semana";
                    break;
                case 43200:
                    entity.Resolucion = "1 Mes";
                    break;

                default:
                    entity.Resolucion = "No Definido";
                    break;
            }
            switch (entity.Formathorizonte)
            {
                case 1:
                    entity.Horizonte = "1 Día";
                    break;
                case 3:
                    entity.Horizonte = "3 Días";
                    break;
                case 7:
                    entity.Horizonte = "7 Días";
                    break;
                case 10:
                    entity.Horizonte = "10 Días";
                    break;
                case 30:
                    entity.Horizonte = "1 Mes";
                    break;
                case 90:
                    entity.Horizonte = "3 Mes";
                    break;
                case 365:
                    entity.Horizonte = "1 Año";
                    break;
                default:
                    entity.Horizonte = "No Definido";
                    break;
            }
            return entity;
        }


        #region Mapeo de Campos

        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Areacode = "AREACODE";
        public string Formatresolucion = "FORMATRESOLUCION";
        public string Formatperiodo = "FORMATPERIODO";
        public string Formatnombre = "FORMATNOMBRE";
        public string Formatcodi = "FORMATCODI";
        public string Formathorizonte = "FORMATHORIZONTE";
        public string Modcodi = "MODCODI";
        public string Formatdiaplazo = "FORMATDIAPLAZO";
        public string Formatminplazo = "FORMATMINPLAZO";
        public string Formatdescrip = "FORMATDESCRIP";
        public string Formatcheckplazo = "FORMATCHECKPLAZO";
        public string Formatcheckblanco = "FORMATCHECKBLANCO";
        public string Formatallempresa = "FORMATALLEMPRESA";
        public string Cabcodi = "CABCODI";
        public string Lectcodi = "LECTCODI";
        public string Formatsecundario = "FORMATSECUNDARIO";
        public string Areaname = "Areaname";
        public string Lecttipo = "Lecttipo";
        public string Formatdiafinplazo = "FORMATDIAFINPLAZO"; 
        public string Formatminfinplazo = "FORMATMINFINPLAZO"; 
        public string Formatnumbloques = "FORMATNUMBLOQUES";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Formatdiafinfueraplazo = "FORMATDIAFINFUERAPLAZO";
        public string Formatminfinfueraplazo = "FORMATMINFINFUERAPLAZO";
        public string Formatmesplazo = "FORMATMESPLAZO";
        public string Formatmesfinplazo = "FORMATMESFINPLAZO";
        public string Formatmesfinfueraplazo = "FORMATMESFINFUERAPLAZO";
        public string Formatcheckplazopunto = "FORMATCHECKPLAZOPUNTO";
        public string Formatdependeconfigptos = "FORMATDEPENDECONFIGPTOS";
        public string FormatnombreOrigen = "FORMATNOMBREORIGEN";

        #endregion

        public string SqlGetByModuloLectura
        {
            get { return base.GetSqlXml("GetByModuloLectura"); }
        }

        public string SqlGetByModuloLecturaMultiple
        {
            get { return base.GetSqlXml("GetByModuloLecturaMultiple"); }
        }

        public string SqlGetByModulo
        {
            get { return base.GetSqlXml("GetByModulo"); }
        }

        public string SqlListCabecera
        {
            get { return base.GetSqlXml("ListCabecera"); }
        }
        
        //- Agregados para PMPO

        public string SqlObtenerListaTipoInformacion
        {
            get { return base.GetSqlXml("ObtenerListaTipoInformacion"); }
        }

        public string SqlListFormatosPmpo
        {
            get { return base.GetSqlXml("ListFormatosPmpo"); }
        }

        public string SqlGetByMesElaboracion
        {
            get { return base.GetSqlXml("GetByMesElaboracion"); }
        }

        public string SqlGetListaEmpresasPMPO
        {
            get { return base.GetSqlXml("GetListaEmpresasPMPO"); }
        }

        public string SqlGetListaEmpresasPendientePMPO
        {
            get { return base.GetSqlXml("GetListaEmpresasPendientePMPO"); }
        }

        public string SqlObtenerPorClave
        {
            get { return base.GetSqlXml("ObtenerPorClave"); }
        }

        public string SqlGetPendientes
        {
            get { return base.GetSqlXml("GetPendientes"); }
        }

        public string SqlListarFormatoOrigen
        {
            get { return base.GetSqlXml("ListarFormatoOrigen"); }
        }
    }
}
