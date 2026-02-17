// This is a simple reusable conversation delegate that allows an NPC to teach the player a cooking recipe.
// To use it in XML, add it as an attribute on a <choice> element like so:
// <choice Target="End" Brothers_GiveRecipe="RecipeClassName">Teach me this recipe.</choice>
// The string value ("RecipeClassName") must match the class name of a cooking recipe in XRL.World.Skills.Cooking.
// When the player selects the choice, the delegate will give them the recipe and show a popup message.

using Qud.API;
using System;
using XRL.UI;
using XRL.World.Parts;
using XRL.World.Skills.Cooking;

namespace XRL.World.Conversations
{
    [HasConversationDelegate]
    public static class Brothers_GiveRecipeDelegate
    {
        [ConversationDelegate]
        public static void Brothers_GiveRecipe(DelegateContext Context)
        {
            if (string.IsNullOrEmpty(Context.Value))
                return;

            // Get and create Recipe
            Type recipeType = ModManager.ResolveType("XRL.World.Skills.Cooking." + Context.Value);
            if (recipeType == null)
                return;

            CookingRecipe recipe = Activator.CreateInstance(recipeType) as CookingRecipe;
            if (recipe == null)
                return;

            if (CookingGameState.KnowsRecipe(recipe))
                return;

            // Message
            Popup.Show($"{The.Speaker.Does("share")} the recipe for {{{{W|{recipe.GetDisplayName()}}}}}!");

            // Teach recipe
            JournalAPI.AddRecipeNote(recipe, silent: true);
        }
    }
}