namespace MonoKle.Input.Keyboard
{
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;

    /// <summary>
    /// Interface providing polling-based keyboard input.
    /// </summary>
    public interface IKeyboard
    {
        /// <summary>
        /// Queries whether the specified keys are down.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>True if the keys are down; otherwise false.</returns>
        bool AreKeysDown(IEnumerable<Keys> keys, CollectionQueryBehavior behavior);

        /// <summary>
        /// Queries whether the specified keys are held.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>True if the keys are held; otherwise false.</returns>
        bool AreKeysHeld(IEnumerable<Keys> keys, CollectionQueryBehavior behavior);

        /// <summary>
        /// Queries whether the specified keys are held for the given amount of time.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <returns>True if the keys are held; otherwise false.</returns>
        bool AreKeysHeld(IEnumerable<Keys> keys, double timeHeld, CollectionQueryBehavior behavior);

        /// <summary>
        /// Queries whether the specified keys are pressed.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>True if keys are pressed; otherwise false.</returns>
        bool AreKeysPressed(IEnumerable<Keys> keys, CollectionQueryBehavior behavior);

        /// <summary>
        /// Queries whether the specified keys are released.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>True if keys are released; otherwise false.</returns>
        bool AreKeysReleased(IEnumerable<Keys> keys, CollectionQueryBehavior behavior);

        /// <summary>
        /// Queries whether the specified keys are up.
        /// </summary>
        /// <param name="keys">The keys to query.</param>
        /// <param name="behavior">The query behavior.</param>
        /// <returns>True if the keys are up; otherwise false.</returns>
        bool AreKeysUp(IEnumerable<Keys> keys, CollectionQueryBehavior behavior);

        /// <summary>
        /// Provides the time, in seconds, that the specified key has been continously held.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>Seconds that the specified key has been held.</returns>
        double GetKeyHeldTime(Keys key);

        /// <summary>
        /// Gets the keys that are down.
        /// </summary>
        /// <returns>Collection of keys down.</returns>
        IEnumerable<Keys> GetKeysDown();

        /// <summary>
        /// Gets the state of the provided key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>State of the key.</returns>
        IPressable GetKeyState(Keys key);

        /// <summary>
        /// Queries whether the specified key is down.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if key is down; otherwise false.</returns>
        bool IsKeyDown(Keys key);

        /// <summary>
        /// Queries whether the specified key is held.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if key is held; otherwise false.</returns>
        bool IsKeyHeld(Keys key);

        /// <summary>
        /// Queries whether the specified key has been held for at least the given amount of time.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <param name="timeHeld">The amount of time.</param>
        /// <returns>True if key has been held for the specified amount of time; otherwise false.</returns>
        bool IsKeyHeld(Keys key, double timeHeld);

        /// <summary>
        /// Queries whether the specified key is pressed.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the key is pressed; otherwise false.</returns>
        bool IsKeyPressed(Keys key);

        /// <summary>
        /// Queries whether the specified key is released.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the key is released; otherwise false.</returns>
        bool IsKeyReleased(Keys key);

        /// <summary>
        /// Queries whether the specified key is up.
        /// </summary>
        /// <param name="key">Key to query.</param>
        /// <returns>True if the specified key is up; otherwise false.</returns>
        bool IsKeyUp(Keys key);
    }
}