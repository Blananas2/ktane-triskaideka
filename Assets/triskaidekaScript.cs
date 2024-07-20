using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class triskaidekaScript : MonoBehaviour {

    public KMBombModule Module;
    public KMAudio Audio;
    public KMColorblindMode Colorblind;

    public GameObject Needle;
    public KMSelectable Submit;
    public KMSelectable[] Smalls;
    public MeshRenderer LED;
    public Material[] Mats; //"Mats", "Palette" and "CBSprites" have colors in the same order: Red, Orange, Yellow, Green, Blue, Purple, blacK, pInk
    public Light Light;
    public Color[] Palette;
    public GameObject Speaker;
    public SpriteRenderer CBSpriteSlot;
    public Sprite[] CBSprites;

    int[][] diag = new int[][] { //this represents the whole diagram: { arrow numbers for Red, Orange, Yellow, Green, Blue, Purple, number in the triangle }
        new int[] { 13, 1, -1, 1, 13, -1, 9 }, //these -1s here mean there is no Yellow 1 arrow for example, I use -1 all throughout this code when I have to put a value but it doesn't make sense to give it one
        new int[] { 12, 1, 2, 2, 13, 12, 7 },
        new int[] { 11, 1, 3, 3, 13, 11, 5 },
        new int[] { 10, 1, 4, 4, 13, 10, 8 },
        new int[] { 9, 1, 5, 5, 13, 9, 7 },
        new int[] { 8, 1, 6, 6, 13, 8, 13 },
        new int[] { 7, 1, 7, 7, 13, 7, 12 },
        new int[] { 6, 1, 8, 8, 13, 6, 4 },
        new int[] { 5, 1, 9, 9, 13, 5, 5 },
        new int[] { 4, 1, 10, 10, 13, 4, 4 },
        new int[] { 3, 1, 11, 11, 13, 3, 7 },
        new int[] { 2, 1, 12, 12, 13, 2, 5 },
        new int[] { -1, 1, 13, -1, 13, 1, 13 },
        new int[] { 13, 2, 2, 1, 12, 12, 1 },
        new int[] { 12, 2, 3, 2, 12, 11, 12 },
        new int[] { 11, 2, 4, 3, 12, 10, 10 },
        new int[] { 10, 2, 5, 4, 12, 9, 2 },
        new int[] { 9, 2, 6, 5, 12, 8, 11 },
        new int[] { 8, 2, 7, 6, 12, 7, 3 },
        new int[] { 7, 2, 8, 7, 12, 6, 9 },
        new int[] { 6, 2, 9, 8, 12, 5, 1 },
        new int[] { 5, 2, 10, 9, 12, 4, 2 },
        new int[] { 4, 2, 11, 10, 12, 3, 10 },
        new int[] { 3, 2, 12, 11, 12, 2, 3 },
        new int[] { 2, 2, 13, 12, 12, 1, 11 },
        new int[] { 13, 3, 3, 1, 11, 11, 3 },
        new int[] { 12, 3, 4, 2, 11, 10, 6 },
        new int[] { 11, 3, 5, 3, 11, 9, 4 },
        new int[] { 10, 3, 6, 4, 11, 8, 2 },
        new int[] { 9, 3, 7, 5, 11, 7, 8 },
        new int[] { 8, 3, 8, 6, 11, 6, 6 },
        new int[] { 7, 3, 9, 7, 11, 5, 12 },
        new int[] { 6, 3, 10, 8, 11, 4, 10 },
        new int[] { 5, 3, 11, 9, 11, 3, 8 },
        new int[] { 4, 3, 12, 10, 11, 2, 1 },
        new int[] { 3, 3, 13, 11, 11, 1, 9 },
        new int[] { 13, 4, 4, 1, 10, 10, 13 },
        new int[] { 12, 4, 5, 2, 10, 9, 11 },
        new int[] { 11, 4, 6, 3, 10, 8, 6 },
        new int[] { 10, 4, 7, 4, 10, 7, 10 },
        new int[] { 9, 4, 8, 5, 10, 6, 11 },
        new int[] { 8, 4, 9, 6, 10, 5, 1 },
        new int[] { 7, 4, 10, 7, 10, 4, 2 },
        new int[] { 6, 4, 11, 8, 10, 3, 6 },
        new int[] { 5, 4, 12, 9, 10, 2, 6 },
        new int[] { 4, 4, 13, 10, 10, 1, 12 },
        new int[] { 13, 5, 5, 1, 9, 9, 1 },
        new int[] { 12, 5, 6, 2, 9, 8, 10 },
        new int[] { 11, 5, 7, 3, 9, 7, 8 },
        new int[] { 10, 5, 8, 4, 9, 6, 9 },
        new int[] { 9, 5, 9, 5, 9, 5, 13 },
        new int[] { 8, 5, 10, 6, 9, 4, 3 },
        new int[] { 7, 5, 11, 7, 9, 3, 12 },
        new int[] { 6, 5, 12, 8, 9, 2, 2 },
        new int[] { 5, 5, 13, 9, 9, 1, 11 },
        new int[] { 13, 6, 6, 1, 8, 8, 13 },
        new int[] { 12, 6, 7, 2, 8, 7, 5 },
        new int[] { 11, 6, 8, 3, 8, 6, 2 },
        new int[] { 10, 6, 9, 4, 8, 5, 7 },
        new int[] { 9, 6, 10, 5, 8, 4, 5 },
        new int[] { 8, 6, 11, 6, 8, 3, 10 },
        new int[] { 7, 6, 12, 7, 8, 2, 7 },
        new int[] { 6, 6, 13, 8, 8, 1, 4 },
        new int[] { 13, 7, 7, 1, 7, 7, 8 },
        new int[] { 12, 7, 8, 2, 7, 6, 12 },
        new int[] { 11, 7, 9, 3, 7, 5, 4 },
        new int[] { 10, 7, 10, 4, 7, 4, 6 },
        new int[] { 9, 7, 11, 5, 7, 3, 3 },
        new int[] { 8, 7, 12, 6, 7, 2, 13 },
        new int[] { 7, 7, 13, 7, 7, 1, 4 },
        new int[] { 13, 8, 8, 1, 6, 6, 9 },
        new int[] { 12, 8, 9, 2, 6, 5, 7 },
        new int[] { 11, 8, 10, 3, 6, 4, 11 },
        new int[] { 10, 8, 11, 4, 6, 3, 1 },
        new int[] { 9, 8, 12, 5, 6, 2, 5 },
        new int[] { 8, 8, 13, 6, 6, 1, 8 },
        new int[] { 13, 9, 9, 1, 5, 5, 3 },
        new int[] { 12, 9, 10, 2, 5, 4, 10 },
        new int[] { 11, 9, 11, 3, 5, 3, 12 },
        new int[] { 10, 9, 12, 4, 5, 2, 6 },
        new int[] { 9, 9, 13, 5, 5, 1, 9 },
        new int[] { 13, 10, 10, 1, 4, 4, 3 },
        new int[] { 12, 10, 11, 2, 4, 3, 5 },
        new int[] { 11, 10, 12, 3, 4, 2, 1 },
        new int[] { 10, 10, 13, 4, 4, 1, 8 },
        new int[] { 13, 11, 11, 1, 3, 3, 13 },
        new int[] { 12, 11, 12, 2, 3, 2, 7 },
        new int[] { 11, 11, 13, 3, 3, 1, 11 },
        new int[] { 13, 12, 12, 1, 2, 2, 2 },
        new int[] { 12, 12, 13, 2, 2, 1, 9 },
        new int[] { 13, -1, 13, 1, -1, 1, 4 }
    };
    int attempts = 1;
    string[] colorNames = { "Red", "Orange", "Yellow", "Green", "Blue", "Purple" };
    float[] Angles = { 180f, 165.75f, 150.75f, 134.5f, 120.8f, 105.65f, 90f, 75.65f, 60.8f, 45.5f, 30.75f, 15.75f, 0f }; //these angles were determined by the placement of the markings which aren't necessarily 15n
    const float moronicVariable = 360f / (float)Math.PI; //for some really stupid reason, i need to take the atan of the raw value then multiply by this thing to get the actual angle value
    int currentlyDisplayed = 0;
    int[] dataA;
    int[] dataB;
    int[] dataC;
    bool hasStruck = false;
    bool beeping = false;
    int submittedNumbers = 0;
    bool cbActive = false;
    int[] correctNumbers = { -1, -1, -1 };
    int offBy = -1;
    string[] ordinals = { "First", "Second", "Third" };
    bool rightHalf = false;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable Small in Smalls) {
            Small.OnInteract += delegate () { SmallPress(Small); return false; };
        }

        Submit.OnInteract += delegate () { SubmitPress(); return false; };
    }

    void Start () {
        cbActive = Colorblind.ColorblindModeActive; //standard cb procedure
        Debug.LogFormat("<Triskaideka #{0}> Colorblind mode: {1}", moduleId, cbActive);

        float scalar = transform.lossyScale.x; //standard light procedure: all lights must be scaled based on the scale of the bomb
        Light.range *= scalar;

        RerollSet:

        dataA = generatePair();
        dataB = generatePair();
        dataC = generatePair();

        if (!SetIsValid(dataA, dataB, dataC)) { //if we can't submit any of the answers in the ranges given we have to reroll the whole set
            attempts++;
            goto RerollSet;
        }

        Debug.LogFormat("<Triskaideka #{0}> Generated in {1} attempts", moduleId, attempts);
        Debug.Log("A: " + dataA.Join(", "));
        Debug.Log("B: " + dataB.Join(", "));
        Debug.Log("C: " + dataC.Join(", "));
        Debug.LogFormat("[Triskaideka #{0}] The needle moves between {1} with a {2} flash and {3} with a {4} flash.", moduleId, dataA[0], colorNames[dataA[2]], dataA[1], colorNames[dataA[3]]);
        Debug.LogFormat("[Triskaideka #{0}] After a right press, the needle moves between {1} with a {2} flash and {3} with a {4} flash.", moduleId, dataB[0], colorNames[dataB[2]], dataB[1], colorNames[dataB[3]]);
        Debug.LogFormat("[Triskaideka #{0}] After another right press, the needle moves between {1} with a {2} flash and {3} with a {4} flash.", moduleId, dataC[0], colorNames[dataC[2]], dataC[1], colorNames[dataC[3]]);
        Debug.LogFormat("[Triskaideka #{0}] The {1} {2} arrow and {3} {4} arrow both point to a {5}, this is A.", moduleId, colorNames[dataA[2]], dataA[0], colorNames[dataA[3]], dataA[1], dataA[4]);
        Debug.LogFormat("[Triskaideka #{0}] The {1} {2} arrow and {3} {4} arrow both point to a {5}, this is B.", moduleId, colorNames[dataB[2]], dataB[0], colorNames[dataB[3]], dataB[1], dataB[4]);
        Debug.LogFormat("[Triskaideka #{0}] The {1} {2} arrow and {3} {4} arrow both point to a {5}, this is C.", moduleId, colorNames[dataC[2]], dataC[0], colorNames[dataC[3]], dataC[1], dataC[4]);

        correctNumbers = new int[] { dataA[4], dataB[4], dataC[4] };
        StartCoroutine(Tennis(dataA));
        rightHalf = IsRightHalf(dataA);
    }

    int[] generatePair() {
        RerollPair:

        int lowEnd = Rnd.Range(1, 14); //generate numbers
        int highEnd;
        do { highEnd = Rnd.Range(1, 14); }
            while (highEnd == lowEnd); //they cannot be the same, since we need the needle to move back and forth, not stay in the same place
        if (lowEnd > highEnd) { //if they're in the wrong order, swap them: the below is a trick to swap without creating a new variable using xor
            lowEnd = lowEnd ^ highEnd;
            highEnd = lowEnd ^ highEnd;
            lowEnd = lowEnd ^ highEnd;
        }
        
        int colorLow = Rnd.Range(0, 6); //generate colors
        int colorHigh;
        do { colorHigh = Rnd.Range(0, 6); } 
            while (colorHigh % 3 == colorLow % 3); //if color's arrows point in opposite directions, there'll be too much overlap (if they point at each other) or not any (if not) which doesn't work, regenerate high

        if (NumberColorPairDoesntExist(lowEnd, colorLow) || NumberColorPairDoesntExist(highEnd, colorHigh)) { //if the number/color pair is not in the diagram, reroll
            attempts++;
            goto RerollPair;
        }

        bool found = false; //find the intersection
        int intersection = -1;
        for (int d = 0; d < diag.Length; d++) {
            if (diag[d][colorLow] == lowEnd && diag[d][colorHigh] == highEnd) {
                found = true; //if we find one it must be the only intersection, proven by exhaustion (all pairs of lines form parallelograms which is analogous to a square grid)
                intersection = diag[d][6];
                break;
            }
        }
        if (found) {
            return new int[] { lowEnd, highEnd, colorLow, colorHigh, intersection };
        } else {
            attempts++;
            goto RerollPair; //if no such intersection exists, reroll
        }
    }

    bool NumberColorPairDoesntExist(int n, int c) {
        switch (n) {
            case 1: return c % 2 == 0; //there's no 1 arrows that are red, yellow, or blue
            case 13: return c % 2 == 1; //there's no 13 arrows that are orange, green, or purple
            default: return false; //anything else is fine
        }
    }

    bool SetIsValid(int[] da, int[] db, int[] dc) {
        bool[] validPositions = { false, false, false, false, false, false, false, false, false, false, false, false, false };
        int lowPos = -1;
        int highPos = -1;
        for (int xd = 0; xd < 3; xd++) {
            switch (xd) {
                case 0: lowPos = da[0] - 1; highPos = da[1]; break; //from the three ranges given (subtracting 1 from the former but not the latter is deliberate, the below "if" needs to take that into account tho)
                case 1: lowPos = db[0] - 1; highPos = db[1]; break;
                case 2: lowPos = dc[0] - 1; highPos = dc[1]; break;
            }
            if (lowPos == 0 && highPos == 13) { validPositions[0] = true; validPositions[12] = true; continue; } //if the ends are at the extremes, the needle uses the right half of the mod instead, only allow ends
            for (int p = lowPos; p < highPos; p++) { //set all their in-between positions to be valid
                validPositions[p] = true;
            }
        }

        return validPositions[da[4]-1] && validPositions[db[4]-1] && validPositions[dc[4]-1]; //just logical AND them together, if any is false it returns false
    }

    private IEnumerator Tennis(int[] given) { //yes im using tennis terms because i thought it was funny you're just gonna have to deal with that
        float server = Angles[given[0] - 1];
        float opponent = Angles[given[1] - 1];
        var serverPlacement = Quaternion.Euler(0f, server, 0f);
        var opponentPlacement = Quaternion.Euler(0f, opponent, 0f);
        int serverShirt = given[2];
        int opponentShirt = given[3];
        bool birdie = Rnd.Range(0, 2) == 0; //randomly choose a direction (up/down)
        float court = server - opponent;

        float elapsed = court / 150f; //at the beginning we want to be in the middle of the movement, this elapsed being exactly half means the animation starts at the halfway point
        while (true) {
            float duration = court / 75f;
            var start = birdie ? serverPlacement : opponentPlacement;
            var end = birdie ? opponentPlacement : serverPlacement;
            while (elapsed < duration)
            {
                Needle.transform.localRotation = Quaternion.Slerp(start, end, elapsed / duration); //this is the line that actually animates, it does a linear interpolation on the two rotations
                if (Math.Abs(Math.Asin(Needle.transform.localRotation.y) * moronicVariable - server) < 7.5f) { //if the needle is within 7.5 degrees of the end marking, light up the LED accordingly
                    LED.material = Mats[serverShirt];
                    Light.color = Palette[serverShirt];
                    if (cbActive) { CBSpriteSlot.sprite = CBSprites[serverShirt]; } 
                } else if (Math.Abs(Math.Asin(Needle.transform.localRotation.y) * moronicVariable - opponent) < 7.5f) {
                    LED.material = Mats[opponentShirt];
                    Light.color = Palette[opponentShirt];
                    if (cbActive) { CBSpriteSlot.sprite = CBSprites[opponentShirt]; } 
                } else { //or if not, turn it off
                    LED.material = Mats[6];
                    Light.color = Palette[6];
                    if (cbActive) { CBSpriteSlot.sprite = null; } 
                }
                yield return null;
                elapsed += Time.deltaTime;
            }
            Needle.transform.localRotation = end;
            birdie = !birdie; //invert direction and start from the beginning once we're over duration
            elapsed = 0f;
        }
    }

    void SmallPress(KMSelectable Small) {
        Small.AddInteractionPunch(0.25f);
        if (moduleSolved || beeping) { Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform); return; } //do nothing if mod is solved or beeping
        if (Small == Smalls[0]) {
            if (currentlyDisplayed == 0) { Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform); return; } //do nothing if we try to go back from 0
            Audio.PlaySoundAtTransform("creepy", transform);
            currentlyDisplayed--;
            rightHalf = IsRightHalf(currentlyDisplayed == 0 ? dataA : dataB);
            StopAllCoroutines();
            StartCoroutine(SpeakerVibe(0.382f));
            StartCoroutine(Tennis(currentlyDisplayed == 0 ? dataA : dataB));
        } else {
            if (currentlyDisplayed == 2) { Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform); return; } //do nothing if we try to go forward from 2
            Audio.PlaySoundAtTransform("creepy", transform);
            currentlyDisplayed++;
            rightHalf = IsRightHalf(currentlyDisplayed == 1 ? dataB : dataC);
            StopAllCoroutines();
            StartCoroutine(SpeakerVibe(0.382f));
            StartCoroutine(Tennis(currentlyDisplayed == 1 ? dataB : dataC));
        }
    }

    void SubmitPress() {
        Submit.AddInteractionPunch(1f);
        if (beeping) { return; } //do nothing if mod is beeping
        if (moduleSolved) {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform); //play correct chime if solved
            StartCoroutine(SpeakerVibe(0.7f));
            return;
        }
        if (hasStruck) { //if we just struck, beep now
            beeping = true;
            hasStruck = false;
            StartCoroutine(CommenceBeepage(offBy));
            return;
        }
        if (Math.Abs(Math.Asin(Needle.transform.localRotation.y) * moronicVariable - Angles[correctNumbers[submittedNumbers] - 1]) < 7.5f) { //if we're within 7.5 degrees of the correct angle, that's good
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
            if (rightHalf && correctNumbers[submittedNumbers] != 1 && correctNumbers[submittedNumbers] != 13) { //if we're on the right half and it isn't 1 or 13, play the horn
                Audio.PlaySoundAtTransform("doot doot", transform);
                StartCoroutine(SpeakerVibe(0.282f));
                return;
            }
            Debug.LogFormat("[Triskaideka #{0}] {1} submit is good{2}.", moduleId, ordinals[submittedNumbers], submittedNumbers == 2 ? ", module solved" : "");
            submittedNumbers++;
            if (submittedNumbers == 3) { //solve mod if all three are good
                StopAllCoroutines();
                Module.HandlePass();
                moduleSolved = true;
                LED.material = Mats[7]; //set the LED to pink
                Light.color = Palette[7];
                if (cbActive) { CBSpriteSlot.sprite = CBSprites[7]; } 
            }
        } else { //otherwise strike it
            if (rightHalf) { //if we're on the right half, play the horn instead of striking
                Audio.PlaySoundAtTransform("doot doot", transform);
                StartCoroutine(SpeakerVibe(0.282f));
                return;
            }
            int thisThingy = 13 - (int)Math.Round(Math.Abs(Math.Asin(Needle.transform.localRotation.y) * moronicVariable / 15f)); //calculate how off we were by for the beep function to use
            offBy = Math.Abs(thisThingy - correctNumbers[submittedNumbers]);
            Debug.LogFormat("[Triskaideka #{0}] {1} submit is not good, it's at {2} which means you're off by {3}. Strike!", moduleId, ordinals[submittedNumbers], thisThingy, offBy);
            Module.HandleStrike();
            hasStruck = true;
            StartCoroutine(SpeakerVibe(1f));
        }
    }

    private IEnumerator SpeakerVibe(float time) {
        float elapsed = 0f;
        while (elapsed < time) { //while we're under the time given
            Speaker.transform.localScale = new Vector3(10f, 10.5f, 10f); //stretch the y scale a bit
            Speaker.transform.localRotation = Quaternion.Euler(-90f, 0f, Rnd.Range(0, 360) * 1f); //rotate to a random angle (it doesn't look as random as i woulda hoped but whatever it still looks neat)
            yield return null;
            elapsed += Time.deltaTime;
        }
        Speaker.transform.localScale = new Vector3(10f, 10f, 10f); //once done put the y scale back to normal
    }

    private IEnumerator CommenceBeepage(int numberOfTimes) {
        for (int b = 0; b < numberOfTimes; b++) { //just use a for loop for beeping the specified number of times, nothing too fancy
            Audio.PlaySoundAtTransform("BEEP", transform);
            StartCoroutine(SpeakerVibe(0.7f));
            yield return new WaitForSeconds(1.381f);
        }
        beeping = false;
    }

    bool IsRightHalf (int[] x) { //right half is just are the two ends the extremes at the moment, if so we can play the horn
        return x[0] == 1 && x[1] == 13;
    }
}
