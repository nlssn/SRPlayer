﻿using System;
using System.Collections.Generic;

namespace SRPlayer
{
    class Program
    {
        // Properties
        private Api SverigesRadio;
        private List<Channel> Channels;
        private Channel SelectedChannel;
        private string[] ChannelNames;
        private Stream Audio;
        private bool Streaming;

        public Program()
        {
            // Get the data from the API
            SverigesRadio = new Api("http://api.sr.se/api/v2/channels?format=json&paginaton=false&size=10");
            SverigesRadio.Run();

            // Get a list of all the channels
            Channels = SverigesRadio.Channels;

            // Get an array of all the the channel names
            List<string> tempList = new List<string>();
            foreach (Channel ch in Channels)
            {
                tempList.Add(ch.Name);
            }
            ChannelNames = tempList.ToArray();

            // Streaming false
            Streaming = false;
        }

        static void Main(string[] args)
        {
            // Set the window title and remove cursor
            Console.Title = "SRPlayer | En liten konsolapp för att lyssna på Sveriges Radio";
            Console.CursorVisible = false;

            // Instantiate the program
            Program SRP = new Program();

            // Display the main menu
            SRP.MainMenu();
        }

        private void MainMenu()
        {
            string prompt = ":: SRPlayer ::\n\n" +
                            "SRPlayer är en liten applikation för att lyssna på Sveriges Radio.\n" +
                            "Använd piltangenterna för att navigera i menyn. Tryck sedan Enter.";
            string[] options = { "Välj kanal", "Om", "Avsluta" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    ChannelPicker();
                    break;
                case 1:
                    About();
                    break;
                case 2:
                    Exit();
                    break;
            }
        }

        private void ChannelPicker()
        {
            string prompt = ":: Välj kanal :: " + SRPlayer.Stream.count;
            Menu mainMenu = new Menu(prompt, ChannelNames);
            int selectedIndex = mainMenu.Run();

            SelectedChannel = Channels[selectedIndex];
            
            StreamPlaying();
        }

        private void StreamPlaying()
        {
            Audio = new Stream(SelectedChannel.Url);
            Audio.Play();
            
            string prompt = $":: {SelectedChannel.Name} ::\n\n" +
                            SelectedChannel.Tagline.Replace(". ", ".\n");
            string[] options = { "Pausa uppspelning", "Ändra kanal", "Till huvudmenyn" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    StreamPaused();
                    break;
                case 1:
                    Audio.Stop();
                    Streaming = false;
                    ChannelPicker();
                    break;
                case 2:
                    Audio.Stop();
                    Streaming = false;
                    MainMenu();
                    break;
            }
        }

        private void StreamPaused()
        {
            Audio.Pause();

            string prompt = $":: {SelectedChannel.Name} :: [PAUSAD]\n\n" +
                            SelectedChannel.Tagline.Replace(". ", ".\n");
            string[] options = { "Återuppta uppspelning", "Ändra kanal", "Till huvudmenyn" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    Audio.Stop();
                    Streaming = false;
                    StreamPlaying();
                    break;
                case 1:
                    Audio.Stop();
                    Streaming = false;
                    ChannelPicker();
                    break;
                case 2:
                    Audio.Stop();
                    Streaming = false;
                    MainMenu();
                    break;
            }
        }

        private void About()
        {
            string prompt = ":: Om SRPlayer ::\n\n" +
                            "Skapades av Johannes Nilsson, som ett avlutande projekt i kursen DT086G.\n" +
                            "All data hämtas från Sveriges Radios Öppna API som finns tillgängligt här:\nhttp://www.blalalal.se";
            string[] options = { "Till huvudmenyn" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    MainMenu();
                    break;
            }
        }

        private void Exit()
        {
            Console.Clear();
            Console.WriteLine(":: Avsluta SRPlayer ::\n");
            Console.WriteLine("Tryck på valfri tangent för avsluta...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}
