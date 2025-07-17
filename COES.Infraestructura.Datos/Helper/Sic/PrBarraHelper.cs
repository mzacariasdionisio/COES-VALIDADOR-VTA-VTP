// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 27/10/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{

    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_BARRA.
    /// </summary>
    public class PrBarraHelper : HelperBase
    {
        #region CAMPOS: Variables de la clase.

        public string Barrcodi = "BARRCODI";

        public string Barrnombre = "BARRNOMBRE";

        public string Barrtension = "BARRTENSION";

        public string Barrpuntosuministrorer = "BARRPUNTOSUMINISTRORER";

        public string Barrbarrabgr = "BARRBARRABGR";

        public string Barrestado = "BARRESTADO";

        public string Barrflagbarratransferencia = "BARRFLAGBARRATRANSFERENCIA";

        public string Areacodi = "AREACODI";

        public string Barrflagdesbalance = "BARRFLAGDESBALANCE";

        public string Barrbarratransferencia = "BARRBARRATRANSFERENCIA";

        public string Barrusername = "BARRUSERNAME";

        public string Barrfecins = "BARRFECINS";

        public string Barrfecact = "BARRFECACT";

        public string Osinergcodi = "OSINERGCODI";

        public string Barrbarratransf = "BARRBARRATRANSF";    

        //- alpha.HDT - 07/08/2017: Cambio para atender el requerimiento. 
        public string Barrflagbarracompensa = "BARRFLAGBARRACOMPENSA";  
        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public PrBarraHelper()
            : base(Consultas.PrBarraSql)
        {
        }

        #endregion

        #region METODOS: Metodos de la clase.

        public PrBarraDTO Create(IDataReader dr)
        {

            PrBarraDTO entity = new PrBarraDTO();

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iBarrnombre = dr.GetOrdinal(this.Barrnombre);
            if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

            int iBarrtension = dr.GetOrdinal(this.Barrtension);
            if (!dr.IsDBNull(iBarrtension)) entity.Barrtension = dr.GetString(iBarrtension);

            int iBarrpuntosuministrorer = dr.GetOrdinal(this.Barrpuntosuministrorer);
            if (!dr.IsDBNull(iBarrpuntosuministrorer)) entity.Barrpuntosuministrorer = dr.GetString(iBarrpuntosuministrorer);

            int iBarrbarrabgr = dr.GetOrdinal(this.Barrbarrabgr);
            if (!dr.IsDBNull(iBarrbarrabgr)) entity.Barrbarrabgr = dr.GetString(iBarrbarrabgr);

            int iBarrestado = dr.GetOrdinal(this.Barrestado);
            if (!dr.IsDBNull(iBarrestado)) entity.Barrestado = dr.GetString(iBarrestado);

            int iBarrflagbarratransferencia = dr.GetOrdinal(this.Barrflagbarratransferencia);
            if (!dr.IsDBNull(iBarrflagbarratransferencia)) entity.Barrflagbarratransferencia = dr.GetString(iBarrflagbarratransferencia);

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iBarrflagdesbalance = dr.GetOrdinal(this.Barrflagdesbalance);
            if (!dr.IsDBNull(iBarrflagdesbalance)) entity.Barrflagdesbalance = dr.GetString(iBarrflagdesbalance);

            int iBarrbarratransferencia = dr.GetOrdinal(this.Barrbarratransferencia);
            if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

            int iBarrusername = dr.GetOrdinal(this.Barrusername);
            if (!dr.IsDBNull(iBarrusername)) entity.Barrusername = dr.GetString(iBarrusername);

            int iBarrfecins = dr.GetOrdinal(this.Barrfecins);
            if (!dr.IsDBNull(iBarrfecins)) entity.Barrfecins = dr.GetDateTime(iBarrfecins);

            int iBarrfecact = dr.GetOrdinal(this.Barrfecact);
            if (!dr.IsDBNull(iBarrfecact)) entity.Barrfecact = dr.GetDateTime(iBarrfecact);

            int iOsinergcodi = dr.GetOrdinal(this.Osinergcodi);
            if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

            int iBarrflagbarracompensa = dr.GetOrdinal(this.Barrbarratransf);
            if (!dr.IsDBNull(iBarrflagbarracompensa)) entity.Barrflagbarracompensa = Convert.ToInt32(dr.GetValue(iBarrflagbarracompensa));
            
            return entity;
        }

        #endregion        


        //- alpha.JDEL - Inicio 03/11/2016: Cambio para atender el requerimiento.
        public string SqlGetByCodOsinergmin
        {
            get { return base.GetSqlXml("GetByCodOsinergmin"); }
        }
        //- JDEL Fin
    }

}
