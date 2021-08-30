using UnityEngine;

namespace Data 
{
    public class WordProperties
    {
        public string word;
        public Vector2Int startPosition;
        public WordPlacement.Direction currentDirection;
        private int score;
        public int Score { get => score; set => score = value; }
        public WordProperties(string word, Vector2Int startPosition, WordPlacement.Direction currentDirection)
        {
            this.word = word;
            this.startPosition = startPosition;
            this.currentDirection = currentDirection;

            score=GetScore(currentDirection);
        }

       

        public int GetScore(WordPlacement.Direction _currentDirection)
        {
            int score=0;
            switch (_currentDirection)
            {
                case WordPlacement.Direction.Right:
                    score= 10;
                    break;
                case WordPlacement.Direction.Left:
                    score= 20;
                    break;
                case WordPlacement.Direction.Up:
                    score= 25;
                    break;
                case WordPlacement.Direction.Down:
                    score= 15;
                    break;
            }

            return score;
        }
    }
}
