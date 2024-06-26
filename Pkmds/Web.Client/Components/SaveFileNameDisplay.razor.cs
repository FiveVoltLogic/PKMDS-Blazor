﻿using static PKHeX.Core.GameVersion;

namespace Pkmds.Web.Client.Components;

public partial class SaveFileNameDisplay
{
    private static string FriendlyGameName(GameVersion gameVersion) => gameVersion switch
    {
        Invalid => "Invalid",
        S => "Sapphire",
        R => "Ruby",
        E => "Emerald",
        FR => "FireRed",
        LG => "LeafGreen",
        CXD => "Colosseum / XD",
        D => "Diamond",
        P => "Pearl",
        Pt => "Platinum",
        HG => "HeartGold",
        SS => "SoulSilver",
        W => "White",
        B => "Black",
        W2 => "White 2",
        B2 => "Black 2",
        X => "X",
        Y => "Y",
        AS => "Alpha Sapphire",
        OR => "Omega Ruby",
        SN => "Sun",
        MN => "Moon",
        US => "Ultra Sun",
        UM => "Ultra Moon",
        GO => "GO",
        RD => "Red",
        GN => "Green",
        BU => "Blue",
        YW => "Yellow",
        GD => "Gold",
        SI => "Silver",
        C => "Crystal",
        GP => "Let's Go Pikachu",
        GE => "Let's Go Eevee",
        SW => "Sword",
        SH => "Shield",
        PLA => "Legends: Arceus",
        BD => "Brilliant Diamond",
        SP => "Shining Pearl",
        SL => "Scarlet",
        VL => "Violet",
        StadiumJ => "Stadium (J)",
        Stadium => "Stadium",
        Stadium2 => "Stadium 2",
        _ => gameVersion.ToString(),
    };
}
