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

    public class PrBarraRepository : RepositoryBase, IPrBarraRepository
    {

        PrBarraHelper helper = new PrBarraHelper();

        public PrBarraRepository(string strConn)
            : base(strConn)
        {
        }

        public List<PrBarraDTO> List()
        {
            List<PrBarraDTO> entities = new List<PrBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }


        //- alpha.JDEL - Inicio 03/11/2016: Cambio para atender el requerimiento.

        public PrBarraDTO GetByCodOsinergmin(string codOsinergmin)
        {
            string queryString = string.Format(helper.SqlGetByCodOsinergmin, codOsinergmin);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            PrBarraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new PrBarraDTO();

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);
                }
            }
            return entity;
        }

        //- JDEL Fin

        //- alpha.HDT - 08/07/2017: Cambio para atender el requerimiento. 
        public int InsertarBarra(PrBarraDTO bar)
        {
            var comando = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            var resultado = dbProvider.ExecuteScalar(comando);
            int barrCodi = (resultado != null ? Convert.ToInt32(resultado) : 1);

            comando = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(comando, helper.Barrcodi, DbType.Int32, barrCodi);
            dbProvider.AddInParameter(comando, helper.Barrnombre, DbType.String, bar.Barrnombre);
            dbProvider.AddInParameter(comando, helper.Barrtension, DbType.String, bar.Barrtension);
            dbProvider.AddInParameter(comando, helper.Barrpuntosuministrorer, DbType.String, bar.Barrpuntosuministrorer);
            dbProvider.AddInParameter(comando, helper.Barrbarrabgr, DbType.String, bar.Barrbarrabgr);
            dbProvider.AddInParameter(comando, helper.Barrestado, DbType.String, bar.Barrestado);
            dbProvider.AddInParameter(comando, helper.Barrflagbarratransferencia, DbType.String, bar.Barrflagbarratransferencia);
            dbProvider.AddInParameter(comando, helper.Areacodi, DbType.Int32, bar.Areacodi);
            dbProvider.AddInParameter(comando, helper.Barrflagdesbalance, DbType.String, bar.Barrflagdesbalance);
            dbProvider.AddInParameter(comando, helper.Barrbarratransferencia, DbType.String, bar.Barrbarratransferencia);
            dbProvider.AddInParameter(comando, helper.Barrusername, DbType.String, bar.Barrusername);
            dbProvider.AddInParameter(comando, helper.Barrfecins, DbType.DateTime, bar.Barrfecins);
            dbProvider.AddInParameter(comando, helper.Barrfecact, DbType.DateTime, bar.Barrfecact);
            dbProvider.AddInParameter(comando, helper.Osinergcodi, DbType.String, bar.Osinergcodi);
            dbProvider.AddInParameter(comando, helper.Barrbarratransf, DbType.Int32, bar.Barrbarratransf);
            dbProvider.AddInParameter(comando, helper.Barrflagbarracompensa, DbType.Int32, bar.Barrflagbarracompensa);

            try
            {
                dbProvider.ExecuteNonQuery(comando);
            }
            catch (Exception ex)
            {   
                throw ex;
            }

            return barrCodi;
        }

    }

}
