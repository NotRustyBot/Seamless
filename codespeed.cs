using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SFDGameScriptInterface;


namespace SFDScript
{

    public partial class GameScript : GameScriptInterface
    {

        /* CLASS STARTS HERE - COPY BELOW INTO THE SCRIPT WINDOW */

        static class CodeSpeed
        {

            public static void Benchmark()
            {
                Game.ShowChatMessage("Initiating benchmark...");
                _Evaluate((float dt) =>
                {
                    float j = 1;
                    for (int i = 0; i < 100000; i++)
                    {
                        j = i / j;
                    }
                }, (int score) =>
                {
                    Game.GetSharedStorage("benchmark").SetItem("result", score);
                    Game.ShowChatMessage("Benchmark complete.", Color.Cyan);
                    Game.ShowChatMessage("Adjustment factor: " + (50f/score).ToString("f2"), Color.Cyan);
                });
            }
            public static void Evaluate(Action<float> action)
            {
                Game.ShowChatMessage("Initiating evaluation...", Color.Cyan);
                _Evaluate(action, (int score) =>
                {
                    Game.ShowChatMessage("Raw score: " + score, Color.Cyan);
                    if (Game.GetSharedStorage("benchmark").ContainsKey("result"))
                    {
                        int compare = (int)Game.GetSharedStorage("benchmark").GetItem("result");
                        score = (int)((50f / compare) * score);
                        Game.ShowChatMessage("Adjusted score: "+ score, Color.Cyan);
                    }
                    else
                    {
                        Game.ShowChatMessage("Adjusted score: not available.", Color.Cyan);
                    }
                });
            }
            static void _Evaluate(Action<float> action, Action<int> callback = null)
            {
                float time = 3000;
                float sample = 3000;
                int sampler = 0;
                int stability = 0;

                Events.UpdateCallback runtime = null;
                runtime = Events.UpdateCallback.Start((float dt) =>
                {
                    int start = DateTime.Now.Millisecond;

                    action(dt);
                    int end = DateTime.Now.Millisecond;
                    if (start > end) end += 1000;
                    time += end - start;
                    sampler++;
                    if (sampler == 60)
                    {
                        sampler = 0;
                        if (sample - time > 10)
                        {
                            time -= sample - time;
                        }
                        if (sample < time)
                        {
                            stability++;
                            if (stability == 10)
                            {
                                if (callback != null) callback((int)time / 10);
                                runtime.Stop();
                            }
                        }
                        Game.WriteToConsole((int)time / 10 + " [" + (sample - time).ToString("f1") + "]");
                        sample = time;
                    }
                    time *= 0.999f;
                });
            }
        }

        /* CLASS ENDS HERE - COPY ABOVE INTO THE SCRIPT WINDOW */

    }
}
