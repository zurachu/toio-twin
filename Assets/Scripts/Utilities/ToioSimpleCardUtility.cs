using System;
using System.Collections.Generic;
using UnityEngine;
using toio.Simulator;

public static class ToioSimpleCardUtility
{
    public static readonly List<StandardID.SimpleCardType> AlphabetTypes = new List<StandardID.SimpleCardType>
    {
        StandardID.SimpleCardType.Char_A,
        StandardID.SimpleCardType.Char_B,
        StandardID.SimpleCardType.Char_C,
        StandardID.SimpleCardType.Char_D,
        StandardID.SimpleCardType.Char_E,
        StandardID.SimpleCardType.Char_F,
        StandardID.SimpleCardType.Char_G,
        StandardID.SimpleCardType.Char_H,
        StandardID.SimpleCardType.Char_I,
        StandardID.SimpleCardType.Char_J,
        StandardID.SimpleCardType.Char_K,
        StandardID.SimpleCardType.Char_L,
        StandardID.SimpleCardType.Char_M,
        StandardID.SimpleCardType.Char_N,
        StandardID.SimpleCardType.Char_O,
        StandardID.SimpleCardType.Char_P,
        StandardID.SimpleCardType.Char_Q,
        StandardID.SimpleCardType.Char_R,
        StandardID.SimpleCardType.Char_S,
        StandardID.SimpleCardType.Char_T,
        StandardID.SimpleCardType.Char_U,
        StandardID.SimpleCardType.Char_V,
        StandardID.SimpleCardType.Char_W,
        StandardID.SimpleCardType.Char_X,
        StandardID.SimpleCardType.Char_Y,
        StandardID.SimpleCardType.Char_Z,
    };

    public static bool IsSimpleCardId(uint standardId)
    {
        return IndexOf(standardId) >= 0;
    }

    public static StandardID.SimpleCardType TypeOf(uint simpleCardId)
    {
        var index = IndexOf(simpleCardId);
        return index >= 0 ? (StandardID.SimpleCardType)index : StandardID.SimpleCardType.Full;
    }

    public static string NameOf(uint simpleCardId)
    {
        var index = IndexOf(simpleCardId);
        return index >= 0 ? StandardID.SimpleCardNames[index] : null;
    }

    public static string NameOf(StandardID.SimpleCardType simpleCardType)
    {
        return StandardID.SimpleCardNames[(int)simpleCardType];
    }

    public static Sprite SpriteOf(StandardID.SimpleCardType simpleCardType)
    {
        return (Sprite)Resources.Load<Sprite>($"StandardID/simple_card/{simpleCardType}");
    }

    public static bool IsAlphabet(uint simpleCardId)
    {
        return IsAlphabet(TypeOf(simpleCardId));
    }

    public static bool IsAlphabet(StandardID.SimpleCardType simpleCardType)
    {
        return AlphabetTypes.Contains(simpleCardType);
    }

    private static int IndexOf(uint simpleCardId)
    {
        return Array.IndexOf(StandardID.SimpleCardIDs, simpleCardId - StandardID.SimpleCardIDOffset);
    }
}
