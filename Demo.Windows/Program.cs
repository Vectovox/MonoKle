﻿using Demo.Domain;
using MonoKle.Engine;
using MonoKle.Graphics;
using System;

namespace Demo.Windows
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using var game = MGame.Create(GraphicsMode.Windowed);
            Boilerplate.ConfigureStates();
            game.Run();
        }
    }
}