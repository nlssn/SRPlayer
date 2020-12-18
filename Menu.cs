using System;
using System.Collections.Generic;
using System.Text;

namespace SRPlayer
{
    /*
     * The Menu class handles generating and displaying navigation menus within the app.
     */
    class Menu
    {
        // Properties
        private string Prompt; // A string of text shown before/above the menu
        private string[] Options; // An array of options that the menu will display
        private int SelectedIndex; // The index of the option that the user has selected

        // Constructor
        public Menu(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
            SelectedIndex = 0;
        }

        // Output the options for the current Menu
        private void DisplayOptions()
        {

            for (int i=0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string prefix;

                if (i == SelectedIndex)
                {
                    prefix = ">";
                } else
                {
                    prefix = " ";
                }

                Console.WriteLine($"{prefix} {currentOption}");
            } 
        }

        // Display the menu and return the option the user picked (SelectedIndex)
        public int Run()
        {
            ConsoleKey keyPressed;

            // Run this loop until the user press Enter
            do
            {
                // Output the Menu
                Console.Clear();
                Console.WriteLine(Prompt);
                Console.WriteLine();
                DisplayOptions();

                Console.CursorVisible = false;

                // Ask for input and store the value of the pressed key in keyPressed
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    // When the user presses the up arrow, decrease current SelectedIndex by one
                    SelectedIndex--;

                    // If the SelectedIndex would be negative,
                    // set it to the last index of the Options array
                    if (SelectedIndex == -1)
                    {
                        SelectedIndex = Options.Length - 1;
                    }
                } else if (keyPressed == ConsoleKey.DownArrow)
                {
                    // When the user presses the down arrow, increase current SelectedIndex by one
                    SelectedIndex++;

                    // If the SelectedIndex would be larger than the array of Options,
                    // set it to the first index of the Options array
                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = 0;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);

            // Return the SelectedIndex, so we can do the appropriate action (option)
            return SelectedIndex;
        }
    }
}
