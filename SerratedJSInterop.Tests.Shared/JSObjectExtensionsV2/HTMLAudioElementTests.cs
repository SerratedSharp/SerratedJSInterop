using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class HTMLAudioElement_DefaultConstructor : JSTest
    {
        public override void Run()
        {
            var audio = new Audio();
            Assert(audio != null && audio.JSObject != null, "Default constructor should produce non-null element");
            Assert(audio.IsPaused, "New audio should be paused");
        }
    }

    public class HTMLAudioElement_ConstructorWithSrc : JSTest
    {
        public override void Run()
        {
            var audio = new Audio("https://example.com/audio.mp3");
            Assert(audio != null, "Constructor with src should produce non-null element");
            Assert(audio.Src == "https://example.com/audio.mp3", "Src should match constructor argument");
        }
    }

    public class HTMLAudioElement_Src_GetSet : JSTest
    {
        public override void Run()
        {
            var audio = new Audio();
            audio.Src = "https://example.com/sound.mp3";
            Assert(audio.Src == "https://example.com/sound.mp3", "Src should round-trip");
        }
    }

    public class HTMLAudioElement_Volume_GetSet : JSTest
    {
        public override void Run()
        {
            var audio = new Audio();
            Assert(audio.Volume >= 0 && audio.Volume <= 1, "Default volume should be in [0,1]");
            audio.Volume = 0.5;
            Assert(audio.Volume == 0.5, "Volume should round-trip");
        }
    }

    public class HTMLAudioElement_IsMuted_GetSet : JSTest
    {
        public override void Run()
        {
            var audio = new Audio();
            audio.IsMuted = true;
            Assert(audio.IsMuted, "IsMuted should be true after set");
            audio.IsMuted = false;
            Assert(!audio.IsMuted, "IsMuted should be false after set");
        }
    }

    public class HTMLAudioElement_Duration_IsPaused : JSTest
    {
        public override void Run()
        {
            var audio = new Audio();
            var duration = audio.Duration;
            Assert(double.IsNaN(duration) || duration >= 0, "Duration should be NaN or non-negative before load");
            Assert(audio.IsPaused, "New audio should be paused");
        }
    }

    public class HTMLAudioElement_Pause_Load_NoThrow : JSTest
    {
        public override void Run()
        {
            var audio = new Audio();
            audio.Pause();
            audio.Load();
            Assert(true, "Pause() and Load() should complete without throw");
        }
    }
}
