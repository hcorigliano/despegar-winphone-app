using Despegar.Core.Business.Flight.BookingCompletePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despegar.Core.Business.Flight.BookingCompletePostResponse
{
    public class RiskQuestion
    {
        public string id { get; set; }
        public string order { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string mandatory { get; set; }
        public string free_text { get; set; }
        public FreeTextDescription free_text_description { get; set; }
        public List<Answers> answers { get; set; }
        public RiskAnswer risk_answer { get; set; }

        private Answers selectedChoice;
        public Answers SelectedChoice { get { return selectedChoice; } 
            set 
             {
                selectedChoice = value; 
                risk_answer.answer_id = value.id;
                risk_answer.answer_text = value.text;
            }
        }

        public RiskQuestion()
        {
            this.risk_answer = new RiskAnswer();
        }
    }
}
