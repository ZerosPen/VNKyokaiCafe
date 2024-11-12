using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DL_SpeakerData
    {
        public string name, castName;

        public string displayName => castName != string.Empty ? castName : name;

        public Vector2 castPosition;
        public List<(int Layer, string expression)> CastExpresion { get; set; }

        private const string NameCast_Id = " as ";
        private const string PositionCast_Id = " at ";
        private const string ExpressionCast_Id = " [";
        private const char AxisDeLimiter = ':';
        private const char ExpressionJoiner_Id = ',';
        private const char ExpressionDeLimiter_Id = ':';

        public DL_SpeakerData(string rawSpeaker)
        {
            string pattern = $"{NameCast_Id}|{PositionCast_Id}| {ExpressionCast_Id.Insert(ExpressionCast_Id.Length - 1, @"\")}";
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            //Polulate this data to avoid null reference to values
            castName = "";
            castPosition = Vector2.zero;
            CastExpresion = new List<(int Layer, string expression)>();

            //if no matches, then this entire line is the speaker name
            if (matches.Count == 0)
            {
                name = rawSpeaker;

                return;
            }


            //isolate data from casting data
            int index = matches[0].Index;

            name = rawSpeaker.Substring(0, index);

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];

                int startIndex = 0;
                int endIndex = 0;

                if (match.Value == NameCast_Id)
                {
                    startIndex = match.Index + NameCast_Id.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }
                else if (match.Value == PositionCast_Id)
                {
                    startIndex = match.Index + PositionCast_Id.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    string castPos = rawSpeaker.Substring(startIndex, endIndex - startIndex);

                    string[] axis = castPos.Split(AxisDeLimiter, System.StringSplitOptions.RemoveEmptyEntries);

                    float.TryParse(axis[0], out castPosition.x);

                    if (axis.Length > 1)
                    {
                        float.TryParse(axis[1], out castPosition.y);
                    }
                }
                else if (match.Value == ExpressionCast_Id)
                {
                    startIndex = match.Index + ExpressionCast_Id.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    string castExp = rawSpeaker.Substring(startIndex, endIndex - (startIndex + 1));

                    CastExpresion = castExp.Split(ExpressionJoiner_Id)
                        .Select(x =>
                        {
                            var parts = x.Trim().Split(ExpressionDeLimiter_Id);
                            return (int.Parse(parts[00]), parts[1]);
                        }).ToList();
                }
            }

        }
    }
}