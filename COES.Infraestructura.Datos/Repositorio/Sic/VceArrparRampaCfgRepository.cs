// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 06/04/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{

    public class VceArrparRampaCfgRepository : RepositoryBase, IVceArrparRampaCfgRepository
    {

        VceArrparRampaCfgHelper helper = new VceArrparRampaCfgHelper();

        public VceArrparRampaCfgRepository(string strConn)
            : base(strConn)
        {
        }

        public VceArrparRampaCfgDTO ObtenerRangoInferiorPar(int grupocodi, string aptopsubtipo
                                                       , decimal apespcargafinal)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRangoInferiorPar);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Apstocodi, DbType.String, aptopsubtipo);
            dbProvider.AddInParameter(command, helper.Aprampotenciabruta, DbType.Decimal, apespcargafinal);

            VceArrparRampaCfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    break;
                }
            }

            return entity;
        }

        public VceArrparRampaCfgDTO ObtenerRangoSuperiorPar(int grupocodi, string aptopsubtipo
                                                       , decimal apespcargafinal)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRangoSuperiorPar);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Apstocodi, DbType.String, aptopsubtipo);
            dbProvider.AddInParameter(command, helper.Aprampotenciabruta, DbType.Decimal, apespcargafinal);

            VceArrparRampaCfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    break;
                }
            }

            return entity;
        }


        public VceArrparRampaCfgDTO ObtenerRangoInferiorArr(int grupocodi, string aptopsubtipo
                                                          , decimal apespcargafinal)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRangoInferiorArr);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Apstocodi, DbType.String, aptopsubtipo);
            dbProvider.AddInParameter(command, helper.Aprampotenciabruta, DbType.Decimal, apespcargafinal);

            VceArrparRampaCfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    break;
                }
            }

            return entity;
        }

        public VceArrparRampaCfgDTO ObtenerRangoSuperiorArr(int grupocodi, string aptopsubtipo
                                                          , decimal apespcargafinal)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRangoSuperiorArr);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Apstocodi, DbType.String, aptopsubtipo);
            dbProvider.AddInParameter(command, helper.Aprampotenciabruta, DbType.Decimal, apespcargafinal);

            VceArrparRampaCfgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    break;
                }
            }

            return entity;
        }

    }

}
