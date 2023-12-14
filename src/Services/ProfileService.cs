using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;

namespace sodoff.Services
{
    public class ProfileService
    {
        private readonly DBContext ctx;

        public ProfileService(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public bool SetAnswer(Viking viking, int qId, int aId)
        {
            // check if answer is in the database already, remove it if it already is
            Model.ProfileAnswer? existingAnswer = ctx.ProfileAnswers.Where(a => a.AnswerID == aId).FirstOrDefault(q => q.QuestionID == qId);
            if(existingAnswer != null) ctx.ProfileAnswers.Remove(existingAnswer);

            // create an answer and store it in database

            Model.ProfileAnswer answer = new Model.ProfileAnswer
            {
                VikingId = viking.Id,
                AnswerID = aId,
                QuestionID = qId,
            };

            viking.ProfileAnswers.Add(answer);
            ctx.SaveChanges();

            return true;
        }

        public ProfileUserAnswer[] GetUserAnswers(Viking viking)
        {
            // create a profile user answer based on each answer on viking

            List<ProfileUserAnswer> userAnswers = new List<ProfileUserAnswer>();
            foreach(Model.ProfileAnswer answer in viking.ProfileAnswers)
            {
                ProfileUserAnswer userAnswer = new ProfileUserAnswer
                {
                    AnswerID = answer.AnswerID,
                    QuestionID = answer.QuestionID
                };
                userAnswers.Add(userAnswer);
            }

            return userAnswers.ToArray();
        }

        public ProfileUserAnswer GetUserAnswerFromQuestionId(Viking viking, int qId)
        {
            // check if answer exists
            Model.ProfileAnswer profileAnswer = viking.ProfileAnswers.FirstOrDefault(e => e.QuestionID == qId);

            if(profileAnswer != null)
            {
                ProfileUserAnswer profileUserAnswer = new ProfileUserAnswer
                {
                    QuestionID = profileAnswer.QuestionID,
                    AnswerID = profileAnswer.AnswerID
                };
                return profileUserAnswer;
            }

            return null;
        }

        public ProfileQuestion GetQuestionFromAnswerId(int aId)
        {
            ProfileQuestionData questionData = XmlUtil.DeserializeXml<ProfileQuestionData>(XmlUtil.ReadResourceXmlString("questiondata"));

            List<Schema.ProfileAnswer> allAnswersFromData = new List<Schema.ProfileAnswer>();
            List<ProfileQuestion> allQuestionsFromData = new List<ProfileQuestion>();

            foreach(var list in questionData.Lists)
            {
                foreach(var question in list.Questions)
                {
                    allQuestionsFromData.Add(question);
                    foreach(var answer in question.Answers)
                    {
                        allAnswersFromData.Add(answer);
                    }
                }
            }

            Schema.ProfileAnswer profileAnswer = allAnswersFromData.FirstOrDefault(e => e.ID == aId);

            if (profileAnswer != null)
            {
                ProfileQuestion questionFromAnswer = allQuestionsFromData.FirstOrDefault(e => e.ID == profileAnswer.QuestionID);
                if (questionFromAnswer != null) return questionFromAnswer;
                else return null!;
            }

            return null!;
        }
    }
}
