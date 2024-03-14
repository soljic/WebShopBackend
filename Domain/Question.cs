using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string[] Choices { get; set; }
        public int CorrectChoiceIndex { get; set; }
        public Quiz Quiz { get; set; }
    }
}
