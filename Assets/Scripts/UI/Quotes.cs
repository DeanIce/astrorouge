using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Quotes
    {
        public static List<(string, string)> quotes = new()
        {
            ("I can't help thinking somewhere in the universe there has to be something better than man. Has to be.",
                "Planet Of The Apes"),
            ("I am the scales of justice! Conductor of the choir of death!", "Mad Max: Fury Road"),
            ("I'll be back.", "The Terminator"),
            ("All those moments will be lost in time, like tears in rain.", "Blade Runner"),
            ("You said science was about admitting what we don't know.", "Interstellar"),
            ("What in the name of Sir Isaac H. Newton happened here?", "Back To The Future II"),
            ("Things are only impossible until they’re not!", "Star Trek: Picard"),
            ("Life is pleasant. Death is peaceful.\n It's the transition that's troublesome.", "Isaac Asimov"),
            ("I guess you could call it a 'failure', but I prefer the term 'learning experience'.", "The Martian"),
            ("The only way of discovering the limits of the possible is to venture a little way past them into the impossible.",
                "Arthur C. Clarke"),
            ("To appreciate the wonder of the universe, one must first remain alive.", "Dark Intelligence")
        };


        public static (string, string) Get()
        {
            var index = Random.Range(0, quotes.Count);
            Debug.Log(index);
            return quotes[index];
        }
    }
}