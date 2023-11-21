using Godot;
using Util.ExtensionMethods;

namespace Game.FX
{
    /// <summary>
    /// Audio object that other objects create to make sounds independent of the object that created it. This object will play a sound
    /// once created, and destroy itself once the sound effect has finished playing. The sound created is also not dependent on the position
    /// of the spawning sound.
    /// </summary>
    public class OneShotSound : AudioStreamPlayer
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // stops the audio in case the designer accidently set this to play on autoplay
            Stop();
            // connect this node to the finished signal to call and destroy this node
            this.Connect("finished", this, "OnSoundFinished");
            Play();
        }

        public void OnSoundFinished()
        {
            this.SafeQueueFree();
        }
    }
}