using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la vista vw_eq_central_generacion
    /// </summary>
    public class CentralGeneracionHelper : HelperBase
    {
        public CentralGeneracionHelper()
            : base(Consultas.CentralGeneracionSql)
        {
        }

        public CentralGeneracionDTO Create(IDataReader dr)
        {
            CentralGeneracionDTO entity = new CentralGeneracionDTO();

            int iCENTGENECODI = dr.GetOrdinal(this.CENTGENECODI);
            if (!dr.IsDBNull(iCENTGENECODI)) entity.CentGeneCodi = dr.GetInt32(iCENTGENECODI);

            int iCENTGENENOMBRE = dr.GetOrdinal(this.CENTGENENOMBRE);
            if (!dr.IsDBNull(iCENTGENENOMBRE)) entity.CentGeneNombre = dr.GetString(iCENTGENENOMBRE);

            return entity;
        }

        public EqEquipoDTO CreateEquipo(IDataReader dr)
        {
            EqEquipoDTO entity = new EqEquipoDTO();

            int iEQUICODI = dr.GetOrdinal(this.EQUICODI);
            if (!dr.IsDBNull(iEQUICODI)) entity.Equicodi = dr.GetInt32(iEQUICODI);

            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.Emprcodi = dr.GetInt32(iEMPRCODI);

            int iGRUPOCODI = dr.GetOrdinal(this.GRUPOCODI);
            if (!dr.IsDBNull(iGRUPOCODI)) entity.Grupocodi = dr.GetInt32(iGRUPOCODI);

            int iELECODI = dr.GetOrdinal(this.ELECODI);
            if (!dr.IsDBNull(iELECODI)) entity.Elecodi = dr.GetInt32(iELECODI);

            int iAREACODI = dr.GetOrdinal(this.AREACODI);
            if (!dr.IsDBNull(iAREACODI)) entity.Areacodi = dr.GetInt32(iAREACODI);

            int iFAMCODI = dr.GetOrdinal(this.FAMCODI);
            if (!dr.IsDBNull(iFAMCODI)) entity.Famcodi = dr.GetInt32(iFAMCODI);

            int iEQUIABREV = dr.GetOrdinal(this.EQUIABREV);
            if (!dr.IsDBNull(iEQUIABREV)) entity.Equiabrev = dr.GetString(iEQUIABREV);

            int iEQUINOMB = dr.GetOrdinal(this.EQUINOMB);
            if (!dr.IsDBNull(iEQUINOMB)) entity.Equinomb = dr.GetString(iEQUINOMB);

            int iEQUIABREV2 = dr.GetOrdinal(this.EQUIABREV2);
            if (!dr.IsDBNull(iEQUIABREV2)) entity.Equiabrev2 = dr.GetString(iEQUIABREV2);

            int iEQUITENSION = dr.GetOrdinal(this.EQUITENSION);
            if (!dr.IsDBNull(iEQUITENSION)) entity.Equitension = dr.GetDecimal(iEQUITENSION);

            int iEQUIPADRE = dr.GetOrdinal(this.EQUIPADRE);
            if (!dr.IsDBNull(iEQUIPADRE)) entity.Equipadre = dr.GetInt32(iEQUIPADRE);

            int iEQUIPOT = dr.GetOrdinal(this.EQUIPOT);
            if (!dr.IsDBNull(iEQUIPOT)) entity.Equipot = dr.GetDecimal(iEQUIPOT);

            int iLASTUSER = dr.GetOrdinal(this.LASTUSER);
            if (!dr.IsDBNull(iLASTUSER)) entity.Lastuser = dr.GetString(iLASTUSER);

            int iLASTDATE = dr.GetOrdinal(this.LASTDATE);
            if (!dr.IsDBNull(iLASTDATE)) entity.Lastdate = dr.GetDateTime(iLASTDATE);

            int iECODIGO = dr.GetOrdinal(this.ECODIGO);
            if (!dr.IsDBNull(iECODIGO)) entity.Ecodigo = dr.GetString(iECODIGO);

            int iEQUIESTADO = dr.GetOrdinal(this.EQUIESTADO);
            if (!dr.IsDBNull(iEQUIESTADO)) entity.Equiestado = dr.GetString(iEQUIESTADO);

            int iOSIGRUPOCODI = dr.GetOrdinal(this.OSIGRUPOCODI);
            if (!dr.IsDBNull(iOSIGRUPOCODI)) entity.Osigrupocodi = dr.GetString(iOSIGRUPOCODI);

            int iLASTCODI = dr.GetOrdinal(this.LASTCODI);
            if (!dr.IsDBNull(iLASTCODI)) entity.Lastcodi = dr.GetInt32(iLASTCODI);

            int iEQUIFECHINIOPCOM = dr.GetOrdinal(this.EQUIFECHINIOPCOM);
            if (!dr.IsDBNull(iEQUIFECHINIOPCOM)) entity.Equifechiniopcom = dr.GetDateTime(iEQUIFECHINIOPCOM);

            int iEQUIFECHFINOPCOM = dr.GetOrdinal(this.EQUIFECHFINOPCOM);
            if (!dr.IsDBNull(iEQUIFECHFINOPCOM)) entity.Equifechfinopcom = dr.GetDateTime(iEQUIFECHFINOPCOM);

            int iEQUIMANIOBRA = dr.GetOrdinal(this.EQUIMANIOBRA);
            if (!dr.IsDBNull(iEQUIMANIOBRA)) entity.EquiManiobra = dr.GetString(iEQUIMANIOBRA);

            int iUSUARIOUPDATE = dr.GetOrdinal(this.USUARIOUPDATE);
            if (!dr.IsDBNull(iUSUARIOUPDATE)) entity.UsuarioUpdate = dr.GetString(iUSUARIOUPDATE);

            int iFECHAUPDATE = dr.GetOrdinal(this.FECHAUPDATE);
            if (!dr.IsDBNull(iFECHAUPDATE)) entity.FechaUpdate = dr.GetDateTime(iFECHAUPDATE);

            //int iEQUIMANTRELEV = dr.GetOrdinal(this.EQUIMANTRELEV);
            //if (!dr.IsDBNull(iEQUIMANTRELEV)) entity.EquiMantRelev = dr.GetString(iEQUIMANTRELEV);

            int iOSINERGCODI = dr.GetOrdinal(this.OSINERGCODI);
            if (!dr.IsDBNull(iOSINERGCODI)) entity.Osinergcodi = dr.GetString(iOSINERGCODI);

            return entity;
        }

        #region Mapeo de Campos
        public string CENTGENECODI = "EQUICODI";
        public string CENTGENENOMBRE = "EQUINOMB";

        //mapeo de campos de EQ_EQUIPO

        public string EQUICODI = "EQUICODI";
        public string EMPRCODI = "EMPRCODI";
        public string GRUPOCODI = "GRUPOCODI";
        public string ELECODI = "ELECODI";
        public string AREACODI = "AREACODI";
        public string FAMCODI = "FAMCODI";
        public string EQUIABREV = "EQUIABREV";
        public string EQUINOMB = "EQUINOMB";
        public string EQUIABREV2 = "EQUIABREV2";
        public string EQUITENSION = "EQUITENSION";
        public string EQUIPADRE = "EQUIPADRE";
        public string EQUIPOT = "EQUIPOT";
        public string LASTUSER = "LASTUSER";
        public string LASTDATE = "LASTDATE";
        public string ECODIGO = "ECODIGO";
        public string EQUIESTADO = "EQUIESTADO";
        public string OSIGRUPOCODI = "OSIGRUPOCODI";
        public string LASTCODI = "LASTCODI";
        public string EQUIFECHINIOPCOM = "EQUIFECHINIOPCOM";
        public string EQUIFECHFINOPCOM = "EQUIFECHFINOPCOM";
        public string EQUIMANIOBRA = "EQUIMANIOBRA";
        public string USUARIOUPDATE = "USUARIOUPDATE";
        public string FECHAUPDATE = "FECHAUPDATE";
        public string EQUIMANTRELEV = "EQUIMANTRELEV";
        public string OSINERGCODI = "OSINERGCODI";
        //ADICIONALES
        public string VCRECACODI = "VCRECACODI";
        #endregion

        public string SqlUnidad
        {
            get { return base.GetSqlXml("Unidad"); }
        }

        public string SqlUnidadCentral
        {
            get { return base.GetSqlXml("UnidadCentral"); }
        }

        public string SqlListaInterCodEnt
        {
            get { return base.GetSqlXml("ListaInterCodEnt"); }
        }

        #region PrimasRER.2023
        public string SqlListaCentralByEmpresa
        {
            get { return base.GetSqlXml("ListaCentralByEmpresa"); }
        }
        public string SqlListaCentralUnidadByEmpresa
        {
            get { return base.GetSqlXml("ListaCentralUnidadByEmpresa"); }
        }
        #endregion
        public string SqlListaInterCodInfoBase
        {
            get { return base.GetSqlXml("ListaInterCodInfoBase"); }
        }

        public string SqlListInfoBase
        {
            get { return base.GetSqlXml("ListInfoBase"); }
        }

        public string SqlGetByCentGeneNombre
        {
            get { return base.GetSqlXml("GetByCentGeneNombre"); }
        }

        public string SqlListEmpresaCentralGeneracion
        {
            get { return base.GetSqlXml("ListEmpresaCentralGeneracion"); }
        }

        public string SqlGetByCentGeneNombVsEN
        {
            get { return base.GetSqlXml("GetByCentGeneNombVsEN"); }
        }

        public string SqlGetByCentGeneTermoelectricaNombre
        {
            get { return base.GetSqlXml("GetByCentGeneTermoelectricaNombre"); }
        }

        public string SqlGetByCentGeneNombreEquipo
        {
            get { return base.GetSqlXml("GetByCentGeneNombreEquipo"); }
        }

        public string SqlGetByCentGeneUniNombreEquipo
        {
            get { return base.GetSqlXml("GetByCentGeneUniNombreEquipo"); }
        }

        public string SqlGetByCentGeneNombreEquipoCenUni
        {
            get { return base.GetSqlXml("GetByCentGeneNombreEquipoUniCen"); }
        }

        public string SqlGetByEquicodi
        {
            get { return base.GetSqlXml("GetByEquicodi"); }
        }

        public string SqlGetByEquiNomb
        {
            get { return base.GetSqlXml("GetByEquiNomb"); }
        }

        public string SqlGetByEquiPadre
        {
            get { return base.GetSqlXml("GetByEquiPadre"); }
        }    
        
        public string SqlListTemporal
        {
            get { return base.GetSqlXml("ListTemporal"); }
        }

        #region SIOSEIN-PRIE-2021
        public string SqlListarEquiposPorEmpresa
        {
            get { return base.GetSqlXml("ListarEquiposPorEmpresa"); }
        }
        #endregion

    }
}
