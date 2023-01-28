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
        public string name;
    }

    [Serializable]
    public class OptionDTO
    {
        public int id;
        public int textId;
        public int[] nextLineId;
        public string nextDialogPath;
        public RequiredAnswerDTO[] requiredAnswers;
        public string name;
    }

    [Serializable]
    public class RequiredAnswerDTO
    {
        public int dialogLinenId;
        public int optionId;
    }

    [Serializable]
    public class TextPath
    {
        public string localizationType;
        public string path;
    }
}
