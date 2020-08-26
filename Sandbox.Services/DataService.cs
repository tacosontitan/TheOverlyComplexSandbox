using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Services {
    internal class DataService {

        #region Singleton Setup

        private static readonly object instanceLock = new object();
        private static DataService instance;
        public static DataService Instance {
            get {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new DataService();

                return instance;
            }
        }

        #endregion

        #region Properties

        private string ConnectionString => ConfigurationManager.ConnectionStrings["Sandbox"].ConnectionString;

        #endregion

        #region Constructors

        private DataService() { }

        #endregion

        #region Public Methods

        public void ExecuteStoredProcedureNonQuery(string storedProcedureName, params SqlParameter[] parameters) {
            using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn)) {
                    conn.Open();

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public DataTable ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters) {
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn)) {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd)) {
                        conn.Open();
                        sda.Fill(result);
                    }
                }
            }

            return result;
        }

        #endregion

    }
}