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
        // Later, this should be administrator-configurable. For now, it's just the executable directory.
        private readonly string configDLLPath = "";

        public CritterLoader()
        {
        }

        // Get list of all dll files in specified folder. Iterate through them to find classes that implement ICritterControllerFactory.
        public List<Critter> LoadCritters()
        {
            List<Critter> critters = new List<Critter>();
            string dllPath = Path.GetDirectoryName(Application.ExecutablePath) + "/" + configDLLPath;
            Console.WriteLine("Loading Critter controllers from " + dllPath);
            try
            {
                string[] dllFiles = System.IO.Directory.GetFiles(dllPath, "*.dll");

                int critterNumber = 1;

                foreach (string file in dllFiles)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(file);
                        assembly.GetTypes().Where(type => type.IsClass && type.GetInterface("ICritterControllerFactory") != null).ToList().ForEach(type =>
                        {
                            try
                            {
                                ICritterControllerFactory critterFactory = (ICritterControllerFactory)Activator.CreateInstance(type);
                                if (critterFactory.Author == null)
                                {
                                    Console.WriteLine("Error loading controller from " + file + ". Loading succeeded but Author property is null.");
                                }
                                else
                                {
                                    Color familyColor = Color.Black;
                                    foreach (ICritterController controller in critterFactory.GetCritterControllers())
                                    {
                                        if (controller == null)
                                        {
                                            Console.WriteLine("Error loading controller from " + file + ". Failed to load; controller is null.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Loaded controller " + controller.Name + " by " + critterFactory.Author + " from " + file);
                                            Critter critter = new Critter(critterNumber++, controller);
                                            if (familyColor == Color.Black)
                                            {
                                                familyColor = critter.Color;
                                            }
                                            else
                                            {
                                                critter.Color = familyColor;
                                            }
                                            critter.Author = critterFactory.Author.Trim();
                                            if (controller.Name != null)
                                            {
                                                critter.Name = controller.Name.Trim();
                                            }
                                            critters.Add(critter);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error loading controller from " + file + ". " + e.Message + "\n" + e.StackTrace);
                            }
                        });                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error loading file " + file + ". Exception is " + e.Message + "\n" + e.StackTrace);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error accessing controller .dll directory " + dllPath + " due to " + e);
            }
            return critters;
        }

    }

}
