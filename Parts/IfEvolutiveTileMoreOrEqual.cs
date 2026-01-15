using System;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Conversations.Parts;

namespace XRL.World.Conversations
{
    [HasConversationDelegate]
    public static class DelegateContainer
    {
        // Checks if the EvolutiveTile stage is >= the provided value
        // Can be used in conversation conditions like:
        //
        // IfEvolutiveTileMoreOrEqual=""
        // IfNotEvolutiveTileMoreOrEqual=""
        // IfSpeakerEvolutiveTileMoreOrEqual=""
        // IfSpeakerNotEvolutiveTileMoreOrEqual=""
        
        [ConversationDelegate(Speaker = true)]
        public static bool IfEvolutiveTileMoreOrEqual(DelegateContext Context)
        {
            Brothers_EvolutiveTile evolutiveTile;

            if (!Context.Target.TryGetPart<Brothers_EvolutiveTile>(out evolutiveTile))
            {
                return false;
            }

            return evolutiveTile.Stage >= Convert.ToInt32(Context.Value);
        }
    }
}