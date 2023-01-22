using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    [Serializable]
    public class DialogDTO
    {
        public DialogLineDTO[] dialogLines;
        public TextPath[] TextPaths;
    }

    [Serializable]
    public class DialogLineDTO
    {
        public int id;
        public int textId;
        public OptionDTO[] options;
        public RequiredAnswerDTO[] requiredAnswers;
        public int[] nextLineId;
        public string nextDialogPath;
    }

    [Serializable]
    public class OptionDTO
    {
        public int id;
        public int textId;
        public int[] nextLineId;
        public string nextDialogPath;
        public RequiredAnswerDTO[] requiredAnswers;
    }

    [Serializable]
    public class RequiredAnswerDTO
    {
        public int questionId;
        public int answerId;
    }

    [Serializable]
    public class TextPath
    {
        public string localization;
        public string path;
    }
}
