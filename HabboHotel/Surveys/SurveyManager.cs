using Galaxy.Database.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Data;

namespace Galaxy.HabboHotel.Surveys
{
    class SurveyManager
    {
        private readonly ConcurrentDictionary<int, Question> _questions;

        public SurveyManager()
        {
            this._questions = new ConcurrentDictionary<int, Question>();

            this.Init();
        }

        public void Init()
        {
            DataTable Table = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `questions`");
                Table = dbClient.getTable();
            }

            if (Table != null)
            {
                foreach (DataRow Row in Table.Rows)
                {
                    if (!this._questions.ContainsKey(Convert.ToInt32(Row["id"])))
                    this._questions.TryAdd(Convert.ToInt32(Row["id"]), new Question());
                }
            }
        }

        public bool TryGetQuestion(int QuestionId, out Question Question)
        {
            return this._questions.TryGetValue(QuestionId, out Question);
        }
    }
}
