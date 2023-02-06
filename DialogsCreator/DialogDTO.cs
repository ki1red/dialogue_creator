using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace DialogsCreator
{
    [Serializable]
    public class DialogDTO
    {
        public DialogLineDTO[] dialogLines;
        public int firstLineId;
        public int lastLineId;
        public TextPathDTO[] textPaths;
        public string nextDialogPath;

        public DialogDTO(DialogLineDTO[] dialogLines, int firstLineId, int lastLineId, TextPathDTO[] textPaths, string nextDialogPath)
        {
            this.dialogLines = dialogLines;
            this.firstLineId = firstLineId;
            this.lastLineId = lastLineId;
            this.textPaths = textPaths;
            this.nextDialogPath = nextDialogPath;
        }

        public DialogDTO()
        {
            this.dialogLines = new DialogLineDTO[0];
            this.firstLineId = -1;
            this.lastLineId = -1;
            this.textPaths = new TextPathDTO[0];
            this.nextDialogPath = null;
        }
    }

    [Serializable]
    public class DialogLineDTO
    {
        public int id;
        public int textId;
        public OptionDTO[] options;
        public int nextLineId;
        public string pathToImage;
        public string pathToSound;

        public DialogLineDTO(int id, int textId, OptionDTO[] options, int nextLineId, string pathToImage, string pathToSound)
        {
            this.id = id;
            this.textId = textId;
            this.options = options;
            this.nextLineId = nextLineId;            this.pathToImage = pathToImage;
            this.pathToSound = pathToSound;
        }

        public DialogLineDTO()
        {
            this.id = -1;
            this.textId = -1;
            this.options = new OptionDTO[0];
            this.nextLineId = -1;
            this.pathToImage = null;
            this.pathToSound = null;
        }
    }

    [Serializable]
    public class OptionDTO
    {
        public int id;
        public int textId;
        public RequiredAnswerDTO[] requiredAnswers;
        public int nextLineId;

        public OptionDTO(int id, int textId, RequiredAnswerDTO[] requiredAnswers, int nextLineId)
        {
            this.id = id;
            this.textId = textId;
            this.requiredAnswers = requiredAnswers;
            this.nextLineId = nextLineId;
        }

        public OptionDTO()
        {
            this.id = -1;
            this.textId = -1;
            this.requiredAnswers = new RequiredAnswerDTO[0];
            this.nextLineId = -1;
        }
    }

    [Serializable]
    public class RequiredAnswerDTO
    {
        public int dialogLineId;
        public int optionId;

        public RequiredAnswerDTO(int dialogLineId, int optionId)
        {
            this.dialogLineId = dialogLineId;
            this.optionId = optionId;
        }

        public RequiredAnswerDTO()
        {
            this.dialogLineId = -1;
            this.optionId = -1;
        }
    }

    [Serializable]
    public class TextPathDTO
    {
        public string language;
        public string path;

        public TextPathDTO(string language, string path)
        {
            this.language = language;
            this.path = path;
        }

        public TextPathDTO()
        {
            this.language = null;
            this.path = null;
        }
    }
}