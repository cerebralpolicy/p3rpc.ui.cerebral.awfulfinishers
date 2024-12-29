using p3rpc.ui.cerebral.awfulfinishers.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;
using p3rpc.ui.cerebral.awfulfinishers.Types;

namespace p3rpc.ui.cerebral.awfulfinishers.Configuration
{
    public class Config : Configurable<Config>
    {
        [Category("Developer Settings")]
        [DisplayName("Log Level")]
        [DefaultValue(LogLevel.Information)]
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        [Category("Mode")]
        [DisplayName("Coin Flip")]
        [Description("Treats all enabled toggles as a coin flip.")]
        [DefaultValue(false)]
        public bool RandomMode { get; set; } = false;
        [Category("Player")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool PlayerToggle { get; set; } = true;
        [Category("Yukari")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool YukariToggle { get; set; } = true;
        [Category("Junpei")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool JunpeiToggle { get; set; } = true;
        [Category("Akihiko")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool AkihikoToggle { get; set; } = true;
        [Category("Mitsuru")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool MitsuruToggle { get; set; } = true;
        [Category("Aigis")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool AigisToggle { get; set; } = true;
        [Category("Ken")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool KenToggle { get; set; } = true;
        [Category("Koromaru")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool KoromaruToggle { get; set; } = true;
        [Category("Shinjiro")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool ShinjiroToggle { get; set; } = true;
        [Category("Metis")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool MetisToggle { get; set; } = true;
        [Category("Metis")]
        [DisplayName("Use French")]
        [DefaultValue(false)]
        public bool FrenchMetis { get; set; } = false;
        [Category("Aigis (DLC)")]
        [DisplayName("Enable")]
        [DefaultValue(true)]
        public bool AigisDLCToggle { get; set; } = true;

        private readonly Random rng = new Random();
        public bool ReadConfigState(Character chara)
        {
            var existing_toggle
                = chara switch
                {
                    Character.Player => this.PlayerToggle,
                    Character.Yukari => this.YukariToggle,
                    Character.Stupei => this.JunpeiToggle,
                    Character.Akihiko => this.AkihikoToggle,
                    Character.Mitsuru => this.MitsuruToggle,
                    Character.Aigis => this.AigisToggle,
                    Character.Ken => this.KenToggle,
                    Character.Koromaru => this.KoromaruToggle,
                    Character.Shinjiro => this.ShinjiroToggle,
                    Character.Metis => this.MetisToggle,
                    Character.AigisDLC => this.AigisDLCToggle,
                    _ => false,
                };
     
            if (this.RandomMode)
            {
                if (chara == Character.None || chara == Character.Fuuka || !existing_toggle)
                {
                    return false;
                }

                var coinFlip = this.rng.Next(1, 100) > 50;
                if (coinFlip)
                {
                    Log.Debug($"The finisher screen for {Characters.GetName(chara)} will be modified");
                    return true;
                }
                Log.Debug($"The finisher screen for {Characters.GetName(chara)} will remain as is");
                return false;
            }
            else
            {
                return existing_toggle;
            }
        }
    }


    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}
