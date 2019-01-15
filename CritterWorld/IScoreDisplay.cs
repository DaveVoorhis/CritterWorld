namespace CritterWorld
{
    public interface IScoreDisplay
    {
        int CritterNumber { set; }

        string Name { set; }

        string Author { set; }

        int CurrentScore { set; }

        bool Escaped { set; }

        int Energy { set; }

        bool Killed { set; }

        int Health { set; }

        int OverallScore { set; }
    }
}