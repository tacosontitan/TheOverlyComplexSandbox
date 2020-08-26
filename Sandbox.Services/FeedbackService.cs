using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Services {
    public class FeedbackService {

        #region Singleton Setup

        private static readonly object instanceLock = new object();
        private static FeedbackService instance;
        public static FeedbackService Instance {
            get {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new FeedbackService();

                return instance;
            }
        }

        #endregion

        #region Public Methods

        public void Submit(string displayName, string feedback) {
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = "Captain Anonymous";

            SqlParameter displayNameParameter = new SqlParameter("DisplayName", displayName) {
                DbType = DbType.String,
                SqlDbType = SqlDbType.NVarChar
            };
            SqlParameter feedbackParameter = new SqlParameter("Feedback", feedback) {
                DbType = DbType.String,
                SqlDbType = SqlDbType.NVarChar
            };
            DataService.Instance.ExecuteStoredProcedureNonQuery("sSubmitFeedback", displayNameParameter, feedbackParameter);
        }
        public List<FeedbackMessage> GetFeedback() {
            List<FeedbackMessage> results = new List<FeedbackMessage>();
            DataTable feedback = DataService.Instance.ExecuteStoredProcedure("sFetchFeedback");
            foreach (DataRow row in feedback.Rows)
                results.Add(new FeedbackMessage(row["DisplayName"].ToString(), row["Message"].ToString()));
            return results;
        }

        #endregion

    }
}