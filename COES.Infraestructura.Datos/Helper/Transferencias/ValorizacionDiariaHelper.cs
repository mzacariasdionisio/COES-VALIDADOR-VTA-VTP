using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTD_VALORIZACION - VTD_VALORIZACIONDETALLE
    /// </summary>
    public class ValorizacionDiariaHelper : HelperBase
    {
        public ValorizacionDiariaHelper(): base(Consultas.ValorizacionDiariaSql)
        {
        }

        public ValorizacionDiariaDTO Create(IDataReader dr)
        {
            ValorizacionDiariaDTO entity = new ValorizacionDiariaDTO();

            //Valorizacion
            int iValocodi = dr.GetOrdinal(this.Valocodi);
            if (!dr.IsDBNull(iValocodi)) entity.Valocodi = Convert.ToInt32(dr.GetValue(iValocodi));

            int iValofecha = dr.GetOrdinal(this.Valofecha);
            if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha);

            int iValomr = dr.GetOrdinal(this.Valomr);
            if (!dr.IsDBNull(iValomr)) entity.Valomr = Convert.ToDecimal(dr.GetValue(iValomr));

            int iValopreciopotencia = dr.GetOrdinal(this.Valopreciopotencia);
            if (!dr.IsDBNull(iValopreciopotencia)) entity.Valopreciopotencia = Convert.ToDecimal(dr.GetValue(iValopreciopotencia));

            int iValodemandacoes = dr.GetOrdinal(this.Valodemandacoes);
            if (!dr.IsDBNull(iValodemandacoes)) entity.Valodemandacoes = Convert.ToDecimal(dr.GetValue(iValodemandacoes));

            int iValofactorreparto = dr.GetOrdinal(this.Valofactorreparto);
            if (!dr.IsDBNull(iValofactorreparto)) entity.Valofactorreparto = Convert.ToDecimal(dr.GetValue(iValofactorreparto));

            int iValoporcentajeperdida = dr.GetOrdinal(this.Valoporcentajeperdida);
            if (!dr.IsDBNull(iValoporcentajeperdida)) entity.Valoporcentajeperdida = Convert.ToDecimal(dr.GetValue(iValoporcentajeperdida));

            int iValofrectotal = dr.GetOrdinal(this.Valofrectotal);
            if (!dr.IsDBNull(iValofrectotal)) entity.Valofrectotal = Convert.ToDecimal(dr.GetValue(iValofrectotal));

            int iValootrosequipos = dr.GetOrdinal(this.Valootrosequipos);
            if (!dr.IsDBNull(iValootrosequipos)) entity.Valootrosequipos = Convert.ToDecimal(dr.GetValue(iValootrosequipos));

            int iValocostofuerabanda = dr.GetOrdinal(this.Valocostofuerabanda);
            if (!dr.IsDBNull(iValocostofuerabanda)) entity.Valocostofuerabanda = Convert.ToDecimal(dr.GetValue(iValocostofuerabanda));

            int iValoestado = dr.GetOrdinal(this.Valoestado);
            if (!dr.IsDBNull(iValoestado)) entity.Valoestado = Convert.ToChar(dr.GetString(iValoestado));

            int iValosucreacion = dr.GetOrdinal(this.Valousucreacion);
            if (!dr.IsDBNull(iValosucreacion)) entity.Valousucreacion = dr.GetString(iValosucreacion);

            int iValofeccreacion = dr.GetOrdinal(this.Valofeccreacion);
            if (!dr.IsDBNull(iValofeccreacion)) entity.Valofeccreacion = dr.GetDateTime(iValofeccreacion);

            int iValousumodificacion = dr.GetOrdinal(this.Valousumodificacion);
            if (!dr.IsDBNull(iValousumodificacion)) entity.Valousumodificacion = dr.GetString(iValousumodificacion);

            int iValofecmodificacion = dr.GetOrdinal(this.Valofecmodificacion);
            if (!dr.IsDBNull(iValofecmodificacion)) entity.Valofecmodificacion = dr.GetDateTime(iValofecmodificacion);

            //ValorizacionDetalle
            int iValdcodi = dr.GetOrdinal(this.Valdcodi);
            if (!dr.IsDBNull(iValdcodi)) entity.Valdcodi = Convert.ToInt32(dr.GetValue(iValdcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValdretiro = dr.GetOrdinal(this.Valdretiro);
            if (!dr.IsDBNull(iValdretiro)) entity.Valdretiro = Convert.ToDecimal(dr.GetValue(iValdretiro));

            int iValdentrega = dr.GetOrdinal(this.Valdentrega);
            if (!dr.IsDBNull(iValdentrega)) entity.Valdentrega = Convert.ToDecimal(dr.GetValue(iValdentrega));

            int iValdpfirremun = dr.GetOrdinal(this.Valdpfirremun);
            if (!dr.IsDBNull(iValdpfirremun)) entity.Valdpfirremun = Convert.ToDecimal(dr.GetValue(iValdpfirremun));

            int iValddemandacoincidente = dr.GetOrdinal(this.Valddemandacoincidente);
            if (!dr.IsDBNull(iValddemandacoincidente)) entity.Valddemandacoincidente = Convert.ToDecimal(dr.GetValue(iValddemandacoincidente));

            int iValdmoncapacidad = dr.GetOrdinal(this.Valdmoncapacidad);
            if (!dr.IsDBNull(iValdmoncapacidad)) entity.Valdmoncapacidad = Convert.ToDecimal(dr.GetValue(iValdmoncapacidad));

            int iValdpeajeuni = dr.GetOrdinal(this.Valdpeajeuni);
            if (!dr.IsDBNull(iValdpeajeuni)) entity.Valdpeajeuni = Convert.ToDecimal(dr.GetValue(iValdpeajeuni));

            int iValdfactorp = dr.GetOrdinal(this.Valdfactorp);
            if (!dr.IsDBNull(iValdfactorp)) entity.Valdfactorp = Convert.ToDecimal(dr.GetValue(iValdfactorp));

            int iValdpagoio = dr.GetOrdinal(this.Valdpagoio);
            if (!dr.IsDBNull(iValdpagoio)) entity.Valdpagoio = Convert.ToDecimal(dr.GetValue(iValdpagoio));

            int iValdpaosc = dr.GetOrdinal(this.Valdpagosc);
            if (!dr.IsDBNull(iValdpaosc)) entity.Valdpagosc = Convert.ToDecimal(dr.GetValue(iValdpaosc));

            int iValdfpgm = dr.GetOrdinal(this.Valdfpgm);
            if (!dr.IsDBNull(iValdfpgm)) entity.Valdfpgm = Convert.ToDecimal(dr.GetValue(iValdfpgm));

            int iValdmcio = dr.GetOrdinal(this.Valdmcio);
            if (!dr.IsDBNull(iValdmcio)) entity.Valdmcio = Convert.ToDecimal(dr.GetValue(iValdmcio));

            int iValdpdsc = dr.GetOrdinal(this.Valdpdsc);
            if (!dr.IsDBNull(iValdpdsc)) entity.Valdpdsc = Convert.ToDecimal(dr.GetValue(iValdpdsc));

            int iValdcargoconsumo = dr.GetOrdinal(this.Valdcargoconsumo);
            if (!dr.IsDBNull(iValdcargoconsumo)) entity.Valdcargoconsumo = Convert.ToDecimal(dr.GetValue(iValdcargoconsumo));


            int iValdco = dr.GetOrdinal(this.Valoco);
            if (!dr.IsDBNull(iValdco)) entity.Valoco = Convert.ToDecimal(dr.GetValue(iValdco));

            int iValdra = dr.GetOrdinal(this.Valora);
            if (!dr.IsDBNull(iValdra)) entity.Valora = Convert.ToDecimal(dr.GetValue(iValdra));

            int iValdofmax = dr.GetOrdinal(this.Valoofmax);
            if (!dr.IsDBNull(iValdofmax)) entity.Valoofmax = Convert.ToDecimal(dr.GetValue(iValdofmax));

            int iValdcompcostosoper = dr.GetOrdinal(this.Valocompcostosoper);
            if (!dr.IsDBNull(iValdcompcostosoper)) entity.Valocompcostosoper = Convert.ToDecimal(dr.GetValue(iValdcompcostosoper));

            int iValdportesadicional = dr.GetOrdinal(this.Valdaportesadicional);
            if (!dr.IsDBNull(iValdportesadicional)) entity.Valdaportesadicional = Convert.ToDecimal(dr.GetValue(iValdportesadicional));

            int iValdcomptermrt = dr.GetOrdinal(this.Valocomptermrt);
            if (!dr.IsDBNull(iValdcomptermrt)) entity.Valocomptermrt = Convert.ToDecimal(dr.GetValue(iValdcomptermrt));

            int iValdsucreacion = dr.GetOrdinal(this.Valdusucreacion);
            if (!dr.IsDBNull(iValdsucreacion)) entity.Valdusucreacion = dr.GetString(iValdsucreacion);

            int iValdfeccreacion = dr.GetOrdinal(this.Valdfeccreacion);
            if (!dr.IsDBNull(iValdfeccreacion)) entity.Valdfeccreacion =dr.GetDateTime(iValdfeccreacion);

            int iValdusumodificacion = dr.GetOrdinal(this.Valdusumodificacion);
            if (!dr.IsDBNull(iValdusumodificacion)) entity.Valdusumodificacion = dr.GetString(iValdusumodificacion);

            int iValdfecmodificacion = dr.GetOrdinal(this.Valdfecmodificacion);
            if (!dr.IsDBNull(iValdfecmodificacion)) entity.Valdfecmodificacion = dr.GetDateTime(iValdfecmodificacion);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));


            return entity;
        }


        #region Mapeo de Campos
        //Valorizacion
        public string Valocodi = "VALOCODI";
        public string Valofecha = "VALOFECHA";
        public string Valomr = "VALOMR";
        public string Valopreciopotencia = "VALOPRECIOPOTENCIA";
        public string Valodemandacoes = "VALODEMANDACOES";
        public string Valofactorreparto = "VALOFACTORREPARTO";
        public string Valoporcentajeperdida = "VALOPORCENTAJEPERDIDA";
        public string Valofrectotal = "VALOFRECTOTAL";
        public string Valootrosequipos = "VALOOTROSEQUIPOS";
        public string Valocostofuerabanda = "VALOCOSTOFUERABANDA";
        public string Valoco = "VALOCO";
        public string Valora = "VALORA";
        public string Valoofmax = "VALOOFMAX";
        public string Valocompcostosoper = "VALOCOMPCOSTOSOPER";
        public string Valocomptermrt = "VALOCOMPTERMRT";
        public string Valoestado = "VALOESTADO";
        public string Valousucreacion = "VALOUSUCREACION";
        public string Valofeccreacion = "VALOFECCREACION";
        public string Valousumodificacion = "VALOUSUMODIFICACION";
        public string Valofecmodificacion = "VALOFECMODIFICACION";
        //ValorizacionDetalle
        public string Valdcodi = "VALDCODI";
        public string Emprcodi = "EMPRCODI";
        public string Valdretiro = "VALDRETIRO";
        public string Valdentrega = "VALDENTREGA";
        public string Valdpfirremun = "VALDPFIRREMUN";
        public string Valddemandacoincidente = "VALDDEMANDACOINCIDENTE";
        public string Valdmoncapacidad = "VALDMONCAPACIDAD";
        public string Valdpeajeuni = "VALDPEAJEUNI";
        public string Valdfactorp = "VALDFACTORP";
        public string Valdpagoio = "VALDPAGOIO";
        public string Valdpagosc = "VALDPAGOSC";
        public string Valdfpgm = "VALDFPGM";
        public string Valdmcio = "VALDMCIO";        
        public string Valdpdsc = "VALDPDSC";
        public string Valdcargoconsumo = "VALDCARGOCONSUMO";
        public string Valdaportesadicional = "VALDAPORTESADICIONAL";
        public string Valdusucreacion = "VALDUSUCREACION";
        public string Valdfeccreacion = "VALDFECCREACION";
        public string Valdusumodificacion = "VALDUSUMODIFICACION";
        public string Valdfecmodificacion = "VALDFECMODIFICACION";

        public string Emprnomb = "EMPRNOMB";


        public string SqlListByFilter
        {
            get { return GetSqlXml("ListByFilter"); }
        }

        public string SqlListPagedByFilter
        {
            get { return GetSqlXml("ListPagedByFilter"); }
        }

        public string SqlListMontoCalculadoPorMes
        {
            get { return GetSqlXml("ListMontoCalculadoPorMes"); }
        }

        #endregion
    }
}
