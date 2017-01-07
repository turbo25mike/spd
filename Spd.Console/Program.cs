﻿using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Spd.Console.Bootstrap;
using Spd.Console.Extensions;
using Spd.Console.Models;
using Spd.Console.Options;

namespace Spd.Console
{
    internal class SpdProgram : Program<Verbs>
    {
        public override bool OptionsValid(bool argumentsProvided, out string errorText)
        {
            if (Options.Add != null)
            {
                if (string.IsNullOrWhiteSpace(Options.Add.Description))
                    Options.Add.Description = "test";

                //foreach (var prop in Options.Add.GetType().GetProperties())
                 //   Output.WriteLine($"{prop.Name} : {prop.GetValue(Options.Add)}");
            }
            
            return base.OptionsValid(true, out errorText);
        }

        public override void Run()
        {
            try
            {
                //Options.Up = new UpOptions{ID = 1};
                Login();

                if(!_configManager.IsLoggedIn)
                    return;
                
                _projectManager = new ProjectManager();
                if (!IsSpdSolution()) return;

                if (Options.Add != null)
                    _projectManager.Add(Options.Add.Title, _configManager.UserName);

                if (Options.Up != null)
                    _projectManager.MoveUp(Options.Up.levels);

                if(Options.Update != null)
                    _projectManager.Update(description: Options.Update.Description);

                if (Options.To != null)
                    _projectManager.MoveTo(Options.To.ID);

                ListCurrentWorkItem();
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.ToString());
            }
        }

        private void Login()
        {
            _configManager = new ConfigManager();
            System.Console.ForegroundColor = ConsoleColor.Yellow;

            if (_configManager.UserName == null)
            {
                Output.WriteLine("Welcome to Spd it appears you have never logged in:");
                Output.WriteLine("Are you new to Spd? Y/N");
                var response = System.Console.ReadLine();
                if (response?.ToLower() == "yes" || response?.ToLower() == "y")
                {
                    Output.WriteLine("Let's take you to www. to sign up.");
                    return;
                }
                else
                {
                    Output.WriteLine("Enter your username:");
                    _configManager.SetUsername(System.Console.ReadLine());
                }
            }

            if (_configManager.PasswordNeeded)
            {
                var securePwd = new SecureString();
                Output.WriteLine("Enter your password:");
                ConsoleKeyInfo key;
                do
                {
                    key = System.Console.ReadKey(true);

                    // Ignore any key out of range.
                    if (((int)key.Key) >= 65 && ((int)key.Key <= 90))
                    {
                        // Append the character to the password.
                        securePwd.AppendChar(key.KeyChar);
                        System.Console.Write("*");
                    }
                    // Exit if Enter key is pressed.
                } while (key.Key != ConsoleKey.Enter);

                _configManager.SetPassword(securePwd);
            }

            System.Console.ForegroundColor = ConsoleColor.White;
            _configManager.LogIn();
        }

        private bool IsSpdSolution()
        {
            if (_projectManager.IsInitialized)
                return true;

            Output.WriteLine("This is not currently a spd project, would you like to make it one? Y/N");
            var response = System.Console.ReadLine();
            if (response?.ToLower() == "yes" || response?.ToLower() == "y")
            {
                _projectManager.Init();
                return true;
            }

            return false;
        }

        private void ListCurrentWorkItem()
        {
            if (!_projectManager.Project.WIS.Any())
                Output.WriteLine("You have not created any work items.  To do so just type: spd add \"WORK ITEM TITLE\"");
            else if (_projectManager.Project.CurrentWorkItem != null)
            {
                Output.WriteLine($"Current: {_projectManager.Project.CurrentWorkItem.ID}");
                Output.WriteLine($" [{_projectManager.Project.CurrentWorkItem.ID}] {_projectManager.Project.CurrentWorkItem.T}");
                Output.WriteLine($"Description: {_projectManager.Project.CurrentWorkItem.D ?? "none"}");
                Output.WriteLine("Children:");
                foreach (var wi in _projectManager.Project.CurrentWorkItem.CHD)
                    Output.WriteLine($"  - [{wi.ID}] {wi.T}");
            }
            else
            {
                Output.WriteLine("Current: root");
                Output.WriteLine("Children:");
                foreach (var wi in _projectManager.Project.WIS)
                    Output.WriteLine($"  - [{wi.ID}] {wi.T}");
            }
        }

        private ProjectManager _projectManager;
        private ConfigManager _configManager;
    }
}
