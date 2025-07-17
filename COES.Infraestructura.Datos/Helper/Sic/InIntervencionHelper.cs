using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;
using System.Text;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_INTERVENCION
    /// </summary>
    public class InIntervencionHelper : HelperBase
    {
        public InIntervencionHelper() : base(Consultas.InIntervencionSql)
        {

        }

        #region Helpers

        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de registro de datos
        /// </summary>
        public InIntervencionDTO Create(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iIntercodi = dr.GetOrdinal(this.Intercodi); //1
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iProgrcodi = dr.GetOrdinal(this.Progrcodi);
            if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iIntermensaje = dr.GetOrdinal(this.Intermensaje);
            if (!dr.IsDBNull(iIntermensaje)) entity.Intermensaje = dr.GetString(iIntermensaje);

            int iIntermensajecoes = dr.GetOrdinal(this.Intermensajecoes);
            if (!dr.IsDBNull(iIntermensajecoes)) entity.Intermensajecoes = Convert.ToInt32(dr.GetValue(iIntermensajecoes));

            int iIntermensajeagente = dr.GetOrdinal(this.Intermensajeagente);
            if (!dr.IsDBNull(iIntermensajeagente)) entity.Intermensajeagente = Convert.ToInt32(dr.GetValue(iIntermensajeagente));

            int iInterfechapreini = dr.GetOrdinal(this.Interfechapreini);
            if (!dr.IsDBNull(iInterfechapreini)) entity.Interfechapreini = dr.GetDateTime(iInterfechapreini);

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechaprefin = dr.GetOrdinal(this.Interfechaprefin);  //10
            if (!dr.IsDBNull(iInterfechaprefin)) entity.Interfechaprefin = dr.GetDateTime(iInterfechaprefin);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iClaprocodi = dr.GetOrdinal(this.Claprocodi);
            if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

            int iIntercodsegempr = dr.GetOrdinal(this.Intercodsegempr);
            if (!dr.IsDBNull(iIntercodsegempr)) entity.Intercodsegempr = dr.GetString(iIntercodsegempr);
            entity.Intercodsegempr = entity.Intercodsegempr ?? "";

            int iInterrepetir = dr.GetOrdinal(this.Interrepetir);
            if (!dr.IsDBNull(iInterrepetir)) entity.Interrepetir = dr.GetString(iInterrepetir);

            int iIntermwindispo = dr.GetOrdinal(this.Intermwindispo);
            if (!dr.IsDBNull(iIntermwindispo)) entity.Intermwindispo = Convert.ToDecimal(dr.GetValue(iIntermwindispo));

            int iInterindispo = dr.GetOrdinal(this.Interindispo);
            if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

            int iInterinterrup = dr.GetOrdinal(this.Interinterrup);
            if (!dr.IsDBNull(iInterinterrup)) entity.Interinterrup = dr.GetString(iInterinterrup);

            int iIntermantrelev = dr.GetOrdinal(this.Intermantrelev);
            if (!dr.IsDBNull(iIntermantrelev)) entity.Intermantrelev = Convert.ToInt32(dr.GetValue(iIntermantrelev));

            int iInterconexionprov = dr.GetOrdinal(this.Interconexionprov);
            if (!dr.IsDBNull(iInterconexionprov)) entity.Interconexionprov = dr.GetString(iInterconexionprov);

            int iIntersistemaaislado = dr.GetOrdinal(this.Intersistemaaislado); //20
            if (!dr.IsDBNull(iIntersistemaaislado)) entity.Intersistemaaislado = dr.GetString(iIntersistemaaislado);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iInterjustifaprobrechaz = dr.GetOrdinal(this.Interjustifaprobrechaz);
            if (!dr.IsDBNull(iInterjustifaprobrechaz)) entity.Interjustifaprobrechaz = dr.GetString(iInterjustifaprobrechaz);

            int iInterfecaprobrechaz = dr.GetOrdinal(this.Interfecaprobrechaz);
            if (!dr.IsDBNull(iInterfecaprobrechaz)) entity.Interfecaprobrechaz = dr.GetDateTime(iInterfecaprobrechaz);

            int iInterprocesado = dr.GetOrdinal(this.Interprocesado);
            if (!dr.IsDBNull(iInterprocesado)) entity.Interprocesado = Convert.ToInt32(dr.GetValue(iInterprocesado));

            int iInterisfiles = dr.GetOrdinal(this.Interisfiles);
            if (!dr.IsDBNull(iInterisfiles)) entity.Interisfiles = dr.GetString(iInterisfiles);

            int iIntermanttocodi = dr.GetOrdinal(this.Intermanttocodi);
            if (!dr.IsDBNull(iIntermanttocodi)) entity.Intermanttocodi = Convert.ToInt32(dr.GetValue(iIntermanttocodi));

            int iInterevenpadre = dr.GetOrdinal(this.Interevenpadre);
            if (!dr.IsDBNull(iInterevenpadre)) entity.Interevenpadre = Convert.ToInt32(dr.GetValue(iInterevenpadre));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iIntercodipadre = dr.GetOrdinal(this.Intercodipadre); //30
            if (!dr.IsDBNull(iIntercodipadre)) entity.Intercodipadre = Convert.ToInt32(dr.GetValue(iIntercodipadre));

            int iInterregprevactivo = dr.GetOrdinal(this.Interregprevactivo);
            if (!dr.IsDBNull(iInterregprevactivo)) entity.Interregprevactivo = dr.GetString(iInterregprevactivo);

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            int iIntercreated = dr.GetOrdinal(this.Intercreated);
            if (!dr.IsDBNull(iIntercreated)) entity.Intercreated = Convert.ToInt32(dr.GetValue(iIntercreated));

            int iInterdeleted = dr.GetOrdinal(this.Interdeleted);
            if (!dr.IsDBNull(iInterdeleted)) entity.Interdeleted = Convert.ToInt32(dr.GetValue(iInterdeleted));

            int iInterusucreacion = dr.GetOrdinal(this.Interusucreacion);
            if (!dr.IsDBNull(iInterusucreacion)) entity.Interusucreacion = dr.GetString(iInterusucreacion);

            int iInterfeccreacion = dr.GetOrdinal(this.Interfeccreacion);
            if (!dr.IsDBNull(iInterfeccreacion)) entity.Interfeccreacion = dr.GetDateTime(iInterfeccreacion);

            int iInterusumodificacion = dr.GetOrdinal(this.Interusumodificacion);
            if (!dr.IsDBNull(iInterusumodificacion)) entity.Interusumodificacion = dr.GetString(iInterusumodificacion);

            int iInterfecmodificacion = dr.GetOrdinal(this.Interfecmodificacion);
            if (!dr.IsDBNull(iInterfecmodificacion)) entity.Interfecmodificacion = dr.GetDateTime(iInterfecmodificacion);

            int iInterfuentestado = dr.GetOrdinal(this.Interfuentestado);
            if (!dr.IsDBNull(iInterfuentestado)) entity.Interfuentestado = Convert.ToInt32(dr.GetValue(iInterfuentestado));

            int iOperadoremprcodi = dr.GetOrdinal(this.Operadoremprcodi); //40
            if (!dr.IsDBNull(iOperadoremprcodi)) entity.Operadoremprcodi = Convert.ToInt32(dr.GetValue(iOperadoremprcodi));

            int iIntertipoindisp = dr.GetOrdinal(this.Intertipoindisp);
            if (!dr.IsDBNull(iIntertipoindisp)) entity.Intertipoindisp = dr.GetString(iIntertipoindisp);

            int iInterpr = dr.GetOrdinal(this.Interpr);
            if (!dr.IsDBNull(iInterpr)) entity.Interpr = Convert.ToDecimal(dr.GetValue(iInterpr));

            int iInterasocproc = dr.GetOrdinal(this.Interasocproc);
            if (!dr.IsDBNull(iInterasocproc)) entity.Interasocproc = dr.GetString(iInterasocproc);

            int iIntercarpetafiles = dr.GetOrdinal(this.Intercarpetafiles);
            if (!dr.IsDBNull(iIntercarpetafiles)) entity.Intercarpetafiles = Convert.ToInt32(dr.GetValue(iIntercarpetafiles));

            int iInteripagente = dr.GetOrdinal(this.Interipagente);
            if (!dr.IsDBNull(iInteripagente)) entity.Interipagente = dr.GetString(iInteripagente);

            int iInterusuagrup = dr.GetOrdinal(this.Interusuagrup);
            if (!dr.IsDBNull(iInterusuagrup)) entity.Interusuagrup = dr.GetString(iInterusuagrup);

            int iInterfecagrup = dr.GetOrdinal(this.Interfecagrup);
            if (!dr.IsDBNull(iInterfecagrup)) entity.Interfecagrup = dr.GetDateTime(iInterfecagrup);

            int iInternota = dr.GetOrdinal(this.Internota);
            if (!dr.IsDBNull(iInternota)) entity.Internota = dr.GetString(iInternota);

            int iInterflagsustento = dr.GetOrdinal(this.Interflagsustento);
            if (!dr.IsDBNull(iInterflagsustento)) entity.Interflagsustento = Convert.ToInt32(dr.GetValue(iInterflagsustento));

            return entity;
        }

        #region Helper Query Editar
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de edición de datos
        /// </summary>
        public InIntervencionDTO CreateQueryEditar(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iInterdeleted = dr.GetOrdinal(this.Interdeleted);
            if (!dr.IsDBNull(iInterdeleted)) entity.Interdeleted = Convert.ToInt32(dr.GetValue(iInterdeleted));

            int iIntercreated = dr.GetOrdinal(this.Intercreated);
            if (!dr.IsDBNull(iIntercreated)) entity.Intercreated = Convert.ToInt32(dr.GetValue(iIntercreated));

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iProgrcodi = dr.GetOrdinal(this.Progrcodi);
            if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iIntermensaje = dr.GetOrdinal(this.Intermensaje);
            if (!dr.IsDBNull(iIntermensaje)) entity.Intermensaje = dr.GetString(iIntermensaje);

            int iIntermensajecoes = dr.GetOrdinal(this.Intermensajecoes);
            if (!dr.IsDBNull(iIntermensajecoes)) entity.Intermensajecoes = Convert.ToInt32(dr.GetValue(iIntermensajecoes));

            int iIntermensajeagente = dr.GetOrdinal(this.Intermensajeagente);
            if (!dr.IsDBNull(iIntermensajeagente)) entity.Intermensajeagente = Convert.ToInt32(dr.GetValue(iIntermensajeagente));

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iClaprocodi = dr.GetOrdinal(this.Claprocodi);
            if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

            int iIntercodsegempr = dr.GetOrdinal(this.Intercodsegempr);
            if (!dr.IsDBNull(iIntercodsegempr)) entity.Intercodsegempr = dr.GetString(iIntercodsegempr);

            int iIntermwindispo = dr.GetOrdinal(this.Intermwindispo);
            if (!dr.IsDBNull(iIntermwindispo)) entity.Intermwindispo = Convert.ToDecimal(dr.GetValue(iIntermwindispo));

            int iInterindispo = dr.GetOrdinal(this.Interindispo);
            if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

            int iInterinterrup = dr.GetOrdinal(this.Interinterrup);
            if (!dr.IsDBNull(iInterinterrup)) entity.Interinterrup = dr.GetString(iInterinterrup);

            int iIntermantrelev = dr.GetOrdinal(this.Intermantrelev);
            if (!dr.IsDBNull(iIntermantrelev)) entity.Intermantrelev = Convert.ToInt32(dr.GetValue(iIntermantrelev));

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iInterjustifaprobrechaz = dr.GetOrdinal(this.Interjustifaprobrechaz);
            if (!dr.IsDBNull(iInterjustifaprobrechaz)) entity.Interjustifaprobrechaz = dr.GetString(iInterjustifaprobrechaz);

            int iInterprocesado = dr.GetOrdinal(this.Interprocesado);
            if (!dr.IsDBNull(iInterprocesado)) entity.Interprocesado = Convert.ToInt32(dr.GetValue(iInterprocesado));

            int iInterisfiles = dr.GetOrdinal(this.Interisfiles);
            if (!dr.IsDBNull(iInterisfiles)) entity.Interisfiles = dr.GetString(iInterisfiles);

            int iInterregprevactivo = dr.GetOrdinal(this.Interregprevactivo);
            if (!dr.IsDBNull(iInterregprevactivo)) entity.Interregprevactivo = dr.GetString(iInterregprevactivo);

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            int iInterusucreacion = dr.GetOrdinal(this.Interusucreacion);
            if (!dr.IsDBNull(iInterusucreacion)) entity.Interusucreacion = dr.GetString(iInterusucreacion);

            int iInterfeccreacion = dr.GetOrdinal(this.Interfeccreacion);
            if (!dr.IsDBNull(iInterfeccreacion)) entity.Interfeccreacion = dr.GetDateTime(iInterfeccreacion);

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES A LA ESTRUCTURA ORIGINAL
            //--------------------------------------------------------------------------------
            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iInterconexionprov = dr.GetOrdinal(this.Interconexionprov);
            if (!dr.IsDBNull(iInterconexionprov)) entity.Interconexionprov = dr.GetString(iInterconexionprov);

            int iIntersistemaaislado = dr.GetOrdinal(this.Intersistemaaislado);
            if (!dr.IsDBNull(iIntersistemaaislado)) entity.Intersistemaaislado = dr.GetString(iIntersistemaaislado);

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------
            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iTipoevenabrev = dr.GetOrdinal(this.Tipoevenabrev); // Tipo
            if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iFamNomb = dr.GetOrdinal(this.FamNomb);
            if (!dr.IsDBNull(iFamNomb)) entity.FamNomb = dr.GetString(iFamNomb);

            int iFamabrev = dr.GetOrdinal(this.Famabrev);
            if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = dr.GetInt32(iSubcausacodi);

            //fuenteestado
            int iInterfuentestado = dr.GetOrdinal(this.Interfuentestado);
            if (!dr.IsDBNull(iInterfuentestado)) entity.Interfuentestado = dr.GetInt32(iInterfuentestado);

            //pr25
            int iIntertipoindisp = dr.GetOrdinal(this.Intertipoindisp);
            if (!dr.IsDBNull(iIntertipoindisp)) entity.Intertipoindisp = dr.GetString(iIntertipoindisp);

            int iInterpr = dr.GetOrdinal(this.Interpr);
            if (!dr.IsDBNull(iInterpr)) entity.Interpr = Convert.ToDecimal(dr.GetValue(iInterpr));

            int iInterasocproc = dr.GetOrdinal(this.Interasocproc);
            if (!dr.IsDBNull(iInterasocproc)) entity.Interasocproc = dr.GetString(iInterasocproc);

            int iGrupotipocogen = dr.GetOrdinal(this.Grupotipocogen);
            if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

            //operador
            int iOperadoremprcodi = dr.GetOrdinal(this.Operadoremprcodi);
            if (!dr.IsDBNull(iOperadoremprcodi)) entity.Operadoremprcodi = Convert.ToInt32(dr.GetValue(iOperadoremprcodi));

            int iOperadornomb = dr.GetOrdinal(this.Operadornomb);
            if (!dr.IsDBNull(iOperadornomb)) entity.Operadornomb = dr.GetString(iOperadornomb);

            return entity;
        }
        #endregion

        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso principal de consulta de datos
        /// </summary>
        public InIntervencionDTO CreateQuery(IDataReader dr)
        {
            InIntervencionDTO entity = Create(dr);

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //-------------------------------------------------------------------------------------------------------
            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iTipoevenabrev = dr.GetOrdinal(this.Tipoevenabrev); // Tipo
            if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

            int iTipoEvenDesc = dr.GetOrdinal(this.TipoEvenDesc);
            if (!dr.IsDBNull(iTipoEvenDesc)) entity.TipoEvenDesc = dr.GetString(iTipoEvenDesc);

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iFamabrev = dr.GetOrdinal(this.Famabrev);
            if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

            //int iFamNomb = dr.GetOrdinal(this.FamNomb);
            //if (!dr.IsDBNull(iFamNomb)) entity.FamNomb = dr.GetString(iFamNomb);

            int iEquiNomb = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiNomb)) entity.Equiabrev = dr.GetString(iEquiNomb);

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 25/06/2019: CAMPOS ADICIONALES PARA SER PRESENTADO EN MANTTO CONSUL. Y EXCEL CONSUL.
            //-------------------------------------------------------------------------------------------------------
            int isubcausadesc = dr.GetOrdinal(this.subcausadesc);
            if (!dr.IsDBNull(isubcausadesc)) entity.Subcausadesc = dr.GetString(isubcausadesc);

            int iOperadornomb = dr.GetOrdinal(this.Operadornomb);
            if (!dr.IsDBNull(iOperadornomb)) entity.Operadornomb = dr.GetString(iOperadornomb);

            int iInterleido = dr.GetOrdinal(this.Interleido);
            if (!dr.IsDBNull(iInterleido)) entity.Interleido = Convert.ToInt32(dr.GetValue(iInterleido));

            return entity;
        }

        #region Helper Reporte Anexos Programa Anual
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de Anexos Programa Anual
        /// </summary>
        public InIntervencionDTO CreateReportMayores(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iProgrcodi = dr.GetOrdinal(this.Progrcodi);
            if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iIntermensaje = dr.GetOrdinal(this.Intermensaje);
            if (!dr.IsDBNull(iIntermensaje)) entity.Intermensaje = dr.GetString(iIntermensaje);

            int iIntermensajecoes = dr.GetOrdinal(this.Intermensajecoes);
            if (!dr.IsDBNull(iIntermensajecoes)) entity.Intermensajecoes = Convert.ToInt32(dr.GetValue(iIntermensajecoes));

            int iIntermensajeagente = dr.GetOrdinal(this.Intermensajeagente);
            if (!dr.IsDBNull(iIntermensajeagente)) entity.Intermensajeagente = Convert.ToInt32(dr.GetValue(iIntermensajeagente));

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iClaprocodi = dr.GetOrdinal(this.Claprocodi);
            if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

            int iIntermwindispo = dr.GetOrdinal(this.Intermwindispo);
            if (!dr.IsDBNull(iIntermwindispo)) entity.Intermwindispo = Convert.ToDecimal(dr.GetValue(iIntermwindispo));

            int iInterindispo = dr.GetOrdinal(this.Interindispo);
            if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

            int iInterinterrup = dr.GetOrdinal(this.Interinterrup);
            if (!dr.IsDBNull(iInterinterrup)) entity.Interinterrup = dr.GetString(iInterinterrup);

            int iIntermantrelev = dr.GetOrdinal(this.Intermantrelev);
            if (!dr.IsDBNull(iIntermantrelev)) entity.Intermantrelev = Convert.ToInt32(dr.GetValue(iIntermantrelev));

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iInterjustifaprobrechaz = dr.GetOrdinal(this.Interjustifaprobrechaz);
            if (!dr.IsDBNull(iInterjustifaprobrechaz)) entity.Interjustifaprobrechaz = dr.GetString(iInterjustifaprobrechaz);

            int iInterprocesado = dr.GetOrdinal(this.Interprocesado);
            if (!dr.IsDBNull(iInterprocesado)) entity.Interprocesado = Convert.ToInt32(dr.GetValue(iInterprocesado));

            int iInterisfiles = dr.GetOrdinal(this.Interisfiles);
            if (!dr.IsDBNull(iInterisfiles)) entity.Interisfiles = dr.GetString(iInterisfiles);

            int iInterregprevactivo = dr.GetOrdinal(this.Interregprevactivo);
            if (!dr.IsDBNull(iInterregprevactivo)) entity.Interregprevactivo = dr.GetString(iInterregprevactivo);

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES A LA ESTRUCTURA ORIGINAL
            //-------------------------------------------------------------------------------------------------------
            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //-------------------------------------------------------------------------------------------------------
            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iTipoEvenDesc = dr.GetOrdinal(this.TipoEvenDesc);
            if (!dr.IsDBNull(iTipoEvenDesc)) entity.TipoEvenDesc = dr.GetString(iTipoEvenDesc);

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iFamNomb = dr.GetOrdinal(this.FamNomb);
            if (!dr.IsDBNull(iFamNomb)) entity.FamNomb = dr.GetString(iFamNomb);

            int iEquiNomb = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiNomb)) entity.Equiabrev = dr.GetString(iEquiNomb);

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 05/06/2018: CAMPOS AUDITORES
            //-------------------------------------------------------------------------------------------------------
            int iInterusucreacion = dr.GetOrdinal(this.Interusucreacion);
            if (!dr.IsDBNull(iInterusucreacion)) entity.Interusucreacion = dr.GetString(iInterusucreacion);

            int iInterfeccreacion = dr.GetOrdinal(this.Interfeccreacion);
            if (!dr.IsDBNull(iInterfeccreacion)) entity.Interfeccreacion = dr.GetDateTime(iInterfeccreacion);

            int iClaseProgramacion = dr.GetOrdinal(this.ClaseProgramacion);
            if (!dr.IsDBNull(iClaseProgramacion)) entity.ClaseProgramacion = dr.GetString(iClaseProgramacion);

            int iIntercodsegempr = dr.GetOrdinal(this.Intercodsegempr);
            if (!dr.IsDBNull(iIntercodsegempr)) entity.Intercodsegempr = dr.GetString(iIntercodsegempr);

            int iIntercodipadre = dr.GetOrdinal(this.Intercodipadre);
            if (!dr.IsDBNull(iIntercodipadre)) entity.Intercodipadre = Convert.ToInt32(dr.GetValue(iIntercodipadre));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 14/06/2018: CAMPOS FLAG PARA PRESENTAR REGISTROS CON STYLOS
            //-------------------------------------------------------------------------------------------------------
            int iIntercreated = dr.GetOrdinal(this.Intercreated);
            if (!dr.IsDBNull(iIntercreated)) entity.Intercreated = Convert.ToInt32(dr.GetValue(iIntercreated));

            int iInterdeleted = dr.GetOrdinal(this.Interdeleted);
            if (!dr.IsDBNull(iInterdeleted)) entity.Interdeleted = Convert.ToInt32(dr.GetValue(iInterdeleted));

            return entity;
        }

        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte cruzado de Anexos Programa Anual
        /// </summary>
        public InIntervencionDTO CreateReporteCruzadoMayores(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iAreanomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreanomb)) entity.AreaNomb = dr.GetString(iAreanomb);

            int iEquiNomb = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiNomb)) entity.Equiabrev = dr.GetString(iEquiNomb);

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iInterindispo = dr.GetOrdinal(this.Interindispo);
            if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            return entity;
        }
        #endregion

        #region Helper Reporte Intervenciones Mayores
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de Anexos Programa Anual
        /// </summary>
        public InIntervencionDTO CreateReportMayoresPorPeriodo(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iProgrcodi = dr.GetOrdinal(this.Progrcodi);
            if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iIntermensaje = dr.GetOrdinal(this.Intermensaje);
            if (!dr.IsDBNull(iIntermensaje)) entity.Intermensaje = dr.GetString(iIntermensaje);

            int iIntermensajecoes = dr.GetOrdinal(this.Intermensajecoes);
            if (!dr.IsDBNull(iIntermensajecoes)) entity.Intermensajecoes = Convert.ToInt32(dr.GetValue(iIntermensajecoes));

            int iIntermensajeagente = dr.GetOrdinal(this.Intermensajeagente);
            if (!dr.IsDBNull(iIntermensajeagente)) entity.Intermensajeagente = Convert.ToInt32(dr.GetValue(iIntermensajeagente));

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iClaprocodi = dr.GetOrdinal(this.Claprocodi);
            if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

            int iIntermwindispo = dr.GetOrdinal(this.Intermwindispo);
            if (!dr.IsDBNull(iIntermwindispo)) entity.Intermwindispo = Convert.ToDecimal(dr.GetValue(iIntermwindispo));

            int iInterindispo = dr.GetOrdinal(this.Interindispo);
            if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

            int iInterinterrup = dr.GetOrdinal(this.Interinterrup);
            if (!dr.IsDBNull(iInterinterrup)) entity.Interinterrup = dr.GetString(iInterinterrup);

            int iIntermantrelev = dr.GetOrdinal(this.Intermantrelev);
            if (!dr.IsDBNull(iIntermantrelev)) entity.Intermantrelev = Convert.ToInt32(dr.GetValue(iIntermantrelev));

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iInterjustifaprobrechaz = dr.GetOrdinal(this.Interjustifaprobrechaz);
            if (!dr.IsDBNull(iInterjustifaprobrechaz)) entity.Interjustifaprobrechaz = dr.GetString(iInterjustifaprobrechaz);

            int iInterprocesado = dr.GetOrdinal(this.Interprocesado);
            if (!dr.IsDBNull(iInterprocesado)) entity.Interprocesado = Convert.ToInt32(dr.GetValue(iInterprocesado));

            int iInterisfiles = dr.GetOrdinal(this.Interisfiles);
            if (!dr.IsDBNull(iInterisfiles)) entity.Interisfiles = dr.GetString(iInterisfiles);

            int iInterregprevactivo = dr.GetOrdinal(this.Interregprevactivo);
            if (!dr.IsDBNull(iInterregprevactivo)) entity.Interregprevactivo = dr.GetString(iInterregprevactivo);

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES A LA ESTRUCTURA ORIGINAL
            //-------------------------------------------------------------------------------------------------------
            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //-------------------------------------------------------------------------------------------------------
            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iTipoEvenDesc = dr.GetOrdinal(this.TipoEvenDesc);
            if (!dr.IsDBNull(iTipoEvenDesc)) entity.TipoEvenDesc = dr.GetString(iTipoEvenDesc);

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iFamNomb = dr.GetOrdinal(this.FamNomb);
            if (!dr.IsDBNull(iFamNomb)) entity.FamNomb = dr.GetString(iFamNomb);

            int iEquiNomb = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiNomb)) entity.Equiabrev = dr.GetString(iEquiNomb);

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 05/06/2018: CAMPOS AUDITORES
            //-------------------------------------------------------------------------------------------------------
            int iInterusucreacion = dr.GetOrdinal(this.Interusucreacion);
            if (!dr.IsDBNull(iInterusucreacion)) entity.Interusucreacion = dr.GetString(iInterusucreacion);

            int iInterfeccreacion = dr.GetOrdinal(this.Interfeccreacion);
            if (!dr.IsDBNull(iInterfeccreacion)) entity.Interfeccreacion = dr.GetDateTime(iInterfeccreacion);

            int iClaseProgramacion = dr.GetOrdinal(this.ClaseProgramacion);
            if (!dr.IsDBNull(iClaseProgramacion)) entity.ClaseProgramacion = dr.GetString(iClaseProgramacion);

            int iIntercodsegempr = dr.GetOrdinal(this.Intercodsegempr);
            if (!dr.IsDBNull(iIntercodsegempr)) entity.Intercodsegempr = dr.GetString(iIntercodsegempr);

            int iIntercodipadre = dr.GetOrdinal(this.Intercodipadre);
            if (!dr.IsDBNull(iIntercodipadre)) entity.Intercodipadre = Convert.ToInt32(dr.GetValue(iIntercodipadre));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 14/06/2018: CAMPOS FLAG PARA PRESENTAR REGISTROS CON STYLOS
            //-------------------------------------------------------------------------------------------------------
            int iIntercreated = dr.GetOrdinal(this.Intercreated);
            if (!dr.IsDBNull(iIntercreated)) entity.Intercreated = Convert.ToInt32(dr.GetValue(iIntercreated));

            int iInterdeleted = dr.GetOrdinal(this.Interdeleted);
            if (!dr.IsDBNull(iInterdeleted)) entity.Interdeleted = Convert.ToInt32(dr.GetValue(iInterdeleted));

            return entity;
        }
        #endregion

        #region Helper Reporte Intervenciones Importantes
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de intervecniones Importantes
        /// </summary>
        public InIntervencionDTO CreateReportImportantes(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH -  11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            int iInterindispo = dr.GetOrdinal(this.Interindispo);
            if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

            return entity;
        }
        #endregion

        #region Helper Reporte Conexiones Provisionales
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de conexiones provisionales
        /// </summary>
        public InIntervencionDTO CreateReportConexionesProvisionales(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH -  11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            return entity;
        }
        #endregion

        #region Helper Reporte Sistemas Aislados
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de sistemas aislados
        /// </summary>
        public InIntervencionDTO CreateReportSistemasAislados(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH -  11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            return entity;
        }
        #endregion

        #region Helper Reporte Interrupcion o Restriccion de Suministros
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de Interrupcion / Restriccion de Suministros
        /// </summary>
        public InIntervencionDTO CreateReportInterrupcionRestriccionSuministros(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iIntermwindispo = dr.GetOrdinal(this.Intermwindispo);
            if (!dr.IsDBNull(iIntermwindispo)) entity.Intermwindispo = Convert.ToDecimal(dr.GetValue(iIntermwindispo));

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH -  11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            return entity;
        }
        #endregion

        #region Helper Reporte Agentes
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte para agentes
        /// </summary>
        public InIntervencionDTO CreateReportAgentes(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iProgrcodi = dr.GetOrdinal(this.Progrcodi);
            if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iIntermensaje = dr.GetOrdinal(this.Intermensaje);
            if (!dr.IsDBNull(iIntermensaje)) entity.Intermensaje = dr.GetString(iIntermensaje);

            int iIntermensajecoes = dr.GetOrdinal(this.Intermensajecoes);
            if (!dr.IsDBNull(iIntermensajecoes)) entity.Intermensajecoes = Convert.ToInt32(dr.GetValue(iIntermensajecoes));

            int iIntermensajeagente = dr.GetOrdinal(this.Intermensajeagente);
            if (!dr.IsDBNull(iIntermensajeagente)) entity.Intermensajeagente = Convert.ToInt32(dr.GetValue(iIntermensajeagente));

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iClaprocodi = dr.GetOrdinal(this.Claprocodi);
            if (!dr.IsDBNull(iClaprocodi)) entity.Claprocodi = Convert.ToInt32(dr.GetValue(iClaprocodi));

            int iIntermwindispo = dr.GetOrdinal(this.Intermwindispo);
            if (!dr.IsDBNull(iIntermwindispo)) entity.Intermwindispo = Convert.ToDecimal(dr.GetValue(iIntermwindispo));

            int iInterindispo = dr.GetOrdinal(this.Interindispo);
            if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

            int iInterinterrup = dr.GetOrdinal(this.Interinterrup);
            if (!dr.IsDBNull(iInterinterrup)) entity.Interinterrup = dr.GetString(iInterinterrup);

            int iIntermantrelev = dr.GetOrdinal(this.Intermantrelev);
            if (!dr.IsDBNull(iIntermantrelev)) entity.Intermantrelev = Convert.ToInt32(dr.GetValue(iIntermantrelev));

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iInterjustifaprobrechaz = dr.GetOrdinal(this.Interjustifaprobrechaz);
            if (!dr.IsDBNull(iInterjustifaprobrechaz)) entity.Interjustifaprobrechaz = dr.GetString(iInterjustifaprobrechaz);

            int iInterprocesado = dr.GetOrdinal(this.Interprocesado);
            if (!dr.IsDBNull(iInterprocesado)) entity.Interprocesado = Convert.ToInt32(dr.GetValue(iInterprocesado));

            int iInterisfiles = dr.GetOrdinal(this.Interisfiles);
            if (!dr.IsDBNull(iInterisfiles)) entity.Interisfiles = dr.GetString(iInterisfiles);

            int iInterregprevactivo = dr.GetOrdinal(this.Interregprevactivo);
            if (!dr.IsDBNull(iInterregprevactivo)) entity.Interregprevactivo = dr.GetString(iInterregprevactivo);

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES A LA ESTRUCTURA ORIGINAL
            //-------------------------------------------------------------------------------------------------------
            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //-------------------------------------------------------------------------------------------------------
            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iTipoEvenDesc = dr.GetOrdinal(this.TipoEvenDesc);
            if (!dr.IsDBNull(iTipoEvenDesc)) entity.TipoEvenDesc = dr.GetString(iTipoEvenDesc);

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iFamNomb = dr.GetOrdinal(this.FamNomb);
            if (!dr.IsDBNull(iFamNomb)) entity.FamNomb = dr.GetString(iFamNomb);

            int iEquiNomb = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiNomb)) entity.Equiabrev = dr.GetString(iEquiNomb);

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 05/06/2018: CAMPOS AUDITORES
            //-------------------------------------------------------------------------------------------------------
            int iInterusucreacion = dr.GetOrdinal(this.Interusucreacion);
            if (!dr.IsDBNull(iInterusucreacion)) entity.Interusucreacion = dr.GetString(iInterusucreacion);

            int iInterfeccreacion = dr.GetOrdinal(this.Interfeccreacion);
            if (!dr.IsDBNull(iInterfeccreacion)) entity.Interfeccreacion = dr.GetDateTime(iInterfeccreacion);

            int iClaseProgramacion = dr.GetOrdinal(this.ClaseProgramacion);
            if (!dr.IsDBNull(iClaseProgramacion)) entity.ClaseProgramacion = dr.GetString(iClaseProgramacion);

            int iIntercodsegempr = dr.GetOrdinal(this.Intercodsegempr);
            if (!dr.IsDBNull(iIntercodsegempr)) entity.Intercodsegempr = dr.GetString(iIntercodsegempr);

            int iIntercodipadre = dr.GetOrdinal(this.Intercodipadre);
            if (!dr.IsDBNull(iIntercodipadre)) entity.Intercodipadre = Convert.ToInt32(dr.GetValue(iIntercodipadre));

            //-------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 14/06/2018: CAMPOS FLAG PARA PRESENTAR REGISTROS CON STYLOS
            //-------------------------------------------------------------------------------------------------------
            int iIntercreated = dr.GetOrdinal(this.Intercreated);
            if (!dr.IsDBNull(iIntercreated)) entity.Intercreated = Convert.ToInt32(dr.GetValue(iIntercreated));

            int iInterdeleted = dr.GetOrdinal(this.Interdeleted);
            if (!dr.IsDBNull(iInterdeleted)) entity.Interdeleted = Convert.ToInt32(dr.GetValue(iInterdeleted));

            return entity;
        }
        #endregion

        #region Helper Reporte Proc - 25 OSINERGMIN
        // ----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte para OSINERGMIN
        /// </summary>
        public InIntervencionDTO CreateReportOSINERGMIN7d(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            //---------------------------------------------------------------------------------------
            // ASSETEC.SGH -  11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //---------------------------------------------------------------------------------------
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            //---------------------------------------------------------------------------------------
            // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------------- 
            int iClase = dr.GetOrdinal(this.Clase);
            if (!dr.IsDBNull(iClase)) entity.Clase = dr.GetString(iClase);

            //---------------------------------------------------------------------------------------
            // ASSETEC.SGH - 09/04/2018: CAMPOS ADICIONALES CALCULADOS
            //---------------------------------------------------------------------------------------
            int iTareacodi = dr.GetOrdinal(this.Tareacodi); // Tipo de area
            if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

            int iInterTEOsinerg = dr.GetOrdinal(this.InterTeosinerg);
            if (!dr.IsDBNull(iInterTEOsinerg)) entity.InterTeosinerg = dr.GetString(iInterTEOsinerg);

            return entity;
        }

        // ----------------------------------------------------------------------------------------------------------       
        #endregion

        #region Helper Reporte Intervenciones Formato OSINERGMIN
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de Intervenciones
        /// </summary>
        public InIntervencionDTO CreateReportIntervenciones(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi); // Cod. Eq
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iIntermwindispo = dr.GetOrdinal(this.Intermwindispo);
            if (!dr.IsDBNull(iIntermwindispo)) entity.Intermwindispo = Convert.ToDecimal(dr.GetValue(iIntermwindispo));

            int iInterindispo = dr.GetOrdinal(this.Interindispo);
            if (!dr.IsDBNull(iInterindispo)) entity.Interindispo = dr.GetString(iInterindispo);

            int iInterinterrup = dr.GetOrdinal(this.Interinterrup);
            if (!dr.IsDBNull(iInterinterrup)) entity.Interinterrup = dr.GetString(iInterinterrup);

            //-----------------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH -  11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //-----------------------------------------------------------------------------------------------------------------
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            //-----------------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES CALCULADOS
            //-----------------------------------------------------------------------------------------------------------------
            int iTipoevenabrev = dr.GetOrdinal(this.Tipoevenabrev); // Tipo
            if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

            //-----------------------------------------------------------------------------------------------------------------
            // ASSETEC.SGH - 09/04/2018: CAMPOS ADICIONALES CALCULADOS
            //-----------------------------------------------------------------------------------------------------------------
            int iInterTEOsinerg = dr.GetOrdinal(this.InterTeosinerg);
            if (!dr.IsDBNull(iInterTEOsinerg)) entity.InterTeosinerg = dr.GetString(iInterTEOsinerg);

            int iProg = dr.GetOrdinal(this.Progr);
            if (!dr.IsDBNull(iProg)) entity.Progr = dr.GetString(iProg);

            return entity;
        }
        #endregion

        #region Helper Reporte Mensajeria
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte de mensajeria
        /// </summary>
        public SiMensajeDTO CreateReportMensajes(IDataReader dr)
        {
            SiMensajeDTO entity = new SiMensajeDTO();

            int iMsgcodi = dr.GetOrdinal(this.Msgcodi);
            if (!dr.IsDBNull(iMsgcodi)) entity.Msgcodi = Convert.ToInt32(dr.GetValue(iMsgcodi));

            int iMsgfeccreacion = dr.GetOrdinal(this.Msgfeccreacion);
            if (!dr.IsDBNull(iMsgfeccreacion)) entity.Msgfecha = dr.GetDateTime(iMsgfeccreacion);

            int iMsgasunto = dr.GetOrdinal(this.Msgasunto);
            if (!dr.IsDBNull(iMsgasunto)) entity.Msgasunto = dr.GetString(iMsgasunto);

            int iMsgcontenido = dr.GetOrdinal(this.Msgcontenido);
            if (!dr.IsDBNull(iMsgcontenido)) entity.Msgcontenido = Encoding.UTF8.GetString((byte[])(dr.GetValue(iMsgcontenido)));

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

            return entity;
        }
        #endregion

        #region Helper Reporte F1F2
        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte para el calculo de indices F1 y F2
        /// </summary>
        public InIntervencionDTO CreateReportF1F2(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            //int iIntercodi = dr.GetOrdinal(this.Intercodi);
            //if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));            

            int iInterfechaini = dr.GetOrdinal(this.Interfechaini);
            if (!dr.IsDBNull(iInterfechaini)) entity.Interfechaini = dr.GetDateTime(iInterfechaini);

            int iInterfechafin = dr.GetOrdinal(this.Interfechafin);
            if (!dr.IsDBNull(iInterfechafin)) entity.Interfechafin = dr.GetDateTime(iInterfechafin);

            int iInterdescrip = dr.GetOrdinal(this.Interdescrip);
            if (!dr.IsDBNull(iInterdescrip)) entity.Interdescrip = dr.GetString(iInterdescrip);

            int iInterjustifaprobrechaz = dr.GetOrdinal(this.Interjustifaprobrechaz);
            if (!dr.IsDBNull(iInterjustifaprobrechaz)) entity.Interjustifaprobrechaz = dr.GetString(iInterjustifaprobrechaz);

            int iIntercodsegempr = dr.GetOrdinal(this.Intercodsegempr);
            if (!dr.IsDBNull(iIntercodsegempr)) entity.Intercodsegempr = dr.GetString(iIntercodsegempr);

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------
            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH - 22/01/2017: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------           
            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            //--------------------------------------------------------------------------------
            // ASSETEC.SGH - 18/04/2018: CAMPOS ADICIONALES CALCULADOS
            //--------------------------------------------------------------------------------
            int iDuracion = dr.GetOrdinal(this.Duracion);
            if (!dr.IsDBNull(iDuracion)) entity.Duracion = Convert.ToInt32(dr.GetValue(iDuracion));

            int iObservaciones = dr.GetOrdinal(this.Observaciones);
            if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = dr.GetString(iObservaciones);

            int iComentario = dr.GetOrdinal(this.Comentario);
            if (!dr.IsDBNull(iComentario)) entity.Comentario = dr.GetString(iComentario);

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iTareacodi = dr.GetOrdinal(this.Tareacodi);
            if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

            return entity;
        }

        /// <summary>
        /// Metodo que mapea de la tabla IN_INTERVENCION para el proceso de reporte para el calculo de indices F1 y F2
        /// </summary>
        public InIntervencionDTO CreateReportIndicesF1F2(IDataReader dr)
        {
            InIntervencionDTO entity = new InIntervencionDTO();

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iEquiabrev = dr.GetOrdinal(this.Equiabrev);
            if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

            return entity;
        }
        #endregion
        #endregion

        #region Mapeo de Campos
        public string Intercodi = "INTERCODI";
        public string Tipoevencodi = "TIPOEVENCODI";
        public string Progrcodi = "PROGRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Intermensaje = "INTERMENSAJE";
        public string Intermensajecoes = "INTERMENSAJECOES";
        public string Intermensajeagente = "INTERMENSAJEAGENTE";
        public string Interfechapreini = "INTERFECHAPREINI";
        public string Interfechaini = "INTERFECHAINI";
        public string Interfechaprefin = "INTERFECHAPREFIN";
        public string Interfechafin = "INTERFECHAFIN";
        public string Claprocodi = "CLAPROCODI";
        public string Intercodsegempr = "INTERCODSEGEMPR";
        public string Interrepetir = "INTERREPETIR";
        public string Intermwindispo = "INTERMWINDISPO";
        public string Interindispo = "INTERINDISPO";
        public string Interinterrup = "INTERINTERRUP";
        public string Intermantrelev = "INTERMANTRELEV";
        public string Interdescrip = "INTERDESCRIP";
        public string Interjustifaprobrechaz = "INTERJUSTIFAPROBRECHAZ";
        public string Interfecaprobrechaz = "INTERFECAPROBRECHAZ";
        public string Interprocesado = "INTERPROCESADO";
        public string Interisfiles = "INTERISFILES";
        public string Intermanttocodi = "INTERMANTTOCODI";
        public string Interevenpadre = "INTEREVENPADRE";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Intercodipadre = "INTERCODIPADRE";
        public string Interregprevactivo = "INTERREGPREVACTIVO";
        public string Estadocodi = "ESTADOCODI";
        public string Intercreated = "INTERCREATED";
        public string Interdeleted = "INTERDELETED";
        public string Interusucreacion = "INTERUSUCREACION";
        public string Interfeccreacion = "INTERFECCREACION";
        public string Interusumodificacion = "INTERUSUMODIFICACION";
        public string Interfecmodificacion = "INTERFECMODIFICACION";
        public string Operadoremprcodi = "OPERADOREMPRCODI";
        public string Operadornomb = "OPERADORNOMB";
        public string Intertipoindisp = "Intertipoindisp";
        public string Interpr = "Interpr";
        public string Interasocproc = "Interasocproc";
        public string Intercarpetafiles = "INTERCARPETAFILES";
        public string Interipagente = "INTERIPAGENTE";
        public string Interusuagrup = "INTERUSUAGRUP";
        public string Interfecagrup = "INTERFECAGRUP";
        public string Internota = "INTERNOTA";
        public string Interflagsustento = "INTERFLAGSUSTENTO";

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES A LA ESTRUCTURA ORIGINAL
        //--------------------------------------------------------------------------------
        public string Areacodi = "AREACODI"; // Id de Area (Id de Ubicación)
        public string Interconexionprov = "INTERCONEXIONPROV"; // Flag de Conexion Provicional
        public string Intersistemaaislado = "INTERSISTEMAAISLADO"; // Flag de Sistema Aislado

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 11/10/2017: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------        
        public string Famcodi = "FAMCODI"; // Id de Familia (Id Tipo de Equipo)
        public string TipoEvenDesc = "TIPOEVENDESC"; // Nombre de Tipo de evento (Nombre de tipo de intervecnión)
        public string EmprNomb = "EMPRNOMB"; // Nombre de Empresa
        public string AreaNomb = "AREANOMB"; // Nombre de Area (Nombre de Ubicación)
        public string FamNomb = "FAMNOMB"; // Nombre de Familia (Nombre de Tipo de Equipo)
        public string EquiNomb = "EQUINOMB"; // Nombre de Equipo

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 21/11/2017: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------
        public string CodEqOsinerg = "EQUICODIOSINERG"; // Cod. Eq  COD.
        public string InterTeosinerg = "TEOSINERG"; // Flag de TE_Osinerg
        public string IntNombTipoProgramacion = "EVENCLASEDESC";   // Clase (Tipo de Programacion)
        public string IntNombTipoIntervencion = "TIPOEVENDESC";   // Tipo de Intervención (Tipo Evento o Mantenimiento)
        public string IntNombClaseProgramacion = "CLASEPROGNOMBRE";  // Clase de programación (Progr.)

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 11/01/2018: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------
        public string Mes = "MES";   // Mes del año
        public string Semana = "SEMANA";  // Semana del Año-Mes
        public string Dia = "DIA";  // Dia al Año-Mes-Semana
        public string NroItem = "ITEM";  // Nro de Item
        public string Clapronombre = "CLAPRONOMBRE";  // Nombre de la clase de programación

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 22/01/2018: CAMPOS ADICIONALES CALCULADOS
        //--------------------------------------------------------------------------------
        public string sInterfechaini = "SINTERFECHAINI"; // Cadena de Fecha y hora de Inicio Formateada
        public string sInterfechafin = "SINTERFECHAFIN"; // Cadena de Fecha y hora de Fin Formateada
        public string Famabrev = "FAMABREV"; // Nombre Abreviado de la familia o tipo de equipo
        public string Equiabrev = "EQUIABREV"; // Nombre abreviado del equipo
        public string Tareacodi = "TAREACODI"; // Código de Tipo de Area
        public string Equipot = "EQUIPOT"; // Potencia promedio del equipo        
        public string Equimaniobra = "EQUIMANIOBRA"; //Flag indicador si el equipo esta en maniobras

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 23/02/2018: CAMPOS ADICIONALES PARA REPORTE MENSAJERIA
        //--------------------------------------------------------------------------------
        public string Msgcodi = "MSGCODI";
        public string Msgfeccreacion = "MSGFECCREACION";
        public string Msgasunto = "MSGASUNTO";
        public string Msgcontenido = "MSGCONTENIDO";

        //---------------------------------------------------------------------------------------
        // ASSETEC.SGH - 09/04/2018: CAMPOS ADICIONALES CALCULADOS
        //---------------------------------------------------------------------------------------
        public string Nombsituacion = "NOMBSITUACION";
        public string Clase = "CLASE";
        public string Progr = "PROGR";
        public string Tipoevenabrev = "TIPOEVENABREV";

        //---------------------------------------------------------------------------------------
        // ASSETEC.SGH - 18/04/2018: CAMPOS ADICIONALES CALCULADOS
        //---------------------------------------------------------------------------------------
        public string Duracion = "DURACION";
        public string Observaciones = "OBSERVACIONES";
        public string Comentario = "COMENTARIO";

        public string ClaseProgramacion = "CLASEPROGRAMACION";

        // ---------------------------------------------------------------
        // ULTIMO CAMPO MAPEADO 07-08-2018
        // ---------------------------------------------------------------
        public string Areaabrev = "AREAABREV";
        // ---------------------------------------------------------------

        //---------------------------------------------------------------------------------------
        // ASSETEC.SGH - 25/06/2019: CAMPO ADICIONAL PARA SER PRESENTADO EN MANTTO CONSUL. Y EXCEL CONSUL.
        //---------------------------------------------------------------------------------------
        public string subcausadesc = "SUBCAUSADESC";
        //---------------------------------------------------------------------------------------

        public string Grupotipocogen = "Grupotipocogen";
        public string Interfuentestado = "INTERFUENTESTADO";
        public string Progrsololectura = "PROGRSOLOLECTURA";

        public string Interleido = "INTERLEIDO";
        public string Estadopadre = "ESTADOPADRE";
        #endregion

        #region CAMPOS PARA ACTUALIZAR LA TABLA FW_USER
        public string Usercode = "USERCODE";
        public string UserflagPermiso = "USERFLAGPERMISO";
        #endregion

        #region Querys SQL

        public string SqlGetByCodigoPadre
        {
            get { return base.GetSqlXml("GetByCodigoPadre"); }
        }

        public string SqlConsultarIntervenciones
        {
            get { return base.GetSqlXml("ConsultarIntervenciones"); }
        }

        public string SqlConsultarTrazabilidad
        {
            get { return base.GetSqlXml("ConsultarTrazabilidad"); }
        }

        public string SqlConsultarIntervencionesXIds
        {
            get { return base.GetSqlXml("ConsultarIntervencionesXIds"); }
        }

        public string SqlDeshabilitarIntervencionEnReversion
        {
            get { return base.GetSqlXml("DeshabilitarIntervencionEnReversion"); }
        }

        public string SqlHabilitarIntervencionesRevertidas
        {
            get { return base.GetSqlXml("HabilitarIntervencionesRevertidas"); }
        }

        public string SqlDeshabilitarIntervencionEliminadaRechazada
        {
            get { return base.GetSqlXml("DeshabilitarIntervencionEliminadaRechazada"); }
        }

        public string SqlRecuperarIntervencion
        {
            get { return base.GetSqlXml("RecuperarIntervencion"); }
        }

        public string SqlUpdateIntervencionLeidoAgente
        {
            get { return base.GetSqlXml("UpdateIntervencionLeidoAgente"); }
        }

        public string SqlListarIntervencionesSinArchivo
        {
            get { return base.GetSqlXml("ListarIntervencionesSinArchivo"); }
        }

        //-----------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------
        // ASSETEC.SGH - 11/01/2018: FUNCIONES PARA OPERACIONES CON INTERVENCIONES
        //-----------------------------------------------------------------------------------------------
        #region Querys Para Las Operaciones con Intervenciones
        public string SqlDesabilitarIntervencionConTrazabilidad
        {
            get { return base.GetSqlXml("DesabilitarIntervencionConTrazabilidad"); }
        }
        public string SqlDeshabilitarIntervencionesRecepcion
        {
            get { return base.GetSqlXml("DeshabilitarIntervencionesRecepcion"); }
        }
        public string SqlDeshabilitarIntervencionesEnReversion
        {
            get { return base.GetSqlXml("DeshabilitarIntervencionesEnReversion"); }
        }

        public string SqlUpdateIsFiles
        {
            get { return base.GetSqlXml("UpdateIsFiles"); }
        }

        public string SqlUpdateTieneMensaje
        {
            get { return base.GetSqlXml("UpdateTieneMensaje"); }
        }

        public string SqlUpdateEstadoMensajeCOES
        {
            get { return base.GetSqlXml("UpdateEstadoMensajeCOES"); }
        }

        public string SqlUpdateEstadoMensajeAgente
        {
            get { return base.GetSqlXml("UpdateEstadoMensajeAgente"); }
        }

        public string SqlExisteCodigoSeguimiento
        {
            get { return base.GetSqlXml("ExisteCodigoSeguimiento"); }
        }

        public string SqlListarIntervencionCandidatoPorCriterio
        {
            get { return base.GetSqlXml("ListarIntervencionCandidatoPorCriterio"); }
        }

        #endregion
        //-----------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------
        // ASSETEC.SGH - 21/11/2017: FUNCIONES PARA REPORTES
        //-----------------------------------------------------------------------------------------------
        #region Querys Para Reportes
        #region Querys Reporte de Anexos Programación Anual
        public string SqlRptIntervencionesMayores
        {
            get { return base.GetSqlXml("RptIntervencionesMayores"); }
        }

        public string SqlRptCruzadoIntervencionesMayoresGeneracionESFS
        {
            get { return base.GetSqlXml("RptIntervencionesMayoresAnexo6CruzadoGeneracion"); }
        }

        public string SqlRptCruzadoIntervencionesMayoresTransmisionESFS
        {
            get { return base.GetSqlXml("RptIntervencionesMayoresAnexo6CruzadoTransmision"); }
        }
        #endregion

        #region Querys Reporte de Intervenciones Mayores
        public string SqlRptIntervencionesMayoresPorPeriodo
        {
            get { return base.GetSqlXml("RptIntervencionesMayoresPorPeriodo"); }
        }
        #endregion

        #region Querys Reporte de Intervenciones Importantes
        public string SqlRptIntervencionesImportantes
        {
            get { return base.GetSqlXml("RptIntervencionesImportantes"); }
        }
        #endregion

        #region Querys Reporte de Eventos
        public string SqlRptEventos
        {
            get { return base.GetSqlXml("RptEventos"); }
        }
        #endregion

        #region Querys Reporte de Conexiones Provisionales
        public string SqlRptConexionesProvisionales
        {
            get { return base.GetSqlXml("RptConexionesProvisionales"); }
        }
        #endregion

        #region Querys Reporte de Sistemas Aislados
        public string SqlRptSistemasAislados
        {
            get { return base.GetSqlXml("RptSistemasAislados"); }
        }
        #endregion

        #region Querys Reporte de Interrupcion Por Restriccion Suministros
        public string SqlRptInterrupcionRestriccionSuministros
        {
            get { return base.GetSqlXml("RptInterrupcionRestriccionSuministros"); }
        }
        #endregion       

        #region Querys Reporte OSINERGMIN Proc25
        public string SqlRptOSINERGMINProc257dListado
        {
            get { return base.GetSqlXml("RptOSINERGMINProc257dListado"); }
        }
        #endregion

        #region Querys Reporte Intervenciones OSINERGMIN
        public string SqlRptIntervenciones
        {
            get { return base.GetSqlXml("RptIntervenciones"); }
        }

        public string SqlListaMantenimientos25
        {
            get { return base.GetSqlXml("ListaMantenimientos25"); }
        }

        public string SqlRptIntervencionesOsinergmin
        {
            get { return base.GetSqlXml("RptIntervencionesOsinergmin"); }
        }
        #endregion

        #region Querys Reporte F1F2
        #region PARA INTERVENCIONES PROGRAMADAS
        public string SqlRptIntervencionesF1F2Programados
        {
            get { return base.GetSqlXml("RptIntervencionesF1F2Programados"); }
        }

        public string SqlRptBuscarEjecutadoPorCodSeguimiento
        {
            get { return base.GetSqlXml("RptBuscarEjecutadoPorCodSeguimiento"); }
        }
        #endregion

        #region PARA INTERVENCIONES EJECUTADAS
        public string SqlRptIntervencionesF1F2Ejecutados
        {
            get { return base.GetSqlXml("RptIntervencionesF1F2Ejecutados"); }
        }

        public string SqlRptBuscarMensualProgramadoPorCodSeguimiento
        {
            get { return base.GetSqlXml("RptBuscarMensualProgramadoPorCodSeguimiento"); }
        }
        #endregion

        #endregion

        #region Querys Reporte Para Agentes
        public string SqlRptAgentes
        {
            get { return base.GetSqlXml("RptAgentes"); }
        }
        #endregion

        #region Querys Reporte de Mensajeria
        public string SqlRptConsultasMensajes
        {
            get { return base.GetSqlXml("RptConsultasMensajes"); }
        }

        public string SqlRptConsultasMensajesPaginado
        {
            get { return base.GetSqlXml("RptConsultasMensajesPaginado"); }
        }

        public string SqlTotalRegistrosRptConsultasMensajes
        {
            get { return base.GetSqlXml("TotalRegistrosRptConsultasMensajes"); }
        }
        #endregion       
        #endregion
        //-----------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------
        #endregion

        #region Métodos para Validación con Aplicativos

        public string SqlListarIntervencionesEquiposGen
        {
            get { return base.GetSqlXml("ListarIntervencionesEquiposGen"); }
        }

        #endregion

        #region Query Para actualizar fw_user
        public string SqlUpdateUserPermiso
        {
            get { return base.GetSqlXml("UpdateUserPermiso"); }
        }
        public string SqlObtenerFlagUserPermiso
        {
            get { return base.GetSqlXml("ObtenerFlagUserPermiso"); }
        }
        public string SqlConsultarPermisos
        {
            get { return base.GetSqlXml("ConsultarPermisos"); }
        }

        public string SqlSaveIntervencion
        {
            get { return base.GetSqlXml("SaveIntervencion"); }
        }

        #endregion

        #region Yupana - Portal

        public string Emprabrev = "EMPRABREV";
        public string Evenclasedesc = "EVENCLASEDESC";
        public string Areadesc = "AREADESC";
        public string Causaevenabrev = "CAUSAEVENABREV";
        public string Equitension = "EQUITENSION";
        public string Tipoevendesc = "TIPOEVENDESC";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Eventipoindisp = "EVENTIPOINDISP";
        public string Evenclaseabrev = "EVENCLASEABREV";
        public string Osigrupocodi = "OSIGRUPOCODI";
        public string Equipadre = "EQUIPADRE";

        public string SqlListarIntervencionesXPagina
        {
            get { return base.GetSqlXml("ListarIntervencionesXPagina"); }
        }

        public string SqlObtenerNroRegistrosPaginado
        {
            get { return base.GetSqlXml("ObtenerNroRegistrosPaginado"); }
        }

        public string SqlListarIntervencionesGrafico
        {
            get { return base.GetSqlXml("ListarIntervencionesGrafico"); }
        }

        public string SqlListarIntervencionesTIITR
        {
            get { return base.GetSqlXml("ListarIntervencionesNTIITR"); }
        }

        #endregion

    }
}
