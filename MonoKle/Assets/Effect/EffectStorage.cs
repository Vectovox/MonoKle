namespace MonoKle.Assets.Effect
{
    using Microsoft.Xna.Framework.Graphics;
    using MonoKle.IO;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Class storing and loading effect files.
    /// </summary>
    public class EffectStorage : AbstractFileLoader
    {
        private GraphicsDevice graphicsDevice;

        private Dictionary<string, Effect> effectByName = new Dictionary<string, Effect>();

        /// <summary>
        /// Initializes a new instance of <see cref="EffectStorage"/>.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device.</param>
        public EffectStorage(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Returns the effect with the provided name.
        /// </summary>
        /// <param name="name">The name of the effect to return.</param>
        /// <returns>Effect</returns>
        public Effect GetEffect(string name)
        {
            return this.effectByName.ContainsKey(name) ? this.effectByName[name] : null;
        }

        /// <summary>
        /// Returns a dictionary of all effects, mapped to by their corresponding names.
        /// </summary>
        /// <returns>Name-Effect dictionary</returns>
        public Dictionary<string, Effect> GetEffects()
        {
            return new Dictionary<string, Effect>(this.effectByName);
        }

        protected override bool OperateOnFile(Stream fileStream, string path)
        {
            BinaryReader br = new BinaryReader(fileStream);
            byte[] byteCode = br.ReadBytes((int)fileStream.Length);
            Effect effect = new Effect(this.graphicsDevice, byteCode);

            string name = Path.GetFileName(path);

            if(this.effectByName.ContainsKey(name) == false)
            {
                this.effectByName.Add(name, effect);
                return true;
            }
            return false;
        }
    }
}
