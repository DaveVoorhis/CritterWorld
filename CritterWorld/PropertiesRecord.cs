using System;

namespace CritterWorld
{
    public class PropertiesRecord
    {
        public int CompetitionControllerLoadMaximum { get; set; }
        public string CritterControllerDLLPath { get; set; }
        public string CritterControllerFilesPath { get; set; }
        public int TerrainDetailFactor { get; set; }

        internal void RestoreDefaults()
        {
            CompetitionControllerLoadMaximum = 5;
            CritterControllerDLLPath = "";
            CritterControllerFilesPath = "";
            TerrainDetailFactor = 80;
        }
    };
}
