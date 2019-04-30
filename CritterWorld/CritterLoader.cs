using CritterController;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;

namespace CritterWorld
{
    public class CritterLoader
    {
        // Get list of all dll files in specified folder. Iterate through them to find classes that implement ICritterControllerFactory.
        public List<Critter> LoadCritters(bool isCompetition)
        {
            string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
            string propertiesCritterControllerFilesPath = PropertiesManager.Properties.CritterControllerDLLPath.Trim();
            string propertiesPathForCritterFiles = PropertiesManager.Properties.CritterControllerFilesPath.Trim();

            string dllPath;
            if (propertiesCritterControllerFilesPath.Length == 0)
            {
                dllPath = executablePath + "/";
            }
            else
            {
                dllPath = propertiesCritterControllerFilesPath;
            }

            string pathForCritterFiles;
            if (propertiesPathForCritterFiles.Length == 0)
            {
                pathForCritterFiles = executablePath + "/CritterFiles";
            }
            else
            {
                pathForCritterFiles = propertiesPathForCritterFiles;
            }

            List<Critter> critters = new List<Critter>();

            if (!File.Exists(pathForCritterFiles))
            {
                try
                {
                    Directory.CreateDirectory(pathForCritterFiles);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unable to create Critter file directory " + pathForCritterFiles + " due to " + e, "Unable to Load Critters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return critters;
                }
            }

            Critterworld.Log(new LogEntry("Loading all Critter controllers from path " + dllPath));
            try
            {
                string[] dllFiles = System.IO.Directory.GetFiles(dllPath, "*.dll");

                int critterNumber = 1;

                Dictionary<string, string> namespacesUsed = new Dictionary<string, string>();

                foreach (string file in dllFiles)
                {
                    Critterworld.Log(new LogEntry("Loading Critter controllers from file " + file));
                    try
                    {
                        Assembly assembly = Assembly.UnsafeLoadFrom(file);
                        assembly.GetTypes().Where(type => type.IsClass && type.GetInterface("ICritterControllerFactory") != null).ToList().ForEach(type =>
                        {
                            try
                            {
                                Critterworld.Log(new LogEntry("Attempting to create instance of type " + type.FullName + " from namespace " + type.Namespace + " from file " + file));
                                if (type.Namespace == "DemonstrationCritters" && !file.EndsWith("DemonstrationCritters.dll"))
                                {
                                    string msg = "Error loading controllers from " + file + ". Namespace matches DemonstrationCritters namespace.";
                                    Critterworld.Log(new LogEntry("*** " + msg));
                                    MessageBox.Show(msg, "Controller Load Problem!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                else if (namespacesUsed.TryGetValue(type.Namespace, out string usedIn))
                                {
                                    string msg = "Error loading controllers from " + file + ". Namespace " + type.Namespace + " has already been used in DLL " + usedIn;
                                    Critterworld.Log(new LogEntry("*** " + msg));
                                    MessageBox.Show(msg, "Controller Load Problem!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                else
                                {
                                    ICritterControllerFactory critterFactory = (ICritterControllerFactory)Activator.CreateInstance(type);
                                    namespacesUsed.Add(type.Namespace, file);
                                    if (critterFactory.Author == null)
                                    {
                                        Critterworld.Log(new LogEntry("*** Error loading controller from " + file + ". Loading succeeded but Author property is null."));
                                    }
                                    else
                                    {
                                        Color familyColor = Color.Black;
                                        ICritterController[] controllers = critterFactory.GetCritterControllers();
                                        if (controllers == null)
                                        {
                                            Critterworld.Log(new LogEntry("*** Error loading controller from " + file + ". CritterFactory.GetCritterControllers() returned null."));
                                        }
                                        else
                                        {
                                            int loadedCount = 0;
                                            foreach (ICritterController controller in controllers)
                                            {
                                                if (controller == null)
                                                {
                                                    Critterworld.Log(new LogEntry("*** Error loading controller from " + file + ". Failed to load; controller is null."));
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        int number = critterNumber++;
                                                        controller.Filepath = PropertiesManager.Properties.CritterControllerFilesPath;
                                                        Critter critter = new Critter(number, controller);
                                                        if (familyColor == Color.Black)
                                                        {
                                                            familyColor = critter.Color;
                                                        }
                                                        else
                                                        {
                                                            critter.Color = familyColor;
                                                        }
                                                        critter.Author = critterFactory.Author.Trim().Replace(':', '_').Replace('\t', '_').Replace('\n', '_');
                                                        if (controller.Name != null)
                                                        {
                                                            critter.Name = controller.Name.Trim().Replace(':', '_').Replace('\t', '_').Replace('\n', '_');
                                                        }
                                                        Critterworld.Log(new LogEntry(number, controller.Name, critterFactory.Author, "Loaded controller from " + file));
                                                        critters.Add(critter);
                                                        loadedCount++;
                                                        if (isCompetition && loadedCount == PropertiesManager.Properties.CompetitionControllerLoadMaximum)
                                                        {
                                                            Critterworld.Log(new LogEntry(number, critter.Name, critter.Author, "During competition, maximum number of controllers loadable from a factory is " + PropertiesManager.Properties.CompetitionControllerLoadMaximum));
                                                            break;
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        Critterworld.Log(new LogEntry("*** Exception loading controller from " + file, e));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Critterworld.Log(new LogEntry("*** Error loading controller from " + file, e));
                            }
                        });                        
                    }
                    catch (Exception e)
                    {
                        Critterworld.Log(new LogEntry("*** Error loading file " + file, e));
                    }
                }
            }
            catch (Exception e)
            {
                Critterworld.Log(new LogEntry("*** Error accessing controller .dll directory " + dllPath, e));
            }
            return critters;
        }

    }

}
