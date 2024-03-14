using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Choice1 { get; set; }
        public string Choice2 { get; set; }
        public string Choice3 { get; set; }
        public string Choice4 { get; set; }
        public int CorrectChoiceIndex { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }

}
