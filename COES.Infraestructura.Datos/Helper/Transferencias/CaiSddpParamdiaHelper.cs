using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_SDDP_PARAMDIA
    /// </summary>
    /// 

    public class CaiSddpParamdiaHelper : HelperBase
        {
            public CaiSddpParamdiaHelper(): base(Consultas.CaiSddpParamdiaSql)
            {
            }

            public CaiSddpParamdiaDTO Create(IDataReader dr)
            {
                CaiSddpParamdiaDTO entity = new CaiSddpParamdiaDTO();

                int iSddppdcodi = dr.GetOrdinal(this.Sddppdcodi);
                if (!dr.IsDBNull(iSddppdcodi)) entity.Sddppdcodi = Convert.ToInt32(dr.GetValue(iSddppdcodi));

                int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
                if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

                int iSddppddia = dr.GetOrdinal(this.Sddppddia);
                if (!dr.IsDBNull(iSddppddia)) entity.Sddppddia = dr.GetDateTime(iSddppddia);

                int iSddppdlaboral = dr.GetOrdinal(this.Sddppdlaboral);
                if (!dr.IsDBNull(iSddppdlaboral)) entity.Sddppdlaboral = Convert.ToInt32(dr.GetValue(iSddppdlaboral));

                int iSddppdusucreacion = dr.GetOrdinal(this.Sddppdusucreacion);
                if (!dr.IsDBNull(iSddppdusucreacion)) entity.Sddppdusucreacion = dr.GetString(iSddppdusucreacion);

                int iSddppdfeccreacion = dr.GetOrdinal(this.Sddppdfeccreacion);
                if (!dr.IsDBNull(iSddppdfeccreacion)) entity.Sddppdfeccreacion = dr.GetDateTime(iSddppdfeccreacion);

                return entity;
            }


            #region Mapeo de Campos

            public string Sddppdcodi = "SDDPPDCODI";
            public string Caiajcodi = "CAIAJCODI";
            public string Sddppddia = "SDDPPDDIA";
            public string Sddppdlaboral = "SDDPPDLABORAL";
            public string Sddppdusucreacion = "SDDPPDUSUCREACION";
            public string Sddppdfeccreacion = "SDDPPDFECCREACION";
           

            #endregion

            public string GetByDiaCaiSddpParamdia
        {
            get { return base.GetSqlXml("GetByDiaCaiSddpParamdia"); }

        }


        
        }
    }

