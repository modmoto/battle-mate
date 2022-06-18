using System;
using System.Collections.Generic;

namespace battle_mate.Pages.BattleResult;

public class CombatResultState
{
    public event EventHandler OnLoadFinished;
    public int PointsLeft =>
        WoundsLeft +
        VerminSwarmWoundsLeftActual +
        RanksLeft +
        BannersLeft +
        OverkillLeft +
        AdditionalPointsLeftActual +
        PossibleChargeValues[ChargingIndexLeft].Points +
        PossibleFlankValues[FlankIndexLeft].Points +
        PossibleRearValues[RearIndexLeft].Points;

    public int VerminSwarmWoundsLeftActual => HasVerminSwarmPoints ? (VerminSwarmWoundsLeft + 1) / 2 : 0;
    public int AdditionalPointsLeftActual => HasAdditionalPoints ? AdditionalPointsLeft : 0;
    public int PointsDifferenceLeft => PointsLeft - PointsRight > 0 ? 0 : PointsLeft - PointsRight;

    public int PointsRight =>
        WoundsRight +
        VerminSwarmWoundsRightActual +
        RanksRight +
        BannersRight +
        OverkillRight +
        AdditionalPointsRightActual +
        PossibleChargeValues[ChargingIndexRight].Points +
        PossibleFlankValues[FlankIndexRight].Points +
        PossibleRearValues[RearIndexRight].Points;

    public int VerminSwarmWoundsRightActual => HasVerminSwarmPoints ? (VerminSwarmWoundsRight + 1) / 2 : 0;
    public int AdditionalPointsRightActual => HasAdditionalPoints ? AdditionalPointsRight : 0;
    public int PointsDifferenceRight => PointsRight - PointsLeft > 0 ? 0 : PointsRight - PointsLeft;

    public int VerminSwarmWoundsLeft { get; set; }
    public int VerminSwarmWoundsRight { get; set; }
    public int WoundsLeft { get; set; }
    public int WoundsRight { get; set; }
    public int RanksLeft { get; set; }
    public int RanksRight { get; set; }
    public int BannersLeft { get; set; }
    public int BannersRight { get; set; }
    public int OverkillLeft { get; set; }
    public int OverkillRight { get; set; }
    public int AdditionalPointsLeft { get; set; }
    public int AdditionalPointsRight { get; set; }

    public int DefaultRanks { get; set; }
    public int DefaultBanners { get; set; }
    public bool HasVerminSwarmPoints { get; set; }
    public bool HasAdditionalPoints { get; set; }
    public int ChargingIndexLeft { get; set; }
    public int FlankIndexLeft { get; set; }
    public int RearIndexLeft { get; set; }
    public int ChargingIndexRight { get; set; }
    public int FlankIndexRight { get; set; }
    public int RearIndexRight { get; set; }
    public bool HasResetRanksAndBanners { get; set; }

    public List<PointStrings> PossibleChargeValues => new()
    {
        new PointStrings(0, "not charging"),
        new PointStrings(1, "Charge +1")
    };

    public List<PointStrings> PossibleFlankValues => new()
    {
        new PointStrings(0, "not flanking"),
        new PointStrings(1, "Flank +1"),
        new PointStrings(2, "Flank +2")
    };

    public List<PointStrings> PossibleRearValues => new()
    {
        new PointStrings(0, "no rear"),
        new PointStrings(2, "Rear +2"),
        new PointStrings(3, "Rear +3")
    };

    public List<PointStrings> PossibleVerminSwarmWoundOptions => new()
    {
        new PointStrings(0, "hide vermin swarm wounds"),
        new PointStrings(1, "hide vermin swarm wounds")
    };

    public List<PointStrings> PossibleAdditionalOptions => new()
    {
        new PointStrings(0, "hide additional points"),
        new PointStrings(1, "hide additional points")
    };
        
    public List<PointStrings> PossibleHasResetRanksAndBannersOptions => new()
    {
        new PointStrings(0, "reset ranks and banners"),
        new PointStrings(1, "reset ranks and banners")
    };

    public void ResetStats(bool isInitializing = false)
    {
        WoundsLeft = 0;
        WoundsRight = 0;
        if (HasResetRanksAndBanners || isInitializing)
        {
            RanksLeft = DefaultRanks;
            RanksRight = DefaultRanks;
            BannersLeft = DefaultBanners;
            BannersRight = DefaultBanners;
        }
            
        OverkillLeft = 0;
        OverkillRight = 0;
        AdditionalPointsLeft = 0;
        AdditionalPointsRight = 0;
        VerminSwarmWoundsLeft = 0;
        VerminSwarmWoundsRight = 0;
        ChargingIndexLeft = 0;
        FlankIndexLeft = 0;
        RearIndexLeft = 0;
        ChargingIndexRight = 0;
        FlankIndexRight = 0;
        RearIndexRight = 0;
    }

    public void SetOptions(CombatOptions options)
    {
        HasAdditionalPoints = options.HasAdditionalPoints;
        HasVerminSwarmPoints = options.HasVerminSwarmPoints;
        DefaultBanners = options.DefaultBanners;
        DefaultRanks = options.DefaultRanks;
        HasResetRanksAndBanners = options.HasResetRanksAndBanners;
    }

    public void InitOptions(CombatOptions options)
    {
        SetOptions(options);
        ResetStats(true);
        OnLoadFinished?.Invoke(this, EventArgs.Empty);
    }

    public void AddWoundLeft(int woundsForLeft, bool verminSwarm)
    {
        if (verminSwarm)
        {
            VerminSwarmWoundsLeft += woundsForLeft;
        }
        else
        {
            WoundsLeft += woundsForLeft;
        }
    }

    public void AddWoundRight(int woundsForRight, bool verminSwarm)
    {
        if (verminSwarm)
        {
            VerminSwarmWoundsRight += woundsForRight;
        }
        else
        {
            WoundsRight += woundsForRight;
        }
    }
}